using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core.Storage;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.Threading;
//TRACING::using TraceTool;
namespace MetaBuilder.Graphing.Persistence
{
    public class FileUtil
    {
        public event EventHandler Report;
        protected void OnReport(object sender, EventArgs e)
        {
            if (Report != null)
                Report(sender, e);
        }

        #region Fields (2)

        private string tempFile;
        private string tempFileName;

        #endregion Fields

        #region Methods (9)

        // Public Methods (7) 

        public FileType GetFileType(string filename)
        {
            if (filename.ToLower().EndsWith("mmdgm"))
                return FileType.MindMap;
            if (filename.ToLower().EndsWith("vicxml"))
                return FileType.SavedViewInContext;
            if (filename.ToLower().EndsWith("dgm"))
                return FileType.Diagram;
            if (filename.ToLower().EndsWith("stn"))
                return FileType.Stencil;
            if (filename.ToLower().EndsWith("sym"))
                return FileType.Symbol;
            return FileType.Unknown;
        }

        public static b.FileTypeList GetFileTypeList(string filename)
        {
            if (filename.ToLower().Contains("dgm"))
                return b.FileTypeList.Diagram;

            if (filename.ToLower().Contains(".sym"))
                return b.FileTypeList.Symbol;

            if (filename.ToLower().Contains(".vicxml"))
                return b.FileTypeList.SavedContextView;

            return b.FileTypeList.SymbolStore;
        }

        public string GetTempFileName(string filename)
        {
            return filename + ".xml";
        }

        public static BaseDocument LoadDocument(string Filename)
        {
            // 1st see if it was compressed (default)
            IFormatter formatter = new BinaryFormatter();
            GZipStream decomp = null;
            FileStream fstream = File.OpenRead(Filename);
            FileTypeList ftl = GetFileTypeList(Filename);

            try
            {
                // MemoryStream -> Decompressor -> RawStreamDecoder -> string
                decomp = new GZipStream(fstream, CompressionMode.Decompress);
                BaseDocument retval = null;
                try
                {
                    if (Filename.ToLower().Contains("dgm"))
                    {
                        retval = formatter.Deserialize(decomp) as NormalDiagram;
                        retval.FileType = b.FileTypeList.Diagram;
                    }
                }
                catch
                {

                }

                if (retval == null)
                {
                    retval = formatter.Deserialize(decomp) as BaseDocument;
                    retval.FileType = ftl;
                }
                retval.Name = Filename;
                return retval;
            }
            catch
            {
                return null;
            }
            finally
            {
                fstream.Close();
                decomp.Close();
            }
        }

        string dots = "";
        private void DepersistTimer_Tick(object info)
        {
            if (dots == ".....")
                dots = "";
            dots += ".";
            OnReport("Depersisting" + dots, EventArgs.Empty);
        }

        public object Open(string filename)
        {
            tempFile = GetTempFileName(filename);
            FileType ftype = GetFileType(filename);
            OnReport("Opening", EventArgs.Empty);
            Core.Log.WriteLog("Opening " + filename + " TYPE: " + ftype.ToString());

            bool unzipped = false;
            try
            {
                OnReport("Extracting", EventArgs.Empty);
                unzipped = ZipUtil.UnzipFile(filename, tempFile);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("Failed Unzip" + Environment.NewLine + ex.ToString());
                if (ftype == FileType.Symbol)
                {
                    FileDialogSpecification fspec = FilePathManager.Instance.GetSpecification(FileTypeList.Symbol);
                    GraphNode node = (GraphNode)StorageManipulator.FileSystemManipulator.LoadObject(filename, fspec);
                    Symbol s = new Symbol();
                    s.ShapeBindingInfo = node.BindingInfo;
                    s.Add(node);
                    return s;
                }
            }

