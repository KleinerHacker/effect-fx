using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using Effect.Shader.Properties;
using NET.Tools;

namespace Effect.Shader
{
    public static class Global
    {
        public static class Groups
        {
            public static EffectGroup Simple { get { return new EffectGroup("Simple", "Simple effects", Resources.Simple.ToWPFImage(16, 16)); } }
            public static EffectGroup Complex { get { return new EffectGroup("Complex", "Complex effects", Resources.Complex.ToWPFImage(16, 16)); } }
        }
    }
}
