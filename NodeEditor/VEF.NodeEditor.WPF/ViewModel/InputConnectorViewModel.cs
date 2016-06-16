using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace DLL.NodeEditor.ViewModel
{

    [DataContract(IsReference = true)]
    public class InputConnectorViewModel : ConnectorViewModel
    {
        public event EventHandler SourceChanged;

        public override ConnectorDirection ConnectorDirection
        {
            get { return ConnectorDirection.Input; }
        }


        private ConnectionViewModel _connection;

        [DataMember]
        public ConnectionViewModel Connection
        {
            get {
                if (_connection == null)
                {

                }

                return _connection;
            }
            set
            {
                if (_connection != null)
                    _connection.From.Element.OutputChanged -= OnSourceElementOutputChanged;
                _connection = value;
                if (_connection != null)
                    _connection.From.Element.OutputChanged += OnSourceElementOutputChanged;
                RaiseSourceChanged();
                RaisePropertyChanged("Connection");

                if(_connection == null)
                {

                }
            }
        }

        private void OnSourceElementOutputChanged(object sender, EventArgs e)
        {
            RaiseSourceChanged();
        }


        //public BitmapSource Value
        //{
        //    get
        //    {
        //        if (Connection == null || Connection.From == null)
        //            return null;

        //        return Connection.From.Value();
        //    }
        //}

        public InputConnectorViewModel()
        {
         //   _connection = new ConnectionViewModel(); // don't do it -> else stack overflow!
        }
        

        private void RaiseSourceChanged()
        {
            var handler = SourceChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
