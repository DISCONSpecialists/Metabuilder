using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace MetaBuilder.Graphing.Persistence
{
    public class ZipUtil
    {

        #region Fields (1)

        const int BUFFER_SIZE = 65536;

        #endregion Fields

        #region Methods (3)

        // Public Methods (3) 

        public static bool UnzipFile(string zipFile, string targetFile)
        {
            FileStream sourceFile = File.OpenRead(zipFile);
            return UnzipStream(sourceFile, targetFile);
        }

        public static bool UnzipStream(Stream zippedStream, string targetFilename)
        {
            bool success = false;
            FileStream destFile = File.Create(targetFilename);
            GZipStream compStream = new GZipStream(zippedStream, CompressionMode.Decompress);
            try
            {
                int theByte = compStream.ReadByte();
                while (theByte != -1)
                {
                    destFile.WriteByte((byte)theByte);
                    theByte = compStream.ReadByte();
                }
                success = true;
            }
            catch
            {
                success = false;
            }
            finally
            {
                compStream.Dispose();
            }
            destFile.Close();
            return success;
        }

        public static void ZipFile(string ifname, string ofname)
        {
            // Open the input file as a stream
            using (FileStream fsi = new FileStream(ifname, FileMode.Open, FileAccess.Read))
            {
                // Create the output stream
                FileStream fso;
                if (File.Exists(ofname))
                {
                    File.Delete(ofname);
                }
                // Workaround for people who save to folders that no longer exist (eg: network drive)
                string directoryName = Core.strings.GetPath(ofname);
                if (!Directory.Exists(directoryName))
                {
                    ofname = Core.Variables.Instance.DiagramPath + "\\NEW - " + Core.strings.GetFileNameOnly(ofname);
                    Core.Log.WriteLog("Directory does not exist for " + ofname + " - Saving to diagram path");
                }

                fso = new FileStream(ofname, FileMode.Create, FileAccess.Write);
                using (fso)
                {
                    // Create the GZip stream, attached to the output stream.
                    using (GZipStream gzs = new GZipStream(fso, CompressionMode.Compress, false))
                    {
                        byte[] buff = new byte[BUFFER_SIZE];
                        int bytesRead;
                        // Read input and write to compression stream.
                        while ((bytesRead = fsi.Read(buff, 0, BUFFER_SIZE)) != 0)
                        {
                            gzs.Write(buff, 0, bytesRead);
                        }
                    }
                }
            }
        }

        #endregion Methods

    }
}
