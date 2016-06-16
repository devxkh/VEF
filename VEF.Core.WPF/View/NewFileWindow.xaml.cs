using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using VEF.Model.Attributes;

namespace VEF.Core.View
{
    /// <summary>
    /// Interaktionslogik für NewFileWindow.xaml
    /// </summary>
    internal partial class NewFileWindow : MetroWindow
    {
        public NewFileWindow()
        {
            InitializeComponent();
        }

        private void listBoxItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.NewContent = (sender as ListBoxItem).DataContext as NewContentAttribute;
            this.DialogResult = true;
        }

        public NewContentAttribute NewContent { get; private set; }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NewContent = this.listView.SelectedItem as NewContentAttribute;
            this.DialogResult = true;
        }
    }
}
