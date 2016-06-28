using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
using VEF.Model.Events;
using VEF.Interfaces.Converters;
using VEF.Interfaces;
using VEF.Interfaces.Services;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using Xceed.Wpf.AvalonDock;
using VEF.Interfaces.Events;

namespace VEF.View.Shell
{
    /// <summary>
    /// Interaktionslogik für ShellViewMetro.xaml
    /// </summary>
    public partial class ShellViewMetro : IShell
    {
        private ILoggerService _logger;
        private IWorkspace _workspace;
        private ContextMenu _docContextMenu;
        private MultiBinding _itemSourceBinding;

        public ShellViewMetro()
        {
            InitializeComponent();

            _docContextMenu = new ContextMenu();

            dockManager.DocumentContextMenu.ContextMenuOpening += _docContextMenu_ContextMenuOpening;
            dockManager.DocumentContextMenu.Opened += _docContextMenu_Opened;
            _itemSourceBinding = new MultiBinding();
            _itemSourceBinding.Converter = new DocumentContextMenuMixingConverter();
            var origModel = new Binding(".");
            var docMenus = new Binding("Model.Menus");
            _itemSourceBinding.Bindings.Add(origModel);
            _itemSourceBinding.Bindings.Add(docMenus);
            origModel.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            docMenus.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _itemSourceBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _docContextMenu.SetBinding(ContextMenu.ItemsSourceProperty, _itemSourceBinding);
        }


        #region IShell Members

        public void LoadLayout()
        {
            var layoutSerializer = new XmlLayoutSerializer(dockManager);
            layoutSerializer.LayoutSerializationCallback += (s, e) =>
            {
                var anchorable = e.Model as LayoutAnchorable;
                var document = e.Model as LayoutDocument;
                _workspace = VEFModule.UnityContainer.Resolve(typeof(AbstractWorkspace), "") as AbstractWorkspace;  


                if (anchorable != null)
                {
                    ToolViewModel model =  _workspace.Tools.FirstOrDefault( f => f.ContentId == e.Model.ContentId);
                    if (model != null)
                    {
                        e.Content = model;
                        model.IsVisible = anchorable.IsVisible;
                        model.IsActive = anchorable.IsActive;
                        model.IsSelected = anchorable.IsSelected;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                if (document != null)
                {
                    var fileService = VEFModule.UnityContainer.Resolve(typeof(IOpenDocumentService), "") as IOpenDocumentService;  

                    ContentViewModel model = fileService.OpenFromID(e.Model.ContentId);
                    if (model != null)
                    {
                        e.Content = model;
                        model.IsActive = document.IsActive;
                        model.IsSelected = document.IsSelected;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            };
            try
            {
                layoutSerializer.Deserialize(AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar + "AvalonDock.Layout.config");
            }
            catch (Exception)
            {
            }
        }

        public void SaveLayout()
        {
            var layoutSerializer = new XmlLayoutSerializer(dockManager);
            layoutSerializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar + "AvalonDock.Layout.config");
        }

        #endregion

        #region Events


        private void dockManager_ActiveContentChanged(object sender, EventArgs e)
        {
            DockingManager manager = sender as DockingManager;
            ContentViewModel cvm = manager.ActiveContent as ContentViewModel;

            VEFModule.EventAggregator.GetEvent<ActiveContentChangedEvent>().Publish(cvm);

          //  if (cvm != null) Logger.Log("Active document changed to " + cvm.Title, LogCategory.Info, LogPriority.None);
        }


        private void _docContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            RefreshMenuBinding();
        }

        private void _docContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            /* When you right click a document - move the focus to that document, so that commands on the context menu
             * which are based on the ActiveDocument work correctly. Example: Save.
             */
            LayoutDocumentItem doc = _docContextMenu.DataContext as LayoutDocumentItem;
            if (doc != null)
            {
                ContentViewModel model = doc.Model as ContentViewModel;
                if (model != null && model != dockManager.ActiveContent)
                {
                    dockManager.ActiveContent = model;
                }
            }
            e.Handled = false;
        }

        private void RefreshMenuBinding()
        {
            MultiBindingExpression b = BindingOperations.GetMultiBindingExpression(_docContextMenu,
                                                                                   ContextMenu.ItemsSourceProperty);
            b.UpdateTarget();
        }

        private void ThemeChanged(ITheme theme)
        {
            //HACK: Reset the context menu or else old menu status is retained and does not theme correctly
            //  dockManager.DocumentContextMenu = null;
            //dockManager.DocumentContextMenu = _docContextMenu;

            //foreach(var item in _docContextMenu.Items)
            //    dockManager.DocumentContextMenu.Items.Insert(0, item);
            //##     _docContextMenu.Style = FindResource("MetroContextMenu") as Style;
            //##     _docContextMenu.ItemContainerStyle = FindResource("MetroMenuStyle") as Style;
        }

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            var workspace = DataContext as IWorkspace;
            if (!workspace.Closing(e))
            {
                e.Cancel = true;
                return;
            }
            VEFModule.EventAggregator.GetEvent<WindowClosingEvent>().Publish(this);

            workspace.Documents.Clear(); // Clear Documents before app close

            SaveLayout();
        }

        private void ContentControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //HACK: Refresh the content control because in AutoHide mode this disappears. Needs to be fixed in AvalonDock.
            ContentControl c = sender as ContentControl;
            if (c != null)
            {
                var backup = c.Content;
                c.Content = null;
                c.Content = backup;
            }
        }

        #endregion

        #region Property

        private ILoggerService Logger
        {
            get
            {
                if (_logger == null)
                    _logger = VEFModule.UnityContainer.Resolve(typeof(ILoggerService), "") as ILoggerService; 

                return _logger;
            }
        }

        #endregion

    }
}
