using System.Collections.Generic;
using MetaBuilder.BusinessLogic;

namespace MetaBuilder.BusinessFacade.Storage.RepositoryTemp
{
    public class ActionResult
    {
        public ActionResult()
        {
            InnerResults = new List<ActionResult>();
        }
        private string targetState;

        public string TargetState
        {
            get { return targetState; }
            set { targetState = value; }
        }

        private string fromState;
        public string FromState
        {
            get { return fromState; }
            set { fromState = value; }
        }
	
        private string item;

        public string Item
        {
            get { return item; }
            set { item = value; }
        }
	
        public ActionResult(IRepositoryItem item, VCStatusList targetState, string Message, bool Success):base()
        {
            if (item != null)
            {
                this.Item = item.ToString();
                this.fromState = Core.strings.GetEnumDescription(item.State);
                this.targetState = Core.strings.GetEnumDescription(targetState);
            }
            this.message = Message;
            this.Success = Success;
        }
        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public bool intermediate = false;

        private bool success;
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        private List<ActionResult> innerResults;

        public List<ActionResult> InnerResults
        {
            get { return innerResults; }
            set { innerResults = value; }
        }

        private string repository;

        public string Repository
        {
            get { return repository; }
            set { repository = value; }
        }
	
	
    }
}
