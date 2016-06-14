using System;
using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using b = MetaBuilder.BusinessLogic;
using System.Collections.ObjectModel;

namespace MetaBuilder.Core.Storage
{
    /// <summary>
    /// This class should be used via the Instance (singleton) property
    /// </summary>
    public class FilePathManager
    {
        private FilePathManager()
        {
            sessionPaths = new Dictionary<string, FileDialogSpecification>();
            lastOpenPath = GetSpecification(FileTypeList.Diagram).DefaultPath;
        }
        private static string lastOpenPath;
        private Dictionary<string, FileDialogSpecification> sessionPaths;
        private static FilePathManager manager;
        public static FilePathManager Instance
        {
            get
            {
                if (manager == null)
                    manager = new FilePathManager();
                return manager;
            }
            set
            {
                manager = value;
            }
        }

        public string GetLastUsedPath()
        {
            return lastOpenPath;
        }
        public void SetLastUsedPath(FileTypeList fileType, string path)
        {
            FileDialogSpecification spec = GetSpecification(fileType);
            spec.DefaultPath = Core.strings.GetPath(path);
        }

        public void SetLastOpenPath(string filename)
        {
            lastOpenPath = strings.GetPath(filename);
        }
        public FileDialogSpecification GetSpecification(FileTypeList fileType)
        {
            FileDialogSpecification retval = new FileDialogSpecification(fileType);
            if (!sessionPaths.ContainsKey(fileType.ToString()))
                sessionPaths.Add(fileType.ToString(), retval);
            retval.DefaultPath = sessionPaths[fileType.ToString()].DefaultPath;
            return sessionPaths[fileType.ToString()];
        }

        public Collection<FileDialogSpecification> GetList()
        {
            Collection<FileDialogSpecification> retval = new Collection<FileDialogSpecification>();
            Array fileTypes = Enum.GetValues(typeof(FileTypeList));
            for (int i = 0; i < fileTypes.Length; i++)
            {
                FileTypeList ftype = (FileTypeList)fileTypes.GetValue(i);
                retval.Add(new FileDialogSpecification(ftype));
            }
            return retval;
        }

    }
}