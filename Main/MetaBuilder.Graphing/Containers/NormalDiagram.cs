using System;
using System.Drawing;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Graphing.Formatting;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Containers
{

    /// <summary>
    /// A diagram with a frame around it
    /// </summary>
    [Serializable]
    public class NormalDiagram : BaseDocument
    {
        private string diagramType;
        public string DiagramType
        {
            get { return diagramType; }
            set { diagramType = value; }
        }

        /*
        public bool SaveToRepository(string filename)
        {
            GraphFileManager gfmanager = new GraphFileManager();
            GraphFile file = gfmanager.ConvertToGraphFile(this);
            file.Name = strings.GetFileNameOnly(file.Name);
            //d.DataRepository.ConnectionProvider connectionProvider = new MetaBuilder.DataAccessLayer.DataRepository.ConnectionProvider(
            DataRepository.AddConnection("DynamicConnectionString", Variables.Instance.ServerConnectionString);
            DataRepository.Connections["DynamicConnectionString"].Provider.GraphFileProvider.Save(file);
            VersionManager.CurrentVersion.PKID = file.pkid;
            return true;
        }*/

        [NonSerialized]
        private bool updateSize;
        public bool UpdateSize
        {
            get { return updateSize; }
            set { updateSize = value; }
        }

        public override void Add(GoObject obj)
        {
            try
            {
                if (!(base.Contains(obj)))
                    base.Add(obj);
            }
            catch
            {
            }
        }

        #region Frame layer

        public void RepositionFrame(GoView myview)
        {
            if (SheetPosition.HasValue && myview.Sheet != null)
            {
                if (lastViewPoint.HasValue && DocScale.HasValue)
                {
                    myview.DocPosition = LastViewPoint.Value;
                    // = LastViewPoint;//.ScrollRectangleToVisible(new RectangleF(lastViewPoint.Value, new SizeF(2, 2)));
                    myview.DocScale = DocScale.Value;
                }
                DocumentFrame.Position = new PointF(SheetPosition.Value.X + myview.Sheet.TopLeftMargin.Width, SheetPosition.Value.Y + myview.Sheet.TopLeftMargin.Height);
                myview.Sheet.Size = SheetSize.Value;
                myview.Sheet.Position = SheetPosition.Value;
                if (LastVisibleRectangle.HasValue)
                    myview.ScrollRectangleToVisible(LastVisibleRectangle.Value);

                if (FrameSize.HasValue)
                    DocumentFrame.Size = FrameSize.Value;
            }
            DocumentFrame.Reposition();
        }

        public void InitVersionManager()
        {
            versionManager = new DocumentVersionManager(FileType);
            if (FileType == FileTypeList.Diagram)
                DocumentFrame.Update(Name, VersionManager.Versions);
            if (VersionManager.Versions != null)
            {
                if (VersionManager.Versions.Count == 0)
                    DocumentFrame.SetDefaults();
            }
            else
            {
                DocumentFrame.SetDefaults();
            }
        }

        #endregion

        #region Fields (12)
        public FrameLayerGroup DocumentFrame;
        private GoLayer _frameLayer;

        /*public void Update()
        {
            GraphFileManager gfmanager = new GraphFileManager();
            GraphFile file = gfmanager.ConvertToGraphFile(this);
            DataRepository.GraphFileProvider.Update(file);
        }*/
        [NonSerialized]
        private float? docScale;
        [NonSerialized]
        private string filemachine;
        [NonSerialized]
        private SizeF? frameSize;
        /*
                private System.Drawing.Printing.PrinterSettings printerSettings;
                public System.Drawing.Printing.PrinterSettings PrinterSettings
                {
                    get { return printerSettings; }
                    set { printerSettings = value; }
                }*/
        [NonSerialized]
        private DiagramPrintSettings lastPrintSettings;
        [NonSerialized]
        private PointF? lastViewPoint;
        [NonSerialized]
        private RectangleF? lastVisibleRectangle;
        [NonSerialized]
        private PointF? sheetPosition;
        [NonSerialized]
        private SizeF? sheetSize;
        [NonSerialized]
        private DocumentVersionManager versionManager;
        [NonSerialized]
        private Rectangle? docExtent;

        #endregion Fields

        #region Constructors (1)
        public NormalDiagram()
        {
            Initialise();
        }
        #endregion Constructors

        private bool wasNumbered;
        public bool WasNumbered
        {
            get { return wasNumbered; }
            set { wasNumbered = value; }
        }

        private bool artefactPointersVisible;
        public bool ArtefactPointersVisible
        {
            get { return artefactPointersVisible; }
            set { artefactPointersVisible = value; }
        }

        private bool showObjectImages = true;
        public bool ShowObjectImages
        {
            get { return showObjectImages; }
            set { showObjectImages = value; }
        }

        #region Properties (11)

        public Rectangle? DocExtent
        {
            get { return docExtent; }
            set { docExtent = value; }
        }

        public float? DocScale
        {
            get { return docScale; }
            set { docScale = value; }
        }

        public string FileMachine
        {
            get { return filemachine; }
            set { filemachine = value; }
        }

        public GoLayer FrameLayer
        {
            get { return _frameLayer; }
            set { _frameLayer = value; }
        }

        public SizeF? FrameSize
        {
            get { return frameSize; }
            set { frameSize = value; }
        }

        public DiagramPrintSettings LastPrintSettings
        {
            get { return lastPrintSettings; }
            set { lastPrintSettings = value; }
        }

        public PointF? LastViewPoint
        {
            get { return lastViewPoint; }
            set { lastViewPoint = value; }
        }

        public RectangleF? LastVisibleRectangle
        {
            get { return lastVisibleRectangle; }
            set { lastVisibleRectangle = value; }
        }

        public PointF? SheetPosition
        {
            get { return sheetPosition; }
            set { sheetPosition = value; }
        }

        public SizeF? SheetSize
        {
            get { return sheetSize; }
            set { sheetSize = value; }
        }

        public DocumentVersionManager VersionManager
        {
            get
            {
                // returns a new versionmanager if it's a new document
                if (versionManager == null)
                    versionManager = new DocumentVersionManager(FileType);
                return versionManager;
            }
            set { versionManager = value; }
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        /// <summary>
        /// Create and fit a framelayer
        /// </summary>
        /// <param name="myview"></param>
        public void CreateFrameLayer(GoView myview)
        {
            DocumentFrame = new FrameLayerGroup();
            FrameLayer = Layers.CreateNewLayerBefore(null);
            DocumentFrame.Initializing = true;

            if (myview != null)
            {
                RectangleF rf = new RectangleF();

                //11 YEARS AGO!>?
                //Go Version 4.2 supported .NET 2.0 (and 3.5) in one set of DLLs and .NET 4.0 in another.
                //With version 5.0, the support for .NET 2.0 will be gone, we will be supporting .NET 3.5, 4.0 and 4.5.  

                //  For Windows 8, MS supports .NET 3.5 and up.
                Version win8version = new Version(6, 2, 9200, 0);
                if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= win8version)
                {
                    //Windows 8+
                    Core.Log.WriteLog("CreateFrameLayer(win8version)" + Environment.NewLine + "PageWidth " + Variables.Instance.PageWidth + " ==> " + Convert.ToSingle(Variables.Instance.PageWidth).ToString() + Environment.NewLine + "PageWidth " + Variables.Instance.PageHeight + " ==> " + Convert.ToSingle(Variables.Instance.PageHeight).ToString());
                    myview.Sheet.Size = new SizeF(Convert.ToSingle(Variables.Instance.PageWidth), Convert.ToSingle(Variables.Instance.PageHeight));
                    rf.Width = myview.Sheet.Size.Width - myview.Sheet.TopLeftMargin.Width - myview.Sheet.BottomRightMargin.Width;
                    rf.Height = myview.Sheet.Size.Height - myview.Sheet.TopLeftMargin.Height - myview.Sheet.BottomRightMargin.Height;
                    Core.Log.WriteLog("CreateFrameLayer(win8version) width " + rf.Width.ToString() + " - height " + rf.Height.ToString());
                }
                else
                {
                    myview.Sheet.Size = new SizeF(Convert.ToSingle(Variables.Instance.PageWidth), Convert.ToSingle(Variables.Instance.PageHeight));
                    //myview.Sheet.Size = new SizeF(float.Parse((Variables.Instance.PageWidth * Convert.ToDecimal(3.12)).ToString()), float.Parse((Variables.Instance.PageHeight * Convert.ToDecimal(3.12)).ToString())); //removed invariant culture
                    rf.Width = myview.Sheet.Size.Width - myview.Sheet.TopLeftMargin.Width - myview.Sheet.BottomRightMargin.Width;
                    rf.Height = myview.Sheet.Size.Height - myview.Sheet.TopLeftMargin.Height - myview.Sheet.BottomRightMargin.Height;
                }

                if (SheetPosition.HasValue)
                    myview.DocPosition = SheetPosition.Value;
                rf.Location = new PointF(myview.DocPosition.X + myview.Sheet.TopLeftMargin.Width, myview.DocPosition.Y + myview.Sheet.TopLeftMargin.Height);
                DocumentFrame.Setup(rf);
            }
            FrameLayer.Add(DocumentFrame);

            DocumentFrame.Initializing = false;
            DocumentFrame.Copyable = false;
            FrameLayer.AllowCopy = false;
            FrameLayer.AllowSelect = false;
            FrameLayer.AllowDelete = false;
            Layers.MoveAfter(DefaultLayer, LinksLayer);
            InitVersionManager();
        }

        public void SizeFrameLayer(RectangleF rf)
        {
            DocumentFrame.Frame.Bounds = rf;
        }

        #endregion Methods

        public GoDocument CopyAsShallow()
        {
            foreach (GoObject o in this)
            {
                if (o is IShallowCopyable)
                {
                    (o as IShallowCopyable).CopyAsShadow = true;
                }
            }
            NormalDiagram doc = base.Copy() as NormalDiagram;
            foreach (GoObject o in this)
            {
                if (o is IShallowCopyable)
                {
                    (o as IShallowCopyable).CopyAsShadow = false;
                }
            }

            return doc;
        }

        public override GoDocument Copy()
        {
            return base.Copy();
        }

    }
}