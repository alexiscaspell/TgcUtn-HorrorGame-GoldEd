using System;
using System.IO;
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
        static int spriteCallCount = 0;
        static int logEvery = 1; // log first N calls, then every 600

        static void SLog(string msg)
        {
            try
            {
                string path = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(
                        System.Reflection.Assembly.GetEntryAssembly()?.Location ?? ""), "sprite.log");
                File.AppendAllText(path, $"[{DateTime.Now:HH:mm:ss.fff}] {msg}\n");
            }
            catch { }
        }

        public TgcDrawer2D()
        {
            this.d3dDevice = GuiController.Instance.D3dDevice;
            SLog($"TgcDrawer2D ctor: creating Sprite, device={d3dDevice?.GetHashCode()}");
            try
            {
                dxSprite = new Sprite(d3dDevice);
                SLog($"Sprite created OK: {dxSprite?.GetHashCode()}");
            }
            catch (Exception ex)
            {
                SLog($"Sprite creation FAILED: {ex.Message}");
                throw;
            }
        }

        public void beginDrawSprite()
        {
            spriteCallCount++;
            bool shouldLog = spriteCallCount <= 5 || spriteCallCount % 600 == 0;
            if (shouldLog) SLog($"beginDrawSprite #{spriteCallCount}");
            try
            {
                dxSprite.Begin(SpriteFlags.AlphaBlend);
                if (shouldLog) SLog($"  Begin OK");
            }
            catch (Exception ex)
            {
                SLog($"  Begin FAILED: {ex.Message} HResult={ex.HResult:X8}");
                throw;
            }
        }

        public void endDrawSprite()
        {
            bool shouldLog = spriteCallCount <= 5 || spriteCallCount % 600 == 0;
            try
            {
                dxSprite.Flush();
                if (shouldLog) SLog($"  Flush OK");
            }
            catch (Exception ex) { SLog($"  Flush FAILED: {ex.Message}"); }
            try
            {
                dxSprite.End();
                if (shouldLog) SLog($"  End OK");
            }
            catch (Exception ex) { SLog($"  End FAILED: {ex.Message}"); throw; }
        }

        public void drawSprite(TgcSprite sprite)
        {
            bool shouldLog = spriteCallCount <= 5 || spriteCallCount % 600 == 0;
            var tex = sprite?.Texture?.D3dTexture;
            var mat = sprite?.TransformationMatrix;
            if (shouldLog)
                SLog($"  drawSprite: tex={tex?.GetHashCode() ?? -1}, " +
                     $"texNull={tex == null}, " +
                     $"scale=({sprite?.Scaling.X:F2},{sprite?.Scaling.Y:F2}), " +
                     $"pos=({sprite?.Position.X:F0},{sprite?.Position.Y:F0}), " +
                     $"transform.M11={mat?.M11:F3} M22={mat?.M22:F3} M41={mat?.M41:F0} M42={mat?.M42:F0}");
            try
            {
                dxSprite.Transform = sprite.TransformationMatrix;
                var c = sprite.Color;
                dxSprite.Draw(tex, new SharpDX.ColorBGRA(c.R, c.G, c.B, c.A));
                if (shouldLog) SLog($"  Draw OK");
            }
            catch (Exception ex)
            {
                SLog($"  Draw FAILED: {ex.Message} HResult={ex.HResult:X8}");
                throw;
            }
        }
    }
}
