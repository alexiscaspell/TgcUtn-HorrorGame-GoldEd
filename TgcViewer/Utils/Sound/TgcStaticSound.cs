using System;
using System.Media;
using System.Threading;

namespace TgcViewer.Utils.Sound
{
    /// <summary>
    /// Reproduce archivos WAV usando System.Media.SoundPlayer.
    /// - Sonidos de un solo uso (play sin loop): cada llamada crea una instancia 
    ///   temporal en un thread del pool, permitiendo reproducción concurrente.
    /// - Sonidos en loop: usa una instancia reutilizable por TgcStaticSound.
    /// </summary>
    public class TgcStaticSound
    {
        private SoundPlayer loopPlayer;   // for PlayLooping() only
        private string loadedPath;
        private bool isLooping;

        public object SoundBuffer => null;

        public TgcStaticSound() { }

        public void loadSound(string soundPath, int volume) => loadSound(soundPath);

        public void loadSound(string soundPath)
        {
            try
            {
                stop();
                loadedPath = soundPath;
                // Pre-load the loopPlayer so looping starts immediately
                loopPlayer?.Dispose();
                loopPlayer = new SoundPlayer(soundPath);
                loopPlayer.Load();
            }
            catch { }
        }

        public void play(bool playLoop)
        {
            if (loadedPath == null) return;
            isLooping = playLoop;

            if (playLoop)
            {
                // Looping: use dedicated instance
                try
                {
                    loopPlayer?.Stop();
                    loopPlayer = new SoundPlayer(loadedPath);
                    loopPlayer.PlayLooping();
                }
                catch { }
            }
            else
            {
                // Non-looping: new instance per call ? concurrent playback
                // Each ThreadPool thread plays one sound then dies
                string path = loadedPath;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        using (var sp = new SoundPlayer(path))
                        {
                            sp.PlaySync(); // blocks thread until sound ends
                        }
                    }
                    catch { }
                });
            }
        }

        public void play() => play(false);

        public void stop()
        {
            isLooping = false;
            try { loopPlayer?.Stop(); } catch { }
        }

        public void dispose()
        {
            stop();
            try { loopPlayer?.Dispose(); loopPlayer = null; } catch { }
            loadedPath = null;
        }

        /// <summary>True si el sonido en loop está activo.</summary>
        public bool IsPlaying => isLooping;
    }
}
