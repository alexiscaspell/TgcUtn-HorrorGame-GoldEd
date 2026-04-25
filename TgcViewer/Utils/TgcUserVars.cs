using System;
using System.Collections.Generic;
using System.Drawing;

namespace TgcViewer.Utils
{
    /// <summary>
    /// Administrador de variables de usuario (UserVars).
    /// Implementado con Dictionary — sin dependencia de DataGridView.
    /// </summary>
    public class TgcUserVars
    {
        private readonly Dictionary<string, object> vars = new Dictionary<string, object>();

        // Constructor original mantenido por compatibilidad (ignora el DataGridView)
        public TgcUserVars(System.Windows.Forms.DataGridView dataGrid)
        {
            // DataGridView ignored in standalone mode
        }

        public TgcUserVars()
        {
        }

        /// <summary>Elimina todas las UserVars.</summary>
        public void clearVars()
        {
            vars.Clear();
        }

        /// <summary>Agrega una nueva UserVar con valor vacío.</summary>
        public void addVar(string name)
        {
            if (!vars.ContainsKey(name))
                vars[name] = "";
        }

        /// <summary>Agrega una nueva variable junto con su valor.</summary>
        public void addVar(string name, object value)
        {
            vars[name] = value ?? "";
        }

        /// <summary>Carga el valor de una variable.</summary>
        public void setValue(string name, object value, Color foreColor)
        {
            if (!vars.ContainsKey(name))
                throw new Exception("Se intentó cargar una UserVar inexistente: " + name);
            vars[name] = value;
        }

        /// <summary>Carga el valor de una variable.</summary>
        public void setValue(string name, object value)
        {
            if (!vars.ContainsKey(name))
                throw new Exception("Se intentó cargar una UserVar inexistente: " + name);
            vars[name] = value;
        }

        /// <summary>Devuelve el valor de la variable especificada.</summary>
        public object getValue(string name)
        {
            if (!vars.TryGetValue(name, out object val))
                throw new Exception("Se intentó acceder una UserVar inexistente: " + name);
            return val;
        }

        public string this[string varName]
        {
            set { setValue(varName, value); }
        }
    }
}
