//-----------------------------------------------------------------------
// <copyright file="Pan.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FreeSCADA.Common.Gestures
{

    /// <summary>
    /// This class provides the ability to pan the target object when dragging the mouse 
    /// </summary>
    public class Pan:BaseTool
    {

        bool _dragging;
        FrameworkElement _target;
        MapZoom _zoom;
        bool _captured;
        FrameworkElement _container;
        Point _mouseDownPoint;
        Point _startTranslate;
        ModifierKeys _mods = ModifierKeys.None;

        /// <summary>
        /// Construct new Pan gesture object.
        /// </summary>
        /// <param name="target">The target to be panned, must live inside a container Panel</param>
        /// <param name="zoom"></param>
        public Pan(IDocumentView view, MapZoom zoom):base(view)
        {
            this._target = view.MainPanel;
            this._container = _target.Parent as FrameworkElement;
            if (this._container == null) {
                // todo: localization
                throw new ArgumentException("Target object must live in a Panel");
            }
            this._zoom = zoom;
         
        }

        /// <summary>
        /// Handle mouse left button event on container by recording that position and setting
        /// a flag that we've received mouse left down.
        /// </summary>
        /// <param name="sender">Container</param>
        /// <param name="e">Mouse information</param>
        protected override void  OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
 	        base.OnPreviewMouseLeftButtonDown(e);

            ModifierKeys mask = Keyboard.Modifiers & _mods;
            if (!e.Handled && mask == _mods && mask == Keyboard.Modifiers)
            {
                Cursor = Cursors.Hand;
                _mouseDownPoint = e.GetPosition(this._target);
                Point offset = _zoom.Offset;
                _startTranslate = new Point(offset.X, offset.Y);
                _dragging = true;
            }
        }

        /// <summary>
        /// Handle the mouse move event and this is where we capture the mouse.  We don't want
        /// to actually start panning on mouse down.  We want to be sure the user starts dragging
        /// first.
        /// </summary>
        /// <param name="sender">Mouse</param>
        /// <param name="e">Move information</param>
        protected override void  OnPreviewMouseMove(MouseEventArgs e)
        {
 	
            base.OnPreviewMouseMove(e);

            if (this._dragging) {
                if (!_captured) {
                    _captured = true;
                    Cursor = Cursors.Hand;
                    Mouse.Capture(this, CaptureMode.SubTree);
                }
                this.MoveBy(_mouseDownPoint - e.GetPosition(this._target));
            }
        }

        /// <summary>
        /// Handle the mouse left button up event and stop any panning.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void  OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
 	        base.OnPreviewMouseLeftButtonUp(e);
            if (_captured) {
                Mouse.Capture(this, CaptureMode.None);
                e.Handled = true;
                Cursor = Cursors.Arrow; ;
                _captured = false;   
            }

            _dragging = false;
        }

        /// <summary>
        /// Move the target object by the given delta delative to the start scroll position we recorded in mouse down event.
        /// </summary>
        /// <param name="v">A vector containing the delta from recorded mouse down position and current mouse position</param>
        public void MoveBy(Vector v) {
            _zoom.Offset = new Point(-_startTranslate.X - v.X, -_startTranslate.Y - v.Y);
            _target.InvalidateVisual();
        }
        public override Type GetToolManipulator()
        {
            throw new NotImplementedException();
        }
    }
}
