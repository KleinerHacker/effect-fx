using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NET.Tools;
using System.IO;
using System.Globalization;

namespace Effect.FX.WPF.IO
{
    public class LanguageExtension : AbstractLanguageExtension
    {
        private static ExternalLanguageManager man;

        static LanguageExtension()
        {
            man = new ExternalLanguageManager(new DirectoryInfo(
                Configuration.Paths.Language));
        }

        public static IList<CultureInfo> CultureList { get { return man.InstalledCultures; } }

        public LanguageExtension()
            : base("")
        {
        }

        public LanguageExtension(String key)
            : base(key)
        {
        }

        protected override string GetString(string key)
        {
            return man.GetString(key);
        }
    }
}
