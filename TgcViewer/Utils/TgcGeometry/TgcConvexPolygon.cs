using System;
using System.Collections.Generic;
using System.Text;
using SharpDX;
using SharpDX.Direct3D9;
using TgcViewer.Utils.TgcSceneLoader;
using System.Drawing;
using TgcViewer.Utils.Shaders;

namespace TgcViewer.Utils.TgcGeometry
{
    /// <summary>
    /// Representa un polgono convexo plano en 3D de una sola cara, compuesto
    /// por varios vrtices que lo delimitan.
    /// </summary>
    public class TgcConvexPolygon : IRenderObject
    {
        public TgcConvexPolygon()
        {
            this.enabled = true;
            this.alphaBlendEnable = false;
            this.color = Color.Purple;
        }


        private Vector3[] boundingVertices;
        /// <summary>
        /// Vertices que definen el contorno polgono.
        /// Estn dados en clockwise-order.
        /// </summary>
        public Vector3[] BoundingVertices
        {
            get { return boundingVertices; }
            set { boundingVertices = value; }
        }

        private bool enabled;
        /// <summary>
        /// Indica si la flecha esta habilitada para ser renderizada
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }


        # region Renderizado del poligono


        protected Effect effect;
        /// <summary>
        /// Shader del mesh
        /// </summary>
        public Effect Effect
        {
            get { return effect; }
            set { effect = value; }
        }

        protected string technique;
        /// <summary>
        /// Technique que se va a utilizar en el effect.
        /// Cada vez que se llama a render() se carga este Technique (pisando lo que el shader ya tenia seteado)
        /// </summary>
        public string Technique
        {
            get { return technique; }
            set { technique = value; }
        }


        VertexBuffer vertexBuffer;

        /// <summary>
        /// Actualizar valores de renderizado.
        /// Hay que llamarlo al menos una vez para poder hacer render()
        /// </summary>
        public void updateValues()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            //Crear VertexBuffer on demand
            if (vertexBuffer == null || vertexBuffer.Disposed)
            {
                vertexBuffer = new VertexBuffer(d3dDevice, boundingVertices.Length * System.Runtime.InteropServices.Marshal.SizeOf(typeof(CustomVertex.PositionColored)), Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
                //Shader
                this.effect = GuiController.Instance.Shaders.VariosShader;
                this.technique = TgcShaders.T_POSITION_COLORED;
            }

            //Crear como TriangleFan
            int c = color.ToArgb();
            CustomVertex.PositionColored[] vertices = new CustomVertex.PositionColored[boundingVertices.Length];
            for (int i = 0; i < boundingVertices.Length; i++)
            {
                vertices[i] = new CustomVertex.PositionColored(boundingVertices[i], c);
            }

            //Cargar vertexBuffer
            vertexBuffer.SetData(vertices, 0, LockFlags.None);
        }

        /// <summary>
        /// Renderizar el polgono
        /// </summary>
        public void render()
        {
            if (!enabled)
                return;

            Device d3dDevice = GuiController.Instance.D3dDevice;
            TgcTexture.Manager texturesManager = GuiController.Instance.TexturesManager;

            texturesManager.clear(0);
            texturesManager.clear(1);

            GuiController.Instance.Shaders.setShaderMatrixIdentity(this.effect);
            d3dDevice.VertexDeclaration = GuiController.Instance.Shaders.VdecPositionColored;
            effect.Technique = this.technique;
            d3dDevice.SetStreamSource(0, vertexBuffer, 0);

            //Renderizar RenderFarm
            effect.Begin(0);
            effect.BeginPass(0);
            d3dDevice.DrawPrimitives(PrimitiveType.TriangleFan, 0, boundingVertices.Length - 2);
            effect.EndPass();
            effect.End();
        }

        /// <summary>
        /// Liberar recursos del polgono
        /// </summary>
        public void dispose()
        {
            if (vertexBuffer != null && !vertexBuffer.Disposed)
            {
                vertexBuffer.Dispose();
            }
        }

        public Vector3 Position
        {
            //Lo correcto sera calcular el centro, pero con un extremo es suficiente.
            get { return boundingVertices[0]; }
        }

        Color color;
        /// <summary>
        /// Color del polgono
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private bool alphaBlendEnable;
        /// <summary>
        /// Habilita el renderizado con AlphaBlending para los modelos
        /// con textura o colores por vrtice de canal Alpha.
        /// Por default est deshabilitado.
        /// </summary>
        public bool AlphaBlendEnable
        {
            get { return alphaBlendEnable; }
            set { alphaBlendEnable = value; }
        }


        # endregion

    }
}
