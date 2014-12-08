using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LD31
{
	public class EnemyRect : BaseEnemyRect
	{
		private byte yellow;
		public EnemyRect(float x, float y, float width, float height)
			: base(x, y, width, height)
		{

		}

		public override void Update()
		{
			yellow += 15;
			Color = Color.FromArgb(255, 255, yellow);
			base.Update();
		}
	}
}
