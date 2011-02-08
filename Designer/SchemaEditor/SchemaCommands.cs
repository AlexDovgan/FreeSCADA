using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Xsl;
using FreeSCADA.Common;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils;
using FreeSCADA.Designer.SchemaEditor.Tools;
using FreeSCADA.Designer.Views;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;
using FreeSCADA.CommonUI;


namespace FreeSCADA.Designer.SchemaEditor.SchemaCommands
{
    //TDOD:refactor commands to common usage

    class SchemaCommand : BaseCommand, IDisposable
    {
        object controlledObject;
        //first stage of refactoring of shcema veiw and commands

        protected DocumentView _view;

        public SchemaCommand(DocumentView sv)
        {
            Priority = (int)CommandManager.Priorities.EditCommands;
            _view = sv;
            _view.SelectionManager.SelectionChanged+= new ObjectSelectedDelegate(schemaView_ObjectSelected);
        }

        void schemaView_ObjectSelected(object sender)
        {
            CheckApplicability();
        }

        public virtual void CheckApplicability() { }



        #region IDisposable Members

        public void Dispose()
        {
            _view.SelectionManager.SelectionChanged -= new ObjectSelectedDelegate(schemaView_ObjectSelected);
        }

        #endregion
    }

    class ToolCommand : SchemaCommand
    {
        public ToolCommand(DocumentView sv,string name,string group,Bitmap icon,Type type):base(sv)
        {
            ToolName = name;
            ToolGroup = group;
            ToolIcon = icon;
            ToolType = type;

        }
        public String ToolName
        {
            get;
            protected set;
        }
        public String ToolGroup
        {
            get;
            protected set;
        }
        public Bitmap ToolIcon
        {
            get;
            protected set;
        }
        public Type ToolType
        {
            get;
            protected set;
        }
        public bool IsActive
        {
            get
            {

                return _view.ActiveTool==null?false:_view.ActiveTool.GetType() == ToolType;
            }
        }

        #region ICommand Members
        public override void Execute()
        {
            _view.ActiveTool=(BaseTool)System.Activator.CreateInstance(ToolType, new object[] { _view});
        }

        public override string Name
        {
            get { return ToolName; }
        }

        public override string Description
        {
            get { return ToolGroup+' '+ToolName; }
        }

        public override Bitmap Icon
        {
            get
            {
                return ToolIcon;
            }
        }
        #endregion ICommand Members
    }
    
    class UngroupCommand : SchemaCommand
    {
        public UngroupCommand(DocumentView  sv)
            : base(sv)
        {
        }
        public override void CheckApplicability()
        {

            if (_view.SelectionManager.SelectedObjects.Count>0
                &&_view.SelectionManager.SelectedObjects[0] is Viewbox)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members
        public override void Execute()
        {
            EditorHelper.BreakGroup((SchemaView)_view);
        }

        public override string Name
        {
            get { return StringResources.CommandUngroupName; }
        }

        public override string Description
        {
            get { return StringResources.CommandUngroupDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.shape_ungroup;
            }
        }
        #endregion ICommand Members
    }

    class GroupCommand : SchemaCommand
    {
        public GroupCommand(DocumentView sv)
            : base(sv)
        {
        }
        public override void CheckApplicability()
        {
            
            if (_view.SelectionManager.SelectedObjects.Count > 1)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members

        public override void Execute()
        {
            EditorHelper.CreateGroup((SchemaView)_view);
        }

        public override string Name
        {
            get { return StringResources.CommandGroupName; }
        }

        public override string Description
        {
            get { return StringResources.CommandGroupDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.shape_group;
            }
        }
        #endregion ICommand Members
    }

