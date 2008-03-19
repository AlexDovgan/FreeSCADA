using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls.Primitives;
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
        public static FreeSCADA.Common.Schema.CustomElements.ElementsTemplates TemplateResources = new FreeSCADA.Common.Schema.CustomElements.ElementsTemplates();
        static  EditorHelper()
        {
            TemplateResources.InitializeComponent();
        }
            
        public static UIElement FindTopParentUnder(Canvas c, DependencyObject el)
        {
            DependencyObject top = el;
            while (VisualTreeHelper.GetParent(top) != c)
            {
                top = VisualTreeHelper.GetParent(top);
            }
            return top as UIElement;
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

                    //this code need for ungrouping not stretched shapes
                    child.SetValue(System.Windows.Controls.Panel.MarginProperty, System.Windows.DependencyProperty.UnsetValue);
                    //Rect b=VisualTreeHelper.GetContentBounds(child);
                    Rect b = VisualTreeHelper.GetDescendantBounds(child);
                    double left = Canvas.GetLeft(child);
                    double top = Canvas.GetTop(child);

                    left = double.IsNaN(left) ? 0 : left;
                    top = double.IsNaN(top) ? 0 : top;

                    
                    if ((child is Shape)&&child.ReadLocalValue(System.Windows.Shapes.Shape.StretchProperty).Equals(System.Windows.DependencyProperty.UnsetValue))
                    {
                        child.Width = b.Width;
                        child.Height = b.Height;
                        Canvas.SetLeft(child,left + b.X);
                        Canvas.SetTop(child, top + b.Y);

                        
                        child.SetValue(System.Windows.Shapes.Shape.StretchProperty, System.Windows.Media.Stretch.Fill);
                        gc.UpdateLayout();
                    }
                    // excluding rotate from common matrix
                    // need to separate scale*skew matrix on scale and transform transformations
 
                    Matrix matrGtr = ((Transform)child.TransformToVisual(parent)).Value;


                    double x= matrGtr.OffsetX;
                    double y=matrGtr.OffsetY;
                    
                    double angleY = Math.Atan(matrGtr.M12 / matrGtr.M11) * 180 / Math.PI;
                    matrGtr.OffsetY = 0; matrGtr.OffsetX = 0;

                    RotateTransform rt = new RotateTransform(angleY);
                    matrGtr.Rotate(-angleY);
                    TransformGroup gtr = new TransformGroup();
                    gtr.Children.Add(new MatrixTransform(matrGtr));
                    gtr.Children.Add(rt);
                    child.RenderTransform = gtr;
                    
                    child.RenderTransformOrigin = new Point(0.5, 0.5);
                    
                    Point pO = new Point(child.Width * child.RenderTransformOrigin.X, child.Height * child.RenderTransformOrigin.Y);
                    Point p = gtr.Transform(pO);
                    Canvas.SetLeft(child, x-(pO-p).X);
                    Canvas.SetTop(child, y - (pO - p).Y);
           
                    gc.Children.Remove(child);
                    parent.Children.Add(child);
                
                
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
            workCanvas.Children.Add(Group);
            tool.selectedElements.Clear();
            tool.AdornedElement.UpdateLayout();
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

                    r = g.GetRenderBounds(new Pen((el as Shape).Stroke,(el as Shape).StrokeThickness));
                }
                else
                    r = el.TransformToVisual(parent).TransformBounds(VisualTreeHelper.GetDescendantBounds(el));
                //r = LayoutInformation.GetLayoutSlot(el as FrameworkElement);
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
                try
                {
                    if (property.ReadOnly != true)
                        destination.SetValue(property, source.ReadLocalValue(property));
                }
                catch (Exception)
                {
                }

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
