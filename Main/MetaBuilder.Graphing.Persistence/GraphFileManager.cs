using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.Collections.Generic;
//TRACING::using TraceTool;

namespace MetaBuilder.Graphing.Persistence
{
    public class GraphFileManager
    {

        #region Fields (4)

        private string mergedBinary;
        private string deletedBinary;
        private string tempFile;
        private TList<b.Workspace> workspaces;
        private string xmlFile;

        #endregion Fields

        #region Methods (14)

        // Public Methods (9) 

        public GraphFile ConvertToCompressedGraphFile(NormalDiagram doc, string filename)
        {
            tempFile = filename + ".dbxml";
            FileUtil futil = new FileUtil();
            futil.Save(doc, tempFile);
            GraphFile GraphFile = new GraphFile();
            RetrieveBlob(tempFile, GraphFile, true);

            if (doc.VersionManager.CurrentVersion.PKID > 0)
            {
                GraphFile.PreviousVersionID = doc.VersionManager.CurrentVersion.PKID;
                GraphFile.OriginalFileUniqueID = doc.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier;
            }
            GraphFile.Name = doc.Name;
            GraphFile.FileTypeID = (int)doc.FileType;
            GraphFile.ModifiedDate = doc.VersionManager.CurrentVersion.ModifiedDate;
            GraphFile.OriginalFileUniqueID = doc.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier;
            GraphFile.AppVersion = doc.VersionManager.CurrentVersion.AppVersion;
            GraphFile.VCStatusID = (int)doc.VersionManager.CurrentVersion.VCStatus;
            GraphFile.MajorVersion = doc.VersionManager.CurrentVersion.MajorVersion;
            GraphFile.MinorVersion = doc.VersionManager.CurrentVersion.MinorVersion;
            if (doc.VersionManager.CurrentVersion.PKID > 0)
            {
                GraphFile.PreviousVersionID = doc.VersionManager.CurrentVersion.PKID;
                GraphFile.OriginalFileUniqueID = doc.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier;
            }
            if (doc.VersionManager.CurrentVersion.WorkspaceName != null)
            {
                GraphFile.WorkspaceName = doc.VersionManager.CurrentVersion.WorkspaceName;
                GraphFile.WorkspaceTypeId = doc.VersionManager.CurrentVersion.WorkspaceTypeId;
            }
            else
            {
                GraphFile.WorkspaceName = Variables.Instance.CurrentWorkspaceName;
                GraphFile.WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;
            }
            GraphFile.Machine = doc.VersionManager.CurrentVersion.MachineName;
            GraphFile.MachineName = doc.VersionManager.CurrentVersion.MachineName;
            GraphFile.Archived = false;
            GraphFile.IsActive = true;

            return GraphFile;
        }

        public NormalDiagram DeserializeBinaryFile(Stream stream, int pkid, string machineName)
        {
            NormalDiagram newDoc = null;
            try
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                binFormatter.Binder = new NormalDiagramSerializationBinder();
                binFormatter.Binder.BindToType("MetaBuilder.Graphing", "Containers.NormalDiagram");
                newDoc = (NormalDiagram)binFormatter.Deserialize(stream);
                newDoc.VersionManager.CurrentVersion.PKID = pkid;
                newDoc.VersionManager.CurrentVersion.MachineName = machineName;
                FixPKIDReferences(newDoc);
            }
            catch
            {
            }
            return newDoc;
        }

        private void FixPKIDReferences(NormalDiagram diagram)
        {
            b.TList<b.MetaObject> moall = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject(
                diagram.VersionManager.CurrentVersion.PKID, diagram.VersionManager.CurrentVersion.MachineName);
            List<GraphNode> nodes = GetNodes(diagram);

            foreach (b.MetaObject mo in moall)
            {
                MetaBase mb = Loader.GetByID(mo.Class, mo.pkid, mo.Machine);
                if (mb != null)
                {
                    foreach (GraphNode node in nodes)
                    {
                        if (node.HasBindingInfo)
                        {
                            if (node.MetaObject != null)
                            {
                                if (node.MetaObject.ToString() == mb.ToString())
                                {
                                    node.MetaObject.pkid = mb.pkid;
                                }
                            }
                        }
                    }
                }
            }
        }

