using DLL.NodeEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DLL.NodeEditor.Nodes
{
    [DataContract(IsReference = true)]
    public abstract class DynamicNode : ElementViewModel
    {
        private BitmapSource _previewImage;

        public override BitmapSource PreviewImage
        {
            get { return _previewImage; }
        }

        protected DynamicNode()
        {
            SetOutputConnector("out", Colors.DarkSeaGreen, () => PreviewImage);
        }

        protected virtual void PrepareDrawingVisual(DrawingVisual drawingVisual)
        {
            
        }

        protected abstract void Draw(DrawingContext drawingContext, Rect bounds);

        protected void UpdatePreviewImage()
        {
            var dv = new DrawingVisual();
            PrepareDrawingVisual(dv);

            DrawingContext dc = dv.RenderOpen();
            Draw(dc, new Rect(0, 0, PreviewSize, PreviewSize));
            dc.Close();

            var rtb = new RenderTargetBitmap((int) PreviewSize, (int) PreviewSize, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);

            if (dv.Effect is IDisposable)
                ((IDisposable) dv.Effect).Dispose();

            _previewImage = rtb;
            RaisePropertyChanged("PreviewImage");

            RaiseOutputChanged();
        }

        protected override void OnInputConnectorConnectionChanged()
        {
            UpdatePreviewImage();
        }
    }
}
