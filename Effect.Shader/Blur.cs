using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using Effect.Shader.Properties;
using System.Windows.Media.Effects;
using NET.Tools;

namespace Effect.Shader
{
    public class Blur : IEffect
    {
        private UIBlur contr = new UIBlur();

        #region IEffect Member

        public UIControl EffectController
        {
            get { return contr; }
        }

        public string Description
        {
            get { return "Create a blur effect on this image"; }
        }

        public EffectGroup Group
        {
            get { return Global.Groups.Complex; }
        }

        public System.Windows.Controls.Image Icon
        {
            get { return Resources.Blur.ToWPFImage(16, 16); }
        }

        public System.Windows.Media.Imaging.BitmapSource DoEffect(System.Windows.Media.Imaging.BitmapSource img, int? def)
        {
            double value = 0d;
            if (!def.HasValue)
                value = contr.BlurValue;
            else
                value = 10d - (double)def.Value;

            BlurEffect eff = new BlurEffect();
            eff.Radius = value;

            return img.UseEffect(eff);
        }

        public string Name
        {
            get { return "Blur"; }
        }

        public void ReinitializeUI()
        {
            contr = new UIBlur();
        }

        public bool HasUI
        {
            get { return true; }
        }

        public int DefaultCount
        {
            get { return 9; }
        }

        public bool NeedProgressBar { get { return false; } }

        #endregion
    }
}
