using TgcViewer;
using TgcViewer.Utils.Input;
using SharpDX.DirectInput;
using SharpDX.XInput;

namespace AlumnoEjemplos.LOS_IMPROVISADOS
{
    /// <summary>
    /// Capa de abstracción de input: unifica teclado/mouse con mando Xbox (XInput).
    /// Todo el código del juego debe leer acciones desde aquí en lugar de consultar
    /// TgcD3dInput directamente.
    /// </summary>
    public class InputManager
    {
        /// <summary>Instancia activa del InputManager, disponible una vez que EjemploAlumno hace init().</summary>
        public static InputManager Current { get; private set; }

        public static InputManager Initialize()
        {
            Current = new InputManager();
            return Current;
        }
        /// <summary>Sensibilidad del stick derecho para la cámara (equiv. a pixels de mouse).</summary>
        public float GamepadLookSensitivity = 8f;

        private TgcD3dInput kb => GuiController.Instance.D3dInput;
        private TgcGamepadInput gp => GuiController.Instance.GamepadInput;

        // ── Movimiento ────────────────────────────────────────────────────

        /// <summary>Eje horizontal de movimiento: -1 izq, +1 der. Combina WASD + stick izquierdo.</summary>
        public float MoveX
        {
            get
            {
                float k = 0f;
                if (kb.keyDown(Key.A)) k -= 1f;
                if (kb.keyDown(Key.D)) k += 1f;
                float g = gp.LeftStickX;
                return k != 0f ? k : g;
            }
        }

        /// <summary>Eje de avance: -1 atrás, +1 adelante. Combina WASD + stick izquierdo.</summary>
        public float MoveZ
        {
            get
            {
                float k = 0f;
                if (kb.keyDown(Key.S)) k -= 1f;
                if (kb.keyDown(Key.W)) k += 1f;
                float g = gp.LeftStickY;
                return k != 0f ? k : g;
            }
        }

        // ── Cámara ────────────────────────────────────────────────────────

        /// <summary>Rotación horizontal de cámara (yaw). Combina mouse X + stick derecho.</summary>
        public float LookX => kb.XposRelative + gp.RightStickX * GamepadLookSensitivity;

        /// <summary>Rotación vertical de cámara (pitch). Combina mouse Y + stick derecho.</summary>
        public float LookY => kb.YposRelative + gp.RightStickY * GamepadLookSensitivity;

        // ── Correr/agacharse ──────────────────────────────────────────────

        /// <summary>True mientras el jugador se agacha/corre (Shift / Botón B).</summary>
        public bool Agacharse() => kb.keyDown(Key.LeftShift) || gp.buttonDown(GamepadButtonFlags.B);

        // ── Acciones ──────────────────────────────────────────────────────

        /// <summary>Interactuar con objetos (E / Botón A).</summary>
        public bool Interactuar() => kb.keyPressed(Key.E) || gp.buttonPressed(GamepadButtonFlags.A);

        /// <summary>Alternar luz principal (click izquierdo / Botón X).</summary>
        public bool ToggleLuzPrincipal() =>
            kb.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT) ||
            gp.buttonPressed(GamepadButtonFlags.X);

        /// <summary>Alternar luz fluorescente (click derecho / Botón Y).</summary>
        public bool ToggleLuzFluor() =>
            kb.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_RIGHT) ||
            gp.buttonPressed(GamepadButtonFlags.Y);

        /// <summary>Siguiente iluminador (F / LB).</summary>
        public bool SiguienteIluminador() =>
            kb.keyPressed(Key.F) ||
            gp.buttonPressed(GamepadButtonFlags.LeftShoulder);

        /// <summary>Defenderse con fluor (solo mando: RB).</summary>
        public bool DefenderConFluor() => gp.buttonPressed(GamepadButtonFlags.RightShoulder);

        /// <summary>Abrir/cerrar inventario (Q / Back).</summary>
        public bool ToggleInventario() =>
            kb.keyPressed(Key.Q) ||
            gp.buttonPressed(GamepadButtonFlags.Back);

        /// <summary>Abrir mapa (en inventario: Space / D-pad derecho).</summary>
        public bool AbrirMapa() =>
            kb.keyPressed(Key.Space) ||
            gp.buttonPressed(GamepadButtonFlags.DPadRight);

        /// <summary>Pausa (P / Start).</summary>
        public bool Pausa() =>
            kb.keyPressed(Key.P) ||
            gp.buttonPressed(GamepadButtonFlags.Start);

        // ── Menús ─────────────────────────────────────────────────────────

        /// <summary>Navegar arriba en menús (↑ / D-pad arriba).</summary>
        public bool MenuArriba() =>
            kb.keyPressed(Key.Up) ||
            gp.buttonPressed(GamepadButtonFlags.DPadUp);

        /// <summary>Navegar abajo en menús (↓ / D-pad abajo).</summary>
        public bool MenuAbajo() =>
            kb.keyPressed(Key.Down) ||
            gp.buttonPressed(GamepadButtonFlags.DPadDown);

        /// <summary>Navegar izquierda en inventario (← / D-pad izquierdo).</summary>
        public bool MenuIzquierda() =>
            kb.keyPressed(Key.Left) ||
            gp.buttonPressed(GamepadButtonFlags.DPadLeft);

        /// <summary>Navegar derecha en inventario (→ / D-pad derecho).</summary>
        public bool MenuDerecha() =>
            kb.keyPressed(Key.Right) ||
            gp.buttonPressed(GamepadButtonFlags.DPadRight);

        /// <summary>Confirmar selección en menús (Space / Botón A).</summary>
        public bool MenuConfirmar() =>
            kb.keyPressed(Key.Space) ||
            gp.buttonPressed(GamepadButtonFlags.A);
    }
}