        public List<GraphNode> GetNodes(NormalDiagram Document)
        {
            List<GraphNode> retval = new List<GraphNode>();
            foreach (GoObject o in Document)
            {
                if (o is GraphNode)
                {
                    retval.Add(o as GraphNode);
                }
                else if (o is GoGroup)
                {
                    GoGroup grp = o as GoGroup;
                    GoGroupEnumerator grpenumerator = grp.GetEnumerator();
                    while (grpenumerator.MoveNext())
                    {
                        if (grpenumerator.Current.ParentNode is GraphNode)
                        {
                            retval.Add(grpenumerator.Current.ParentNode as GraphNode);
                        }
                    }
                }
            }
            return retval;
        }
        public bool Exists(int WorkspaceTypeId, string workspacename)
        {
            foreach (b.Workspace ws in workspaces)
            {
                if ((ws.Name == workspacename) && (ws.WorkspaceTypeId == WorkspaceTypeId))
                    return true;
            }
            return false;
        }

        public NormalDiagram LoadCompressedXMLFile(Stream stream, string Filename)
        {
            //TRACING::TTrace.Debug.Send("LoadCompressedXMLFile", Filename);
            NormalDiagram newDoc = null;
            try
            {
                xmlFile = Core.Storage.FilePathManager.Instance.GetSpecification(FileTypeList.Diagram).DefaultPath + Core.strings.GetFileNameOnly(Filename) + ".xml";
                //TRACING::TTrace.Debug.Indent("Attempting to decompress file");
                bool success = ZipUtil.UnzipStream(stream, xmlFile);
                //TRACING::TTrace.Debug.UnIndent();
                //TRACING::TTrace.Debug.SendValue("Success?",success);
                if (success)
                {
                    XmlPersistor xmlPersistor = new XmlPersistor();
                    newDoc = xmlPersistor.DepersistCollection(xmlFile, FileType.Diagram) as NormalDiagram;

                }
                newDoc.Name = Filename;
                // delete temp file
                System.Threading.ThreadStart ts = new System.Threading.ThreadStart(DeleteXmlFile);
                System.Threading.Thread t = new System.Threading.Thread(ts);
                t.Start();
            }
            catch (Exception x)
            {
                LogEntry lentry = new LogEntry();
                lentry.Title = "Error when unzipping";
                lentry.Message = x.ToString();
                Logger.Write(lentry);
            }
            return newDoc;
        }

