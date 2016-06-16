using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace DLL.NodeEditor.ViewModel
{
    public class OutputConnectorViewModel : ConnectorViewModel
    {
        private readonly Func<BitmapSource> _valueCallback;

        [XmlIgnore]
        public override ConnectorDirection ConnectorDirection
        {
            get { return ConnectorDirection.Output; }
        }

        private ObservableCollection<ConnectionViewModel> _connections;

        [XmlIgnore]
        public ObservableCollection<ConnectionViewModel> Connections
        {
            get { return _connections; }
            set { _connections = value; }
        }

        [XmlIgnore]
        public BitmapSource Value
        {
            get { return _valueCallback(); }
        }

        public OutputConnectorViewModel() { }

        public OutputConnectorViewModel(ElementViewModel element, string name, Color color, Func<BitmapSource> valueCallback)
            : base(element, name, color)
        {
            _connections = new ObservableCollection<ConnectionViewModel>();
            _valueCallback = valueCallback;
        }
    }
}
