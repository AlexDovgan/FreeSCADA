using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors;
using FreeSCADA.Common.Schema.Actions;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
    static class PropertiesMap
    {
        static Dictionary<Type, Dictionary<string, Type>> propertiesMap = new Dictionary<Type, Dictionary<string, Type>>();

        static PropertiesMap()
        {
            propertiesMap.Add(typeof(Object), new Dictionary<string, Type>());
            RegisterPrperties();
        }

        static public void RegisterProperty(Type type, string propName, Type editor)
        {
            if (!propertiesMap.ContainsKey(type))
                propertiesMap.Add(type, new Dictionary<string, Type>());
            propertiesMap[type].Add(propName, editor);
        }
        static public List<string> GetProperties(Type type)
        {
            List<String> l = new List<string>();
            Type bt = SearchClosestType(type);
            if (bt == typeof(Object))
                return l;
            l.AddRange(propertiesMap[bt].Keys);    
            bt = bt.BaseType;
            l.AddRange(GetProperties(bt));
            
       
            return l;
        }

        static public Type GetEditor(Type type, string propName)
        {
            do
            {
                type = SearchClosestType(type);
                if (propertiesMap[type].ContainsKey(propName))
                    return propertiesMap[type][propName];
                else
                    type = type.BaseType;
                

            } while (type != typeof(Object));
            return null;
        }
        static public Type SearchClosestType(Type type)
        {
            do
            {
                if (!propertiesMap.ContainsKey(type))
                    type = type.BaseType;
                else
                    return type;
            } while (type != typeof(FrameworkElement));
            return type;
        }
        static public void RegisterPrperties()
        {
            RegisterProperty(typeof(FrameworkElement),"Width", typeof(DoubleEditor));
            RegisterProperty(typeof(FrameworkElement),"Height", typeof(DoubleEditor));
            RegisterProperty(typeof(FrameworkElement), "Background", typeof(DoubleEditor));
            RegisterProperty(typeof(FrameworkElement), "Foreground", typeof(DoubleEditor));
            RegisterProperty(typeof(FrameworkElement), "Opacity", typeof(DoubleEditor));
            RegisterProperty(typeof(FrameworkElement), "Canvas.Top", typeof(DoubleEditor));                            
            RegisterProperty(typeof(FrameworkElement), "Canvas.Left", typeof(DoubleEditor));
            RegisterProperty(typeof(FrameworkElement), "Canvas.ZIndex", null);
            RegisterProperty(typeof(FrameworkElement), "RenderTransform", null);
            RegisterProperty(typeof(FrameworkElement), "RenderTransformOrigin", null);
            RegisterProperty(typeof(FrameworkElement), "Name", null);
            RegisterProperty(typeof(RangeBase), "Value", typeof(DoubleEditor));                            
            RegisterProperty(typeof(RangeBase), "Maximum", typeof(DoubleEditor));                                                            
            RegisterProperty(typeof(RangeBase), "Mimimum", typeof(DoubleEditor));
            RegisterProperty(typeof(RangeBase), "Orientation", null);
            RegisterProperty(typeof(ContentControl), "Content", typeof(ContentEditor));
            RegisterProperty(typeof(ContentControl), "ContentTemplate", null);
            RegisterProperty(typeof(Control), "Style", typeof(StyleEditor));                    
            RegisterProperty(typeof(Shape), "StrokeThickness", null);                    
            RegisterProperty(typeof(Shape), "Stroke", typeof(ColorEditor));                    
            RegisterProperty(typeof(Shape), "Fill", typeof(ColorEditor));
            RegisterProperty(typeof(BaseAction), "ActionChannelName", typeof(ChannelSelectEditor));
            RegisterProperty(typeof(BaseAction), "MinChannelValue", null);
            RegisterProperty(typeof(BaseAction), "MaxChannelValue", null);
            RegisterProperty(typeof(RotateAction), "MinAngle", null);
            RegisterProperty(typeof(RotateAction), "MaxAngle", null);
            RegisterProperty(typeof(TextBlock), "Text", typeof(StringEditor));
            RegisterProperty(typeof(TextBlock), "FontFamily", null);
            RegisterProperty(typeof(TextBlock), "FontSize", null);
            RegisterProperty(typeof(TextBlock), "FontStretch", null);
            RegisterProperty(typeof(TextBlock), "FontStyle", null);
            RegisterProperty(typeof(TextBlock), "FontWeight", null);
            RegisterProperty(typeof(TextBlock), "TextAlignment", null);
            RegisterProperty(typeof(CheckBox), "IsChecked", typeof(NullableBoolEditor));                                         

			
        }
        
    }
}
