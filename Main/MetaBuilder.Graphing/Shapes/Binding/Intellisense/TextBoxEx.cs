using System.Windows.Forms;

namespace MetaBuilder.Graphing.Shapes.Binding.Intellisense
{
    public class TextBoxEx : TextBox
    {
        public int CurrentColumn
        {
            get { return SelectionStart - GetFirstCharIndexOfCurrentLine() + 1; }
        }

        public int CurrentLine
        {
            get { return GetLineFromCharIndex(SelectionStart) + 1; }
        }

        public void GoTo(int line, int column)
        {
            if (line < 1 || column < 1 || Lines.Length < line)
                return;

            SelectionStart = GetFirstCharIndexFromLine(line - 1) + column - 1;
            SelectionLength = 0;
        }
    }
}