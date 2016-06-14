using System;
using System.Windows.Forms;
using MetaBuilder.Meta;

namespace MetaBuilder.MetaControls
{
    public partial class ObjectPropertiesWindow : Form
    {

        #region Constructors (1)

        private bool Server;
        public ObjectPropertiesWindow(bool server)
        {
            Server = server;
            InitializeComponent();
            objectProperties1.ViewInContext += new ViewInContextEventHandler(objectProperties1_ViewInContext);
            objectProperties1.OpenDiagram += new EventHandler(objectProperties1_OpenDiagram);
        }

        #endregion Constructors

        #region Properties (1)

        public MetaBase SelectedObject
        {
            get { return objectProperties1.MyMetaObject; }
            set
            {
                objectProperties1.MyMetaObject = value;
                Text = value.ToString();
            }
        }

        #endregion Properties

        #region Delegates and Events (2)

        // Events (2) 

        public event EventHandler OpenDiagram;

        public event ViewInContextEventHandler ViewInContext;

        #endregion Delegates and Events

        #region Methods (4)

        // Protected Methods (2) 

        protected void OnOpenDiagram(object sender)
        {
            if (OpenDiagram != null)
            {
                OpenDiagram(sender, EventArgs.Empty);
            }
        }

        protected void OnViewInContext(MetaBase mbase)
        {
            if (ViewInContext != null)
                ViewInContext(mbase);
        }

        // Private Methods (2) 

        void objectProperties1_OpenDiagram(object sender, EventArgs e)
        {
            OnOpenDiagram(sender);
        }

        void objectProperties1_ViewInContext(MetaBase mbase)
        {
            OnViewInContext(mbase);
        }

        #endregion Methods

    }
}