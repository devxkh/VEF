using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DLL.NodeEditor.ViewModel
{
    public class InputConnectorViewModel : ConnectorViewModel
    {
        public event EventHandler SourceChanged;

        public override ConnectorDirection ConnectorDirection
        {
            get { return ConnectorDirection.Input; }
        }

        private ConnectionViewModel _connection;
        public ConnectionViewModel Connection
        {
            get { return _connection; }
            set
            {
                if (_connection != null)
                    _connection.From.Element.OutputChanged -= OnSourceElementOutputChanged;
                _connection = value;
                if (_connection != null)
                    _connection.From.Element.OutputChanged += OnSourceElementOutputChanged;
                RaiseSourceChanged();
                RaisePropertyChanged("Connection");
            }
        }

        private void OnSourceElementOutputChanged(object sender, EventArgs e)
        {
            RaiseSourceChanged();
        }

        public BitmapSource Value
        {
            get
            {
                if (Connection == null || Connection.From == null)
                    return null;

                return Connection.From.Value;
            }
        }

        public InputConnectorViewModel() { }

        public InputConnectorViewModel(ElementViewModel element, string name, Color color)
            : base(element, name, color)
        {

        }

        private void RaiseSourceChanged()
        {
            var handler = SourceChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
