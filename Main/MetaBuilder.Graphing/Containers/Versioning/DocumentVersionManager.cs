#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Modification and/or tampering with the code is also strictly prohibited, and
// punishable by law.
//
// Filename: DocumentVersionManager.cs
// Author: Deon Fourie
// Last Modified: 2007-14-26
//

#endregion

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.Graphing.Containers
{
    [Serializable]
    public class DocumentVersionManager
    {
        #region Fields (1)

        private FileTypeList fileType;

        #endregion Fields

        #region Constructors (1)

        public DocumentVersionManager(FileTypeList fileType)
        {
            FileType = fileType;
            InitVersions();
        }

        #endregion Constructors

        #region Properties (2)

        public DocumentVersion CurrentVersion
        {
            get { return Versions[Versions.Count - 1]; }
        }

        public FileTypeList FileType
        {
            get { return fileType; }
            set { fileType = value; }
        }

        #endregion Properties

        #region Methods (4)

        // Public Methods (2) 

        public void IncreaseMajorVersion()
        {
            //   CheckInPreviousVersion();
            Versions.Add(CreateVersion(CurrentVersion.MajorVersion + 1, 0));
        }

        public void IncreaseMinorVersion()
        {
            //  CheckInPreviousVersion();
            Versions.Add(CreateVersion(CurrentVersion.MajorVersion, CurrentVersion.MinorVersion + 1));
        }

        // Private Methods (2) 

        private DocumentVersion CreateVersion(int major, int minor)
        {
            DocumentVersion version = new DocumentVersion();
            version.AppVersion = Application.ProductVersion;
            version.MajorVersion = major;
            version.MinorVersion = minor;
            version.ModifiedDate = DateTime.Now;
            version.FileType = FileType;
            if (versions.Count == 0)
            {
                version.OriginalFileUniqueIdentifier = Guid.NewGuid();
                version.WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;
                version.WorkspaceName = Variables.Instance.CurrentWorkspaceName;
                version.VCStatus = VCStatusList.None;
            }
            else
            {
                version.OriginalFileUniqueIdentifier = CurrentVersion.OriginalFileUniqueIdentifier;
                version.WorkspaceName = CurrentVersion.WorkspaceName;
                version.WorkspaceTypeId = CurrentVersion.WorkspaceTypeId;
                version.VCStatus = CurrentVersion.VCStatus;
            }
            return version;
        }

        private void InitVersions()
        {
            if (versions == null)
            {
                versions = new List<DocumentVersion>();
                DocumentVersion version = CreateVersion(0, 0);
                version.OriginalFileUniqueIdentifier = Guid.NewGuid();
                versions.Add(version);
            }
        }

        #endregion Methods

        #region Document Properties

        private List<DocumentVersion> versions;

        public List<DocumentVersion> Versions
        {
            get { return versions; }
            set { versions = value; }
        }

        #endregion
    }
}