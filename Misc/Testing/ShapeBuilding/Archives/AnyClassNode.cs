using System.Collections;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace ShapeBuilding
{
    public class AnyClassNode:GraphNode
    {
        public AnyClassNode()
        {
            GoText txtType = new GoText();
            txtType.Choices = new ArrayList();
            txtType.Choices.Add("Module");
            txtType.Choices.Add("Package");
            txtType.EditorStyle = GoTextEditorStyle.ComboBox;
            txtType.Text = "Module";
            txtType.Editable = true;
            this.EditMode = false;
            Add(txtType);

            GoText txtValue = new GoText();
            txtValue.Text = "New Value";
            Add(txtValue);

        }
    }
}
