using TgcViewer;
using SharpDX;
using TgcViewer.Utils.TgcSceneLoader;
using SharpDX.Direct3D9;
using System.Drawing;
using AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA;
using TgcViewer.Utils.TgcGeometry;
using System.Collections.Generic;
using TgcViewer.Utils.TgcSkeletalAnimation;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.linternas
{
    class LuzLinterna : ALuz
    {
    	public LuzLinterna(Mapa mapa, CamaraFPS camaraFPS)
        {
    		this.mapa = mapa;
    		this.camaraFPS = camaraFPS;
        }

        public override void configInicial()
        {
            ///luz
            GuiController.Instance.Modifiers.addColor("linternaColor", Color.White);
            GuiController.Instance.Modifiers.addFloat("linternaIntensidad", 0, 400f, 190f);
            GuiController.Instance.Modifiers.addFloat("linternaAtenuacion", 0.1f, 2, 0.4f);
            GuiController.Instance.Modifiers.addFloat("linternaSpecularEx", 0, 20, 15f);
            GuiController.Instance.Modifiers.addFloat("linternaAngulo", 0, 180, 45f);
            GuiController.Instance.Modifiers.addFloat("linternaSpotExponent", 0, 40, 18f);

            //material
            GuiController.Instance.Modifiers.addColor("linternaEmissive", Color.Black);
            GuiController.Instance.Modifiers.addColor("linternaAmbient", Color.White);
            GuiController.Instance.Modifiers.addColor("linternaDiffuse", Color.White);
            GuiController.Instance.Modifiers.addColor("linternaSpecular", Color.White);
            
            currentShader = GuiController.Instance.Shaders.TgcMeshSpotLightShader;
                    
        } 

        public override void configurarEfecto(TgcMesh mesh)
        {
        	
        	//Cargar variables shader de la luz
                mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.camaraFramework.Position));
                mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.camaraFramework.Position));
                mesh.Effect.SetValue("spotLightDir", TgcParserUtils.vector3ToFloat3Array(camaraFPS.camaraFramework.viewDir));
                mesh.Effect.SetValue("lightColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["linternaColor"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaColor"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaColor"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaColor"]).A/255f));
                mesh.Effect.SetValue("lightIntensity", (float)GuiController.Instance.Modifiers["linternaIntensidad"]);
                mesh.Effect.SetValue("lightAttenuation", (float)GuiController.Instance.Modifiers["linternaAtenuacion"]);
                mesh.Effect.SetValue("spotLightExponent", (float)GuiController.Instance.Modifiers["linternaSpotExponent"]);
                mesh.Effect.SetValue("spotLightAngleCos", FastMath.ToRad((float)GuiController.Instance.Modifiers["linternaAngulo"]));

                //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["linternaEmissive"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaEmissive"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaEmissive"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaEmissive"]).A/255f));
                mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["linternaAmbient"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaAmbient"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaAmbient"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaAmbient"]).A/255f));
                mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["linternaDiffuse"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaDiffuse"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaDiffuse"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaDiffuse"]).A/255f));
                mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["linternaSpecular"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaSpecular"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaSpecular"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaSpecular"]).A/255f));
                mesh.Effect.SetValue("materialSpecularExp", (float)GuiController.Instance.Modifiers["linternaSpecularEx"]);

        }

        public override void configurarSkeletal(TgcSkeletalMesh mesh)
        {
        	mesh.Effect.SetValue("lightColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
            mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
            mesh.Effect.SetValue("lightIntensity", 190f);
            mesh.Effect.SetValue("lightAttenuation", 0.4f);
            mesh.Effect.SetValue("materialSpecularExp", 15f);

            mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(Color.Black.R/255f, Color.Black.G/255f, Color.Black.B/255f, Color.Black.A/255f));
            mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
            mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
            mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));

        }
        //public override void render()
        //{
        //    Effect currentShader = GuiController.Instance.Shaders.TgcMeshSpotLightShader;
        //    foreach (TgcMesh mesh in mapa.escenaFiltrada)
        //    {
        //        mesh.Effect = currentShader;
        //        mesh.Technique = GuiController.Instance.Shaders.getTgcMeshTechnique(mesh.RenderType);
        //    }

        //    Vector3 lightDir = camaraFPS.direccionVista;
        //    lightDir.Normalize();

        //    foreach (TgcMesh mesh in mapa.escenaFiltrada)
        //    {
        //        //Cargar variables shader de la luz
        //        mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
        //        mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
        //        mesh.Effect.SetValue("spotLightDir", TgcParserUtils.vector3ToFloat3Array(lightDir));
        //        mesh.Effect.SetValue("lightColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["linternaColor"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaColor"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaColor"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaColor"]).A/255f));
        //        mesh.Effect.SetValue("lightIntensity", (float)GuiController.Instance.Modifiers["linternaIntensidad"]);
        //        mesh.Effect.SetValue("lightAttenuation", (float)GuiController.Instance.Modifiers["linternaAtenuacion"]);
        //        mesh.Effect.SetValue("spotLightExponent", (float)GuiController.Instance.Modifiers["linternaSpotExponent"]);
        //        mesh.Effect.SetValue("spotLightAngleCos", FastMath.ToRad((float)GuiController.Instance.Modifiers["linternaAngulo"]));

        //        //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
        //        mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["linternaEmissive"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaEmissive"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaEmissive"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaEmissive"]).A/255f));
        //        mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["linternaAmbient"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaAmbient"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaAmbient"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaAmbient"]).A/255f));
        //        mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["linternaDiffuse"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaDiffuse"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaDiffuse"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaDiffuse"]).A/255f));
        //        mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(((System.Drawing.Color)GuiController.Instance.Modifiers["linternaSpecular"]).R/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaSpecular"]).G/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaSpecular"]).B/255f, ((System.Drawing.Color)GuiController.Instance.Modifiers["linternaSpecular"]).A/255f));
        //        mesh.Effect.SetValue("materialSpecularExp", (float)GuiController.Instance.Modifiers["linternaSpecularEx"]);

        //        mesh.render();
        //    }
        //}

    }
}
