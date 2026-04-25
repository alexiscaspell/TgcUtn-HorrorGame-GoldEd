using System;
using System.IO;
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

            try
            {
                GuiController.newInstance();
                GuiController gui = GuiController.Instance;
                gui.initGraphicsStandalone(this, panel3d);
                gui.executeExample(new EjemploAlumno());
            }
            catch (Exception ex)
            {
                LogCrash("INIT ERROR", ex);
                ApplicationRunning = false;
                return;
            }

            GuiController gui2 = GuiController.Instance;
            bool firstRender = true;

            // Game loop
            while (ApplicationRunning)
            {
                if (ContainsFocus || firstRender)
                {
                    firstRender = false;
                    try
                    {
                        gui2.render();
                    }
                    catch (Exception ex)
                    {
                        LogCrash("RENDER ERROR", ex);
                        ApplicationRunning = false;
                        break;
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(16);
                }

                Application.DoEvents();
            }
        }

        private static void LogCrash(string title, Exception ex)
        {
            try
            {
                string msg = title + ":\n" + ex.ToString();
                File.WriteAllText("crash.log", msg);
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch { }
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ApplicationRunning = false;
            try { GuiController.Instance.shutDown(); } catch { }
        }
    }
}
