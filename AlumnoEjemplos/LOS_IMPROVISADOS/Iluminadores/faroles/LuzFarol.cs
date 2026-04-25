using TgcViewer;
using SharpDX;
using TgcViewer.Utils.TgcSceneLoader;
using SharpDX.Direct3D9;
using System.Drawing;
using AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA;
using TgcViewer.Utils.TgcGeometry;
using System.Collections.Generic;
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils.TgcSkeletalAnimation;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.faroles
{
    class LuzFarol : ALuz
    {
        public LuzFarol(Mapa mapa, CamaraFPS camaraFPS)
        {
        	this.mapa = mapa;
        	this.camaraFPS = camaraFPS;
        }
        
        
        public override void configInicial()
        {
            GuiController.Instance.Modifiers.addColor("farolColor", Color.LightYellow);
            GuiController.Instance.Modifiers.addFloat("farolIntensidad", 0f, 150f, 19f);
            GuiController.Instance.Modifiers.addFloat("farolAtenuacion", 0.1f, 2f, 0.1f);
            GuiController.Instance.Modifiers.addFloat("farolEspecularEx", 0, 20, 4f);

            //Modifiers de material
            GuiController.Instance.Modifiers.addColor("farolEmissive", Color.Black);
            GuiController.Instance.Modifiers.addColor("farolAmbient", Color.LightYellow);
            GuiController.Instance.Modifiers.addColor("farolDiffuse", Color.Gray);
            GuiController.Instance.Modifiers.addColor("farolSpecular", Color.LightYellow);
            
            currentShader = GuiController.Instance.Shaders.TgcMeshPointLightShader;
      		}
        
        public override void configurarEfecto(TgcMesh mesh)
        {
			//Cargar variables shader de la luz
			mesh.Effect.SetValue("lightColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).A/255f));
			mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
			mesh.Effect.SetValue("lightIntensity", (float)GuiController.Instance.Modifiers["farolIntensidad"]);
			mesh.Effect.SetValue("lightAttenuation", (float)GuiController.Instance.Modifiers["farolAtenuacion"]);
			
			//Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
			mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).A/255f));
			mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).A/255f));
			mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).A/255f));
			mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).A/255f));
			mesh.Effect.SetValue("materialSpecularExp", (float)GuiController.Instance.Modifiers["farolEspecularEx"]);
        }
        
        public override void configurarSkeletal(TgcSkeletalMesh mesh)
        {
			//Cargar variables shader de la luz
			mesh.Effect.SetValue("lightColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).A/255f));
			mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
			mesh.Effect.SetValue("lightIntensity", (float)GuiController.Instance.Modifiers["farolIntensidad"]);
			mesh.Effect.SetValue("lightAttenuation", (float)GuiController.Instance.Modifiers["farolAtenuacion"]);
			
			//Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
			mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).A/255f));
			mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).A/255f));
			mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).A/255f));
			mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).A/255f));
			mesh.Effect.SetValue("materialSpecularExp", (float)GuiController.Instance.Modifiers["farolEspecularEx"]);
        }

//        public override void render()
//        {
//            Effect currentShader = GuiController.Instance.Shaders.TgcMeshPointLightShader;
//
//            inv.render();
//            
//            //Dibujo el fondo para evitar el azul
//            updateFondo();
//            cajaNegra.render();
//            esferaNegra.render();
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
//                mesh.Effect.SetValue("lightColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolColor"]).A/255f));
//                mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
//                mesh.Effect.SetValue("lightIntensity", (float)GuiController.Instance.Modifiers["farolIntensidad"]);
//                mesh.Effect.SetValue("lightAttenuation", (float)GuiController.Instance.Modifiers["farolAtenuacion"]);
//
//                //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
//                mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolEmissive"]).A/255f));
//                mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolAmbient"]).A/255f));
//                mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolDiffuse"]).A/255f));
//                mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["farolSpecular"]).A/255f));
//                mesh.Effect.SetValue("materialSpecularExp", (float)GuiController.Instance.Modifiers["farolEspecularEx"]);
//                
//				if( TgcCollisionUtils.testAABBAABB(mesh.BoundingBox, cajaNegra.BoundingBox))
//				{
//				   	mesh.render();
//                }
//                
//				
//            }
//        }
    }
}
