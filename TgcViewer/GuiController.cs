using System;
using System.Windows.Forms;
using SharpDX.Direct3D9;
using TgcViewer.Utils;
using TgcViewer.Example;
using System.Drawing;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using SharpDX;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils.Fog;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.Shaders;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;
using Font = System.Drawing.Font;

namespace TgcViewer
{
    /// <summary>
    /// Controlador principal del motor gráfico (modo standalone / biblioteca).
    /// Usado directamente por HorrorGame.exe.
    /// </summary>
    public class GuiController
    {
        #region Singleton

        private static volatile GuiController instance;

        public static GuiController Instance
        {
            get { return instance; }
        }

        internal static void newInstance()
        {
            instance = new GuiController();
        }

        #endregion

        Control panel3d;
        TgcD3dDevice tgcD3dDevice;
        Logger logger;
        string examplesDir;
        string examplesMediaDir;
        string alumnoEjemplosDir;
        string alumnoEjemplosMediaDir;
        TgcExample currentExample;
        TgcFpsCamera fpsCamera;
        TgcRotationalCamera rotCamera;
        TgcThirdPersonCamera thirdPersonCamera;
        TgcAxisLines axisLines;
        TgcDrawText text3d;
        TgcUserVars userVars;
        TgcModifiers modifiers;
        bool fpsCounterEnable;
        TgcD3dInput tgcD3dInput;
        float elapsedTime;
        TgcFrustum frustum;
        TgcTexture.Pool texturesPool;
        TgcTexture.Manager texturesManager;
        TgcMp3Player mp3Player;
        TgcDirectSound directSound;
        TgcFog fog;
        TgcCamera currentCamera;
        bool customRenderEnabled;
        TgcDrawer2D drawer2D;
        TgcShaders shaders;

        private GuiController() { }

        #region Internal Methods

        /// <summary>
        /// Inicializa el motor gráfico en modo standalone (sin MainForm del viewer).
        /// Llamar desde HorrorGame.exe antes de ejecutar cualquier TgcExample.
        /// </summary>
        internal void initGraphicsStandalone(System.Windows.Forms.Form hostForm, Control panel3d)
        {
            this.panel3d = panel3d;
            panel3d.Focus();

            this.tgcD3dDevice = new TgcD3dDevice(panel3d);
            this.texturesManager = new TgcTexture.Manager();
            this.tgcD3dDevice.OnResetDevice(tgcD3dDevice.D3dDevice, null);

            this.texturesPool = new TgcTexture.Pool();
            this.logger = new Logger(new System.Windows.Forms.RichTextBox());
            this.text3d = new TgcDrawText(tgcD3dDevice.D3dDevice);
            this.tgcD3dInput = new TgcD3dInput(hostForm, panel3d);
            this.fpsCamera = new TgcFpsCamera();
            this.rotCamera = new TgcRotationalCamera();
            this.thirdPersonCamera = new TgcThirdPersonCamera();
            this.axisLines = new TgcAxisLines(tgcD3dDevice.D3dDevice);
            this.userVars = new TgcUserVars(new System.Windows.Forms.DataGridView());
            this.modifiers = new TgcModifiers(new System.Windows.Forms.Panel());
            this.elapsedTime = -1;
            this.frustum = new TgcFrustum();
            this.mp3Player = new TgcMp3Player();
            this.directSound = new TgcDirectSound();
            this.fog = new TgcFog();
            this.currentCamera = this.rotCamera;
            this.customRenderEnabled = false;
            this.drawer2D = new TgcDrawer2D();
            this.shaders = new TgcShaders();

            this.rotCamera.Enable = true;
            this.fpsCamera.Enable = false;
            this.thirdPersonCamera.Enable = false;
            this.fpsCounterEnable = true;
            this.axisLines.Enable = true;

            examplesDir = System.Environment.CurrentDirectory + "\\" + "Examples" + "\\";
            examplesMediaDir = examplesDir + "Media" + "\\";
            alumnoEjemplosDir = System.Environment.CurrentDirectory + "\\" + "AlumnoEjemplos" + "\\";
            alumnoEjemplosMediaDir = alumnoEjemplosDir + "AlumnoMedia" + "\\";

            this.shaders.loadCommonShaders();
        }

