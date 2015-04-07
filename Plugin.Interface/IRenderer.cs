using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Plugin.Interface
{
    /// <summary>
    /// Represent the interface for all renderer plugins.
    /// 
    /// Call order:
    /// 1. Need Source Image
    /// 2. generate Image
    /// 3. Need New Window
    /// The DefaultCount will be call if the GUI will be loaded.
    /// </summary>
    public interface IRenderer : IUIPlugin<RendererGroup>
    {
        /// <summary>
        /// Generate the renderer image with or without the basic image
        /// </summary>
        /// <param name="currentImage">The current image in the window</param>
        /// <param name="defaultIndex">The index of the default settings, clicked by the user. Can be NULL if the user use custom settings</param>
        /// <returns>The renderer image</returns>
        BitmapSource GenerateImage(BitmapSource currentImage, int? defaultIndex);
        /// <summary>
        /// Get the defaults of this renderer. Can be zero.
        /// </summary>
        int DefaultCount { get; }
        /// <summary>
        /// Returns TRUE to show a progress bar while do the rendering
        /// </summary>
        bool NeedProgressBar { get; }
        /// <summary>
        /// Returns an example image for tzhe effect explorer preview. If it is null the GenerateImage Method will be called with defaultIndex parameter value zero.
        /// </summary>
        BitmapSource ExampleImage { get; }
    }
}
