using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKUtil.Minimal;

namespace LD31
{
	public abstract class BaseEnemyRect : GameRect
	{
		public Vector2 Direction;

		public BaseEnemyRect(float x, float y, float width, float height)
			: base(x, y, width, height, 1.5f, Color.Yellow)
		{
			RandomizeDirection();
		}

		protected void RandomizeDirection()
		{
			Direction = new Vector2((float)Game.Instance.Random.NextDouble() - 0.5f, (float)Game.Instance.Random.NextDouble() - 0.5f);
		}

		public override void Update()
		{
			Center += Direction.Normalized() * Speed;

			byte r = (byte)(Game.Instance.Random.Next() % 256);
			byte g = 0;
			byte b = 0;
			if (r < 255)
			{
				g = (byte)(Game.Instance.Random.Next(256 - r));

				if ((g + r) < 255)
				{
					b = (byte)(Game.Instance.Random.Next(256 - g - r));
				}
			}

			base.Update();
		}

		public override void HitLeft()
		{
			RandomizeDirection();
			Direction.X = Math.Abs(Direction.X);
			Game.Instance.Points += 1;
		}

		public override void HitRight()
		{
			RandomizeDirection();
			Direction.X = -Math.Abs(Direction.X);
			Game.Instance.Points += 1;
		}

		public override void HitTop()
		{
			RandomizeDirection();
			Direction.Y = Math.Abs(Direction.Y);
			Game.Instance.Points += 1;
		}

		public override void HitBottom()
		{
			RandomizeDirection();
			Direction.Y = -Math.Abs(Direction.Y);
			Game.Instance.Points += 1;
		}
	}
}
