using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Interface;
using System.IO;
using System.Reflection;
using Configuration;

namespace Plugin.Manager
{
    public static class PluginManager
    {
        public static IList<IEffect> PluginEffectList { get; private set; }
        public static IList<IRenderer> PluginRendererList { get; private set; }

        static PluginManager()
        {
            PluginEffectList = new List<IEffect>();
            PluginRendererList = new List<IRenderer>();

            if (Directory.Exists(Paths.Startup + "\\Plugins"))
            {
                foreach (FileInfo fi in new DirectoryInfo(Paths.Startup + "\\Plugins").GetFiles("*.dll"))
                {
                    try
                    {
                        Assembly asm = Assembly.LoadFrom(fi.FullName);
                        foreach (Type t in asm.GetTypes())
                        {
                            if (t.GetInterface(typeof(IEffect).Name, true) != null)
                            {
                                PluginEffectList.Add((IEffect)Activator.CreateInstance(t));
                            }
                            else if (t.GetInterface(typeof(IRenderer).Name, true) != null)
                            {
                                PluginRendererList.Add((IRenderer)Activator.CreateInstance(t));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine("Cannot load plugins from file: " + fi.Name + "\n" + e);
                    }
                }
            }  
        }
    }
}
