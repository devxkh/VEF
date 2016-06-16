using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DLL.NodeEditor.Nodes
{
    public class Multiply : DynamicNode
    {
        //protected override Effect GetEffect()
        //{
        //    return new MultiplyEffect
        //    {
        //        Input1 = new ImageBrush(InputConnectors[0].Value),
        //        Input2 = new ImageBrush(InputConnectors[1].Value)
        //    };
        //}

        protected override void PrepareDrawingVisual(DrawingVisual drawingVisual)
        {
          //  drawingVisual.Effect = GetEffect();
        }

        protected override void Draw(DrawingContext drawingContext, Rect bounds)
        {
            drawingContext.DrawRectangle(
                new SolidColorBrush(Colors.Transparent), null,
                bounds);
        }


        public Multiply()
        {
            AddInputConnector("Left", Colors.DarkSeaGreen);
            AddInputConnector("Right", Colors.DarkSeaGreen);
        }
    }
}
