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
		private const int minR = 200;
		private const int maxR = 256;
		private const int minG = 100;
		private const int maxG = 156;
		
		public float Size;

		private byte R, G;

		public HitParticle(Vector2 center, float size, float length, float speed, Direction direction)
			: this(center, size, length, speed)
		{
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
			FixDirection(speed);
		}

		public HitParticle(Vector2 center, float size, float length, float speed)
			: base(center, new Vector2(Rand.NextFloat() - 0.5f, Rand.NextFloat() - 0.5f), length)
		{
			Size = size;
			FixDirection(speed);
			Direction = Direction.Normalized() * (Rand.NextFloat() + 0.5f) * speed * (Rand.NextFloat() + 0.5f);

			R = (byte)Rand.Next(minR, maxR);
			G = (byte)Rand.Next(minG, maxG);
		}

		private void FixDirection(float speed)
		{
			Direction = Direction.Normalized() * (Rand.NextFloat() + 0.5f) * speed * (Rand.NextFloat() + 0.5f);
		}

		public override void Render()
		{
			GL.Color4(R, G, (byte)0, (byte)((Life / LifeLength) * 255));
			(new RectangleF(Position.X - Size / 2, Position.Y - Size / 2, Size, Size)).Render();
		}
	}
}
