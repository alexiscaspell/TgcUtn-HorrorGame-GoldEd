using System;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace TgcViewer.Utils.Sound
{
    /// <summary>
    /// Reproduce archivos WAV con dos mecanismos:
    /// - Loops (música, pasos): SoundPlayer.PlayLooping() — sin gaps, seamless.
    /// - Efectos de un solo uso: MCI con alias único — verdaderamente concurrente.
    ///   MCI usa waveOut handles independientes de PlaySound(), por lo que
    ///   no cancela sonidos en loop ni otros efectos simultáneos.
    /// </summary>
    public class TgcStaticSound
    {
        // ??? MCI (for non-looping concurrent effects) ??????????????????????
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciSendString(string command, StringBuilder retVal, int retLen, IntPtr cb);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(string longPath, StringBuilder shortPath, int bufSize);

        private static int _counter = 0;
        private static string NewAlias() => "snd" + Interlocked.Increment(ref _counter);

        // ??? Fields ?????????????????????????????????????????????????????????
        private string loadedPath;
        private string shortPath;

        // For looping sounds (SoundPlayer — seamless gapless loop)
        private SoundPlayer loopPlayer;

        public object SoundBuffer => null;
        public bool IsPlaying => loopPlayer != null;

        public TgcStaticSound() { }

        public void loadSound(string soundPath, int volume) => loadSound(soundPath);

        public void loadSound(string soundPath)
        {
            stop();
            loadedPath = soundPath;
            var sb = new StringBuilder(260);
            shortPath = GetShortPathName(soundPath, sb, 260) > 0 ? sb.ToString() : soundPath;

            // Pre-load loop player (fast start when play(true) is called)
            loopPlayer?.Dispose();
            loopPlayer = new SoundPlayer(soundPath);
            try { loopPlayer.Load(); } catch { }
        }

        public void play(bool playLoop)
        {
            if (loadedPath == null) return;

            if (playLoop)
            {
                // Seamless looping via SoundPlayer — no gaps between repetitions
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
                // Concurrent one-shot via MCI — doesn't cancel loops or other effects
                string alias = NewAlias();
                string path = shortPath ?? loadedPath;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        mciSendString($"open \"{path}\" type waveaudio alias {alias}", null, 0, IntPtr.Zero);
                        mciSendString($"play {alias} wait", null, 0, IntPtr.Zero);
                        mciSendString($"close {alias}", null, 0, IntPtr.Zero);
                    }
                    catch { }
                });
            }
        }

        public void play() => play(false);

        public void stop()
        {
            try { loopPlayer?.Stop(); } catch { }
        }

        public void dispose()
        {
            stop();
            try { loopPlayer?.Dispose(); loopPlayer = null; } catch { }
            loadedPath = null;
            shortPath = null;
        }
    }
}