        /// <summary>
        /// Renderiza el frame actual del ejemplo en ejecución.
        /// </summary>
        internal void render()
        {
            Device d3dDevice = tgcD3dDevice.D3dDevice;
            elapsedTime = HighResolutionTimer.Instance.FrameTime;

            tgcD3dDevice.doClear();

            tgcD3dInput.update();

            if (currentCamera.Enable)
            {
                this.currentCamera.updateCamera();
                this.currentCamera.updateViewMatrix(d3dDevice);
            }

            frustum.updateVolume(
                d3dDevice.GetTransform(SharpDX.Direct3D9.TransformState.View),
                d3dDevice.GetTransform(SharpDX.Direct3D9.TransformState.Projection));

            texturesManager.clearAll();

            directSound.updateListener3d();

            if (customRenderEnabled)
            {
                if (currentExample != null)
                    currentExample.render(elapsedTime);
            }
            else
            {
                d3dDevice.BeginScene();

                TgcSprite pantalla = new TgcSprite();
                pantalla.Texture = TgcTexture.createTexture(
                    Instance.AlumnoEjemplosDir + "Media\\Menu\\imagenLoading.png");
                pantalla.Position = new Vector2(0, 0);

                Size screenSize = Instance.Panel3d.Size;
                Size textureSize = pantalla.Texture.Size;
                float widthScale = (float)screenSize.Width / textureSize.Width;
                float heightScale = (float)screenSize.Height / textureSize.Height;
                pantalla.Scaling = new Vector2(widthScale, heightScale);
                pantalla.Position = new Vector2(
                    FastMath.Max(screenSize.Width / 2 - textureSize.Width / 2, 0),
                    FastMath.Max(screenSize.Height / 2 - textureSize.Height / 2, 0));

                Instance.Drawer2D.beginDrawSprite();
                pantalla.render();
                Instance.Drawer2D.endDrawSprite();

                if (currentExample != null)
                    currentExample.render(elapsedTime);

                d3dDevice.EndScene();
            }

            d3dDevice.Present();
        }

        /// <summary>
        /// Inicia la ejecución de un TgcExample (para el juego, normalmente EjemploAlumno).
        /// </summary>
        internal void executeExample(TgcExample example)
        {
            stopCurrentExample();
            userVars.clearVars();
            modifiers.clear();
            resetDefaultConfig();
            fpsCamera.resetValues();
            rotCamera.resetValues();
            thirdPersonCamera.resetValues();

            try
            {
                example.init();
                this.currentExample = example;
                panel3d.Focus();
                Logger.log("Ejecutando ejemplo: " + example.getName(), Color.Blue);
            }
            catch (Exception e)
            {
                Logger.logError("Error en init() de ejemplo: " + example.getName(), e);
            }
        }

        /// <summary>
        /// Detiene el ejemplo actualmente en ejecución.
        /// </summary>
        internal void stopCurrentExample()
        {
            if (currentExample != null)
            {
                currentExample.close();
                tgcD3dDevice.resetWorldTransofrm();
                Logger.log("Ejemplo " + currentExample.getName() + " terminado");
                currentExample = null;
                elapsedTime = -1;
            }
        }

        /// <summary>
        /// Finaliza la ejecución de la aplicación.
        /// </summary>
        internal void shutDown()
        {
            if (currentExample != null)
                currentExample.close();
            tgcD3dDevice.shutDown();
            texturesPool.clearAll();
        }

        /// <summary>
        /// Reinicia el ejemplo actual.
        /// </summary>
        internal void resetCurrentExample()
        {
            if (currentExample != null)
            {
                TgcExample exampleBackup = currentExample;
                stopCurrentExample();
                executeExample(exampleBackup);
            }
        }

        /// <summary>
        /// Cuando el D3D Device se resetea.
        /// </summary>
        internal void onResetDevice()
        {
            TgcExample exampleBackup = currentExample;
            if (exampleBackup != null)
                stopCurrentExample();
            tgcD3dDevice.doResetDevice();
            if (exampleBackup != null)
                executeExample(exampleBackup);
        }

