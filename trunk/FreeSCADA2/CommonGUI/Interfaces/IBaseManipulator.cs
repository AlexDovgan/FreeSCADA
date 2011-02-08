using System;
namespace FreeSCADA.CommonUI.Interfaces
{
    public  interface IManipulator
    {
        void Activate();
        void Deactivate();
        System.Windows.Media.GeneralTransform GetDesiredTransform(System.Windows.Media.GeneralTransform transform);
        bool IsApplicable();
        event FreeSCADA.CommonUI.Interfaces.ObjectChanged ObjectChangedEvent;
    }
}
