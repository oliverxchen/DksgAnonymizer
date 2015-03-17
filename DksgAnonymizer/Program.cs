using System;
using Gtk;

namespace DksgAnonymizer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length == 0) {
				Application.Init ();
				MainWindow win = new MainWindow ();
				win.Show ();
				Application.Run ();
			} else {
				Console.WriteLine ("Running DksgAnonymizer from command line...");
				string cmd = args [0];
				if (cmd == "anon") {
					var anon = new Anonymizer ();
					anon.write_from_console (0, 2313251);
				} else if (cmd == "deanon") {
					// TODO implement deanonymization by streaming from stdin
				}
			}
		}
	}
}
