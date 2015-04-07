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
    /// Interaktionslogik für UIZoomBlur.xaml
    /// </summary>
    public partial class UIZoomBlur : UIControl
    {
        public Point ZoomBlurPosition
        {
            get { return new Point(sldPosX.Value, sldPosY.Value); }
        }

        public double ZoomBlurAmount { get { return sldAmount.Value; } }

        public UIZoomBlur()
        {
            InitializeComponent();
        }

        private void sldAmount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DoValueChanged();
        }
    }
}
