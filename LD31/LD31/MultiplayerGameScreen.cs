using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class MultiplayerGameScreen : GameScreen
	{
		public PlayerRect Player2Rectangle;

		public MultiplayerGameScreen()
		{
			saveFile = "multi.dat";
		}

		public override void Update()
		{
			Player2Rectangle.Update();
			base.Update();
		}

		public override void Render()
		{
			Player2Rectangle.Render();
			base.Render();
		}

		public override void Reset()
		{
			base.Reset();
			Player2Rectangle = new PlayerRect(PlayerRect.PlayerInputMode.Arrows)
			{
				Color = Color.Blue
			};
			PlayerRectangle = new PlayerRect(PlayerRect.PlayerInputMode.WASD);
		}

		protected override void RenderRects()
		{
			base.RenderRects();
			Player2Rectangle.Render();
		}

		protected override bool CheckPlayerCollisions(EnemyRect rect)
		{
			return base.CheckPlayerCollisions(rect) || Player2Rectangle.Rectangle.IntersectsWith(rect.Rectangle);
		}
	}
}
