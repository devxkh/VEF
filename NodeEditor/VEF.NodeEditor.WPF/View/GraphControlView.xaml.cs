using DLL.NodeEditor.Nodes;
using DLL.NodeEditor.ViewModel;
using Gemini.Modules.GraphEditor.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLL.NodeEditor.View
{
    public class MyConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // (This code has zero tolerance)
            var attributes = value.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Count() > 0)
            {
                return (attributes[0] as DisplayNameAttribute).DisplayName;
            }
            else
            {
                var prop = value.GetType().Name;//.GetProperty("Name");
                return prop.ToString();
            }

        //    return ((Enum)value).GetAttributeOfType<DisplayAttribute>().Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// Interaktionslogik für GraphControl.xaml
    /// </summary>
    public partial class GraphControlView : UserControl
    {
        private GraphViewModel m_DataContext;
        private Point _originalContentMouseDownPoint;

        public GraphControlView()
        {
            InitializeComponent();

            this.DataContext = m_DataContext = new GraphViewModel();

        }



        public ObservableCollection<ElementViewModel> ElementList
        {
            get
            {            
                return m_DataContext.Elements;// (ObservableCollection<ElementViewModel>)GetValue(ElementsProperty);
            }
            set
            {
                m_DataContext.Elements = value;
              //  SetValue(ElementsProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Pages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register("ElementList", typeof(ObservableCollection<ElementViewModel>), typeof(GraphControlView), new UIPropertyMetadata(null));


        public ObservableCollection<ConnectionViewModel> ConnectionList
        {
            get
            {
                return m_DataContext.Connections;
            }
            set
            {
                m_DataContext.Connections = value;
            }
        }

        // Using a DependencyProperty as the backing store for Pages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectionListProperty =
            DependencyProperty.Register("ConnectionList", typeof(ObservableCollection<ConnectionViewModel>), typeof(GraphControlView), new UIPropertyMetadata(null));


        private GraphViewModel ViewModel
        {
            get { return (GraphViewModel)DataContext; }
        }


        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            base.OnPreviewMouseDown(e);
        }


        private void OnGraphControlRightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            _originalContentMouseDownPoint = e.GetPosition(GraphControl);
            GraphControl.CaptureMouse();
            Mouse.OverrideCursor = Cursors.ScrollAll;
            e.Handled = true;
        }

        private void OnGraphControlRightMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = null;
            GraphControl.ReleaseMouseCapture();
            e.Handled = true;
        }

        private void OnGraphControlMouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed && GraphControl.IsMouseCaptured)
            {
                Point currentContentMousePoint = e.GetPosition(GraphControl);
                Vector dragOffset = currentContentMousePoint - _originalContentMouseDownPoint;

                ZoomAndPanControl.ContentOffsetX -= dragOffset.X;
                ZoomAndPanControl.ContentOffsetY -= dragOffset.Y;

                e.Handled = true;
            }
        }

        private void OnGraphControlMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ZoomAndPanControl.ZoomAboutPoint(
            ZoomAndPanControl.ContentScale + e.Delta / 1000.0f,
            e.GetPosition(GraphControl));

            e.Handled = true;
        }

        private void OnGraphControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.OnSelectionChanged();
        }

        private void OnGraphControlConnectionDragStarted(object sender, ConnectionDragStartedEventArgs e)
        {
            var sourceConnector = (ConnectorViewModel)e.SourceConnector.DataContext;
            var currentDragPoint = Mouse.GetPosition(GraphControl);
            var connection = ViewModel.OnConnectionDragStarted(sourceConnector, currentDragPoint);
            e.Connection = connection;
        }

        private void OnGraphControlConnectionDragging(object sender, ConnectionDraggingEventArgs e)
        {
            var currentDragPoint = Mouse.GetPosition(GraphControl);
            var connection = (ConnectionViewModel)e.Connection;
            ViewModel.OnConnectionDragging(currentDragPoint, connection);
        }

        private void OnGraphControlConnectionDragCompleted(object sender, ConnectionDragCompletedEventArgs e)
        {
            var currentDragPoint = Mouse.GetPosition(GraphControl);
            var sourceConnector = (ConnectorViewModel)e.SourceConnector.DataContext;
            var newConnection = (ConnectionViewModel)e.Connection;
            ViewModel.OnConnectionDragCompleted(currentDragPoint, newConnection, sourceConnector);
        }

        private void OnGraphControlDragEnter(object sender, DragEventArgs e)
        {
        //##    if (!e.Data.GetDataPresent(ToolboxDragDrop.DataFormat))
            //##        e.Effects = DragDropEffects.None;
        }

        private void OnGraphControlDrop(object sender, DragEventArgs e)
        {
            //todo
            //if (e.Data.GetDataPresent(ToolboxDragDrop.DataFormat))
            //{
            //    var mousePosition = e.GetPosition(GraphControl);

            //    var toolboxItem = (ToolboxItem)e.Data.GetData(ToolboxDragDrop.DataFormat);
            //    var element = (ElementViewModel)Activator.CreateInstance(toolboxItem.ItemType);
            //    element.X = mousePosition.X;
            //    element.Y = mousePosition.Y;

            //    ViewModel.Elements.Add(element);
            //}
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //m_DataContext = new GraphViewModel();
            //this.DataContext = m_DataContext;

            //var element = new Multiply();//(ElementViewModel)Activator.CreateInstance(toolboxItem.ItemType);
            //element.X = 100;// mousePosition.X;
            //element.Y = 100;// mousePosition.Y;

            //m_DataContext.Elements.Add(element);


            Console.WriteLine("GraphControl - UserControl_Loaded");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var test = this.DataContext as GraphViewModel;

            var element1 = test.AddElement<ColorInput>(100, 50);
            //  element1.Bitmap = BitmapUtility.CreateFromBytes(DesignTimeImages.Desert);

            var element2 = test.AddElement<ColorInput>(100, 300);
            element2.Color = Colors.Green;

            var element3 = test.AddElement<Multiply>(400, 250);

            test.Connections.Add(new ConnectionViewModel()
            {
                From = element1.OutputConnector,
                To = element3.InputConnectors[0]
            });

            test.Connections.Add(new ConnectionViewModel()
            {
                From = element2.OutputConnector,
                To = element3.InputConnectors[1]
            });
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                ((GraphViewModel)DataContext).DeleteSelectedElements();
            base.OnKeyDown(e);
        }
    }
}
