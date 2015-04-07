using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Configuration
{
    public static class Paths
    {
        public static String Startup { get { return Application.StartupPath; } }

        //public static String Language { get { return Startup + "\\lan"; } }
        public static String Language { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2010\Projects\Effect FX\Effect.FX.WPF\lan"; } }
    }
}
