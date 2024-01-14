namespace diub.Burnin;

public partial class Burnin {

#if !DEBUG

	static public void FormatTests () {
		bool b;
		Burnin burnin;
		BurninDevice device;

		burnin = new Burnin ();

		device = burnin.GetDevice ("W");

		b = burnin.FormatMedia (device);
	}

#endif
}   // class

//	namespace	2024-01-03 - 15.48.20