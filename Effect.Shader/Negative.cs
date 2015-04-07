using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using Effect.Shader.Properties;
using System.Windows.Media.Imaging;
using NET.Tools;
using NET.Tools.WPF;

namespace Effect.Shader
{
    public class Negative : IEffect
    {
        #region IEffect Member

        public string Name
        {
            get { return "Negative"; }
        }

        public string Description
        {
            get { return "Create a negative from this image."; }
        }

        public EffectGroup Group
        {
            get { return Global.Groups.Simple; }
        }

        public UIControl EffectController
        {
            get { return null; }
        }

        public System.Windows.Controls.Image Icon
        {
            get { return Resources.Negation.ToWPFImage(16, 16); }
        }

        public System.Windows.Media.Imaging.BitmapSource DoEffect(System.Windows.Media.Imaging.BitmapSource img, int? def)
        {
            return img.UseEffect(new NegativeEffect());
        }

        public void ReinitializeUI()
        {
            
        }

        public bool HasUI
        {
            get { return false; }
        }

        public int DefaultCount
        {
            get { return 1; }
        }

        public bool NeedProgressBar { get { return false; } }

        #endregion
    }
}
