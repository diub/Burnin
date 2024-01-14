using IMAPI2.Interop;
using IMAPI2.MediaItem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace diub.Burnin;

/// <summary>
/// </summary>
public partial class Burnin {


	private Int64 GetFilesSize (List<IMediaItem> MediaItems) {
		Int64 size;

		size = 0;
		foreach (IMediaItem mediaItem in MediaItems)
			size += mediaItem.SizeOnDisc;
		return size;
	}

}   // class

//	namespace	2023-12-27 - 14.00.391
