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
        
        System.Windows.Media.Geometry helperObject;

        public System.Windows.Media.Geometry HelperObject
        {
            get { return helperObject; }
            set { helperObject = value; }
        }

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
                actionChannel.PropertyChanged += Execute;
                
                return true;
            }
            return false;
        }
        public void DeactivateAction()
        {
            actionChannel.PropertyChanged -= Execute;
        }
        
        protected virtual void PrepareExecute()
        {
            
        }

        protected virtual void Execute(object sender,EventArgs e)
        {

        }
        public  virtual bool CheckActionFor(UIElement obj)
        {
            if (obj is FrameworkElement)
                return true;
            
            return false;
        }
        
        
    }
}
