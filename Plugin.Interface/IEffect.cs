using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Plugin.Interface
{
    //public class StatusEventArgs : EventArgs
    //{
    //    public int Maximum { get; private set; }
    //    public int Status { get; private set; }

    //    public StatusEventArgs(int max, int stat)
    //    {
    //        Maximum = max;
    //        Status = stat;
    //    }
    //}

    //public delegate void StatusEventHandler(object sender, StatusEventArgs e);

    /// <summary>
    /// Represent an effect plugin-object
    /// </summary>
    public interface IEffect : IUIPlugin<EffectGroup>
    {
        /// <summary>
        /// Do the effect on the given image
        /// </summary>
        /// <param name="img">The image to do the effect</param>
        /// <param name="defaultIndex">An integer with the default number (maximum is DefaultCount) or NULL for using values from the UI</param>
        /// <returns>A new image with the effect on it</returns>
        BitmapSource DoEffect(BitmapSource img, int? defaultIndex);
        /// <summary>
        /// Get the defaults of this effect. Minimum is one!
        /// </summary>
        int DefaultCount { get; }
        /// <summary>
        /// Returns TRUE to show a progress bar while do the effect
        /// </summary>
        bool NeedProgressBar { get; }
    }
}
