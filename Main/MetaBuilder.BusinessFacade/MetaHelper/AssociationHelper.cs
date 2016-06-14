#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: AssociationHelper.cs
//

#endregion

using System.Collections.Generic;
using System.Data;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer.OldCode.Meta;
using MetaBuilder.Meta;
using System.Collections.ObjectModel;

namespace MetaBuilder.BusinessFacade.MetaHelper
{
    /// <summary>
    /// Summary description for AssociationHelper.
    /// </summary>
    public class AssociationHelper
    {

        public int GetAssociationTypeForCAID(int CAID)
        {
            if (allowedAssociations == null)
            {
                allowedAssociations = new AssociationInfoCollection();
                allowedAssociations.Init();
            }
            foreach (AllowedAssociationInfo info in allowedAssociations.Allowed)
            {
                if (info.ClassAssociationID == CAID)
                {
                    return (int)info.LinkAssociationType;
                }
            }
            Core.Log.WriteLog("AssociationHelper::GetAssociationTypeForCAID(" + CAID.ToString() + ") returning 0");
            return 0;
        }

        public int GetAssociationTypeForParentChildClass(string parent, string child, LinkAssociationType associationType)
        {
            if (allowedAssociations == null)
            {
                allowedAssociations = new AssociationInfoCollection();
                allowedAssociations.Init();
            }
            foreach (AllowedAssociationInfo info in allowedAssociations.Allowed)
            {
                if (info.ParentClass == parent && info.ChildClass == child && info.LinkAssociationType == associationType)
                {
                    return (int)info.ClassAssociationID;
                }
            }
            return 0;
        }
        private AssociationInfoCollection allowedAssociations;
        public AssociationInfoCollection AllowedAssociations
        {
            get
            {
                if (allowedAssociations == null)
                {
                    allowedAssociations = new AssociationInfoCollection();
                    allowedAssociations.Init();
                }
                return allowedAssociations;
            }
        }
        public AssociationHelper()
        {
        }

        public void DeleteGraphFileAssociationByGraphFileIDGraphFileMachine(int graphfileid, string machine)
        {
            AssociationAdapter assAdapter = new AssociationAdapter(false);
            assAdapter.DeleteGraphFileAssociationByGraphFileIDGraphFileMachine(graphfileid, machine);
        }
        public AllowedAssociationInfo GetDefaultAllowedAssociationInfo(string parentclass, string childclass)
        {
            AllowedAssociationInfo retval = null;
            if (allowedAssociations == null)
            {
                allowedAssociations = new AssociationInfoCollection();
                allowedAssociations.Init();
            }

            foreach (AllowedAssociationInfo allowed in AllowedAssociations.Allowed)
            {
                if ((allowed.ParentClass == parentclass) && (allowed.ChildClass == childclass))
                {
                    retval = allowed; // set this up to be the default - this will be returned if no match is found
                    if (allowed.IsDefault)
                    {
                        return allowed;
                    }
                }
            }
            return retval;
        }
        public Collection<AllowedAssociationInfo> GetAllowedAssociationInfoCollection(string parentclass, string childclass)
        {
            Collection<AllowedAssociationInfo> retval = new Collection<AllowedAssociationInfo>();
            if (allowedAssociations == null)
            {
                allowedAssociations = new AssociationInfoCollection();
                allowedAssociations.Init();
            }

            foreach (AllowedAssociationInfo allowed in this.allowedAssociations.Allowed)
            {
                if (allowed.ParentClass == parentclass && allowed.ChildClass == childclass)
                {
                    retval.Add(allowed);
                }
            }
            return retval;
        }
        public int CheckForValidArtifact(string parentclass, string childclass, string artifactclass, int AssociationTypeID)
        {
            int retval = -1;
            if (allowedAssociations == null)
            {
                allowedAssociations = new AssociationInfoCollection();
                allowedAssociations.Init();
            }

            LinkAssociationType linkType = (LinkAssociationType)AssociationTypeID;
            foreach (AllowedAssociationInfo allowed in this.allowedAssociations.Allowed)
            {
                if ((allowed.ParentClass == parentclass) && (allowed.ChildClass == childclass) && (allowed.LinkAssociationType == linkType))
                {

                    foreach (AllowedAssociationInfo.ArtifactAllowance artClass in allowed.AllowedArtifactClasses)
                    {
                        if (artClass.ClassName == artifactclass)
                        {
                            return allowed.ClassAssociationID;
                        }
                    }
                }
            }
            return retval;
        }
        public List<BusinessLogic.ObjectAssociation> GetOrphanedAssociations()
        {
            AssociationAdapter adapter = new AssociationAdapter(false);
            return adapter.GetOrphanedAssociations();
        }

