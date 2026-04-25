using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using SharpDX.Direct3D9;
using System.Drawing;
using SharpDX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;

namespace Examples.Otros
{
    /// <summary>
    /// EjemploDisposeMesh2
    /// </summary>
    public class EjemploDisposeMesh2 : TgcExample
    {

        TgcScene scene1;

        public override string getCategory()
        {
            return "Otros";
        }

        public override string getName()
        {
            return "Dispose Mesh 2";
        }

        public override string getDescription()
        {
            return "Dispose Mesh 2";
        }

        public override void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            for (int i = 0; i < 100; i++)
            {
                TgcSceneLoader loader = new TgcSceneLoader();
                TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
                scene.disposeAll();
            }

            TgcSceneLoader loader1 = new TgcSceneLoader();
            scene1 = loader1.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
        }



        public override void render(float elapsedTime)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            GuiController.Instance.Text3d.drawText("ok", 100, 100, Color.Red);
            scene1.renderAll();
        }

        public override void close()
        {
            scene1.disposeAll();
        }

    }
}
