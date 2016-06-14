// Example header text. Can be configured in the options.
using System;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Helpers;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Containers
{
    [Serializable]
    public class MetaUndoManager : GoUndoManager
    {
        public MetaUndoManager()
        {
#if DEBUG
            ChecksTransactionLevel = true;
#endif
            //MaximumEditCount = 100;
        }

        public override bool SkipEvent(GoChangedEventArgs evt)
        {
            if (evt.GoObject is FrameLayerGroup || evt.GoObject is FrameLayerRect)
                return true;
            //ChangedVisible 1003 
            //ChangedSelectable 1004 
            //ChangedMovable 1005 
            //ChangedCopyable 1006 
            //ChangedResizable 1007 
            //ChangedReshapable 1008 
            //ChangedDeletable 1009 
            //ChangedEditable 1010 
            //ChangedAutoRescales 1011 
            //ChangedResizeRealtime 1012 
            //ChangedShadowed 1013 
            //ChangedAddedObserver 1014 
            //ChangedRemovedObserver 1015 
            //ChangedDragsNode 1016 
            //ChangedPrintable 1017 
            if (evt.Hint == 201)
                return true;
            if (evt.SubHint - 1000 > 2 && evt.SubHint - 1000 < 18)
                return true;
            return base.SkipEvent(evt);
        }

        public override bool StartTransaction()
        {
            return base.StartTransaction();
        }
        public override bool EndTransaction(bool commit, string tname, string pname)
        {
            return base.EndTransaction(commit, tname, pname);
        }

        public override GoUndoManagerCompoundEdit CommitCompoundEdit(GoUndoManagerCompoundEdit cedit)
        {
            return base.CommitCompoundEdit(cedit);
        }

        public override void Undo()
        {
            base.Undo();
        }
        public override void Redo()
        {
            base.Redo();
        }
    }

    [Serializable]
    public abstract class BaseDocument : GoDocument
    {
        #region Fields (4)
        public bool TestedForILinkContainers;
        public bool debugme;
        [NonSerialized]
        private FileTypeList fileType;

        #endregion Fields

        #region Properties (3)

        [NonSerialized]
        public bool? ContainsILinkContainers; //IMPLICATION  true/false?

        public SerializableDictionary<string, QuickImage> EmbeddedImages;

        public FileTypeList FileType
        {
            get
            {
                return this.fileType;
            }
            set
            {
                if (this.fileType != value)
                {
                    GoChangedEventArgs evtArgs = new GoChangedEventArgs();
                    evtArgs.Hint = 1998; // FileType
                    //OnChanged(evtArgs);
                }
                this.fileType = value;
            }
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        public virtual void Initialise()
        {
            Initializing = true;
            this.RoutingTime = GoRoutingTime.Immediate;
            this.MaintainsPartID = true;
            if (this.Name == null)
            {
                this.Name = string.Format("New {0}", this.FileType.ToString());
            }
            this.UndoManager = new MetaUndoManager();
            if (this.EmbeddedImages == null)
            {
                this.EmbeddedImages = new SerializableDictionary<string, QuickImage>();
            }
            Initializing = false;
        }
        #endregion Methods

    }
}