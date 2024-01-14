using IMAPI2.Interop;
using IMAPI2.MediaItem;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace diub.Burnin;

/// <summary>
/// Created 2023-12-27 by diub - Dipl.-Ing. Uwe Barth
/// </summary>
public partial class Burnin {

	const int DVD_SECTOR_SIZE = 2048;
	const int DVD_1x_SPEED = 1385 * 1024;   // kByte * 1024 Byte

	/// <summary>
	/// Brennt die übergebenen Daten.
	/// </summary>
	/// <param name="Device"></param>
	/// <param name="VolumeName"></param>
	/// <param name="MediaItems"></param>
	/// <returns></returns>
	public int Burn (BurninDevice Device, string VolumeName, BurninItems MediaItems) {
		return Burn (Device, VolumeName, MediaItems.Items);
	}

	/// <summary>
	/// Brennt die übergebenen Daten.
	/// </summary>
	/// <param name="Device"></param>
	/// <param name="VolumeName"></param>
	/// <param name="MediaItems"></param>
	/// <returns>Status-Code: 0=Ok, -1=FileSystemError, -2=Unbekannter Fehler, +XX=Fehlercode vom Brennprozess.</returns>
	public int Burn (BurninDevice Device, string VolumeName, List<IMediaItem> MediaItems) {
		int speed;
		MsftDiscRecorder2 discRecorder;
		MsftDiscFormat2Data discFormatData;
		IStream istream;

		is_burning = true;
		discFormatData = null;

		discRecorder = null;
		discFormatData = null;
		try {
			discRecorder = new MsftDiscRecorder2 ();
			discRecorder.InitializeDiscRecorder (Device.UniqueDriveId);

			discFormatData = new MsftDiscFormat2Data {
				Recorder = discRecorder,
				ClientName = Firm.CLIENT_NAME,
				ForceMediaToBeClosed = close_media
			};

			istream = CreateMediaFileSystem (discRecorder, VolumeName, MediaItems);
			if (istream == null)
				return -1;
			discFormatData.Update += BurninStatusUpdate;
			(discFormatData as IBurnVerification).BurnVerificationLevel = Firm.VERIFICATION_LEVEL;

			stopwatch.Restart ();
			cancel_action = 0;
			try {
				speed = GetSpeed (discFormatData, 4);
				discFormatData.SetWriteSpeed (speed, false);
				discFormatData.Write (istream);
			} catch (COMException e) {
				return e.ErrorCode;
			} catch (Exception) {
				return -2;
			} finally {
				stopwatch.Stop ();
			}
			return 0;
		} catch (Exception) {
			return -2;
		} finally {
			if (discRecorder != null)
				Marshal.ReleaseComObject (discRecorder);
			if (discFormatData != null) {
				//discFormatData.Update -= BurninStatusUpdate;
				Marshal.ReleaseComObject (discFormatData);
			}
		}
	}

	/// <summary>
	/// Liefert eine Geschwindigkeit in Sektoren/Sekunde in bester Annäherung zum gewählten DVD-Schreib-Faktor.
	/// </summary>
	/// <param name="discFormatData"></param>
	/// <param name="DvdFactor">Gewählter DVD-Schreib-Faltor.</param>
	/// <returns></returns>
	private int GetSpeed (MsftDiscFormat2Data discFormatData, int DvdFactor) {
		int sectors;
		float speed, option, speed_diff, option_diff;
		object [] desc, speeds;

		speeds = discFormatData.SupportedWriteSpeeds;
		desc = discFormatData.SupportedWriteSpeedDescriptors;

		speed = 0;
		sectors = 0;
		foreach (object item in speeds) {
			option = ((int) item) * (float) DVD_SECTOR_SIZE / DVD_1x_SPEED;
			option_diff = Math.Abs (option - DvdFactor);
			speed_diff = Math.Abs (speed - DvdFactor);
			if (option_diff < speed_diff) {
				sectors = (int) item;
				speed = option;
			}
		}
		return sectors;
	}

	public void CancelAction () {
		if (cancel_action == 0)
			cancel_action = 1;
	}

}   // class

//	namespace	2023-12-27 - 14.09.32