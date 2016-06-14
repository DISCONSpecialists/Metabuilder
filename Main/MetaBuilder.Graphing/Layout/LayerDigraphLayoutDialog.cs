using System;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using Northwoods.Go.Layout;

namespace MetaBuilder.Graphing.Layout
{
    public partial class LayerDigraphLayoutDialog : Form
    {
        #region Fields (30)

        private readonly GraphView myView;
        private CheckBox aggressive;
        private Button cancelButton;
        private TextBox column;
        private RadioButton depthFirst;
        private RadioButton dfsin;
        private RadioButton dfsout;
        private RadioButton down;
        private RadioButton greedy;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private TextBox iter;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox layer;
        private RadioButton left;
        private RadioButton length;
        private SimpleLDAL myAL;
        private RadioButton naive;
        private Button okButton;
        private RadioButton right;
        private RadioButton sink;
        private RadioButton source;
        private RadioButton up;

        #endregion Fields

        #region Constructors (1)

        public LayerDigraphLayoutDialog(GraphView view)
        {
            myView = view;
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (1)

        public GoLayout GoLayout
        {
            get { return myAL; }
        }

        #endregion Properties

        #region Methods (2)

        // Public Methods (2) 

        public void okButton_Click(object sender, EventArgs e)
        {
            UpdateControl();
            DialogResult = DialogResult.OK;
        }

        public void UpdateControl()
        {
            if (myView != null)
            {
                BaseDocument doc = myView.Doc;
                GoLayoutLayeredDigraphCycleRemove cycle;
                GoLayoutLayeredDigraphLayering layering;
                GoLayoutLayeredDigraphInitIndices initialize;
                GoLayoutDirection direction;
                GoLayoutLayeredDigraphAggressive agr;
                if (greedy.Checked)
                    cycle = GoLayoutLayeredDigraphCycleRemove.Greedy;
                else
                    cycle = GoLayoutLayeredDigraphCycleRemove.DepthFirst;
                if (sink.Checked)
                    layering = GoLayoutLayeredDigraphLayering.LongestPathSink;
                else if (source.Checked)
                    layering = GoLayoutLayeredDigraphLayering.LongestPathSource;
                else
                    layering = GoLayoutLayeredDigraphLayering.OptimalLinkLength;
                if (naive.Checked)
                    initialize = GoLayoutLayeredDigraphInitIndices.Naive;
                else if (dfsout.Checked)
                    initialize = GoLayoutLayeredDigraphInitIndices.DepthFirstOut;
                else
                    initialize = GoLayoutLayeredDigraphInitIndices.DepthFirstIn;
                if (up.Checked)
                    direction = GoLayoutDirection.Up;
                else if (down.Checked)
                    direction = GoLayoutDirection.Down;
                else if (left.Checked)
                    direction = GoLayoutDirection.Left;
                else
                    direction = GoLayoutDirection.Right;
                if (aggressive.Checked)
                    agr = GoLayoutLayeredDigraphAggressive.More;
                else
                    agr = GoLayoutLayeredDigraphAggressive.Less;
                myAL = new SimpleLDAL();
                myAL.Document = doc;
                myAL.LayerSpacing = int.Parse(layer.Text);
                myAL.ColumnSpacing = int.Parse(column.Text);
                myAL.DirectionOption = direction;
                myAL.CycleRemoveOption = cycle;
                myAL.LayeringOption = layering;
                myAL.InitializeOption = initialize;
                myAL.Iterations = int.Parse(iter.Text);
                myAL.AggressiveOption = agr;
            }
        }

        #endregion Methods
    }
}