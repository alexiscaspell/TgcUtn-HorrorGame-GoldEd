using System;
using System.Media;

namespace TgcViewer.Utils.Sound
{
    /// <summary>
    /// Reproduce un archivo WAV usando System.Media.SoundPlayer.
    /// Cada instancia maneja su propio canal independiente.
    /// </summary>
    public class TgcStaticSound
    {
        private SoundPlayer player;
        private string loadedPath;

        public object SoundBuffer => null;

        public TgcStaticSound() { }

        public void loadSound(string soundPath, int volume)
        {
            loadSound(soundPath);
        }

        public void loadSound(string soundPath)
        {
            try
            {
                loadedPath = soundPath;
                player?.Dispose();
                player = new SoundPlayer(soundPath);
                player.Load();
            }
            catch { }
        }

        public void play(bool playLoop)
        {
            try
            {
                if (player == null) return;
                if (playLoop)
                    player.PlayLooping();
                else
                    player.Play();
            }
            catch { }
        }

        public void play()
        {
            play(false);
        }

        public void stop()
        {
            try { player?.Stop(); } catch { }
        }

        public void dispose()
        {
            try { player?.Dispose(); player = null; } catch { }
        }
    }
}