        public List<BusinessLogic.ObjectAssociation> GetOrphanedAssociations(GraphFileKey excludingFileKey)
        {
            AssociationAdapter adapter = new AssociationAdapter(false);
            return adapter.GetOrphanedAssociations(excludingFileKey);
        }
        public Association GetAssociation(string ParentClass, string ChildClass, int LimitToAssociationType)
        {
            AssociationAdapter adapter = new AssociationAdapter(false);
            return adapter.GetAssociation(ParentClass, ChildClass, LimitToAssociationType);
        }

        public DataSet GetAssociationDataSet()
        {
            AssociationAdapter adapter = new AssociationAdapter(false);
            return adapter.GetAssociationDataSet();
        }
        public DataSet GetAssociationInfo()
        {
            AssociationAdapter adapter = new AssociationAdapter(false);
            return adapter.GetAssociationInformation();
        }

        public DataTable GetAssociatedObjects(int ObjectID, string ObjectMachine)
        {
            AssociationAdapter adapter = new AssociationAdapter(false);
            return adapter.GetAssociatedObjects(ObjectID, ObjectMachine);
        }

        public DataSet GetArtifactData(int CAid, int ObjectID, int ChildObjectID, string objectmachine, string childmachine)
        {
            AssociationAdapter ada = new AssociationAdapter(false);
            return ada.GetArtifactData(CAid, ObjectID, ChildObjectID, objectmachine, childmachine);
        }

        public void AddArtifact(int CAid, int ObjectID, int ChildObjectID, int ArtifactID, string objectmachine, string childmachine, string artefactmachine)
        {
            AssociationAdapter ada = new AssociationAdapter(false);
            ada.AddArtifact(CAid, ObjectID, ChildObjectID, ArtifactID, objectmachine, childmachine, artefactmachine);
        }

        public void AddAssociationObject(int CAid, int ObjectID, int ChildObjectID, int AssociationObjectID, string objectmachine, string childmachine, string artefactmachine)
        {
            AssociationAdapter ada = new AssociationAdapter(false);
            ada.AddAssociationObject(CAid, ObjectID, ChildObjectID, AssociationObjectID, objectmachine, childmachine, artefactmachine);
        }

        public void RemoveArtifact(int CAid, int ObjectID, int ChildObjectID, int ArtifactID, string objectmachine, string childmachine, string artefactmachine)
        {
            AssociationAdapter ada = new AssociationAdapter(false);
            ada.RemoveArtifact(CAid, ObjectID, ChildObjectID, ArtifactID, objectmachine, childmachine, artefactmachine);
        }

        public void ClearArtifacts(int CAid, int ObjectID, int ChildObjectID, string objectmachine, string childmachine)
        {
            AssociationAdapter ada = new AssociationAdapter(false);
            ada.ClearArtifacts(CAid, ObjectID, ChildObjectID, objectmachine, childmachine);
        }

        public int AddQuickAssociation(int ObjectID1, int ObjectID2, string objectmachine, string objectmachine2, int AssociationTypeID)
        {
            AssociationAdapter ada = new AssociationAdapter(false);
            return ada.AddQuickAssociation(ObjectID1, ObjectID2, AssociationTypeID, objectmachine, objectmachine2);
        }

    }
}