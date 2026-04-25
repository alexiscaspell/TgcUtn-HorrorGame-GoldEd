// Compilation stubs for Docker build check (net8.0-windows on Linux).
// SharpDX 4.2.0 (netstandard1.3) has limited Mesh API surface.
// Real implementations are in TgcMesh.cs, TgcSceneLoader.cs, etc.
// NOT included in TgcViewer.csproj.
using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D9;
using TgcViewer.Utils.TgcGeometry;

namespace TgcViewer.Utils.TgcSceneLoader
{
    public partial class TgcMesh : ITransformObject, IDisposable
    {
        public enum MeshRenderType { DIFFUSE_MAP, DIFFUSE_MAP_AND_LIGHTMAP, VERTEX_COLOR }

        public string Name { get; set; }
        public Effect Effect { get; set; }
        public string Technique { get; set; }
        public bool AutoTransformEnable { get; set; } = true;
        public bool Enabled { get; set; } = true;
        public bool AlphaBlend { get; set; }
        public bool AutoUpdateBoundingBox { get; set; } = true;

        public Matrix Transform { get; set; } = Matrix.Identity;
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; } = Vector3.One;

        public TgcBoundingBox BoundingBox { get; set; }
        public TgcTexture[] DiffuseMaps { get; set; }
        public TgcMesh.MeshRenderType RenderType { get; set; }

        public int NumberVertices { get; private set; }
        public int NumberTriangles { get; private set; }

        public void move(Vector3 v) { }
        public void move(float x, float y, float z) { }
        public void moveOrientedY(float movement) { }
        public void getPosition(Vector3 pos) { }
        public void rotateX(float angle) { }
        public void rotateY(float angle) { }
        public void rotateZ(float angle) { }

        public TgcMesh() { }
        public Vector3[] getVertexPositions() { return new Vector3[0]; }
        public void render() { }
        public void renderBoundingBox() { }
        public void dispose() { }
        public void Dispose() { }
        public void setTexture(TgcTexture texture) { }
        public void setTextures(TgcTexture[] textures) { }
        public TgcMesh createMeshInstance(string newName) { return new TgcMesh(); }
        public TgcMesh createMeshInstance(string newName, TgcTexture[] textures) { return new TgcMesh(); }
        public void updateMeshTransform() { }
        public void initializeTextureData(TgcTexture[] textures) { }
        public Effect setShaderProgram(string path) { return null; }
    }

    public partial class TgcSceneLoader
    {
        public TgcScene loadSceneFromFile(string filePath) { return new TgcScene("", filePath); }
        public TgcScene loadSceneFromFile(string filePath, string mediaPath) { return loadSceneFromFile(filePath); }
        public TgcScene loadSceneFromFile(string filePath, string mediaPath, string[] meshFilters) { return loadSceneFromFile(filePath); }
    }

    public partial class TgcTexture : IDisposable
    {
        public static TgcTexture createTexture(Device d3dDevice, string filePath) { return new TgcTexture(); }
        public static TgcTexture createTexture(string filePath) { return new TgcTexture(); }
        public Texture D3dTexture { get; private set; }
        public string FilePath { get; set; }
        public System.Drawing.Size Size { get; private set; }
        public bool IsDisposed { get; private set; }
        public void dispose() { }
        public void Dispose() { }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public class Manager
        {
            public void clearAll() { }
            public void clear(int stage) { }
            public void clear(int stage1, int stage2) { }
            public void set(int stage, TgcTexture texture) { }
            public void shaderSet(SharpDX.Direct3D9.Effect effect, string param, TgcTexture texture) { }
        }

        public class Pool
        {
            public TgcTexture get(string filePath) { return null; }
            public void clearAll() { }
        }
    }

    public partial class TgcSceneExporter { }
}

namespace TgcViewer.Utils.TgcKeyFrameLoader
{
    public partial class TgcKeyFrameMesh : TgcViewer.Utils.TgcSceneLoader.ITransformObject, IDisposable, TgcViewer.Utils.TgcSceneLoader.IRenderObject
    {
        public enum MeshRenderType { DIFFUSE_MAP, VERTEX_COLOR }
        public string Name { get; set; }
        public SharpDX.Matrix Transform { get; set; } = SharpDX.Matrix.Identity;
        public bool AutoTransformEnable { get; set; } = true;
        public SharpDX.Vector3 Position { get; set; }
        public SharpDX.Vector3 Rotation { get; set; }
        public SharpDX.Vector3 Scale { get; set; } = SharpDX.Vector3.One;
        public bool Enabled { get; set; } = true;
        public bool AlphaBlendEnable { get; set; }

        public void move(SharpDX.Vector3 v) { }
        public void move(float x, float y, float z) { }
        public void moveOrientedY(float m) { }
        public void getPosition(SharpDX.Vector3 pos) { }
        public void rotateX(float a) { }
        public void rotateY(float a) { }
        public void rotateZ(float a) { }

        public TgcKeyFrameMesh() { }
        public void render() { }
        public void dispose() { }
        public void Dispose() { }
        public void playAnimation(string name, bool loop = true) { }
        public void stopAnimation() { }
        public bool isAnimating() { return false; }
        public void updateAnimation(float dt) { }
    }

    public partial class TgcKeyFrameLoader
    {
        public TgcKeyFrameMesh loadMeshFromFile(string path, string mediaDir) { return new TgcKeyFrameMesh(); }
        public TgcKeyFrameMesh loadMeshAndAnimationsFromFile(string path, string mediaDir, string[] animFiles) { return new TgcKeyFrameMesh(); }
    }
}
