using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Menu
{
    class ButtonChangeGraphics:GameButton
    {

        private List<String> niveles;
        private int nivelSeleccionado = 0;
        public override void execute(EjemploAlumno app, GameMenu menu)
        {
            FactoryMenu factory = FactoryMenu.Instance;

            factory.setMenuAnterior(menu);
            seleccionarNiveliguiente();
            setearConfigGlobal();

        }

        public void init()
        {
            niveles = new List<String>();
            niveles.Add("Low");
            niveles.Add("Normal"); 
            niveles.Add("High");
            nivelSeleccionado = (int)GameConfig.Instance.calidadGraficos;

            nivelSeleccionado--;
            seleccionarNiveliguiente();//QUE TRUCAZO NO?

            setearConfigGlobal();
        }

        private void setearConfigGlobal()
        {
            GameConfig.Instance.cambiarGraficos((GameConfig.graphics)nivelSeleccionado);
            GameConfig.Instance.execute();

        }

        private void seleccionarNiveliguiente()
        {
            nivelSeleccionado++;
            if (nivelSeleccionado>=niveles.Count())
            {
                nivelSeleccionado=0;
            }
            base.init("boton" + niveles[nivelSeleccionado] + "Config", new Vector2(0.45f, 0.145f));
        }
    }
}