    class ZMoveTopCommand : SchemaCommand
    {
        public ZMoveTopCommand(DocumentView sv)
            : base(sv)
        {
        }
        public override void CheckApplicability()
        {
            
            if (_view.SelectionManager.SelectedObjects.Count > 0)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members

        public override void Execute()
        {
            base.Execute();
            
            
            List<FrameworkElement> sortedList = new List<FrameworkElement>();

            foreach (FrameworkElement uie in _view.MainPanel.Children)
            {
                if (_view.SelectionManager.SelectedObjects.Contains(uie))
                {
                    sortedList.Add(uie);
                }
            }
            foreach (FrameworkElement suie in sortedList)
            {
                _view.MainPanel.Children.Remove(suie);
                _view.MainPanel.Children.Add(suie);
            }
        }

        public override string Name
        {
            get { return StringResources.CommandZMoveTopName; }
        }

        public override string Description
        {
            get { return StringResources.CommandZMoveTopDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.move_object_front;
            }
        }
        #endregion
    }

    class ZMoveBottomCommand : SchemaCommand
    {
        public ZMoveBottomCommand(DocumentView sv)
            : base(sv)
        {
        }

        public override void CheckApplicability()
        {
            
            if (_view.SelectionManager.SelectedObjects.Count > 0)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members

        public override void Execute()
        {
            base.Execute();

            Panel currCanvas = _view.MainPanel;
            List<UIElement> sortedList = new List<UIElement>();

            for (int i = currCanvas.Children.Count - 1; i >= 0; i--)
            {
                if (_view.SelectionManager.SelectedObjects.Contains(currCanvas.Children[i]))
                {
                    sortedList.Add(currCanvas.Children[i]);
                }
            }
            foreach (FrameworkElement suie in sortedList)
            {
                currCanvas.Children.Remove(suie);
                currCanvas.Children.Insert(0, suie);
            }
        }

        public override string Name
        {
            get { return StringResources.CommandZMoveBottomName; }
        }

        public override string Description
        {
            get { return StringResources.CommandZMoveBottomDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.move_object_back;
            }
        }
        #endregion ICommand Members
    }

    class CopyCommand : SchemaCommand
    {
        public CopyCommand(DocumentView sv)
            : base(sv)
        {

        }

        public override void CheckApplicability()
        {
            
            if (_view.SelectionManager.SelectedObjects.Count > 0)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members
        public override void Execute()
        {
            

            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("");
            Rect b = EditorHelper.CalculateBounds(_view.SelectionManager.SelectedObjects.Cast<UIElement>().ToList(),
                _view.MainPanel);
            string xaml = string.Format(ci.NumberFormat,
                "<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Left=\"{0}\" Top=\"{1}\">"
                , b.X, b.Y);

            foreach (FrameworkElement el in _view.SelectionManager.SelectedObjects)
            {

                xaml += XamlWriter.Save(el);
            }
            xaml += "</Canvas>";

            System.Windows.Clipboard.SetText(xaml, System.Windows.TextDataFormat.Xaml);
        }

        public override string Name
        {
            get { return StringResources.CommandCopyName; }
        }

        public override string Description
        {
            get { return StringResources.CommandCopyDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.page_copy;
            }
        }
        #endregion ICommand Members
    }

    class CutCommand : SchemaCommand
    {
        public CutCommand(DocumentView sv)
            : base(sv)
        {
        }

        CopyCommand copyCommand;
        public override void CheckApplicability()
        {
            copyCommand = new CopyCommand(_view);
            copyCommand.CheckApplicability();
            CanExecute = copyCommand.CanExecute;
        }

        #region ICommand Members
        public override void Execute()
        {
         
            copyCommand.Execute();
            foreach (FrameworkElement el in _view.SelectionManager.SelectedObjects)
            {
                _view.UndoBuff.AddCommand(new DeleteGraphicsObject(el));
                    //ActiveTool.NotifyObjectDeleted(el);
            }

            _view.SelectionManager.SelectObject(null);
        }

        public override string Name
        {
            get { return StringResources.CommandCutName; }
        }

