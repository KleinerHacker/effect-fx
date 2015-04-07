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
    /// Interaktionslogik für UIColorGrayScale.xaml
    /// </summary>
    public partial class UIColorGrayScale : UIControl
    {
        private static readonly Color defaultColor = Color.FromScRgb(1f, 0.3f, 0.59f, 0.11f);

        public Color SelectedColor { get { return sldColor.Color; } }

        public UIColorGrayScale()
        {
            InitializeComponent();
            sldColor.Color = defaultColor;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            sldColor.Color = defaultColor;
        }

        private void sldColor_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;

            DoValueChanged();
        }
    }
}
