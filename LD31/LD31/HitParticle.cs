using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKUtil.Minimal;
using OpenTK.Graphics.OpenGL;

namespace LD31
{
	public class HitParticle : Particle
	{
		public float Size;

		public HitParticle(Vector2 center, float size, float length, float speed, Direction direction)
			: base(center, new Vector2((float)Game.Instance.Random.NextDouble() - 0.5f, (float)Game.Instance.Random.NextDouble() - 0.5f) * speed, length)
		{
			Size = size;
			switch (direction)
			{
				case LD31.Direction.Right:
					Direction.X = -Math.Abs(Direction.X);
					break;
				case LD31.Direction.Left:
					Direction.X = Math.Abs(Direction.X);
					break;
				case LD31.Direction.Down:
					Direction.Y = -Math.Abs(Direction.Y);
					break;
				case LD31.Direction.Up:
					Direction.Y = Math.Abs(Direction.Y);
					break;
			}
			Direction = Direction.Normalized() * (float)(Game.Instance.Random.NextDouble()+0.5f);
		}

		public override void Render()
		{
			GL.Color4((byte)255, (byte)127, (byte)0, (byte)((Life / LifeLength) * 255));
			(new RectangleF(Position.X - Size / 2, Position.Y - Size / 2, Size, Size)).Render();
		}
	}
}
