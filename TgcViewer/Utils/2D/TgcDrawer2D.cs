using System;
using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D9;
using TgcViewer;
using SharpDX;
using System.Drawing;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;
using Font = System.Drawing.Font;

namespace TgcViewer.Utils._2D
{
    /// <summary>
    /// Herramienta para dibujar Sprites 2D
    /// </summary>
    public class TgcDrawer2D
    {
        Device d3dDevice;
        Sprite dxSprite;

        public TgcDrawer2D()
        {
            this.d3dDevice = GuiController.Instance.D3dDevice;
            dxSprite = new Sprite(d3dDevice);
        }

        /// <summary>
        /// Iniciar render de Sprites
        /// </summary>
        public void beginDrawSprite()
        {
            dxSprite.Begin(SpriteFlags.AlphaBlend | SpriteFlags.SortDepthFrontToBack);
        }

        /// <summary>
        /// Finalizar render de Sprites
        /// </summary>
        public void endDrawSprite()
        {
            dxSprite.End();
        }

        /// <summary>
        /// Renderizar Sprite
        /// </summary>
        /// <param name="sprite">Sprite a dibujar</param>
        public void drawSprite(TgcSprite sprite)
        {
            dxSprite.Transform = sprite.TransformationMatrix;
            var c = sprite.Color;
            dxSprite.Draw(sprite.Texture.D3dTexture,
                new SharpDX.ColorBGRA(c.B, c.G, c.R, c.A));
        }

    }

}
