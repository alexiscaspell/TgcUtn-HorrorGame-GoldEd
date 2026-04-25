using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;

namespace TgcViewer.Example
{
    /// <summary>
    /// Utilidad para cargar dinamicamente las DLL de los ejemplos
    /// </summary>
    class ExampleLoader
    {
        public const string EXAMPLES_DIR = "Examples";
        public static string[] DIR_SKIP_LISP = new string[] { "\\bin", "\\obj"};

        private Dictionary<TreeNode, TgcExample> treeExamplesDict;

        List<TgcExample> currentExamples;
        /// <summary>
        /// Ejemplos actualmente cargados
        /// </summary>
        public List<TgcExample> CurrentExamples
        {
            get { return currentExamples; }
        }

        public ExampleLoader()
        {
            treeExamplesDict = new Dictionary<TreeNode, TgcExample>();
        }

        /// <summary>
