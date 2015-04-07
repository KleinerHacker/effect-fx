using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Plugin.Interface;

namespace Effect.FX.WPF.Controls
{
    public partial class GroupComboBoxItem : ComboBoxItem
    {
        public GroupComboBoxItem(EffectGroup group)
        {
            InitializeComponent();

            imgImage.Source = group.Image.Source;
            lblText.Content = group.Name;
            Tag = group;
        }
    }
}
