// Stubs for geometry types excluded from Docker build check wrapper.
// NOT included in TgcViewer.csproj (real implementations exist in separate files).
using SharpDX;
using SharpDX.Direct3D9;
using TgcViewer.Utils.TgcSceneLoader;

namespace TgcViewer.Utils.TgcGeometry
{
    public partial class TgcFrustum
    {
        public SharpDX.Plane[] FrustumPlanes { get; private set; } = new SharpDX.Plane[6];
        public SharpDX.Plane NearPlane   => FrustumPlanes[4];
        public SharpDX.Plane FarPlane    => FrustumPlanes[5];
        public SharpDX.Plane LeftPlane   => FrustumPlanes[0];
        public SharpDX.Plane RightPlane  => FrustumPlanes[1];
        public SharpDX.Plane TopPlane    => FrustumPlanes[2];
        public SharpDX.Plane BottomPlane => FrustumPlanes[3];
        public TgcFrustum() { }
        public void updateVolume(SharpDX.Matrix viewMatrix, SharpDX.Matrix projectionMatrix) { }
        public bool sphereInFrustum(TgcBoundingSphere sphere) { return true; }
        public bool boxInFrustum(TgcBoundingBox aabb) { return true; }
        public void renderFrustum(SharpDX.Direct3D9.Device device) { }
    }

    public partial class TgcBox : ITransformObject, System.IDisposable
    {
        public Matrix Transform { get; set; } = Matrix.Identity;
        public bool AutoTransformEnable { get; set; } = true;
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; } = Vector3.One;
        public System.Drawing.Color Color { get; set; }
        public bool Enabled { get; set; } = true;
        public bool AlphaBlend { get; set; }
        public TgcBoundingBox BoundingBox { get; private set; }
        public SharpDX.Direct3D9.Effect Effect { get; set; }
        public string Technique { get; set; }
        public Vector3 Size { get; set; }
        public TgcBox() { }
        public static TgcBox fromExtremes(Vector3 pMin, Vector3 pMax) { return new TgcBox(); }
        public static TgcBox fromSize(Vector3 size) { return new TgcBox(); }
        public static TgcBox fromSize(Vector3 size, System.Drawing.Color color) { return new TgcBox(); }
        public static TgcBox fromSize(Vector3 size, System.Drawing.Color color, TgcTexture texture) { return new TgcBox(); }
        public static TgcBox fromSize(Vector3 center, Vector3 size, System.Drawing.Color color) { return new TgcBox(); }
        public void setTexture(TgcTexture t) { }
        public void setColor(System.Drawing.Color c) { }
        public TgcMesh toMesh(string name) { return new TgcMesh(); }
        public void render() { }
        public void dispose() { }
        public void Dispose() { }
        public void move(Vector3 v) { }
        public void move(float x, float y, float z) { }
        public void moveOrientedY(float m) { }
        public void getPosition(Vector3 pos) { }
        public void rotateX(float a) { }
        public void rotateY(float a) { }
        public void rotateZ(float a) { }
        public void updateValues() { }
    }

    public partial class TgcSphere : ITransformObject, System.IDisposable
    {
        public Matrix Transform { get; set; } = Matrix.Identity;
        public bool AutoTransformEnable { get; set; } = true;
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; } = Vector3.One;
        public System.Drawing.Color Color { get; set; }
        public float Radius { get; set; }
        public bool Enabled { get; set; } = true;
        public SharpDX.Direct3D9.Effect Effect { get; set; }
        public string Technique { get; set; }
        public TgcSphere() { }
        public TgcSphere(float radius, System.Drawing.Color color, Vector3 center) { }
        public void setTexture(TgcTexture t) { }
        public void render() { }
        public void dispose() { }
        public void Dispose() { }
        public void move(Vector3 v) { }
        public void move(float x, float y, float z) { }
        public void moveOrientedY(float m) { }
        public void getPosition(Vector3 pos) { }
        public void rotateX(float a) { }
        public void rotateY(float a) { }
        public void rotateZ(float a) { }
        public void updateValues() { }
    }

    public partial class TgcPlaneWall : ITransformObject, System.IDisposable
    {
        public enum Orientations { XYplane, XZplane, YZplane }
        public Matrix Transform { get; set; } = Matrix.Identity;
        public bool AutoTransformEnable { get; set; } = true;
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; } = Vector3.One;
        public bool AlphaBlend { get; set; }
        public SharpDX.Direct3D9.Effect Effect { get; set; }
        public string Technique { get; set; }
        public static TgcPlaneWall fromSize(Vector3 size, Orientations orientation, System.Drawing.Color color, TgcTexture texture) { return new TgcPlaneWall(); }
        public TgcPlaneWall() { }
        public TgcPlaneWall(Vector3 center, Vector3 size, Orientations orientation, TgcTexture texture, float tileU, float tileV) { }
        public TgcPlaneWall(Vector3 n, Vector3 o, float tileU, float tileV, TgcTexture t, System.Drawing.Color color) { }
        public void setTexture(TgcTexture t) { }
        public void render() { }
        public void dispose() { }
        public void Dispose() { }
        public void move(Vector3 v) { }
        public void move(float x, float y, float z) { }
        public void moveOrientedY(float m) { }
        public void getPosition(Vector3 pos) { }
        public void rotateX(float a) { }
        public void rotateY(float a) { }
        public void rotateZ(float a) { }
        public void updateValues() { }
    }

    public partial class TgcAxisLines
    {
        public bool Enable { get; set; }
        public TgcAxisLines(Device d3dDevice) { }
        public void render() { }
        public void dispose() { }
    }

    public partial class TgcConvexPolyhedron
    {
        public TgcConvexPolyhedron() { }
        public SharpDX.Plane[] Planes { get; set; } = new SharpDX.Plane[0];
        public Vector3[] BoundingVertices { get; set; } = new Vector3[0];
    }

    public partial class TgcConvexPolygon
    {
        public TgcConvexPolygon() { }
        public Vector3[] Points { get; set; } = new Vector3[0];
        public Vector3[] BoundingVertices { get; set; } = new Vector3[0];
        public System.Drawing.Color Color { get; set; }
        public void updateValues() { }
        public void render() { }
        public void dispose() { }
    }

    public partial class TgcTriangle
    {
        public Vector3 A { get; set; }
        public Vector3 B { get; set; }
        public Vector3 C { get; set; }
        public SharpDX.Plane Plane { get; set; }
        public TgcBoundingSphere BoundingSphere { get; set; }
        public System.Drawing.Color Color { get; set; }
        public bool Enabled { get; set; } = true;
        public TgcTriangle(Vector3 a, Vector3 b, Vector3 c) { A = a; B = b; C = c; }
        public TgcTriangle() { }
        public void updateValues() { }
        public void render() { }
        public void dispose() { }
    }
}
