﻿using System.Windows.Controls.Primitives;


namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controls
{
    /// <summary>
    /// Drag controll for DragResizeRotateManipulator
    /// </summary>
    class PointDragThumb: Thumb
    {

        
        public PointDragThumb()
        {

            ThumbsResources tr = new ThumbsResources();
            tr.InitializeComponent();
            Resources = tr;
                       
        }
      
    }
}


