using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DksgAnonymizer
{
	public class Anonymizer
	{
		Dictionary<string, int> map;
		private int[] rand_perm;
		private int index;
		private const string delimiter = ",";
		private const char delimiter_char = ',';
		private const int batch_size = 1000;

		public Anonymizer ()
		{
			map = new Dictionary<string, int> ();
			index = 0;
		}

		/// <summary>
		/// Write the anonymized files and mapping from original data to anonymized values. 
		/// </summary> 
		/// <param name="input_file_names">File names.</param>
		/// <param name="linked_cols">Linked columns.</param>
		/// <param name="seed">Seed.</param>
		public void write(string[] input_file_names, int[] linked_cols, int seed)
		{
			int n_rows = 0;
			int n_files = input_file_names.Length;

			for (int i = 0; i < n_files; i++) {
				n_rows += countLineBreaksInFile (input_file_names [i]);
			}

			var ks = new KnuthShuffle ();
			rand_perm = ks.get_random_permutation (n_rows, seed);

			for (int i = 0; i < n_files; i++) {
				write_anonymized_file (input_file_names [i], linked_cols [i]);
			}

			write_map ("anonymizing_map.csv");
		}


		/// <summary>
		/// Writes out the map from original value to anonymized value. 
		/// We should probably also encrypt this is in a password-protected zip file.
		/// </summary>
		/// <param name="map_name">Map_name.</param>
		private void write_map (string map_name)
		{
			using (TextWriter tw = new StreamWriter (map_name)) {
				tw.WriteLine("original_value,anonymized_value");

				foreach (var key_value in map) {
					tw.WriteLine(key_value.Key + "," + key_value.Value.ToString());
				}
			}	
		}

		/// <summary>
		/// Write anonymized file by randomly enumerating the linked column and then sorting
		/// the random enumeration in batches.
		/// </summary>
		/// <param name="file_name">File_name.</param>
		/// <param name="linked_col">Linked_col.</param>
		private void write_anonymized_file(string file_name, int linked_col)
		{
			string output_file_name = file_name.Substring(0, file_name.LastIndexOf(".")) + "_anon.csv";

			using (TextReader tr = new StreamReader (file_name))
			using (TextWriter tw = new StreamWriter (output_file_name))
			{
				string line = tr.ReadLine ();
				tw.WriteLine (line);

				int counter = 0;
				var batch = new string[batch_size];	
				var batch_index = new int[batch_size];

				line = tr.ReadLine ();
				while (line != null) {
					int row_count = counter % batch_size;

					string[] tokens = line.Split (delimiter_char);
					batch_index [row_count] = get_set_map (tokens [linked_col]); 
					tokens [linked_col] = batch_index [row_count].ToString ();
					batch [row_count] = String.Join (delimiter, tokens);

					line = tr.ReadLine ();

					if (row_count + 1 == batch_size) {
						write_batch (tw, batch_index, batch);
					
						batch_index = new int[batch_size];
						batch = new string[batch_size];
					} 
					else if (line == null) {
						var short_batch_index = batch_index.Take (row_count + 1).ToArray ();
						var short_batch = batch.Take (row_count + 1).ToArray ();

						write_batch (tw, short_batch_index, short_batch);
					}

					counter++;
				}
			}
		}

		private void write_batch(TextWriter tw, int[] batch_index, string[] batch)
		{
			Array.Sort (batch_index, batch);
			tw.WriteLine(String.Join(Environment.NewLine, batch));
		}


		/// <summary>
		/// Establishes the link between fields in different files.
		/// Retrieves the previously set value for a key if it's been used before.
		/// If it hasn't been set previously, set a value now and return that.
		/// Case-insensitive and white-space trimmed.
		/// </summary>
		/// <param name="key">Key.</param>
		private int get_set_map(string key)
		{
			int value;

			key = key.Trim().ToLower();

			if (!map.TryGetValue (key, out value)) {
				value = rand_perm [index];
				map.Add(key, value);
				index++;
			}

			return value;
		}

		/// <summary>
		/// Number of linebreaks (platform independent) in the file.
		/// </summary>
		/// <returns>The number of linebreaks in file.</returns>
		/// <param name="file_name">File name.</param>
		private int countLineBreaksInFile(string file_name)
		{
			using (TextReader tr = new StreamReader (file_name)) {
				string whole_file = tr.ReadToEnd();
				return whole_file.Count (c => c == Convert.ToChar(Environment.NewLine));
			}
		}
	}
}

