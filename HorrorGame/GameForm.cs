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

                // Keep the D3D device alive during the long init() by presenting
                // empty frames via a timer. Without this the device gets
                // D3DERR_DEVICELOST after a few minutes of inactivity.
                var keepAlive = new System.Threading.Timer(_ =>
                {
                    try { gui.D3dDevice?.Present(); } catch { }
                }, null, 0, 100);

                try
                {
                    gui.executeExample(new EjemploAlumno());
                }
                finally
                {
                    keepAlive.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogCrash("INIT ERROR", ex);
                ApplicationRunning = false;
                return;
            }

            GuiController gui2 = GuiController.Instance;

            // Game loop
            while (ApplicationRunning)
            {
                if (ContainsFocus)
                {
                    try
                    {
                        gui2.render();
                    }
                    catch (SharpDX.SharpDXException sEx) when (IsDeviceLost(sEx))
                    {
                        // D3DERR_DEVICELOST (screen saver, alt-tab, sleep)
                        // Wait until the device can be reset and recover
                        RecoverDevice(gui2);
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

        private static bool IsDeviceLost(SharpDX.SharpDXException ex)
        {
            // D3DERR_DEVICELOST = 0x88760868,  D3DERR_DEVICENOTRESET = 0x88760869
            return ex.HResult == unchecked((int)0x88760868)
                || ex.HResult == unchecked((int)0x88760869);
        }

        private static void RecoverDevice(GuiController gui)
        {
            // Poll until the device can be reset
            for (int attempts = 0; attempts < 300; attempts++)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
                try
                {
                    gui.onResetDevice();
                    return; // recovered
                }
                catch { }
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
