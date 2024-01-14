// BurnData.cs
//
// by Eric Haddan
//

namespace IMAPI2.Interop;

public enum BURN_STEPS {
	CREATING_FILE_SYSTEM,
	WRITING_TO_DISK
}

public class BurninStatus {

	public string UniqueRecorderId;
	public string StatusMessage;
	public BURN_STEPS Task;

	// IDiscFormat2DataEventArgs Interface
	public long ElapsedTime;        // Elapsed time in seconds
	public long RemainingTime;      // Remaining time in seconds
	public long TotalTime;          // total estimated time in seconds
									// IWriteEngine2EventArgs Interface
	public IMAPI_FORMAT2_DATA_WRITE_ACTION currentAction;
	public long StartLba;           // the starting lba of the current operation
	public long SectorCount;        // the total sectors to write in the current operation
	public long LastReadLba;        // the last read lba address
	public long LastWrittenLba;		// the last written lba address
	public long TotalSystemBuffer;  // total size of the system buffer
	public long UsedSystemBuffer;   // size of used system buffer
	public long FreeSystemBuffer;   // size of the free system buffer

}   // class

//	namespace	2023-12-27 - 14.48.45

