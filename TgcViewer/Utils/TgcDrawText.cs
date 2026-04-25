using System;
using SharpDX.Direct3D9;
using System.Windows.Forms;
using System.Drawing;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;
using Font = System.Drawing.Font;

namespace TgcViewer.Utils
{
    public class TgcDrawText
    {
        public static readonly System.Drawing.Font VERDANA_10 =
            new System.Drawing.Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Pixel);

        SharpDX.Direct3D9.Font dxFont;

        Sprite textSprite;
        public Sprite TextSprite => textSprite;

        public TgcDrawText(Device d3dDevice)
        {
            textSprite = new Sprite(d3dDevice);
            var desc = new FontDescription
            {
                Height          = VERDANA_10.Height,
                FaceName        = VERDANA_10.Name,
                Weight          = FontWeight.Normal,
                Italic          = false,
                CharacterSet    = FontCharacterSet.Default,
                OutputPrecision = FontPrecision.Default,
                Quality         = FontQuality.Default,
                PitchAndFamily  = FontPitchAndFamily.Default | FontPitchAndFamily.DontCare,
            };
            dxFont = new SharpDX.Direct3D9.Font(d3dDevice, desc);
        }

        public void drawText(string text, int x, int y, System.Drawing.Color color)
        {
            if (string.IsNullOrEmpty(text)) return;
            try
            {
                // Font.DrawText must be called inside BeginScene but without our own Sprite.Begin.
                // Pass null for sprite so Font manages its own batch internally.
                var rc = new SharpDX.Mathematics.Interop.RawRectangle(x, y, x + 800, y + 50);
                dxFont.DrawText(null, text, rc, FontDrawFlags.Left,
                    new SharpDX.ColorBGRA(color.R, color.G, color.B, color.A));
            }
            catch { /* non-critical - FPS counter/debug text */ }
        }
    }
}
