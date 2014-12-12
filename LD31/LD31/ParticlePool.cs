using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class ParticlePool
	{
		public List<Particle> Particles = new List<Particle>();

		public void Add(params Particle[] particles)
		{
			Particles.AddRange(particles);
		}

		public void Update()
		{
			Particles.RemoveAll((p) => p.Dead);
			foreach (var particle in Particles)
			{
				particle.Update();
			}
		}

		public void Render()
		{
			foreach (var particle in Particles)
			{
				particle.Render();
			}
		}

		public void Clear()
		{
			Particles.Clear();
		}

		public void GenerateExplosion(Vector2 center, int count, float size, float length, float speed, Direction direction)
		{
			for (int i = 0; i < count; i++)
			{
				Particles.Add(new HitParticle(center, 3, 30, 1, direction));
			}
		}

		public void GenerateExplosion(Vector2 center, int count, float size, float length, float speed)
		{
			for (int i = 0; i < count; i++)
			{
				Particles.Add(new HitParticle(center, 3, 30, 1));
			}
		}
	}
}
