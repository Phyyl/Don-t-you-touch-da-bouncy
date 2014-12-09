using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public static class Rand
	{
		private static Random random = new Random();
		public static int Next()
		{
			return random.Next();
		}
		public static int Next(int maxValue)
		{
			return random.Next(maxValue);
		}
		public static int Next(int minValue, int maxValue)
		{
			return random.Next(minValue,maxValue);
		}

		public static float NextFloat()
		{
			return (float)random.NextDouble();
		}
		public static double NextDouble()
		{
			return random.NextDouble();
		}

		public static void NextBytes(byte[] bytes)
		{
			random.NextBytes(bytes);
		}
	}
}
