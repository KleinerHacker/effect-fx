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
using System.Drawing.Printing;
using NET.Tools;

namespace Effect.FX.WPF
{
    /// <summary>
    /// Interaktionslogik für PrinterWin.xaml
    /// </summary>
    public partial class PrinterWin : Window
    {
        private PrintDocument printDoc = new PrintDocument();
        private Stretch stretch = Stretch.None;
        private AlignmentX alignX = AlignmentX.Left;
        private AlignmentY alignY = AlignmentY.Top;
        private bool pageBounds = true;

        private System.Drawing.Image picture;

        public PrinterWin(BitmapSource bmp)
        {
            picture = bmp.ToBitmap();

            InitializeComponent();
            ppcPrint.Document = printDoc;

            foreach (String printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                cmbPrinters.Items.Add(printer);
            }
            cmbPrinters.SelectedItem = new PrinterSettings().PrinterName;

            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            System.Drawing.Rectangle bounds = System.Drawing.Rectangle.Empty;
            if (pageBounds)
                bounds = e.PageBounds;
            else
                bounds = e.MarginBounds;

            System.Drawing.Image tmp = (System.Drawing.Image)picture.Clone();
            if (stretch == Stretch.Fill)
                tmp = picture.Resize(bounds.Width, bounds.Height);
            else if (stretch == Stretch.Uniform)
                tmp = picture.ResizeProportional(bounds.Width, bounds.Height);

            int x = bounds.Left, y = bounds.Top;
            if (alignX == AlignmentX.Center)
                x = bounds.Left + bounds.Width / 2 - tmp.Width / 2;
            else if (alignX == AlignmentX.Right)
                x = bounds.Left + bounds.Width - tmp.Width;
            if (alignY == AlignmentY.Center)
                y = bounds.Top + bounds.Height / 2 - tmp.Height / 2;
            else if (alignY == AlignmentY.Bottom)
                y = bounds.Top + bounds.Height - tmp.Height;

            e.Graphics.DrawImage(tmp, new System.Drawing.Point(x, y));
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            printDoc.Print();
            Close();
        }

        private void cmbPrinters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            printDoc.PrinterSettings.PrinterName = cmbPrinters.SelectedItem as String;
            printDoc.DefaultPageSettings.PrinterSettings = printDoc.PrinterSettings;

            UpdatePaperSize();
            ppcPrint.InvalidatePreview();
        }

        private void UpdatePaperSize()
        {
            cmbPaperSize.Items.Clear();
            foreach (PaperSize size in printDoc.PrinterSettings.PaperSizes)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = size.PaperName + " (" + size.Kind + ")";
                item.Tag = size;

                cmbPaperSize.Items.Add(item);

                if (printDoc.DefaultPageSettings.PaperSize.PaperName.Equals(size.PaperName))
                    cmbPaperSize.SelectedItem = item;
            }
        }

        private void rbPortrait_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;

            printDoc.DefaultPageSettings.Landscape = false;
            ppcPrint.InvalidatePreview();
        }

        private void rbLandscape_Checked(object sender, RoutedEventArgs e)
        {
            printDoc.DefaultPageSettings.Landscape = true;
            ppcPrint.InvalidatePreview();
        }

        private void cmbPaperSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPaperSize.SelectedItem == null)
                return;

            printDoc.DefaultPageSettings.PaperSize =
                (cmbPaperSize.SelectedItem as ComboBoxItem).Tag as PaperSize;
            ppcPrint.InvalidatePreview();
        }

        private void rbNormal_Checked(object sender, RoutedEventArgs e)
        {
            stretch = Stretch.None;
            ppcPrint.InvalidatePreview();
        }

        private void rbStretch_Checked(object sender, RoutedEventArgs e)
        {
            stretch = Stretch.Fill;
            ppcPrint.InvalidatePreview();
        }

        private void rbUniform_Checked(object sender, RoutedEventArgs e)
        {
            stretch = Stretch.Uniform;
            ppcPrint.InvalidatePreview();
        }

        private void rbTop_Checked(object sender, RoutedEventArgs e)
        {
            alignY = AlignmentY.Top;
            ppcPrint.InvalidatePreview();
        }

        private void rbVCenter_Checked(object sender, RoutedEventArgs e)
        {
            alignY = AlignmentY.Center;
            ppcPrint.InvalidatePreview();
        }

        private void rbBottom_Checked(object sender, RoutedEventArgs e)
        {
            alignY = AlignmentY.Bottom;
            ppcPrint.InvalidatePreview();
        }

        private void rbLeft_Checked(object sender, RoutedEventArgs e)
        {
            alignX = AlignmentX.Left;
            ppcPrint.InvalidatePreview();
        }

        private void rbHCenter_Checked(object sender, RoutedEventArgs e)
        {
            alignX = AlignmentX.Center;
            ppcPrint.InvalidatePreview();
        }

        private void rbRight_Checked(object sender, RoutedEventArgs e)
        {
            alignX = AlignmentX.Right;
            ppcPrint.InvalidatePreview();
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            pageBounds = true;
            ppcPrint.InvalidatePreview();
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            pageBounds = false;
            ppcPrint.InvalidatePreview();
        }
    }
}
