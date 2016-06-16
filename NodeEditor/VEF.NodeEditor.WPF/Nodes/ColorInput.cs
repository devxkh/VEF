using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DLL.NodeEditor.Nodes
{
    public class ColorInput : DynamicNode
    {
        private Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                UpdatePreviewImage();
                RaisePropertyChanged("Color");
            }
        }

        public ColorInput()
        {
            Color = Colors.Red;
            UpdatePreviewImage();
        }

        protected override void Draw(DrawingContext drawingContext, Rect bounds)
        {
            drawingContext.DrawRectangle(new SolidColorBrush(Color), null, bounds);
        }
    }
}
