using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using VEF.Core.Util;

namespace DLL.NodeEditor.ViewModel
{
    [DataContract(IsReference = true)]
    public class OutputConnectorViewModel : ConnectorViewModel
    {
        private Func<BitmapSource> _valueCallback;
        
        public override ConnectorDirection ConnectorDirection
        {
            get { return ConnectorDirection.Output; }
        }

        private ObservableCollection<ConnectionViewModel> _connections;

        [DataMember]
        public ObservableCollection<ConnectionViewModel> Connections
        {
            get {
                return _connections;
            }
            set {
                _connections = value;
            }
        }

    //    [DataMember] //??? circular??
        public Func<BitmapSource> Value
        {
            get { return _valueCallback; }
            set { _valueCallback = value; }
        }
        //public BitmapSource Value
        //{
        //    get { return _valueCallback(); }
        //}

        //public void SetValueCallback(Func<BitmapSource> valueCallback)
        //{
        //    _valueCallback = valueCallback;
        //}

        public OutputConnectorViewModel()
        {
            _connections = new ObservableCollection<ConnectionViewModel>();
            //      _connections = new ObservableCollection<ConnectionViewModel>();
        }

        //public OutputConnectorViewModel(ElementViewModel element, string name, Color color, Func<BitmapSource> valueCallback)
        //    : base(element, name, color)
        //{
        //    _connections = new ObservableCollection<ConnectionViewModel>();
        //    _valueCallback = valueCallback;
        //}
    }
}
