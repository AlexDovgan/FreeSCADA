using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using FreeSCADA.Common;
using FreeSCADA.Designer.SchemaEditor.Tools;
using FreeSCADA.Designer.Views;

namespace FreeSCADA.Designer.SchemaEditor.SchemaCommands
{
	class SchemaCommand : BaseCommand
	{
		object controlledObject;

		public virtual object ControlledObject
		{
			get{return controlledObject;}
			set
			{
				controlledObject = value;
				CheckApplicability();
			}
		}

		public virtual void CheckApplicability(){}
	}

	class UngroupCommand : SchemaCommand
    {
		public override void CheckApplicability()
        {
			SelectionTool tool = ControlledObject as SelectionTool;
            if (tool != null && tool.ToolManipulator != null && tool.ToolManipulator.AdornedElement is Viewbox)
                CanExecute = true;
			else
				CanExecute = false;
        }

		#region ICommand Members
        public override void Execute()
        {
            EditorHelper.BreakGroup(ControlledObject as SelectionTool);
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
		public override void CheckApplicability()
		{
			SelectionTool tool = ControlledObject as SelectionTool;
			if (tool != null && tool.selectedElements.Count > 0)
				CanExecute = true;
			else
				CanExecute = false;
		}

        #region ICommand Members

		public override void Execute()
        {
            EditorHelper.CreateGroup(ControlledObject as SelectionTool);   
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

	class CopyCommand : SchemaCommand
	{
		public override void CheckApplicability()
		{
			SelectionTool tool = ControlledObject as SelectionTool;
			if (tool != null && tool.SelectedObject != null)
				CanExecute = true;
			else
				CanExecute = false;
		}

		#region ICommand Members
		public override void Execute()
		{
			string xaml = XamlWriter.Save((ControlledObject as SelectionTool).SelectedObject);
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
		public override void CheckApplicability()
		{
			SelectionTool tool = ControlledObject as SelectionTool;
			if (tool != null && tool.SelectedObject != null)
				CanExecute = true;
			else
				CanExecute = false;
		}

		#region ICommand Members
        public override void Execute()
        {
			SelectionTool tool = (SelectionTool)ControlledObject;

            string xaml = XamlWriter.Save(tool.SelectedObject);
            System.Windows.Clipboard.SetText(xaml, System.Windows.TextDataFormat.Xaml);
            tool.NotifyObjectDeleted(tool.SelectedObject);
            tool.SelectedObject = null;
        }

        public override string Name
        {
            get { return StringResources.CommandCutName;}
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
		public override void CheckApplicability()
		{
			SelectionTool tool = ControlledObject as SelectionTool;
			if (tool != null && System.Windows.Clipboard.ContainsText(System.Windows.TextDataFormat.Xaml))
				CanExecute = true;
			else
				CanExecute = false;
		}

        #region ICommand Members
        public override void Execute()
        {
			SelectionTool tool = ControlledObject as SelectionTool;
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
                        UIElement el = XamlReader.Load(stream) as UIElement;
                        Canvas.SetLeft(el, Mouse.GetPosition(tool).X);
                        Canvas.SetTop(el, Mouse.GetPosition(tool).Y);
                        tool.NotifyObjectCreated(el);
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

    class XamlViewCommand : BaseCommand
    {
        SchemaView view;
        public XamlViewCommand(SchemaView view)
        {
            this.view = view;
			CanExecute = true;
        }

        #region ICommand Members
        public override void Execute()
        {
            try
            {
				if (!view.XamlView.Visible)
                {
					view.XamlView.Show();
					view.UpdateXamlView();
                }
                else
                {
					view.XamlView.Hide();
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

    class ZoomLevelCommand : SchemaCommand
    {
        double Level;
        public ZoomLevelCommand(double level)
        {
            Level = level;
        }

		public override void CheckApplicability()
		{
			CanExecute = ControlledObject is SchemaView;
		}
        public override void Execute()
        {
			SchemaView view = (SchemaView)ControlledObject;
            view.ZoomLevel = Level;
        }
    }

    class ZoomInCommand : SchemaCommand
    {
		public override void CheckApplicability()
		{
			CanExecute = ControlledObject is SchemaView;
		}

        #region ICommand Members
        public override void Execute()
        {
			SchemaView view = (SchemaView)ControlledObject;
            view.ZoomIn();
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
		public override void CheckApplicability()
		{
			CanExecute = ControlledObject is SchemaView;
		}

		public override void Execute()
        {
			SchemaView view = (SchemaView)ControlledObject;
            view.ZoomOut();
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
		public override void CheckApplicability()
		{
			DocumentView doc = ControlledObject as DocumentView;

			if (doc != null && doc.undoBuff.CanUndo())
				CanExecute = true;
			else
				CanExecute = false;
		}

		public override void Execute()
		{
			DocumentView doc = (DocumentView)ControlledObject;
			doc.undoBuff.UndoCommand();
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
		public override void CheckApplicability()
		{
			DocumentView doc = ControlledObject as DocumentView;

			if (doc != null && doc.undoBuff.CanRedo())
				CanExecute = true;
			else
				CanExecute = false;
		}

		public override void Execute()
		{
			DocumentView doc = (DocumentView)ControlledObject;
			doc.undoBuff.RedoCommand();
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
}
