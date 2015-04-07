using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using NET.Tools;
using NET.Tools.WPF.CodeComplex;
using Effect.CodeComplex.Properties;

namespace Effect.CodeComplex
{
    public class ColorTone : IEffect
    {
        private UIColorTone contr = new UIColorTone();

        #region IEffect Member

        public UIControl EffectController
        {
            get { return contr; }
        }

        public string Name
        {
            get { return "Saphir"; }
        }

        public string Description
        {
            get { return "Create a color tone effect on the image"; }
        }

        public EffectGroup Group
        {
            get { return Global.Groups.CodeComplex; }
        }

        public System.Windows.Controls.Image Icon
        {
            get { return Resources.Complex.ToWPFImage(16, 16); }
        }

        public bool HasUI
        {
            get { return true; }
        }

        public System.Windows.Media.Imaging.BitmapSource DoEffect(System.Windows.Media.Imaging.BitmapSource img, int? def)
        {
            ColorToneEffect eff = new ColorToneEffect();
            if (!def.HasValue)
            {
                eff.DarkColor = contr.DarkColor;
                eff.LightColor = contr.LightColor;
                eff.Desaturation = contr.Desaturation;
                eff.Toned = contr.Tone;
            }

            return img.UseEffect(eff);
        }

        public void ReinitializeUI()
        {
            contr = new UIColorTone();
        }

        public int DefaultCount
        {
            get { return 1; }
        }

        public bool NeedProgressBar { get { return false; } }

        #endregion
    }
}
