
using System;

namespace MetaBuilder.BusinessLogic
{
    /// <summary>
    /// An enum representation of the 'VCStatus' table. [No description found in the database]
    /// </summary>
    /// <remark>this enumeration contains the items contained in the table VCStatus</remark>
    [Serializable]
    public enum VCStatusList
    {
        /// <summary> 
        /// CheckedIn
        /// </summary>
        CheckedIn = 1,

        /// <summary> 
        /// CheckedOut
        /// </summary>
        CheckedOut = 2,

        /// <summary> 
        /// CheckedOutRead
        /// </summary>
        CheckedOutRead = 5,

        /// <summary> 
        /// Locked
        /// </summary>
        Locked = 4,

        /// <summary> 
        /// MarkedForDelete
        /// </summary>
        MarkedForDelete = 8,

        /// <summary> 
        /// None
        /// </summary>
        None = 7,

        /// <summary> 
        /// Obsolete
        /// </summary>
        Obsolete = 3,

        /// <summary> 
        /// PartiallyCheckedIn
        /// </summary>
        PartiallyCheckedIn = 6,
        PCI_Revoked = 9,
        Skipped = 19,
        Deleted = 20,
        Ignore = 25
    }
}
