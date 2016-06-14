using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core.Storage;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.Graphing.Utilities
{
    public class StorageManipulator
    {
        #region Nested Classes (2)

        #region Nested type: DatabaseManipulator

        public class DatabaseManipulator
        {
            #region Methods (4)

            // Public Methods (4) 

            public static BaseDocument Load(int DocumentID)
            {
                throw new NotImplementedException("Not implemented");
            }

            public static void LoadObject(GraphNode node, int ObjectID, LoadOptions options)
            {
            }

            public static void Save(BaseDocument document)
            {
                throw new NotImplementedException("Not implemented");
            }

            public static void SaveObject(GraphNode node)
            {
            }

            #endregion Methods
        }

        #endregion

        #region Nested type: FileSystemManipulator

        public class FileSystemManipulator
        {
            #region Methods (6)

            // Public Methods (5) 

            public static BaseDocument LoadDocument(string Filename, FileDialogSpecification dialogSpecification)
            {
                // 1st see if it was compressed (default)
                FileStream fstream = File.OpenRead(Filename);
                IFormatter formatter = new BinaryFormatter();
                GZipStream decomp = null;
                FileStream fstreamNormal = File.OpenRead(Filename);
                IFormatter formatterNormal = new BinaryFormatter();
                try
                {
                    // MemoryStream -> Decompressor -> RawStreamDecoder -> string
                    decomp = new GZipStream(fstream, CompressionMode.Decompress);
                    try
                    {
                        if (Filename.ToLower().Contains("dgm"))
                        {
                            NormalDiagram ndiag = formatter.Deserialize(decomp) as NormalDiagram;
                            ndiag.Name = Filename;
                            ndiag.FileType = dialogSpecification.FileType;
                            decomp.Close();
                            fstream.Close();
                            return ndiag;
                        }
                        BaseDocument retval = formatter.Deserialize(decomp) as BaseDocument;
                        retval.Name = Filename;
                        retval.FileType = dialogSpecification.FileType;
                        if (retval is NormalDiagram)
                        {
                            decomp.Close();
                            fstream.Close();
                            return retval as NormalDiagram;
                        }
                        if (retval is Stencil)
                        {
                            decomp.Close();
                            fstream.Close();
                            return retval as Stencil;
                        }
                        decomp.Close();
                        fstream.Close();
                        return retval;
                    }
                    catch (Exception xxx)
                    {
                        // Console.WriteLine(xxx.ToString());
                        throw (xxx);
                    }
                    // Deserializes from decompressor stream
                }
                catch
                {
                    try
                    {
                        BaseDocument doc = formatterNormal.Deserialize(fstreamNormal) as BaseDocument;
                        BaseDocument baseDoc = doc;
                        NormalDiagram objdoc;
                        if (baseDoc == null)
                        {
                            fstreamNormal = File.OpenRead(Filename);
                            objdoc = formatterNormal.Deserialize(fstreamNormal) as NormalDiagram;
                            baseDoc = objdoc;
                        }
                        if (baseDoc == null)
                        {
                            fstreamNormal = File.OpenRead(Filename);
                            Stencil stn = formatterNormal.Deserialize(fstreamNormal) as Stencil;
                            baseDoc = stn;
                        }
                        baseDoc.FileType = dialogSpecification.FileType;
                        baseDoc.Name = Filename;
                        fstreamNormal.Close();
                        return baseDoc;
                    }
                    catch (Exception x)
                    {
                        LogEntry logEntry = new LogEntry();
                        logEntry.Message = x.ToString();
                        // Console.WriteLine(x.ToString());
                        Logger.Write(logEntry);
                        return null;
                    }
                }
                finally
                {
                    try
                    {

                        fstream.Close();
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (decomp != null)
                        {
                            decomp.Close();
                        }
                    }
                    catch
                    {
                    }
                    try
                    {

                        fstreamNormal.Close();
                    }
                    catch
                    {
                    }
                }
            }

            public static GoObject LoadObject(string Filename, FileDialogSpecification dialogSpecification)
            {
                if (File.Exists(Filename))
                {
                    FileStream fstream = File.OpenRead(Filename);
                    IFormatter formatter = new BinaryFormatter();
                    switch (dialogSpecification.FileType)
                    {
                        case FileTypeList.ArrowHead:
                            GoObject obj = (GoObject)formatter.Deserialize(fstream);
                            fstream.Close();
                            return obj;
                        case FileTypeList.Image:
                            QuickImage graphImage = (QuickImage)formatter.Deserialize(fstream);
                            fstream.Close();
                            return graphImage;
                        case FileTypeList.Symbol:
                            try
                            {
                                GraphNode node = (GraphNode)formatter.Deserialize(fstream);
                                fstream.Close();
                                return node;
                            }
                            catch
                            {
                            }
                            break;
                    }
                }
                return null;
            }

            public static GoObject LoadSymbol(string Filename)
            {
                if (File.Exists(Filename))
                {
                    FileStream fstream = File.OpenRead(Filename);
                    IFormatter formatter = new BinaryFormatter();
                    GraphNode node = (GraphNode)formatter.Deserialize(fstream);
                    fstream.Close();
                    return node;
                }
                return null;
            }

            public static bool SaveDocument(BaseDocument obj, string Filename)
            {
                // string -> RawStreamEncoder -> Compressor -> MemoryStream
                FileInfo finfo = new FileInfo(Filename);
                if (finfo.Exists)
                {
                    try
                    {
                        finfo.Delete();
                    }
                    catch
                    {
                        Thread.Sleep(500);
                    }
                }
                try
                {
                    return AttemptSave(obj, Filename);
                }
                catch
                {
                    try
                    {
                        Thread.Sleep(500);
                        bool saved = AttemptSave(obj, Filename);
                        return saved;
                    }
                    catch (Exception xx)
                    {
                        LogEntry logEntry = new LogEntry();
                        logEntry.Message = xx.ToString();
                        logEntry.Title = "Cannot save";
                        if (xx is IOException || xx is UnauthorizedAccessException)
                        {
                            //changed properties to permissions
                            logEntry.Message += Environment.NewLine +
                                                "Please ensure that the file is in a location where you have write permissions";
                        }
                        Logger.Write(logEntry);
                        return false;
                    }
                }
                /*
                
                formatter.Serialize(memStream, obj);
                memStream.Close();

                System.IO.Compression.GZipStream zipStream = new System.IO.Compression.GZipStream(fstream, System.IO.Compression.CompressionMode.Compress);
                        * */
            }

            public static void SaveObject(GoObject obj, string Filename)
            {
                IFormatter formatter = new BinaryFormatter();
                FileInfo finfo = new FileInfo(Filename);
                if (finfo.Exists)
                {
                    finfo.Delete();
                }
                FileStream fstream = new FileStream(Filename, FileMode.CreateNew);
                formatter.Serialize(fstream, obj);
                fstream.Close();
            }

            // Private Methods (1) 

            private static bool AttemptSave(BaseDocument obj, string Filename)
            {
                FileStream fstream = new FileStream(Filename, FileMode.Create);
                GZipStream comp = new GZipStream(fstream, CompressionMode.Compress);
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(comp, obj);
                comp.Close();
                fstream.Close();
                return true;
            }

            #endregion Methods
        }

        #endregion

        #endregion Nested Classes
    }

    public enum LoadOptions
    {
        Shallow,
        Deep
    }
}