using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTKUtil.Minimal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class MenuScreen : Screen
	{
		private static readonly string[] bounceSounds = new string[] { "bounce", "bounce2", "bounce3", "bounce4" };
		private Menu menu;
		private ParticlePool particles;

		public MenuScreen()
		{
			menu = new Menu("Play", "Exit");
			menu.OnMenuItemSelected += menu_OnMenuItemSelected;

			particles = new ParticlePool();
		}

		void menu_OnMenuItemSelected(int index)
		{
			switch (index)
			{
				case 0:
					Game.Instance.Game_Screen.Reset();
					Game.Instance.SetState(GameState.Playing);
					break;
				case 1:
					Game.Instance.Close();
					break;
			}
		}

		public override void Update()
		{
			menu.Update();
			particles.Update();
			if (Rand.NextFloat()<0.05f)
			{
				particles.GenerateExplosion(new Vector2(Rand.NextFloat(), Rand.NextFloat()) * Game.Instance.ClientSize, 50, 3, 30, 10);
				Sounds.Play(bounceSounds[Rand.Next(bounceSounds.Length)]);
			}
		}

		public override void Render()
		{
			menu.Render();
			GL.Disable(EnableCap.Texture2D);
			particles.Render();
			Game.Instance.Game_Screen.RenderCredits();
		}
	}
}
