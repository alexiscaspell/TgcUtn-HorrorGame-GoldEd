using System;
using System.Runtime.InteropServices;
using SharpDX.Direct3D9;
using TgcViewer.Utils.SharpDxCompat;

namespace TgcViewer.Utils.Shaders
{
    /// <summary>
    /// Utilidad para crear y renderizar un FullScreen Quad, útil para efectos de post-procesado
    /// </summary>
    public class TgcScreenQuad
    {
        VertexBuffer screenQuadVB;
        public VertexBuffer ScreenQuadVB
        {
            get { return screenQuadVB; }
            set { screenQuadVB = value; }
        }

        public TgcScreenQuad()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            CustomVertex.PositionTextured[] screenQuadVertices = new CustomVertex.PositionTextured[]
            {
                new CustomVertex.PositionTextured( -1,  1, 1, 0, 0),
                new CustomVertex.PositionTextured(  1,  1, 1, 1, 0),
                new CustomVertex.PositionTextured( -1, -1, 1, 0, 1),
                new CustomVertex.PositionTextured(  1, -1, 1, 1, 1)
            };

            int stride = Marshal.SizeOf(typeof(CustomVertex.PositionTextured));
            screenQuadVB = new VertexBuffer(d3dDevice, 4 * stride,
                Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
            screenQuadVB.SetData(screenQuadVertices, 0, LockFlags.None);
        }

        public void render(Effect effect)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            int stride = Marshal.SizeOf(typeof(CustomVertex.PositionTextured));
            d3dDevice.VertexFormat = CustomVertex.PositionTextured.Format;
            d3dDevice.SetStreamSource(0, screenQuadVB, 0, stride);

            effect.Begin(0);
            effect.BeginPass(0);
            d3dDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
        }

        public void dispose()
        {
            screenQuadVB.Dispose();
        }
    }
}
