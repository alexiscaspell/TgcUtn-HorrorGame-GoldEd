using System;
using System.Windows.Forms;
using System.Drawing;

namespace TgcViewer.Utils.Modifiers
{
    /// <summary>
    /// Modificador para elegir una textura mediante un dialogo de archivo.
    /// </summary>
    public class TgcTextureModifier : TgcModifierPanel
    {
        PictureBox textureBox;
        string defaultPath;
        string selectedPath;

        public TgcTextureModifier(string varName, string defaultPath)
            : base(varName)
        {
            this.defaultPath = defaultPath;
            this.selectedPath = defaultPath;

            textureBox = new PictureBox();
            textureBox.Margin = new Padding(0);
            textureBox.Size = new Size(100, 100);
            textureBox.Image = getImage(defaultPath);
            textureBox.BorderStyle = BorderStyle.FixedSingle;
            textureBox.SizeMode = PictureBoxSizeMode.Zoom;
            textureBox.Click += new EventHandler(textureButton_click);

            contentPanel.Controls.Add(textureBox);
        }

        private Image getImage(string path)
        {
            try { return Image.FromFile(path); }
            catch { return null; }
        }

        private void textureButton_click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Imágenes|*.png;*.jpg;*.bmp;*.tga|Todos|*.*";
                dlg.Title = "Seleccionar textura";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    selectedPath = dlg.FileName;
                    textureBox.Image = getImage(selectedPath);
                }
                else
                {
                    selectedPath = defaultPath;
                    textureBox.Image = getImage(defaultPath);
                }
            }
        }

        public override object getValue()
        {
            return selectedPath;
        }
    }
}
