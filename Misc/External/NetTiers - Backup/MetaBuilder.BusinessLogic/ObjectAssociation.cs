#region Using directives

using System;

#endregion

namespace MetaBuilder.BusinessLogic
{	
	///<summary>
	/// An object representation of the 'ObjectAssociation' table. [No description found the database]	
	///</summary>
	/// <remarks>
	/// This file is generated once and will never be overwritten.
	/// </remarks>	
	[Serializable]
	[CLSCompliant(true)]
	public partial class ObjectAssociation : ObjectAssociationBase,IRepositoryItem
	{		
		#region Constructors

		///<summary>
		/// Creates a new <see cref="ObjectAssociation"/> instance.
		///</summary>
		public ObjectAssociation():base(){
            machineName = Environment.MachineName;
            VCStatusID = (int)VCStatusList.None;
            State = VCStatusList.None;
        }	
		
		#endregion



        private string machineName;
        public string MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }

        [NonSerialized]
        public VCStatusList state;
        public VCStatusList State
        {
            get { return (VCStatusList)VCStatusID;} 
            set { this.VCStatusID = (int)value; }
        }

       
        /// <summary>
        /// Identifies the user that currently controls the object
        /// </summary>
        private string vcUser;
        public string VCUser
        {
            get { return vcUser; }
            set
            {
                vcUser = value;
                VCMachineID = value;
            }
        }

        public int pkid
        {
            get
            {
                return this.CAid; // throw new Exception("PKID is not implemented on Associations. Convert to Association first");
            }
        }

	}
}
