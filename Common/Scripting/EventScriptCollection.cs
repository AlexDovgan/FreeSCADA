using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace FreeSCADA.Common.Scripting
{
	public class ScriptCallInfo
	{
		string scriptName;
		string handlerName;

		public string ScriptName
		{
			get { return scriptName; }
			set { scriptName = value; }
		}

		public string HandlerName
		{
			get { return handlerName; }
			set { handlerName = value; }
		}
	}

	public class EventScriptCollection
	{
		Dictionary<string, ScriptCallInfo> associations = new Dictionary<string, ScriptCallInfo>();

		public Dictionary<string, ScriptCallInfo> Associations
		{
			get { return associations; }
			set { associations = value; }
		}

		public static readonly DependencyProperty EventScriptCollectionProperty = DependencyProperty.RegisterAttached(
			typeof(EventScriptCollection).Name,
			typeof(EventScriptCollection),
			typeof(EventScriptCollection),
			new FrameworkPropertyMetadata(new EventScriptCollection(), new PropertyChangedCallback(PropertyChangedCallback)));

		public static void SetEventScriptCollection(DependencyObject obj, EventScriptCollection value)
		{
			obj.SetValue(EventScriptCollectionProperty, value);
			//Env.Current.Project.IsModified = true;
		}
		public static EventScriptCollection GetEventScriptCollection(DependencyObject obj)
		{
			if (obj.ReadLocalValue(EventScriptCollectionProperty) == DependencyProperty.UnsetValue)
				return new EventScriptCollection();
			else
				return (EventScriptCollection)obj.GetValue(EventScriptCollectionProperty);
		}

		public static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != null && e.NewValue is EventScriptCollection)
				(e.NewValue as EventScriptCollection).InvalidateEvents(d);
		}

		private void InvalidateEvents(DependencyObject obj)
		{
			if (Env.Current.Mode == FreeSCADA.Interfaces.EnvironmentMode.Runtime)
			{
				foreach (string eventName in associations.Keys)
				{
					EventInfo evnt = obj.GetType().GetEvent(eventName);
					if (evnt == null)
						continue;

					//connect events to script objects
					object eventHandler = EventHandlerFactory.GetEventHandler(evnt);

					// Create a delegate, which points to the custom event handler
					Delegate customEventDelegate = Delegate.CreateDelegate(evnt.EventHandlerType, eventHandler, "CustomEventHandler");
					// Link event handler to event
					evnt.AddEventHandler(obj, customEventDelegate);

					// Map our own event handler to the common event
					Script script = Env.Current.ScriptManager.GetScript(associations[eventName].ScriptName);
					if (script == null)
						continue;

					SchemaEventProxy proxy = new SchemaEventProxy(obj, script, associations[eventName].HandlerName);

					EventInfo commonEventInfo = eventHandler.GetType().GetEvent("CommonEvent");
					Delegate commonDelegate = Delegate.CreateDelegate(commonEventInfo.EventHandlerType, proxy, "OnEvent");
					commonEventInfo.AddEventHandler(eventHandler, commonDelegate);
				}
			}
		}

		public void AddAssociation(string evnt, ScriptCallInfo info)
		{
			associations[evnt] = info;
		}

		public void RemoveAssociation(string evnt)
		{
			if (associations.ContainsKey(evnt))
				associations.Remove(evnt);
		}

		public ScriptCallInfo GetAssosiation(string evnt)
		{
			if (associations.ContainsKey(evnt))
				return associations[evnt];
			else
				return null;
		}
	}

	class SchemaEventProxy
	{
		DependencyObject obj;
		Script script;
		string handlerName;

		public SchemaEventProxy(DependencyObject obj, Script script, string handlerName)
		{
			this.obj = obj;
			this.script = script;
			this.handlerName = handlerName;
		}

		public void OnEvent(Type EventType, object[] args)
		{
			//Need to check if "obj" object should also be passed to the handler. 
			//As far as I see it is always available as first argument in args
			
			script.Execute(handlerName, args);
		}
	}
}
