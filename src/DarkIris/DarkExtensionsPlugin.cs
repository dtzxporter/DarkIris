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
using System.Windows.Forms;

namespace DarkIris
{
    [EffectCategory(EffectCategory.DoNotDisplay), PluginSupportInfo(typeof(PluginSupportInfo), DisplayName = "Dark Iris")]
    public class DarkExtensionsPlugin : PropertyBasedEffect
    {
        public static string StaticName { get { return "DarkExtensions"; } }

        public static Image StaticIcon { get { return DarkIris.Properties.Resources.IrisIcon; } }

        private void OnSaveToFile(object Sender, EventArgs e)
        {
            // Prepare to save the current layer to a file using the built-in FileType class
            var Instance = DarkNet.PaintForm.GetMainWindow();
            var Workspace = DarkNet.PaintWorkspace.GetWorkspaceFromInstance(Instance);

            var ActiveDocument = Workspace.GetActiveDocumentWorkspace();
            if (ActiveDocument == null) return;

            var ActiveLayer = ActiveDocument.GetActiveLayer() as BitmapLayer;
            if (ActiveLayer == null) return;

            // Ask paint.net to save this layer w/ the built-in filetypes
            ActiveDocument.SaveLayerAs(Workspace, ActiveLayer);
        }

        private void OnContentAware(object Sender, EventArgs e)
        {
            // TODO: Make this undoable...
            // TODO: Hook SelectionChanged and enable/disable the menu item
            var Instance = DarkNet.PaintForm.GetMainWindow();
            var Workspace = DarkNet.PaintWorkspace.GetWorkspaceFromInstance(Instance);

            var ActiveDocument = Workspace.GetActiveDocumentWorkspace();
            if (ActiveDocument == null) return;

            var ActiveLayer = ActiveDocument.GetActiveLayer() as BitmapLayer;
            if (ActiveLayer == null) return;

            var Selection = ActiveDocument.GetCurrentSelection();
            if (Selection.IsEmpty())
            {
                MessageBox.Show("Content Aware Fill must be used with a selection!", "DarkIris", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ask the matcher library to determine what we need
            Engine.PatchMatch.PerformPatchMatch(ref ActiveLayer, ref Selection, ref ActiveDocument);
        }

        public DarkExtensionsPlugin()
            : base(DarkExtensionsPlugin.StaticName, DarkExtensionsPlugin.StaticIcon, "DarkIris Tools", EffectFlags.ForceAliasedSelectionQuality)
        {
            if (!Global.AppInitialized)
            {
                Global.AppInitialized = true;

                // Fetch information on the current paint.net instance
                var Instance = DarkNet.PaintForm.GetMainWindow();
                var Workspace = DarkNet.PaintWorkspace.GetWorkspaceFromInstance(Instance);
                var Toolbar = Workspace.GetToolbar();
                var MainMenuStrip = Toolbar.GetMainMenu();

                // We want to add an item to the layers menu
                var LayersMenu = MainMenuStrip.GetLayersMenu();
                LayersMenu.DropDownItems.Insert(5, DarkNet.PaintMenuStrip.MakeMenuItem("Save To File...", DarkNet.PaintResources.GetImageResource("PaintDotNet.Icons.MenuFileSaveAsIcon.png"), OnSaveToFile));

                // We want to add an item to the edit menu
                var EditMenu = MainMenuStrip.GetEditMenu();
                EditMenu.DropDownItems.Insert(12, DarkNet.PaintMenuStrip.MakeMenuItem("Content Aware Fill", DarkNet.PaintResources.GetImageResource("PaintDotNet.Icons.MenuEditFillSelectionIcon.png"), OnContentAware, Keys.Control | Keys.Back));
            }
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
        }

        private unsafe void Render(Surface dst, Surface src, Rectangle rect)
        {
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            return new PropertyCollection(new List<Property>());
        }
    }
}
