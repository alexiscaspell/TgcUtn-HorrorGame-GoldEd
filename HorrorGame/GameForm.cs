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

        private static readonly string LogFile = System.IO.Path.Combine(
            System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "startup.log");

        private static void Log(string msg)
        {
            try { File.AppendAllText(LogFile, $"[{DateTime.Now:HH:mm:ss.fff}] {msg}\n"); } catch { }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            File.WriteAllText(LogFile, $"[{DateTime.Now:HH:mm:ss.fff}] === HorrorGame startup ===\n");
            Log($"Form.Handle={Handle}, ContainsFocus={ContainsFocus}");

            this.Show();
            panel3d.Focus();
            Log($"After Show()+Focus(): ContainsFocus={ContainsFocus}, Focused={Focused}, panel3d.Focused={panel3d.Focused}");

            ApplicationRunning = true;

            try
            {
                GuiController.newInstance();
                GuiController gui = GuiController.Instance;
                Log("Calling initGraphicsStandalone...");
                gui.initGraphicsStandalone(this, panel3d);
                Log("initGraphicsStandalone OK");

                // Keep the D3D device alive AND process Windows messages during init().
                // Without this, the form freezes and loses focus (orange screen),
                // and the device gets D3DERR_DEVICELOST from inactivity.
                var keepAlive = new System.Threading.Timer(_ =>
                {
                    try
                    {
                        // Marshal to UI thread: process messages + keep device alive
                        BeginInvoke((Action)(() =>
                        {
                            gui.keepDeviceAlive();
                            Application.DoEvents();
                        }));
                    }
                    catch { }
                }, null, 0, 100);

                try
                {
                    Log("Calling executeExample (may take several minutes)...");
                    gui.executeExample(new EjemploAlumno());
                    Log("executeExample OK");
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

            // Regain focus after the long init() (form may have lost it)
            this.Activate();
            panel3d.Focus();
            Log($"Before game loop: ContainsFocus={ContainsFocus}, panel3d.Focused={panel3d.Focused}, Visible={Visible}, WindowState={WindowState}");

            int frameCount = 0;
            int noFocusCount = 0;

            // Game loop
            while (ApplicationRunning)
            {
                if (ContainsFocus)
                {
                    if (frameCount == 0) Log("First render frame executing...");
                    frameCount++;
                    try
                    {
                        gui2.render();
                    }
                    catch (Exception renderEx) when (IsDeviceLost(renderEx))
                    {
                        // D3DERR_DEVICELOST (screen saver, alt-tab, sleep)
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
                    noFocusCount++;
                    if (noFocusCount == 1 || noFocusCount % 60 == 0)
                        Log($"No focus (frame {noFocusCount}): ContainsFocus={ContainsFocus}, ActiveForm={(Form.ActiveForm?.GetType().Name ?? "null")}, panel3d.Focused={panel3d.Focused}");
                    System.Threading.Thread.Sleep(16);
                }

                Application.DoEvents();
            }
            Log($"Game loop ended. frames={frameCount}, noFocus={noFocusCount}");
        }

        private static bool IsDeviceLost(Exception ex)
        {
            // D3DERR_DEVICELOST = 0x88760868, D3DERR_DEVICENOTRESET = 0x88760869
            int hr = ex.HResult;
            return hr == unchecked((int)0x88760868)
                || hr == unchecked((int)0x88760869);
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
