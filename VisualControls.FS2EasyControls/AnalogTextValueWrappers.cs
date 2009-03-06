using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors;
using FreeSCADA.Interfaces;
using System.Windows.Media;

namespace FreeSCADA.VisualControls.FS2EasyControls
{
    public class AnalogTextValueDescriptor : IVisualControlDescriptor
    {
        public static Plugin Plugin { get; set; }

        #region interface IVisualControlDescriptor

        public string Name
        {
            get { return "AnalogTextValue"; }
        }

        public string PluginId
        {
            get { return Plugin.PluginId; }
        }

        public Type Type
        {
            get { return typeof(AnalogTextValue); }
        }

        public ManipulatorKind ManipulatorKind
        {
            get { return ManipulatorKind.DragResizeRotateManipulator; }
        }

        public UIElement CreateControl()
        {
            return new AnalogTextValue();
        }

        public object Tag
        {
            get;
            set;
        }

        public ICustomTypeDescriptor getPropProxy(object o)
        {
            return (ICustomTypeDescriptor)new AnalogTextValuePropProxy(o);
        }
        #endregion interface IVisualControlDescriptor
    }
    /// <summary>
    /// Proxy class for Property editing in the Designer. Not all properties should be visible to and edited by the user,
    /// this class is a filter and passes through the necessary properties only
    /// </summary>
    public class AnalogTextValuePropProxy : PropProxy
    {
        /// <summary>
        /// Pass the argument to base constructor
        /// </summary>
        /// <param name="controlledObject"></param>
        public AnalogTextValuePropProxy(object controlledObject)
            : base(controlledObject)
        {
        }
        /// <summary>
        /// Add here all properties which you want edit in PropertyGrid
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<PropertyWrapper> result = new List<PropertyWrapper>();
            RegisterProperty(typeof(AnalogTextValue), "Canvas.Top", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(AnalogTextValue), "Canvas.Left", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(AnalogTextValue), "Width", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(AnalogTextValue), "Height", typeof(CommonTypeEditor), result);

            RegisterProperty(typeof(AnalogTextValue), "Background", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(AnalogTextValue), "Foreground", typeof(CommonTypeEditor), result);
            //RegisterProperty(typeof(AnalogTextValue), "BorderThickness", typeof(CommonTypeEditor), result);
            //RegisterProperty(typeof(AnalogTextValue), "BorderBrush", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(AnalogTextValue), "Opacity", typeof(CommonTypeEditor), result);
            //RegisterProperty(typeof(AnalogTextValue), "RenderTransform", null, result);
            //RegisterProperty(typeof(AnalogTextValue), "RenderTransformOrigin", null, result);

            RegisterProperty(typeof(AnalogTextValue), "DecimalPlaces", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(AnalogTextValue), "FontSize", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(AnalogTextValue), "FontFamily", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(AnalogTextValue), "ChannelBadFill", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(AnalogTextValue), "ChannelName", typeof(ChannelSelectEditor), result);
            RegisterProperty(typeof(AnalogTextValue), "Unit", typeof(ChannelSelectEditor), result);

            return new PropertyDescriptorCollection(result.ToArray());
        }
        /// <summary>
        ///  Helper method
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="sourceProperty"></param>
        /// <param name="editor"></param>
        /// <param name="result"></param>

        void RegisterProperty(Type objectType, string sourceProperty, Type editor, List<PropertyWrapper> result)
        {
            PropertyInfo info = new PropertyInfo();
            info.SourceProperty = sourceProperty;
            info.Editor = editor;
            result.Add(new PropertyWrapper(base.controlledObject, info));
        }
    }
}