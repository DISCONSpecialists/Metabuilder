using System.Windows.Forms;
using MetaBuilder.Docking;
using MetaBuilder.Graphing.Containers;
using Northwoods.Go;
using MetaBuilder.UIControls.GraphingUI.Tools.Help;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class HelpDocker : DockContent
    {
        public HelpDocker()
        {
            InitializeComponent();
        }
        private ContextHelp MyHelp;

        //For F1 proliferation from docking form
        private object selectedObject = null;
        public object SelectedObject
        {
            get { return selectedObject; }
            set
            {
                if (value == selectedObject) return; //skip repopulation of same object

                selectedObject = value;
                populateTopics(selectedObject != null);
            }
        }
        private void HelpDocker_Load(object sender, System.EventArgs e)
        {
            if (!Core.Variables.Instance.ShowDeveloperItems)
                Close();

            CloseButtonVisible = false;
            MyHelp = new ContextHelp();
            //axAcroPDF1.LoadFile(FILENAME);
            populateTopics(false);
        }
        private void populateTopics(bool selected)
        {
            if (selected)
            {
                //get all help topics which pertain to this selected object (this is phase 2)
                return;
            }
            //not selected
            //load the comobobox with every single topic in the contents
        }
        private void populateTopics(string text)
        {
            //remove excludedWords
            foreach (string s in MyHelp.ExcludedWords)
                text.Replace(s, "");
        }
        private void textFilter_TextChanged(object sender, System.EventArgs e)
        {
            //filter comobotopics
            if (textFilter.Text.Length > 2)
                populateTopics(textFilter.Text);
            else
                populateTopics(selectedObject != null);
        }
        private void comboTopics_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                int pageNumber = int.Parse(comboTopics.SelectedValue.ToString());
                //get page of dictionary item and go to it
                //axAcroPDF1.setCurrentPage(pageNumber);
            }
            catch
            {

            }
        }
    }
}