        public void MarkPreviousVersionsInactive(GraphFile file)
        {
            //foreach (GraphFile f in DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.Find("OriginalFileUniqueID=" + file.OriginalFileUniqueID.ToString()))
            //{
            //    if (f.pkid == file.pkid)
            //        continue;
            //    f.IsActive = false;
            //    DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.Save(f);
            //}

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("UPDATE GRAPHFILE SET ISACTIVE=0  WHERE ISACTIVE=1 AND   (OriginalFileUniqueID = '" + file.OriginalFileUniqueID.ToString() + "') AND PKID <>" + file.pkid.ToString());
            SqlCommand cmd = new SqlCommand(sbQuery.ToString(), new SqlConnection(Variables.Instance.ConnectionString)); //Provider == Core.Variables.Instance.ServerProvider ? Variables.Instance.ServerConnectionString : 

            cmd.CommandType = CommandType.Text;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public bool Server = false;
        public void ReplaceMetaObjectsInDatabaseFile(GraphFile file, List<KeyValuePair<MetaBase, MetaBase>> replacements, string Provider)
        {
            tempFile = Core.Variables.Instance.DiagramPath + "\\merged.xml";

            MemoryStream memStream = new MemoryStream();
            int len = file.Blob.Length;
            int count = 0;
            memStream = new MemoryStream(file.Blob.Length);
            while (count < len)
            {
                memStream.WriteByte(file.Blob[count]);
                count++;
            }
            memStream.Position = 0;

            ZipUtil.UnzipStream(memStream, tempFile);
            memStream.Close();

            StringBuilder sb = new StringBuilder();
            StreamReader sreader = new StreamReader(tempFile);
            while (sreader.Peek() > -1)
            {
                string line = sreader.ReadLine();
                //Pkid="170207" MachineName="KNERSIS"
                if (line.Contains("Pkid")) //Why not contains metaobject?
                {
                    /*
                     * THIS IS RATHER IMPORTANT! BELOW KVP is KVP<ToBeReplaced,WhatItWillBeReplacedBy>
                     * 
                     foreach (KeyValuePair<BeingReplaced, NewItem> replacement in replacements) <-- like so :)
                     */
                    foreach (KeyValuePair<MetaBase, MetaBase> replacement in replacements)
                    {
                        string oldPKID = "Pkid=\"" + replacement.Key.pkid.ToString() + "\"";
                        //22 October 2013 - Objects are identified by pkid & Machine!
                        string oldMachine = "MachineName=\"" + replacement.Key.MachineName + "\"";

                        string newPKID = "Pkid=\"" + replacement.Value.pkid.ToString() + "\"";
                        string newMachine = "MachineName=\"" + replacement.Value.MachineName + "\"";

                        if (line.Contains(oldPKID) && line.Contains(oldMachine))
                        {
                            string oldWSName = "Workspace=\"" + replacement.Key.WorkspaceName.ToString() + "\"";
                            string oldWSType = "WorkspaceTypeId=\"" + replacement.Key.WorkspaceTypeId.ToString() + "\"";

                            string newWSName = "Workspace=\"" + replacement.Value.WorkspaceName.ToString() + "\"";
                            string newWSType = "WorkspaceTypeId=\"" + replacement.Value.WorkspaceTypeId.ToString() + "\"";

                            string replaced = line.Replace(oldPKID, newPKID);
                            replaced = replaced.Replace(oldMachine, newMachine);
                            replaced = replaced.Replace(oldWSName, newWSName);
                            replaced = replaced.Replace(oldWSType, newWSType);

                            if (Server)
                            {
                                string oldState = "State=\"" + replacement.Key.State.ToString() + "\"";
                                string oldVCUser = "VCUser=\"" + replacement.Key.VCUser.ToString() + "\"";

                                string newState = "State=\"" + replacement.Value.State.ToString() + "\"";
                                string newVCUser = "VCUser=\"" + replacement.Value.VCUser.ToString() + "\"";

                                replaced = replaced.Replace(oldState, newState);
                                replaced = replaced.Replace(oldVCUser, newVCUser);
                            }

                            try
                            {
                                foreach (PropertyInfo i in replacement.Value.GetMetaPropertyList(false))
                                {
                                    PropertyInfo pInfo = i;
                                    object value = replacement.Value.Get(pInfo.Name);
                                    if (value != null && value.ToString().Length > 0)
                                    {
                                        //replace
                                        if (replaced.Contains("m" + pInfo.Name + "="))
                                        {
                                            string start = replaced.Substring(replaced.IndexOf("m" + pInfo.Name + "="));
                                            start = start.Substring(0, start.IndexOf("\" ") + 1);
                                            try
                                            {
                                                replaced = replaced.Replace(start, "m" + pInfo.Name + "=\"" + value.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ") + "\" ");
                                            }
                                            catch (Exception ex)
                                            {
                                                Log.WriteLog(pInfo.Name.ToString() + "::" + replaced + Environment.NewLine + ex.ToString());
                                            }
                                        }
                                        else //add
                                        {
                                            replaced = replaced.Insert(replaced.Length - 2, "m" + pInfo.Name + "=\"" + value.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ") + "\" ");
                                        }
                                    }
                                    else
                                    {
                                        replaced.ToString();
                                        //remove?
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.WriteLog("OLD::" + ex.ToString());
                            }

                            line = replaced;
                        }
                        else if (line.Contains(newPKID) && line.Contains(newMachine))
                        {
                            string replaced = line;

                            try
                            {
                                foreach (PropertyInfo i in replacement.Value.GetMetaPropertyList(false))
                                {
                                    PropertyInfo pInfo = i;
                                    object value = replacement.Value.Get(pInfo.Name);
                                    if (value != null && value.ToString().Length > 0)
                                    {
                                        //replace
                                        if (replaced.Contains("m" + pInfo.Name + "="))
                                        {
                                            string start = replaced.Substring(replaced.IndexOf("m" + pInfo.Name + "="));
                                            start = start.Substring(0, start.IndexOf("\" ") + 1);
                                            try
                                            {
                                                replaced = replaced.Replace(start, "m" + pInfo.Name + "=\"" + value.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ") + "\" ");
                                            }
                                            catch (Exception ex)
                                            {
                                                Log.WriteLog(pInfo.Name.ToString() + "::" + replaced + Environment.NewLine + ex.ToString());
                                            }
                                        }
                                        else //add
                                        {
                                            replaced = replaced.Insert(replaced.Length - 2, "m" + pInfo.Name + "=\"" + value.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ") + "\" ");
                                        }
                                    }
                                    else
                                    {
                                        replaced.ToString();
                                        //remove?
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.WriteLog("NEW::" + ex.ToString());
                            }

                            line = replaced;
                        }
                    }
                }
                sb.AppendLine(line);
            }

            sreader.Close();

            StreamWriter swriter = new StreamWriter(tempFile);
            swriter.WriteLine(sb.ToString());
            swriter.Close();

            mergedBinary = Core.Variables.Instance.DiagramPath + Core.strings.GetFileNameOnly(file.Name) + "";
            ZipUtil.ZipFile(tempFile, mergedBinary);
            RetrieveBlob(mergedBinary, file, false);
            UpdateBlob(file, Provider);

            /*System.Threading.ThreadStart ts = new System.Threading.ThreadStart(WaitAndDeleteMergedFiles);
            System.Threading.Thread t = new System.Threading.Thread(ts);
            t.Start();*/
        }
        public bool DeleteMetaObjectsInDatabaseFile(GraphFile fullFile, List<MetaBase> objs, string Provider)
        {
            //Unzip File
            try
            {
                tempFile = Core.Variables.Instance.DiagramPath + "\\tempDeleteObjectsFile.xml";

                MemoryStream memStream = new MemoryStream();
                int len = fullFile.Blob.Length;
                int count = 0;
                memStream = new MemoryStream(fullFile.Blob.Length);
                while (count < len)
                {
                    memStream.WriteByte(fullFile.Blob[count]);
                    count++;
                }
                memStream.Position = 0;

                ZipUtil.UnzipStream(memStream, tempFile);
                memStream.Close();

                //Read into XMLDocument
                XmlDocument doc = new XmlDocument();
                doc.Load(tempFile);
                XmlNodeList elemList = doc.DocumentElement.GetElementsByTagName("metaobject");
                List<int> goObjectIDs = new List<int>();
                Dictionary<XmlNode, XmlNode> itemsToRemoveWithParent = new Dictionary<XmlNode, XmlNode>();

                //OBJECTS
                foreach (XmlNode n in elemList)
                {
                    XmlNode parentNode = n.ParentNode; //parent of metaobject
                    XmlNode grandParentNode = parentNode.ParentNode; //parent of metaobjects parent
                    //should we remove this node
                    bool remove = false;
                    foreach (MetaBase mObj in objs)
                    {
                        //should we check state?
                        //(n.Attributes["State"].Value.ToString() == "Obsolete" || n.Attributes["State"].Value.ToString() == "MarkedForDelete") &&
                        if (n.Attributes["Pkid"].Value.ToString() == mObj.pkid.ToString() && n.Attributes["MachineName"].Value.ToString() == mObj.MachineName.ToString())
                        {
                            remove = true;
                            break;
                        }
                    }
                    if (!remove)
                        continue;

                    //get 'GOids' so we can remove attached associations
                    int id = 0;
                    int.TryParse(n.Attributes["id"].Value.ToString(), out id);
                    if (id > 0)
                        goObjectIDs.Add(id);

                    if (parentNode.Attributes["id"] != null)
                    {
                        int idParent = 0;
                        int.TryParse(parentNode.Attributes["id"].Value.ToString(), out idParent);
                        if (id > 0)
                            goObjectIDs.Add(idParent);
                    }
                    //add all child object ids of parentNode
                    foreach (XmlNode childNode in parentNode.ChildNodes)
                    {
                        //what if it is missing the id attribute?
                        if (childNode.Attributes["id"] != null)
                        {
                            int.TryParse(childNode.Attributes["id"].Value.ToString(), out id);
                            if (id > 0 && !(goObjectIDs.Contains(id)))
                                goObjectIDs.Add(id);
                        }
                    }

                    itemsToRemoveWithParent.Add(parentNode, grandParentNode);
                }

                //LINKS
                List<int> linkGoIDSRemoved = new List<int>();
                elemList = doc.DocumentElement.GetElementsByTagName("link");
                //itemsToRemoveWithParent = new Dictionary<XmlNode, XmlNode>();
                foreach (XmlNode n in elemList)
                {
                    foreach (int goID in goObjectIDs)
                    {
                        //remove all links have to or from with this id
                        if (n.Attributes["to"].Value.ToString() == goID.ToString() || n.Attributes["from"].Value.ToString() == goID.ToString())
                        {
                            itemsToRemoveWithParent.Add(n, n.ParentNode);

                            int linkID = 0;
                            int.TryParse(n.Attributes["id"].Value.ToString(), out linkID);
                            if (linkID > 0)
                                linkGoIDSRemoved.Add(linkID);

                            break;
                        }
                    }
                }

                //ARTEFACTS
                elemList = doc.DocumentElement.GetElementsByTagName("fishLink");
                foreach (XmlNode n in elemList)
                {
                    int toLinkID = 0;
                    int.TryParse(n.Attributes["toLink"].Value.ToString(), out toLinkID);
                    if (toLinkID <= 0)
                        continue;

                    foreach (int removedLinkID in linkGoIDSRemoved)
                    {
                        if (toLinkID == removedLinkID)
                        {
                            if (!itemsToRemoveWithParent.ContainsKey(n))
                                itemsToRemoveWithParent.Add(n, n.ParentNode);

                            int fromArtID = 0;
                            int.TryParse(n.Attributes["fromArt"].Value.ToString(), out fromArtID);
                            if (fromArtID == 0)
                                break;
                            //find this fishlinks artefact
                            foreach (XmlNode art in doc.DocumentElement.GetElementsByTagName("artefact"))
                            {
                                int ArtID = 0;
                                int.TryParse(art.Attributes["id"].Value.ToString(), out ArtID);
                                if (ArtID == 0 || ArtID != fromArtID)
                                    continue;

                                if (!itemsToRemoveWithParent.ContainsKey(art))
                                    itemsToRemoveWithParent.Add(art, art.ParentNode);
                                break; //Only 1 fromArt per fishlink
                            }
                            break;
                        }
                    }
                }

                //RATIONALES anchored to link OR OBJECT
                elemList = doc.DocumentElement.GetElementsByTagName("rationale");
                foreach (XmlNode rat in elemList)
                {
                    int anchorID = 0;
                    //Bug where rationales are not anchored
                    if (rat.Attributes["anchor"] != null)
                        int.TryParse(rat.Attributes["anchor"].Value.ToString(), out anchorID);
                    if (anchorID <= 0)
                        continue;

                    bool found = false;
                    //Try find in objects
                    foreach (int gNodeID in goObjectIDs)
                    {
                        if (anchorID == gNodeID)
                        {
                            if (!itemsToRemoveWithParent.ContainsKey(rat))
                                itemsToRemoveWithParent.Add(rat, rat.ParentNode);
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        continue;

                    //Try find on links
                    foreach (int linkID in linkGoIDSRemoved)
                    {
                        if (anchorID == linkID)
                        {
                            if (!itemsToRemoveWithParent.ContainsKey(rat))
                                itemsToRemoveWithParent.Add(rat, rat.ParentNode);
                            break;
                        }
                    }
                }
                //LINKS

                //REMOVE ALL ITEMS WE FOUND
                foreach (KeyValuePair<XmlNode, XmlNode> kvp in itemsToRemoveWithParent)
                {
                    kvp.Key.RemoveAll(); //remove all children of node
                    kvp.Value.RemoveChild(kvp.Key); //remove node from parent
                }

                //Save file
                elemList = null;
                goObjectIDs = null;
                doc.Save(tempFile);
                doc = null;

                //Zip file and update blob @ Provider
                deletedBinary = Core.Variables.Instance.DiagramPath + Core.strings.GetFileNameOnly(fullFile.Name) + "";
                ZipUtil.ZipFile(tempFile, deletedBinary);
                RetrieveBlob(deletedBinary, fullFile, false);
                UpdateBlob(fullFile, Provider);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                Core.Log.WriteLog("Failed to delete objects in " + fullFile.Name + " because..." + Environment.NewLine + ex.ToString());
            }
        }
        public void UpdateObjectWorkspaceInFiles(GraphFile fullFile, TList<MetaObject> objs, string Provider)
        {
            //open file and change all objects found with the new workspace
            try
            {
                tempFile = Core.Variables.Instance.DiagramPath + "\\tempDeleteObjectsFile.xml";

                MemoryStream memStream = new MemoryStream();
                int len = fullFile.Blob.Length;
                int count = 0;
                memStream = new MemoryStream(fullFile.Blob.Length);
                while (count < len)
                {
                    memStream.WriteByte(fullFile.Blob[count]);
                    count++;
                }
                memStream.Position = 0;

                ZipUtil.UnzipStream(memStream, tempFile);
                memStream.Close();

                //Read into XMLDocument
                XmlDocument doc = new XmlDocument();
                doc.Load(tempFile);
                XmlNodeList elemList = doc.DocumentElement.GetElementsByTagName("metaobject");

                XmlNodeList elemListImageNodes = doc.DocumentElement.GetElementsByTagName("imageNode");
                XmlNodeList elemListQLinks = doc.DocumentElement.GetElementsByTagName("link");
                //OBJECTS
                foreach (XmlNode n in elemList)
                {
                    //should we remove this node
                    foreach (MetaObject mObj in objs)
                    {
                        //image nodes need to have this replace
                        // WriteAttrVal("shallowCN", node.MetaObject.pkid + ":" + node.Location.ToString());
                        //links have to have this replace as well
                        // WriteAttrVal("shallowFromN", node.MetaObject.pkid + ":" + node.Location.ToString());
                        // WriteAttrVal("shallowToN", node.MetaObject.pkid + ":" + node.Location.ToString());

                        // WriteAttrVal("fromMB", node.MetaObject.pkid + ":" + node.Location.ToString());
                        // WriteAttrVal("toMB", node.MetaObject.pkid + ":" + node.Location.ToString());

                        //(n.Attributes["State"].Value.ToString() == "Obsolete" || n.Attributes["State"].Value.ToString() == "MarkedForDelete") &&
                        if (n.Attributes["Pkid"].Value.ToString() == mObj.pkid.ToString() && n.Attributes["MachineName"].Value.ToString() == mObj.Machine.ToString())
                        {
                            //set workspace
                            n.Attributes["Workspace"].Value = mObj.WorkspaceName;
                            n.Attributes["WorkspaceTypeId"].Value = mObj.WorkspaceTypeId.ToString();
                            continue;
                        }
                    }
                }

                //Save file
                elemList = null;
                doc.Save(tempFile);
                doc = null;

                //Zip file and update blob @ Provider
                deletedBinary = Core.Variables.Instance.DiagramPath + Core.strings.GetFileNameOnly(fullFile.Name) + "";
                ZipUtil.ZipFile(tempFile, deletedBinary);
                RetrieveBlob(deletedBinary, fullFile, false);
                UpdateBlob(fullFile, Provider);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("Failed to update objects workspace in " + fullFile.Name + " because..." + Environment.NewLine + ex.ToString());
            }
        }

        public void UpdateBlob(GraphFile file)
        {
            //file.EntityState = EntityState.Changed;
            //d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.Save(file);
            //return;
            SqlConnection conn = new SqlConnection(Core.Variables.Instance.ConnectionString);
            SqlCommand addEmp = new SqlCommand("UPDATE GRAPHFILE SET BLOB = @Blob, ModifiedDate = @Modified WHERE PKID=@PKID", conn);

            addEmp.Parameters.Add("@Blob", SqlDbType.Image, file.Blob.Length).Value = file.Blob;
            addEmp.Parameters.Add("@PKID", SqlDbType.Int).Value = file.pkid;
            addEmp.Parameters.Add("@Modified", SqlDbType.DateTime).Value = DateTime.Now;

            // Open the Connection and INSERT the BLOB into the Database
            conn.Open();
            addEmp.ExecuteNonQuery();
            conn.Close();
        }
        public void UpdateBlob(GraphFile file, string Provider)
        {
            //file.EntityState = EntityState.Changed;
            //d.DataRepository.Connections[Provider].Provider.GraphFileProvider.Save(file);
            //return;
            string connstring = Provider == Core.Variables.Instance.ServerProvider ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand addEmp = new SqlCommand("UPDATE GRAPHFILE SET BLOB = @Blob, ModifiedDate = @Modified WHERE PKID=@PKID", conn);

            addEmp.Parameters.Add("@Blob", SqlDbType.Image, file.Blob.Length).Value = file.Blob;
            addEmp.Parameters.Add("@PKID", SqlDbType.Int).Value = file.pkid;
            addEmp.Parameters.Add("@Modified", SqlDbType.DateTime).Value = DateTime.Now;

            // Open the Connection and INSERT the BLOB into the Database
            conn.Open();
            addEmp.ExecuteNonQuery();
            conn.Close();
        }

        public NormalDiagram RetrieveGraphDoc(GraphFile graphFile)
        {
            MemoryStream memStream = new MemoryStream();
            int len = graphFile.Blob.Length;
            int count = 0;
            memStream = new MemoryStream(graphFile.Blob.Length);
            while (count < len)
            {
                memStream.WriteByte(graphFile.Blob[count]);
                count++;
            }
            memStream.Position = 0;

            NormalDiagram retval = null;
            try
            {
                retval = LoadCompressedXMLFile(memStream, Core.strings.GetFileNameOnly(graphFile.Name));
            }
            catch
            {

            }

            if (retval == null)
            {
                // reopen stream and deserialize for old format
                memStream = new MemoryStream();
                len = graphFile.Blob.Length;
                count = 0;
                memStream = new MemoryStream(graphFile.Blob.Length);
                while (count < len)
                {
                    memStream.WriteByte(graphFile.Blob[count]);
                    count++;
                }
                memStream.Position = 0;
                retval = DeserializeStream(memStream, graphFile.Name);

                if (retval == null)
                {
                    // Rebuild the stream for binary deserialization
                    memStream = new MemoryStream();
                    len = graphFile.Blob.Length;
                    count = 0;
                    memStream = new MemoryStream(graphFile.Blob.Length);
                    while (count < len)
                    {
                        memStream.WriteByte(graphFile.Blob[count]);
                        count++;
                    }
                    memStream.Position = 0;
                    retval = DeserializeBinaryFile(memStream, graphFile.pkid, graphFile.MachineName);
                }
            }
            if (retval != null)
            {
                retval.VersionManager.CurrentVersion.PKID = graphFile.pkid;
                retval.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier = graphFile.OriginalFileUniqueID;
                retval.VersionManager.CurrentVersion.MachineName = graphFile.MachineName;
                retval.VersionManager.CurrentVersion.WorkspaceName = graphFile.WorkspaceName;
                retval.VersionManager.CurrentVersion.WorkspaceTypeId = graphFile.WorkspaceTypeId;
                retval.VersionManager.CurrentVersion.VCStatus = (VCStatusList)graphFile.VCStatusID;
            }
            else
            {
                Log.WriteLog("GraphFileManager::RetrieveGraphDoc::All attempted deserialization failed for file " + graphFile.Name + " - BLOBL Length " + graphFile.Blob.LongLength);
            }
            memStream.Close();
            return retval;
        }
        public GraphFile savedFile = null;
        public NormalDiagram SaveToDatabase(NormalDiagram ndiagram, string filename, bool SaveNewRecord)
        {
            workspaces = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetAll();
            bool oldskips = ndiagram.SkipsUndoManager;
            ndiagram.SkipsUndoManager = true;
            GraphFileManager gfmanager = new GraphFileManager();
            GraphFile file = gfmanager.ConvertToCompressedGraphFile(ndiagram, filename);
            file.IsActive = true;
            file.ModifiedDate = DateTime.Now;
            foreach (DocumentVersion v in ndiagram.VersionManager.Versions)
            {
                if (v.PreviousDocumentID > 0)
                {
                    file.PreviousVersionID = v.PreviousDocumentID;
                    break;
                }
            }
            if (!(SaveNewRecord) && (ndiagram.VersionManager.CurrentVersion.PKID > 0))//
            {
                file.pkid = ndiagram.VersionManager.CurrentVersion.PKID;
                file.OriginalFileUniqueID = ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier;
            }
            if (file.OriginalFileUniqueID == Guid.Empty)
            {
                file.OriginalFileUniqueID = Guid.NewGuid();
            }
            if (!Exists(file.WorkspaceTypeId, file.WorkspaceName))
            {
                //If it does not exist set it to the current workspace

                //Workspace ws = d.DataRepository.WorkspaceProvider.GetByNameWorkspaceTypeId("Sandbox", 1);
                //file.WorkspaceName = (ws != null) ? ws.Name : Variables.Instance.CurrentWorkspaceName;
                file.WorkspaceName = Variables.Instance.CurrentWorkspaceName;
                //file.WorkspaceTypeId = (ws != null) ? ws.WorkspaceTypeId : Variables.Instance.CurrentWorkspaceTypeId;
                file.WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;
            }
            if (file.WorkspaceTypeId == 3)
                file.VCUser = strings.GetVCIdentifier();
            try
            {
                if (SaveNewRecord)
                    d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.Save(file);
                else
                {
                    ndiagram.VersionManager.CurrentVersion.PKID = file.pkid;// = ndiagram.VersionManager.CurrentVersion.PKID;
                    file.OriginalFileUniqueID = ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier;
                    savedFile = file;
                    UpdateBlob(file);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("GraphFileManager - SaveToDatabase Cannot Save Blob", ex.ToString(), System.Diagnostics.TraceEventType.Critical);
            }
            try
            {
                gfmanager.MarkPreviousVersionsInactive(file);
                try
                {
                    if (ndiagram != null)
                    {
                        ndiagram.FileMachine = file.MachineName;
                        ndiagram.VersionManager.CurrentVersion.PKID = file.pkid;
                        ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier = file.OriginalFileUniqueID;

                        if (ndiagram.DocumentFrame != null)
                        {
                            ndiagram.DocumentFrame.txtFileName.Text = strings.GetFileNameOnly(filename);
                        }
                        ndiagram.IsModified = false;
                        ndiagram.SkipsUndoManager = oldskips;
                    }
                }
                catch
                {
                    //catch bug where ndiagram is null because it is closed
                }
                return ndiagram;
            }
            catch (Exception ex)
            {
                Log.WriteLog("GraphFileManager - SaveToDatabase Did Not Mark Inactive", ex.ToString(), System.Diagnostics.TraceEventType.Critical);
            }
            return null;
        }

        // Private Methods (5) 

        private void DeleteXmlFile()
        {
            try
            {
                //System.Threading.Thread.Sleep(5000);
                File.Delete(xmlFile);
            }
            catch
            {
                try
                {
                    System.Threading.Thread.Sleep(5000);
                    File.Delete(xmlFile);
                }
                catch
                {
                }
            }
        }

        private NormalDiagram DeserializeStream(Stream stream, string Filename)
        {
            NormalDiagram ndiagram = LoadCompressedXMLFile(stream, Filename);
            if (ndiagram != null)
                return ndiagram;

            return null;
            //ndiagram = DeserializeBinaryFile(stream);
            return ndiagram;
        }

        private void RetrieveBlob(string tempFile, GraphFile GraphFile, bool delete)
        {
            GraphFile.Blob = File.ReadAllBytes(tempFile);
            this.tempFile = tempFile;
            if (delete)
            {
                WaitAndDeleteBlobFile();
                //System.Threading.ThreadStart ts = new System.Threading.ThreadStart(WaitAndDeleteBlobFile);
                //System.Threading.Thread t = new System.Threading.Thread(WaitAndDeleteBlobFile);
                //t.Start();
            }
        }

        private void WaitAndDeleteBlobFile()
        {
            bool success = false;
            while (!success)
            {
                try
                {
                    //System.Threading.Thread.Sleep(2000);
                    File.Delete(tempFile);
                    success = true;
                }
                catch
                {
                }
            }

            //try
            //{
            //    System.Threading.Thread.Sleep(2000);
            //    File.Delete(tempFile);
            //}
            //catch
            //{
            //    try
            //    {
            //        System.Threading.Thread.Sleep(2000);
            //        File.Delete(tempFile);
            //    }
            //    catch
            //    {
            //    }
            //}
        }

        private void WaitAndDeleteMergedFiles()
        {
            try
            {
                System.Threading.Thread.Sleep(5000);
                File.Delete(tempFile);
                File.Delete(mergedBinary);
            }
            catch
            {
                try
                {
                    System.Threading.Thread.Sleep(5000);
                    File.Delete(tempFile);
                    File.Delete(mergedBinary);
                }
                catch
                {

                }
            }
        }

        #endregion Methods

        private sealed class NormalDiagramSerializationBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Type typeToDeserialize = null;
                // For each assemblyName/typeName that you want to deserialize to
                // a different type, set typeToDeserialize to the desired type.
                String assemVer1 = Assembly.GetExecutingAssembly().FullName;
                if (assemblyName == assemVer1 && typeName == "NormalDiagram")
                {
                    // To use a type from a different assembly version, 
                    // change the version number.
                    // To do this, uncomment the following line of code.

                    // To use a different type from the same assembly, 
                    // change the type name.
                    typeName = "NormalDiagram";
                }
                assemblyName = assemblyName.Replace("0.0.0.0", "1.0.7.0");
                // The following line of code returns the type.
                typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
                return typeToDeserialize;
            }
        }

    }
}