using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using FreeSCADA.Interfaces;


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

	public class ChannelsScriptHandlers
	{
		//channelId, event, callInfo
		Dictionary<string, Dictionary<string, ScriptCallInfo>> associations = new Dictionary<string, Dictionary<string, ScriptCallInfo>>();

		public void AddAssociation(string evnt, IChannel ch, ScriptCallInfo info)
		{
			if (associations.ContainsKey(ch.FullId) == false)
				associations[ch.FullId] = new Dictionary<string, ScriptCallInfo>();
			
			associations[ch.FullId][evnt] = info;
			Save();
		}

		public void RemoveAssociation(string evnt, IChannel ch)
		{
			if (associations.ContainsKey(ch.FullId) && associations[ch.FullId].ContainsKey(evnt))
			{
				associations[ch.FullId].Remove(evnt);
				if (associations[ch.FullId].Count == 0)
					associations.Remove(ch.FullId);
			}
			Save();
		}

		public ScriptCallInfo GetAssosiation(string evnt, IChannel ch)
		{
			if (associations.ContainsKey(ch.FullId) && associations[ch.FullId].ContainsKey(evnt))
				return associations[ch.FullId][evnt];
			else
				return null;
		}

		public void InstallHandlers()
		{
			if (Env.Current.Mode == FreeSCADA.Interfaces.EnvironmentMode.Runtime)
			{
				foreach (string channelName in associations.Keys)
				{
					IChannel channel = Env.Current.CommunicationPlugins.GetChannel(channelName);
					if (channel == null)
						continue;

					foreach (string eventName in associations[channelName].Keys)
					{
						EventInfo evnt = channel.GetType().GetEvent(eventName);
						if (evnt == null)
							continue;

						//connect events to script objects
						object eventHandler = EventHandlerFactory.GetEventHandler(evnt);

						// Create a delegate, which points to the custom event handler
						Delegate customEventDelegate = Delegate.CreateDelegate(evnt.EventHandlerType, eventHandler, "CustomEventHandler");
						// Link event handler to event
						evnt.AddEventHandler(channel, customEventDelegate);

						// Map our own event handler to the common event
						Script script = Env.Current.ScriptManager.GetScript(associations[channelName][eventName].ScriptName);
						if (script == null)
							continue;

						ChannelEventProxy proxy = new ChannelEventProxy(channel, script, associations[channelName][eventName].HandlerName);

						EventInfo commonEventInfo = eventHandler.GetType().GetEvent("CommonEvent");
						Delegate commonDelegate = Delegate.CreateDelegate(commonEventInfo.EventHandlerType, proxy, "OnEvent");
						commonEventInfo.AddEventHandler(eventHandler, commonDelegate);
					}
				}
			}
		}

		internal void Load()
		{
			associations.Clear();

			XmlDocument doc = new System.Xml.XmlDocument();
			using (Stream stream = Env.Current.Project.GetData(ProjectEntityType.Settings, "channels_script_handlers"))
			{
				if (stream != null)
				{	
					try
					{
						doc.Load(stream);
					}
					catch
					{
						return;
					}
				}
			}

			XmlNodeList channelNodes = doc.GetElementsByTagName("channel");
			foreach (XmlElement channelNode in channelNodes)
			{
				XmlNodeList eventNodes = channelNode.GetElementsByTagName("event");
				if (eventNodes.Count > 0)
				{
					string channelName = channelNode.GetAttribute("name");
					associations[channelName] = new Dictionary<string, ScriptCallInfo>();
					foreach (XmlElement eventNode in eventNodes)
					{
						ScriptCallInfo ci = new ScriptCallInfo();
						ci.HandlerName = eventNode.GetAttribute("handlerName");
						ci.ScriptName = eventNode.GetAttribute("scriptName");
						associations[channelName][eventNode.GetAttribute("name")] = ci;
					}
				}
			}
		}

		internal void Save()
		{
			XmlDocument doc = new System.Xml.XmlDocument();
			XmlElement rootElem = doc.CreateElement("root");
			foreach (string channelName in associations.Keys)
			{
				XmlElement channelElem = doc.CreateElement("channel");
				channelElem.SetAttribute("name", channelName);

				foreach (string eventName in associations[channelName].Keys)
				{
					XmlElement eventElem = doc.CreateElement("event");
					eventElem.SetAttribute("name", eventName);
					eventElem.SetAttribute("scriptName", associations[channelName][eventName].ScriptName);
					eventElem.SetAttribute("handlerName", associations[channelName][eventName].HandlerName);
					channelElem.AppendChild(eventElem);
				}
				rootElem.AppendChild(channelElem);
			}
			doc.AppendChild(rootElem);

			using (MemoryStream stream = new MemoryStream())
			{
				doc.Save(stream);
				Env.Current.Project.SetData(ProjectEntityType.Settings, "channels_script_handlers", stream);
			}
		}
	}

	class ChannelEventProxy
	{
		IChannel channel;
		Script script;
		string handlerName;

		public ChannelEventProxy(IChannel channel, Script script, string handlerName)
		{
			this.channel = channel;
			this.script = script;
			this.handlerName = handlerName;
		}

		public void OnEvent(Type EventType, object[] args)
		{
			script.Execute(handlerName, args);
		}
	}
}
