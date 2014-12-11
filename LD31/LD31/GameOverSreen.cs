using OpenTK.Input;
using OpenTKUtil.Minimal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class GameOverSreen : Screen
	{
		public const string DefaultText = "Game over! (Press Enter)";
		public string Text = DefaultText;

		public override void Update()
		{
			if (Input.NewKey(Key.Enter))
			{
				Game.Instance.SetState(GameState.Menu);
			}
		}

		public override void Render()
		{
			Game.Instance.Game_Screen.RenderPoints();
			Fonts.RenderStringCentered(Text, Game.Instance.ClientCenter, "pdos25");
			Game.Instance.RenderCredits();
		}
	}
}
