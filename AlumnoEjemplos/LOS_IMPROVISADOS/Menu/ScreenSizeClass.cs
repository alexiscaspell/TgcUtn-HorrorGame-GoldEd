/*
 * Created by SharpDevelop.
 * User: Lelouch
 * Date: 09/06/2016
 * Time: 23:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using TgcViewer;

namespace AlumnoEjemplos.LOS_IMPROVISADOS
{
	/// <summary>
	/// Description of ScreenSize.
	/// </summary>
	public static class ScreenSizeClass
	{
		private static bool yaActivado = false;
		private static Size screenSize;

		/// <summary>
		/// Fuerza que el tamaño se lea nuevamente del panel en el próximo acceso.
		/// Llamar después de que el form esté completamente inicializado.
		/// </summary>
		public static void Reset() { yaActivado = false; }

		public static Size ScreenSize
		{
			get{
				if(!yaActivado){
					var size = GuiController.Instance.Panel3d.Size;
					// Guard: si el tamaño es inválido (0,0 pre-layout), no cacheamos
					if (size.Width > 0 && size.Height > 0)
					{
						screenSize = size;
						yaActivado = true;
					}
					else
					{
						// Fallback a resolución del monitor primario
						screenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
					}
				}
				return screenSize;
			}
		}
	}
}
