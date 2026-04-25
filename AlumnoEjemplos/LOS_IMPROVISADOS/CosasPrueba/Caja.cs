using SharpDX.DirectInput;
﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using System.Drawing;
using TgcViewer.Example;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Input;
using SharpDX.Direct3D9;
using TgcViewer.Utils.Shaders;



namespace AlumnoEjemplos.LOS_IMPROVISADOS
{
    class Caja
    {
        private TgcBox caja;

        public Vector3 posicion;

        private Vector3 tamanio;

        private System.Drawing.Color color;
        
        public void move(Vector3 v){
        	caja.move(v);
        }
        
        public void changePosicion(Vector3 v){
        	posicion = v;
        }

        public TgcBoundingBox getBoundingBox()
        {
            return caja.BoundingBox;
        }

        public void init()
        {
            posicion = new Vector3(280, 10, 120);

            tamanio = new Vector3(20, 20, 20);

            color = Color.Red;

            caja = TgcBox.fromSize(tamanio, color);

            caja.Position = posicion;

        }

        public void render(CamaraFPS camara)
        {
            if (GuiController.Instance.D3dInput.keyPressed(Key.B)&&estaCerca(camara.posicion))
            {
                color = Color.Blue;
            }
            if (GuiController.Instance.D3dInput.keyPressed(Key.R) && estaCerca(camara.posicion))
            {
                color = Color.Red;
            }

            if (GuiController.Instance.D3dInput.keyPressed(Key.F))
            {
                //caja.rotateY(45);
                //caja.rotateY(MathUtil.DegreesToRadians(45f));
                //posicion.TransformCoordinate()
                    //caja.Rotation = GuiController.Instance.FpsCamera.getLookAt();

                //TgcBoundingBox colisionador = new TgcBoundingBox();

                
            }

            

            update();

            caja.render();

        }

        private void update()
        {
            caja.Position = posicion;
            caja.Color = color;
            caja.Size = tamanio;

            caja.updateValues();
        }

        internal void changeColor(System.Drawing.Color color)
        {
            this.color = color;
        }

        public bool estaCerca(Vector3 posObjeto)
        {
            Vector3 vectorAux = posObjeto;
            Vector3.Subtract(vectorAux, posicion);
            //Vector3.Subtract(vectorAux, new Vector3(0, vectorAux.Y, 0));//ESTO ES PARA Q NO IMPORTE LA ALTURA

            return vectorAux.Length() <= 30;

        }

    }
}
