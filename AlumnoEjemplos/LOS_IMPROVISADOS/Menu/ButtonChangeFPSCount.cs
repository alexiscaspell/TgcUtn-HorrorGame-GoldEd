using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Menu
{
    class ButtonChangeFPSCount : GameButton
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
            niveles.Add("False");
            niveles.Add("True");

            nivelSeleccionado--;
            seleccionarNiveliguiente();//QUE TRUCAZO NO?

            nivelSeleccionado = GameConfig.Instance.contadorFPSActivo ? 1 : 0;

            setearConfigGlobal();
        }

        private void setearConfigGlobal()
        {
            GameConfig.Instance.cambiarFPSCount(niveles[nivelSeleccionado]=="True");
            GameConfig.Instance.execute();

        }

        private void seleccionarNiveliguiente()
        {
            nivelSeleccionado++;
            if (nivelSeleccionado>=niveles.Count())
            {
                nivelSeleccionado=0;
            }
            base.init("boton" + niveles[nivelSeleccionado] + "Config", new Vector2(0.44f, 0.58f));
        }
    }
}
