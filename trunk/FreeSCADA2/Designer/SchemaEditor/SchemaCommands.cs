using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Common;
using FreeSCADA.Designer.SchemaEditor.Tools;
using System.Windows.Input;
using FreeSCADA.Designer.Views;
using System.Drawing;
// TODO: all commands mast receive DocumentView class for operate with him

namespace FreeSCADA.Designer.SchemaEditor.SchemaCommands
{
    
    class ToolContextMenu : System.Windows.Forms.ContextMenu
    {
      /*  public MenuItem unGroupMenuItem = new MenuItem();
        public MenuItem groupMenuItem = new MenuItem();
        public MenuItem copyMenuItem = new MenuItem();
        public MenuItem cutMenuItem = new MenuItem();
        public MenuItem pasteMenuItem = new MenuItem();
        public MenuItem viewXamlMenuItem = new MenuItem();
        

        public ToolContextMenu()
        {

            unGroupMenuItem.Header = "Ungroup";
            unGroupMenuItem.Command = new UngroupCommand();
            MenuItems.Add(unGroupMenuItem);


            groupMenuItem.Header = "Group";
            groupMenuItem.Command = new GroupCommand();
            Items.Add(groupMenuItem);

            copyMenuItem.Header = "Copy";
            copyMenuItem.Command = new CopyCommand();
            Items.Add(copyMenuItem);

            cutMenuItem.Header = "Cut";
            cutMenuItem.Command = new CutCommand();
            Items.Add(cutMenuItem);

            pasteMenuItem.Header = "Paste";
            pasteMenuItem.Command = new PasteCommand();
            Items.Add(pasteMenuItem);

            viewXamlMenuItem.Command = new XamlViewCommand();
            viewXamlMenuItem.Header = ((ICommandData)viewXamlMenuItem.Command).CommandName;
            //viewXamlMenuItem.Image = ((ICommandData)viewXamlMenuItem.Tag).CommandIcon;
            Items.Add(viewXamlMenuItem);
        }*/
    }
    ///////////
    abstract class BaseCommand : ICommandData
    {
        public BaseCommand()
        {
        }
        #region ICommand Members
        public event EventHandler CanExecuteChanged;
        public virtual bool CanExecute(object o) { return false;}
        public virtual void Execute(object o) { }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
        #endregion ICommand Members
        #region ICommandData Members

        public virtual string CommandName { get { return null; } }
		public virtual string CommandDescription { get { return null; }	}
        public virtual Bitmap CommandIcon { get { return null; } }
        System.Windows.Forms.ToolStripItem m_tsi;
        public System.Windows.Forms.ToolStripItem CommandToolStripItem
        {
            get
            {
                return m_tsi;
            }
            set
            {
                m_tsi = value;
            }
        }
        public virtual Type ToolStripItemType { get { return null; } }
        object tag;
        public Object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        public bool Enabled
        {
            get
            {
                if (m_tsi != null)
                    return m_tsi.Enabled;
                else
                    return false;
            }
            set
            {
                if (m_tsi != null)
                    m_tsi.Enabled = value;
            }
        }

        public void EvtHandler(object sender, System.EventArgs e)
        {
            this.Execute((sender as System.Windows.Forms.ToolStripItem).Tag);
        }
        #endregion ICommandData Members
    }

    class UngroupCommand : BaseCommand
    {
        SelectionTool tool;
        public UngroupCommand()
        {
        }
        #region ICommand Members
        public override bool CanExecute(object o)
        {

            if (o is SelectionTool)
                tool = o as SelectionTool;
            else
            {
                Enabled = false;
                return false;
            }
            if ((tool.ToolManipulator != null) && (tool.ToolManipulator.AdornedElement is Viewbox))
            {
                Enabled = true;
                return true;
            }
            Enabled = false;
            return false;
        }
        public override void Execute(object o)
        {
            EditorHelper.BreakGroup(o as SelectionTool);
        }
        #endregion ICommand Members
        #region ICommandData Members

        public override string CommandName
        {
            get { return StringResources.CommandUngroupName; }
        }

		public override string CommandDescription 
		{ 
			get { return StringResources.CommandUngroupDescription; } 
		}

