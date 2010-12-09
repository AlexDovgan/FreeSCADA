using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common;


namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    /// <summary>
    /// Description of Class1.
    /// </summary>
    
    class TextBoxManipulator : BaseManipulator
    {
        TextBox textEditor = new TextBox();
        TextBlock textBlock;
        public TextBoxManipulator(IDocumentView view, FrameworkElement el)
            : base(view, el)
        {
            
         
        }
        public override void Activate()
        {
            textBlock = AdornedElement as TextBlock;
            if (textBlock == null)
                throw new ArgumentException();

            textEditor.Text = textBlock.Text;
            textEditor.RenderTransform = AdornedElement.RenderTransform;
            textEditor.Focus();
            visualChildren.Add(textEditor);
            RaiseObjectChanged(new ModifyGraphicsObject(AdornedElement));
            base.Activate();
        }

        public override void Deactivate()
        {
            EditorHelper.SetDependencyProperty(textBlock,TextBlock.TextProperty , textEditor.Text);
            base.Deactivate();
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            var m = (MatrixTransform)AdornedElement.TransformToVisual(this);

            var p= m.Transform(new Point(0, 0));
            textEditor.Arrange(new Rect(p, AdornedElement.DesiredSize));
            return finalSize;
        }
        public override bool IsApplicable()
        {
            if (AdornedElement is TextBlock)
                return true;
            return false;
        }
        
    }
}
