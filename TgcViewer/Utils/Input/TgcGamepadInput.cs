using System;
using SharpDX.XInput;

namespace TgcViewer.Utils.Input
{
    /// <summary>
    /// Soporte de mando Xbox/XInput (jugador 1).
    /// Expone la misma interfaz de buttonDown/buttonPressed que TgcD3dInput para teclado/mouse.
    /// Se actualiza desde TgcD3dInput.update() en cada frame.
    /// </summary>
    public class TgcGamepadInput
    {
        private const float DEAD_ZONE = 0.15f;
        private const float STICK_MAX = 32767f;
        private const float TRIGGER_MAX = 255f;

        private readonly Controller controller;
        private State prevState;
        private State currState;
        private bool wasConnected;

        public TgcGamepadInput()
        {
            controller = new Controller(UserIndex.One);
        }

        /// <summary>True si hay un mando conectado en el puerto 1.</summary>
        public bool IsConnected => controller.IsConnected;

        internal void update()
        {
            if (!controller.IsConnected)
            {
                wasConnected = false;
                return;
            }

            prevState = wasConnected ? currState : new State();
            controller.GetState(out currState);
            wasConnected = true;
        }

        // ── Sticks analógicos ─────────────────────────────────────────────

        /// <summary>Stick izquierdo X: -1 (izq) a +1 (der), con dead zone aplicada.</summary>
        public float LeftStickX  => ApplyDeadZone(currState.Gamepad.LeftThumbX  / STICK_MAX);
        /// <summary>Stick izquierdo Y: -1 (abajo) a +1 (arriba), con dead zone aplicada.</summary>
        public float LeftStickY  => ApplyDeadZone(currState.Gamepad.LeftThumbY  / STICK_MAX);
        /// <summary>Stick derecho X: -1 (izq) a +1 (der), con dead zone aplicada.</summary>
        public float RightStickX => ApplyDeadZone(currState.Gamepad.RightThumbX / STICK_MAX);
        /// <summary>Stick derecho Y: -1 (abajo) a +1 (arriba), con dead zone aplicada.</summary>
        public float RightStickY => ApplyDeadZone(currState.Gamepad.RightThumbY / STICK_MAX);

        /// <summary>Gatillo izquierdo: 0 a 1.</summary>
        public float LeftTrigger  => currState.Gamepad.LeftTrigger  / TRIGGER_MAX;
        /// <summary>Gatillo derecho: 0 a 1.</summary>
        public float RightTrigger => currState.Gamepad.RightTrigger / TRIGGER_MAX;

        // ── Botones ───────────────────────────────────────────────────────

        /// <summary>Devuelve true mientras el botón está presionado.</summary>
        public bool buttonDown(GamepadButtonFlags button)
        {
            if (!wasConnected) return false;
            return currState.Gamepad.Buttons.HasFlag(button);
        }

        /// <summary>Devuelve true solo en el frame en que se presionó (flanco ascendente).</summary>
        public bool buttonPressed(GamepadButtonFlags button)
        {
            if (!wasConnected) return false;
            bool prev = prevState.Gamepad.Buttons.HasFlag(button);
            bool curr = currState.Gamepad.Buttons.HasFlag(button);
            return curr && !prev;
        }

        /// <summary>Devuelve true solo en el frame en que se soltó (flanco descendente).</summary>
        public bool buttonReleased(GamepadButtonFlags button)
        {
            if (!wasConnected) return false;
            bool prev = prevState.Gamepad.Buttons.HasFlag(button);
            bool curr = currState.Gamepad.Buttons.HasFlag(button);
            return !curr && prev;
        }

        private static float ApplyDeadZone(float value)
        {
            if (Math.Abs(value) < DEAD_ZONE) return 0f;
            // Rescalar para que empiece en 0 justo después de la dead zone
            float sign = value > 0 ? 1f : -1f;
            return sign * (Math.Abs(value) - DEAD_ZONE) / (1f - DEAD_ZONE);
        }
    }
}
