//
// IMediaItem.cs
//
// by Eric Haddan
//
using IMAPI2.Interop;
using System;

namespace IMAPI2.MediaItem {

	public interface IMediaItem {
		/// <summary>
		/// Returns the full path of the file or directory
		/// </summary>
		string Path {
			get;
		}

		/// <summary>
		/// Returns the size of the file or directory to the next largest sector
		/// </summary>
		Int64 SizeOnDisc {
			get;
		}

		/// <summary>
		/// Returns the Icon of the file or directory
		/// </summary>
		System.Drawing.Image FileIconImage {
			get;
		}

		// Adds the file or directory to the directory item, usually the root.
		bool AddToFileSystem (IFsiDirectoryItem rootItem);
	}
}
