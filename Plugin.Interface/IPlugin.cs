using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Plugin.Interface
{
    /// <summary>
    /// Represent a plugin interface for all plugins (basic)
    /// </summary>
    /// <typeparam name="T">Group Type extends IGroup</typeparam>
    public interface IPlugin<T> where T : IGroup
    {
        /// <summary>
        /// Name of the plugin
        /// </summary>
        String Name { get; }
        /// <summary>
        /// Description of the plugin
        /// </summary>
        String Description { get; }
        /// <summary>
        /// Group of the plugin
        /// </summary>
        T Group { get; }
        /// <summary>
        /// Icon of the plugin
        /// </summary>
        Image Icon { get; }
    }
}
