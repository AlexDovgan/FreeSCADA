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
	class PropertyInfo
	{
		public string SourceProperty;
		public string TargetProperty;
		public string DisplayName;
		public string Description;
		public string Group;
		public Type Editor;

		public string GetTargetPropertyName()
		{
			if (string.IsNullOrEmpty(TargetProperty))
				return SourceProperty;
			else
				return TargetProperty;
		}

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

			RegisterProperty(typeof(FrameworkElement), "Width", typeof(DoubleEditor));
			RegisterProperty(typeof(FrameworkElement), "Height", typeof(DoubleEditor));
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
			RegisterProperty(typeof(RangeBase), "Minimum", typeof(DoubleEditor));
			RegisterProperty(typeof(RangeBase), "Orientation", null);
			RegisterProperty(typeof(Button), "Content","Image",  typeof(ImageEditor),"Image" );
            RegisterProperty(typeof(Button), "Content", "Text", typeof(StringEditor), "Text");
            RegisterProperty(typeof(FreeSCADA.Common.Schema.AnimatedImage), "ImageName", "Image", typeof(ImageEditor), "Image");
            RegisterProperty(typeof(FreeSCADA.Common.Schema.AnimatedImage), "AnimatedControl", "AnimatedControl", typeof(NullableBoolEditor), "Animation controling");
			//RegisterProperty(typeof(ContentControl), "ContentTemplate", null);
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
