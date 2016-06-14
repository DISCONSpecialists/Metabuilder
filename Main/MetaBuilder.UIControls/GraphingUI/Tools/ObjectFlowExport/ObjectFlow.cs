using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.Tools.ObjectFlowExport
{
    public class ObjectFlow
    {
        public ObjectFlow(string n, int p, string m)
        {
            Name = n;
            PKID = p;
            Machine = m;
            FileNames = new List<string>();
            Flows = new List<Flow>();

            foreach (Artifact a in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.GetByObjectIDObjectMachine(PKID, Machine))
            {
                if (a.ChildObjectID == PKID && a.ChildObjectMachine == Machine)
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
                        Flows.Add(new Flow(sequence + " " + N, mb.pkid, mb.Machine));
                    }
                }
            }

            foreach (GraphFile f in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(PKID, Machine))
            {
                //FileNames.Add(f.Name);
                if (f.IsActive && !FileNames.Contains(f.Name))
                    FileNames.Add(Core.strings.GetFileNameOnly(f.Name));
            }
        }
        private int pkid;
        public int PKID { get { return pkid; } set { pkid = value; } }
        private string machine;
        public string Machine { get { return machine; } set { machine = value; } }
        private string name;
        public string Name { get { return name; } set { name = value; } }
        private List<string> filenames;
        public List<string> FileNames { get { return filenames; } set { filenames = value; } }

        private List<Flow> flows;
        public List<Flow> Flows { get { return flows; } set { flows = value; } }
    }
}