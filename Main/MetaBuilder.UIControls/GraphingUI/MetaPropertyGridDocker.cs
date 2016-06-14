using System.ComponentModel;
using MetaBuilder.Docking;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.GraphingUI
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
            }
        }

        #endregion Properties

        #region Nested Classes (1)

        public class UnselectableObject
        {

            #region Properties (1)

            [Description("Property Description")]
            [CategoryAttribute("General")]
            public string Property
            {
                get { return "Select a shape on the diagram to edit the properties"; }
            }

            #endregion Properties

        }
        #endregion Nested Classes

    }
}