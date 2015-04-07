using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Interface
{
    /// <summary>
    /// Represent a plugin interface for all plugins needed an UI
    /// </summary>
    /// <typeparam name="T">Group Type extends IGroup</typeparam>
    public interface IUIPlugin<T> : IPlugin<T> where T : IGroup
    {
        /// <summary>
        /// Control element to setup the plugin (UI)
        /// </summary>
        UIControl EffectController { get; }

        /// <summary>
        /// Has a Plugin-Controller-Element (UI)?
        /// </summary>
        bool HasUI { get; }

        /// <summary>
        /// Create a new Plugin-Controller Object (UI)
        /// </summary>
        void ReinitializeUI();
    }
}
