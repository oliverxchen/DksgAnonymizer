using System;
using System.Linq;
using System.IO;

namespace DksgAnonymizer
{
	public class TestFileGenerator
	{
		private const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		private Random rnd;
		private const string delimiter = ",";

		public TestFileGenerator ()
		{
			rnd = new Random ();
		}

		public void write(string file_name, int email_col, int n_cols, int n_rows)
		{
			using (TextWriter tw = new StreamWriter (file_name)) {
				for (int i = 0; i < n_cols; i++) {
					if (i != email_col) {
						tw.Write ("field_" + i.ToString () + delimiter);
					} 
					else {
						tw.Write ("email" + delimiter);
					}
				}
				tw.Write (Environment.NewLine);

				for (int j = 0; j < n_rows; j++) {
					for (int i = 0; i < n_cols; i++) {
						if (i != email_col) {
							tw.Write (getRandomString (5) + delimiter);
						} 
						else {
							tw.Write (getRandomString (1) + "@datakind.org" + delimiter);
						}
					}
					tw.Write (Environment.NewLine);
				}
			}
		}

		private string getRandomString(int length)
		{
			var result = new string(
				Enumerable.Repeat(chars, length)
				.Select(s => s[rnd.Next(s.Length)])
				.ToArray());

			return result;
		}

	}
}

