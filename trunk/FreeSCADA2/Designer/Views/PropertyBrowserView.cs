using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using FreeSCADA.Common;
using FreeSCADA.Common.Schema;
using FreeSCADA.Common.Scripting;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.Views
{
    class PropertyBrowserView : ToolWindow
    {
        private System.Windows.Forms.PropertyGrid propertyGrid;

        public PropertyBrowserView()
		{
			TabText = "Property Browser";
            InitializeComponent();
		}

        private void InitializeComponent()
        {
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(292, 273);
            this.propertyGrid.TabIndex = 0;
            // 
            // PropertyBrowserView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.propertyGrid);
            this.Name = "PropertyBrowserView";
            this.ResumeLayout(false);

        }

        public void ShowProperties(object obj)
        {
            try
            {
                if (propertyGrid.SelectedObject is IDisposable)
                    (propertyGrid.SelectedObject as IDisposable).Dispose();

				if (obj != null)
				{
					propertyGrid.SelectedObject = obj;
					propertyGrid.PropertyTabs.AddTabType(typeof(EventsTab));
				}
            /*if(obj is CommonShortProp)
                (obj as CommonShortProp).PropertiesChanged += new CommonShortProp.PropertiesChangedDelegate(PropertyBrowserView_PropertiesChanged);*/
			}
            catch { };
        }
        
		delegate void InvokeDelegate();
        void PropertyBrowserView_PropertiesChanged()
        {
			propertyGrid.BeginInvoke(new InvokeDelegate(delegate() { propertyGrid.Refresh(); }));
        }
	}

	class EventsTab : System.Windows.Forms.Design.PropertyTab
	{
		public override System.ComponentModel.PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			//Return list of known events
            EventDescriptorCollection events_info;
			if (component is PropProxy)
				events_info = (component as PropProxy).GetEvents();
            else
				events_info = TypeDescriptor.GetEvents(component, true);

            

            var events = new PropertyDescriptor[events_info.Count];
            for (int i = 0; i < events_info.Count; i++)
                events[i] = new EventWrapper(events_info[i].Name);

            return new PropertyDescriptorCollection(events);

		}

		public override string TabName
		{
			get { return "Events"; }
		}

		public override System.Drawing.Bitmap Bitmap
		{
			get { return Properties.Resources.open_events; }
		}
	}

	class EventWrapper : PropertyDescriptor
	{
		string name;
		public EventWrapper(string name)
			: base(name, null)
		{
			this.name = name;
		}

		public override bool CanResetValue(object component)
		{
			return true;
		}

		public override string DisplayName
		{
			get
			{
				return name;
			}
		}

		public override string Description
		{
			get
			{
				return string.Format("{0} description", name);
			}
		}

		public override bool IsReadOnly
		{
			get { return false; }
		}

		public override string Name
		{
			get
			{
				return "fs2_"+name;
			}
		}

		public override Type ComponentType
		{
			get { throw new NotImplementedException(); }
		}

		public override object GetValue(object component)
		{
			if (component is PropProxy)
			{
				object obj = (component as PropProxy).ControlledObject;
				if (obj is DependencyObject)
				{
					var events = EventScriptCollection.GetEventScriptCollection(obj as DependencyObject);
					var callInfo = events.GetAssosiation(name);
					if (callInfo == null)
						return "";
					else
						return callInfo.HandlerName;
				}
			}

			if (component is IChannel)
			{
				var callInfo = Env.Current.ScriptManager.ChannelsHandlers.GetAssosiation(name, component as IChannel);

				if (callInfo == null)
					return "";
				else
					return callInfo.HandlerName;
			}

			return null;
		}

		public override Type PropertyType
		{
			get { return typeof(string); }
		}

		public override void ResetValue(object component)
		{
			throw new NotImplementedException();
		}

		public override void SetValue(object component, object value)
		{
			if (component is PropProxy)
			{
				object obj = (component as PropProxy).ControlledObject;
				if (obj is DependencyObject)
				{

                    var ub = SchemaEditor.UndoRedoManager.GetUndoBufferFor(obj as System.Windows.UIElement);
                    ub.AddCommand(new SchemaEditor.ModifyGraphicsObject(obj as System.Windows.UIElement));
            
					if (string.IsNullOrEmpty(value as string))
					{
						var events = EventScriptCollection.GetEventScriptCollection(obj as DependencyObject);
						events.RemoveAssociation(name);
						EventScriptCollection.SetEventScriptCollection(obj as DependencyObject, events);
					}
					else
					{
						var c = SchemaDocument.GetMainCanvas(obj as DependencyObject);
						var callInfo = new ScriptCallInfo();
						callInfo.HandlerName = value as string;
						if (c != null)
							callInfo.ScriptName = (c.Tag as DocumentView).DocumentName;
						else
							callInfo.ScriptName = "unnamed";

						var events = EventScriptCollection.GetEventScriptCollection(obj as DependencyObject);
						events.AddAssociation(name, callInfo);
						EventScriptCollection.SetEventScriptCollection(obj as DependencyObject, events);

						var script = Env.Current.ScriptManager.GetScript(callInfo.ScriptName);
						if (script == null)
							script = Env.Current.ScriptManager.CreateNewScript(callInfo.ScriptName);

						var evnt = obj.GetType().GetEvent(name);
						script.AddHandlerTemplate(callInfo.HandlerName, evnt);
					}
				}
			}

			if (component is IChannel)
			{
				if (string.IsNullOrEmpty(value as string))
				{
					Env.Current.ScriptManager.ChannelsHandlers.RemoveAssociation(name, component as IChannel);
				}
				else
				{
					var callInfo = new ScriptCallInfo();
					callInfo.HandlerName = value as string;
					callInfo.ScriptName = ScriptManager.ChannelsScriptName;

					Env.Current.ScriptManager.ChannelsHandlers.AddAssociation(name, component as IChannel, callInfo);

					var script = Env.Current.ScriptManager.GetScript(ScriptManager.ChannelsScriptName);
					if (script == null)
						script = Env.Current.ScriptManager.CreateNewScript(ScriptManager.ChannelsScriptName);

					var evnt = component.GetType().GetEvent(name);
					script.AddHandlerTemplate(value as string, evnt);
				}
			}
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
	}	
}
