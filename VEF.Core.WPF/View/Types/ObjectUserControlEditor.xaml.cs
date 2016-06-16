using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Microsoft.Win32;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

using System.ComponentModel.DataAnnotations;

namespace VEF.View.Types
{
    /// <summary>
    /// Interaktionslogik für ByteArray.xaml
    /// </summary>
    public partial class ObjectUserControlEditor : UserControl, ITypeEditor
    {
        public ObjectUserControlEditor()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(ObjectUserControlEditor),
                                                                                                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //[NotNullValidator(MessageTemplate = "Customer must have valid no")]
        //[StringLengthValidator(5, RangeBoundaryType.Inclusive, 5, RangeBoundaryType.Inclusive, MessageTemplate = "Customer no must have {3} characters.")]
        //[RegexValidator("[A-Z]{2}[0-9]{3}", MessageTemplate = "Customer no must be 2 capital letters and 3 numbers.")]
        [Required(ErrorMessage = "Customer no can not be empty")]
        [StringLength(5, ErrorMessage = "Customer no must be 5 characters.")]
        [RegularExpression("[A-Z]{2}[0-9]{3}", ErrorMessage = "Customer no must be 2 capital letters and 3 numbers.")]
        public object Value
        {
            get { return GetValue(ValueProperty); }
            private set
            {
                SetValue(ValueProperty, value);
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog o = new OpenFileDialog();
        //    //o.Filter = "SQL Server Compact Edition Database File|*.sdf";
        //    o.ShowDialog();
            
        //    string filename = o.FileName;
        //    Console.WriteLine(filename);
        //    Value = new Byte[10];//string.Empty;
        //}

        public FrameworkElement ResolveEditor(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            Binding binding = new Binding("Value");
            binding.Source = propertyItem;
            binding.Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            BindingOperations.SetBinding(this, ObjectUserControlEditor.ValueProperty, binding);
            return this;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
