using System.Collections.Generic;
using System.Data;
using MetaBuilder.Meta;
using System.Collections.ObjectModel;

namespace MetaBuilder.BusinessFacade.MetaHelper
{
    public class AssociationInfoCollection
    {
        private List<AllowedAssociationInfo> allowed;
        public List<AllowedAssociationInfo> Allowed
        {
            get { return allowed; }
            set { allowed = value; }
        }

        public void Init()
        {
            Allowed = new List<AllowedAssociationInfo>();
            AssociationHelper associationHelper = new AssociationHelper();
            DataSet ds = associationHelper.GetAssociationInfo();
            DataView dvAssociatedClasses = ds.Tables["Classes"].DefaultView;

            //Collection<AllowedAssociationInfo.ArtifactAllowance> allowedArtifactClasses;
            foreach (DataRowView drvAssociation in dvAssociatedClasses)
            {
                AllowedAssociationInfo association = new AllowedAssociationInfo(
                        drvAssociation["ParentClass"].ToString(),
                        drvAssociation["ChildClass"].ToString(),
                        int.Parse(drvAssociation["CAid"].ToString()),
                        int.Parse(drvAssociation["AssociationTypeID"].ToString()),
                        bool.Parse(drvAssociation["IsDefault"].ToString()),
                        drvAssociation["Caption"].ToString()
                        );
                DataView dvAllowedArtifactClasses = drvAssociation.CreateChildView(ds.Relations[0]);
                //allowedArtifactClasses = new Collection<AllowedAssociationInfo.ArtifactAllowance>();
                association.AllowedArtifactClasses = new Collection<AllowedAssociationInfo.ArtifactAllowance>();
                foreach (DataRowView drvArtifacClass in dvAllowedArtifactClasses)
                {
                    association.AllowedArtifactClasses.Add(new AllowedAssociationInfo.ArtifactAllowance(drvArtifacClass["Class"].ToString()));
                }
                //association.AllowedArtifactClasses = allowedArtifactClasses;
                Allowed.Add(association);
            }

        }
    }

    public class AllowedAssociationInfo
    {
        public AllowedAssociationInfo(string ParentClass, string ChildClass, int CAid, int AssociationType, bool isdefault, string caption)
        {
            parentClass = ParentClass;
            childClass = ChildClass;
            classAssociationID = CAid;
            LinkAssociationType = (LinkAssociationType)AssociationType;
            IsDefault = isdefault;
            Caption = caption;
        }
        private string parentClass;
        public string ParentClass
        {
            get { return parentClass; }
            set { parentClass = value; }
        }

        private string childClass;
        public string ChildClass
        {
            get { return childClass; }
            set { childClass = value; }
        }

        private int classAssociationID;
        public int ClassAssociationID
        {
            get { return classAssociationID; }
            set { classAssociationID = value; }
        }

        private LinkAssociationType linkAssociationType;
        public LinkAssociationType LinkAssociationType
        {
            get { return linkAssociationType; }
            set { linkAssociationType = value; }
        }


        private Collection<ArtifactAllowance> allowedArtifactClasses;
        public Collection<ArtifactAllowance> AllowedArtifactClasses
        {
            get { return allowedArtifactClasses; }
            set { allowedArtifactClasses = value; }
        }

      
        private bool isDefault;
        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }
        private string caption;
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public class ArtifactAllowance
        {
            public ArtifactAllowance(string ArtifactClassName, bool IsAllowed)
            {
                this.ClassName = ArtifactClassName;
                this.IsAllowed = IsAllowed;
            }
            public ArtifactAllowance(string ArtifactClassName)
            {
                this.ClassName = ArtifactClassName;
                this.IsAllowed = false;
            }
            private string className;
            public string ClassName
            {
                get{return className;}
                set { className = value; }
            }

            private bool isAllowed;
            public bool IsAllowed
            {
                get { return isAllowed; }
                set { isAllowed = value; }
            }
        }
    }
    public class ClassInfo
    {
        private int classID;
        private string name;
        public int ClassID
        {
            get { return classID; }
            set { classID = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }


  
}
