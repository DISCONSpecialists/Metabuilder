using System.Windows.Forms;
using MetaBuilder.Meta;
using MetaBuilder.Docking;
using System.ComponentModel;
using MetaBuilder.Graphing.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MetaBuilder.MetaControls
{
    public partial class MetaPropertyGridDocker : DockContent
    {

        #region Constructors (1)

        public MetaPropertyGridDocker()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (1)

        public object SelectedObject
        {
            get { return metaPropertyGrid1.SelectedObject; }
            set
            {
                if (value == null)
                {
                    UnselectableObject uso = new UnselectableObject();
                    metaPropertyGrid1.SelectedObject = uso;
                }
                else if (value is MetaBase)
                {
                    metaPropertyGrid1.SelectedObject = value;
                    metaPropertyGrid1.Enabled = true;

                    MetaBase m = value as MetaBase;
                    metaPropertyGrid1.Enabled = VCStatusTool.UserHasControl(m);
                }
                else if (value is MetaBuilder.Graphing.Containers.DocumentInfo)
                {
                    metaPropertyGrid1.SelectedObject = value;
                    metaPropertyGrid1.Enabled = true;
                }
                else if (value is QLink)
                {
                    QlinkObject q = new QlinkObject(value as QLink);
                    metaPropertyGrid1.SelectedObject = q;
                }
            }
        }

        private Collection<MetaBase> selectedMetaBases;
        public Collection<MetaBase> SelectedMetaBases
        {
            get { return selectedMetaBases; }
            set
            {
                selectedMetaBases = value;
                metaPropertyGrid1.SelectedMetaBases = selectedMetaBases;
            }
        }

        #endregion Properties

        #region Nested Classes (2)

        public class UnselectableObject
        {

            [Description("Property Description")]
            [CategoryAttribute("General")]
            public string Property
            {
                get { return "Select a shape on the diagram to edit the properties"; }
            }

        }

        public class QlinkObject
        {
            public QLink LinkReference;

            public QlinkObject(QLink link)
            {
                if (link == null)
                    return;
                if (link.FromNode != null && link.FromNode is IMetaNode)
                    Parent = (link.FromNode as IMetaNode).MetaObject.ToString() + " [" + (link.FromNode as IMetaNode).MetaObject.Class + "]";
                else
                    Parent = "NULL";

                if (link.ToNode != null && link.ToNode is IMetaNode)
                    Child = (link.ToNode as IMetaNode).MetaObject.ToString() + " [" + (link.ToNode as IMetaNode).MetaObject.Class + "]";
                else
                    Child = "NULL";

                ArtefactCount = link.GetArtefacts().Count;
                AssociationType = link.AssociationType.ToString();
                gapType = link.GapType;

                LinkReference = link;
            }

            private string from;
            [DisplayName("Parent Node")]
            [Description("Parent Node")]
            [CategoryAttribute("System Properties")]
            [ReadOnly(true)]
            public string Parent
            {
                get { return from; }
                set { from = value; }
            }

            private string to;
            [DisplayName("Child Node")]
            [Description("Child Node")]
            [CategoryAttribute("System Properties")]
            [ReadOnly(true)]
            public string Child
            {
                get { return to; }
                set { to = value; }
            }

            private string associationType;
            [DisplayName("Association Type")]
            [Description("Type of Association")]
            [CategoryAttribute("System Properties")]
            [ReadOnly(true)]
            public string AssociationType
            {
                get { return associationType; }
                set { associationType = value; }
            }

            private GapType gapType;
            [DisplayName("Gap Type")]
            [Description("Gap Between Shapes")]
            [CategoryAttribute("General")]
            //[ReadOnly(false)]
            public GapType GapType
            {
                get { return gapType; }
                set
                {
                    gapType = value;
                    LinkReference.GapType = value;
                }
            }

            private int artefactCount;
            [DisplayName("Artefact Count")]
            [Description("Number of artefacts currently connected to this link")]
            [CategoryAttribute("Other")]
            [ReadOnly(true)]
            public int ArtefactCount
            {
                get { return artefactCount; }
                set { artefactCount = value; }
            }

        }

        #endregion Nested Classes

    }
}