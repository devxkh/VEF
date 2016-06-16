using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VEF.Interfaces;
using VEF.Interfaces.Services;
using VEF.Model.Services;

namespace VEF.Core.View
{
    /// <summary>
    /// Interaktionslogik für ProjectToolView.xaml
    /// </summary>
    public partial class ProjectToolView : UserControl, IContentView, INotifyPropertyChanged
    {
        IPropertiesService mPropertiesService;
        IProjectTreeService mProjectTreeService;
        IUnityContainer m_Container;

        public ProjectToolView()
        {
            InitializeComponent();

            m_Container = VEFModule.UnityContainer;
            mPropertiesService = m_Container.Resolve(typeof(IPropertiesService), "") as IPropertiesService;

            mProjectTreeService = m_Container.Resolve(typeof(IProjectTreeService), "") as IProjectTreeService;
            //   mProjectTreeService.TreeList = _treeList;

            // Add an event in order to know when an TreeViewItem is Expanded
            AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(treeItemExpanded), true);
        }

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

        // Event when a treeitem expands
        private void treeItemExpanded(object sender, RoutedEventArgs e)
        {
            // Get the source
            var item = e.OriginalSource as TreeViewItem;

            // If the item source is a Simple TreeViewItem
            if (item == null)
                return;

            //if (item.Name == "root")
            //{
            //    List<Server> servers = new List<Server>();

            //    servers = Server.GetServers();

            //    root.Items.Clear();

            //    // Fill the treeview with the servers
            //    root.ItemsSource = servers;
            //}

            // Get data from item as Folder (also works for Server)
            //var treeViewElement = item.DataContext as IItem;

            //// If there is no data
            //if (treeViewElement == null)
            //    return;

            // Load Children ( populate the treeview )
            ThreadPool.QueueUserWorkItem(delegate
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, (ThreadStart)delegate
                {
                    // treeViewElement.Items ;

                });
            });
        }


        // Load Children ( populate the treeview )
        //ThreadPool.QueueUserWorkItem(delegate
        //{
        //   List<Server> servers = Server.GetServers();

        //   Dispatcher.BeginInvoke(DispatcherPriority.Background, (ThreadStart)delegate
        //   {
        //      root.Items.Clear();

        //      // Fill the treeview with the servers
        //      root.ItemsSource = servers;
        //   });
        //});
        /// <summary>
        /// Should be called when a property value has changed
        /// </summary>
        /// <param name="propertyName">The property name</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
            //standardOutput.AutoFlush = true;
            //Console.SetOut(standardOutput);
        }

        //doesnt trigger?
        //private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //  ContextMenu tmp =  (ContextMenu)sender;
        //  tmp.ItemsSource = mProjectTreeService.CurrentItem.MenuOptions;
        //}

        private void ContextMenu_Opened(object sender, System.Windows.RoutedEventArgs e)
        {
            ContextMenu tmp = (ContextMenu)sender;
            var tvi = tmp.PlacementTarget as TreeViewItem;
            if (tvi != null)
            {
                //multiple items selected
                //if (treeView.SelectedItems.Count > 1)
                //{
                //    List<MenuItem> lmi = new List<MenuItem>();
                //    lmi.Add(new MenuItem() { Header = "Multiselected!" });
                //    tmp.ItemsSource = lmi;
                //}
                ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(tvi);

                //one item selected
                if (mProjectTreeService.SelectedItem != null)
                {
                    mProjectTreeService.SelectedItem.Parent = parent.DataContext as PItem;
              //      mProjectTreeService.SelectedItem.UnityContainer = m_Container;
                    tmp.ItemsSource = mProjectTreeService.SelectedItem.MenuOptions;
                }
            }
            //nothing selected          
        }

        private void _treeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void _treeList_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (mProjectTreeService.SelectedItem == null || mProjectTreeService.SelectedItem.MenuOptions == null)
            {
                e.Handled = true;
            }
        }

        protected void ProjItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tviSender = sender as TreeViewItem;

            if (!tviSender.IsSelected)
                return; //solution for multiple fired event http://stackoverflow.com/questions/2280049/why-is-the-treeviewitems-mousedoubleclick-event-being-raised-multiple-times-per

            var item = ((TreeViewItem)sender).Header as IItem; //Casting back to the binded IITem

            if (item != null)
            {
                //TreeNode tn = tmp.GetValue();.SelectedNode;
                //if (tn != null)
                //{
                IWorkspace workspace = m_Container.Resolve<AbstractWorkspace>();

                IOpenDocumentService odS = m_Container.Resolve<IOpenDocumentService>();

                var openValue = odS.OpenFromID(item.ContentID, true);

                //no handler found? -> try open model
                if (openValue == null)
                {
                    item.Open( null);
                }
                //creg.GetContentHandler();

                //NewContentAttribute newContent = window.NewContent;
                //if (newContent != null)
                //{
                //    IContentHandler handler = _dictionary[newContent];
                //  var openValue = handler.NewContent(newContent);
                //   workspace.Documents.Add(openValue);
                //    workspace.ActiveDocument = openValue;
                //}
                //       mProjectTreeService.SelectedItem = (IItem)tn.Tag;
                //       mPropertiesService.CurrentItem = (IItem)tn.Tag;
                //       }
            }

            //  var track = ((ListViewItem)sender).Content as Track; //Casting back to the binded Track
        }

        private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tmp = (TreeView)sender;
            if (tmp != null)
            {
                PItem tn = tmp.SelectedItem as PItem;
                if (tn != null)
                {
                    mProjectTreeService.SelectedItem = tn;
                    mPropertiesService.CurrentItem = tn;
                }
            }
        }

        private void TreeView_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            Application.Current.MainWindow.Focus();
            e.Effects = DragDropEffects.Move;

            Console.WriteLine("TreeView_DragOver");
        }

        private void TreeView_Drop(object sender, System.Windows.DragEventArgs e)
        {


        }

        private void TreeView_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine("DragEnter");
        }


    }
}