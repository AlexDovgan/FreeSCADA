using Core.UIItems;
using NUnit.Framework;

namespace Designer.Tests
{
    class GridWrapper
    {
        IUIItem grid;
        Core.UIItems.WindowItems.Window window;

        const int maxRows = 10;
        const int maxColumns = 10;

        public GridWrapper(Core.UIItems.WindowItems.Window window, IUIItem grid)
        {
            this.grid = grid;
            this.window = window;
            Assert.IsNotNull(grid);
            Assert.IsNotNull(window);
        }

        public void MoveToCell(int rowIndex, int colIndex)
        {
            grid.Focus();
            for (int i = 0; i < maxColumns; i++)
                window.Keyboard.PressSpecialKey(Core.WindowsAPI.KeyboardInput.SpecialKeys.LEFT);
            for (int i = 0; i < maxRows; i++)
                window.Keyboard.PressSpecialKey(Core.WindowsAPI.KeyboardInput.SpecialKeys.UP);

            for(int i=0;i<rowIndex;i++)
                window.Keyboard.PressSpecialKey(Core.WindowsAPI.KeyboardInput.SpecialKeys.DOWN);
            for (int i = 0; i < colIndex; i++)
                window.Keyboard.PressSpecialKey(Core.WindowsAPI.KeyboardInput.SpecialKeys.RIGHT);
        }

        public void Select(int valueIndex)
        {
            window.Keyboard.PressSpecialKey(Core.WindowsAPI.KeyboardInput.SpecialKeys.F2);

            for (int i = 0; i < 10; i++)
                window.Keyboard.PressSpecialKey(Core.WindowsAPI.KeyboardInput.SpecialKeys.UP);

            for (int i = 0; i < valueIndex; i++)
                window.Keyboard.PressSpecialKey(Core.WindowsAPI.KeyboardInput.SpecialKeys.DOWN);
            window.Keyboard.PressSpecialKey(Core.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
        }

        public void TypeText(string text)
        {
        }
    }
}
