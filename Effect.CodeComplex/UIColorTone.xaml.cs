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
using System.Windows.Media.Effects;
using NET.Tools;

namespace Effect.CodeComplex
{
    /// <summary>
    /// Interaktionslogik für UIColorTone.xaml
    /// </summary>
    public partial class UIColorTone : UIControl
    {
        private System.Windows.Forms.ColorDialog colorDlg;

        public Color LightColor
        {
            get { return (Color)btnLightColor.Tag; }
        }

        public Color DarkColor
        {
            get { return (Color)btnDarkColor.Tag; }
        }

        public double Desaturation
        {
            get { return sldDesaturation.Value; }
        }

        public double Tone
        {
            get { return sldTone.Value; }
        }

        public UIColorTone()
        {
            InitializeComponent();
            SetColor(btnDarkColor, Color.FromScRgb(1.0f, 0.2f, 0.05f, 0.0f));
            SetColor(btnLightColor, Color.FromScRgb(1.0f, 1.0f, 0.9f, 0.5f));

            colorDlg = new System.Windows.Forms.ColorDialog();
        }

        private void sld_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;

            DoValueChanged();

            if (cmbDefault.SelectedIndex >= 0)
                cmbDefault.SelectedIndex = -1;
        }

        private void btnLightColor_Click(object sender, RoutedEventArgs e)
        {
            colorDlg.Color = ((Color)btnLightColor.Tag).ToDrawingColor();
            if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SetColor(btnLightColor, colorDlg.Color.ToWPFColor());
                sld_ValueChanged(this, null);
            }
        }

        private void btnDarkColor_Click(object sender, RoutedEventArgs e)
        {
            colorDlg.Color = ((Color)btnDarkColor.Tag).ToDrawingColor();
            if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SetColor(btnDarkColor, colorDlg.Color.ToWPFColor());
                sld_ValueChanged(this, null);
            }
        }

        private void cmbDefault_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDefault.SelectedIndex < 0)
                return;

            switch (cmbDefault.SelectedIndex)
            {
                case 0:
                    sldDesaturation.Value = 0.5d;
                    sldTone.Value = 1.0d;
                    SetColor(btnDarkColor, Color.FromScRgb(1.0f, 0.2f, 0.05f, 0.0f));
                    SetColor(btnLightColor, Color.FromScRgb(1.0f, 1.0f, 0.9f, 0.5f));
                    break;
            }
            
        }

        private void SetColor(Button btn, Color color)
        {
            btn.Foreground = new SolidColorBrush(color);
            btn.Tag = color;
        }
    }
}
