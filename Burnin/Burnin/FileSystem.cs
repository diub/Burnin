using IMAPI2.Interop;
using IMAPI2.MediaItem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace diub.Burnin;

/// <summary>
/// Created 2023-12-27 by diub - Dipl.-Ing. Uwe Barth
/// </summary>
public partial class Burnin {

	/// <summary>
	/// Keine Multisession Unterstützung!!!
	/// </summary>
	/// <param name="discRecorder"></param>
	/// <param name="VolumeName"></param>
	/// <param name="MediaItems"></param>
	/// <param name="multisessionInterfaces"></param>
	/// <returns></returns>
	private IStream CreateMediaFileSystem (IDiscRecorder2 discRecorder, string VolumeName, List<IMediaItem> MediaItems) {
		MsftFileSystemImage fileSystemImage;
		IStream stream;
		IFsiDirectoryItem rootItem;

		fileSystemImage = null;

		try {
			fileSystemImage = new MsftFileSystemImage ();
			fileSystemImage.ChooseImageDefaults (discRecorder);
			fileSystemImage.FileSystemsToCreate = FsiFileSystems.FsiFileSystemJoliet | FsiFileSystems.FsiFileSystemISO9660;
			fileSystemImage.VolumeName = VolumeName;

			fileSystemImage.Update += FileSystemImageUpdate;

			// multisessions not supported !!!

			rootItem = fileSystemImage.Root;

			// Add Files and Directories to File System Image
			foreach (IMediaItem mediaItem in MediaItems) {
				if (cancel)
					break;
				mediaItem.AddToFileSystem (rootItem);
			}
			fileSystemImage.Update -= FileSystemImageUpdate;
			if (cancel)
				return null;

			stream = fileSystemImage.CreateResultImage ().ImageStream;
		} catch (Exception) {
			return null;
		} finally {
			if (fileSystemImage != null)
				Marshal.ReleaseComObject (fileSystemImage);
		}
		return stream;
	}

	private void FileSystemImageUpdate ([In, MarshalAs (UnmanagedType.IDispatch)] object sender, [In, MarshalAs (UnmanagedType.BStr)] string currentFile, [In] int copiedSectors, [In] int totalSectors) {
		var percentProgress = 0;
		if (copiedSectors > 0 && totalSectors > 0)
			percentProgress = (copiedSectors * 100) / totalSectors;

		if (!string.IsNullOrEmpty (currentFile)) {
			var fileInfo = new FileInfo (currentFile);
			//_burnData.statusMessage = "Adding \"" + fileInfo.Name + "\" to image...";

			//
			// report back to the ui
			//
			//_burnData.task = BURN_MEDIA_TASK.BURN_MEDIA_TASK_FILE_SYSTEM;
			//backgroundBurnWorker.ReportProgress (percentProgress, _burnData);
		}

	}

}   // class

//	namespace	2023-12-27 - 14.24.56
