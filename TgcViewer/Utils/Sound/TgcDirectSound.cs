using System;
using SharpDX.DirectSound;
using TgcViewer.Utils.TgcSceneLoader;
using SharpDX;

namespace TgcViewer.Utils.Sound
{
    /// <summary>
    /// Herramienta para manipular el Device de DirectSound.
    /// SharpDX.DirectSound stub — audio initialization disabled pending full API migration.
    /// </summary>
    public class TgcDirectSound
    {
        object dsDevice;
        public object DsDevice => dsDevice;

        object listener3d;
        public object Listener3d => listener3d;

        public ITransformObject ListenerTracking { get; set; }

        public TgcDirectSound()
        {
            // DirectSound initialization stubbed — SharpDX API migration pending
        }

        internal void updateListener3d() { }
    }
}
