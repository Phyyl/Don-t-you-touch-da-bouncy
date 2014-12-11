using OpenTK;
using OpenTK.Input;
using OpenTKUtil.Minimal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class PlayerRect : GameRect
	{
		public enum PlayerInputMode
		{
			WASD = 1, Arrows = 2
		}

		private PlayerInputMode playerInputMode;

		public PlayerRect(PlayerInputMode playerInputMode)
			: base(Game.Instance.ClientCenter.X, Game.Instance.ClientCenter.Y, 15, 15, 3, Color.Green)
		{
			this.playerInputMode = playerInputMode;
		}

		public override void Update()
		{
			bool wasd = (playerInputMode & PlayerInputMode.WASD) == PlayerInputMode.WASD;
			bool arrows = (playerInputMode & PlayerInputMode.Arrows) == PlayerInputMode.Arrows;
			bool leftKey = (Input.Key(Key.Left) && arrows) | (Input.Key(Key.A) && wasd);
			bool rightKey = (Input.Key(Key.Right) && arrows) | (Input.Key(Key.D) && wasd);
			bool upKey = (Input.Key(Key.Up) && arrows) | (Input.Key(Key.W) && wasd);
			bool downKey = (Input.Key(Key.Down) && arrows) | (Input.Key(Key.S) && wasd);

			Vector2 movement = new Vector2();

			if (leftKey ^ rightKey)
			{
				if (leftKey) movement.X = -1;
				else if (rightKey) movement.X = 1;
			}

			if (upKey ^ downKey)
			{
				if (upKey) movement.Y = -1;
				else if (downKey) movement.Y = 1;
			}

			if (movement.Length != 0)
			{
				Center += movement.Normalized() * Speed;
			}

			base.Update();
		}
	}
}
