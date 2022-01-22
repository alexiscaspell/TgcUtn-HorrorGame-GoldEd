using Microsoft.DirectX;
using System.Diagnostics;

namespace AlumnoEjemplos.MiGrupo
{
    internal class ButtonExit : GameButton
    {
        public void init()
        {
            base.init("botonExit2", new Vector2(0.72f, 0.58f));
        }

        public override void execute(EjemploAlumno app, GameMenu menu)
        {
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.Kill();

        }
    }
}