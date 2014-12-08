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
		public PlayerRect(float width, float height)
			: base(Game.Instance.ClientCenter.X, Game.Instance.ClientCenter.Y, width, height, 3, Color.Green)
		{

		}

		public override void Update()
		{
			bool leftKey = Input.Key(Key.Left);
			bool rightKey = Input.Key(Key.Right);
			bool upKey = Input.Key(Key.Up);
			bool downKey = Input.Key(Key.Down);

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
