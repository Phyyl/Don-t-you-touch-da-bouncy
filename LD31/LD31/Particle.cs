using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public abstract class Particle
	{
		public Vector2 Position;
		public Vector2 Direction;
		public float Life;
		public float LifeLength;

		public bool Dead { get { return Life <= 0; } }

		public Particle(Vector2 position, Vector2 direction, float lifeLength)
		{
			Position = position;
			Direction = direction;
			LifeLength = lifeLength;
			Life = lifeLength;
		}

		public virtual void Update()
		{
			float distance = Math.Min(Direction.Length, Life);
			Life -= distance;
			Position += Direction.Normalized() * distance;
		}

		public abstract void Render();
	}
}
