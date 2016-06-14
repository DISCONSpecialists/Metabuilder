using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Threading;
using MetaBuilder.Core;
using MetaBuilder.Meta.Editors;

namespace MetaBuilder.Meta
{
    /// <summary>
    /// Summary description for LongText.
    /// </summary>
    [Serializable]
    [Editor(typeof(FileEditor), typeof(UITypeEditor))]
    public class Attachment
    {

        public override string ToString()
        {
            return name;
        }
        public Attachment()
        {
        }

        public string Name
        {
            get
            {
                return name;
            }
            set { name = value; }
        }

        private string usermachine;

        public string UserMachine
        {
            get { return usermachine; }
            set { usermachine = value; }
        }


        private string filename;

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        private byte[] bytes;
        public byte[] Bytes
        {
            get { return bytes; }
            set { bytes = value; }
        }

        private string name;


        public void Open()
        {
            if (name != null)
            {
                // 1st save it to disk
                string exportPath = Variables.Instance.ExportsPath + "\\TEMP_" + name;
                FileStream fstream = new FileStream(exportPath, FileMode.Create);
                fstream.Write(Bytes, 0, Bytes.Length - 1);
                fstream.Close();

                Thread.Sleep(1000);
                Process proc = new Process();
                Process.Start(exportPath);
            }
        }

        public void Load(string filename)
        {
            FileStream fstream = new FileStream(filename, FileMode.Open);
            Bytes = new byte[fstream.Length];
            fstream.Read(Bytes, 0, ((int)fstream.Length) - 1);
            fstream.Close();
            name = strings.GetFileNameOnly(filename);
            this.filename = filename;
            this.usermachine = Environment.UserName + "\\" + Environment.MachineName;

        }


    }
}
