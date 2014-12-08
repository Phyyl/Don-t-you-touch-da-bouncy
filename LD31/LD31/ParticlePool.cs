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
	}
}
