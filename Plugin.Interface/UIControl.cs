using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Plugin.Interface
{
    public class UIControl : UserControl
    {
        public event RoutedEventHandler ValueChanged;

        public UIControl()
        {
        }

        protected void DoValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, new RoutedEventArgs());
        }
    }
}
