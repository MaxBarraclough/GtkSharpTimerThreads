/*
using System;
using Gtk;

namespace GtkSharpHW
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.Init();
			MainWindow win = new MainWindow();
			win.Show();
			Application.Run();
		}
	}
}
/* */

//*
using Gtk;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

sealed class Hello
{
	// runs when the button is clicked.
	static void hello(Scale scale, Label label)
	{
		//System.Diagnostics.Debug.Assert(null != obj);
		//System.Diagnostics.Debug.Assert(null != args);
		System.Diagnostics.Debug.Assert(null != label);

		// Contract.Requires(obj != null); // much the same idea as an assert, right?

		// Console.WriteLine("Hello World");

		// what happens if we lock up the event thread?
		// System.Threading.Thread.Sleep(2000); // answer: total GUI freeze. Can't even drag the window!

		// we *definitely* want this to be in a different thread.
		// if execution takes place in current thread, we block the GUI.
		// Task.Run is essentially identical to Task.Factory.StartNew
		Task.Run( () => {

//			int result = Task.Run( //async
//			                         () =>
//				{
//					//await Task.Delay(1000);
//					return 42;
//				}).Result;

			Task t1 = Task.Run( () => {
				System.Threading.Thread.Sleep(1000);
				Console.WriteLine("Task 1 done");
				Gtk.Application.Invoke(
					(object sender, EventArgs e) =>  {
						label.Text = "1 done";
						scale.Adjustment.Value = 1.0d;
					}
				);

			});

			Task t2 = Task.Run( () => {
				System.Threading.Thread.Sleep(2000);
				Console.WriteLine("Task 2 done");
				Gtk.Application.Invoke(
					(object sender, EventArgs e) =>  {
						label.Text = "2 done";
						scale.Adjustment.Value = 2.0d;
					}
				);
			});

			Task t3 = Task.Run( () => {
				System.Threading.Thread.Sleep(3000);
				Console.WriteLine("Task 3 done");
				Gtk.Application.Invoke(
					(object sender, EventArgs e) =>  {
						label.Text = "3 done";
						scale.Adjustment.Value = 3.0d;
					}
				);
			});

			{
				Task[] ts = { t1, t2, t3 };
				Task.WaitAll(ts);
			}

			// can't get this to compile. https://stackoverflow.com/a/686592
			//System.Diagnostics.Debug.Assert.IsInstanceOfType( Hello.btn.Child.GetType, Label );

			// http://blog.drorhelper.com/2011/02/multiple-asserts-done-right.html
			//var result = 2 + 2;
			//System.Diagnostics.Debug.Assert.IsInstanceOfType(result, typeof(double));

//			System.Diagnostics.Debug.Assert(
//				((object)Hello.btn.Child).GetType() == Label
//			);

			Gtk.Application.Invoke(
				//delegate {  label.Text = "All done";  }
				(object sender, EventArgs e) => {
					label.Text = "All done";
					scale.Adjustment.Value = scale.Adjustment.Upper;
				} // delegate and lambda seem equivalent here
			);

		});

		//Application.Quit();
	}


	// runs when the user deletes the window using the "close
	// window" widget in the window frame.
	static void delete_event(object obj, DeleteEventArgs args)
	{
		// Console.WriteLine("Deletion time");
		Application.Quit();
	}

	static void Main()
	{
		Application.Init(); // Gtk# init

		Console.WriteLine("Application starting up");

		// Create HScale slider before Button, but add them in opposite order

		// see http://www.gtk.org/tutorial1.2/gtk_tut-7.html
		// http://inti.sourceforge.net/tutorial/libinti/rangewidgets.html
		// http://www.mono-project.com/docs/gui/gtksharp/widgets/range-widgets/
		Adjustment adj = new Adjustment(
			5.0d,  // Initial value
			0.0d,  // Lower limit
			10.0d, // Upper limit
			0.5,   // Step increment
			2.0d,  // Page increment
			0.0d   // Page size, seems like it should be 0 for a slider,
			       // as it's non-panning, and it avoid surprising subtraction
		);

		HScale hs = new HScale(adj);
		var p = new PositionType();
		hs.AddMark(0.1, p, null); // breaks build in Windows!
		// hs.addMark SHOULD BE AVAILABLE! Old GTK# library?

		Label label = new Label("Start timer threads");
		Button btn  = new Button(label);

		btn.Clicked += new EventHandler( (object obj, EventArgs args) => { hello(hs,label); } );

		Window window = new Window("Timer Thread Demo");

		// when this window is deleted, run delete_event()
		window.DeleteEvent += delete_event;

		Box box = new VBox();
		box.Add(btn);

		box.Add(hs);
		window.Add(box);
		window.ShowAll();
		Application.Run();
	}

}
/* */

/*
// Xamarin: be sure to add System.Windows.Forms to References
using System;
using System.Windows.Forms;

public class HelloWorld : Form
{
	static public void Main()
	{
		Application.Run(new HelloWorld());
	}

	public HelloWorld()
	{
		Text = "Hello Mono World";
	}
}

/* */
