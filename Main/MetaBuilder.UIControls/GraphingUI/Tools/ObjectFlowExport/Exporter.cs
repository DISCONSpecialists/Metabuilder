using System;
//using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MetaBuilder.BusinessFacade.Exports;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.Tools.ObjectFlowExport
{
    public class Exporter
    {
        private string fileName = "";

        public Exporter(string filename)
        {
            fileName = filename;
            Export();
        }

        List<ObjectFlow> rootObjects;
        List<ObjectFlowAssociation> Flows;
        public void Export()
        {
            int maxParentFlows = 0;
            int maxParentFiles = 0;
            int maxLinkFlows = 0;
            //int maxChildflows = 0;
            rootObjects = new List<ObjectFlow>();
            Flows = new List<ObjectFlowAssociation>();

            #region Build Flows

            foreach (MetaObject mb in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetByClass("Object"))
            {
                ObjectFlow thisOFlow = null;
                if (mb != null)
                {
                    string name = "";
                    foreach (ObjectFieldValue v in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectFieldValueProvider.GetByObjectIDMachineID(mb.pkid, mb.Machine))
                    {
                        if (v.ValueString.Length > 0)
                        {
                            name = v.ValueString;
                            break;
                        }
                    }

                    thisOFlow = new ObjectFlow(name, mb.pkid, mb.Machine);
                }

                if (thisOFlow != null)
                {
                    rootObjects.Add(thisOFlow);
                    maxParentFlows = thisOFlow.Flows.Count > maxParentFlows ? thisOFlow.Flows.Count : maxParentFlows;
                    maxParentFiles = thisOFlow.FileNames.Count > maxParentFiles ? thisOFlow.FileNames.Count : maxParentFiles;
                }
            }

            //combine flows into lines
            foreach (ObjectFlow of in rootObjects)
            {
                foreach (ObjectAssociation ass in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(of.PKID, of.Machine))
                {
                    if (ass.ChildObjectID == of.PKID)
                        continue; //Skip associations to itself

                    foreach (ObjectFlow ofC in rootObjects)
                    {
                        if (ofC.PKID == ass.ChildObjectID)
                        {
                            ObjectFlowAssociation ofAss = new ObjectFlowAssociation(of, ofC, ass.CAid.ToString());
                            maxLinkFlows = maxLinkFlows < ofAss.Flows.Count ? ofAss.Flows.Count : maxLinkFlows;
                            Flows.Add(ofAss);
                            continue;
                        }
                    }
                }
            }

            #endregion

            #region Write Flows

            string lines = "";
            foreach (ObjectFlowAssociation f in Flows)
            {
                string line = "";
                int parentFlowCount = 0;
                int parentFileCount = 0;
                int linkFlowcount = 0;
                //Filenames
                foreach (string fName in f.ParentFlow.FileNames)
                {
                    line += fName + ",";
                    parentFileCount += 1;
                }
                for (int x = parentFileCount; x < maxParentFiles; x++)
                {
                    line += "NULL,";
                }

                //Parent
                line += f.ParentFlow.Name + ",";
                foreach (Flow pF in f.ParentFlow.Flows)
                {
                    line += pF.Name + ",";
                    parentFlowCount += 1;
                }
                for (int x = parentFlowCount; x < maxParentFlows; x++)
                {
                    line += "NULL,";
                }

                //Association Flow
                if (f.Flows.Count == 0)
                {
                    line += "Dynamic Flow,";
                    linkFlowcount += 1;
                }
                else
                {
                    foreach (Flow linkFlow in f.Flows)
                    {
                        line += linkFlow.Name + ",";
                        linkFlowcount += 1;
                    }
                }
                for (int x = linkFlowcount; x < maxLinkFlows; x++)
                {
                    line += "NULL,";
                }

                //Child
                line += f.ChildFlow.Name + ",";
                foreach (Flow cF in f.ChildFlow.Flows)
                {
                    line += cF.Name + ",";
                }

                lines += line;
                //NewLine
                lines += Environment.NewLine;
            }

            string header = "Filename,";
            if (maxParentFiles > 1)
                for (int x = 0; x < maxParentFiles - 1; x++)
                    header += ",";

            header += "ParentObject,ParentFlows...,";
            if (maxParentFlows > 1)
                for (int x = 0; x < maxParentFlows - 1; x++)
                    header += ",";

            header += "Parent To Child Flow (Association),";
            if (maxLinkFlows > 1)
                for (int x = 0; x < maxLinkFlows - 1; x++)
                    header += ",";

            header += "ChildObject,ChildFlows...,";

            lines = header + Environment.NewLine + lines;

            #endregion

            if (System.IO.File.Exists(fileName))
                System.IO.File.Delete(fileName);

            System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);
            file.WriteLine(lines);
            file.Close();
        }
    }
}