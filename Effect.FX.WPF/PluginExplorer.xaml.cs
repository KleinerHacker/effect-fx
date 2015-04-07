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
using NET.Tools;
using NET.Tools.WPF;
using System.IO;

namespace Effect.FX.WPF
{
    /// <summary>
    /// Interaktionslogik für PluginExplorer.xaml
    /// </summary>
    public partial class PluginExplorer : Window
    {
        public enum Site
        {
            Effect,
            Renderer
        }

        private ImageSource original;

        public PluginExplorer(Site site)
        {
            InitializeComponent();
            UpdateEffectDLLList();
            UpdateRenderDLLList();

            original = imgEffectPreview.Source.Clone();

            switch (site)
            {
                case Site.Effect:
                    tabControl.SelectedIndex = 0;
                    break;
                case Site.Renderer:
                    tabControl.SelectedIndex = 1;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        #region Effects

        private void UpdateEffectDLLList()
        {
            var list = from value in Plugin.Manager.PluginManager.PluginEffectList 
                       orderby value.GetType().Assembly.Location ascending
                       group value by value.GetType().Assembly.Location;

            foreach (var group in list)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = new FileInfo(group.Key).Name;
                lbi.Tag = group.Key;

                lstEffectDLL.Items.Add(lbi);
            }
        }

        private void UpdateEffectGroupList()
        {
            if (lstEffectDLL.SelectedIndex < 0)
            {
                lstEffectGroup.Items.Clear();
                lstEffect.Items.Clear();
                lblEffectDescription.Text = "";
                imgEffectPreview.Source = original;
                return;
            }

            String asmName = (lstEffectDLL.SelectedItem as ListBoxItem).Tag as String;

            var list = from value in Plugin.Manager.PluginManager.PluginEffectList
                       where value.GetType().Assembly.Location.EqualsIgnoreCase(asmName)
                       orderby value.Group.Name ascending
                       group value by value.Group;

            lstEffectGroup.Items.Clear();
            foreach (var group in list)
            {
                ImageListBoxItem lbi = new ImageListBoxItem();
                lbi.Icon = group.Key.Image.Source;
                lbi.Content = group.Key.Name;
                lbi.Tag = group.Key;

                lstEffectGroup.Items.Add(lbi);
            }
        }

        private void UpdateEffectList()
        {
            if (lstEffectGroup.SelectedIndex < 0)
            {
                lstEffect.Items.Clear();
                lblEffectDescription.Text = "";
                imgEffectPreview.Source = original;
                return;
            }

            EffectGroup effGroup = (lstEffectGroup.SelectedItem as ListBoxItem).Tag as EffectGroup;

            var list = from value in Plugin.Manager.PluginManager.PluginEffectList
                       where value.Group.Equals(effGroup)
                       orderby value.Name ascending
                       select value;

            lstEffect.Items.Clear();
            foreach (IEffect eff in list)
            {
                ImageListBoxItem lbi = new ImageListBoxItem();
                lbi.Icon = eff.Icon.Source;
                lbi.Content = eff.Name;
                lbi.Tag = eff;

                lstEffect.Items.Add(lbi);
            }
        }

        private void lstEffectDLL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEffectGroupList();
        }        

        private void lstEffectGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEffectList();
        }
        
        private void lstEffect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstEffect.SelectedIndex < 0)
            {
                lblEffectDescription.Text = "";
                imgEffectPreview.Source = original;
                return;
            }

            IEffect eff = (lstEffect.SelectedItem as ListBoxItem).Tag as IEffect;

            lblEffectDescription.Text = eff.Description;
            imgEffectPreview.Source = eff.DoEffect(original as BitmapSource, 0);
        }

        #endregion

        #region Renderer

        private void UpdateRenderDLLList()
        {
            var list = from value in Plugin.Manager.PluginManager.PluginRendererList
                       orderby value.GetType().Assembly.Location ascending
                       group value by value.GetType().Assembly.Location;

            foreach (var group in list)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = new FileInfo(group.Key).Name;
                lbi.Tag = group.Key;

                lstRenderDLL.Items.Add(lbi);
            }
        }

        private void UpdateRenderGroupList()
        {
            if (lstRenderDLL.SelectedIndex < 0)
            {
                lstRenderGroup.Items.Clear();
                lstRenderer.Items.Clear();
                lblRenderDescription.Text = "";
                imgRenderPreview.Source = original;
                return;
            }

            String asmName = (lstRenderDLL.SelectedItem as ListBoxItem).Tag as String;

            var list = from value in Plugin.Manager.PluginManager.PluginRendererList
                       where value.GetType().Assembly.Location.EqualsIgnoreCase(asmName)
                       orderby value.Group.Name ascending
                       group value by value.Group;

            lstRenderGroup.Items.Clear();
            foreach (var group in list)
            {
                ImageListBoxItem lbi = new ImageListBoxItem();
                lbi.Icon = group.Key.Image.Source;
                lbi.Content = group.Key.Name;
                lbi.Tag = group.Key;

                lstRenderGroup.Items.Add(lbi);
            }
        }

        private void UpdateRenderList()
        {
            if (lstRenderGroup.SelectedIndex < 0)
            {
                lstRenderer.Items.Clear();
                lblRenderDescription.Text = "";
                imgRenderPreview.Source = original;
                return;
            }

            RendererGroup rendererGroup = (lstRenderGroup.SelectedItem as ListBoxItem).Tag as RendererGroup;

            var list = from value in Plugin.Manager.PluginManager.PluginRendererList
                       where value.Group.Equals(rendererGroup)
                       orderby value.Name ascending
                       select value;

            lstRenderer.Items.Clear();
            foreach (IRenderer renderer in list)
            {
                ImageListBoxItem lbi = new ImageListBoxItem();
                lbi.Icon = renderer.Icon.Source;
                lbi.Content = renderer.Name;
                lbi.Tag = renderer;

                lstRenderer.Items.Add(lbi);
            }
        }

        private void lstRenderDLL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRenderGroupList();
        }

        private void lstRenderGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRenderList();
        }

        private void lstRenderer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRenderer.SelectedIndex < 0)
            {
                lblRenderDescription.Text = "";
                imgRenderPreview.Source = original;
                return;
            }

            IRenderer renderer = (lstRenderer.SelectedItem as ListBoxItem).Tag as IRenderer;

            lblRenderDescription.Text = renderer.Description;
            if (renderer.ExampleImage == null)
            {
                imgRenderPreview.Source = renderer.GenerateImage(original as BitmapSource, 0);
            }
            else
            {
                imgRenderPreview.Source = renderer.ExampleImage;
            }
        }

        #endregion
    }
}
