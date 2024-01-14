using IMAPI2.Interop;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace diub.Burnin;

/// <summary>
/// Created 2023-12-27 by diub - Dipl.-Ing. Uwe Barth
/// </summary>
public partial class Burnin {

	public event EventHandler<BurninStatus> BurninStatusChanged;

	public event EventHandler<FormatEreaseProgressEventArg> FormatEreaseStatusChanged;

	private Stopwatch stopwatch = new Stopwatch ();

	/// <summary>
	/// Flag für Abbrechen Status: 0→Laufen laasen, 1→Abbrechen steht an, 2→Abgebrochen.
	/// </summary>
	private int cancel_action = 0;


	/// <summary>
	/// Gematsche ala Microsoft.<para></para>
	/// Callback Funktion für den Brennprozess.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="progress"></param>
	private void BurninStatusUpdate ([In, MarshalAs (UnmanagedType.IDispatch)] object sender, [In, MarshalAs (UnmanagedType.IDispatch)] object progress) {
		IDiscFormat2Data discFormat2;
		IDiscFormat2DataEventArgs eventArgs;
		BurninStatus burnin_status;

		if (cancel_action == 1) {
			cancel_action = 2;
			discFormat2 = (IDiscFormat2Data) sender;
			discFormat2.CancelWrite ();
			return;
		}

		if (BurninStatusChanged == null)
			return;
		if (stopwatch.ElapsedMilliseconds < 1000)
			return;
		eventArgs = (IDiscFormat2DataEventArgs) progress;

		burnin_status = new BurninStatus ();

		burnin_status.Task = BURN_STEPS.WRITING_TO_DISK;

		// IDiscFormat2DataEventArgs Interface
		burnin_status.ElapsedTime = eventArgs.ElapsedTime;
		burnin_status.RemainingTime = eventArgs.RemainingTime;
		burnin_status.TotalTime = eventArgs.TotalTime;

		// IWriteEngine2EventArgs Interface
		burnin_status.currentAction = eventArgs.CurrentAction;
		burnin_status.StartLba = eventArgs.StartLba;
		burnin_status.SectorCount = eventArgs.SectorCount;
		burnin_status.LastReadLba = eventArgs.LastReadLba;
		burnin_status.LastWrittenLba = eventArgs.LastWrittenLba;
		burnin_status.TotalSystemBuffer = eventArgs.TotalSystemBuffer;
		burnin_status.UsedSystemBuffer = eventArgs.UsedSystemBuffer;
		burnin_status.FreeSystemBuffer = eventArgs.FreeSystemBuffer;

		BurninStatusChanged.Invoke (this, burnin_status);
		stopwatch.Restart ();
	}

	private void FormatEraseStatusUpdate ([In, MarshalAs (UnmanagedType.IDispatch)] object sender, [In] int elapsedSeconds, [In] int estimatedTotalSeconds) {
		//MsftDiscFormat2Erase discFormat2;

		//if (cancel_action == 1) {
		//	cancel_action = 2;
		//	discFormat2 = (MsftDiscFormat2Erase) sender;
		//	discFormat2.();
		//	return;
		//}
		if (FormatEreaseStatusChanged == null)
			return;
		if (stopwatch.ElapsedMilliseconds < 1000)
			return;
		FormatEreaseStatusChanged.Invoke (this, new FormatEreaseProgressEventArg () { ElapsedSeconds = elapsedSeconds, EstimatedTotalSecond = estimatedTotalSeconds });
		stopwatch.Restart ();
	}

}   // class

//	namespace	2023-12-27 - 14.56.46
