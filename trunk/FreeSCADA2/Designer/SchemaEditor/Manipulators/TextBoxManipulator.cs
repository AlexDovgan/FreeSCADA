using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common.Schema;


namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    /// <summary>
    /// Description of Class1.
    /// </summary>
    
    class TextBoxManipulator : BaseManipulator
    {
        TextBox textEditor = new TextBox();
        TextBlock textBlock;
        public TextBoxManipulator(UIElement el)
            : base(el)
        {
            
            textBlock = AdornedElement as TextBlock;
            if (textBlock == null)
                throw new ArgumentException();
        }
        public override void Activate()
        {
            textEditor.Text = textBlock.Text;
            textEditor.RenderTransform = AdornedElement.RenderTransform;
            textEditor.Focus();
            visualChildren.Add(textEditor);
            var ub = UndoRedoManager.GetUndoBufferFor(AdornedElement);
            ub.AddCommand(new ModifyGraphicsObject(AdornedElement));
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
        public override bool IsSelactable(UIElement el)
        {
            if (el is TextBlock)
                return true;
            return false;
        }
    }
}
