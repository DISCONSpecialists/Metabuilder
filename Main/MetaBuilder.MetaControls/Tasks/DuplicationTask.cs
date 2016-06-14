using System.Collections.Generic;
using MetaBuilder.Meta;

namespace MetaBuilder.MetaControls.Tasks
{
    public class DuplicationTask : DiagrammingTask
    {

        #region Fields (1)

        private List<MetaBase> matches;

        #endregion Fields

        #region Constructors (1)

        public DuplicationTask()
            : base()
        {
        }

        #endregion Constructors

        #region Properties (1)

        public List<MetaBase> Matches
        {
            get { return matches; }
            set { matches = value; }
        }

        public override string Caption
        {
            get
            {
                return "Possible Duplication: " + Tag.ToString();
            }
            set
            {
                base.Caption = value;
            }
        }

        public override string Description
        {
            get
            {
                return "A possible duplicate for this object may already exist. Double click the task to start the merge/create process.";
            }
            set
            {
                base.Description = value;
            }
        }

        #endregion Properties

    }
}
