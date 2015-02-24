using System;

namespace DksgAnonymizer
{
	public class LinearCongruentialGenerator
	{
		/// <summary>
		/// Multiplier used in LCG by Numerical Recipes.
		/// </summary>
		private const int a = 1664525;

		/// <summary>
		/// Increment used in LCG by Numerical Recipes.
		/// </summary>
		private const int c = 1013904223;

		/// <summary>
		/// The current number in the sequence.
		/// </summary>
		private int x;

		/// <summary>
		/// Generates an array of random integers using a linear congruential generator.
		/// See: http://en.wikipedia.org/wiki/Linear_congruential_generator
		/// </summary>
		/// <returns>The random int array.</returns>
		/// <param name="seed">Seed of the sequence.</param>
		public LinearCongruentialGenerator (int seed)
		{
			x = seed;
		}
			
		/// <summary>
		/// Next number in sequence. 
		/// </summary>
		/// <param name="min_value">Minimum value (inclusive).</param>
		/// <param name="max_value">Maximum value (inclusive).</param>
		public int next(int min_value, int max_value)
		{
			x = (x * a + c) & int.MaxValue;

			int mod_value = max_value - min_value + 1;
			return min_value + x % mod_value;
		}
	}
}

