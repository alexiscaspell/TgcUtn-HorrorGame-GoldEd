using System;
using SharpDX;
using SharpDX.Direct3D9;
using System.Windows.Forms;
using System.Drawing;
using TgcViewer.Utils.TgcGeometry;

namespace TgcViewer.Utils
{
    public class TgcD3dDevice
    {
        private Direct3D d3d;
        private Device d3dDevice = null;

        public Device D3dDevice
        {
            get { return d3dDevice; }
        }

        Control panel3d;
        readonly System.Drawing.Color DEFAULT_CLEAR_COLOR = System.Drawing.Color.FromArgb(255, 78, 129, 179);
        public static readonly Material DEFAULT_MATERIAL = new Material();

        public static readonly int DIRECTX_MULTITEXTURE_COUNT = 8;

        public static float fieldOfViewY = FastMath.ToRad(45.0f);
        public static float aspectRatio = -1f;
        public static float zNearPlaneDistance = 1f;
        public static float zFarPlaneDistance = 10000f;

        private System.Drawing.Color clearColor;
        public System.Drawing.Color ClearColor
        {
            get { return clearColor; }
            set { clearColor = value; }
        }

        public TgcD3dDevice(Control panel3d)
        {
            this.panel3d = panel3d;
            aspectRatio = (float)this.panel3d.Width / this.panel3d.Height;

            d3d = new Direct3D();

            var caps = d3d.GetDeviceCaps(0, DeviceType.Hardware);
            CreateFlags flags;

            Console.WriteLine("Max primitive count:" + caps.MaxPrimitiveCount);

            if (caps.DeviceCaps.HasFlag(DeviceCaps.HWTransformAndLight))
                flags = CreateFlags.HardwareVertexProcessing;
            else
                flags = CreateFlags.SoftwareVertexProcessing;

            var d3dpp = new PresentParameters
            {
                BackBufferFormat = Format.Unknown,
                SwapEffect = SwapEffect.Discard,
                Windowed = true,
                EnableAutoDepthStencil = true,
                AutoDepthStencilFormat = Format.D24S8,
                PresentationInterval = PresentInterval.Immediate,
                MultiSampleType = MultisampleType.None
            };

            d3dDevice = new Device(d3d, 0, DeviceType.Hardware, panel3d.Handle, flags, d3dpp);
        }

        public void OnResetDevice(object sender, EventArgs e)
        {
            GuiController.Instance.onResetDevice();
        }

        internal void doResetDevice()
        {
            setDefaultValues();
            HighResolutionTimer.Instance.Reset();
        }

        internal void setDefaultValues()
        {
            // Projection matrix
            d3dDevice.SetTransform(TransformState.Projection,
                Matrix.PerspectiveFovLH(fieldOfViewY, aspectRatio, zNearPlaneDistance, zFarPlaneDistance));

            // Render states
            d3dDevice.SetRenderState(RenderState.SpecularEnable, false);
            d3dDevice.SetRenderState(RenderState.FillMode, (int)FillMode.Solid);
            d3dDevice.SetRenderState(RenderState.CullMode, (int)Cull.None);
            d3dDevice.SetRenderState(RenderState.ShadeMode, (int)ShadeMode.Gouraud);
            d3dDevice.SetRenderState(RenderState.MultisampleAntialias, true);
            d3dDevice.SetRenderState(RenderState.SlopeScaleDepthBias, -0.1f);
            d3dDevice.SetRenderState(RenderState.DepthBias, 0f);
            d3dDevice.SetRenderState(RenderState.ColorVertex, true);
            d3dDevice.SetRenderState(RenderState.Lighting, false);
            d3dDevice.SetRenderState(RenderState.ZEnable, true);
            d3dDevice.SetRenderState(RenderState.FogEnable, false);

            // Alpha blending
            d3dDevice.SetRenderState(RenderState.AlphaBlendEnable, false);
            d3dDevice.SetRenderState(RenderState.AlphaTestEnable, false);
            d3dDevice.SetRenderState(RenderState.AlphaRef, 100);
            d3dDevice.SetRenderState(RenderState.AlphaFunc, (int)Compare.Greater);
            d3dDevice.SetRenderState(RenderState.BlendOperation, (int)BlendOperation.Add);
            d3dDevice.SetRenderState(RenderState.SourceBlend, (int)Blend.SourceAlpha);
            d3dDevice.SetRenderState(RenderState.DestinationBlend, (int)Blend.InverseSourceAlpha);

            // Texture filtering
            d3dDevice.SetSamplerState(0, SamplerState.MinFilter, (int)TextureFilter.Linear);
            d3dDevice.SetSamplerState(0, SamplerState.MagFilter, (int)TextureFilter.Linear);
            d3dDevice.SetSamplerState(0, SamplerState.MipFilter, (int)TextureFilter.Linear);
            d3dDevice.SetSamplerState(1, SamplerState.MinFilter, (int)TextureFilter.Linear);
            d3dDevice.SetSamplerState(1, SamplerState.MagFilter, (int)TextureFilter.Linear);
            d3dDevice.SetSamplerState(1, SamplerState.MipFilter, (int)TextureFilter.Linear);

            // Disable all lights
            for (int i = 0; i < 8; i++)
                d3dDevice.EnableLight(i, false);

            GuiController.Instance.TexturesManager.clearAll();

            d3dDevice.Material = DEFAULT_MATERIAL;
            clearColor = DEFAULT_CLEAR_COLOR;
            d3dDevice.Indices = null;
        }

        internal void doClear()
        {
            var sdxColor = new ColorBGRA(clearColor.B, clearColor.G, clearColor.R, clearColor.A);
            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, sdxColor, 1.0f, 0);
            HighResolutionTimer.Instance.Set();
        }

        internal void resetWorldTransofrm()
        {
            d3dDevice.SetTransform(TransformState.World, Matrix.Identity);
        }

        internal void shutDown()
        {
            d3dDevice.Dispose();
            d3d.Dispose();
        }
    }
}
