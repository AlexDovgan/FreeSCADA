using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeSCADA.Designer
{
    static class WindowManager
    {

        static Dictionary<String, ToolWindow> toolsWindows = new Dictionary<String, ToolWindow>();
        static List<DocumentWindow> documentWindows = new List<DocumentWindow>();
        static DocumentWindow CurrentDocument = null;

        static public void  AddToolWindow(String name,ToolWindow wnd)
        {
            if (wnd!=null)
                toolsWindows[name] = wnd;
            else throw new Exception("Can't add null refference");
                
        }
        static public ToolWindow GetToolWindow(String name)
        {
            return toolsWindows[name];
        }
        

    }
}
