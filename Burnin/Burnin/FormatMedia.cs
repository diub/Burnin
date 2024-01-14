using IMAPI2.Interop;
using System;
using System.Runtime.InteropServices;

namespace diub.Burnin;

/// <summary>
/// Created 2023-12-27 by diub - Dipl.-Ing. Uwe Barth
/// </summary>
public partial class Burnin {

	public bool FormatMedia (string Drive, bool FullErease = false) {
		try {
			return FormatMedia (devices [Drive.ToLower () [0]], FullErease);
		} catch (Exception) {
			return false;
		}
	}

	public bool FormatMedia (BurninDevice Device, bool FullErease = false) {
		MsftDiscFormat2Erase discFormatErase;
		MsftDiscRecorder2 discRecorder;

		discRecorder = new MsftDiscRecorder2 ();
		try {
			discRecorder.InitializeDiscRecorder (Device.UniqueDriveId);
			discFormatErase = new MsftDiscFormat2Erase {
				Recorder = discRecorder,
				ClientName = Firm.CLIENT_NAME,
				FullErase = FullErease
			};
			discFormatErase.Update += FormatEraseStatusUpdate;
			stopwatch.Restart ();
			try {
				discFormatErase.EraseMedia ();
				return true;
			} catch (COMException e) {
				return false;
			} finally {
				stopwatch.Stop ();
				discFormatErase.Update -= FormatEraseStatusUpdate;
				if (discRecorder != null)
					Marshal.ReleaseComObject (discRecorder);
				if (discFormatErase != null)
					Marshal.ReleaseComObject (discFormatErase);
			}
		} catch (Exception) {
			return false;
		}
	}


}   // class

//	namespace	2023-12-27 - 14.38.06
