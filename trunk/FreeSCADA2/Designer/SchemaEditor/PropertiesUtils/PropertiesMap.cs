using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using FreeSCADA.Common.Schema.Actions;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
    /// <summary>
    /// 
    /// </summary>
	public class PropertyInfo
	{
        /// <summary>
        /// 
        /// </summary>
		public string SourceProperty = "";
        /// <summary>
        /// 
        /// </summary>
		public string TargetProperty = "";
        /// <summary>
        /// 
        /// </summary>
        public Type TargetType=null;
        /// <summary>
        /// 
        /// </summary>
		public string DisplayName = "";
        /// <summary>
        /// 
        /// </summary>
		public string Description = "";
        /// <summary>
        /// 
        /// </summary>
		public string Group = "";
        /// <summary>
        /// 
        /// </summary>
        public Type Editor;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public string GetTargetPropertyName()
		{
			if (string.IsNullOrEmpty(TargetProperty))
				return SourceProperty;
			else
				return TargetProperty;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public string GetTargetPropertyDisplayName()
		{
			if (string.IsNullOrEmpty(DisplayName))
			{
				if (string.IsNullOrEmpty(TargetProperty))
					return SourceProperty;
				else
					return TargetProperty;
			}
			else
				return DisplayName;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return GetTargetPropertyDisplayName();
		}
	}

	static class PropertiesMap
	{
		static Dictionary<Type, List<PropertyInfo>> propertiesMap = new Dictionary<Type, List<PropertyInfo>>();

		static PropertiesMap()
		{
			RegisterPrperties();
		}

		static public void RegisterProperty(Type objectType, string sourceProperty, string targetProperty, Type editor, string description, string group)
		{
			PropertyInfo info = new PropertyInfo();
			info.SourceProperty = sourceProperty;
			info.TargetProperty = targetProperty;
			info.Editor = editor;
			info.Description = description;
			info.Group = group;
			RegisterProperty(objectType, info);
		}

		static public void RegisterProperty(Type objectType, string sourceProperty, Type editor)
		{
			PropertyInfo info = new PropertyInfo();
			info.SourceProperty = sourceProperty;
			info.Editor = editor;
			RegisterProperty(objectType, info);
		}

		static public void RegisterProperty(Type objectType, string sourceProperty, string targetProperty, Type editor, string description)
		{
			PropertyInfo info = new PropertyInfo();
			info.SourceProperty = sourceProperty;
			info.TargetProperty = targetProperty;
			info.Editor = editor;
			info.Description = description;
			RegisterProperty(objectType, info);
		}
        static public void RegisterProperty(Type objectType, string sourceProperty, string targetProperty, Type editor, string description, Type targetType)
        {

            PropertyInfo info = new PropertyInfo();
            info.SourceProperty = sourceProperty;
            info.TargetProperty = targetProperty;
            info.TargetType = targetType;
            info.Editor = editor;
            info.Description = description;
            RegisterProperty(objectType, info);
        }

		static public void RegisterProperty(Type objectType, string sourceProperty)
		{
			PropertyInfo info = new PropertyInfo();
			info.SourceProperty = sourceProperty;
			RegisterProperty(objectType, info);
		}

		static public void RegisterProperty(Type objectType, PropertyInfo propertyInfo)
		{
			if (propertiesMap.ContainsKey(objectType) == false)
				propertiesMap.Add(objectType, new List<PropertyInfo>());
			propertiesMap[objectType].Add(propertyInfo);
		}

		static public List<PropertyInfo> GetProperties(Type type)
		{
			List<PropertyInfo> result = new List<PropertyInfo>();
			do
			{
				if (propertiesMap.ContainsKey(type))
					result.AddRange(propertiesMap[type]);
				type = type.BaseType;
			} while (type != null);

			return result;
		}

		static public void RegisterPrperties()
		{
			//Check duplicating properties
			//RegisterProperty(typeof(FrameworkElement), "Width", "Width1", typeof(DoubleEditor), "Description for width1 property");
			//RegisterProperty(typeof(FrameworkElement), "Width", "Width2", typeof(DoubleEditor), "Description for width2 property");

			RegisterProperty(typeof(FrameworkElement), "Width", typeof(CommonTypeEditor));
            RegisterProperty(typeof(FrameworkElement), "Height", typeof(CommonTypeEditor));
            RegisterProperty(typeof(FrameworkElement), "Background", typeof(CommonTypeEditor));
            RegisterProperty(typeof(FrameworkElement), "Foreground", typeof(CommonTypeEditor));
            RegisterProperty(typeof(FrameworkElement), "Opacity", typeof(CommonTypeEditor));
            RegisterProperty(typeof(FrameworkElement), "Canvas.Top", typeof(CommonTypeEditor));
            RegisterProperty(typeof(FrameworkElement), "Canvas.Left", typeof(CommonTypeEditor));
			RegisterProperty(typeof(FrameworkElement), "Canvas.ZIndex", null);
			RegisterProperty(typeof(FrameworkElement), "RenderTransform", null);
			RegisterProperty(typeof(FrameworkElement), "RenderTransformOrigin", null);
			RegisterProperty(typeof(FrameworkElement), "Name", null);
            RegisterProperty(typeof(RangeBase), "Value", typeof(CommonTypeEditor));
            RegisterProperty(typeof(RangeBase), "Maximum", typeof(CommonTypeEditor));
            RegisterProperty(typeof(RangeBase), "Minimum", typeof(CommonTypeEditor));
			RegisterProperty(typeof(RangeBase), "Orientation", null);
			RegisterProperty(typeof(System.Windows.Controls.Primitives.ButtonBase), "Content","Image",  typeof(ImageEditor),"Image" );
            RegisterProperty(typeof(System.Windows.Controls.Primitives.ButtonBase), "Content", "Text", typeof(CommonTypeEditor), "Text",typeof(String));
            RegisterProperty(typeof(FreeSCADA.Common.Schema.AnimatedImage), "ImageName", "Image", typeof(ImageEditor), "Image");
            RegisterProperty(typeof(FreeSCADA.Common.Schema.AnimatedImage), "AnimatedControl", "AnimatedControl", typeof(CommonTypeEditor), "Animation controling");
			RegisterProperty(typeof(Control), "Style", typeof(StyleEditor));
			RegisterProperty(typeof(Shape), "StrokeThickness", null);
			RegisterProperty(typeof(Shape), "Stroke", typeof(ColorEditor));
			RegisterProperty(typeof(Shape), "Fill", typeof(ColorEditor));
			RegisterProperty(typeof(BaseAction), "ActionChannelName", typeof(ChannelSelectEditor));
			RegisterProperty(typeof(BaseAction), "MinChannelValue", null);
			RegisterProperty(typeof(BaseAction), "MaxChannelValue", null);
			RegisterProperty(typeof(RotateAction), "MinAngle", null);
			RegisterProperty(typeof(RotateAction), "MaxAngle", null);
            RegisterProperty(typeof(TextBlock), "Text", typeof(CommonTypeEditor));
			RegisterProperty(typeof(TextBlock), "FontFamily", null);
			RegisterProperty(typeof(TextBlock), "FontSize", null);
			RegisterProperty(typeof(TextBlock), "FontStretch", null);
			RegisterProperty(typeof(TextBlock), "FontStyle", null);
			RegisterProperty(typeof(TextBlock), "FontWeight", null);
			RegisterProperty(typeof(TextBlock), "TextAlignment", null);
            RegisterProperty(typeof(CheckBox), "IsChecked", typeof(CommonTypeEditor));
            RegisterProperty(typeof(Canvas), "GridManager.GridOn", "GridOn",null,"Grid snaping on/off");
            RegisterProperty(typeof(Canvas), "GridManager.GridDelta","GridDelta", null,"Grid step");
            RegisterProperty(typeof(Canvas), "GridManager.ShowGrid","ShowGrid" ,null,"Grid showing on/off");
		}

	}
}
