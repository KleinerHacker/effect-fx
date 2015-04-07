using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using Effect.CodeComplex.Properties;
using NET.Tools;
using NET.Tools.WPF.CodeComplex;

namespace Effect.CodeComplex
{
    public class Toon : IEffect
    {
        private UIToon contr = new UIToon();

        #region IEffect Member

        public UIControl EffectController
        {
            get { return contr; }
        }

        public string Name
        {
            get { return "Toon-Effect"; }
        }

        public string Description
        {
            get { return "Creates a toon-effect on the image."; }
        }

        public EffectGroup Group
        {
            get { return Global.Groups.CodeComplex; }
        }

        public System.Windows.Controls.Image Icon
        {
            get { return Resources.Toon.ToWPFImage(16, 16); }
        }

        public bool HasUI
        {
            get { return true; }
        }

        public System.Windows.Media.Imaging.BitmapSource DoEffect(System.Windows.Media.Imaging.BitmapSource img, int? def)
        {
            ToonShaderEffect eff = new ToonShaderEffect();
            if (!def.HasValue)
                eff.Factor = contr.Factor;

            return img.UseEffect(eff);
        }

        public void ReinitializeUI()
        {
            contr = new UIToon();
        }

        public int DefaultCount
        {
            get { return 1; }
        }

        public bool NeedProgressBar { get { return false; } }

        #endregion
    }
}
