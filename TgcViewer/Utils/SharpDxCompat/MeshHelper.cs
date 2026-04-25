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
        private static MethodInfo _createMeshFvfMethod;
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
                        if (m.Name == "CreateMeshFVF") _createMeshFvfMethod = m;
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

        /// <summary>
        /// Computes an FVF code from a VertexElement[] declaration.
        /// D3DXCreateMesh takes VertexElement (singular) via reflection which causes boxing issues.
        /// D3DXCreateMeshFVF takes a simple int (FVF) which works perfectly with reflection.
        /// </summary>
        private static VertexFormat DeclarationToFvf(VertexElement[] declaration)
        {
            VertexFormat fvf = VertexFormat.None;
            int texCount = 0;
            foreach (var elem in declaration)
            {
                if (elem.Stream == 0xFF) break; // VertexDeclarationEnd
                switch (elem.Usage)
                {
                    case DeclarationUsage.Position: fvf |= VertexFormat.Position; break;
                    case DeclarationUsage.Normal:   fvf |= VertexFormat.Normal; break;
                    case DeclarationUsage.Color:    fvf |= VertexFormat.Diffuse; break;
                    case DeclarationUsage.TextureCoordinate: texCount++; break;
                }
            }
            // Encode texture set count in FVF bits 8-11 (D3DFVF_TECOUNTn = n << 8)
            if (texCount > 0) fvf |= (VertexFormat)(texCount << 8);
            return fvf;
        }

        /// <summary>
        /// Creates a D3DX Mesh using D3DXCreateMeshFVF (avoids VertexElement[] boxing issue).
        /// FVF is computed from the declaration array. Vertex layouts match standard D3D FVF.
        /// </summary>
        public static Mesh CreateMesh(Device device, int numFaces, int numVertices,
            MeshFlags flags, VertexElement[] declaration)
        {
            EnsureInitialized();
            if (_createMeshFvfMethod == null)
                throw new InvalidOperationException(
                    "D3DX9.CreateMeshFVF not found. Ensure SharpDX.Direct3D9 4.2.0 net45 is referenced.");

            // D3DXCreateMeshFVF takes: (int numFaces, int numVertices, int options, int fvf, Device device, out Mesh mesh)
            // All simple types — no VertexElement[] boxing issue!
            VertexFormat fvf = DeclarationToFvf(declaration);
            var args = new object[] { numFaces, numVertices, (int)flags, (int)fvf, device, null };
            _createMeshFvfMethod.Invoke(null, args);
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
            if (_getDeclarationMethod == null) return Array.Empty<VertexElement>();

            var parms = _getDeclarationMethod.GetParameters();
            try
            {
                if (parms.Length == 0)
                {
                    // Returns VertexElement[] directly
                    var result = _getDeclarationMethod.Invoke(mesh, null);
                    if (result is VertexElement[] arr) return arr;
                }
                else if (parms.Length == 1)
                {
                    // GetDeclaration(VertexElement[] decl) fills a pre-allocated array in-place
                    // D3DXBaseMesh::GetDeclaration takes MAX_FVF_DECL_SIZE (64) element array
                    var decl = new VertexElement[64];
                    var args = new object[] { decl };
                    _getDeclarationMethod.Invoke(mesh, args);
                    // args[0] may have been updated (out param) or decl was filled in-place
                    var filled = args[0] as VertexElement[] ?? decl;
                    // Trim at VertexDeclarationEnd (Stream=0xFF)
                    int count = filled.Length;
                    for (int i = 0; i < filled.Length; i++)
                    {
                        if (filled[i].Stream == 0xFF) { count = i + 1; break; }
                    }
                    var trimmed = new VertexElement[count];
                    Array.Copy(filled, trimmed, count);
                    return trimmed;
                }
            }
            catch { }
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

        /// <summary>
        /// Reads a typed array from a DataStream (helper for LockVertexBuffer/LockIndexBuffer results).
        /// Disposes the DataStream after reading.
        /// </summary>
        public static T[] ReadRange<T>(SharpDX.DataStream stream, int count) where T : struct
        {
            if (stream == null) return new T[count];
            using (stream)
            {
                stream.Position = 0;
                return stream.ReadRange<T>(count);
            }
        }

        /// <summary>
        /// Reads vertex data from a BaseMesh using LockVertexBuffer(int, out IntPtr).
        /// BaseMesh.LockVertexBuffer in SharpDX net45 returns an IntPtr, not DataStream.
        /// </summary>
        private static T[] ReadPtrRange<T>(IntPtr ptr, int count) where T : struct
        {
            T[] result = new T[count];
            if (ptr == IntPtr.Zero) return result;
            int stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            for (int i = 0; i < count; i++)
                result[i] = (T)System.Runtime.InteropServices.Marshal.PtrToStructure(
                    new IntPtr(ptr.ToInt64() + i * stride), typeof(T));
            return result;
        }

        /// <summary>
        /// Reads vertex data from a BaseMesh via VertexBuffer.Lock() (DataStream approach).
        /// Avoids the unsafe pointer trick that left dataPtr=0 and broke bounding boxes.
        /// </summary>
        public static T[] LockVertexBufferData<T>(this BaseMesh mesh, LockFlags flags, int count) where T : struct
        {
            try
            {
                using (var vb = mesh.VertexBuffer)
                {
                    if (vb == null) return new T[count];
                    SharpDX.DataStream ds = vb.Lock(0, 0, flags);
                    try
                    {
                        ds.Position = 0;
                        return ds.ReadRange<T>(count);
                    }
                    finally
                    {
                        vb.Unlock();
                    }
                }
            }
            catch { return new T[count]; }
        }

        /// <summary>
        /// Reads index data from a BaseMesh via IndexBuffer.Lock() (DataStream approach).
        /// </summary>
        public static T[] LockIndexBufferData<T>(this BaseMesh mesh, LockFlags flags, int count) where T : struct
        {
            try
            {
                using (var ib = mesh.IndexBuffer)
                {
                    if (ib == null) return new T[count];
                    SharpDX.DataStream ds = ib.Lock(0, 0, flags);
                    try
                    {
                        ds.Position = 0;
                        return ds.ReadRange<T>(count);
                    }
                    finally
                    {
                        ib.Unlock();
                    }
                }
            }
            catch { return new T[count]; }
        }

        /// <summary>
        /// Clones a Mesh. SharpDX CloneMesh signature: (int options, VertexElement first, Device, out Mesh).
        /// The declaration parameter is VertexElement (singular) - the first element of the declaration array.
        /// </summary>
        public static Mesh CloneMeshFromDecl(this BaseMesh mesh, MeshFlags flags, VertexElement[] declaration, Device device)
        {
            Mesh result;
            // SharpDX net45 CloneMesh takes single VertexElement (pointer to array start)
            mesh.CloneMesh((int)flags, declaration[0], device, out result);
            return result;
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
