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

	private bool cancel = false;

	private bool is_burning = false;
	private bool close_media = true;
	private bool eject_media = false;

	//private BurnStatus burn_status = null;

	//private Dictionary<string, IMediaItem> items_to_burn = new Dictionary<string, IMediaItem> ();


}

