using System;
namespace FreeSCADA.CommonUI.Interfaces
{
    public interface ITool
    {
        void Activate();
        void Deactivate();
        System.Windows.Media.GeneralTransform GetDesiredTransform(System.Windows.Media.GeneralTransform transform);
        Type GetToolManipulator();
        void NotifyObjectCreated(System.Windows.UIElement obj);
        event EventHandler ObjectCreated;
        event EventHandler ObjectDeleted;
        Type ToolEditingType();
        event EventHandler ToolFinished;
        event EventHandler ToolStarted;
        event EventHandler ToolWorking;
    }
}
