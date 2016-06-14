#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Modification and/or tampering with the code is also strictly prohibited, and
// punishable by law.
//
// Filename: DocumentVersion.cs
// Author: Deon Fourie
// Last Modified: 2007-14-26
//

#endregion

using System;
using System.Drawing;
using MetaBuilder.BusinessLogic;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.Graphing.Containers
{
    [Serializable]
    public class DocumentVersion
    {
        #region Fields (14) 

        private int _WorkspaceTypeId;
        private string _appVersion;
        private FileTypeList _fileType;
        private int _majorVersion;
        private int _minorVersion;
        private DateTime _modifiedDate;
        private string _notes;
        private int _pkid;
        private string _workspaceName;
        private string machineName;
        private Guid originalFileUniqueIdentifier;
        private int previousDocumentID;
        private RectangleF sheetBounds;
        private VCStatusList vcStatus;

        #endregion Fields 

        #region Properties (14) 

        public string AppVersion
        {
            get { return _appVersion; }
            set { _appVersion = value; }
        }

        public FileTypeList FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        public string MachineName
        {
            get
            {
                //TODO Save random number here
                if (machineName == null)
                    machineName = Environment.MachineName;
                return machineName;
            }
            set { machineName = value; }
        }

        public int MajorVersion
        {
            get { return _majorVersion; }
            set { _majorVersion = value; }
        }

        public int MinorVersion
        {
            get { return _minorVersion; }
            set { _minorVersion = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        public Guid OriginalFileUniqueIdentifier
        {
            get { return originalFileUniqueIdentifier; }
            set { originalFileUniqueIdentifier = value; }
        }

        public int PKID
        {
            get { return _pkid; }
            set { _pkid = value; }
        }

        public int PreviousDocumentID
        {
            get { return previousDocumentID; }
            set { previousDocumentID = value; }
        }

        public RectangleF SheetBounds
        {
            get { return sheetBounds; }
            set { sheetBounds = value; }
        }

        public VCStatusList VCStatus
        {
            get { return vcStatus; }
            set { vcStatus = value; }
        }

        public string WorkspaceName
        {
            get { return _workspaceName; }
            set { _workspaceName = value; }
        }

        public int WorkspaceTypeId
        {
            get { return _WorkspaceTypeId; }
            set { _WorkspaceTypeId = value; }
        }

        #endregion Properties 

        #region Methods (2) 

        // Public Methods (2) 

        public void LoadFromFile(GraphFile file)
        {
            FileType = (FileTypeList) file.FileTypeID;
            MachineName = file.Machine;
            MajorVersion = file.MajorVersion;
            MinorVersion = file.MinorVersion;
            ModifiedDate = file.ModifiedDate;
            PKID = file.pkid;
            if (file.PreviousVersionID.HasValue)
                PreviousDocumentID = file.PreviousVersionID.Value;
            OriginalFileUniqueIdentifier = file.OriginalFileUniqueID;
            VCStatus = (VCStatusList) file.VCStatusID;
            WorkspaceName = file.WorkspaceName;
            WorkspaceTypeId = file.WorkspaceTypeId;
        }

        /*
        private b.Technique technique;
        public b.Technique Technique
        {
            get { return technique; }
            set { technique = value;} 
        }*/

        public override string ToString()
        {
            return
                "Version " + MajorVersion.ToString() + "." + MinorVersion.ToString() + " [" +
                ModifiedDate.ToShortDateString() + "]";
        }

        #endregion Methods 
    }
}