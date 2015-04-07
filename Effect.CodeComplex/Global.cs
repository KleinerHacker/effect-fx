using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using NET.Tools;
using Effect.CodeComplex.Properties;

namespace Effect.CodeComplex
{
    public static class Global
    {
        public static class Groups
        {
            public static EffectGroup CodeComplex { get { return new EffectGroup("Code-Complex", "Effects from Code-Complex", Resources.Complex.ToWPFImage(16, 16)); } }
        }
    }
}
