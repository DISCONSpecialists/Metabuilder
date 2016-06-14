using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.MetaControls.Tasks
{
    public abstract class DiagrammingTask : TaskBase
    {

        #region Fields (2)

        private IMetaNode myGoObject;
        private GraphView myView;

        #endregion Fields

        #region Properties (2)

        public IMetaNode MyGoObject
        {
            get { return myGoObject; }
            set { myGoObject = value; }
        }

        public GraphView MyView
        {
            get { return myView; }
            set { myView = value; }
        }

        private string containerID;
        public string ContainerID
        {
            get { return containerID; }
            set { containerID = value; }
        }

        #endregion Properties

    }
}
