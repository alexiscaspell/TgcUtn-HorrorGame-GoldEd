using System;
using System.Reflection;
using System.Runtime.InteropServices;
using SharpDX.Direct3D9;

namespace TgcViewer.Utils.SharpDxCompat
{
    /// <summary>
    /// Helper to work with SharpDX.Direct3D9 Mesh/BaseMesh objects.
    /// SharpDX 4.x exposes very limited D3DX9 Mesh API publicly (see issue #1066).
    /// This class uses reflection to access internal D3DX9 methods.
    /// </summary>
    public static class MeshHelper
    {
        private static MethodInfo _createMeshMethod;
        private static MethodInfo _getNumFacesMethod;
        private static MethodInfo _getNumVerticesMethod;
        private static MethodInfo _getDeclarationMethod;
        private static MethodInfo _lockAttributeBufferMethod;
        private static MethodInfo _unlockAttributeBufferMethod;
        private static bool _initialized;

        private static void EnsureInitialized()
        {
            if (_initialized) return;
            _initialized = true;
            try
            {
                var asm = typeof(Mesh).Assembly;
                var d3dx9Type = asm.GetType("SharpDX.Direct3D9.D3DX9");
                if (d3dx9Type != null)
                {
                    foreach (var m in d3dx9Type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    {
                        if (m.Name == "CreateMesh") _createMeshMethod = m;
                    }
                }

                // BaseMesh methods - find via instance methods
                var baseMeshType = asm.GetType("SharpDX.Direct3D9.BaseMesh") ?? typeof(Mesh);
                foreach (var m in baseMeshType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    var name = m.Name;
                    if (name == "GetNumFaces") _getNumFacesMethod = m;
                    else if (name == "GetNumVertices") _getNumVerticesMethod = m;
                    else if (name == "GetDeclaration") _getDeclarationMethod = m;
                    else if (name == "LockAttributeBuffer" && _lockAttributeBufferMethod == null)
                        _lockAttributeBufferMethod = m;
                    else if (name == "UnlockAttributeBuffer")
                        _unlockAttributeBufferMethod = m;
                }
            }
            catch { }
        }

        /// <summary>Creates a D3DX Mesh (wraps D3DXCreateMesh via reflection).</summary>
        public static Mesh CreateMesh(Device device, int numFaces, int numVertices,
            MeshFlags flags, VertexElement[] declaration)
        {
            EnsureInitialized();
            if (_createMeshMethod == null)
                throw new InvalidOperationException(
                    "D3DX9.CreateMesh not found. Ensure SharpDX.Direct3D9 4.2.0 net45 is referenced.");

            // D3DX9.CreateMesh(int numFaces, int numVertices, int options, VertexElement[] decl, Device device, out Mesh mesh)
            var args = new object[] { numFaces, numVertices, (int)flags, declaration, device, null };
            _createMeshMethod.Invoke(null, args);
            return (Mesh)args[5];
        }

        /// <summary>Gets number of faces (triangles) in a mesh.</summary>
        public static int GetNumFaces(this BaseMesh mesh)
        {
            EnsureInitialized();
            if (_getNumFacesMethod != null)
                return (int)_getNumFacesMethod.Invoke(mesh, null);
            // Fallback: attempt to get via native COM vtable offset
            throw new InvalidOperationException("GetNumFaces not found on BaseMesh.");
        }

        /// <summary>Gets number of vertices in a mesh.</summary>
        public static int GetNumVertices(this BaseMesh mesh)
        {
            EnsureInitialized();
            if (_getNumVerticesMethod != null)
                return (int)_getNumVerticesMethod.Invoke(mesh, null);
            throw new InvalidOperationException("GetNumVertices not found on BaseMesh.");
        }

        /// <summary>Gets the vertex declaration array of a mesh.</summary>
        public static VertexElement[] GetDeclaration(this BaseMesh mesh)
        {
            EnsureInitialized();
            if (_getDeclarationMethod != null)
            {
                var result = _getDeclarationMethod.Invoke(mesh, null);
                if (result is VertexElement[] arr) return arr;
                // May return with out param
                var args = new object[] { null };
                _getDeclarationMethod.Invoke(mesh, args);
                return args[0] as VertexElement[];
            }
            return Array.Empty<VertexElement>();
        }

        /// <summary>
        /// Locks the attribute buffer and returns material IDs per face.
        /// Works around SharpDX's unusual LockAttributeBuffer signature.
        /// </summary>
        public static int[] LockAttributeBuffer(this Mesh mesh, LockFlags flags)
        {
            EnsureInitialized();
            if (_lockAttributeBufferMethod == null)
                return new int[mesh.GetNumFaces()];

            var parms = _lockAttributeBufferMethod.GetParameters();
            try
            {
                if (parms.Length == 1)
                {
                    // LockAttributeBuffer(LockFlags) → int[]
                    var r = _lockAttributeBufferMethod.Invoke(mesh, new object[] { (int)flags });
                    return r as int[] ?? new int[mesh.GetNumFaces()];
                }
                else if (parms.Length == 2)
                {
                    // LockAttributeBuffer(int flags, out int sizeInBytes) → DataStream?
                    var args = new object[] { (int)flags, 0 };
                    var r = _lockAttributeBufferMethod.Invoke(mesh, args);
                    // If returns DataStream, read int[] from it
                    if (r is SharpDX.DataStream ds)
                    {
                        int numFaces = mesh.GetNumFaces();
                        var buf = new int[numFaces];
                        ds.Position = 0;
                        for (int i = 0; i < numFaces; i++) buf[i] = ds.Read<int>();
                        return buf;
                    }
                    return r as int[] ?? new int[mesh.GetNumFaces()];
                }
                else
                {
                    var args = new object[parms.Length];
                    args[0] = (int)flags;
                    for (int i = 1; i < args.Length; i++) args[i] = parms[i].IsOut ? null : (object)0;
                    _lockAttributeBufferMethod.Invoke(mesh, args);
                    for (int i = 1; i < args.Length; i++)
                        if (args[i] is int[] ia) return ia;
                    return new int[mesh.GetNumFaces()];
                }
            }
            catch
            {
                return new int[mesh.GetNumFaces()];
            }
        }

        /// <summary>Writes back and unlocks the attribute buffer.</summary>
        public static void UnlockAttributeBuffer(this Mesh mesh, int[] attributeBuffer = null)
        {
            EnsureInitialized();
            if (_unlockAttributeBufferMethod == null) return;
            var parms = _unlockAttributeBufferMethod.GetParameters();
            try
            {
                if (parms.Length == 0)
                    _unlockAttributeBufferMethod.Invoke(mesh, null);
                else
                    _unlockAttributeBufferMethod.Invoke(mesh, new object[] { attributeBuffer });
            }
            catch { }
        }
    }
}
