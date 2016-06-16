using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using VEF.Utils;

namespace DLL.NodeEditor.ViewModel
{
    public abstract class ElementViewModel : ViewModelBase
    {
        public event EventHandler OutputChanged;

        public const double PreviewSize = 100;

        private double _x;

        [Browsable(false)]
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

        private readonly ObservableCollection<InputConnectorViewModel> _inputConnectors;
        public IList<InputConnectorViewModel> InputConnectors
        {
            get {
                return _inputConnectors;
            }
        }

        private OutputConnectorViewModel _outputConnector;

        [XmlIgnore]
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
            var inputConnector = new InputConnectorViewModel(this, name, color);
            inputConnector.SourceChanged += (sender, e) => OnInputConnectorConnectionChanged();
            _inputConnectors.Add(inputConnector);
        }

        protected void SetOutputConnector(string name, Color color, Func<BitmapSource> valueCallback)
        {
            OutputConnector = new OutputConnectorViewModel(this, name, color, valueCallback);
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
