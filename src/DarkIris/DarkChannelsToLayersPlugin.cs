using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.PropertySystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DarkIris
{
    [EffectCategory(EffectCategory.Effect), PluginSupportInfo(typeof(PluginSupportInfo), DisplayName = "Dark Iris")]
    public class DarkChannelsToLayersPlugin : PropertyBasedEffect
    {
        public static string StaticName
        {
            get
            {
                return "Extract channels to layers";
            }
        }

        public static Image StaticIcon
        {
            get
            {
                return DarkIris.Properties.Resources.IrisIcon;
            }
        }

        public DarkChannelsToLayersPlugin()
            : base(DarkChannelsToLayersPlugin.StaticName, DarkChannelsToLayersPlugin.StaticIcon, "DarkIris Tools", EffectFlags.SingleRenderCall)
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
            if (length > 1)
            {
                DarkNet.PaintWorkspace.GetWorkspaceFromInstance(DarkNet.PaintForm.GetMainWindow()).RunOnMainThread((Action)delegate
                {
                    System.Windows.Forms.MessageBox.Show("Extracting channels to layers is only supported for one layer at a time.", "DarkIris", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                });
                return;
            }
            for (int i = startIndex; i < startIndex + length; i++)
            {
                this.Render(base.DstArgs.Surface, base.SrcArgs.Surface, rois[i]);
            }
        }

        private unsafe void Render(Surface dst, Surface src, Rectangle rect)
        {
            var Instance = DarkNet.PaintForm.GetMainWindow();
            var Workspace = DarkNet.PaintWorkspace.GetWorkspaceFromInstance(Instance);

            Workspace.RunOnMainThread((Action)delegate
            {
                // Make a new layer
                var DocumentLayer = Layer.CreateBackgroundLayer(dst.Width, dst.Height, "Red Channel");

                // Assign pixel data in the layer
                for (int i = rect.Top; i < rect.Bottom; i++)
                {
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
                        DocumentLayer.Surface[j, i] = colorBgra;
                        // Increase source pixel
                        ptr++;
                    }
                }

                // Add it if we can!
                Workspace.GetActiveDocumentWorkspace().AddLayer(DocumentLayer);
            });

            Workspace.RunOnMainThread((Action)delegate
            {
                // Make a new layer
                var DocumentLayer = Layer.CreateBackgroundLayer(dst.Width, dst.Height, "Green Channel");

                // Assign pixel data in the layer
                for (int i = rect.Top; i < rect.Bottom; i++)
                {
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
                        DocumentLayer.Surface[j, i] = colorBgra;
                        // Increase source pixel
                        ptr++;
                    }
                }

                // Add it if we can!
                Workspace.GetActiveDocumentWorkspace().AddLayer(DocumentLayer);
            });

            Workspace.RunOnMainThread((Action)delegate
            {
                // Make a new layer
                var DocumentLayer = Layer.CreateBackgroundLayer(dst.Width, dst.Height, "Blue Channel");

                // Assign pixel data in the layer
                for (int i = rect.Top; i < rect.Bottom; i++)
                {
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
                        DocumentLayer.Surface[j, i] = colorBgra;
                        // Increase source pixel
                        ptr++;
                    }
                }

                // Add it if we can!
                Workspace.GetActiveDocumentWorkspace().AddLayer(DocumentLayer);
            });

            Workspace.RunOnMainThread((Action)delegate
            {
                // Make a new layer
                var DocumentLayer = Layer.CreateBackgroundLayer(dst.Width, dst.Height, "Alpha Channel");

                // Assign pixel data in the layer
                for (int i = rect.Top; i < rect.Bottom; i++)
                {
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
                        DocumentLayer.Surface[j, i] = colorBgra;
                        // Increase source pixel
                        ptr++;
                    }
                }

                // Add it if we can!
                Workspace.GetActiveDocumentWorkspace().AddLayer(DocumentLayer);
            });

            // Set back to source
            dst.CopySurface(src);
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            return new PropertyCollection(new List<Property>());
        }
    }
}
