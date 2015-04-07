using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Plugin.Interface;

namespace Effect.CodeComplex
{
    /// <summary>
    /// Interaktionslogik für UIEmbossed.xaml
    /// </summary>
    public partial class UIEmbossed : UIControl
    {
        public double EmbossedWidth { get { return sldWidth.Value; } }
        public double EmbossedAmount { get { return sldAmount.Value; } }

        public UIEmbossed()
        {
            InitializeComponent();
        }

        private void sldAmount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DoValueChanged();
        }
    }
}
