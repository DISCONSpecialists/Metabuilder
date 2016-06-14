using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.Help
{
    public class ContextHelp
    {
        public ContextHelp()
        {
            ExcludedWords.Add("the");
            ExcludedWords.Add("and");
            ExcludedWords.Add("this");
            ExcludedWords.Add("that");
            ExcludedWords.Add("when");
            ExcludedWords.Add("then");
        }
        public List<string> ExcludedWords = new List<string>();

        public void loadHelp()
        {
            //this loads the xml file
        }
    }
}