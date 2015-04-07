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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Plugin.Interface;
using Plugin.Manager;
using Effect.FX.WPF.Controls;
using NET.Tools;
using System.Diagnostics;
using Microsoft.Windows.Controls.Ribbon;
using NET.Tools.WPF;
using Effect.FX.WPF.IO;
using System.Globalization;
using System.Threading;

namespace Effect.FX.WPF
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow, IEqualityComparer<MenuItem>
    {
        private System.Windows.Forms.OpenFileDialog openDlg;
        private System.Windows.Forms.SaveFileDialog saveDlg;
        private BitmapMetadata newMetaData = null;

        private List<MenuItem> groupMenuItemList = new List<MenuItem>();
        private UndoRedoManager<BitmapFrame> manager = new UndoRedoManager<BitmapFrame>();
        private IList<Image> previewImageList = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
            grdMain.Background = new LinearGradientBrush(
                Color.FromArgb(255, 0xDF, 0xE9, 0xF5),
                Color.FromArgb(255, 0xB9, 0xC9, 0xDA), 
                90);

            foreach (CultureInfo ci in LanguageExtension.CultureList)
            {
                RibbonMenuItem rmi = new RibbonMenuItem();
                rmi.Header = ci.EnglishName;
                rmi.Tag = ci;
                rmi.Click += (s, e) =>
                {
                    Thread.CurrentThread.CurrentUICulture = rmi.Tag as CultureInfo;
                    
                    MainWindow win = new MainWindow();
                    win.Show();

                    this.Close();
                };

                rmiLanguage.Items.Add(rmi);
            }

            manager.RedoListChanged += (s, e) =>
            {
                if (manager.CanRedo)
                {
                    btnQuickRedo.ToolTip = "Redo: " + manager.LastRedoActionName;
                    btnRedo.ToolTip = manager.LastRedoActionName;
                }
                else
                {
                    btnQuickRedo.ToolTip = "Redo";
                    btnRedo.ToolTip = "";
                }
            };
            manager.UndoListChanged += (s, e) =>
            {
                if (manager.CanUndo)
                {
                    btnQuickUndo.ToolTip = "Undo: " + manager.LastUndoActionName;
                    btnUndo.ToolTip = manager.LastUndoActionName;
                }
                else
                {
                    btnQuickUndo.ToolTip = "Undo";
                    btnUndo.ToolTip = "";
                }
            };

            openDlg = new System.Windows.Forms.OpenFileDialog();
            openDlg.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|Portable Network Graphic (*.png)|*.png";
            openDlg.DefaultExt = "bmp|jpg|png";
            openDlg.FilterIndex = 2;
            openDlg.Multiselect = true;

            saveDlg = new System.Windows.Forms.SaveFileDialog();
            saveDlg.Filter = openDlg.Filter;
            saveDlg.DefaultExt = openDlg.DefaultExt;
            saveDlg.FilterIndex = openDlg.FilterIndex;

            UpdateHistoryList();
            UpdateEffectList();
            UpdateRendererList();
        }

        private void UpdateHistoryList()
        {
            pnlLastPictures.Children.Clear();

            foreach (FileInfo fi in History.FileList)
            {
                RibbonButton btn = new RibbonButton();
                btn.Label = fi.Name;
                btn.Tag = fi;
                btn.Click += (s, e) =>
                {
                    FileInfo lastFile = btn.Tag as FileInfo;

                    if (imgPicture.Source == null)
                        LoadImage(lastFile.FullName);
                    else
                        StartNewInstanceWithImage(lastFile.FullName);
                };

                pnlLastPictures.Children.Add(btn);
            }
        }

        private void LoadImage(String fileName)
        {
            try
            {
                imgPicture.Source = BitmapFrame.Create(new Uri("file://" + fileName));
                imgPicture.Tag = fileName;

                //Update History
                FileInfo file = new FileInfo(fileName);
                History.Add(file);

                UpdateImageInformations(fileName);
                UpdatePreviewImages();
            }
            catch (IOException e)
            {
                ErrorDialogEx.ShowDialog("Unable to open file", "The file '" + fileName + "' cannot be opened!", e);
            }
        }

        private void UpdatePreviewImages()
        {
            BitmapSource source = (imgPicture.Source as BitmapSource).GetThumbnailImage(230, 124);

            foreach (Image prevImage in previewImageList)
            {
                if (prevImage.Tag is KeyValuePair<int, IEffect>)
                {
                    KeyValuePair<int, IEffect> value = (KeyValuePair<int, IEffect>)prevImage.Tag;
                    prevImage.Source = value.Value.DoEffect(source, value.Key);
                }
                else if (prevImage.Tag is KeyValuePair<int, IRenderer>)
                {
                    KeyValuePair<int, IRenderer> value = (KeyValuePair<int, IRenderer>)prevImage.Tag;
                    prevImage.Source = value.Value.GenerateImage(source, value.Key);
                }
                else
                    throw new NotImplementedException();
            }
        }

        private void UpdateImageInformations(String fileName)
        {
            lblSize.Content = new FileInfo(fileName).Length.ToByteSizeString();
            lblDimension.Content = imgPicture.Source.Width + "x" + imgPicture.Source.Height;
            lblDPI.Content = (imgPicture.Source as BitmapFrame).DpiX + "x" + (imgPicture.Source as BitmapFrame).DpiY;
            lblPixelFormat.Content = (imgPicture.Source as BitmapFrame).Format.ToString();
            lblDepth.Content = (imgPicture.Source as BitmapFrame).Format.BitsPerPixel;
        }

        private void UpdateEffectList()
        {
            var list = from value in Plugin.Manager.PluginManager.PluginEffectList
                       orderby value.Group.Name ascending
                       group value by value.Group;

            foreach (var group in list)
            {
                RibbonGroup rgGroup = new RibbonGroup();
                rgGroup.Header = group.Key.Name;
                rgGroup.SmallImageSource = group.Key.Image.Source;
                rgGroup.Tag = group.Key;

                rtEffects.Items.Add(rgGroup);


                /* Effects */
                IEnumerator<IEffect> effects = group.GetEnumerator();
                while (effects.MoveNext())
                {
                    RibbonMenuButton btn = new RibbonMenuButton();
                    btn.Label = effects.Current.Name;
                    btn.SmallImageSource = effects.Current.Icon.Source;

                    btn.ToolTipTitle = effects.Current.Name;
                    btn.ToolTipImageSource = BitmapFrame.Create(new Uri("pack://application:,,, /Effect FX;component/Resources/Info.ico"));
                    btn.ToolTipDescription = effects.Current.Description;
                    btn.ToolTipFooterTitle = effects.Current.Group.Name;
                    btn.ToolTipFooterImageSource = effects.Current.Group.Image.Source;
                    btn.ToolTipFooterDescription = effects.Current.Group.Description;

                    btn.Tag = effects.Current;

                    RibbonGallery gallery = new RibbonGallery();
                    gallery.MaxHeight = 500;
                    gallery.Command = (ICommand)Resources["cmdImageNull"];
                    RibbonGalleryCategory cat = new RibbonGalleryCategory();
                    gallery.Items.Add(cat);

                    for (int i = 0; i < effects.Current.DefaultCount; i++)
                    {
                        RibbonGalleryItem item = new RibbonGalleryItem();
                        Image image = new Image();
                        image.Width = 230;
                        image.Height = 124;
                        image.Stretch = Stretch.UniformToFill;
                        image.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        image.Source = BitmapFrame.Create(new Uri("pack://application:,,, /Effect FX;component/Resources/preview.png"));
                        image.Source = effects.Current.DoEffect(image.Source as BitmapSource, i);
                        image.Tag = new KeyValuePair<int, IEffect>(i, effects.Current);
                        previewImageList.Add(image);
                        item.Content = image;
                        cat.Items.Add(item);

                        int finalIndex = i;
                        image.MouseDown += (s, e) =>
                        {
                            //Console.WriteLine("OK: " + finalIndex);
                            manager.AddAction((btn.Tag as IEffect).Name, imgPicture.Source as BitmapFrame);
                            imgPicture.Source = (btn.Tag as IEffect).DoEffect(
                                imgPicture.Source as BitmapFrame, finalIndex);
                            UpdatePreviewImages();

                            gallery.SelectedItem = null;
                        };
                    }

                    btn.Items.Add(gallery);
                    rgGroup.Items.Add(btn);
                } //while 
            } //for
        }

        private void UpdateRendererList()
        {
            var list = from value in Plugin.Manager.PluginManager.PluginRendererList
                       orderby value.Group.Name ascending
                       group value by value.Group;

            foreach (var group in list)
            {
                RibbonGroup rgGroup = new RibbonGroup();
                rgGroup.Header = group.Key.Name;
                rgGroup.SmallImageSource = group.Key.Image.Source;
                rgGroup.Tag = group.Key;

                rtRenderer.Items.Add(rgGroup);


                /* Effects */
                IEnumerator<IRenderer> renderers = group.GetEnumerator();
                while (renderers.MoveNext())
                {
                    if (renderers.Current.DefaultCount > 0)
                    {
                        RibbonMenuButton btn = new RibbonMenuButton();
                        btn.Label = renderers.Current.Name;
                        btn.SmallImageSource = renderers.Current.Icon.Source;

                        btn.ToolTipTitle = renderers.Current.Name;
                        btn.ToolTipImageSource = BitmapFrame.Create(new Uri("pack://application:,,, /Effect FX;component/Resources/Info.ico"));
                        btn.ToolTipDescription = renderers.Current.Description;
                        btn.ToolTipFooterTitle = renderers.Current.Group.Name;
                        btn.ToolTipFooterImageSource = renderers.Current.Group.Image.Source;
                        btn.ToolTipFooterDescription = renderers.Current.Group.Description;

                        btn.Tag = renderers.Current;

                        RibbonGallery gallery = new RibbonGallery();
                        gallery.MaxHeight = 500;
                        gallery.Command = (ICommand)Resources["cmdImageNull"];
                        RibbonGalleryCategory cat = new RibbonGalleryCategory();
                        gallery.Items.Add(cat);

                        for (int i = 0; i < renderers.Current.DefaultCount; i++)
                        {
                            RibbonGalleryItem item = new RibbonGalleryItem();
                            Image image = new Image();
                            image.Width = 230;
                            image.Height = 124;
                            image.Stretch = Stretch.UniformToFill;
                            image.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            image.Source = BitmapFrame.Create(new Uri("pack://application:,,, /Effect FX;component/Resources/preview.png"));
                            image.Source = renderers.Current.GenerateImage(image.Source as BitmapSource, i);
                            image.Tag = new KeyValuePair<int, IRenderer>(i, renderers.Current);
                            previewImageList.Add(image);
                            item.Content = image;
                            cat.Items.Add(item);

                            int finalIndex = i;
                            image.MouseDown += (s, e) =>
                            {
                                //Console.WriteLine("OK: " + finalIndex);
                                manager.AddAction((btn.Tag as IRenderer).Name, imgPicture.Source as BitmapFrame);
                                imgPicture.Source = (btn.Tag as IRenderer).GenerateImage(
                                    imgPicture.Source as BitmapFrame, finalIndex);
                                UpdatePreviewImages();

                                gallery.SelectedItem = null;
                            };
                        }

                        btn.Items.Add(gallery);
                        rgGroup.Items.Add(btn);
                    }
                    else
                    {
                        RibbonButton btn = new RibbonButton();
                        btn.Label = renderers.Current.Name;
                        btn.SmallImageSource = renderers.Current.Icon.Source;

                        btn.ToolTipTitle = renderers.Current.Name;
                        btn.ToolTipImageSource = BitmapFrame.Create(new Uri("pack://application:,,, /Effect FX;component/Resources/Info.ico"));
                        btn.ToolTipDescription = renderers.Current.Description;
                        btn.ToolTipFooterTitle = renderers.Current.Group.Name;
                        btn.ToolTipFooterImageSource = renderers.Current.Group.Image.Source;
                        btn.ToolTipFooterDescription = renderers.Current.Group.Description;

                        btn.Tag = renderers.Current;

                        rgGroup.Items.Add(btn);
                    }
                } //while 
            } //for
        }

        private void RotateFlip(System.Drawing.RotateFlipType type)
        {
            manager.AddAction(type.ToString(), imgPicture.Source as BitmapFrame);
            imgPicture.Source = BitmapFrame.Create(
                (imgPicture.Source as BitmapFrame).RotateFlip(type));

            UpdatePreviewImages();
        }

        private int GetGroupSeparatorInToolBar(EffectGroup group)
        {
            //for (int i = 0; i < tbEffects.Items.Count; i++ )
            //{
            //    object obj = tbEffects.Items[i];

            //    if (obj is Separator)
            //    {
            //        if ((obj as Separator).ToolTip.Equals(group.Name))
            //            return i;
            //    }
            //}

            return -1;
        }

        #region IEqualityComparer<MenuItem> Member

        public bool Equals(MenuItem x, MenuItem y)
        {
            return
                x.Header.Equals(y.Header);
        }

        public int GetHashCode(MenuItem obj)
        {
            return
                obj.Header.GetHashCode();
        }

        #endregion

        private void UndoCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = manager.CanUndo;
        }

        private void UndoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            imgPicture.Source = manager.Undo(imgPicture.Source as BitmapFrame);
            UpdatePreviewImages();
        }

        private void ExitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void AlwaysCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (openDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (String fileName in openDlg.FileNames)
                {
                    if (imgPicture.Source == null)
                        LoadImage(fileName);
                    else
                        StartNewInstanceWithImage(fileName);
                }
            }
        }

        private void RedoCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = manager.CanRedo;
        }

        private void RedoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            imgPicture.Source = manager.Redo(imgPicture.Source as BitmapFrame);
            UpdatePreviewImages();
        }

        private void ImageNotNullCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (imgPicture == null)
                return;

            e.CanExecute = imgPicture.Source != null;
        }

        private void RotateLeftExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
        }

        private void RotateRightExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
        }

        private void FlipHorizontalExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipX);
        }

        private void FlipVerticalExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);
        }

        private void SaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                imgPicture.Tag = saveDlg.FileName;
                (imgPicture.Source as BitmapFrame).Save(saveDlg.FileName, newMetaData);
                newMetaData = null;
            }
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            String fileName = imgPicture.Tag as String;
            (imgPicture.Source as BitmapFrame).Save(fileName, newMetaData);
            newMetaData = null;
        }

        private void EffectExplorerExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            EffectExplorer exp = new EffectExplorer(imgPicture.Source as BitmapFrame);
            exp.DoEffect += (s, args) =>
            {
                manager.AddAction(exp.SelectedEffect.Name, imgPicture.Source as BitmapFrame);
                imgPicture.Source = exp.SelectedEffect.DoEffect(imgPicture.Source as BitmapFrame, null);
                UpdatePreviewImages();
            };
            exp.ShowDialog();
        }

        /// <summary>
        /// Read the arguments for the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PureArgument<String> arg = App.Arguments.ArgumentList[App.FILENAMES] as PureArgument<String>;

            if (arg.IsSet)
            {
                try
                {
                    if (File.Exists(arg.ValueList[0] as String))
                        LoadImage(arg.ValueList[0] as String);
                    else
                        throw new Exception("Cannot find the given file!");

                    if (arg.ValueList.Count > 1)
                        for (int i = 1; i < arg.ValueList.Count; i++)
                        {
                            Console.WriteLine(this.GetType().Assembly.Location);
                            StartNewInstanceWithImage(arg.ValueList[i]);                            
                        }
                }
                catch (Exception ex)
                {
                    ErrorDialogEx.ShowDialog("Error", "The file <" + arg.ValueList[0] + "> cannot be loaded!", ex);
                }
            }
        }

        private void StartNewInstanceWithImage(String image)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = this.GetType().Assembly.Location;
            info.Arguments = "\"" + image + "\" /nosplash";
            info.WorkingDirectory = new FileInfo(this.GetType().Assembly.Location).Directory.FullName;

            Process.Start(info);
        }

        private void EffectManagerExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new PluginExplorer(PluginExplorer.Site.Effect).ShowDialog();
        }

        private void ImageInfoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ImageInfoResult res = ImageInfoWindow.ShowDialog(imgPicture.Source as BitmapFrame);
            if (res != null)
            {
                newMetaData = res.MetaData;
                //TODO Resize / DPI
            }
        }

        private void CopyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetImage(imgPicture.Source as BitmapSource);
        }

        private void PrinterSetupExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new System.Windows.Forms.PrintDialog().ShowDialog();
        }

        private void PrintExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new PrinterWin(imgPicture.Source as BitmapSource).ShowDialog();
        }

        private void InfoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void EnglishClick(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            MainWindow win = new MainWindow();
            win.Show();

            this.Close();
        }

        private void RenderExplorerExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RendererManagerExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new PluginExplorer(PluginExplorer.Site.Renderer).ShowDialog();
        }
    }
}
