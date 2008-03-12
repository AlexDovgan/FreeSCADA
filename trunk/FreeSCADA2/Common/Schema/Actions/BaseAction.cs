using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.ComponentModel;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Common.Schema.Actions
{
    /// <summary>
    /// base action class 
    /// </summary>
    abstract public class BaseAction
    {
        protected string actionChannelName;
        protected FrameworkElement actionedObject;
        protected string objectName;
        protected IChannel actionChannel;
        protected bool isLinked=false;

        public string ActionChannelName
        {
            get{  return actionChannelName; }
            set{  actionChannelName = value; }
        }

        /*public string ActionedObjectName
        {
            get { return objectName; }
            set {objectName=value;}
        }
        */

         
        public bool ActivateActionFor(FrameworkElement obj)
        {
            if ((actionChannel = Env.Current.CommunicationPlugins.GetChannel(ActionChannelName)) != null &&
                CheckActionFor(obj))
            {
                actionedObject = obj;
                isLinked = true;
                PrepareExecute();
                actionChannel.ValueChanged += Execute;
                
                return true;
            }
            return false;
        }
        public void DeactivateAction()
        {
            actionChannel.ValueChanged -= Execute;
        }
        
        protected virtual void PrepareExecute()
        {

        }

        protected virtual void Execute(object sender,EventArgs e)
        {

        }
        public  virtual bool CheckActionFor(FrameworkElement obj)
        {
            return true;
        }
        
    }
}
