using System;
using Gtk;
using DksgAnonymizer;
using System.Reflection;
using System.IO;
using System.Diagnostics;

public partial class MainWindow: Gtk.Window
{
	private string[] input_files;
	private int[] email_cols;
	private Stopwatch sw;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

		input_files = new string[2];
		input_files [0] = "input_file_1.csv";
		input_files [1] = "input_file_2.csv";

		email_cols = new int[2];
		email_cols [0] = 0;
		email_cols [1] = 2;

		sw = new Stopwatch ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnGenerateInputClick (object sender, EventArgs e)
	{
		sw.Start ();

		var tfg = new TestFileGenerator ();
		tfg.write (input_files[0], email_cols[0], 4, 24655);
		tfg.write (input_files[1], email_cols[1], 5, 35268);

		sw.Stop ();
		labelStatus.LabelProp = "Finished generating test in: " + sw.ElapsedMilliseconds.ToString() + "ms.";
	}

	protected void OnAnonymizeClick (object sender, EventArgs e)
	{
		sw.Start ();

		int seed = 0;
		try	{
			seed = Convert.ToInt32(entrySeed.Text);
		}
		catch {
			MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Warning, ButtonsType.Ok, "Please enter a valid integer seed.");
			md.Run ();
			md.Destroy();
		}

		var anon = new Anonymizer ();
		anon.write (input_files, email_cols, seed);

		sw.Stop ();
		labelStatus.LabelProp = "Finished anonymizing in: " + sw.ElapsedMilliseconds.ToString() + "ms.";
	}
}
