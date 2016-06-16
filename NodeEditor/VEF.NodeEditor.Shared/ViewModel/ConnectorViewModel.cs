using System;
using System.Collections.Generic;
using System.Linq;
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

    public abstract class ConnectorViewModel : ViewModelBase
    {
        public event EventHandler PositionChanged;

        private ElementViewModel _element;
        [XmlIgnore]
        public ElementViewModel Element
        {
            get { return _element; }
            set { _element = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private Color _color = Colors.Black;
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private Point _position;
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

        public ConnectorViewModel() { }

        protected ConnectorViewModel(ElementViewModel element, string name, Color color)
        {
            _element = element;
            _name = name;
            _color = color;
        }

        private void RaisePositionChanged()
        {
            var handler = PositionChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
