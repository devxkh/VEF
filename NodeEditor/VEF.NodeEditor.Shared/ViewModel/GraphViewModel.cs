using DLL.NodeEditor.Nodes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VEF.Utils;

namespace DLL.NodeEditor.ViewModel
{
    //[Export(typeof(GraphViewModel))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class GraphViewModel : ViewModelBase
    {
     //   private readonly IInspectorTool _inspectorTool;

        private ObservableCollection<ElementViewModel> _elements;
        public ObservableCollection<ElementViewModel> Elements
        {
            get { return _elements; }
            set { _elements = value; RaisePropertyChanged("Elements"); }
        }

        private ObservableCollection<ConnectionViewModel> _connections;
        public ObservableCollection<ConnectionViewModel> Connections
        {
            get { return _connections; }
            set { _connections = value; RaisePropertyChanged("Connections"); }
        }

        public IEnumerable<ElementViewModel> SelectedElements
        {
            get { return _elements.Where(x => x.IsSelected); }
        }

   //     [ImportingConstructor]
        public GraphViewModel()//IInspectorTool inspectorTool)
        {
          //  DisplayName = "[New Graph]";

            _elements = new ObservableCollection<ElementViewModel>();
            _connections = new ObservableCollection<ConnectionViewModel>();

            return;

          //  _inspectorTool = inspectorTool;

            var element1 = AddElement<ColorInput>(100, 50);
          //  element1.Bitmap = BitmapUtility.CreateFromBytes(DesignTimeImages.Desert);

            var element2 = AddElement<ColorInput>(100, 300);
            element2.Color = Colors.Green;

            var element3 = AddElement<Multiply>(400, 250);

            Connections.Add(new ConnectionViewModel()
            {
                From =
                element1.OutputConnector,
                To = element3.InputConnectors[0]
            });

            Connections.Add(new ConnectionViewModel()
            {
                From = element2.OutputConnector,
                To = element3.InputConnectors[1]
            });

       //     element1.IsSelected = true;
        }

        public TElement AddElement<TElement>(double x, double y)
            where TElement : ElementViewModel, new()
        {
            var element = new TElement { X = x, Y = y };
            _elements.Add(element);
            return element;
        }

        public ConnectionViewModel OnConnectionDragStarted(ConnectorViewModel sourceConnector, Point currentDragPoint)
        {
            if (!(sourceConnector is OutputConnectorViewModel))
                return null;

            var connection = new ConnectionViewModel() 
            {
                From = (OutputConnectorViewModel)sourceConnector,
                ToPosition = currentDragPoint
            };

            Connections.Add(connection);

            return connection;
        }

        public void OnConnectionDragging(Point currentDragPoint, ConnectionViewModel connection)
        {
            // If current drag point is close to an input connector, show its snapped position.
            var nearbyConnector = FindNearbyInputConnector(currentDragPoint);
            connection.ToPosition = (nearbyConnector != null)
                ? nearbyConnector.Position
                : currentDragPoint;
        }

        public void OnConnectionDragCompleted(Point currentDragPoint, ConnectionViewModel newConnection, ConnectorViewModel sourceConnector)
        {
            var nearbyConnector = FindNearbyInputConnector(currentDragPoint);

            if (nearbyConnector == null || sourceConnector.Element == nearbyConnector.Element)
            {
                Connections.Remove(newConnection);
                return;
            }

            var existingConnection = nearbyConnector.Connection;
            if (existingConnection != null)
                Connections.Remove(existingConnection);

            newConnection.To = nearbyConnector;
        }

        private InputConnectorViewModel FindNearbyInputConnector(Point mousePosition)
        {
            return Elements.SelectMany(x => x.InputConnectors)
                .FirstOrDefault(x => AreClose(x.Position, mousePosition, 10));
        }

        private static bool AreClose(Point point1, Point point2, double distance)
        {
            return (point1 - point2).Length < distance;
        }

        public void DeleteElement(ElementViewModel element)
        {
         //todo   Connections.re.RemoveRange(element.AttachedConnections);
            Elements.Remove(element);
        }

        public void DeleteSelectedElements()
        {
            Elements.Where(x => x.IsSelected)
                .ToList()
                .ForEach(DeleteElement);
        }

        public void OnSelectionChanged()
        {
            var selectedElements = SelectedElements.ToList();

            //if (selectedElements.Count == 1)
            //    _inspectorTool.SelectedObject = new InspectableObjectBuilder()
            //        .WithObjectProperties(selectedElements[0], x => true)
            //        .ToInspectableObject();
            //else
            //    _inspectorTool.SelectedObject = null;
        }
    }
}
