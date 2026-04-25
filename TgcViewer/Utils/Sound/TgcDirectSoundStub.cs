// Compilation stub: used ONLY by the Docker build check wrapper (net8.0-windows on Linux).
// NOT included in the real TgcViewer.csproj build.
// Provides minimal type signatures so GuiController and callers compile when
// TgcDirectSound.cs / Tgc3dSound.cs are excluded from the Linux check.
using SharpDX;
using TgcViewer.Utils.TgcSceneLoader;

namespace TgcViewer.Utils.Sound
{
    public partial class TgcStaticSound
    {
        public object SoundBuffer { get; private set; }
        public TgcStaticSound() { }
        public void loadSound(string soundPath, int volume) { }
        public void loadSound(string soundPath) { }
        public void play(bool playLoop) { }
        public void play() { }
        public void stop() { }
        public void dispose() { }
    }

    public partial class TgcDirectSound
    {
        public object DsDevice { get; private set; }
        public object Listener3d { get; private set; }
        public ITransformObject ListenerTracking { get; set; }
        public TgcDirectSound() { }
        internal void updateListener3d() { }
    }

    public partial class Tgc3dSound
    {
        public object SoundBuffer { get; private set; }
        public object Buffer3dObj { get; private set; }
        public Vector3 Position { get; set; }
        public float MinDistance { get; set; }
        public Tgc3dSound() { }
        public Tgc3dSound(string soundPath, Vector3 position) { }
        public void loadSound(string soundPath, int volume) { }
        public void loadSound(string soundPath) { }
        public void play(bool playLoop) { }
        public void play() { }
        public void stop() { }
        public void dispose() { }
    }
}
