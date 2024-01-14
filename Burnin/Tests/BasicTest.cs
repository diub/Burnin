namespace diub.Burnin;

public partial class Burnin {
#if !DEBUG

	static public void TestBurnin () {
		bool b;
		string str;
		Burnin burnin;
		BurninDevice device;

		burnin = new Burnin ();

		device = burnin.GetDevice ("W");
		b = burnin.IsLoadedBlankMedia (device);


		b = burnin.IsBlankMedia ("W");

		b = burnin.Eject (device);

		b = burnin.CloseTray (device);

	}

#endif
}   // class

//	namespace	2023-12-07 - 14.23.58