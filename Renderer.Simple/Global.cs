using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using Renderer.Simple.Properties;
using NET.Tools;

namespace Renderer.Simple
{
    public static class Global
    {
        private static RendererGroup simpleGroup = null;

        public static RendererGroup GetSimpleGroup()
        {
            if (simpleGroup == null)
            {
                simpleGroup = new RendererGroup("Simple", "Simple Renderer Group", Resources.simple.ToWPFImage(16, 16));
            }

            return simpleGroup;
        }
    }
}
