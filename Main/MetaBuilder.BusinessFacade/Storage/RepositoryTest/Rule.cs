using MetaBuilder.BusinessLogic;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.BusinessFacade.Storage.RepositoryTemp
{
    public class RepositoryRule
    {

        private int ruleNumber;
        public int RuleNumber
        {
            get { return ruleNumber; }
            set { ruleNumber = value; }
        }

        private string provider;
        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        private VCStatusList currentState;
        public VCStatusList CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        private VCStatusList targetState;
        public VCStatusList TargetState
        {
            get { return targetState; }
            set { targetState = value; }
        }

        //private bool executeOnServer;
        //public bool ExecuteOnServer
        //{
        //    get { return executeOnServer; }
        //    set { executeOnServer = value; }
        //}

        //private bool executeOnClient;
        //public bool ExecuteOnClient
        //{
        //    get { return executeOnClient; }
        //    set { executeOnClient = value; }
        //}

        private bool changeUser;
        public bool ChangeUser
        {
            get { return changeUser; }
            set { changeUser = value; }
        }

        private bool allowAdmin;
        public bool AllowAdmin
        {
            get { return allowAdmin; }
            set { allowAdmin = value; }
        }

        private bool allowWriter;
        public bool AllowWriter
        {
            get { return allowWriter; }
            set { allowWriter = value; }
        }

        private bool allowReader;
        public bool AllowReader
        {
            get { return allowReader; }
            set { allowReader = value; }
        }

        private bool validateUser;

        public bool SameUser
        {
            get { return validateUser; }
            set { validateUser = value; }
        }

        private bool includeEmbeddedItems;
        public bool IncludeEmbeddedItems
        {
            get { return includeEmbeddedItems; }
            set { includeEmbeddedItems = value; }
        }

        private GeneralRepositoryEventHandler command;
        public GeneralRepositoryEventHandler CommandToExecute
        {
            get { return command; }
            set { command = value; }
        }

        private string caption;
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        private VCStatusList serverState;
        public VCStatusList ServerState
        {
            get { return serverState; }
            set { serverState = value; }
        }

        private VCStatusList clientState;
        public VCStatusList ClientState
        {
            get { return clientState; }
            set { clientState = value; }
        }

        private bool mustBeDifferentUser;
        public bool MustBeDifferentUser
        {
            get { return mustBeDifferentUser; }
            set { mustBeDifferentUser = value; }
        }
        //, bool ExecuteServer, bool ExecuteClient
        public RepositoryRule(int ruleNumber, string Provider, VCStatusList State, VCStatusList TargetState, bool admin, bool write, bool read, bool RequireSameUser, bool IncludeEmbeddedItems, GeneralRepositoryEventHandler CommandToExecute, string Caption, VCStatusList serverState, VCStatusList clientState, bool UpdateUser)
        {
            this.provider = Provider;
            this.currentState = State;
            this.targetState = TargetState;
            //this.ExecuteOnServer = ExecuteServer;
            //this.ExecuteOnClient = ExecuteClient;
            this.allowAdmin = admin;
            this.AllowWriter = write;
            this.AllowReader = read;
            this.SameUser = RequireSameUser;
            this.includeEmbeddedItems = IncludeEmbeddedItems;
            this.command = CommandToExecute;
            this.caption = Caption;
            this.serverState = serverState;
            this.clientState = clientState;
            this.ruleNumber = ruleNumber;

            this.ChangeUser = UpdateUser;
        }
        //, ExecuteServer, ExecuteClient
        //, bool ExecuteServer,                              bool ExecuteClient
        public RepositoryRule(int ruleNumber, string Provider, VCStatusList State, VCStatusList TargetState, bool admin, bool write, bool read, bool RequireSameUser, bool IncludeEmbeddedItems, GeneralRepositoryEventHandler CommandToExecute, string Caption, VCStatusList serverState, VCStatusList clientState, bool UpdateUser, bool MustBeDiffUser)
            : this(ruleNumber, Provider, State, TargetState, admin, write, read, RequireSameUser, IncludeEmbeddedItems, CommandToExecute, Caption, serverState, clientState, UpdateUser)
        {

            this.MustBeDifferentUser = MustBeDiffUser;
        }
    }
}