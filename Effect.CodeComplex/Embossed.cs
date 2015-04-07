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
    public class Embossed : IEffect
    {
        private UIEmbossed ui = new UIEmbossed();

        #region IEffect Member

        public UIControl EffectController
        {
            get { return ui; }
        }

        public string Name
        {
            get { return "Embossed"; }
        }

        public string Description
        {
            get { return "Embossed Effect"; }
        }

        public EffectGroup Group
        {
            get { return Global.Groups.CodeComplex; }
        }

        public System.Windows.Controls.Image Icon
        {
            get { return Resources.Embossed.ToWPFImage(16, 16); }
        }

        public bool HasUI
        {
            get { return true; }
        }

        public System.Windows.Media.Imaging.BitmapSource DoEffect(System.Windows.Media.Imaging.BitmapSource img, int? def)
        {
            EmbossedEffect eff = new EmbossedEffect();
            if (!def.HasValue)
            {
                eff.Amount = ui.EmbossedAmount;
                eff.Width = ui.EmbossedWidth;
            }
            else if (def.Value == 1)
            {
                eff.Amount = 50;
            }
            else if (def.Value == 2)
            {
                eff.Amount = 20;
                eff.Width = 0.0005;
            }

            return img.UseEffect(eff);
        }

        public int DefaultCount
        {
            get { return 3; }
        }

        public void ReinitializeUI()
        {
            ui = new UIEmbossed();
        }

        public bool NeedProgressBar { get { return false; } }

        #endregion
    }
}
