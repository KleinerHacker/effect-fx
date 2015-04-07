using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Plugin.Interface;
using Plugin.Manager;
using Effect.FX.WPF.Controls;
using NET.Tools;

namespace Effect.FX.WPF
{
    /// <summary>
    /// Interaktionslogik für EffectExplorer.xaml
    /// </summary>
    public partial class EffectExplorer : Window
    {
        public event RoutedEventHandler DoEffect;

        public IEffect SelectedEffect
        {
            get { return (lstEffects.SelectedItem as EffectListBoxItem).Tag as IEffect; }
        }

        public EffectExplorer(BitmapFrame source)
        {
            InitializeComponent();
            imgSource.Source = source.GetThumbnailImage(300, 200, 96d, 96d);

            UpdateGroupList();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();

            DoDoEffect();
        }

        private void UpdateGroupList()
        {
            var list = from value in Plugin.Manager.PluginManager.PluginEffectList
                       orderby value.Group.Name ascending
                       group value by value.Group;

            foreach (var group in list)
            {
                cmbGroups.Items.Add(new GroupComboBoxItem(group.Key));
            }
            cmbGroups.SelectedIndex = 0;
        }

        private void UpdateEffectList()
        {
            EffectGroup effGroup = (cmbGroups.SelectedItem as GroupComboBoxItem).Tag as EffectGroup;

            var list = from value in Plugin.Manager.PluginManager.PluginEffectList
                       where value.Group.Equals(effGroup)
                       orderby value.Name ascending
                       select value;

            lstEffects.Items.Clear();
            foreach (IEffect effect in list)
            {
                lstEffects.Items.Add(new EffectListBoxItem(effect));
            }
            lstEffects.SelectedIndex = 0;
        }

        private void cmbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEffectList();
        }

        private void lstEffects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pnlControl.Child = null;

            if (lstEffects.SelectedIndex < 0)
                return;

            IEffect effect = (lstEffects.SelectedItem as EffectListBoxItem).Tag as IEffect;

            //Create the control element
            if (effect.HasUI)
            {
                effect.ReinitializeUI();
                effect.EffectController.Width = Double.NaN;
                effect.EffectController.Height = Double.NaN;
                effect.EffectController.HorizontalAlignment = HorizontalAlignment.Stretch;
                effect.EffectController.VerticalAlignment = VerticalAlignment.Stretch;
                effect.EffectController.ValueChanged += (s, args) =>
                {
                    imgDest.Source = effect.DoEffect((imgSource.Source as BitmapSource), null);
                };
                pnlControl.Child = effect.EffectController;
            }

            //The first time effect on image
            imgDest.Source = effect.DoEffect((imgSource.Source as BitmapSource), null);
        }

        private void DoDoEffect()
        {
            if (DoEffect != null)
                DoEffect(this, new RoutedEventArgs());
        }

        private void btnUse_Click(object sender, RoutedEventArgs e)
        {
            DoDoEffect();

            imgSource.Source = imgDest.Source;
            IEffect effect = (lstEffects.SelectedItem as EffectListBoxItem).Tag as IEffect;
            imgDest.Source = effect.DoEffect((imgSource.Source as BitmapSource), null);
        }
    }
}