        internal void resetDefaultConfig()
        {
            this.axisLines.Enable = true;
            this.fpsCamera.Enable = false;
            this.rotCamera.Enable = true;
            this.currentCamera = this.rotCamera;
            this.thirdPersonCamera.Enable = false;
            this.fpsCounterEnable = true;
            tgcD3dDevice.setDefaultValues();
            this.mp3Player.closeFile();
            this.fog.resetValues();
            customRenderEnabled = false;
        }

        internal void focus3dPanel()
        {
            panel3d.Focus();
        }

        internal void printCurrentPosition()
        {
            Logger.log(fpsCamera.getPositionCode());
        }

        #endregion

        #region Public Properties

        public Device D3dDevice
        {
            get { return tgcD3dDevice.D3dDevice; }
        }

        public Logger Logger
        {
            get { return logger; }
        }

        public TgcFpsCamera FpsCamera
        {
            get { return fpsCamera; }
        }

        public TgcRotationalCamera RotCamera
        {
            get { return rotCamera; }
        }

        public TgcThirdPersonCamera ThirdPersonCamera
        {
            get { return thirdPersonCamera; }
        }

        public string ExamplesDir
        {
            get { return examplesDir; }
        }

        public string ExamplesMediaDir
        {
            get { return examplesMediaDir; }
        }

        public string AlumnoEjemplosDir
        {
            get { return alumnoEjemplosDir; }
        }

        public string AlumnoEjemplosMediaDir
        {
            get { return alumnoEjemplosMediaDir; }
        }

        public TgcDrawText Text3d
        {
            get { return text3d; }
        }

        public bool FpsCounterEnable
        {
            get { return fpsCounterEnable; }
            set { fpsCounterEnable = value; }
        }

        public TgcUserVars UserVars
        {
            get { return userVars; }
        }

        public TgcModifiers Modifiers
        {
            get { return modifiers; }
        }

        public TgcAxisLines AxisLines
        {
            get { return axisLines; }
        }

        public TgcD3dInput D3dInput
        {
            get { return tgcD3dInput; }
        }

        /// <summary>
        /// Mando Xbox/XInput (jugador 1). Verificar IsConnected antes de usar.
        /// </summary>
        public TgcViewer.Utils.Input.TgcGamepadInput GamepadInput
        {
            get { return tgcD3dInput.GamepadInput; }
        }

        public float ElapsedTime
        {
            get { return elapsedTime; }
        }

        public Control Panel3d
        {
            get { return panel3d; }
        }

        public TgcFrustum Frustum
        {
            get { return frustum; }
        }

        public TgcTexture.Pool TexturesPool
        {
            get { return texturesPool; }
        }

        public TgcTexture.Manager TexturesManager
        {
            get { return texturesManager; }
        }

        /// <summary>
        /// Configura la posición de la cámara.
        /// </summary>
        public void setCamera(Vector3 pos, Vector3 lookAt)
        {
            tgcD3dDevice.D3dDevice.SetTransform(
                SharpDX.Direct3D9.TransformState.View,
                Matrix.LookAtLH(pos, lookAt, new Vector3(0, 1, 0)));
        }

        public TgcMp3Player Mp3Player
        {
            get { return this.mp3Player; }
        }

        public TgcDirectSound DirectSound
        {
            get { return this.directSound; }
        }

        public bool FullScreenEnable
        {
            get { return false; }
            set { }
        }

        public TgcFog Fog
        {
            get { return fog; }
        }

        public Color BackgroundColor
        {
            get { return tgcD3dDevice.ClearColor; }
            set { tgcD3dDevice.ClearColor = value; }
        }

        public TgcCamera CurrentCamera
        {
            get { return currentCamera; }
            set { currentCamera = value; }
        }

        public bool CustomRenderEnabled
        {
            get { return customRenderEnabled; }
            set { customRenderEnabled = value; }
        }

        public TgcDrawer2D Drawer2D
        {
            get { return drawer2D; }
        }

        public TgcShaders Shaders
        {
            get { return shaders; }
        }

        #endregion
    }
}
