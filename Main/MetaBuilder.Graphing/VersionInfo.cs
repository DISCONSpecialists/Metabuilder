namespace MetaBuilder.Graphing
{
    public class VersionInfo
    {
        #region Fields (5) 

        private string author;
        private string description;
        private string diskLocation;
        private int majorVersion;
        private int minorVersion;

        #endregion Fields 

        #region Properties (5) 

        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string DiskLocation
        {
            get { return diskLocation; }
            set { diskLocation = value; }
        }

        public int MajorVersion
        {
            get { return majorVersion; }
            set { majorVersion = value; }
        }

        public int MinorVersion
        {
            get { return minorVersion; }
            set { minorVersion = value; }
        }

        #endregion Properties 
    }
}