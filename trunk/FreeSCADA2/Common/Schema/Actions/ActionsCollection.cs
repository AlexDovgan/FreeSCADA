using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace FreeSCADA.Common.Schema.Actions
{
    /// <summary>
    /// Actions manager class 
    /// </summary>
    [ContentProperty("ActionsList")]
    public class ActionsCollection:DependencyObject
    {
        public static Type[] ActionsTypes = 
        { 
            typeof(MoveAction), 
            typeof(RotateAction), 
        //    typeof(ShowAction),
        //  typeof(ValueAction) 
        };

        List<BaseAction> actionsList = new List<BaseAction>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<BaseAction> ActionsList
        {
            get { return actionsList; }
        }

        public static readonly DependencyProperty Actions = DependencyProperty.RegisterAttached(
            "Actions",
            typeof(ActionsCollection),
            typeof(ActionsCollection),
            new FrameworkPropertyMetadata());


        public static ActionsCollection GetActions(FrameworkElement el)
        {
            if(el.GetValue(Actions)==null)
                el.SetValue(Actions, new ActionsCollection());
            return (ActionsCollection)el.GetValue(Actions);
        }
        public static void  SetActions(FrameworkElement el,ActionsCollection ac)
        {
            el.SetValue(Actions,ac);
        }

     }
}