        public override string Description
        {
            get { return StringResources.CommandCutDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.cut;
            }
        }
        #endregion ICommand Members
    }
    class PasteCommand : SchemaCommand
    {
        public PasteCommand(DocumentView sv) :
            base(sv)
        {
        }
        public override void CheckApplicability()
        {
            SelectionTool tool = _view.ActiveTool as SelectionTool;
            if (tool != null && System.Windows.Clipboard.ContainsText(System.Windows.TextDataFormat.Xaml))
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members
        public override void Execute()
        {
            SelectionTool tool = _view.ActiveTool as SelectionTool;
            string xaml = System.Windows.Clipboard.GetText(System.Windows.TextDataFormat.Xaml);
            if (tool != null && xaml != null)
            {
                using (MemoryStream stream = new MemoryStream(xaml.Length))
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(xaml);
                        sw.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        Canvas elements = XamlReader.Load(stream) as Canvas;
                        while (elements.Children.Count != 0)
                        {
                            UIElement el = elements.Children[0];
                            elements.Children.Remove(el);
                            Canvas.SetLeft(el, Canvas.GetLeft(el) - Canvas.GetLeft(elements) + tool.LastClickedPoint.X);
                            Canvas.SetTop(el, Canvas.GetTop(el) - Canvas.GetTop(elements) + tool.LastClickedPoint.Y);
                            tool.NotifyObjectCreated(el);
                        }

                    }
                }
            }
        }

        public override string Name
        {
            get { return StringResources.CommandPasteName; }
        }

        public override string Description
        {
            get { return StringResources.CommandPasteDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.paste_plain;
            }
        }
        #endregion ICommand Members
    }

    class XamlViewCommand : SchemaCommand
    {
        SchemaView _scView;
        public XamlViewCommand(SchemaView sv)
            : base(sv)
        {
            _scView = sv;
        }


        public override void CheckApplicability()
        {
            CanExecute = true;
            Priority = (int)CommandManager.Priorities.EditCommands;
        }

        #region ICommand Members
        public override void Execute()
        {
            try
            {
                //TODO: косяк с XAMLView не должно быть конкретизации класса DocumentView
                if (!_scView.XamlView.Visible)
                {
                    
                    _scView.XamlView.Show();
                    _scView.UpdateXamlView();

					
                }
                else
                {
                    _scView.XamlView.Hide();
                }
            }
            catch { }
        }

        public override string Name
        {
            get { return StringResources.CommandXamlViewName; }
        }

        public override string Description
        {
            get { return StringResources.CommandXamlViewDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.page_white_code_red;
            }
        }
        #endregion ICommand Members
    }

    class ZoomLevelCommand : SchemaCommand, ICommandItems
    {
        double level;
        public event EventHandler CurrentChanged;

        public ZoomLevelCommand(DocumentView sv) :
            base(sv)
        {
            Priority = (int)CommandManager.Priorities.ViewCommands;
            sv.ZoomManager.ZoomChanged += new EventHandler(ZoomGesture_ZoomChanged);
            this.level = 1.0;
        }

        void ZoomGesture_ZoomChanged(object sender, EventArgs e)
        {
            Level = (sender as FreeSCADA.Common.Gestures.MapZoom).Zoom;
        }

        #region Informational properties

        public override string Name
        {
            get { return StringResources.CommandZoomName; }
        }

