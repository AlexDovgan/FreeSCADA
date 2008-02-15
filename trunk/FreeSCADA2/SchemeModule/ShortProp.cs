using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class ShortProp
    {
        public ShortProp(System.Windows.Shapes.Shape shp)
        {
            m_Shp = shp;
        }
        public System.Windows.Media.Brush Fill
        {
            get { return m_Shp.Fill; }
            set { m_Shp.Fill = value; }

        }
        public double Width
        {
            get { return m_Shp.Width; }
            set { m_Shp.Width = value; }

        }
        public double Height
        {
            get { return m_Shp.Height; }
            set { m_Shp.Height = value; }

        }
        /*  public System.Windows.Media.Pen Stroke
         {
              get { return m_Shp.Pen; }
             set { m_Shp.Pen = value; }

          }
          */

        System.Windows.Shapes.Shape m_Shp;
    }
}
