using System.Collections.Generic;
using System.Diagnostics;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using bl = MetaBuilder.BusinessLogic;
using dl = MetaBuilder.DataAccessLayer;
namespace MetaBuilder.BusinessFacade.MetaHelper
{
    public class AssociationManager
    {
        private TList<AssociationType> assocTypes;
        private TList<bl.Class> classes;
        private TList<ClassAssociation> classAssociations;
        private Dictionary<ClassAssociation, List<AllowedArtifact>> associationArtifacts;

        public AssociationManager(bool activeOnly)
        {
            //Trace.Listeners.Add(new ConsoleTraceListener());

            Init(activeOnly);
        }
        public void Init(bool activeOnly)
        {
            assocTypes = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetAll();
            classes = DataRepository.Classes(Core.Variables.Instance.ClientProvider);//.Provider.ClassProvider.GetAll();

            // classes.Filter = "IsActive = TRUE";

            classAssociations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetAll();

            TList<AllowedArtifact> allowedArtifacts = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AllowedArtifactProvider.GetAll();

            associationArtifacts = new Dictionary<ClassAssociation, List<AllowedArtifact>>();
            activeOnly = false;
            if (activeOnly)
            {
                classes.Filter = "IsActive = 'True'";
                classAssociations.Filter = "IsActive = 'True'";
                allowedArtifacts.Filter = "IsActive = 'True'";
            }
            List<AllowedArtifact> lstAssociationArtifacts;
            foreach (Class cls in classes)
            {
                foreach (ClassAssociation clsAssoc in classAssociations)
                {
                    if (clsAssoc.ParentClass == cls.Name)
                    {
                        lstAssociationArtifacts = new List<AllowedArtifact>();
                        associationArtifacts.Add(clsAssoc, lstAssociationArtifacts);

                        foreach (AllowedArtifact artefact in allowedArtifacts)
                        {
                            if (clsAssoc.CAid == artefact.CAid)
                            {
                                lstAssociationArtifacts.Add(artefact);
                            }
                        }
                    }
                }
            }
        }

        public void InspectCollections()
        {
            return;
            foreach (KeyValuePair<ClassAssociation, List<AllowedArtifact>> keyValuePair in associationArtifacts)
            {
                //// Console.WriteLine("Parent Class: {0} Association:{1} Child: {2}", new object[] { keyValuePair.Key.ParentClass, GetAssociation(keyValuePair.Key.AssociationTypeID).Name, keyValuePair.Key.ChildClass });
                foreach (AllowedArtifact allowed in keyValuePair.Value)
                {
                    //// Console.WriteLine("\t" + allowed.Class);
                }
            }
        }
        private AssociationType GetAssociation(int AssocID)
        {
            foreach (AssociationType type in assocTypes)
            {
                if (type.pkid == AssocID)
                {
                    return type;
                }
            }
            return null;
        }

        public List<ClassAssociation> GetAssociationsForParentAndChildClasses(string ParentClass, string ChildClass)
        {
            List<ClassAssociation> retval = new List<ClassAssociation>();
            foreach (KeyValuePair<ClassAssociation, List<AllowedArtifact>> kvp in this.associationArtifacts)
            {
                if (kvp.Key.ParentClass == ParentClass && kvp.Key.ChildClass == ChildClass)
                {
                    retval.Add(kvp.Key);
                }
            }
            return retval;
        }

        /// <summary>
        /// returns a list of allowed artifacts for a certain CAid
        /// </summary>
        /// <param name="CAid"></param>
        private List<AllowedArtifact> GetArtefacts(int CAid)
        {
            List<AllowedArtifact> retval = new List<AllowedArtifact>();
            foreach (KeyValuePair<ClassAssociation, List<AllowedArtifact>> kvp in associationArtifacts)
            {
                if (kvp.Key.CAid == CAid)
                {
                    foreach (AllowedArtifact allowed in kvp.Value)
                    {
                        retval.Add(allowed);
                    }
                }
            }
            return retval;
        }

        private static void PrintArtefact(AllowedArtifact allowed)
        {
            //// Console.WriteLine("\tArtefact:" + allowed.Class);
        }
        /// <summary>
        /// Returns a list of associations for a certain class
        /// </summary>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public List<ClassAssociation> GetAssociationForClass(string Parent)
        {
            List<ClassAssociation> retval = new List<ClassAssociation>();
            foreach (KeyValuePair<ClassAssociation, List<AllowedArtifact>> kvp in associationArtifacts)
            {
                if (kvp.Key.ParentClass == Parent)
                {
                    retval.Add(kvp.Key);
                }
            }
            return retval;
        }

        private void PrintAssociation(int AssociationTypeID, string Child)
        {
            //// Console.WriteLine("Child: {0} Association: {1}", new object[] { Child, GetAssociation(AssociationTypeID).Name });
        }
        private static AssociationManager globalInstance = null;
        // This is the method we will call when we want to
        // get a hold of the global vavriables in out application.
        public static AssociationManager Instance
        {
            get
            {
                if (globalInstance == null)
                {
                    globalInstance = new AssociationManager(true);
                }
                return globalInstance;
            }
            set
            {
                globalInstance = value;
            }
        }
    }
}
