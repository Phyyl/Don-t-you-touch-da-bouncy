using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTKUtil.Minimal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class PauseScreen : Screen
	{
		private Menu menu;

		public PauseScreen()
		{
			menu = new Menu("Resume", "Menu");
			menu.OnMenuItemSelected += menu_OnMenuItemSelected;
		}

		void menu_OnMenuItemSelected(int index)
		{
			switch (index)
			{
				case 0:
					Game.Instance.SetState(GameState.Playing);
					break;
				case 1:
					Game.Instance.SetState(GameState.Menu);
					break;
			}
		}

		public override void Update()
		{
			menu.Update();
		}

		public override void Render()
		{
			Game.Instance.Game_Screen.Render();
		
			GL.Disable(EnableCap.Texture2D);
			GL.Color4(0, 0, 0, 0.8f);
			Game.Instance.ClientRectangle.Render();

			menu.Render();
		}
	}
}
