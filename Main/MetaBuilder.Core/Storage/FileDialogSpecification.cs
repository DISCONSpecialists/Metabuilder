using System;
using MetaBuilder.BusinessLogic;
using b = MetaBuilder.BusinessLogic;
namespace MetaBuilder.Core.Storage
{
    [Serializable]
    public class FileDialogSpecification
    {
        private string filter;
        private string title;
        private string defaultpath;
        private string extension;

        public string Filter
        {
            get
            {
                return filter;
            }
        }
        public string Extension
        {
            get
            {
                return extension;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
        }

        public string DefaultPath
        {
            get { return defaultpath; }
            set { defaultpath = value; }
        }

        private FileTypeList fileType;
        public FileTypeList FileType
        {
            get
            {
                return fileType;
            }
        }

        internal FileDialogSpecification(FileTypeList fType)
        {
            fileType = fType;
            this.title = fType.ToString();
            switch (this.fileType)
            {
                case FileTypeList.Diagram:
                    extension = "mdgm";
                    filter = "MetaBuilder Diagram (*." + extension + ")|*." + extension + ";*.dgm";
                    defaultpath = Variables.Instance.DiagramPath;
                    break;
                case FileTypeList.Symbol:
                    extension = "sym";
                    filter = "MetaBuilder Symbol (*." + extension + ")|*." + extension;
                    defaultpath = Variables.Instance.SymbolPath;
                    break;
                case FileTypeList.SymbolStore:
                    extension = "stn";
                    filter = "MetaBuilder Stencil (*." + extension + ")|*." + extension;
                    defaultpath = Variables.Instance.StencilPath;
                    break;
                case FileTypeList.Image:
                    defaultpath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    filter = "Supported Image Types (jpg,gif,bmp,png)|*.jpg;*.bmp;*.gif;*.png";
                    defaultpath = Variables.Instance.ImagesPath;
                    break;
                case FileTypeList.ArrowHead:
                    extension = "arh";
                    filter = "MetaBuilder Arrowhead (*." + extension + ")|*." + extension;
                    defaultpath = Variables.Instance.StencilPath;
                    break;
                case FileTypeList.SavedContextView:
                    extension = "vicxml";
                    filter = "Saved View In Context (*." + extension + ")|*." + extension;
                    defaultpath = Variables.Instance.DiagramPath;
                    break;
                case FileTypeList.MindMap:
                    extension = "mmdgm";
                    filter = "Meta Map (*." + extension + ")|*." + extension;
                    defaultpath = Variables.Instance.DiagramPath;
                    break;
                case FileTypeList.Other:
                    extension = "";
                    filter = "";
                    defaultpath = System.Windows.Forms.Application.StartupPath;
                    break;
            }
        }
    }
}