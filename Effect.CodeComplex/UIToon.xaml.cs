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
    /// Interaktionslogik für UIFactor.xaml
    /// </summary>
    public partial class UIToon : UIControl
    {
        public float Factor { get { return (float)sldFactor.Value; } }

        public UIToon()
        {
            InitializeComponent();
        }

        private void sldFactor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DoValueChanged();
        }
    }
}
