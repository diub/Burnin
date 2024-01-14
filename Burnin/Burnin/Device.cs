using IMAPI2.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace diub.Burnin;

/// <summary>
/// Created  2023-12-07 by diub - Dipl.-Ing. Uwe Barth
/// </summary>
public partial class Burnin {

	/// <summary>
	/// Drive letter mapping to unique id.
	/// </summary>
	private Dictionary<char, BurninDevice> devices = new Dictionary<char, BurninDevice> ();

	public Burnin () {
		UpdateDeviceList ();
	}

	private bool UpdateDeviceList () {
		List<string> strl;
		MsftDiscMaster2 discMaster = null;
		MsftDiscRecorder2 discRecorder = null;

		devices.Clear ();
		discMaster = new MsftDiscMaster2 ();
		if (!discMaster.IsSupportedEnvironment)
			return false;
		try {
			foreach (string uniqueRecorderId in discMaster) {
				discRecorder = new MsftDiscRecorder2 ();
				discRecorder.InitializeDiscRecorder (uniqueRecorderId);
				strl = new List<string> ();
				foreach (var item in discRecorder.VolumePathNames) {
					strl.Add (item.ToString ());
				}
				if (strl.Count > 0)
					devices.Add (strl [0].ToLower () [0], new BurninDevice () { Drive = strl [0].ToLower () [0], UniqueDriveId = uniqueRecorderId });
			}
		} catch (Exception) { }
		return true;
	}

	public List<string> ListDevicesLetters () {
		List<string> items;

		items = new List<string> ();
		foreach (char item in devices.Keys)
			items.Add (item + ":");
		return items;
	}

	public BurninDevice GetDevice (string Drive) {
		char drive;
		BurninDevice device;

		drive = Drive.ToLower () [0];
		if (devices.TryGetValue (drive, out device))
			return device;
		return null;
	}


	public bool Eject (string Drive) {
		return Eject (devices [Drive.ToLower () [0]]);
	}

	public bool Eject (BurninDevice Device) {
		MsftDiscRecorder2 discRecorder = null;

		try {
			discRecorder = new MsftDiscRecorder2 ();
			discRecorder.InitializeDiscRecorder (Device.UniqueDriveId);
			return true;
		} catch (Exception) {
			return false;
		}
	}

	public bool CloseTray (string Drive) {
		return CloseTray (devices [Drive.ToLower () [0]]);
	}

	public bool CloseTray (BurninDevice Device) {
		MsftDiscRecorder2 discRecorder = null;

		try {
			discRecorder = new MsftDiscRecorder2 ();
			discRecorder.InitializeDiscRecorder (Device.UniqueDriveId);
			discRecorder.CloseTray ();
			//object o = discRecorder.CloseTray ();
			//return (int) o == 0;
			return true;
		} catch (Exception e) {
			return false;
		}
	}

	/// <summary>
	/// Ist ein Medium eingelegt?
	/// </summary>
	/// <param name="Drive"></param>
	/// <returns></returns>
	public bool IsLoaded (string Drive) {
		return IsLoadedMedia (devices [Drive.ToLower () [0]]);
	}

	/// <summary>
	/// Ist ein Medium eingelegt?
	/// </summary>
	/// <param name="Device"></param>
	/// <returns></returns>
	public bool IsLoadedMedia (BurninDevice Device) {
		bool is_loaded;
		MsftDiscMaster2 discMaster = null;
		MsftDiscRecorder2 discRecorder = null;
		MsftDiscFormat2Data discFormatData = null;

		discMaster = new MsftDiscMaster2 ();
		if (!discMaster.IsSupportedEnvironment)
			return false;

		discFormatData = new MsftDiscFormat2Data ();
		discRecorder = new MsftDiscRecorder2 ();
		discRecorder.InitializeDiscRecorder (Device.UniqueDriveId);

		try {
			discFormatData = new MsftDiscFormat2Data {
				Recorder = discRecorder,
				ClientName = "eh_Burnin",
				ForceMediaToBeClosed = true
			};
			is_loaded = !discFormatData.MediaPhysicallyBlank;
		} catch (Exception) {
			return false;
		} finally {
			if (discFormatData != null)
				Marshal.ReleaseComObject (discFormatData);
		}
		return is_loaded;
	}

	/// <summary>
	/// Liefert auch False bei fehlendem Medium.
	/// </summary>
	/// <param name="Drive"></param>
	/// <returns></returns>
	public bool IsBlankMedia (string Drive) {
		return IsBlankMedia (devices [Drive.ToLower () [0]]);
	}

	/// <summary>
	/// Liefert auch False bei fehlendem Medium.
	/// </summary>
	/// <param name="Device"></param>
	/// <returns></returns>
	public bool IsBlankMedia (BurninDevice Device) {
		bool is_blank;
		MsftDiscMaster2 discMaster = null;
		MsftDiscRecorder2 discRecorder = null;
		MsftDiscFormat2Data discFormatData = null;

		discMaster = new MsftDiscMaster2 ();
		if (!discMaster.IsSupportedEnvironment)
			return false;

		discFormatData = new MsftDiscFormat2Data ();
		discRecorder = new MsftDiscRecorder2 ();
		discRecorder.InitializeDiscRecorder (Device.UniqueDriveId);

		try {
			discFormatData = new MsftDiscFormat2Data {
				Recorder = discRecorder,
				ClientName = "eh_Burnin",
				ForceMediaToBeClosed = true
			};
			is_blank = discFormatData.MediaHeuristicallyBlank;
		} catch (Exception) {
			return false;
		} finally {
			if (discFormatData != null)
				Marshal.ReleaseComObject (discFormatData);
		}
		return is_blank;
	}

	/// <summary>
	/// Abfrage auf ein eingelegtes *und* leeres Medium!
	/// </summary>
	/// <param name="Drive"></param>
	/// <returns></returns>
	public bool IsLoadedBlankMedia (string Drive) {
		return IsLoadedBlankMedia (devices [Drive.ToLower () [0]]);
	}

	/// <summary>
	/// Abfrage auf ein eingelegtes *und* leeres Medium!
	/// </summary>
	/// <param name="Device"></param>
	/// <returns></returns>
	public bool IsLoadedBlankMedia (BurninDevice Device) {
		bool is_loaded, is_blank;

		MsftDiscMaster2 discMaster = null;
		MsftDiscRecorder2 discRecorder = null;
		MsftDiscFormat2Data discFormatData = null;

		discMaster = new MsftDiscMaster2 ();
		if (!discMaster.IsSupportedEnvironment)
			return false;

		discFormatData = new MsftDiscFormat2Data ();
		discRecorder = new MsftDiscRecorder2 ();
		discRecorder.InitializeDiscRecorder (Device.UniqueDriveId);

		try {
			discFormatData = new MsftDiscFormat2Data {
				Recorder = discRecorder,
				ClientName = "eh_Burnin",
				ForceMediaToBeClosed = true
			};
			is_loaded = !discFormatData.MediaPhysicallyBlank;
			is_blank = discFormatData.MediaHeuristicallyBlank;
		} catch (Exception) {
			return false;
		} finally {
			if (discFormatData != null)
				Marshal.ReleaseComObject (discFormatData);
		}
		return is_loaded && is_blank;
	}

}   // class

//	namespace	2023-12-07 - 14.17.58
