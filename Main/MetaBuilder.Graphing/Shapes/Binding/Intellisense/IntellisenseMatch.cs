namespace MetaBuilder.Graphing.Shapes.Binding.Intellisense
{
    public class IntellisenseMatch
    {
        #region Fields (3)

        private string machineID;
        private int pkID;
        private string stringValue;
        private string workSpaceName;

        #endregion Fields

        #region Properties (3)

        public string _Class;

        public string MachineID
        {
            get { return machineID; }
            set { machineID = value; }
        }

        public int PKid
        {
            get { return pkID; }
            set { pkID = value; }
        }

        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

        public string WorkSpaceName
        {
            get { return workSpaceName; }
            set { workSpaceName = value; }
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        public override string ToString()
        {
            return StringValue; // return base.ToString();
        }

        public string DisplayValue
        {
            get
            {
#if DEBUG
                return StringValue + ((PKid > 0) ? " {" + PKid.ToString() + "-" + MachineID + "}" : "");
#endif
                return StringValue;
            }
        }


        #endregion Methods
    }
}