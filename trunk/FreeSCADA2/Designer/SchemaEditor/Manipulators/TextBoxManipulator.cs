using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


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

            
        }
        public override void Activate()
        {
         
            
            textBlock = AdornedElement as TextBlock;
           /* Paragraph pargraph = new Paragraph();

            while (textBlock.Inlines.Count > 0)
            {

                pargraph.Inlines.Add(textBlock.Inlines.FirstInline);
            }
            textEditor.Document = new FlowDocument(pargraph);*/
            textEditor.Text = textBlock.Text;
            textEditor.RenderTransform = AdornedElement.RenderTransform;
            textEditor.Focus();
            visualChildren.Add(textEditor);
            base.Activate();
        }

        public override void Deactivate()
        {
            
            /*textBlock.Inlines.Clear();
            Paragraph paragraph = textEditor.Document.Blocks.FirstBlock as Paragraph;
            while(paragraph.Inlines.Count>0)
            {
                textBlock.Inlines.Add(paragraph.Inlines.FirstInline);

            }*/
            textBlock.Text = textEditor.Text;
            base.Deactivate();
        }
        protected override Size ArrangeOverride(Size finalSize)
        {

            MatrixTransform m = (MatrixTransform)AdornedElement.TransformToVisual(this);

            Point p= m.Transform(new Point(0, 0));
            textEditor.Arrange(new Rect(p, AdornedElement.DesiredSize));
            return finalSize;
        }
        public override bool IsSelactable(UIElement el)
        {
            if (el is TextBlock)
                return true;
            else return false;
        }
    }
}
