using SharpDX;
﻿using AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.LOS_IMPROVISADOS.EfectosPosProcesado
{
    class EfectoEscondido:ALuz
    {
        private float time = 0;
        private const float tiempoDeRenderizado = 20;
        public bool terminoEfecto = false;

        public EfectoEscondido(Mapa mapa, CamaraFPS camaraFPS)
        {
            this.mapa = mapa;
            this.camaraFPS = camaraFPS;
        }

        public override void configInicial()
        {
            currentShader = TgcShaders.loadEffect(GuiController.Instance.AlumnoEjemplosDir + "Media\\Shaders\\shaderTerror.fx");

            //GuiController.Instance.Modifiers.addColor("luzOscuraColor", Color.LightYellow);
            //GuiController.Instance.Modifiers.addFloat("luzOscuraIntensidad", 0f, 150f, 19f);
            //GuiController.Instance.Modifiers.addFloat("luzOscuraAtenuacion", 0.1f, 2f, 0.1f);
            //GuiController.Instance.Modifiers.addFloat("luzOscuraEspecularEx", 0, 20, 4f);

            //Modifiers de material
            //GuiController.Instance.Modifiers.addColor("luzOscuraEmissive", Color.Black);
            //GuiController.Instance.Modifiers.addColor("luzOscuraAmbient", Color.LightYellow);
            //GuiController.Instance.Modifiers.addColor("luzOscuraDiffuse", Color.Gray);
            //GuiController.Instance.Modifiers.addColor("luzOscuraSpecular", Color.LightYellow);
        }

        public override void configurarEfecto(TgcMesh mesh)
        {
            //Cargar variables shader de la luz
            mesh.Effect.SetValue("lightColor", new SharpDX.Color4(Color.Gray.R/255f, Color.Gray.G/255f, Color.Gray.B/255f, Color.Gray.A/255f));
            mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(camaraFPS.posicion));
            mesh.Effect.SetValue("lightIntensity", 8f);
            //mesh.Effect.SetValue("lightAttenuation", 0.13f);
            mesh.Effect.SetValue("lightAttenuation", 0.025f);
            mesh.Effect.SetValue("materialSpecularExp", 0.2f);

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
            mesh.Effect.SetValue("lightIntensity", 8f);
            //mesh.Effect.SetValue("lightAttenuation", 0.13f);
            mesh.Effect.SetValue("lightAttenuation", 0.1f);
            mesh.Effect.SetValue("materialSpecularExp", 0.2f);

            mesh.Effect.SetValue("materialEmissiveColor", new SharpDX.Color4(Color.Black.R/255f, Color.Black.G/255f, Color.Black.B/255f, Color.Black.A/255f));
            mesh.Effect.SetValue("materialAmbientColor", new SharpDX.Color4(Color.DarkGray.R/255f, Color.DarkGray.G/255f, Color.DarkGray.B/255f, Color.DarkGray.A/255f));
            mesh.Effect.SetValue("materialDiffuseColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));
            mesh.Effect.SetValue("materialSpecularColor", new SharpDX.Color4(Color.White.R/255f, Color.White.G/255f, Color.White.B/255f, Color.White.A/255f));

        }

        public override void render()
        {
            inv.render();

            updateFondo();

            time += GuiController.Instance.ElapsedTime;

            foreach (TgcMesh mesh in mapa.escena.Meshes)
            {
                if (TgcCollisionUtils.testAABBAABB(mesh.BoundingBox, cajaNegra.BoundingBox))
                {
                    mesh.Effect = currentShader;
                    mesh.Technique = "ShaderTerror";

                    configurarEfecto(mesh);

                    mesh.Effect.SetValue("time", time);

                    mesh.render();
                }
            }

            foreach (Accionable a in mapa.objetos)
            {
                if (TgcCollisionUtils.testAABBAABB(a.getMesh().BoundingBox, cajaNegra.BoundingBox))
                {
                    a.getMesh().Effect = currentShader;
                    a.getMesh().Technique = "ShaderTerror";

                    configurarEfecto(a.getMesh());

                    a.getMesh().Effect.SetValue("time", time);

                    a.render();
                }
            }
            
            //renderizo el boss
            AnimatedBoss.Instance.update();
            if( TgcCollisionUtils.testAABBAABB(cajaNegra.BoundingBox, AnimatedBoss.Instance.getBoundingBox() ))
            {
            	AnimatedBoss.Instance.cuerpo.Effect = skeletalShader;
            	AnimatedBoss.Instance.cuerpo.Technique = GuiController.Instance.Shaders.getTgcSkeletalMeshTechnique(
            		AnimatedBoss.Instance.cuerpo.RenderType);
            	//AnimatedBoss.Instance.cuerpo.Technique = GuiController.Instance.Shaders.getTgcMeshTechnique(
            	//	((TgcMesh.MeshRenderType)AnimatedBoss.Instance.cuerpo.RenderType) );
            	
            	configurarSkeletal(AnimatedBoss.Instance.cuerpo);
            	
            	//AnimatedBoss.Instance.cuerpo.Effect.SetValue("time",time);
            	
            	AnimatedBoss.Instance.render();
            }

            terminoEfecto = time > tiempoDeRenderizado;

            if (terminoEfecto)
            {
                time = 0;
            }

        }
    }
}
