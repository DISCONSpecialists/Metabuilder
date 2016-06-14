using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Meta;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.Tools.HeatMap
{
    class Capability
    {
        private MetaBase mBase;
        public MetaBase MBase
        {
            get { return mBase; }
            set { mBase = value; }
        }

        //if we need to bubble up add to constructor
        private Capability parent;
        public Capability Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private List<Capability> childCapabilities;
        public List<Capability> ChildCapabilities
        {
            get
            {
                if (childCapabilities == null)
                    childCapabilities = new List<Capability>();
                return childCapabilities;
            }
            set { childCapabilities = value; }
        }
        //private List<Capability> parentCapabilities;
        //public List<Capability> ParentCapabilities
        //{
        //    get
        //    {
        //        if (parentCapabilities == null)
        //            parentCapabilities = new List<Capability>();
        //        return parentCapabilities;
        //    }
        //    set { parentCapabilities = value; }
        //}

        private List<MetaBase> measures;
        public List<MetaBase> Measures
        {
            get { return measures; }
            set { measures = value; }
        }

        public Capability(MetaObject dbObject, List<MetaBase> measure)
        {
            MBase = Loader.GetByID(dbObject.Class, dbObject.pkid, dbObject.Machine);
            Measures = measure;
            bool clearChildren = false;
            foreach (ObjectAssociation childAssociation in DataRepository.Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(dbObject.pkid, dbObject.Machine))
            {
                if (childAssociation.Machine == "DB-TRIGGER")
                    continue;
                MetaObject o = DataRepository.Provider.MetaObjectProvider.GetBypkidMachine(childAssociation.ChildObjectID, childAssociation.ChildObjectMachine);
                if (o.Class == "MeasureType")
                {
                    clearChildren = true;
                    break;
                }
                //if (o.Class != "MeasureType" && o.Class != "Rationale" && o.Class != "Process")
                ChildCapabilities.Add(new Capability(o, Measures));
            }
            if (clearChildren)
                ChildCapabilities.Clear();
            Calculate();
        }
        public Capability(MetaBase mbObject, List<MetaBase> measure)
        {
            MBase = mbObject;
            Measures = measure;
            bool clearChildren = false;
            foreach (ObjectAssociation childAssociation in DataRepository.Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mbObject.pkid, mbObject.MachineName))
            {
                if (childAssociation.Machine == "DB-TRIGGER")
                    continue;
                MetaObject o = DataRepository.Provider.MetaObjectProvider.GetBypkidMachine(childAssociation.ChildObjectID, childAssociation.ChildObjectMachine);
                if (o.Class == "MeasureType")
                {
                    clearChildren = true;
                    break;
                }
                //if (o.Class != "MeasureType" && o.Class != "Rationale" && o.Class != "Process")
                ChildCapabilities.Add(new Capability(o, Measures));
            }
            if (clearChildren)
                ChildCapabilities.Clear();
            Calculate();
        }

        public double MeasureValue;
        public void Calculate()
        {
            MeasureValue = 0;
            //find each measuretypes value
            foreach (MetaBase measure in Measures)
            {
                double m = 0;
                TList<Artifact> artifacts = DataRepository.ArtifactProvider.GetByObjectIDObjectMachine(MBase.pkid, MBase.MachineName);
                int measureCount = 0;
                foreach (Artifact artifact in artifacts)
                {
                    if (artifact.ChildObjectID == measure.pkid && artifact.ChildObjectMachine == measure.MachineName)
                    {
                        MetaObject artObj = DataRepository.MetaObjectProvider.GetBypkidMachine(artifact.ArtifactObjectID, artifact.ArtefactMachine);
                        if (artObj.Class == "MeasureValue")
                        {
                            double mIn = 0;
                            double.TryParse(Loader.GetByID(artObj.Class, artObj.pkid, artObj.Machine).ToString(), out mIn);
                            m += mIn;
                            measureCount += 1;
                        }
                    }
                }

                if (measureCount > 0)
                    MeasureValue += m / measureCount;
            }
        }

        public double totalNumber = 0;
        public int totalCount = 0;
        public double Total
        {
            get
            {
                totalCount = 0;
                totalNumber = 0;

                if (ChildCapabilities.Count == 0) //MeasureValue>0
                {
                    return MeasureValue / Measures.Count;
                }

                foreach (Capability cap in ChildCapabilities)
                {
                    totalCount += 1;
                    totalNumber += cap.Total;
                }

                return totalNumber / totalCount;
            }
        }

    }
}
