using System;
using SharpDX;
using SharpDX.DirectSound;

namespace TgcViewer.Utils.Sound
{
    /// <summary>
    /// Herramienta para reproducir un sonido WAV en 3D.
    /// SharpDX.DirectSound stub — audio playback disabled pending full API migration.
    /// </summary>
    public class Tgc3dSound
    {
        public object SoundBuffer => null;
        public object Buffer3d => null;

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
