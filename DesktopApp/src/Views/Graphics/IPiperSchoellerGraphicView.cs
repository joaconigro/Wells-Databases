using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Wells.View
{
    interface IPiperSchoellerGraphicView : IGraphicsView
    {
        void CreateGraphics();
        System.Windows.Media.Color ShowColorDialog(Color selectedColor);
    }
}
