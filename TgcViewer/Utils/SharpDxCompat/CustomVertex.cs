using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D9;

// Compatibility shim: replicates MDX's CustomVertex structs in the same namespace
// so existing code compiles unchanged after the MDX -> SharpDX.Direct3D9 migration.
namespace CustomVertex
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PositionTextured
    {
        public float X, Y, Z;
        public float Tu, Tv;

        public static readonly int Stride = Marshal.SizeOf(typeof(PositionTextured));
        public static readonly VertexFormat Format = VertexFormat.Position | VertexFormat.Texture1;

        public PositionTextured(Vector3 position, float u, float v)
        {
            X = position.X; Y = position.Y; Z = position.Z;
            Tu = u; Tv = v;
        }

        public PositionTextured(float x, float y, float z, float u, float v)
        {
            X = x; Y = y; Z = z; Tu = u; Tv = v;
        }

        public Vector3 Position
        {
            get => new Vector3(X, Y, Z);
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PositionNormalTextured
    {
        public float X, Y, Z;
        public float Nx, Ny, Nz;
        public float Tu, Tv;

        public static readonly int Stride = Marshal.SizeOf(typeof(PositionNormalTextured));
        public static readonly VertexFormat Format = VertexFormat.Position | VertexFormat.Normal | VertexFormat.Texture1;

        public PositionNormalTextured(Vector3 position, Vector3 normal, float u, float v)
        {
            X = position.X; Y = position.Y; Z = position.Z;
            Nx = normal.X; Ny = normal.Y; Nz = normal.Z;
            Tu = u; Tv = v;
        }

        public Vector3 Position
        {
            get => new Vector3(X, Y, Z);
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }

        public Vector3 Normal
        {
            get => new Vector3(Nx, Ny, Nz);
            set { Nx = value.X; Ny = value.Y; Nz = value.Z; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColored
    {
        public float X, Y, Z;
        public int Color;

        public static readonly int Stride = Marshal.SizeOf(typeof(PositionColored));
        public static readonly VertexFormat Format = VertexFormat.Position | VertexFormat.Diffuse;

        public PositionColored(Vector3 position, int color)
        {
            X = position.X; Y = position.Y; Z = position.Z;
            Color = color;
        }

        public PositionColored(float x, float y, float z, int color)
        {
            X = x; Y = y; Z = z; Color = color;
        }

        public Vector3 Position
        {
            get => new Vector3(X, Y, Z);
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PositionNormalColored
    {
        public float X, Y, Z;
        public float Nx, Ny, Nz;
        public int Color;

        public static readonly int Stride = Marshal.SizeOf(typeof(PositionNormalColored));
        public static readonly VertexFormat Format = VertexFormat.Position | VertexFormat.Normal | VertexFormat.Diffuse;

        public PositionNormalColored(Vector3 position, Vector3 normal, int color)
        {
            X = position.X; Y = position.Y; Z = position.Z;
            Nx = normal.X; Ny = normal.Y; Nz = normal.Z;
            Color = color;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TransformedColored
    {
        public float X, Y, Z, Rhw;
        public int Color;

        public static readonly int Stride = Marshal.SizeOf(typeof(TransformedColored));
        public static readonly VertexFormat Format = VertexFormat.PositionRhw | VertexFormat.Diffuse;

        public TransformedColored(float x, float y, float z, float rhw, int color)
        {
            X = x; Y = y; Z = z; Rhw = rhw; Color = color;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColoredTextured
    {
        public float X, Y, Z;
        public int Color;
        public float Tu, Tv;

        public static readonly int Stride = Marshal.SizeOf(typeof(PositionColoredTextured));
        public static readonly VertexFormat Format = VertexFormat.Position | VertexFormat.Diffuse | VertexFormat.Texture1;

        public PositionColoredTextured(Vector3 position, int color, float u, float v)
        {
            X = position.X; Y = position.Y; Z = position.Z;
            Color = color; Tu = u; Tv = v;
        }

        public PositionColoredTextured(float x, float y, float z, int color, float u, float v)
        {
            X = x; Y = y; Z = z; Color = color; Tu = u; Tv = v;
        }

        public Vector3 Position
        {
            get => new Vector3(X, Y, Z);
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TransformedTextured
    {
        public float X, Y, Z, Rhw;
        public float Tu, Tv;

        public static readonly int Stride = Marshal.SizeOf(typeof(TransformedTextured));
        public static readonly VertexFormat Format = VertexFormat.PositionRhw | VertexFormat.Texture1;

        public TransformedTextured(float x, float y, float z, float rhw, float u, float v)
        {
            X = x; Y = y; Z = z; Rhw = rhw; Tu = u; Tv = v;
        }
    }
}
