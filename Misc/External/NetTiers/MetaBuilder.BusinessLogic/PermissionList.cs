using System;

namespace MetaBuilder.BusinessLogic
{
    /// <summary>
    /// 
    /// </summary>
    /// <remark>makes PermissionIds easily accessible</remark>
    [Serializable]
    public enum PermissionList
    {
        /// <summary> 
        /// Read
        /// </summary>
        Read = 1,

        /// <summary> 
        /// Write
        /// </summary>
        Write = 2,

        /// <summary> 
        /// Admin
        /// </summary>
        Admin = 3,

        /// <summary> 
        /// Delete
        /// </summary>
        Delete = 4,

        /// <summary> 
        /// None
        /// </summary>
        None = 0

    }
}