using SharpDX;
using System.Diagnostics;

namespace AlumnoEjemplos.LOS_IMPROVISADOS
{
    internal class ButtonExit : GameButton
    {
        public void init()
        {
            base.init("botonExit2", new Vector2(0.72f, 0.72f));
        }

        public override void execute(EjemploAlumno app, GameMenu menu)
        {
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.Kill();

        }
    }
}