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
using VEF.Interfaces;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace VEF.Core.WPF.View
{
    /// <summary>
    /// Interaktionslogik für PropertiesToolView.xaml
    /// </summary>
    public partial class PropertiesToolView : UserControl, IContentView, INotifyPropertyChanged
    {
        public PropertiesToolView()
        {
            InitializeComponent();
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void propGrid_SelectedObjectChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            foreach (PropertyItem prop in propGrid.Properties)
            {
                if (prop.IsExpandable) //Only expand things marked as Expandable, otherwise it will expand everything possible, such as strings, which you probably don't want.
                {
                    prop.IsExpanded = true; //This will expand the property.
                  //  prop.IsExpandable = false; //This will remove the ability to toggle the expanded state.
                }
            }
        }
    }
}