        public override string Description
        {
            get { return StringResources.CommandZoomDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override CommandType Type { get { return CommandType.DropDownBox; } }
        public override ICommandItems DropDownItems { get { return this; } }
        #endregion

        public override void CheckApplicability()
        {
            CanExecute = true;
        }
        public override void Execute()
        {

            _view.ZoomManager.Zoom =level ;

            _view.Focus();
        }

        public List<object> Items
        {
            get
            {
                List<object> res = new List<object>();
                res.Add("25%");
                res.Add("50%");
                res.Add("75%");
                res.Add("100%");
                res.Add("150%");
                res.Add("200%");
                return res;
            }
        }

        public double Level
        {
            get { return level; }
            set { level = value; Current = string.Format("Zoom {0}%", level * 100); }
        }

        public virtual object Current
        {
            get
            {
                return string.Format("Zoom {0}%", level * 100);
            }
            set
            {
                if (value.ToString().Length == 0)
                    level = 1.0;
                else
                {
                    MatchCollection matches = Regex.Matches(value.ToString(), @"\d+");
                    level = int.Parse(matches[0].Value) / 100.0;
                    if (level < 0.25)
                        level = 0.25;
                    if (level > 10)
                        level = 10;
                }
                if (CurrentChanged != null)
                    CurrentChanged(this, new EventArgs());
            }
        }
    }

    class ZoomInCommand : SchemaCommand
    {
        public ZoomInCommand(DocumentView sv) :
            base(sv)
        {
            Priority = (int)CommandManager.Priorities.ViewCommands;
        }

        public override void CheckApplicability()
        {
            CanExecute = true;
        }

        #region ICommand Members
        public override void Execute()
        {

            _view.ZoomManager.Zoom *= 1.05;
        }

        public override string Name
        {
            get { return StringResources.CommandZoomInName; }
        }

        public override string Description
        {
            get { return StringResources.CommandZoomInDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.zoom_in;
            }
        }
        #endregion ICommand Members
    }

    class ZoomOutCommand : SchemaCommand
    {
        public ZoomOutCommand(DocumentView sv) :
            base(sv)
        {
            Priority = (int)CommandManager.Priorities.ViewCommands;
        }

        public override void CheckApplicability()
        {
            CanExecute = true;
        }

        public override void Execute()
        {

            _view.ZoomManager.Zoom /= 1.05;
        }

        #region Informational properties
        public override string Name
        {
            get { return StringResources.CommandZoomOutName; }
        }

        public override string Description
        {
            get { return StringResources.CommandZoomOutDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.zoom_out;
            }
        }
        #endregion ICommand Members
    }

    class UndoCommand : SchemaCommand
    {
        public UndoCommand(DocumentView sv) :
            base(sv)
        {

        }

        public override void CheckApplicability()
        {
            DocumentView doc = _view;

            if (doc.UndoBuff.CanUndo())
                CanExecute = true;
            else
                CanExecute = false;
        }

        public override void Execute()
        {
            DocumentView doc = (DocumentView)_view;
            doc.UndoBuff.UndoCommand();
        }

        #region Informational properties

        public override string Name
        {
            get { return StringResources.CommandUndoName; }
        }

        public override string Description
        {
            get { return StringResources.CommandUndoDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.arrow_undo;
            }
        }
        #endregion
    }

    class RedoCommand : SchemaCommand
    {
        public RedoCommand(DocumentView sv) :
            base(sv)
        {

        }

        public override void CheckApplicability()
        {
            DocumentView doc = _view;

            if (doc != null && doc.UndoBuff.CanRedo())
                CanExecute = true;
            else
                CanExecute = false;
        }

        public override void Execute()
        {
            DocumentView doc = _view;
            doc.UndoBuff.RedoCommand();
        }

        #region Informational properties

        public override string Name
        {
            get { return StringResources.CommandRedoName; }
        }

        public override string Description
        {
            get { return StringResources.CommandRedoDescription; }
        }

        public override Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.Designer.Resources.arrow_redo;
            }
        }
        #endregion Informational properties
    }

    class CommonBindingCommand : SchemaCommand
    {
        public CommonBindingCommand(DocumentView sv) :
            base(sv)
        {
            
        }

        public override void CheckApplicability()
        {
            
            if (_view.SelectionManager.SelectedObjects.Count == 1)
                CanExecute = true;
            else
                CanExecute = false;
        }

        public override void Execute()
        {
            SelectionTool tool = _view.ActiveTool as SelectionTool;
            
            
            CommonBindingDialog dlg = new CommonBindingDialog(new PropProxy(_view.SelectionManager.SelectedObjects[0],_view.Document));
            dlg.ShowDialog(Env.Current.MainWindow);
        }

        #region Informational properties

        public override string Name
        {
            get { return StringResources.CommandCommonBindingName; }
        }

