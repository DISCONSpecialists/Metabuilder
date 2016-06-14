using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Meta;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using System.Configuration.Provider;

namespace MetaBuilder.UIControls.GraphingUI.Tools
{
    public static class DeleteObjectsFromFiles
    {
        //This class acts on database BLOBS in order to remove the objects from them and then increment the version and insert the 'new' BLOB
        //First of all we can only delete Marked For Delete or Obsolete Objects
        //This should only be used on the server as clients do not have a 'Delete' VCStatus
        //Once a client deletes an object it is no longer in his database, therefore it being on a file is of little significance however on the server many poeple may have the file.
        public static bool DeleteObjects(List<MetaBase> objs, List<GraphFile> files, string Provider)
        {
            if (files == null) //MFD!
            {
                files = new List<GraphFile>();
                //Get Files
                MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter fileAdapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                foreach (MetaBase mbObj in objs)
                {
                    TList<GraphFile> xFiles = fileAdapter.GetFilesByObjectId(mbObj.pkid, mbObj.MachineName, Provider == Core.Variables.Instance.ServerProvider);
                    foreach (GraphFile f in xFiles)
                    {
                        if (!f.IsActive)
                            continue;

                        if (!files.Contains(f))
                            files.Add(f);
                    }
                }
            }

            //Open each file and remove objects
            MetaBuilder.Graphing.Persistence.GraphFileManager gfm = new MetaBuilder.Graphing.Persistence.GraphFileManager();
            foreach (GraphFile f in files)
            {
                //GraphFile fullFile = DataRepository.Connections[Provider].Provider.GraphFileProvider.GetBypkidMachine(f.pkid, f.Machine);
                gfm.DeleteMetaObjectsInDatabaseFile(f, objs, Provider);
            }
            return true;
        }

    }
}
