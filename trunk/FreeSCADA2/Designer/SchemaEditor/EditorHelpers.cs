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
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.Tools;
using System.ComponentModel;

namespace FreeSCADA.Designer.SchemaEditor
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

        public static void BreakGroup(SelectionTool tool)
        {

            Canvas parent = tool.AdornedElement as Canvas;
            Viewbox g = tool.SelectedObject as Viewbox;
            try
            {
                Canvas gc = (Canvas)g.Child;

                while (gc.Children.Count > 0)
                {

                    System.Windows.FrameworkElement child = gc.Children[0] as System.Windows.FrameworkElement;
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
                g.Child = null;
                parent.Children.Remove(g);
                tool.SelectedObject = null;
            }
            catch (Exception)
            {
            }

        }
        public static void CreateGroup(SelectionTool tool)
        {

            Viewbox Group = new Viewbox();
            Group.SetValue(Viewbox.StretchProperty, Stretch.Fill);
            Canvas workCanvas = (Canvas)tool.AdornedElement;
            Canvas g = new Canvas();
            Rect r = EditorHelper.CalculateBoundce(tool.selectedElements, workCanvas);
            Canvas.SetLeft(Group, r.X);
            Canvas.SetTop(Group, r.Y);
            Group.Width = g.Width = r.Width;
            Group.Height = g.Height = r.Height;


            workCanvas.Children.Add(Group);

            foreach (UIElement ch in tool.selectedElements)
            {
                Vector off = VisualTreeHelper.GetOffset(ch);
                Canvas.SetLeft(ch, Canvas.GetLeft(ch) - r.X);
                Canvas.SetTop(ch, Canvas.GetTop(ch) - r.Y);
                workCanvas.Children.Remove(ch);
                g.Children.Add(ch);
            }
            Group.Child = g;
            Canvas.SetTop(g, 0); Canvas.SetLeft(g, 0);
            tool.selectedElements.Clear();
            tool.SelectedObject = Group;
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
        public static IList<DependencyProperty> GetSetedProperties(DependencyObject obj)
        {
            List<DependencyProperty> seted = new List<DependencyProperty>();

            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(obj,
                new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) }))
            {
                DependencyPropertyDescriptor dpd =
                    DependencyPropertyDescriptor.FromProperty(pd);

                if (dpd != null && obj.ReadLocalValue(dpd.DependencyProperty) != DependencyProperty.UnsetValue)
                {
                    seted.Add(dpd.DependencyProperty);
                }
            }

            return seted;
        }
        public static void CopyObjects(DependencyObject source, DependencyObject destination)
        {
            IList<DependencyProperty> spl = GetSetedProperties(source);
            IList<DependencyProperty> dpl = GetSetedProperties(destination);
            foreach (DependencyProperty property in dpl)
            {
                if (property.ReadOnly != true)
                    destination.SetValue(property, source.ReadLocalValue(property));

            }
            /*foreach (DependencyProperty property in spl)
            {
                destination.SetValue(property, source.ReadLocalValue(property));

            }*/


        }
        public static BaseTool GetActiveTool(SchemaDocument doc)
        {
            AdornerLayer al = AdornerLayer.GetAdornerLayer(doc.MainCanvas);
            Adorner[] adorners= al.GetAdorners(doc.MainCanvas);
            if (adorners.Length> 0 && adorners[0] is BaseTool)
                return adorners[0] as BaseTool;
            else return null;
        }
    }

}
