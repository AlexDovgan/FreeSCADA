using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;

namespace FreeSCADA.Common
{
    public class ReadOnlyPropertyGrid : PropertyGrid
    {
        private bool isReadOnly;

        public bool ReadOnly
        {
            get { return this.isReadOnly; }
            set
            {
                this.isReadOnly = value;
                this.SetBrowsablePropertiesAsReadOnly(this.SelectedObject, value);
            }
        }

        protected override void OnSelectedObjectsChanged(EventArgs e)
        {
            this.SetBrowsablePropertiesAsReadOnly(this.SelectedObject, this.isReadOnly);
            base.OnSelectedObjectsChanged(e);
        }

        public ReadOnlyPropertyGrid()
            : base()
        {
        }

        /// <summary>
        /// Chnages the state of the ReadOnly attribute regarding the isReadOnly flag value.
        /// </summary>
        /// <param name="selectedObject">The current object exposed by the PropertyGrid.</param>
        /// <param name="isReadOnly">The current read-only state of the PropertyGrid.</param>
        private void SetBrowsablePropertiesAsReadOnly(object selectedObject, bool isReadOnly)
        {
            if (selectedObject != null)
            {
                // Get all the properties of the selected object...
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(selectedObject.GetType());
                foreach (PropertyDescriptor propDescript in props)
                {
                    // Consider only the properties which are browsable and are not collections...
                    if (propDescript.IsBrowsable && propDescript.PropertyType.GetInterface("ICollection", true) == null)
                    {
                        ReadOnlyAttribute attr = propDescript.Attributes[typeof(ReadOnlyAttribute)] as ReadOnlyAttribute;
                        // If the current property has a ReadOnly attribute,
                        // update its state regarding the current ReadOnly state of the PropertyGrid.
                        if (attr != null)
                        {
                            FieldInfo field = attr.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                            field.SetValue(attr, isReadOnly, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                        }
                    }
                }
            }
        }
    }
}
