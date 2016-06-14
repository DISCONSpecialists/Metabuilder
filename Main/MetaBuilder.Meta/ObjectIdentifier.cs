namespace MetaBuilder.Meta
{
    public class ObjectIDentifier
    {
        public ObjectIDentifier()
        {
        }
        public ObjectIDentifier(int pk, string mach)
        {
            pkid = pk;
            machine = mach;
        }

        private int pkid;

        public int PKID
        {
            get { return pkid; }
            set { pkid = value; }
        }
	
        private string machine;

        public string Machine
        {
            get { return machine; }
            set { machine = value; }
        }
	
    }
}
