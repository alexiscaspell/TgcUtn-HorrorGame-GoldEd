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
    class RoomLinterna : ARoomLuz
    {
        public Vector3 direccionVista { get; set; }

        public RoomLinterna(Vector3 posicion, Vector3 direccionVista) : base()
        {
            this.posicion = posicion;
            this.direccionVista = direccionVista;

            TgcSceneLoader loader = new TgcSceneLoader();
            escenaLampara = loader.loadSceneFromFile(
                GuiController.Instance.AlumnoEjemplosDir + "Media\\Objetos\\RoomsIluminados\\roomLinterna-TgcScene.xml",
                GuiController.Instance.AlumnoEjemplosDir + "Media\\Objetos\\RoomsIluminados\\");

            foreach (TgcMesh mesh in escenaLampara.Meshes)
            {
                mesh.Position = this.posicion;
            }

            init();
        }

        public override void render()
        {
            foreach (TgcMesh mesh in meshesRoom)
            {
                mesh.Effect.SetValue("lightColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
                mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(posicion));
                mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(posicion));
                mesh.Effect.SetValue("spotLightDir", TgcParserUtils.vector3ToFloat3Array(direccionVista));
                mesh.Effect.SetValue("lightIntensity", 150f);
                mesh.Effect.SetValue("lightAttenuation", 0.4f);
                mesh.Effect.SetValue("spotLightAngleCos", 90f);
                mesh.Effect.SetValue("spotLightExponent", 0f);

                mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(Color.Black.R/255f, Color.Black.G/255f, Color.Black.B/255f, Color.Black.A/255f));
                mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
                mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
                mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(Color.LightGray.R/255f, Color.LightGray.G/255f, Color.LightGray.B/255f, Color.LightGray.A/255f));
                mesh.Effect.SetValue("materialSpecularExp", 18f);

                mesh.render();
            }
        }
    }
}