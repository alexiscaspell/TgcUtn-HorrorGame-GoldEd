using AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TgcViewer;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.linternas
{
    class BateriaLinterna : ABateria
    {
        public int cantidadBaterias { get; set; }
        public int cantidadRecarga { get; set; }
        
        private List<TgcSprite> listaSprites;

        public BateriaLinterna() : base()
        {

            tiempoDesgaste = 30;//Gasta bateria cada 12seg
            cantidadDesgaste = 1;//Gasta una barra por vez     
            
            cantidadBaterias = 100000; //le pongo 4 para probar
            cantidadRecarga = 5; //cada vez que recarga, le carga 2 barras
            cargaActual = 5;//Saque el 6 porque lobezzzno lo usa cmo indice de una lista que termina en 5
        }

        public override void init()
        {
            //bateria
            listaSprites = new List<TgcSprite>();

            for (int i = 0; i < 6; i++)
            {
                TgcSprite sprite = new TgcSprite();
                sprite.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosDir + "Media\\Texturas\\bateria"+i.ToString()+".png");
                sprite.Scaling = new Vector2((float)0.0002 * screenSize.Width, (float)0.0005 * screenSize.Height);
                sprite.Position= new Vector2(0, 0.03f * screenSize.Height);
                listaSprites.Add(sprite);
            }
            
            //texto
            texto = new TgcText2d();
            texto.Color = Color.White;
            texto.Align = TgcText2d.TextAlign.LEFT;
            texto.Position = new Point(screenSize.Width/9, screenSize.Height/32);
            texto.Size = new Size(300, 100);
            texto.changeFont(new System.Drawing.Font("TimesNewRoman", 25, FontStyle.Bold));

        }

        public override void render()
        {
            gastarBateria();

            //linterna
            sprite = listaSprites.ElementAt(cargaActual);

            GuiController.Instance.Drawer2D.beginDrawSprite();
            sprite.render();
            GuiController.Instance.Drawer2D.endDrawSprite();
        }

        public override void recargar()
        {
            if (cantidadBaterias == 0 || cargaActual == 6) return;

            cantidadBaterias--;
            cargarBateria(cantidadRecarga);

            sprite = listaSprites.ElementAt(cargaActual);

            tiempoTranscurrido = 0;//Cuando cargo hago que empiece a desgastar desde 0
        }

        private void cargarBateria(int cantidadACargar)
        {
            if (cargaActual + cantidadACargar >= 5)
            {
                cargaActual = 5;
            }
            else
            {
                cargaActual = cargaActual + cantidadACargar;
            }
        }
    }
}
