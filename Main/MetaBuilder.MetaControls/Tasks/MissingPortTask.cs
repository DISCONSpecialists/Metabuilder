using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.MetaControls.Tasks
{
    public class MissingPortTask : DiagrammingTask
    {
        public MissingPortTask()
            : base()
        {
        }

        public override string Caption
        {
            get
            {
                return "Missing Port: " + base.Caption;
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
                return "The link has one or more ports which could not be found. Double click to select the link.";
            }
            set
            {
                base.Description = value;
            }
        }

    }

    public class NotInModelLinkTask : DiagrammingTask
    {
        public NotInModelLinkTask()
            : base()
        {
        }

        public override string Caption
        {
            get
            {
                return "Invalid Link: " + base.Caption;
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
                return "This link is not supported by the metamodel. It will not be saved.";
            }
            set
            {
                base.Description = value;
            }
        }

    }

}
