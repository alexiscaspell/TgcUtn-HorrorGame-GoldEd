using TgcViewer;
using SharpDX;
using TgcViewer.Utils.TgcSceneLoader;
using SharpDX.Direct3D9;
using System.Drawing;
using TgcViewer.Utils.TgcGeometry;
using AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA;
using System.Collections.Generic;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.fluors
{
    class LuzFluor : ALuz
    {
        public LuzFluor(Mapa mapa, CamaraFPS camaraFPS)
        {
        	this.mapa = mapa;
        	this.camaraFPS = camaraFPS;
        }

        public override void configInicial()
        {
            GuiController.Instance.Modifiers.addColor("fluorColor", Color.LightGreen);
            GuiController.Instance.Modifiers.addFloat("fluorIntensidad", 0f, 150f, 70f);
            GuiController.Instance.Modifiers.addFloat("fluorAtenuacion", 0.1f, 2f, 0.2f);
            GuiController.Instance.Modifiers.addFloat("fluorEspecularEx", 0, 40, 6f);

            //Modifiers de material
            GuiController.Instance.Modifiers.addColor("fluorEmissive", Color.Black);
            GuiController.Instance.Modifiers.addColor("fluorAmbient", Color.LightGreen);
            GuiController.Instance.Modifiers.addColor("fluorDiffuse", Color.LightGreen);
            GuiController.Instance.Modifiers.addColor("fluorSpecular", Color.LightGreen);
            
            currentShader = GuiController.Instance.Shaders.TgcMeshPointLightShader;
        }
        
        public override void configurarEfecto(TgcMesh mesh)
        {
        			
        	//Cargar variables shader de la luz
            mesh.Effect.SetValue("lightColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
            mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
            mesh.Effect.SetValue("lightIntensity", (float)GuiController.Instance.Modifiers["fluorIntensidad"]);
            mesh.Effect.SetValue("lightAttenuation", (float)GuiController.Instance.Modifiers["fluorAtenuacion"]);

            //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
            mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
            mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
            mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
            mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
            mesh.Effect.SetValue("materialSpecularExp", (float)GuiController.Instance.Modifiers["fluorEspecularEx"]);

        }
        
        public override void configurarSkeletal(TgcSkeletalMesh mesh)
        {
        			
        	//Cargar variables shader de la luz
            mesh.Effect.SetValue("lightColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
            mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
            mesh.Effect.SetValue("lightIntensity", (float)GuiController.Instance.Modifiers["fluorIntensidad"]);
            mesh.Effect.SetValue("lightAttenuation", (float)GuiController.Instance.Modifiers["fluorAtenuacion"]);

            //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
            mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
            mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
            mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
            mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
            mesh.Effect.SetValue("materialSpecularExp", (float)GuiController.Instance.Modifiers["fluorEspecularEx"]);

        }
//
//        public override void render()
//        {
//            Effect currentShader = GuiController.Instance.Shaders.TgcMeshPointLightShader;
//
//            foreach (TgcMesh mesh in mapa.escenaFiltrada)
//            {
//                mesh.Effect = currentShader;
//                mesh.Technique = GuiController.Instance.Shaders.getTgcMeshTechnique(mesh.RenderType);
//            }
//            
//            foreach (TgcMesh mesh in mapa.escenaFiltrada)
//            {
//                //Cargar variables shader de la luz
//                mesh.Effect.SetValue("lightColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
//                mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
//                mesh.Effect.SetValue("lightIntensity", (float)GuiController.Instance.Modifiers["fluorIntensidad"]);
//                mesh.Effect.SetValue("lightAttenuation", (float)GuiController.Instance.Modifiers["fluorAtenuacion"]);
//
//                //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
//                mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
//                mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
//                mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
//                mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(this.Color.R/255f, this.Color.G/255f, this.Color.B/255f, this.Color.A/255f));
//                mesh.Effect.SetValue("materialSpecularExp", (float)GuiController.Instance.Modifiers["fluorEspecularEx"]);
//
//                mesh.render();
//            }          
//        }
    }
}
