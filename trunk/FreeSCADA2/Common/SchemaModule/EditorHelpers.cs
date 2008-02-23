using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Documents;
using System.Windows.Input;
using FreeSCADA.Schema.Manipulators;
using FreeSCADA.Schema.Tools;

namespace FreeSCADA.Schema
{
    static class EditorHelper
    {
        public static UIElement FindTopParentUnder(Canvas c, FrameworkElement el)
        {
            FrameworkElement top = el;
            while (top.Parent != c)
            {
                top = (FrameworkElement)top.Parent;
            }
            return top;
        }

        public static void BreakGroup(Viewbox g)
        {

            Canvas parent = g.Parent as Canvas;

            try
            {
                Canvas gc = (Canvas)g.Child;

                while (gc.Children.Count > 0)
                {

                    System.Windows.FrameworkElement child = gc.Children[0] as System.Windows.FrameworkElement;
                    /* if (child is System.Windows.Controls.Panel)
                     {
                         //Canvas c = convertToCanvas(child as System.Windows.Controls.Panel);
                         Rect b = VisualTreeHelper.GetDescendantBounds(child);

                         GeneralTransform tr1 = g.TransformToVisual(parent);
                         GeneralTransform tr2 = child.TransformToVisual(g);

                         Rect r = tr2.TransformBounds(b);
                         Point p = tr1.Transform(new Point(r.X, r.Y));

                         panel.Children.Remove(panel.Children[0]);
                         panel.SetValue(System.Windows.Controls.Panel.MarginProperty, System.Windows.DependencyProperty.UnsetValue);
                        
                         canvas.Children.Add(c);

                     }
                     else*/
                    {
                        Rect b = VisualTreeHelper.GetDescendantBounds(child);
                        child.SetValue(System.Windows.Controls.Panel.MarginProperty, System.Windows.DependencyProperty.UnsetValue);
                        child.SetValue(System.Windows.Shapes.Shape.StretchProperty, System.Windows.Media.Stretch.Fill);



                        Matrix matrGtr = child.RenderTransform.Value * ((Transform)gc.TransformToVisual(g)).Value * g.RenderTransform.Value;



                        matrGtr.OffsetX = 0; matrGtr.OffsetY = 0;

                        // wos rotate*scale make skew*rotate
                        double angleY = Math.Atan(matrGtr.M12 / matrGtr.M11) * 180 / Math.PI;

                        RotateTransform rt = new RotateTransform(angleY);
                        matrGtr.Rotate(-angleY);

                        TransformGroup gtr = new TransformGroup();
                        gtr.Children.Add(new MatrixTransform(matrGtr));
                        gtr.Children.Add(rt);

                        child.RenderTransform = gtr;//new MatrixTransform(matrGtr);


                        Point p = child.TransformToVisual(parent).Transform(new Point(b.X, b.Y));

                        Canvas.SetLeft(child, p.X);
                        Canvas.SetTop(child, p.Y);


                        gc.Children.Remove(child);
                        parent.Children.Add(child);


                        //code for modify object properties by group transform
                        //Matrix matrGtr = ((Transform)g.RenderTransform.Inverse).Value * ((Transform)gc.TransformToVisual(g).Inverse).Value * ((Transform)child.RenderTransform.Inverse).Value;
                        //matrGtr.Invert();
                        /*
                        child.Width *= matrGtr.M11;
                        child.Height *= matrGtr.M22;
                        matrGtr.M12 *= 1/matrGtr.M11;
                        matrGtr.M11 = 1;
                        matrGtr.M21 *=1/matrGtr.M22;
                        matrGtr.M22 = 1;*/

                    }

                }
                g.Child = null;
                parent.Children.Remove(g);
            }
            catch (Exception ex)
            {
            }

        }
        public static void CreateGroup(GroupEditManipulator gm)
        {

            if (gm.selectedElements.Count == 0) return;
            Viewbox Group = new Viewbox();
            Group.SetValue(Viewbox.StretchProperty, Stretch.Fill);
            Canvas workCanvas = (Canvas)gm.AdornedElement;
            Canvas g = new Canvas();
            Rect r = EditorHelper.CalculateBoundce(gm.selectedElements, (Canvas)gm.AdornedElement);
            Rectangle rr = new Rectangle();


            Canvas.SetLeft(Group, r.X);
            Canvas.SetTop(Group, r.Y);
            Group.Width = g.Width = r.Width;
            Group.Height = g.Height = r.Height;


            gm.workSchema.AddObject(Group);

            foreach (UIElement ch in gm.selectedElements)
            {
                Vector off = VisualTreeHelper.GetOffset(ch);
                Canvas.SetLeft(ch, Canvas.GetLeft(ch) - r.X);
                Canvas.SetTop(ch, Canvas.GetTop(ch) - r.Y);
                ((Canvas)gm.AdornedElement).Children.Remove(ch);
                g.Children.Add(ch);
            }
            AdornerLayer al = AdornerLayer.GetAdornerLayer(gm.AdornedElement);
            Group.Child = g;
            Canvas.SetTop(g, 0); Canvas.SetLeft(g, 0);

            al.Remove(gm);

        }

        public static Rect CalculateBoundce(List<UIElement> elList, Canvas parent)
        {
            Rect boundce = Rect.Empty;
            foreach (UIElement el in elList)
            {
                Rect r;
                if (el is Shape)
                {
                    Geometry g;
                    g = ((Shape)el).RenderedGeometry;
                    g.Transform = (Transform)el.TransformToVisual(parent);

                    r = g.GetRenderBounds(null);
                }
                else
                    r = el.TransformToVisual(parent).TransformBounds(VisualTreeHelper.GetDescendantBounds(el));

                if (boundce.IsEmpty)
                    boundce = r;
                else
                    boundce.Union(r);
            }


            return boundce;
        }
        
    }

}
