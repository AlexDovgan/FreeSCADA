using System;
using System.Windows;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Common.Schema.Actions
{
    /// <summary>
    /// base action class 
    /// </summary>
    abstract public class BaseAction
    {
        protected string channelName;
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
            get{  return channelName; }
            set{  channelName = value; }
        }

        
        public bool ActivateActionFor(FrameworkElement obj)
        {

            if ((actionChannel = Env.Current.CommunicationPlugins.GetChannel(ActionChannelName)) != null &&
                CheckActionFor(obj))
            {
                actionedObject = obj;
                System.Windows.Media.TransformGroup tg = new System.Windows.Media.TransformGroup();
                tg.Children.Add(actionedObject.RenderTransform);
                tg.Children.Add(new System.Windows.Media.ScaleTransform());
                tg.Children.Add(new System.Windows.Media.SkewTransform());
                tg.Children.Add(new System.Windows.Media.RotateTransform());
                tg.Children.Add(new System.Windows.Media.TranslateTransform());
                actionedObject.RenderTransform = tg;

                
                isLinked = true;
                PrepareExecute();
                actionChannel.PropertyChanged += Execute;
                
                return true;
            }
            else
                Env.Current.Logger.LogWarning(string.Format("BaseAction: Failed to activate Channel '{0}' for an action on object Name: '{1}', Type {2}", ActionChannelName, obj.Name, obj.GetType()));
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
        public virtual bool IsHelperObjectNeded()
        {
            return false;
        }
        
        
    }
}
