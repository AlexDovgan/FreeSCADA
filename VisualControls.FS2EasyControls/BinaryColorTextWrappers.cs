using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors;
using FreeSCADA.Interfaces;

namespace FreeSCADA.VisualControls.FS2EasyControls
{
    public class BinaryColorTextDescriptor : IVisualControlDescriptor
    {
        public static Plugin Plugin { get; set; }

        #region interface IVisualControlDescriptor

        public string Name
        {
            get { return "BinaryColorText"; }
        }

        public string PluginId
        {
            get { return Plugin.PluginId; }
        }

        public Type Type
        {
            get { return typeof(BinaryColorText); }
        }

        public ManipulatorKind ManipulatorKind
        {
            get { return ManipulatorKind.DragResizeRotateManipulator; }
        }

        public UIElement CreateControl()
        {
            return new BinaryColorText();
        }

        public object Tag
        {
            get;
            set;
        }

        public ICustomTypeDescriptor getPropProxy(object o)
        {
            return (ICustomTypeDescriptor)new BinaryColorTextPropProxy(o);
        }
        #endregion interface IVisualControlDescriptor
    }
    /// <summary>
    /// Proxy class for Property editing in the Designer. Not all properties should be visible to and edited by the user,
    /// this class is a filter and passes through the necessary properties only
    /// </summary>
    public class BinaryColorTextPropProxy : PropProxy
    {
        /// <summary>
        /// Pass the argument to base constructor
        /// </summary>
        /// <param name="controlledObject"></param>
        public BinaryColorTextPropProxy(object controlledObject)
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
            RegisterProperty(typeof(BinaryColorText), "Canvas.Top", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "Canvas.Left", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "Width", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "Height", typeof(CommonTypeEditor), result);

            RegisterProperty(typeof(BinaryColorText), "Background", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "Foreground", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "Background1", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "Foreground1", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "Text", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "Text1", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "Opacity", typeof(CommonTypeEditor), result);

            RegisterProperty(typeof(BinaryColorText), "FontSize", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "FontFamily", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "ChannelBadEdge", typeof(CommonTypeEditor), result);
            RegisterProperty(typeof(BinaryColorText), "ChannelName", typeof(ChannelSelectEditor), result);

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
            result.Add(new PropertyWrapper(base.ControlledObject, info));
        }
    }
}