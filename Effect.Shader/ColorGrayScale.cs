using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using NET.Tools;
using NET.Tools.WPF;

namespace Effect.Shader
{
    public class ColorGrayScale : IEffect
    {
        private UIColorGrayScale contr = new UIColorGrayScale();

        #region IEffect Member

        public UIControl EffectController
        {
            get { return contr; }
        }

        public string Description
        {
            get { return "Create a gray scale with only one color channel"; }
        }

        public EffectGroup Group
        {
            get { return Global.Groups.Simple; }
        }

        public System.Windows.Controls.Image Icon
        {
            get { return Properties.Resources.GrayScale.ToWPFImage(16, 16); }
        }

        public bool HasUI
        {
            get { return true; }
        }

        public System.Windows.Media.Imaging.BitmapSource DoEffect(System.Windows.Media.Imaging.BitmapSource img, int? def)
        {
            GrayscaleEffect eff = new GrayscaleEffect();
            if (!def.HasValue)
            {
                eff.ChannelR = contr.SelectedColor.ScR;
                eff.ChannelG = contr.SelectedColor.ScG;
                eff.ChannelB = contr.SelectedColor.ScB;
            }
            else
            {
                switch (def)
                {
                    case 0:
                        //Use default
                        break;
                    case 1:
                        eff.ChannelR = 1;
                        eff.ChannelG = 0;
                        eff.ChannelB = 0;
                        break;
                    case 2:
                        eff.ChannelR = 0;
                        eff.ChannelG = 1;
                        eff.ChannelB = 0;
                        break;
                    case 3:
                        eff.ChannelR = 0;
                        eff.ChannelG = 0;
                        eff.ChannelB = 1;
                        break;
                    case 4:
                        eff.ChannelR = 1;
                        eff.ChannelG = 1;
                        eff.ChannelB = 0;
                        break;
                    case 5:
                        eff.ChannelR = 1;
                        eff.ChannelG = 0;
                        eff.ChannelB = 1;
                        break;
                    case 6:
                        eff.ChannelR = 0;
                        eff.ChannelG = 1;
                        eff.ChannelB = 1;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return img.UseEffect(eff);
        }

        public void ReinitializeUI()
        {
            contr = new UIColorGrayScale();
        }

        public string Name
        {
            get { return "GrayScale"; }
        }

        public int DefaultCount
        {
            get { return 7; }
        }

        public bool NeedProgressBar { get { return false; } }

        #endregion
    }
}
