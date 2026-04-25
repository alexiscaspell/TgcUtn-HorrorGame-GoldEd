using System;
using System.Reflection;
using SharpDX.Direct3D9;
using TgcViewer.Utils.SharpDxCompat;

namespace TgcViewer.Utils.SharpDxCompat
{
    /// <summary>
    /// Helper to create D3DX Mesh objects in SharpDX.Direct3D9.
    /// SharpDX exposes D3DXCreateMesh as an internal method (MeshHelper.CreateMesh),
    /// so we access it via reflection as recommended by the SharpDX community
    /// (see: https://github.com/sharpdx/SharpDX/issues/1066).
    /// </summary>
    public static class MeshHelper
    {
        private static MethodInfo _createMeshMethod;
        private static bool _initialized;

        private static void EnsureInitialized()
        {
            if (_initialized) return;
            _initialized = true;
            try
            {
                var asm = typeof(Mesh).Assembly;
                var d3dx9Type = asm.GetType("SharpDX.Direct3D9.D3DX9");
                if (d3dx9Type == null) return;
                // MeshHelper.CreateMesh(int numFaces, int numVertices, int options,
                //   VertexElement[] declaration, Device device, out Mesh mesh)
                _createMeshMethod = d3dx9Type.GetMethod(
                    "CreateMesh",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            }
            catch { }
        }

        /// <summary>
        /// Creates a D3DX Mesh (wraps D3DXCreateMesh via reflection).
        /// </summary>
        public static Mesh CreateMesh(Device device, int numFaces, int numVertices,
            MeshFlags flags, VertexElement[] declaration)
        {
            EnsureInitialized();
            if (_createMeshMethod == null)
                throw new InvalidOperationException(
                    "MeshHelper.CreateMesh not found in SharpDX.Direct3D9 assembly. " +
                    "Ensure SharpDX.Direct3D9 4.2.0 net45 is referenced.");

            var args = new object[] { numFaces, numVertices, (int)flags, declaration, device, null };
            _createMeshMethod.Invoke(null, args);
            return (Mesh)args[5];
        }
    }
}
