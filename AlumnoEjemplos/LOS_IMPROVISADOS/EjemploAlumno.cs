using TgcViewer.Example;
using TgcViewer;
using SharpDX;
using TgcViewer.Utils.Input;
using SharpDX.Direct3D9;
using AlumnoEjemplos.LOS_IMPROVISADOS;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace AlumnoEjemplos.LOS_IMPROVISADOS
{
    public class EjemploAlumno : TgcExample
    {
        private CamaraFPS camaraFPS;

        private Personaje personaje;

        private Mapa mapa; 
        
        private TgcStaticSound sonidoFondo;

        private AnimatedBoss bossAnimado;
        
        public bool playing;
        public GameMenu menuActual;
        public InputManager input;

        DiosMapa diosMapa;
        private FactoryMenu factoryMenu;
        private GameMenu menuPausa;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }
        
        public override string getName()
        {
            return "Los Improvisados";
        }
        
        public override string getDescription()
        {
            return "Escape from Hospital - Juego de terror en primera persona basado en juegos famosos como Amnesia, Outlast, Penumbra, etc";
        }


        public override void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            mapa = Mapa.Instance;
            mapa.init();

            camaraFPS = CamaraFPS.Instance;

            personaje = Personaje.Instance;

            Cursor.Hide();

            diosMapa = DiosMapa.Instance;//ESTO DEJARLO ANTES DE LA INSTANCIACION DEL BOSS!!!
            diosMapa.init(0.009f);//Quiero que mapee 100x100 ptos del mapa
            diosMapa.generarMatriz();//Genera matriz de vias del boss
            diosMapa.generarCaminos();

            bossAnimado = AnimatedBoss.Instance;
            bossAnimado.init(300f, new Vector3(1062, 0, 3020));

            sonidoFondo = new TgcStaticSound();
            sonidoFondo.loadSound(GuiController.Instance.AlumnoEjemplosDir + "Media\\Sonidos\\asd16.wav");
            sonidoFondo.play(true);

            factoryMenu = FactoryMenu.Instance;
            factoryMenu.setApplication(this);

            menuActual = factoryMenu.menuPrincipal();

            menuPausa = factoryMenu.menuPausa();

            input = InputManager.Initialize();

            playing = false;

            GameConfig.Instance.execute();

        }

        public override void render(float elapsedTime)
        {
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
            Device d3dDevice = GuiController.Instance.D3dDevice;

            Size screenSize = ScreenSizeClass.ScreenSize;
            Cursor.Position = new Point(screenSize.Width / 2, screenSize.Height / 2);        

            if (!playing)
            {
                menuActual.render();
            }

            else {

                if (input.Pausa())
                {
                    playing = false;
                    menuActual = menuPausa;
                    return;
                }

             d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, new SharpDX.ColorBGRA( Color.Black.B,  Color.Black.G,  Color.Black.R,  Color.Black.A), 1.0f, 0);

             //d3dDevice.Transform.Projection =  Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(45.0f),
               //                                TgcD3dDevice.aspectRatio, TgcD3dDevice.zNearPlaneDistance, 5000);

            personaje.calcularColisiones();

            camaraFPS.render();

            //bossAnimado.update();
            //bossAnimado.render();

            personaje.update();

            if(personaje.perdioOGano())
            {
                sonidoFondo.stop();
            }

            //personaje.configPosProcesado.renderizarPosProcesado(elapsedTime, 2);
            
            GameOver.Instance.render(elapsedTime);
            
            }

            GuiController.Instance.Text3d.drawText(GameConfig.Instance.mensajeFPS, 0, 0, Color.Yellow);

        }

        public override void close()
        {
            mapa.dispose();

            bossAnimado.dispose();
        }
        
    }
}
