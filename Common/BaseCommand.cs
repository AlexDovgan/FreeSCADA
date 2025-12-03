using System;
using System.Drawing;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Common
{
    /// <summary>
    /// 基础命令类，实现了 ICommand 接口的大部分通用逻辑。
    /// 开发者可继承此类并重写必要属性或 Execute 方法。
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        bool canExecute = false;
        int priority = -1;

        public BaseCommand()
        {
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 命令名称（子类应覆盖）
        /// </summary>
        public virtual string Name { get { throw new NotImplementedException(); } }

        /// <summary>
        /// 命令描述（子类应覆盖）
        /// </summary>
        public virtual string Description { get { throw new NotImplementedException(); } }

        /// <summary>
        /// 图标（子类可覆盖返回具体图标）
        /// </summary>
        public virtual Bitmap Icon { get { return null; } }

        /// <summary>
        /// 命令类型（默认 Standard）
        /// </summary>
        public virtual CommandType Type { get { return CommandType.Standard; } }

        /// <summary>
        /// 下拉项（若命令为下拉菜单类型，子类应返回相应项）
        /// </summary>
        public virtual ICommandItems DropDownItems { get { return null; } }

        /// <summary>
        /// 命令优先级，值越大优先级越高（默认 -1）
        /// </summary>
        public int Priority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
            }
        }

        /// <summary>
        /// 表示命令是否可执行。发生变化时会触发 CanExecuteChanged 事件。
        /// </summary>
        public bool CanExecute
        {
            get { return canExecute; }
            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    FireCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// 执行命令（子类重写具体实现）
        /// </summary>
        public virtual void Execute()
        {
        }

        /// <summary>
        /// 触发 CanExecuteChanged 事件的保护方法
        /// </summary>
        protected void FireCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}