        public override string Description
        {
            get { return StringResources.CommandCommonBindingName; }
        }
        #endregion Informational properties
    }

    class ImportElementCommand : SchemaCommand
    {
        public override string Description 
        {
            get { return "Import Graphics"; } 
        }
        
        public override string Name
        {
            get { return "Import Graphics"; }
        }
        
        public ImportElementCommand(DocumentView sv)
            : base(sv)
        {
            CanExecute = true;
            Priority = (int)CommandManager.Priorities.FileCommandsEnd;
        }
        public override void Execute()
        {

            System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog();

            fd.Filter = StringResources.FileImportDialogFilter;
            fd.FilterIndex = 0;
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ImportFile(fd.FileName);
          
        }
        bool ImportFile(String filename)
         {
             if (filename.Contains("xaml"))
             {
                 XmlReader xmlReader = XmlReader.Create(filename);
                 FrameworkElement obj = XamlReader.Load(xmlReader) as FrameworkElement;
                 Canvas.SetTop(obj, 0);
                 Canvas.SetLeft(obj, 0);
                 _view.MainPanel.Children.Add(obj);
             }
             else if (filename.Contains("svg"))
             {
                 XmlReaderSettings rsettings = new XmlReaderSettings();
                 rsettings.ProhibitDtd = false;
                 rsettings.ConformanceLevel = ConformanceLevel.Fragment;
                 XmlReader reader = XmlReader.Create(filename, rsettings);

                 try
                 {
                     reader.MoveToContent();
                     XslCompiledTransform xslt = new XslCompiledTransform(true);
                        xslt.Load("resources\\svg2xaml.xsl", new XsltSettings(true, true), null);

                     //XmlReader.Create(new StringReader (StringResources.svg2xaml))

                     XmlWriterSettings settings = new XmlWriterSettings();
                     settings.Indent = true;
                     settings.IndentChars = "\t";
                     settings.ConformanceLevel = ConformanceLevel.Fragment;
                     StringWriter sw = new StringWriter();
                     XmlWriter writer = XmlWriter.Create(sw, settings);
                     xslt.Transform(reader, writer);
                     XmlReader xmlReader = XmlReader.Create(new StringReader(sw.ToString()));
                     System.Windows.Controls.Canvas obj = (System.Windows.Controls.Canvas)XamlReader.Load(xmlReader);
                     System.Windows.Controls.Viewbox v = new System.Windows.Controls.Viewbox();
                     v.Child = obj;
                     v.Width = 640;
                     v.Height = 480;
                     Canvas.SetTop(v, 0);
                     Canvas.SetLeft(v, 0);
                     v.Stretch = System.Windows.Media.Stretch.Fill;
                     _view.MainPanel.Children.Add(v);
                     reader.Close();

                 }
                 catch (Exception ex)
                 {
                     if (ex is DirectoryNotFoundException)
                         MessageBox.Show("File resources/svg2xaml.xsl not found");
                     else if ((ex is XmlException) && ex.Message.Contains("DTD"))
                         MessageBox.Show("Please remove DTD declaration from your SVG file");
                     else MessageBox.Show("Sorry but this SVG file can not be converted");
                     reader.Close();
                 }

             }
             return true;
         }
    }
}

/*
            SelectionTool tool = ControlledObject as SelectionTool;
            bool found = false;
            foreach (IVisualControlsPlug p in Env.Current.VisualPlugins.Plugins)
            {
                foreach (IVisualControlDescriptor d in p.Controls)
                {
                    if (tool.SelectedObjects[0].GetType() == d.Type)
                    {
                        CommonBindingDialog dlg = new CommonBindingDialog(d.getPropProxy(tool.SelectedObjects[0]));
                        dlg.ShowDialog(Env.Current.MainWindow);
                        found = true;
                    }
                }
            }
            if (!found)
            {
                CommonBindingDialog dlg = new CommonBindingDialog(new PropProxy(tool.SelectedObjects[0]));
                dlg.ShowDialog(Env.Current.MainWindow);
            }


*/