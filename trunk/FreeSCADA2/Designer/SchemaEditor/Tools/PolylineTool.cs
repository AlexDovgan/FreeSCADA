﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.Context_Menu;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.ShellInterfaces;


namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class PolylineTool:BaseTool,ITool
    {
        DrawingVisual objectPrview = new DrawingVisual();
        PointCollection pointsCollection = new PointCollection();


        public PolylineTool(SchemaDocument schema)
            : base(schema)
        {
            objectPrview.Opacity = 0.5;
            visualChildren.Add(objectPrview);
        }

        #region ITool Implementation
        public String ToolName
        {
            get { return "Polyline Tool"; }
        }

        public String ToolGroup
        {
            get { return "Graphics Tools"; }
        }
        public System.Drawing.Bitmap ToolIcon
        {
            get
            {

                return new System.Drawing.Bitmap(10, 10);
            }
        }
        #endregion

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (pointsCollection.Count>0)
            {
                
                DrawingContext drawingContext = objectPrview.RenderOpen();
                for (int i = 1; i < pointsCollection.Count;i++ )
                {

                    drawingContext.DrawLine(new Pen(Brushes.Black, 1), pointsCollection[i - 1], pointsCollection[i]);
                        
               }

                drawingContext.DrawLine(new Pen(Brushes.Black, 1), pointsCollection[pointsCollection.Count-1],e.GetPosition(this));

                drawingContext.Close();
            }

        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            //ReleaseMouseCapture();
            base.OnPreviewMouseLeftButtonUp(e);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            base.OnPreviewMouseLeftButtonDown(e);
            if (!e.Handled)
            {
                CaptureMouse();
                pointsCollection.Add(e.GetPosition(this));

            }

            e.Handled = false;

        }
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            if (pointsCollection.Count > 1)
            {
                Rect b = VisualTreeHelper.GetContentBounds(objectPrview);
                Polyline poly = new Polyline();
                poly.Points = pointsCollection.Clone();
                Canvas.SetLeft(poly, b.X);
                Canvas.SetTop(poly, b.Y);
                poly.Width = b.Width;
                poly.Height = b.Height;
                poly.Stroke = Brushes.Black;
                poly.Fill = Brushes.Transparent;
                
                ObjectDescriptor desc = new ObjectDescriptor();
                desc.DefaultManipulatorType = typeof(GeometryHilightManipulator);
                desc.DefaultShortPropType = typeof(ShortProperties.ShapeShortProp);
                poly.Tag = desc;
                poly.Stretch = Stretch.Fill;
                UndoRedoManager.GetUndoBuffer(workedSchema).AddCommand(new AddGraphicsObject(poly));
                SelectedObject = poly;
            }
            pointsCollection.Clear();
            objectPrview.RenderOpen().Close();
        }

    }
}
