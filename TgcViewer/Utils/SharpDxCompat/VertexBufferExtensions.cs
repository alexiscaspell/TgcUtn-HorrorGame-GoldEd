using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D9;

namespace TgcViewer.Utils.SharpDxCompat
{
    /// <summary>
    /// Extension method for IndexBuffer, replicating MDX's SetData API.
    /// </summary>
    public static class IndexBufferExtensions
    {
        public static void SetData<T>(this IndexBuffer ib, T[] data, int offsetToLock, LockFlags flags)
            where T : struct
        {
            var stream = ib.Lock(offsetToLock, 0, flags);
            try { stream.WriteRange(data); }
            finally { ib.Unlock(); }
        }
    }

    /// <summary>
    /// Extension methods for SharpDX Mesh, replicating MDX's SetVertexBufferData.
    /// </summary>
    public static class MeshExtensions
    {
        public static void SetVertexBufferData<T>(this Mesh mesh, T[] data, LockFlags flags)
            where T : struct
        {
            // SharpDX.Direct3D9 Mesh vertex buffer access via VertexBuffer
            using (var vb = new VertexBuffer(mesh.Device,
                data.Length * Marshal.SizeOf(typeof(T)), Usage.WriteOnly,
                VertexFormat.None, Pool.Default))
            {
                var stream = vb.Lock(0, 0, flags);
                try { stream.WriteRange(data); }
                finally { vb.Unlock(); }
            }
        }
    }


    /// <summary>
    /// Extension methods that replicate MDX's VertexBuffer.SetData API for SharpDX.
    /// Usage: vb.SetData(vertices, 0, LockFlags.None)
    /// </summary>
    public static class VertexBufferExtensions
    {
        public static void SetData<T>(this VertexBuffer vb, T[] data, int offsetToLock, LockFlags flags)
            where T : struct
        {
            var stream = vb.Lock(offsetToLock, 0, flags);
            try { stream.WriteRange(data); }
            finally { vb.Unlock(); }
        }

        /// <summary>
        /// MDX-compatible VertexBuffer factory: accepts typeof(T) + element count
        /// instead of SharpDX's byte-size constructor.
        /// Usage: VertexBufferExtensions.Create(typeof(CustomVertex.PositionTextured), 4, device, usage, format, pool)
        /// </summary>
        public static VertexBuffer Create(System.Type vertexType, int elementCount,
            Device device, Usage usage, VertexFormat format, Pool pool)
        {
            int sizeInBytes = Marshal.SizeOf(vertexType) * elementCount;
            return new VertexBuffer(device, sizeInBytes, usage, format, pool);
        }
    }
}
