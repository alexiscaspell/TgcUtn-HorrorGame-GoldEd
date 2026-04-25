using AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.general
{
    class LuzOscura : ALuz
    {
        public LuzOscura(Mapa mapa, CamaraFPS camaraFPS)
        {
            this.mapa = mapa;
            this.camaraFPS = camaraFPS;
        }

        public override void configInicial()
        {
            currentShader = GuiController.Instance.Shaders.TgcMeshPointLightShader;
        }

        public override void configurarEfecto(TgcMesh mesh)
        {
            //Cargar variables shader de la luz
            mesh.Effect.SetValue("lightColor", new SharpDX.Color4(Color.Gray.R/255f, Color.Gray.G/255f, Color.Gray.B/255f, Color.Gray.A/255f));
            mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
            mesh.Effect.SetValue("lightIntensity", 9f);
            mesh.Effect.SetValue("lightAttenuation", 0.13f);
            mesh.Effect.SetValue("materialSpecularExp", 0.5f);

            mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(Color.Black.R/255f, Color.Black.G/255f, Color.Black.B/255f, Color.Black.A/255f));
            mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(Color.DarkGray.R/255f, Color.DarkGray.G/255f, Color.DarkGray.B/255f, Color.DarkGray.A/255f));
            mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
            mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
        }
        
        public override void configurarSkeletal(TgcSkeletalMesh mesh)
        {
            //Cargar variables shader de la luz
            mesh.Effect.SetValue("lightColor", new SharpDX.Color4(Color.Gray.R/255f, Color.Gray.G/255f, Color.Gray.B/255f, Color.Gray.A/255f));
            mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
            mesh.Effect.SetValue("lightIntensity", 9f);
            mesh.Effect.SetValue("lightAttenuation", 0.13f);
            mesh.Effect.SetValue("materialSpecularExp", 0.5f);

            mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(Color.Black.R/255f, Color.Black.G/255f, Color.Black.B/255f, Color.Black.A/255f));
            mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(Color.DarkGray.R/255f, Color.DarkGray.G/255f, Color.DarkGray.B/255f, Color.DarkGray.A/255f));
            mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
            mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
        }
    }
}
