using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using FreeSCADA.Common;
using FreeSCADA.Designer.Views;
namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controls
{
    public class BaseControl: Thumb
    {
        protected IDocumentView _view;
        protected FrameworkElement _controlledItem;
        public BaseControl(IDocumentView view,FrameworkElement el):base()
        {
            if (el == null) throw new ArgumentNullException();
            if (!(view.MainPanel is Canvas)) throw new ArgumentException();

            _view = view;
            _controlledItem = el;
            ThumbsResources tr = new ThumbsResources();
            tr.InitializeComponent();
            Resources = tr;
        }
        protected GridManager GridManager
        {
            get
            {
                return ((SchemaView)_view).GridManager;
            }   
        }

    }
}
 