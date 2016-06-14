using System;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class Hyperlink : GoText
    {
        #region Fields (3)

        private string _arguments;
        private HyperlinkType _hyperlinkType;
        private string bookmarkName;

        #endregion Fields

        #region Constructors (1)

        public Hyperlink()
        {
            Underline = true;
            TextColor = Color.Blue;
            Text = "Hyperlink";
            HyperlinkType = HyperlinkType.URL;
            AutoResizes = true;
            AutoRescales = true;
        }

        public override bool Underline
        {
            get
            {
                return true;
            }
            set
            {
                base.Underline = true;
            }
        }

        #endregion Constructors

        #region Properties (4)

        public string Arguments
        {
            get { return _arguments; }
            set
            {
                _arguments = value;

                // cater for previously created bookmarks
                if (value.ToLower().Contains(".doc#") || value.ToLower().Contains(".docx#"))
                {
                    char splitchar = '#';
                    string[] args = Arguments.Split(splitchar);
                    _arguments = args[0];
                    BookmarkName = args[1];
                }
            }
        }

        public string BookmarkName
        {
            get { return bookmarkName; }
            set { bookmarkName = value; }
        }

        public HyperlinkType HyperlinkType
        {
            get { return _hyperlinkType; }
            set { _hyperlinkType = value; }
        }

        public bool IsWordDocument
        {
            get { return (Arguments.ToLower().EndsWith(".doc") || Arguments.ToLower().EndsWith(".docx")); }
        }

        #endregion Properties

        #region Methods (2)

        // Public Methods (2) 

        public override string GetToolTip(GoView view)
        {
            return "Ctrl+Click to follow the link";
            //return base.GetToolTip(view);
        }

        #endregion Methods

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

    }

    [Serializable]
    public enum HyperlinkType
    {
        File,
        Diagram,
        EmbeddedObject,
        URL
    }
}