using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using FreeSCADA.RunTime.Views;


namespace FreeSCADA.RunTime.DocumentCommands
{
	class BaseDocumentCommand : BaseCommand
	{
		protected object controlledObject;

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

	class ZoomLevelCommand : BaseDocumentCommand, ICommandItems
	{
		double level;
		public event EventHandler CurrentChanged;
        public override  object ControlledObject
        {
            get { return controlledObject; }
            set
            {
                controlledObject = value;
                (controlledObject as SchemaView).ZoomGesture.ZoomChanged += new EventHandler(ZoomGesture_ZoomChanged);
                CheckApplicability();
            }
        }

        void ZoomGesture_ZoomChanged(object sender, EventArgs e)
        {
            Level = (sender as FreeSCADA.Common.Gestures.MapZoom).Zoom;
        }

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
			view.ZoomGesture.Zoom = level;
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

	class ZoomInCommand : BaseDocumentCommand
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
			view.ZoomGesture.Zoom*=1.05;
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
				return global::FreeSCADA.RunTime.Properties.Resources.zoom_in;
			}
		}
		#endregion ICommand Members
	}

	class ZoomOutCommand : BaseDocumentCommand
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
            view.ZoomGesture.Zoom /= 1.05;
			//view.ZoomOut();
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
				return global::FreeSCADA.RunTime.Properties.Resources.zoom_out;
			}
		}
		#endregion ICommand Members
	}
}
