
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace VEF.View.Types
{
    /// <summary>
    /// Interaktionslogik für Vector3Editor.xaml
    /// </summary>
    public partial class Vector3Editor : UserControl, ITypeEditor
    {
        public Vector3Editor()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty Vector3ValueProperty = DependencyProperty.Register("Vector3Value", typeof(object), typeof(Vector3Editor),
                                                                                             new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //[NotNullValidator(MessageTemplate = "Customer must have valid no")]
        //[StringLengthValidator(5, RangeBoundaryType.Inclusive, 5, RangeBoundaryType.Inclusive, MessageTemplate = "Customer no must have {3} characters.")]
        //[RegexValidator("[A-Z]{2}[0-9]{3}", MessageTemplate = "Customer no must be 2 capital letters and 3 numbers.")]
        //[Required(ErrorMessage = "Customer no can not be empty")]
        //[StringLength(5, ErrorMessage = "Customer no must be 5 characters.")]
        //[RegularExpression("[A-Z]{2}[0-9]{3}", ErrorMessage = "Customer no must be 2 capital letters and 3 numbers.")]
        public object Vector3Value
        {
            get {
                return GetValue(Vector3ValueProperty);
            }
            private set
            {
                //var dataContext = this.DataContext as Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem;
                //if (dataContext != null)
                //    dataContext.Value = value;

                SetValue(Vector3ValueProperty, value);
            }
        }

        public FrameworkElement ResolveEditor(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            Binding binding = new Binding("Vector3Value");
            binding.Source = propertyItem;
            binding.Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            BindingOperations.SetBinding(this, Vector3Editor.Vector3ValueProperty, binding);
            return this;
        }
    }
}
