using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using VEF.Utils;

namespace DLL.NodeEditor.ViewModel
{
    [DataContract(IsReference = true)]
    public abstract class ElementViewModel : ViewModelIsRefBase 
    {
        public event EventHandler OutputChanged;

        public const double PreviewSize = 100;

        private double _x;

        [Browsable(false)]
        [DataMember]
        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                RaisePropertyChanged("X");
            }
        }

        private double _y;

        [Browsable(false)]
        [DataMember]
        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                RaisePropertyChanged("Y");
            }
        }

        private string _name;

        [Browsable(false)]
        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private bool _isSelected;

        [Browsable(false)]
        [DataMember]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public abstract BitmapSource PreviewImage { get; }

        private ObservableCollection<InputConnectorViewModel> _inputConnectors;

        [DataMember]
        public ObservableCollection<InputConnectorViewModel> InputConnectors
        {
            get {
                return _inputConnectors;
            }
            set
            {
                _inputConnectors = value;
            }
        }

        private OutputConnectorViewModel _outputConnector;

        [DataMember]
        public OutputConnectorViewModel OutputConnector
        {
            get { return _outputConnector; }
            set
            {
                _outputConnector = value;
                RaisePropertyChanged("OutputConnector");
            }
        }

        public IEnumerable<ConnectionViewModel> AttachedConnections
        {
            get
            {
                return _inputConnectors.Select(x => x.Connection)
                    .Union(_outputConnector.Connections)
                    .Where(x => x != null);
            }
        }

        public ElementViewModel()
        {
            _inputConnectors = new ObservableCollection<InputConnectorViewModel>();
            _name = GetType().Name;
        }

        protected void AddInputConnector(string name, Color color)
        {
            var inputConnector = new InputConnectorViewModel() { PortId = (uint)(_inputConnectors.Count + 1), Element = this, Name = name, Color = color };
            inputConnector.SourceChanged += (sender, e) => OnInputConnectorConnectionChanged();
            _inputConnectors.Add(inputConnector);
        }

        protected void SetOutputConnector(string name, Color color, Func<BitmapSource> valueCallback)
        {
            OutputConnector = new OutputConnectorViewModel() { Element = this, Name = name, Color = color, Value = valueCallback };
        }

        protected virtual void OnInputConnectorConnectionChanged()
        {

        }

        protected virtual void RaiseOutputChanged()
        {
            EventHandler handler = OutputChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