        public override Bitmap CommandIcon
        {
            get
            {
                return global::FreeSCADA.Designer.Properties.Resources.shape_ungroup;
            }
        }
        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripButton);
            }
        }
        #endregion ICommandData Members
    }

    class GroupCommand : BaseCommand
    {
        SelectionTool tool;
        public GroupCommand()
        {
        }
        #region ICommand Members
        public override bool CanExecute(object o)
        {

            if (o is SelectionTool)
                tool = o as SelectionTool;
            else
            {
                Enabled = false;
                return false;
            }
            if (tool.selectedElements.Count > 0)
            {
                Enabled = true;
                return true;
            }
            Enabled = false;
            return false;
        }
        public override void Execute(object o)
        {

            EditorHelper.CreateGroup(o as SelectionTool);
            
        }
        #endregion ICommand Members
        #region ICommandData Members

        public override string CommandName
        {
			get { return StringResources.CommandGroupName; }
        }

		public override string CommandDescription
		{
			get { return StringResources.CommandGroupDescription; }
		}

        public override Bitmap CommandIcon
        {
            get {
                return global::FreeSCADA.Designer.Properties.Resources.shape_group;
            }
        }

        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripButton);
            }
        }
        #endregion ICommandData Members
    }
    
    class CopyCommand : BaseCommand
    {
        SelectionTool tool;
        public CopyCommand()
        {
        }
       #region ICommand Members
       public override bool CanExecute(object o)
        {

            if (o is SelectionTool)
                tool = o as SelectionTool;
            else
            {
                Enabled = false;
                return false;
            }
            if (tool.SelectedObject != null)
            {
                Enabled = true;
                return true;
            }
            Enabled = false;
            return false;
            
        }
        public override void Execute(object o)
        {

            string xaml = XamlWriter.Save((o as SelectionTool).SelectedObject );
            System.Windows.Clipboard.SetText(xaml, System.Windows.TextDataFormat.Xaml);
  
        }
        #endregion ICommand Members
        #region ICommandData Members

        public override string CommandName
        {
            get { return StringResources.CommandCopyName;}
        }

		public override string CommandDescription
		{
			get { return StringResources.CommandCopyDescription; }
		}

        public override Bitmap CommandIcon
        {
            get {
                return global::FreeSCADA.Designer.Properties.Resources.page_copy;
            }
        }

        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripButton);
            }
        }
        #endregion ICommandData Members
    }

    class CutCommand : BaseCommand
    {
        SelectionTool tool;
        public CutCommand()
        {
        }
        #region ICommand Members
        public override bool CanExecute(object o)
        {
            if (o is SelectionTool)
                tool = o as SelectionTool;
            else
            {
                Enabled = false;
                return false;
            }
            if (tool.SelectedObject != null)
            {
                Enabled = true;
                return true;
            }
            Enabled = false;
            return false;
        }
        public override void Execute(object o)
        {
            string xaml = XamlWriter.Save((o as SelectionTool).SelectedObject);
            System.Windows.Clipboard.SetText(xaml, System.Windows.TextDataFormat.Xaml);
            (o as SelectionTool).NotifyObjectDeleted((o as SelectionTool).SelectedObject);
            (o as SelectionTool).SelectedObject = null;
        }
        #endregion ICommand Members
        #region ICommandData Members

        public override string CommandName
        {
            get { return StringResources.CommandCutName;}
        }

		public override string CommandDescription
		{
			get { return StringResources.CommandCutDescription; }
		}

        public override Bitmap CommandIcon
        {
            get {
                return global::FreeSCADA.Designer.Properties.Resources.cut;
            }
        }

        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripButton);
            }
        }
        #endregion ICommandData Members
    }
    class PasteCommand : BaseCommand
    {
        SelectionTool tool;
        public PasteCommand()
        {
        }
        #region ICommand Members
        public override bool CanExecute(object o)
        {
            if (o is SelectionTool)
                tool = o as SelectionTool;
            else
            {
                Enabled = false;
                return false;
            }
            if (System.Windows.Clipboard.ContainsText(System.Windows.TextDataFormat.Xaml))
            {
                Enabled = true;
                return true;
            }
            Enabled = false;
            return false;
        }
        public override void Execute(object o)
        {
            string xaml = System.Windows.Clipboard.GetText(System.Windows.TextDataFormat.Xaml);
            if (xaml != null)
            {
                using (MemoryStream stream = new MemoryStream(xaml.Length))
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(xaml);
                        sw.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        UIElement el = XamlReader.Load(stream) as UIElement;
                        Canvas.SetLeft(el, Mouse.GetPosition(tool).X);
                        Canvas.SetTop(el, Mouse.GetPosition(tool).Y);
                        (o as SelectionTool).NotifyObjectCreated(el);
                     }
                }
            }
        }
        #endregion ICommand Members
        #region ICommandData Members

		public override string CommandName
		{
			get { return StringResources.CommandPasteName; }
		}

		public override string CommandDescription
		{
			get { return StringResources.CommandPasteDescription; }
		}

        public override Bitmap CommandIcon
        {
            get {
                return global::FreeSCADA.Designer.Properties.Resources.paste_plain;
            }
        }

        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripButton);
            }
        }
        #endregion ICommandData Members
    }

    class XamlViewCommand : BaseCommand
    {

        SchemaView _view;
        public XamlViewCommand(SchemaView view)
        {
            _view = view;
        }
        #region ICommand Members
        public override bool CanExecute(object o)
        {
            
                return true;
     
    
            
        }
        public override void Execute(object o)
        {
            
            try
            {
                if (!_view.XamlView.Visible)
                {
                    _view.XamlView.Show();
                    _view.UpdateXamlView();
                }
                else
                {
                    _view.XamlView.Hide();
                }
              
            }
            catch { }
        }

        #endregion ICommand Members
        #region ICommandData Members

		public override string CommandName
		{
			get { return StringResources.CommandXamlViewName; }
		}

		public override string CommandDescription
		{
			get { return StringResources.CommandXamlViewDescription; }
		}

        public override Bitmap CommandIcon
        {
            get { 
                return global::FreeSCADA.Designer.Properties.Resources.page_white_code_red;
            }
        }
        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripButton);
            }
        }
        #endregion ICommandData Members
    }

    class ZoomLevelCommand : ICommand
    {
        double Level;
        public ZoomLevelCommand(double level)
        {
            Level = level;
        }
        public bool CanExecute(object o)
        {
            if (o is SchemaView) return true;
            return false;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {
            if (o is SchemaView) ((SchemaView)o).ZoomLevel = Level;
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }

    class ZoomInCommand : BaseCommand
    {
        public ZoomInCommand()
        {
            Enabled = true;
        }
        #region ICommand Members
        public override bool CanExecute(object o)
        {
            return true;
        }
        public override void Execute(object o)
        {
            if (o is SchemaView) ((SchemaView)o).ZoomIn();
        }
        #endregion ICommand Members
        #region ICommandData Members

		public override string CommandName
		{
			get { return StringResources.CommandZoomInName; }
		}

		public override string CommandDescription
		{
			get { return StringResources.CommandZoomInDescription; }
		}

        public override Bitmap CommandIcon
        {
            get { 
                return global::FreeSCADA.Designer.Properties.Resources.zoom_in;
            }
        }
        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripButton);
            }
        }
        #endregion ICommandData Members
   }

    class ZoomOutCommand : BaseCommand
    {
        public ZoomOutCommand()
        {
            Enabled = true;
        }
        #region ICommand Members
        public override bool CanExecute(object o)
        {
            return true;
        }
        public override void Execute(object o)
        {
            if (o is SchemaView) ((SchemaView)o).ZoomOut();
        }
        #endregion ICommand Members
        #region ICommandData Members

		public override string CommandName
		{
			get { return StringResources.CommandZoomOutName; }
		}

		public override string CommandDescription
		{
			get { return StringResources.CommandZoomOutDescription; }
		}

        public override Bitmap CommandIcon
        {
            get
            {
                return global::FreeSCADA.Designer.Properties.Resources.zoom_out;
            }
        }
        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripButton);
            }
        }
        #endregion ICommandData Members
    }

    class UndoCommand : BaseCommand
    {
        public UndoCommand()
        {
        }
        #region ICommand Members
        public override bool CanExecute(object o)
        {
            if (o is DocumentView)
                if ((o as DocumentView).undoBuff.CanUndo())
                {
                    Enabled = true;
                    return true;
                }
                else
                {
                    Enabled = false;
                    return false;
                }
            return Enabled;
        }
        public override void Execute(object o)
        {
            if (o is DocumentView) ((DocumentView)o).undoBuff.UndoCommand();
        }
        #endregion ICommand Members
        #region ICommandData Members

		public override string CommandName
		{
			get { return StringResources.CommandUndoName; }
		}

		public override string CommandDescription
		{
			get { return StringResources.CommandUndoDescription; }
		}

        public override Bitmap CommandIcon
        {
            get
            {
                return global::FreeSCADA.Designer.Properties.Resources.arrow_undo;
            }
        }
        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripButton);
            }
        }
        #endregion ICommandData Members
    }

    class RedoCommand : BaseCommand
    {
        public RedoCommand()
        {
        }
        #region ICommand Members
       public override bool CanExecute(object o)
        {
            if (o is DocumentView)
                if ((o as DocumentView).undoBuff.CanRedo())
                {
                    Enabled = true;
                    return true;
                }
                else
                {
                    Enabled = false;
                    return false;
                }
            return Enabled;
        }
        public override void Execute(object o)
        {
            if (o is DocumentView) ((DocumentView)o).undoBuff.RedoCommand();
        }
        #endregion ICommand Members
        #region ICommandData Members

		public override string CommandName
		{
			get { return StringResources.CommandRedoName; }
		}

		public override string CommandDescription
		{
			get { return StringResources.CommandRedoDescription; }
		}

        public override Bitmap CommandIcon
        {
            get
            {
                return global::FreeSCADA.Designer.Properties.Resources.arrow_redo;
            }
        }
        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripButton);
            }
        }
        #endregion ICommandData Members
    }

    class NullCommand : BaseCommand
    {
        public NullCommand()
        {
        }
        #region ICommand Members
        public override bool CanExecute(object o)
        {
            Enabled = true;
            return true;
        }
        public override void Execute(object o)
        {
        }
        #endregion ICommand Members
        #region ICommandData Members

        public override string CommandName
        {
            get { return "Null"; }
        }

		public override string CommandDescription
		{
			get { return ""; }
		}

        public override Bitmap CommandIcon
        {
            get
            {
                return null;
            }
        }

        public override Type ToolStripItemType
        {
            get
            {
                return typeof(System.Windows.Forms.ToolStripSeparator);
            }
        }
        #endregion ICommandData Members
    }

}
