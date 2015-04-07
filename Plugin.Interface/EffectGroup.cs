using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Plugin.Interface
{
    /// <summary>
    /// Represent a effect group
    /// </summary>
    public sealed class EffectGroup : IGroup
    {
        public String Name { get; private set; }
        public String Description { get; private set; }
        public Image Image { get; private set; }

        public EffectGroup(String name, String description, Image image)
        {
            Name = name;
            Description = description;
            Image = image;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (!(obj is EffectGroup))
                return false;
            else if (Object.ReferenceEquals(obj, this))
                return true;

            return Name.Equals((obj as EffectGroup).Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
