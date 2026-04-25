using SharpDX;
using SharpDX.Direct3D9;
using System.Collections.Generic;
using System.Drawing;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace AlumnoEjemplos.LOS_IMPROVISADOS
{
    class RoomFarol : ARoomLuz
    {
        public RoomFarol(Vector3 posicion) : base()
        {
            this.posicion = posicion;

            TgcSceneLoader loader = new TgcSceneLoader();
            escenaLampara = loader.loadSceneFromFile(
                GuiController.Instance.AlumnoEjemplosDir + "Media\\Objetos\\RoomsIluminados\\roomFarol-TgcScene.xml",
                GuiController.Instance.AlumnoEjemplosDir + "Media\\Objetos\\RoomsIluminados\\");

            foreach (TgcMesh mesh in escenaLampara.Meshes)
            {
                mesh.Scale *= 0.5f;
                mesh.Position = this.posicion;
            }

            init();
        }

        public override void render()
        {
            foreach (TgcMesh mesh in meshesRoom)
            {
                mesh.Effect.SetValue("lightColor", new SharpDX.Color4(Color.LightYellow.R/255f, Color.LightYellow.G/255f, Color.LightYellow.B/255f, Color.LightYellow.A/255f));
                mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(posicion));
                mesh.Effect.SetValue("lightIntensity", 15f);
                mesh.Effect.SetValue("lightAttenuation", 0.1f);

                mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(Color.Black.R/255f, Color.Black.G/255f, Color.Black.B/255f, Color.Black.A/255f));
                mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(Color.LightYellow.R/255f, Color.LightYellow.G/255f, Color.LightYellow.B/255f, Color.LightYellow.A/255f));
                mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(Color.Gray.R/255f, Color.Gray.G/255f, Color.Gray.B/255f, Color.Gray.A/255f));
                mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(Color.LightYellow.R/255f, Color.LightYellow.G/255f, Color.LightYellow.B/255f, Color.LightYellow.A/255f));
                mesh.Effect.SetValue("materialSpecularExp", 4f);

                mesh.render();
            }
        }
    }
}