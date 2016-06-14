#region Using directives

using System;

#endregion

namespace MetaBuilder.BusinessLogic
{	
	///<summary>
	/// An object representation of the 'GraphFile' table. [No description found the database]	
	///</summary>
	/// <remarks>
	/// This file is generated once and will never be overwritten.
	/// </remarks>	
	[Serializable]
	[CLSCompliant(true)]
    public partial class GraphFile : GraphFileBase, IRepositoryItem, IWorkspaceItem

	{		
		#region Constructors

		///<summary>
		/// Creates a new <see cref="GraphFile"/> instance.
		///</summary>
		public GraphFile():base()
		{
            VCStatusID = (int)VCStatusList.None;
            State = VCStatusList.None;
            if (MachineName == null)
                MachineName = Environment.MachineName;

		}	
		
		#endregion


        public string MachineName
        {
            get { return Machine; }
            set { Machine = value; }
        }

        [NonSerialized]
        public VCStatusList state;
        public VCStatusList State
        {
            get { return (VCStatusList)VCStatusID; }
            set
            {
                VCStatusID = (int)value;
            }
        }


        /// <summary>
        /// Identifies the user that currently controls the object
        /// </summary>
        public string VCUser
        {
            get { return this.VCMachineID; }
            set { this.VCMachineID = value; }
        }

        public override string ToString()
        {
            return GetFileNameOnly(Name.ToString());// +" ver " + MajorVersion + "." + MinorVersion;
        }
        public static string GetFileNameOnly(string fullPath)
        {
            int lastSlash = fullPath.LastIndexOf('\\');
            return fullPath.Substring(lastSlash + 1, fullPath.Length - (lastSlash + 1));

        }

	}

}
