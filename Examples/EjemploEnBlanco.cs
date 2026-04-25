using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using SharpDX.Direct3D9;
using System.Drawing;
using SharpDX;
using TgcViewer.Utils.Modifiers;

namespace Examples
{
    /// <summary>
    /// Ejemplo en Blanco. Ideal para copiar y pegar cuando queres empezar a hacer tu propio ejemplo.
    /// </summary>
    public class EjemploEnBlanco : TgcExample
    {

        public override string getCategory()
        {
            return "Otros";
        }

        public override string getName()
        {
            return "Ejemplo en Blanco";
        }

        public override string getDescription()
        {
            return "Ejemplo en Blanco. Ideal para copiar y pegar cuando queres empezar a hacer tu propio ejemplo.";
        }

        public override void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

        }


        public override void render(float elapsedTime)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

        }

        public override void close()
        {

        }

    }
}
