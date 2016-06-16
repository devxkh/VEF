using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VEF.View.Types
{
    public class PasswordEditor : Xceed.Wpf.Toolkit.PropertyGrid.Editors.ITypeEditor
    {
        Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem m_PropertyItem;
        PasswordBox m_PasswordBox;

        public FrameworkElement ResolveEditor(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            m_PropertyItem = propertyItem;
            m_PasswordBox = new PasswordBox();
            m_PasswordBox.Password = (String)propertyItem.Value;
            m_PasswordBox.LostFocus += OnLostFocus;
            return m_PasswordBox;
        }

        void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (0 != m_PasswordBox.Password.CompareTo((string)m_PropertyItem.Value))
            {
                m_PropertyItem.Value = m_PasswordBox.Password;
            }
        }
    }
}
