// Compilation stub: TgcSkeletalLoader/TgcSkeletalMesh use deep Mesh API that changed in SharpDX.
// Only compiled in the Docker build check; NOT in TgcViewer.csproj.
using System;
using SharpDX;
using SharpDX.Direct3D9;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;

namespace TgcViewer.Utils.TgcSkeletalAnimation
{
    public partial class TgcSkeletalMesh : ITransformObject, IDisposable
    {
        public enum MeshRenderType { DIFFUSE_MAP, DIFFUSE_MAP_AND_LIGHTMAP, VERTEX_COLOR }

        public string Name { get; set; }
        public Effect Effect { get; set; }
        public string Technique { get; set; }
        public bool Visible { get; set; } = true;
        public bool AlphaBlend { get; set; }
        public TgcBoundingBox BoundingBox { get; private set; }
        public TgcSkeletalMesh.MeshRenderType RenderType { get; set; }
        public bool AutoUpdateBoundingBox { get; set; } = true;
        public TgcSkeletalBone getBoneByName_typed(string name) { return null; }

        public Matrix Transform { get; set; } = Matrix.Identity;
        public bool AutoTransformEnable { get; set; } = true;
        public Vector3 Position { get { return Transform.TranslationVector; } set { } }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; } = Vector3.One;

        public void move(Vector3 v) { }
        public void move(float x, float y, float z) { }
        public void moveOrientedY(float movement) { }
        public void getPosition(Vector3 pos) { }
        public void rotateX(float angle) { }
        public void rotateY(float angle) { }
        public void rotateZ(float angle) { }

        public TgcSkeletalMesh() { }
        public void initializeAnimations() { }
        public void playAnimation(string animationName, bool loop = true) { }
        public void stopAnimation() { }
        public bool isAnimating() { return false; }
        public void updateAnimation(float elapsedTime) { }
        public void animateAndRender(float elapsedTime) { }
        public void buildSkletonMesh() { }
        public TgcSkeletalBone getBoneByName(string name) { return null; }
        public void render() { }
        public void dispose() { }
        public void Dispose() { }
        public void setTexture(TgcTexture texture) { }
        public TgcSkeletalMesh createMeshInstance(string newName) { return new TgcSkeletalMesh(); }
    }

    public partial class TgcSkeletalLoader
    {
        public TgcSkeletalMesh loadMeshAndAnimationsFromFile(string filepath, string mediaDir,
            string[] animationsFile) { return new TgcSkeletalMesh(); }
        public TgcSkeletalMesh loadMeshFromFile(string filepath, string mediaDir) { return new TgcSkeletalMesh(); }
        public void loadAnimationFromFile(TgcSkeletalMesh mesh, string animationFile) { }
    }
}
