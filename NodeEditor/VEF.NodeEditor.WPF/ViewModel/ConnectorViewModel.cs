using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using VEF.Utils;

namespace DLL.NodeEditor.ViewModel
{
    public enum ConnectorDataType
    {

    }

    public enum ConnectorDirection
    {
        Input,
        Output
    }

    [DataContract(IsReference = true)]
    public abstract class ConnectorViewModel : ViewModelIsRefBase
    {
        public event EventHandler PositionChanged;
        
        private ElementViewModel _element;

        [DataMember]
        public ElementViewModel Element
        {
            get { return _element; }
            set { _element = value; }
        }


        private uint _portId;

        [DataMember]
        public uint PortId
        {
            get { return _portId; }
            set { _portId = value; }
        }

        private string _name;

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private Color _color = Colors.Black;
        [DataMember]
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private Point _position;
     //   [DataMember]
        public Point Position
        {
            get { return _position; }
            set
            {
                _position = value;
                RaisePropertyChanged("Position");
                RaisePositionChanged();
            }
        }

        public abstract ConnectorDirection ConnectorDirection { get; }

        public ConnectorViewModel()
        {

        }
        
        private void RaisePositionChanged()
        {
            var handler = PositionChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
