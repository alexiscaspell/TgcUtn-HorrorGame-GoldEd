using SharpDX;
using System;

namespace AlumnoEjemplos.LOS_IMPROVISADOS
{
    class ButtonConfig : GameButton
    {
        public override void execute(EjemploAlumno app, GameMenu menu)
        {
            FactoryMenu factory = FactoryMenu.Instance;

            factory.setMenuAnterior(menu);

            app.menuActual = factory.menuConfig();
        }

        public void init()
        {
            base.init("botonConfig", new Vector2(0.72f, 0.58f));
        }
    }
}