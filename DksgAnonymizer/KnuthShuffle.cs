using System;
using System.Linq;

namespace DksgAnonymizer
{
	public class KnuthShuffle
	{
		public KnuthShuffle ()
		{
		}

		/// <summary>
		/// Gets a random permutation of the integers from 0 to max_value inclusive,
		/// using the Knuth Shuffle. See: http://en.wikipedia.org/wiki/Random_permutation
		/// </summary>
		/// <param name="max_value">Maximum value.</param>
		/// <param name="seed">Seed for the linear congruential generator.</param>
		public int[] get_random_permutation(int max_value, int seed)
		{
			var rand_perm = new int[max_value + 1];
			for (int i = 0; i <= max_value; i++) {
				rand_perm[i] = i;
			}

			var lng = new LinearCongruentialGenerator (seed);

			for (int i = 0; i < max_value; i++) {
				int j = lng.next (i, max_value); 
				int swap = rand_perm [i];
				rand_perm [i] = rand_perm [j];
				rand_perm [j] = swap;
			}

			return rand_perm;
		}
	}
}

