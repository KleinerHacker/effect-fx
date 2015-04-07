using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using NET.Tools;
using Effect.CodeComplex.Properties;
using NET.Tools.WPF.CodeComplex;

namespace Effect.CodeComplex
{
    public class ZoomBlur : IEffect
    {
        private UIZoomBlur ui = new UIZoomBlur();

        #region IEffect Member

        public UIControl EffectController
        {
            get { return ui; }
        }

        public string Name
        {
            get { return "Zoom Blur"; }
        }

        public string Description
        {
            get { return "Creates an Zoom Blur Effect on this image. The point mof zooming is changable."; }
        }

        public EffectGroup Group
        {
            get { return Global.Groups.CodeComplex; }
        }

        public System.Windows.Controls.Image Icon
        {
            get { return Resources.ZoomBlur.ToWPFImage(16, 16); }
        }

        public bool HasUI
        {
            get { return true; }
        }

        public System.Windows.Media.Imaging.BitmapSource DoEffect(System.Windows.Media.Imaging.BitmapSource img, int? def)
        {
            ZoomBlurEffect eff = new ZoomBlurEffect();
            if (!def.HasValue)
            {
                eff.BlurAmount = ui.ZoomBlurAmount;
                eff.Center = ui.ZoomBlurPosition;
            }
            else
            {
                switch (def.Value)
                {
                    case 0:
                        eff.BlurAmount = -0.1;
                        break;
                    case 1:
                        eff.BlurAmount = 0.1;
                        break;
                    case 2:
                        eff.BlurAmount = -0.1;
                        eff.Center = new System.Windows.Point(0.1, 0.1);
                        break;
                    case 3:
                        eff.BlurAmount = -0.1;
                        eff.Center = new System.Windows.Point(0.9, 0.1);
                        break;
                    case 4:
                        eff.BlurAmount = -0.1;
                        eff.Center = new System.Windows.Point(0.9, 0.9);
                        break;
                    case 5:
                        eff.BlurAmount = -0.1;
                        eff.Center = new System.Windows.Point(0.1, 0.9);
                        break;
                    case 6:
                        eff.BlurAmount = 0.1;
                        eff.Center = new System.Windows.Point(0.1, 0.1);
                        break;
                    case 7:
                        eff.BlurAmount = 0.1;
                        eff.Center = new System.Windows.Point(0.9, 0.1);
                        break;
                    case 8:
                        eff.BlurAmount = 0.1;
                        eff.Center = new System.Windows.Point(0.9, 0.9);
                        break;
                    case 9:
                        eff.BlurAmount = 0.1;
                        eff.Center = new System.Windows.Point(0.1, 0.9);
                        break;
                }
                
            }

            return img.UseEffect(eff);
        }

        public int DefaultCount
        {
            get { return 10; }
        }

        public void ReinitializeUI()
        {
            ui = new UIZoomBlur();
        }

        public bool NeedProgressBar { get { return false; } }

        #endregion
    }
}
