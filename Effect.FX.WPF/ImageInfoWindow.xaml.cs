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
using NET.Tools;
using NET.Tools.WPF;
using System.Collections.ObjectModel;

namespace Effect.FX.WPF
{
    /// <summary>
    /// Interaktionslogik für ImageInfoWindow.xaml
    /// </summary>
    public partial class ImageInfoWindow : Window
    {
        #region Static 

        public static ImageInfoResult ShowDialog(BitmapFrame source)
        {
            ImageInfoWindow win = new ImageInfoWindow(source);
            if (win.ShowDialog().GetValueOrDefault(false))
                return new ImageInfoResult(win.MetaData,
                    new Size(Double.Parse(win.txtSizeX.Text), Double.Parse(win.txtSizeY.Text)),
                    new Size(Double.Parse(win.txtDPIX.Text), Double.Parse(win.txtDPIY.Text)));
            else
                return null;
        }

        #endregion

        public BitmapMetadata MetaData { get; private set; }

        private ImageInfoWindow(BitmapFrame source)
        {
            InitializeComponent();
            MetaData = source.Metadata as BitmapMetadata;

            if ((MetaData == null) || MetaData.IsFrozen)
            {
                gbList.IsEnabled = false;
                gbMetaData.IsEnabled = false;
            }

            txtRating.AddInputValidator(TextValidator.ValidateInt32, Key.Space);
            //txtDateTaken.AddValidator(TextValidator.ValidateDateTime);
            txtDPIX.AddInputValidator(TextValidator.ValidateDouble, Key.Space);
            txtDPIY.AddInputValidator(TextValidator.ValidateDouble, Key.Space);
            txtSizeX.AddInputValidator(TextValidator.ValidateDouble, Key.Space);
            txtSizeY.AddInputValidator(TextValidator.ValidateDouble, Key.Space);

            txtSizeX.Text = source.PixelWidth.ToString();
            txtSizeY.Text = source.PixelHeight.ToString();
            txtDPIX.Text = source.DpiX.ToString();
            txtDPIY.Text = source.DpiY.ToString();

            if (MetaData == null)
                return;

            try
            {
                txtComment.Text = MetaData.Comment;
                txtCopyright.Text = MetaData.Copyright;
                txtDateTaken.Text = MetaData.DateTaken;
                txtRating.Text = MetaData.Rating.ToString();
                txtSubject.Text = MetaData.Subject;
                txtTitle.Text = MetaData.Title;

                lblFormat.Content = MetaData.Format;
                lblLocation.Content = MetaData.Location;

                if (MetaData.Author != null)
                    foreach (String str in MetaData.Author)
                        lstAuthors.Items.Add(str);
                if (MetaData.Keywords != null)
                    foreach (String str in MetaData.Keywords)
                        lstKeywords.Items.Add(str);
            }
            catch (Exception e)
            {
                ErrorDialogEx.ShowDialog("Error", "Cannot read Meta-Data!", e);
                gbList.IsEnabled = false;
                gbMetaData.IsEnabled = false;
            }
        }

        private void AlwaysCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddAuthorExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            String res = InputDialogEx.ShowDialog("Add author", "Enter a author name:");
            if (res != null)
                lstAuthors.Items.Add(res);
        }

        private void SelectedAuthorCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = lstAuthors.SelectedIndex >= 0;
        }

        private void EditAuthorExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            String res = InputDialogEx.ShowDialog("Edit author", "Enter a new author name:",
                lstAuthors.SelectedItem.ToString());
            if (res != null)
            {
                int index = lstAuthors.SelectedIndex;
                lstAuthors.Items.Remove(lstAuthors.SelectedItem);
                lstAuthors.Items.Insert(index, res);
            }
        }

        private void RemoveAuthorExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            lstAuthors.Items.Remove(lstAuthors.SelectedItem);
        }

        private void SelectedKeywordCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = lstKeywords.SelectedIndex >= 0;
        }

        private void AddKeywordExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            String res = InputDialogEx.ShowDialog("Add keyword", "Enter a keyword:");
            if (res != null)
                lstKeywords.Items.Add(res);
        }

        private void EditKeywordExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            String res = InputDialogEx.ShowDialog("Edit keyword", "Enter a new keyword:",
                lstKeywords.SelectedItem.ToString());
            if (res != null)
            {
                int index = lstKeywords.SelectedIndex;
                lstKeywords.Items.Remove(lstKeywords.SelectedItem);
                lstKeywords.Items.Insert(index, res);
            }
        }

        private void RemoveKeywordExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            lstKeywords.Items.Remove(lstKeywords.SelectedItem);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!MetaData.IsFrozen)
            {
                MetaData.Author = new ReadOnlyCollection<string>(
                    new List<String>(lstAuthors.Items.Cast<String>()));
                MetaData.Comment = txtComment.Text;
                MetaData.Copyright = txtCopyright.Text;
                MetaData.DateTaken = txtDateTaken.Text;
                MetaData.Keywords = new ReadOnlyCollection<string>(
                    new List<String>(lstKeywords.Items.Cast<String>()));
                MetaData.Rating = Int32.Parse(txtRating.Text);
                MetaData.Subject = txtSubject.Text;
                MetaData.Title = txtTitle.Text;
            }

            DialogResult = true;
            Close();
        }
    }

    public class ImageInfoResult
    {
        public BitmapMetadata MetaData { get; private set; }
        public Size ImageSize { get; private set; }
        public Size DPI { get; private set; }

        internal ImageInfoResult(BitmapMetadata data, Size size, Size dpi)
        {
            MetaData = data;
            ImageSize = size;
            DPI = dpi;
        }
    }
}