            object returnedObject = null;
            bool success = false;
            if (unzipped)
            {
                // try XMLPersistor first
                XmlPersistor xpers = new XmlPersistor();
                try
                {
                    OnReport("Depersisting", EventArgs.Empty);

                    // Create an event to signal the timeout count threshold in the
                    // timer callback.
                    AutoResetEvent autoEvent = new AutoResetEvent(false);
                    // Create an inferred delegate that invokes methods for the timer.
                    TimerCallback tcb = DepersistTimer_Tick;
                    // Create a timer that signals the delegate to invoke 
                    // DepersistTimer_Tick after one second, and every 1/2 second 
                    // thereafter.
                    //Console.WriteLine("{0} Creating timer.\n", DateTime.Now.ToString("h:mm:ss.fff"));
                    Timer stateTimer = new Timer(tcb, autoEvent, 1000, 500);

                    //TRACING::TTrace.Debug.Indent("Attempting to DepersistCollection");
                    returnedObject = xpers.DepersistCollection(tempFile, ftype);
                    //TRACING::TTrace.Debug.SendObject("Returned Object", returnedObject);

                    // When autoEvent signals the second time, dispose of 
                    // the timer.
                    //autoEvent.WaitOne(5000, false);
                    stateTimer.Dispose();

                    if (returnedObject == null)
                        success = false;
                    else
                        success = true;
                }
                catch (Exception x)
                {
                    Core.Log.WriteLog("Failed Depersist" + Environment.NewLine + x.ToString());
                }
                finally
                {
#if DEBUG
                    //TRACING::TTrace.Debug.Unindent("Attempt to Depersist Collection Completed");
#endif
                    xpers = null;
                }

                if (!success)
                {
                    try
                    {
                        OnReport("Attempt Binary", EventArgs.Empty);
                        returnedObject = OpenBinary(tempFile, ftype);
                        if (returnedObject == null)
                        {
                            try
                            {
                                OnReport("Attempt Load", EventArgs.Empty);
                                returnedObject = LoadDocument(filename);
                            }
                            catch
                            {
                                //returnedObject = LoadDocument(filename);
                            }
                        }
                        else
                        {
                            Core.Log.WriteLog("Completed Binary Open");
                            success = true;
                        }
                    }
                    catch (Exception xx)
                    {
                        Core.Log.WriteLog("Failed Binary Open" + Environment.NewLine + xx.ToString());
                        //Console.WriteLine(xx.ToString());
                        try
                        {
                            OnReport("Attempt Load", EventArgs.Empty);
                            returnedObject = LoadDocument(filename);
                        }
                        catch
                        {
                            //returnedObject = LoadDocument(filename);
                        }
                    }
                }
            }
            else
            {
                OnReport("Unzip Error - Attempt Load", EventArgs.Empty);
                Core.Log.WriteLog("Failed Unzip - Loading Document");
                returnedObject = LoadDocument(filename);
            }
            OnReport("Delete Temp File", EventArgs.Empty);
            //DeleteTempFile();
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(DeleteTempFile));
            t.Start();
            OnReport("Finalizing", EventArgs.Empty);
            return returnedObject;
            //}
        }

        public object OpenBinary(string filename, FileType ftype)
        {
            object retval = null;
            FileStream fstream = File.OpenRead(filename);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                switch (ftype)
                {
                    case FileType.Diagram:
                        retval = formatter.Deserialize(fstream) as NormalDiagram;
                        break;
                    case FileType.Stencil:
                        retval = formatter.Deserialize(fstream) as Stencil;
                        break;
                    case FileType.Symbol:
                        retval = formatter.Deserialize(fstream) as Symbol;
                        break;
                }
            }
            catch
            {

            }
            fstream.Close();
            return retval;
        }

        public void Save(object graph, string filename)
        {
            //22 October 2013 - Removed duplicate try catch in catch
            try
            {
                TrySavingFile(graph, filename);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("TrySavingFile(" + filename + ") Error " + Environment.NewLine + ex.ToString());
            }
        }

        private void TrySavingFile(object graph, string filename)
        {
            XmlPersistor persistor = new XmlPersistor();
            tempFileName = filename + ".xml";
            FileType ftype = GetFileType(filename);
            if (ftype == MetaBuilder.Graphing.Persistence.FileType.Unknown)
            {
                ftype = MetaBuilder.Graphing.Persistence.FileType.Diagram;
            }
            persistor.PersistGoCollection(graph as ICollection, tempFileName, ftype);
            ZipUtil.ZipFile(tempFileName, filename);

            //DeleteTempZippedFile();
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(DeleteTempZippedFile));
            t.Start();
        }

        // Private Methods (2) 

        private void DeleteTempFile()
        {
            //if (IsFileLocked(tempFile))
            //{
            //    System.Threading.Thread.Sleep(1000);
            //    DeleteTempZippedFile();
            //}
            //else
            //{
            //    System.IO.File.Delete(tempFile);
            //    //success = true;
            //}
            //return;

            bool success = false;
            while (!success)
            {
                try
                {
                    //System.Threading.Thread.Sleep(2000);
                    System.IO.File.Delete(tempFile);
                    success = true;
                }
                catch
                {
                }
            }
        }

        private void DeleteTempZippedFile()
        {
            //if (IsFileLocked(tempFileName))
            //{
            //    System.Threading.Thread.Sleep(1000);
            //    DeleteTempZippedFile();
            //}
            //else
            //{
            //    System.IO.File.Delete(tempFileName);
            //    //success = true;
            //}
            //return;

            bool success = false;
            while (!success)
            {
                try
                {
                    //System.Threading.Thread.Sleep(2000);
                    System.IO.File.Delete(tempFileName);
                    success = true;
                }
                catch
                {
                    //System.Threading.Thread.Sleep(2000);
                    //System.IO.File.Delete(tempFileName);
                    //success = true;
                }
            }
        }

        #endregion Methods

        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException ioex)
            {
                Core.Log.WriteLog(ioex.ToString());
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

    }
    public enum FileType
    {
        Diagram, Symbol, Stencil, Unknown, MindMap, SavedViewInContext
    }
}