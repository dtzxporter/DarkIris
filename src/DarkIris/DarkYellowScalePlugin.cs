using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkIris
{
    [EffectCategory(EffectCategory.Effect), PluginSupportInfo(typeof(PluginSupportInfo), DisplayName = "Dark Iris")]
    public class DarkYellowScalePlugin : PropertyBasedEffect
    {
        public static string StaticName
        {
            get
            {
                return "Patch yellow-scale normal";
            }
        }

        public static Image StaticIcon
        {
            get
            {
                return DarkIris.Properties.Resources.IrisIcon;
            }
        }

        public DarkYellowScalePlugin()
            : base(DarkYellowScalePlugin.StaticName, DarkYellowScalePlugin.StaticIcon, "DarkIris Tools", EffectFlags.ForceAliasedSelectionQuality)
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
            return (byte)iValue;
        }

        private unsafe void Render(Surface dst, Surface src, Rectangle rect)
        {
            
            // Loop for all of the scanlines
            for (int i = rect.Top; i < rect.Bottom; i++)
            {
                // Just stop now
                if (base.IsCancelRequested)
                {
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

                    // Calculate B value
                    float nx = 2 * Utilities.ColorByteToFloat(colorBgra.R) - 1;
                    float ny = 2 * Utilities.ColorByteToFloat(colorBgra.G) - 1;
                    float nz = 0.0f;

                    // Check if it can be averaged
                    if (1 - nx * nx - ny * ny > 0) nz = (float)Math.Sqrt(1 - nx * nx - ny * ny);

                    // Calculate the final value
                    float ResultBlue = Utilities.ClampFloat((nz + 1) / 2.0f, 0.0f, 1.0f);

                    // Set it to the swapped sequence, and also ensure that alpha is 255
                    colorBgra = ColorBgra.FromBgra(Utilities.ColorFloatToByte(ResultBlue), colorBgra.G, colorBgra.R, 255);
                    // Apply to destination
                    *ptr2 = colorBgra;
                    // Increase source pixel
                    ptr++;
                    // Increase dest pixel
                    ptr2++;
                }
            }
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            return new PropertyCollection(new List<Property>());
        }
    }
}
