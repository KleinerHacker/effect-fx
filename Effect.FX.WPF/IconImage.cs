using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using WPF.Extreme.Effects;
using System.Windows.Media.Imaging;

namespace Effect.FX.WPF
{
    public class IconImage : Image
    {
        static IconImage()
        {
            IsEnabledProperty.OverrideMetadata(typeof(IconImage),
                new FrameworkPropertyMetadata(true, new PropertyChangedCallback(
                    OnGrayImage)));
            WidthProperty.OverrideMetadata(typeof(IconImage),
                new FrameworkPropertyMetadata(16d));
            HeightProperty.OverrideMetadata(typeof(IconImage),
                new FrameworkPropertyMetadata(16d));
        }

        private static void OnGrayImage(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            IconImage img = (source as IconImage);
            bool isEnabled = Convert.ToBoolean(e.NewValue);

            if (img != null)
            {
                if (!isEnabled)
                {
                    img.Effect = new GrayscaleEffect();
                }
                else
                {
                    img.Effect = null;
                }
            }
        }
    }
}
