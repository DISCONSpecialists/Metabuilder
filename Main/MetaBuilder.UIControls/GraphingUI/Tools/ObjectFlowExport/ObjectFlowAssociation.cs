using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.Tools.ObjectFlowExport
{
    public class ObjectFlowAssociation
    {
        public ObjectFlow ParentFlow;
        public ObjectFlow ChildFlow;
        public string Association;
        //public Flow LinkFlow;
        public List<Flow> Flows;

        public ObjectFlowAssociation(ObjectFlow p, ObjectFlow c, string caid)
        {
            ParentFlow = p;
            ChildFlow = c;
            Association = caid;
            Flows = new List<Flow>();
            foreach (Artifact a in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.GetByObjectIDObjectMachine(ParentFlow.PKID, ParentFlow.Machine))
            {
                if (a.ChildObjectMachine == ChildFlow.Machine && a.ChildObjectID == ChildFlow.PKID)
                {
                    MetaObject mb = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(a.ArtifactObjectID, a.ArtefactMachine);
                    if (mb != null)
                    {
                        string N = "";
                        string sequence = "";
                        foreach (ObjectFieldValue v in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectFieldValueProvider.GetByObjectIDMachineID(mb.pkid, mb.Machine))
                        {
                            Field f = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.FieldProvider.GetBypkid(v.FieldID);
                            if (f.Name == "Sequence")
                            {
                                sequence = v.ValueString;
                            }
                            else if (f.Name == "Description")
                            {
                                N = v.ValueString;
                            }
                        }
                        //LinkFlow = new Flow(sequence + " " + N, mb.pkid, mb.Machine);
                        //break;
                        Flows.Add(new Flow(sequence + " " + N, mb.pkid, mb.Machine));
                    }
                }
            }
        }
    }
}