using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using NET.Tools;
using NET.Tools.WPF;
using Renderer.Simple.Properties;

namespace Renderer.Simple
{
    public class Noise : IRenderer
    {
        public BitmapSource GenerateImage(BitmapSource currentImage, int? defaultIndex)
        {
            if (defaultIndex != null)
            {
                if (defaultIndex == 0)
                {
                    ColorSnowEffect effect = new ColorSnowEffect();

                    effect.Alpha = 1f;
                    effect.Details = 1f;
                    effect.Random = 5f;

                    return currentImage.UseEffect(effect);
                }
                else if (defaultIndex == 1)
                {
                    BlackWhiteSnowEffect effect = new BlackWhiteSnowEffect();

                    effect.Alpha = 1f;
                    effect.Details = 1f;
                    effect.Random = 5f;

                    return currentImage.UseEffect(effect);
                }
                else
                    throw new NotImplementedException();
            }

            throw new NotImplementedException();
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
            get { return null; }
        }

        public bool HasUI
        {
            get { return false; }
        }

        public void ReinitializeUI()
        {
        }

        public string Name
        {
            get { return "Noise Renderer"; }
        }

        public string Description
        {
            get { return "Render a full noise"; }
        }

        public RendererGroup Group
        {
            get { return Global.GetSimpleGroup(); }
        }

        public Image Icon
        {
            get { return Resources.noise.ToWPFImage(16, 16); }
        }


        public BitmapSource ExampleImage
        {
            get { return null; }
        }
    }
}
