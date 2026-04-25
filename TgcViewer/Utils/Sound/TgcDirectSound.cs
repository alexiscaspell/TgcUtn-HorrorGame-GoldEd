using System;
using System.Collections.Generic;
using System.Text;
using SharpDX.DirectSound;
using TgcViewer.Utils.TgcSceneLoader;
using SharpDX;

namespace TgcViewer.Utils.Sound
{
    /// <summary>
    /// Herramienta para manipular el Device de DirectSound
    /// </summary>
    public class TgcDirectSound
    {

        DirectSound dsDevice;
        /// <summary>
        /// Device de DirectSound
        /// </summary>
        public DirectSound DsDevice
        {
            get { return dsDevice; }
        }

        private SoundListener3D listener3d;
        /// <summary>
        /// Representa el objeto central del universo 3D que escucha todos los sonidos.
        /// </summary>
        public SoundListener3D Listener3d
        {
            get { return listener3d; }
        }

        private ITransformObject listenerTracking;
        /// <summary>
        /// Objeto al cual el SoundListener3D va a seguir para variar su posición en cada cuadro.
        /// </summary>
        public ITransformObject ListenerTracking
        {
            get { return listenerTracking; }
            set { listenerTracking = value; }
        }

        private SoundBuffer primaryBuffer;

        public TgcDirectSound()
        {
            dsDevice = new DirectSound();
            dsDevice.SetCooperativeLevel(GuiController.Instance.MainForm.Handle, CooperativeLevel.Normal);

            SoundBufferDescription primaryBufferDesc = new SoundBufferDescription();
            primaryBufferDesc.Flags = BufferFlags.Control3D | BufferFlags.PrimaryBuffer;
            primaryBuffer = new SoundBuffer(dsDevice, primaryBufferDesc);
            listener3d = new SoundListener3D(primaryBuffer);
            listener3d.Position = new Vector3(0f, 0f, 0f);
        }

        /// <summary>
        /// Actualiza la posición del SoundListener3D en base al ListenerTracking
        /// </summary>
        internal void updateListener3d()
        {
            if (listenerTracking != null)
            {
                listener3d.Position = listenerTracking.Position;
            }
        }
    }
}
