using System;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace TgcViewer.Utils.Sound
{
    /// <summary>
    /// Reproduce archivos WAV con dos mecanismos:
    /// - Loops (musica, pasos): SoundPlayer.PlayLooping() - sin gaps, seamless.
    ///   Solo inicia si no esta ya en loop (evita reinicios por llamada per-frame).
    /// - Efectos de un solo uso: MCI con alias unico - verdaderamente concurrente.
    ///   MCI usa waveOut handles independientes de PlaySound().
    /// </summary>
    public class TgcStaticSound
    {
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciSendString(string cmd, StringBuilder retVal, int retLen, IntPtr cb);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(string longPath, StringBuilder shortPath, int bufSize);

        private static int _counter = 0;
        private static string NewAlias() => "snd" + Interlocked.Increment(ref _counter);

        private string loadedPath;
        private string shortPath;
        private SoundPlayer loopPlayer;
        private bool _isLooping = false;

        public object SoundBuffer => null;
        public bool IsPlaying => _isLooping;

        public TgcStaticSound() { }

        public void loadSound(string soundPath, int volume) => loadSound(soundPath);

        public void loadSound(string soundPath)
        {
            stop();
            loadedPath = soundPath;
            var sb = new StringBuilder(260);
            shortPath = GetShortPathName(soundPath, sb, 260) > 0 ? sb.ToString() : soundPath;
            loopPlayer?.Dispose();
            loopPlayer = new SoundPlayer(soundPath);
            try { loopPlayer.Load(); } catch { }
        }

        public void play(bool playLoop)
        {
            if (loadedPath == null) return;

            if (playLoop)
            {
                // Guard: only start if not already looping.
                // Personaje.cs calls play(true) EVERY FRAME while moving.
                // Without this guard, SoundPlayer restarts each frame = "rain" sound.
                if (!_isLooping)
                {
                    _isLooping = true;
                    try
                    {
                        loopPlayer?.Stop();
                        loopPlayer = new SoundPlayer(loadedPath);
                        loopPlayer.PlayLooping();
                    }
                    catch { _isLooping = false; }
                }
                // Already looping: do nothing, let SoundPlayer continue gaplessly
            }
            else
            {
                // Concurrent one-shot via MCI: own waveOut handle, never cancels loops
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
            _isLooping = false;
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
