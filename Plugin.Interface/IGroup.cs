using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Plugin.Interface
{
    /// <summary>
    /// Represent a group interface for default groups
    /// </summary>    
    public interface IGroup
    {
        /// <summary>
        /// Name of group
        /// </summary>
        String Name { get; }
        /// <summary>
        /// Description of group
        /// </summary>
        String Description { get;  }
        /// <summary>
        /// Image of group
        /// </summary>
        Image Image { get; }
    }
}
