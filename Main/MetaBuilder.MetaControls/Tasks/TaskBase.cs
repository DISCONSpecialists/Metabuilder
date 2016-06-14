using System;

namespace MetaBuilder.MetaControls.Tasks
{
    public abstract class TaskBase
    {

		#region Fields (6) 

        private string caption;
        private string description;
        private Guid guid;
        private bool isComplete;
        private bool isCritical;
        private object tag;

		#endregion Fields 

		#region Constructors (1) 

        public TaskBase()
        {
            guid = Guid.NewGuid();
        }

		#endregion Constructors 

		#region Properties (6) 

        public virtual string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public virtual string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public bool IsComplete
        {
            get { return isComplete; }
            set { isComplete = true; }
        }

        public bool IsCritical
        {
            get { return isCritical; }
            set { isCritical = value; }
        }

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

		#endregion Properties 

		#region Delegates and Events (1) 


		// Events (1) 

        public event EventHandler Completed;


		#endregion Delegates and Events 

		#region Methods (1) 


		// Protected Methods (1) 

        protected void OnCompleted()
        {
            if (Completed != null)
            {
                Completed(this, EventArgs.Empty);
            }
        }


		#endregion Methods 

    }
}
