using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using VEF.Core.Util;
using VEF.Utils;

namespace DLL.NodeEditor.ViewModel
{
    [DataContract(IsReference = true)]
    public class ConnectionViewModel : ViewModelIsRefBase
    {
        private OutputConnectorViewModel _from;

        [DataMember]
        public OutputConnectorViewModel From
        {
            get
            {
                return _from;
            }
            set
            {
                if (_from != null)
                {
                    _from.PositionChanged -= OnFromPositionChanged;
                    if (_from.Connections != null)
                        _from.Connections.Remove(this);
                }

                _from = value;

                //if(!value.Connections.Contains(this)) //for deserialization
                //    value.Connections.Add(this);

                if (_from != null)
                {
                    _from.PositionChanged += OnFromPositionChanged;

                    if (_from.Connections == null)
                       _from.Connections = new ObservableCollection<ConnectionViewModel>();

                    _from.Connections.Add(this);
                    FromPosition = value.Position;
                }

                RaisePropertyChanged("From");
            }
        }

        private InputConnectorViewModel _to;

        [DataMember]
        public InputConnectorViewModel To
        {
            get
            {
                return _to;
            }
            set
            {
                if (_to != null)
                {
                    _to.PositionChanged -= OnToPositionChanged;

                    if(_to.Connection != null)
                        _to.Connection = null;
                }

                _to = value;
                
                if (_to != null)
                {
                    _to.PositionChanged += OnToPositionChanged;

                    if(_to.Connection != null)
                        _to.Connection = this; //Attention circular reference

                    ToPosition = _to.Position;
                }

                RaisePropertyChanged("To");
            }
        }

        private Point _fromPosition;

        [DataMember]
        public Point FromPosition
        {
            get { return _fromPosition; }
            set
            {
                _fromPosition = value;
                RaisePropertyChanged("FromPosition");
            }
        }

        private Point _toPosition;

        [DataMember]
        public Point ToPosition
        {
            get { return _toPosition; }
            set
            {
                _toPosition = value;
                RaisePropertyChanged("ToPosition");
            }
        }

        public ConnectionViewModel()
        {
            _from = new OutputConnectorViewModel();
            _to = new InputConnectorViewModel();
        }

        private void OnFromPositionChanged(object sender, EventArgs e)
        {
            FromPosition = From.Position;
        }

        private void OnToPositionChanged(object sender, EventArgs e)
        {
            ToPosition = To.Position;
        }
    }
}
