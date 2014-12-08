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
	public class EnemyRect : GameRect
	{
		public Vector2 Direction;

		public EnemyRect(float x, float y, float width, float height)
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
			base.Update();
		}

		public override void HitLeft()
		{
			Direction.X = (float)Math.Abs(Game.Instance.Random.NextDouble());
			Game.Instance.Points += 1;
		}

		public override void HitRight()
		{
			Direction.X = -(float)Math.Abs(Game.Instance.Random.NextDouble());
			Game.Instance.Points += 1;
		}

		public override void HitTop()
		{
			Direction.Y = (float)Math.Abs(Game.Instance.Random.NextDouble());
			Game.Instance.Points += 1;
		}

		public override void HitBottom()
		{
			Direction.Y = -(float)Math.Abs(Game.Instance.Random.NextDouble());
			Game.Instance.Points += 1;
		}
	}
}
