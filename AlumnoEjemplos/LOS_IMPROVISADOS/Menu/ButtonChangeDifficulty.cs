using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Menu
{
    class ButtonChangeDifficulty : GameButton
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
            niveles.Add("Normal"); 
            niveles.Add("Hard");
            niveles.Add("Carnage");

            nivelSeleccionado = (int)GameConfig.Instance.dificultad;

            nivelSeleccionado--;
            seleccionarNiveliguiente();//QUE TRUCAZO NO?

            setearConfigGlobal();
        }

        private void setearConfigGlobal()
        {
            GameConfig.Instance.cambiarDificultad((GameConfig.difficulty)nivelSeleccionado);
            GameConfig.Instance.execute();

        }

        private void seleccionarNiveliguiente()
        {
            nivelSeleccionado++;
            if (nivelSeleccionado>=niveles.Count())
            {
                nivelSeleccionado=0;
            }
            base.init("boton" + niveles[nivelSeleccionado] + "Config", new Vector2(0.47f, 0.347f));
        }
    }
}
