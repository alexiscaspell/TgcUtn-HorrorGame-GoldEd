using System;
using SharpDX.Direct3D9;
using System.Drawing;

namespace TgcViewer.Utils._2D
{
    public class TgcText2d
    {
        SharpDX.Direct3D9.Font d3dFont;
        public SharpDX.Direct3D9.Font D3dFont => d3dFont;

        private System.Drawing.Color color;
        public System.Drawing.Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private System.Drawing.Rectangle rectangle;
        public System.Drawing.Point Position
        {
            get { return rectangle.Location; }
            set { rectangle.Location = value; }
        }
        public System.Drawing.Size Size
        {
            get { return rectangle.Size; }
            set { rectangle.Size = value; }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private FontDrawFlags format;
        public FontDrawFlags Format
        {
            get { return format; }
            set { format = value; }
        }

        public enum TextAlign { LEFT, RIGHT, CENTER }
        private TextAlign align;
        public TextAlign Align
        {
            get { return align; }
            set { changeTextAlign(value); }
        }

        public TgcText2d()
        {
            changeTextAlign(TextAlign.CENTER);
            changeFont(TgcDrawText.VERDANA_10);
            color = System.Drawing.Color.Black;

            var vp = GuiController.Instance.D3dDevice.Viewport;
            rectangle = new System.Drawing.Rectangle(0, 0, vp.Width, vp.Height);
        }

        public void render()
        {
            Sprite sprite = GuiController.Instance.Text3d.TextSprite;
            sprite.Begin(SpriteFlags.AlphaBlend);
            var rc = new SharpDX.Mathematics.Interop.RawRectangle(
                rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            d3dFont.DrawText(sprite, text, rc, format,
                new SharpDX.ColorBGRA(color.B, color.G, color.R, color.A));
            sprite.End();
        }

        public void changeFont(System.Drawing.Font font)
        {
            var desc = new FontDescription
            {
                Height         = font.Height,
                FaceName       = font.Name,
                Weight         = font.Bold ? FontWeight.Bold : FontWeight.Normal,
                Italic         = font.Italic,
                CharacterSet   = FontCharacterSet.Default,
                OutputPrecision = FontPrecision.Default,
                Quality        = FontQuality.Default,
                PitchAndFamily = FontPitchAndFamily.Default | FontPitchAndFamily.DontCare,
            };
            d3dFont = new SharpDX.Direct3D9.Font(GuiController.Instance.D3dDevice, desc);
        }

        private void changeTextAlign(TextAlign align)
        {
            this.align = align;
            FontDrawFlags fAlign = 0;
            switch (align)
            {
                case TextAlign.LEFT:   fAlign = FontDrawFlags.Left;   break;
                case TextAlign.RIGHT:  fAlign = FontDrawFlags.Right;  break;
                case TextAlign.CENTER: fAlign = FontDrawFlags.Center; break;
            }
            format = FontDrawFlags.NoClip | FontDrawFlags.ExpandTabs | FontDrawFlags.WordBreak | fAlign;
        }

        public void dispose() { }
    }
}
