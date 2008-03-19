﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.Context_Menu;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using System.Windows.Documents;
using FreeSCADA.Designer.SchemaEditor.Manipulators.Controlls;


namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    /// <summary>
    /// Description of Class1.
    /// </summary>
    
    class TextBoxManipulator : BaseManipulator
    {
        RichTextBox textEditor = new RichTextBox();
        TextBlock textBlock;
        public TextBoxManipulator(UIElement element)
            : base(element)
        {

            
        }
        public override void Activate()
        {
            base.Activate();
            if (!(AdornedElement  is TextBlock)) return;
            textBlock = AdornedElement as TextBlock;
            Paragraph pargraph = new Paragraph();

            while (textBlock.Inlines.Count > 0)
            {

                pargraph.Inlines.Add(textBlock.Inlines.FirstInline);
            }
            textEditor.Document = new FlowDocument(pargraph);
            textEditor.RenderTransform = AdornedElement.RenderTransform;

            visualChildren.Add(textEditor);
    
        }

        public override void Deactivate()
        {
            base.Deactivate();
            textBlock.Inlines.Clear();
            Paragraph paragraph = textEditor.Document.Blocks.FirstBlock as Paragraph;
            while(paragraph.Inlines.Count>0)
            {
                textBlock.Inlines.Add(paragraph.Inlines.FirstInline);

            }
        }
        protected override Size ArrangeOverride(Size finalSize)
        {

            MatrixTransform m = (MatrixTransform)AdornedElement.TransformToVisual(this);

            Point p= m.Transform(new Point(0, 0));
            textEditor.Arrange(new Rect(p, AdornedElement.DesiredSize));
            return finalSize;
        }
    }
}
