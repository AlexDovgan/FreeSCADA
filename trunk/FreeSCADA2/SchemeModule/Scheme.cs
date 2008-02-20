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
using FreeSCADA.Scheme.Manipulators;
using FreeSCADA.Scheme.Tools;
using FreeSCADA.Scheme.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Scheme
{
    public class FSSchemeViewer : ScrollViewer
    {
        public FSSchemeDocument Scheme;
        ScaleTransform scale;
        public FSSchemeViewer(FSSchemeDocument d)
            : base()
        {
            BorderBrush = Brushes.Black;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            Scheme = d;
            Content = d.MainCanvas;
            scale = new ScaleTransform();
            Focusable = false;
            MouseWheel += new MouseWheelEventHandler(FSSchemeViewer_MouseWheel);
            
        }

        
        void FSSchemeViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            scale.ScaleX += e.Delta;
            scale.ScaleY += e.Delta;
            Scheme.MainCanvas.RenderTransform = scale;
        }
        public void SetScale()
        {


        }
    }

    public class FSSchemeExecutor
    {
    }
    public class FSSchemeEditor : FSSchemeViewer
    {

        AdornerLayer adornerLayer;
        GeometryEditManipulator SelectedObject;
        BasicUndoBuffer undoBuff;
        Tool  activeTool;
        
        public List<ITool> toolsList
        {
            get
            {
                List<ITool> tl = new List<ITool>();
                tl.Add(new SelectionTool(Scheme.MainCanvas));
                tl.Add(new RectangleTool(Scheme.MainCanvas));
                tl.Add(new EllipseTool(Scheme.MainCanvas));
                return tl;

            }

        }
        public Type CurrentTool
        {
            get { return activeTool.GetType();}
            set
            {
                activeTool.Deactivate();
                AdornerLayer.GetAdornerLayer(Scheme.MainCanvas).Remove(activeTool);
                object[] a = new object[1];
                a[0] = Scheme.MainCanvas;
                activeTool = (Tool)System.Activator.CreateInstance(value, a);
                AdornerLayer.GetAdornerLayer(Scheme.MainCanvas).Add(activeTool);
                activeTool.Activate();
            }
        }
        

        public FSSchemeEditor(FSSchemeDocument d)
            : base(d)
        {
            Scheme = d;

           
            Scheme.MainCanvas.Loaded += new RoutedEventHandler(MainCanvas_Loaded);
            undoBuff = UndoRedoManager.GetUndoBuffer(Scheme.MainCanvas);
            activeTool = new SelectionTool(Scheme.MainCanvas);
        }

        void MainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            adornerLayer = AdornerLayer.GetAdornerLayer(Scheme.MainCanvas);
            Scheme.MainCanvas.KeyDown += new KeyEventHandler(MainCanvas_KeyDown);
            
            AdornerLayer.GetAdornerLayer(Scheme.MainCanvas).Add(activeTool);
            activeTool.Activate();
        }

        void MainCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z &&Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                undoBuff.UndoCommand();
            }
            else if (e.Key == Key.Y && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                undoBuff.RedoCommand();
            }


        }

        

        void Tool_Started(MouseEventArgs e)
        {
            /*if (SelectedObject!=null)
            {
                adornerLayer.Remove(SelectedObject);
                SelectedObject = null;
            }*/
        }

        void Tool_Finished(Manipulator m)
        {
            /*if (m!=null)
            {
                SelectedObject = m as GeometryEditManipulator;
                adornerLayer.Add(m);
            }else if (SelectedObject != null)
            {
                    adornerLayer.Remove(SelectedObject);
                    SelectedObject = null;
              }
             * */
        }

        public void SelectObjects(UIElement obj)
        {
            try
            {

                if (SelectedObject.AdornedElement != obj)
                {

                    adornerLayer.Remove(SelectedObject); ;
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {

                SelectedObject = new GeometryEditManipulator((System.Windows.UIElement)obj);
                adornerLayer.Add(SelectedObject);
            }
        }

    }
    public class FSSchemeDocument
    {
        public Canvas MainCanvas; //single Layer Model


        private void Init()
        {
            MainCanvas.ClipToBounds = true;
            if (MainCanvas.Background == null)
                MainCanvas.Background = System.Windows.Media.Brushes.White;
        }


        public FSSchemeDocument(double w, double h)
        {

            MainCanvas = new Canvas();
            MainCanvas.Width = w; MainCanvas.Height = h;
            Init();


        }
        public FSSchemeDocument(Stream xamlStream)
        {
            try
            {
                // Insert code to read the stream here.
                XmlReader xmlReader = XmlReader.Create(xamlStream);
                Object obj = XamlReader.Load(xmlReader);
                if (obj is Canvas)
                {

                    MainCanvas = obj as Canvas;
                    Init();
                }
                else throw (new Exception("This is not FreeSCADA scheme"));
            }
            catch (Exception ex)
            {
 
            }

        }
        public static FSSchemeDocument CreateNewScheme()
        {
            //it must be a dilog for size enter here
            return new FSSchemeDocument(800, 600);

        }

        public void ImportFile(Stream xamlStream)
        {
            XmlReader xmlReader = XmlReader.Create(xamlStream);
            Object obj = XamlReader.Load(xmlReader);
            MainCanvas.Children.Add(obj as UIElement);


        }



        public void SaveScheme(Stream myStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            XamlWriter.Save(MainCanvas, XmlWriter.Create(myStream, settings));
        }


    }

}