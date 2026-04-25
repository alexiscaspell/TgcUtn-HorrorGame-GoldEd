using System;
using System.Windows.Forms;
using TgcViewer;
using AlumnoEjemplos.LOS_IMPROVISADOS;

namespace HorrorGame
{
    /// <summary>
    /// Ventana principal de HorrorGame: inicializa el engine TgcViewer en modo standalone
    /// y lanza EjemploAlumno directamente sin el picker de ejemplos.
    /// </summary>
    public class GameForm : Form
    {
        private Panel panel3d;
        public static bool ApplicationRunning = false;

        public GameForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Escape from Hospital";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            panel3d = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.Black
            };
            this.Controls.Add(panel3d);

            this.Load += GameForm_Load;
            this.FormClosing += GameForm_FormClosing;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            this.Show();
            panel3d.Focus();

            ApplicationRunning = true;

            GuiController.newInstance();
            GuiController gui = GuiController.Instance;
            gui.initGraphicsStandalone(this, panel3d);

            // Lanzar el juego directamente sin picker de ejemplos
            gui.executeExample(new EjemploAlumno());

            // Game loop: igual que MainForm pero sin la UI del viewer
            while (ApplicationRunning)
            {
                if (ContainsFocus || gui.FullScreenPanel != null && gui.FullScreenPanel.ContainsFocus)
                {
                    gui.render();
                }
                else
                {
                    System.Threading.Thread.Sleep(16);
                }

                Application.DoEvents();
            }
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ApplicationRunning = false;
            try { GuiController.Instance.shutDown(); } catch { }
        }
    }
}
