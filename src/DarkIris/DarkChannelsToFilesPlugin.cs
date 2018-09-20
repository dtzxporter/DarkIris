using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DarkIris
{
    [EffectCategory(EffectCategory.Effect), PluginSupportInfo(typeof(PluginSupportInfo), DisplayName = "Dark Iris")]
    public class DarkChannelsToFilesPlugin : PropertyBasedEffect
    {
        public static string StaticName
        {
            get
            {
                return "Extract channels to file";
            }
        }

        public static Image StaticIcon
        {
            get
            {
                return DarkIris.Properties.Resources.IrisIcon;
            }
        }

        public DarkChannelsToFilesPlugin()
            : base(DarkChannelsToFilesPlugin.StaticName, DarkChannelsToFilesPlugin.StaticIcon, "DarkIris Tools", EffectFlags.SingleRenderCall)
        {
        }

        protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection props)
        {
            props.ElementAt(21).Value = "DarkIris Tools";
            base.OnCustomizeConfigUIWindowProperties(props);
        }

        protected override void OnRender(Rectangle[] rois, int startIndex, int length)
        {
            if (length == 0)
            {
                return;
            }
            for (int i = startIndex; i < startIndex + length; i++)
            {
                this.Render(base.DstArgs.Surface, base.SrcArgs.Surface, rois[i]);
            }
        }

        private byte Clamp2Byte(int iValue)
        {
            if (iValue < 0)
            {
                return 0;
            }
            if (iValue > 255)
            {
                return 255;
            }
            return (byte)iValue;
        }

        private unsafe void Render(Surface dst, Surface src, Rectangle rect)
        {
            // Do this 4 times per channel, first is RED
            for (int i = rect.Top; i < rect.Bottom; i++)
            {
                // Cancel if need be
                if (base.IsCancelRequested)
                {
                    // Cancel it
                    return;
                }
                // Get the source pixel and the destination pixels
                ColorBgra* ptr = src.GetPointAddressUnchecked(rect.Left, i);
                ColorBgra* ptr2 = dst.GetPointAddressUnchecked(rect.Left, i);
                // Loop through pixels in this scanline
                for (int j = rect.Left; j < rect.Right; j++)
                {
                    // Get source
                    ColorBgra colorBgra = *ptr;
                    // Set it to the swapped sequence (RED)
                    colorBgra = ColorBgra.FromBgra(colorBgra.R, colorBgra.R, colorBgra.R, 255);
                    // Apply to destination
                    *ptr2 = colorBgra;
                    // Increase source pixel
                    ptr++;
                    // Increase dest pixel
                    ptr2++;
                }
            }
            // Use and dispose
            using (Bitmap workingBitmap = dst.CreateAliasedBitmap())
            {
                // Launch
                Thread thread = new Thread(() =>
                {
                    // Ask to save the red channel
                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        // Set title
                        saveDialog.Title = "Save the (Red) channel to a file";
                        // Setup file types
                        saveDialog.Filter = "PNG (*.png)|*.png;|JPEG (*.jpg)|*.jpg;|BMP (*.bmp)|*.bmp;|TIFF (*.tiff)|*.tiff;";
                        // Ask to save
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Save the image
                            workingBitmap.Save(saveDialog.FileName);
                        }
                    }
                });
                // Set state
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                // Merge
                thread.Join();
            }
            // Green Channel
            for (int i = rect.Top; i < rect.Bottom; i++)
            {
                // Cancel if need be
                if (base.IsCancelRequested)
                {
                    // Cancel it
                    return;
                }
                // Get the source pixel and the destination pixels
                ColorBgra* ptr = src.GetPointAddressUnchecked(rect.Left, i);
                ColorBgra* ptr2 = dst.GetPointAddressUnchecked(rect.Left, i);
                // Loop through pixels in this scanline
                for (int j = rect.Left; j < rect.Right; j++)
                {
                    // Get source
                    ColorBgra colorBgra = *ptr;
                    // Set it to the swapped sequence (GREEN)
                    colorBgra = ColorBgra.FromBgra(colorBgra.G, colorBgra.G, colorBgra.G, 255);
                    // Apply to destination
                    *ptr2 = colorBgra;
                    // Increase source pixel
                    ptr++;
                    // Increase dest pixel
                    ptr2++;
                }
            }
            // Use and dispose
            using (Bitmap workingBitmap = dst.CreateAliasedBitmap())
            {
                // Launch
                Thread thread = new Thread(() =>
                {
                    // Ask to save the red channel
                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        // Set title
                        saveDialog.Title = "Save the (Green) channel to a file";
                        // Setup file types
                        saveDialog.Filter = "PNG (*.png)|*.png;|JPEG (*.jpg)|*.jpg;|BMP (*.bmp)|*.bmp;|TIFF (*.tiff)|*.tiff;";
                        // Ask to save
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Save the image
                            workingBitmap.Save(saveDialog.FileName);
                        }
                    }
                });
                // Set state
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                // Merge
                thread.Join();
            }
            // Blue Channel
            for (int i = rect.Top; i < rect.Bottom; i++)
            {
                // Cancel if need be
                if (base.IsCancelRequested)
                {
                    // Cancel it
                    return;
                }
                // Get the source pixel and the destination pixels
                ColorBgra* ptr = src.GetPointAddressUnchecked(rect.Left, i);
                ColorBgra* ptr2 = dst.GetPointAddressUnchecked(rect.Left, i);
                // Loop through pixels in this scanline
                for (int j = rect.Left; j < rect.Right; j++)
                {
                    // Get source
                    ColorBgra colorBgra = *ptr;
                    // Set it to the swapped sequence (BLUE)
                    colorBgra = ColorBgra.FromBgra(colorBgra.B, colorBgra.B, colorBgra.B, 255);
                    // Apply to destination
                    *ptr2 = colorBgra;
                    // Increase source pixel
                    ptr++;
                    // Increase dest pixel
                    ptr2++;
                }
            }
            // Use and dispose
            using (Bitmap workingBitmap = dst.CreateAliasedBitmap())
            {
                // Launch
                Thread thread = new Thread(() =>
                {
                    // Ask to save the red channel
                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        // Set title
                        saveDialog.Title = "Save the (Blue) channel to a file";
                        // Setup file types
                        saveDialog.Filter = "PNG (*.png)|*.png;|JPEG (*.jpg)|*.jpg;|BMP (*.bmp)|*.bmp;|TIFF (*.tiff)|*.tiff;";
                        // Ask to save
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Save the image
                            workingBitmap.Save(saveDialog.FileName);
                        }
                    }
                });
                // Set state
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                // Merge
                thread.Join();
            }
            // Alpha Channel
            for (int i = rect.Top; i < rect.Bottom; i++)
            {
                // Cancel if need be
                if (base.IsCancelRequested)
                {
                    // Cancel it
                    return;
                }
                // Get the source pixel and the destination pixels
                ColorBgra* ptr = src.GetPointAddressUnchecked(rect.Left, i);
                ColorBgra* ptr2 = dst.GetPointAddressUnchecked(rect.Left, i);
                // Loop through pixels in this scanline
                for (int j = rect.Left; j < rect.Right; j++)
                {
                    // Get source
                    ColorBgra colorBgra = *ptr;
                    // Set it to the swapped sequence (ALPHA)
                    colorBgra = ColorBgra.FromBgra(colorBgra.A, colorBgra.A, colorBgra.A, 255);
                    // Apply to destination
                    *ptr2 = colorBgra;
                    // Increase source pixel
                    ptr++;
                    // Increase dest pixel
                    ptr2++;
                }
            }
            // Use and dispose
            using (Bitmap workingBitmap = dst.CreateAliasedBitmap())
            {
                // Launch
                Thread thread = new Thread(() =>
                {
                    // Ask to save the red channel
                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        // Set title
                        saveDialog.Title = "Save the (Alpha) channel to a file";
                        // Setup file types
                        saveDialog.Filter = "PNG (*.png)|*.png;|JPEG (*.jpg)|*.jpg;|BMP (*.bmp)|*.bmp;|TIFF (*.tiff)|*.tiff;";
                        // Ask to save
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Save the image
                            workingBitmap.Save(saveDialog.FileName);
                        }
                    }
                });
                // Set state
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                // Merge
                thread.Join();
            }
            // Set back to source
            dst.CopySurface(src);
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            return new PropertyCollection(new List<Property>());
        }
    }
}
