using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using SharpDX;

namespace TgcViewer.Utils.PortalRendering
{
    /// <summary>
    /// Conexión unidireccional entre una celda y otra, a travez de un Portal
    /// </summary>
    public class TgcPortalRenderingConnection
    {
        public TgcPortalRenderingConnection(TgcPortalRenderingPortal portal, TgcPortalRenderingCell nextCell, TgcConvexPolygon polygon, Plane plane)
        {
            this.portal = portal;
            this.nextCell = nextCell;
            this.polygon = polygon;
            this.plane = plane;
        }

        private TgcPortalRenderingPortal portal;
        /// <summary>
        /// Portal de la conexion
        /// </summary>
        public TgcPortalRenderingPortal Portal
        {
            get { return portal; }
        }

        private TgcPortalRenderingCell nextCell;
        /// <summary>
        /// Celda con la cual comunica
        /// </summary>
        public TgcPortalRenderingCell NextCell
        {
            get { return nextCell; }
            set { nextCell = value; }
        }

        private TgcConvexPolygon polygon;
        /// <summary>
        /// Polígono plano que representa el portal.
        /// Los vértices están en clockwise-order según la celda de origen
        /// </summary>
        public TgcConvexPolygon Polygon
        {
            get { return polygon; }
        }

        private Plane plane;
        /// <summary>
        /// Plano del portal, apuntando hacia la celda origen
        /// </summary>
        public Plane Plane
        {
            get { return plane; }
        }

        public override string ToString()
        {
            return "Connection: [" + portal.ToString() + "] - [" + nextCell.ToString() + "]";
        }

    }
}
