using IMAPI2.MediaItem;
using System;
using System.Collections.Generic;

namespace diub.Burnin;

/// <summary>
/// Created 2023-12-27 by diub - Dipl.-Ing. Uwe Barth
/// </summary>
public partial class BurninItems {

	internal Dictionary<string, IMediaItem> media_items = new Dictionary<string, IMediaItem> ();

	public List<IMediaItem> Items {
		get {
			List<IMediaItem> items;

			items = new List<IMediaItem> ();
			foreach (IMediaItem item in media_items.Values)
				items.Add (item);
			return items;
		}
	}

	public bool AddPath (string Pathname) {
		DirectoryItem item;

		try {
			item = new DirectoryItem (Pathname);
			media_items.Add (Pathname.ToLower (), item);
			return true;
		} catch (Exception) {
			return false;
		}
	}


	public bool AddFiles (params string [] PathFilenames) {
		FileItem item;

		try {
			foreach (string pathfilename in PathFilenames) {
				item = new FileItem (pathfilename);
				media_items.Add (pathfilename.ToLower (), item);
			}
			return true;
		} catch (Exception) {
			return false;
		}
	}

	/// <summary>
	/// Entfernt den Eintrag aus der Liste.
	/// </summary>
	/// <param name="Name"></param>
	public bool RemovePathOrFiles (params string [] Name) {
		try {
			foreach (string name in Name)
				media_items.Remove (name.ToLower ());
			return true;
		} catch (Exception) {
			return false;
		}
	}


}   // class

//	namespace	2023-12-27 - 15.18.32