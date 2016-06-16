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
using VEF.Model;

namespace VEF.Core.View
{
    /// <summary>
    /// Interaktionslogik für OutputToolView.xaml
    /// </summary>
    public partial class OutputToolView : UserControl, IContentView, INotifyPropertyChanged
    {
        public OutputToolView()
        {
            InitializeComponent();
        }

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


        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.DataContext as OutputModel).TextBoxStreamWriter.setTB(tbOutput);
        }
    }
}
