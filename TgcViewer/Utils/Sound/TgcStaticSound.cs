using System;
using System.Collections.Generic;
using System.Text;
using SharpDX.DirectSound;

namespace TgcViewer.Utils.Sound
{
    /// <summary>
    /// Herramienta para reproducir un sonido WAV esttico
    /// </summary>
    public class TgcStaticSound
    {
        private SoundBuffer soundBuffer;
        /// <summary>
        /// Buffer con la informacin del sonido cargado
        /// </summary>
        public SoundBuffer SoundBuffer
        {
            get { return soundBuffer; }
        }


        public TgcStaticSound()
        {
        }

        /// <summary>
        /// Carga un archivo WAV de audio, indicando el volumen del mismo
        /// </summary>
        /// <param name="soundPath">Path del archivo WAV</param>
        /// <param name="volume">Volumen del mismo</param>
        public void loadSound(string soundPath, int volume)
        {
            try
            {
                dispose();

                SoundBufferDescription bufferDescription = new SoundBufferDescription();
                if (volume != -1)
                {
                    bufferDescription.ControlVolume = true;
                }

                soundBuffer = new SoundBuffer(GuiController.Instance.DirectSound.DsDevice, bufferDescription);

                if (volume != -1)
                {
                    soundBuffer.Volume = volume;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar sonido esttico WAV: " + soundPath, ex);
            }
        }

        /// <summary>
        /// Carga un archivo WAV de audio, con el volumen default del mismo
        /// </summary>
        /// <param name="soundPath">Path del archivo WAV</param>
        public void loadSound(string soundPath)
        {
            loadSound(soundPath, -1);
        }

        /// <summary>
        /// Reproduce el sonido, indicando si se hace con Loop.
        /// Si ya se est reproduciedo, no vuelve a empezar.
        /// </summary>
        /// <param name="playLoop">TRUE para reproducir en loop</param>
        public void play(bool playLoop)
        {
            soundBuffer.Play(0, playLoop ? PlayFlags.Looping : PlayFlags.None);
        }

        /// <summary>
        /// Reproduce el sonido, sin Loop.
        /// Si ya se est reproduciedo, no vuelve a empezar.
        /// </summary>
        public void play()
        {
            play(false);
        }

        /// <summary>
        /// Pausa la ejecucin del sonido.
        /// Si el sonido no se estaba ejecutando, no hace nada.
        /// Si se hace stop() y luego play(), el sonido continua desde donde haba dejado la ltima vez.
        /// </summary>
        public void stop()
        {
            soundBuffer.Stop();
        }

        /// <summary>
        /// Liberar recursos del sonido
        /// </summary>
        public void dispose()
        {
            if (soundBuffer != null && !soundBuffer.Disposed)
            {
                soundBuffer.Dispose();
                soundBuffer = null;
            }
        }

    }
}
