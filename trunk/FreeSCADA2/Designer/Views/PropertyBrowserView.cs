﻿using System;
using FreeSCADA.Schema.ShortProperties;

namespace FreeSCADA.Designer.Views
{
    class PropertyBrowserView : ToolWindow
    {
        private System.Windows.Forms.PropertyGrid propertyGrid;

        public PropertyBrowserView()
		{
			TabText = "Property Browser";
            InitializeComponent();
		}

        private void InitializeComponent()
        {
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(292, 273);
            this.propertyGrid.TabIndex = 0;
            // 
            // PropertyBrowserView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.propertyGrid);
            this.Name = "PropertyBrowserView";
            this.ResumeLayout(false);

        }
        public void ShowProperties(object obj)
        {
            if (propertyGrid.SelectedObject is IDisposable)
                (propertyGrid.SelectedObject as IDisposable).Dispose();

            propertyGrid.SelectedObject = obj;
            if(obj is CommonShortProp)
                (obj as CommonShortProp).PropertiesChanged += new CommonShortProp.PropertiesChangedDelegate(PropertyBrowserView_PropertiesChanged);
        }

        void PropertyBrowserView_PropertiesChanged()
        {
            propertyGrid.Refresh();
        }
        /*
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(String info)
		{
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(NotifyPropertyChangedAsync), this);
			//NotifyPropertyChangedAsync(this);
		}
		protected void NotifyPropertyChangedAsync(Object info)
		{
			BaseBoxPrimitive obj = (BaseBoxPrimitive)info;
			if (obj.PropertyChanged != null)
				obj.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(""));
		}


Обработка:
		delegate void InvokeDelegate();
		void OnObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			m_PropertyGrid.BeginInvoke(new InvokeDelegate(delegate() { m_PropertyGrid.Refresh(); }));
		}

		private void OnFocusedObjectChanged(object sender, object primitive)
		{
			Object old_obj = m_PropertyGrid.SelectedObject;
			if (old_obj != null && old_obj is System.ComponentModel.INotifyPropertyChanged)
				(old_obj as System.ComponentModel.INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(OnObjectPropertyChanged);

			m_PropertyGrid.SelectedObject = primitive;

			if (primitive != null && primitive is System.ComponentModel.INotifyPropertyChanged)
				(primitive as System.ComponentModel.INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(OnObjectPropertyChanged);
		}

*/
	}

}
