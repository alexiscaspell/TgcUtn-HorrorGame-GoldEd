using System;
using SharpDX.DirectSound;

namespace TgcViewer.Utils.Sound
{
    /// <summary>
    /// Herramienta para reproducir un sonido WAV estático.
    /// SharpDX.DirectSound stub — audio playback disabled pending full API migration.
    /// </summary>
    public class TgcStaticSound
    {
        public object SoundBuffer => null;

        public TgcStaticSound() { }

        public void loadSound(string soundPath, int volume) { }
        public void loadSound(string soundPath) { }
        public void play(bool playLoop) { }
        public void play() { }
        public void stop() { }
        public void dispose() { }
    }
}
