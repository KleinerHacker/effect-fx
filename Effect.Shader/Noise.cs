using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using System.Windows.Media.Imaging;
using Effect.Shader.Properties;
using NET.Tools;
using System.Windows.Controls;
using NET.Tools.WPF;

namespace Effect.Shader
{
    public class Noise : IEffect
    {
        private UINoise control = null;

        public BitmapSource DoEffect(BitmapSource img, int? defaultIndex)
        {
            if (defaultIndex != null)
            {
                if (defaultIndex == 0)
                {
                    ColorSnowEffect effect = new ColorSnowEffect();
                    return img.UseEffect(effect);
                }
                else if (defaultIndex == 1)
                {
                    BlackWhiteSnowEffect effect = new BlackWhiteSnowEffect();
                    return img.UseEffect(effect);
                }
                else
                    throw new NotImplementedException();
            }

            if (control.rbColored.IsChecked.GetValueOrDefault(false))
            {
                ColorSnowEffect effect = new ColorSnowEffect();
                effect.Alpha = (float)control.sldAlpha.Value;
                effect.Random = (float)control.sldRandomizing.Value;
                effect.Details = (float)control.sldDetails.Value;

                return img.UseEffect(effect);
            }
            else
            {
                BlackWhiteSnowEffect effect = new BlackWhiteSnowEffect();
                effect.Alpha = (float)control.sldAlpha.Value;
                effect.Random = (float)control.sldRandomizing.Value;
                effect.Details = (float)control.sldDetails.Value;

                return img.UseEffect(effect);
            }
        }

        public int DefaultCount
        {
            get { return 2; }
        }

        public bool NeedProgressBar
        {
            get { return false; }
        }

        public UIControl EffectController
        {
            get { return control; }
        }

        public bool HasUI
        {
            get { return true; }
        }

        public void ReinitializeUI()
        {
            control = new UINoise();
        }

        public string Name
        {
            get { return "Noise"; }
        }

        public string Description
        {
            get { return "Create a noise effect on the image"; }
        }

        public EffectGroup Group
        {
            get { return Global.Groups.Simple; }
        }

        public Image Icon
        {
            get { return Resources.Noise.ToWPFImage(16, 16); }
        }
    }
}
