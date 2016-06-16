using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VEF.Utils;

namespace DLL.NodeEditor.ViewModel
{
    public class ConnectionViewModel : ViewModelBase
    {
        private OutputConnectorViewModel _from;
        public OutputConnectorViewModel From
        {
            get { return _from; }
            set
            {
                if (_from != null)
                {
                    _from.PositionChanged -= OnFromPositionChanged;
                    _from.Connections.Remove(this);
                }

                _from = value;

                if (_from != null)
                {
                    _from.PositionChanged += OnFromPositionChanged;
                    _from.Connections.Add(this);
                    FromPosition = value.Position;
                }

                RaisePropertyChanged("From"); 
            }
        }

        private InputConnectorViewModel _to;
        public InputConnectorViewModel To
        {
            get { return _to; }
            set
            {
                if (_to != null)
                {
                    _to.PositionChanged -= OnToPositionChanged;
                    _to.Connection = null;
                }

                _to = value;

                if (_to != null)
                {
                    _to.PositionChanged += OnToPositionChanged;
                    _to.Connection = this;
                    ToPosition = _to.Position;
                }

                RaisePropertyChanged("To"); 
            }
        }

        private Point _fromPosition;
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
        public Point ToPosition
        {
            get { return _toPosition; }
            set
            {
                _toPosition = value;
                RaisePropertyChanged("ToPosition"); 
            }
        }

        public ConnectionViewModel()  { }

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
