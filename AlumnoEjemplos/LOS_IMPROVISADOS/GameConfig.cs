using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.LOS_IMPROVISADOS
{
    class GameConfig
    {
        #region Singleton
        private static volatile GameConfig instancia = null;

        public static GameConfig Instance
        {
            get
            { return newInstance(); }
        }

        internal static GameConfig newInstance()
        {
            if (instancia != null) { }
            else
            {
                instancia = new GameConfig();
            }
            return instancia;
        }

        #endregion

        public bool contadorFPSActivo;
        public enum difficulty { NORMAL, HARD, CARNAGE };
        public difficulty dificultad;
        public enum graphics { LOW, NORMAL, HIGH};
        public graphics calidadGraficos;

        public String mensajeFPS;
        internal int renderDistance;

        private GameConfig()
        {
            contadorFPSActivo = false;
            dificultad = difficulty.NORMAL;
            calidadGraficos = graphics.LOW;
            mensajeFPS = "";
            renderDistance = 5000;

    }

    public void execute()
        {
            mensajeFPS = contadorFPSActivo ? "FPS: " + HighResolutionTimer.Instance.FramesPerSecond : "";

            AnimatedBoss.Instance.cambiarDificultad(dificultad);

            switch (calidadGraficos)
            {//PENSE QUE IBA A PODER RENDERIZAR MAS COSAS PERO NO, HAY QUE ORDENARLAS SI SE SUBE LA DISTANCIA DE RENDERIZADO
                case graphics.LOW:
                    renderDistance = 5000;
                    break;
                case graphics.NORMAL:
                    renderDistance = 5035;
                     
                    break;
                case graphics.HIGH:
                    renderDistance = 5070;

                    break;

            }
        }

        internal void cambiarFPSCount(bool v)
        {
            contadorFPSActivo = v;
        }
        internal void cambiarDificultad(difficulty d)
        {
            dificultad = d;
        }
        internal void cambiarGraficos(graphics g)
        {
            calidadGraficos = g;
        }

    }
}
