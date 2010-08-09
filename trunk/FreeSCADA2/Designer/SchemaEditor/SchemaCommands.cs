using System;
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


namespace FreeSCADA.Designer.SchemaEditor.SchemaCommands
{
    //TDOD:refactor commands to common usage

    class SchemaCommand : BaseCommand, IDisposable
    {
        object controlledObject;
        //first stage of refactoring of shcema veiw and commands

        protected SchemaView schemaView;

        public SchemaCommand(SchemaView sv)
        {
            Priority = (int)CommandManager.Priorities.EditCommands;
            schemaView = sv;
            schemaView.ObjectSelected += new DocumentView.ObjectSelectedDelegate(schemaView_ObjectSelected);
        }

        void schemaView_ObjectSelected(object sender)
        {
            CheckApplicability();
        }

        public virtual void CheckApplicability() { }



        #region IDisposable Members

        public void Dispose()
        {
            schemaView.ObjectSelected -= new DocumentView.ObjectSelectedDelegate(schemaView_ObjectSelected);
        }

        #endregion
    }

    class ToolCommand : SchemaCommand
    {
        public ToolCommand(SchemaView sv,string name,string group,Bitmap icon,Type type):base(sv)
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

                return schemaView.ActiveTool==null?false:schemaView.ActiveTool.GetType() == ToolType;
            }
        }

        #region ICommand Members
        public override void Execute()
        {
            schemaView.ActiveTool=(BaseTool)System.Activator.CreateInstance(ToolType, new object[] { schemaView.MainCanvas });
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
        public UngroupCommand(SchemaView sv)
            : base(sv)
        {
        }
        public override void CheckApplicability()
        {

            if (schemaView.SelectionManager.SelectedObjects.Count>0
                &&schemaView.SelectionManager.SelectedObjects[0] is Viewbox)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members
        public override void Execute()
        {
            EditorHelper.BreakGroup(schemaView);
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
                return global::FreeSCADA.Designer.Properties.Resources.shape_ungroup;
            }
        }
        #endregion ICommand Members
    }

    class GroupCommand : SchemaCommand
    {
        public GroupCommand(SchemaView sv)
            : base(sv)
        {
        }
        public override void CheckApplicability()
        {
            
            if (schemaView.SelectionManager.SelectedObjects.Count > 1)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members

        public override void Execute()
        {
            EditorHelper.CreateGroup(schemaView);
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
                return global::FreeSCADA.Designer.Properties.Resources.shape_group;
            }
        }
        #endregion ICommand Members
    }

    class ZMoveTopCommand : SchemaCommand
    {
        public ZMoveTopCommand(SchemaView sv)
            : base(sv)
        {
        }
        public override void CheckApplicability()
        {
            
            if (schemaView.SelectionManager.SelectedObjects.Count > 0)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members

        public override void Execute()
        {
            base.Execute();
            
            
            List<UIElement> sortedList = new List<UIElement>();

            foreach (UIElement uie in schemaView.MainCanvas.Children)
            {
                if (schemaView.SelectionManager.SelectedObjects.Contains(uie))
                {
                    sortedList.Add(uie);
                }
            }
            foreach (UIElement suie in sortedList)
            {
                schemaView.MainCanvas.Children.Remove(suie);
                schemaView.MainCanvas.Children.Add(suie);
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
                return global::FreeSCADA.Designer.Properties.Resources.move_object_front;
            }
        }
        #endregion
    }

    class ZMoveBottomCommand : SchemaCommand
    {
        public ZMoveBottomCommand(SchemaView sv)
            : base(sv)
        {
        }

        public override void CheckApplicability()
        {
            
            if (schemaView.SelectionManager.SelectedObjects.Count > 0)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members

        public override void Execute()
        {
            base.Execute();

            Canvas currCanvas = schemaView.MainCanvas;
            List<UIElement> sortedList = new List<UIElement>();

            for (int i = currCanvas.Children.Count - 1; i >= 0; i--)
            {
                if (schemaView.SelectionManager.SelectedObjects.Contains(currCanvas.Children[i]))
                {
                    sortedList.Add(currCanvas.Children[i]);
                }
            }
            foreach (UIElement suie in sortedList)
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
                return global::FreeSCADA.Designer.Properties.Resources.move_object_back;
            }
        }
        #endregion ICommand Members
    }

    class CopyCommand : SchemaCommand
    {
        public CopyCommand(SchemaView sv)
            : base(sv)
        {
        }

        public override void CheckApplicability()
        {
            
            if (schemaView.SelectionManager.SelectedObjects.Count > 0)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members
        public override void Execute()
        {
            

            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("");
            Rect b = schemaView.SelectionManager.CalculateBounds();
            string xaml = string.Format(ci.NumberFormat,
                "<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Left=\"{0}\" Top=\"{1}\">"
                , b.X, b.Y);

            foreach (UIElement el in schemaView.SelectionManager.SelectedObjects)
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
                return global::FreeSCADA.Designer.Properties.Resources.page_copy;
            }
        }
        #endregion ICommand Members
    }

    class CutCommand : SchemaCommand
    {
        public CutCommand(SchemaView sv)
            : base(sv)
        {
        }

        CopyCommand copyCommand;
        public override void CheckApplicability()
        {
            copyCommand = new CopyCommand(schemaView);
            copyCommand.CheckApplicability();
            CanExecute = copyCommand.CanExecute;
        }

        #region ICommand Members
        public override void Execute()
        {
         
            copyCommand.Execute();
            foreach (UIElement el in schemaView.SelectionManager.SelectedObjects)
            {
                schemaView.UndoBuff.AddCommand(new DeleteGraphicsObject(el));
                    //ActiveTool.NotifyObjectDeleted(el);
            }

            schemaView.SelectionManager.SelectObject(null);
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
                return global::FreeSCADA.Designer.Properties.Resources.cut;
            }
        }
        #endregion ICommand Members
    }
    class PasteCommand : SchemaCommand
    {
        public PasteCommand(SchemaView sv) :
            base(sv)
        {
        }
        public override void CheckApplicability()
        {
            SelectionTool tool = schemaView.ActiveTool as SelectionTool;
            if (tool != null && System.Windows.Clipboard.ContainsText(System.Windows.TextDataFormat.Xaml))
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members
        public override void Execute()
        {
            SelectionTool tool = schemaView.ActiveTool as SelectionTool;
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
                        Canvas uielements = XamlReader.Load(stream) as Canvas;
                        while (uielements.Children.Count != 0)
                        {
                            UIElement el = uielements.Children[0];
                            uielements.Children.Remove(el);
                            Canvas.SetLeft(el, Canvas.GetLeft(el) - Canvas.GetLeft(uielements) + tool.LastClickedPoint.X);
                            Canvas.SetTop(el, Canvas.GetTop(el) - Canvas.GetTop(uielements) + tool.LastClickedPoint.Y);
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
                return global::FreeSCADA.Designer.Properties.Resources.paste_plain;
            }
        }
        #endregion ICommand Members
    }

    class XamlViewCommand : SchemaCommand
    {
        public XamlViewCommand(SchemaView sv)
            : base(sv)
        {
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
                if (!schemaView.XamlView.Visible)
                {
				
                    schemaView.XamlView.Show();
                    schemaView.UpdateXamlView();

					
                }
                else
                {
                    schemaView.XamlView.Hide();
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
                return global::FreeSCADA.Designer.Properties.Resources.page_white_code_red;
            }
        }
        #endregion ICommand Members
    }

    class ZoomLevelCommand : SchemaCommand, ICommandItems
    {
        double level;
        public event EventHandler CurrentChanged;

        public ZoomLevelCommand(SchemaView sv) :
            base(sv)
        {
            Priority = (int)CommandManager.Priorities.ViewCommands;
            sv.ZoomManager.ZoomChanged += new EventHandler(ZoomGesture_ZoomChanged);
            this.level = 1.0;
        }

        void ZoomGesture_ZoomChanged(object sender, EventArgs e)
        {
            Level = (sender as FreeSCADA.Common.Schema.Gestures.MapZoom).Zoom;
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

            schemaView.ZoomManager.Zoom =level ;

            schemaView.Focus();
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
        public ZoomInCommand(SchemaView sv) :
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

            schemaView.ZoomManager.Zoom *= 1.05;
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
                return global::FreeSCADA.Designer.Properties.Resources.zoom_in;
            }
        }
        #endregion ICommand Members
    }

    class ZoomOutCommand : SchemaCommand
    {
        public ZoomOutCommand(SchemaView sv) :
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

            schemaView.ZoomManager.Zoom /= 1.05;
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
                return global::FreeSCADA.Designer.Properties.Resources.zoom_out;
            }
        }
        #endregion ICommand Members
    }

    class UndoCommand : SchemaCommand
    {
        public UndoCommand(SchemaView sv) :
            base(sv)
        {

        }

        public override void CheckApplicability()
        {
            DocumentView doc = schemaView;

            if (doc.UndoBuff.CanUndo())
                CanExecute = true;
            else
                CanExecute = false;
        }

        public override void Execute()
        {
            DocumentView doc = (DocumentView)schemaView;
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
                return global::FreeSCADA.Designer.Properties.Resources.arrow_undo;
            }
        }
        #endregion
    }

    class RedoCommand : SchemaCommand
    {
        public RedoCommand(SchemaView sv) :
            base(sv)
        {

        }

        public override void CheckApplicability()
        {
            DocumentView doc = schemaView;

            if (doc != null && doc.UndoBuff.CanRedo())
                CanExecute = true;
            else
                CanExecute = false;
        }

        public override void Execute()
        {
            DocumentView doc = schemaView;
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
                return global::FreeSCADA.Designer.Properties.Resources.arrow_redo;
            }
        }
        #endregion Informational properties
    }

    class CommonBindingCommand : SchemaCommand
    {
        public CommonBindingCommand(SchemaView sv) :
            base(sv)
        {
            
        }

        public override void CheckApplicability()
        {
            
            if (schemaView.SelectionManager.SelectedObjects.Count == 1)
                CanExecute = true;
            else
                CanExecute = false;
        }

        public override void Execute()
        {
            SelectionTool tool = schemaView.ActiveTool as SelectionTool;
            
            bool found = false;
            foreach (IVisualControlsPlug p in Env.Current.VisualPlugins.Plugins)
            {
                foreach (IVisualControlDescriptor d in p.Controls)
                {
                    if (schemaView.SelectionManager.SelectedObjects[0].GetType() == d.Type)
                    {
                        CommonBindingDialog dlg = new CommonBindingDialog(d.getPropProxy(schemaView.SelectionManager.SelectedObjects[0]));
                        dlg.ShowDialog(Env.Current.MainWindow);
                        found = true;
                    }
                }
            }
            if (!found)
            {
                CommonBindingDialog dlg = new CommonBindingDialog(new PropProxy(schemaView.SelectionManager.SelectedObjects[0]));
                dlg.ShowDialog(Env.Current.MainWindow);
            }

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
        public override void CheckApplicability()
        {

                CanExecute = true;
        }
        public ImportElementCommand(SchemaView sv)
            : base(sv)
        {
            Priority = (int)CommandManager.Priorities.EditCommands;
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
                 UIElement obj = XamlReader.Load(xmlReader) as UIElement;
                 Canvas.SetTop(obj, 0);
                 Canvas.SetLeft(obj, 0);
                 schemaView.MainCanvas.Children.Add(obj);
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
                     schemaView.MainCanvas.Children.Add(v);
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