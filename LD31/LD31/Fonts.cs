using OpenTK;
using QuickFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public static class Fonts
	{
		private static Dictionary<string, QFont> fonts = new Dictionary<string, QFont>();

		public static void Load(string name, string filename, float size, FontStyle style)
		{
			fonts.Add(name, new QFont(filename, size, style));
		}

		public static Vector2 MesureString(string str, string fontName)
		{
			var size = fonts[fontName].Measure(str);
			return new Vector2(size.Width, size.Height);
		}

		public static void RenderString(string str, Vector2 position, Vector2 origin, string fontName) { RenderString(str, position, origin, fontName, Color.White); }
		public static void RenderString(string str, Vector2 position, Vector2 origin, string fontName, Color color)
		{
			QFont.Begin();
			fonts[fontName].Options.Colour = new OpenTK.Graphics.Color4(color.R, color.G, color.B, color.A);
			fonts[fontName].Print(str, position - origin);
			QFont.End();
		}

		public static void RenderStringCentered(string str, Vector2 position, string fontName) { RenderStringCentered(str, position, fontName, Color.White); }
		public static void RenderStringCentered(string str, Vector2 position, string fontName, Color color)
		{
			RenderString(str, position, MesureString(str, fontName) / 2, fontName, color);
		}
	}
}
