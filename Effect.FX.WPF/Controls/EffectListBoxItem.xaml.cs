using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Plugin.Interface;

namespace Effect.FX.WPF.Controls
{
    public partial class EffectListBoxItem : ListBoxItem
    {
        public EffectListBoxItem(IEffect effect)
        {
            InitializeComponent();

            imgIcon.Source = effect.Icon.Source;
            lblText.Content = effect.Name;
            Tag = effect;
        }
    }
}
