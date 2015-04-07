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

namespace Effect.Shader
{
    /// <summary>
    /// Interaktionslogik für UINoise.xaml
    /// </summary>
    public partial class UINoise : UIControl
    {
        public UINoise()
        {
            InitializeComponent();
        }

        private void sldDetails_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DoValueChanged();
        }

        private void sldRandomizing_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DoValueChanged();
        }

        private void sldAlpha_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DoValueChanged();
        }

        private void rbColored_Checked(object sender, RoutedEventArgs e)
        {
            DoValueChanged();
        }

        private void rbBlackWhite_Checked(object sender, RoutedEventArgs e)
        {
            DoValueChanged();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            rbColored.IsChecked = true;
            sldAlpha.Value = 0.1f;
            sldDetails.Value = 0.9f;
            sldRandomizing.Value = 5f;
        }
    }
}
