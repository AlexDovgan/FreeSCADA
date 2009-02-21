using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using FreeSCADA.Common;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils;
using FreeSCADA.Designer.SchemaEditor.Tools;
using FreeSCADA.Designer.Views;
using FreeSCADA.Interfaces;


namespace FreeSCADA.Designer.SchemaEditor.SchemaCommands
{
	class SchemaCommand : BaseCommand
	{
		object controlledObject;

		public SchemaCommand()
		{
			Priority = (int)CommandManager.Priorities.EditCommands;
		}
		public virtual object ControlledObject
		{
			get { return controlledObject; }
			set
			{
				controlledObject = value;
				CheckApplicability();
			}
		}

		public virtual void CheckApplicability() { }
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
			if (tool != null && tool.SelectedObjects.Count > 0)
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

    class ZMoveTopCommand : SchemaCommand
    {
        public override void CheckApplicability()
        {
            SelectionTool tool = ControlledObject as SelectionTool;
            if (tool != null && tool.SelectedObjects.Count > 0)
                CanExecute = true;
            else
                CanExecute = false;
        }

        #region ICommand Members

        public override void Execute()
        {
            base.Execute();
            SelectionTool tool = ControlledObject as SelectionTool;
            Canvas currCanvas = tool.AdornedElement as Canvas;
            List<UIElement> sortedList = new List<UIElement>();

            foreach (UIElement uie in currCanvas.Children)
            {
                if (tool.SelectedObjects.Contains(uie))
                {
                    sortedList.Add(uie);
                }
            }
            foreach (UIElement suie in sortedList)
            {
                currCanvas.Children.Remove(suie);
                currCanvas.Children.Add(suie);
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
		public override void CheckApplicability()
		{
			SelectionTool tool = ControlledObject as SelectionTool;
			if (tool != null && tool.SelectedObjects.Count > 0)
				CanExecute = true;
			else
				CanExecute = false;
		}

		#region ICommand Members

		public override void Execute()
		{
            base.Execute();
            SelectionTool tool = ControlledObject as SelectionTool;
            Canvas currCanvas = tool.AdornedElement as Canvas;
            List<UIElement> sortedList = new List<UIElement>();

            for (int i = currCanvas.Children.Count -1;i >= 0;i--)
            {
                if (tool.SelectedObjects.Contains(currCanvas.Children[i]))
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
		public override void CheckApplicability()
		{
			SelectionTool tool = ControlledObject as SelectionTool;
            if (tool != null && tool.SelectedObjects.Count>0)
				CanExecute = true;
			else
				CanExecute = false;
		}

		#region ICommand Members
		public override void Execute()
		{
            SelectionTool tool = ControlledObject as SelectionTool;

			System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("");
            Rect b = EditorHelper.CalculateBounds(tool.SelectedObjects, tool.AdornedElement);
            string xaml = string.Format(ci.NumberFormat,
                "<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Left=\"{0}\" Top=\"{1}\">"
                ,b.X, b.Y);

            foreach (UIElement el in tool.SelectedObjects)
            {
                
                xaml+=XamlWriter.Save(el);
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
        CopyCommand copyCommand= new CopyCommand();
		public override void CheckApplicability()
		{
            copyCommand.ControlledObject = ControlledObject;
            copyCommand.CheckApplicability();
            CanExecute = copyCommand.CanExecute;
		}

		#region ICommand Members
		public override void Execute()
		{
			SelectionTool tool = (SelectionTool)ControlledObject;
            copyCommand.Execute();
            foreach(UIElement el in tool.SelectedObjects)
            {
                tool.NotifyObjectDeleted(el);
            }
   
			tool.SelectedObject = null;
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
                        Canvas uielements  = XamlReader.Load(stream) as Canvas;
                        while (uielements.Children.Count != 0)
                        {
                            UIElement el = uielements.Children[0];
                            uielements.Children.Remove(el);
                            Canvas.SetLeft(el,Canvas.GetLeft(el) - Canvas.GetLeft(uielements)+tool.LastClickedPoint.X);
                            Canvas.SetTop(el, Canvas.GetTop(el) - Canvas.GetTop(uielements)+tool.LastClickedPoint.Y);
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

	class XamlViewCommand : BaseCommand
	{
		SchemaView view;
		public XamlViewCommand(SchemaView view)
		{
			this.view = view;
			CanExecute = true;
			Priority = (int)CommandManager.Priorities.EditCommands;
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

	class ZoomLevelCommand : SchemaCommand, ICommandItems
	{
		double level;
		public event EventHandler CurrentChanged;

		public ZoomLevelCommand()
		{
			Priority = (int)CommandManager.Priorities.ViewCommands;
			this.level = 1.0;
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
			CanExecute = ControlledObject is SchemaView;
		}
		public override void Execute()
		{
			SchemaView view = (SchemaView)ControlledObject;
			view.ZoomLevel = level;
			view.Focus();
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
		public ZoomInCommand()
		{
			Priority = (int)CommandManager.Priorities.ViewCommands;
		}

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
		public ZoomOutCommand()
		{
			Priority = (int)CommandManager.Priorities.ViewCommands;
		}

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

	class CommonBindingCommand : SchemaCommand
	{
		public override void CheckApplicability()
		{
			SelectionTool tool = ControlledObject as SelectionTool;
			if (tool != null && tool.SelectedObjects.Count == 1)
				CanExecute = true;
			else
				CanExecute = false;
		}

		public override void Execute()
		{
			SelectionTool tool = ControlledObject as SelectionTool;

			CommonBindingDialog dlg = new CommonBindingDialog(new PropProxy(tool.SelectedObjects[0]));
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
}
