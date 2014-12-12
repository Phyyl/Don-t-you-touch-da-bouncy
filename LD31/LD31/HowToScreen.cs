using OpenTK;
using OpenTK.Input;
using OpenTKUtil.Minimal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class HowToScreen : Screen
	{
		private bool single = true;

		public override void Update()
		{
			if (Input.NewKey(Key.Escape))
			{
				Game.Instance.SetState(GameState.Menu);
			}
			else if (Input.NewKey(Key.Right))
			{
				single = false;
			}
			else if (Input.NewKey(Key.Left))
			{
				single = true;
			}
		}

		public override void Render()
		{
			if (single)
			{
				Fonts.RenderStringCentered("Singleplayer", new Vector2(Game.Instance.ClientCenter.X, 25), "pdos25");
				Vector2 position = new Vector2(15, 75);
				Vector2 increment = new Vector2(0, 25);
				Fonts.RenderString("You are the green square", position += increment, Vector2.Zero, "pdos16");
				Fonts.RenderString("You must never touch da bouncy", position += increment, Vector2.Zero, "pdos16");
				Fonts.RenderString("NEVER", position += increment * 2, Vector2.Zero, "pdos16");
				Fonts.RenderString("Avoid it using WASD or the arrows", position += increment * 2, Vector2.Zero, "pdos16");
				Fonts.RenderString("2 points when the bouncy bounces", position += increment * 2, Vector2.Zero, "pdos16");
				Fonts.RenderString("1 point when the bouncy's children bounce", position += increment, Vector2.Zero, "pdos16");
				Fonts.RenderString("Have fun!", position += increment * 2, Vector2.Zero, "pdos25");

				Fonts.RenderStringCentered("=>", Game.Instance.ClientSize - new Vector2(25), "pdos16");
			}
			else
			{
				Fonts.RenderStringCentered("Multiplayer", new Vector2(Game.Instance.ClientCenter.X, 25), "pdos25");
				Vector2 position = new Vector2(15, 75);
				Vector2 increment = new Vector2(0, 25);
				Fonts.RenderString("Player1 the green square with WASD", position += increment, Vector2.Zero, "pdos16");
				Fonts.RenderString("Player2 the blue square with the arrows", position += increment, Vector2.Zero, "pdos16");
				Fonts.RenderString("You must never touch da bouncy", position += increment, Vector2.Zero, "pdos16");
				Fonts.RenderString("NEVER", position += increment * 2, Vector2.Zero, "pdos16");
				Fonts.RenderString("You must never touch your teammate's square", position += increment * 2, Vector2.Zero, "pdos16");
				Fonts.RenderString("NEVER", position += increment * 2, Vector2.Zero, "pdos16");
				Fonts.RenderString("2 points when the bouncy bounces", position += increment * 2, Vector2.Zero, "pdos16");
				Fonts.RenderString("1 point when the bouncy's children bounce", position += increment, Vector2.Zero, "pdos16");
				Fonts.RenderString("Have fun!", position += increment * 2, Vector2.Zero, "pdos25");

				Fonts.RenderStringCentered("<=", new Vector2(25, Game.Instance.ClientSize.Y - 25), "pdos16");
			}
		}
	}
}
