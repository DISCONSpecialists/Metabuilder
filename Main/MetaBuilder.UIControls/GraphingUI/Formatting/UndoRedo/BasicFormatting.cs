namespace MetaBuilder.UIControls.GraphingUI.Formatting.FormatUndo
{
    public class BasicFormatting
    {

		#region Fields (10) 

        private bool? autoRescales;
        private bool? deletable;
        private bool? movable;
        private bool? printable;
        private bool? reshapable;
        private bool? resizable;
        private bool? resizesRealtime;
        private bool? selectable;
        private bool? shadowed;
        /*public void SetDefaults(GoObject o)
        {
            if (o == null)
            {
                deletable = true;
                movable = true;
                printable = true;
                resizable = true;
                reshapable = true;
                autoRescales = true;
                visible = true;
            }
        }
        public void ReadBaseProperties(GoObject o)
        {
            deletable = o.Deletable;
            selectable = o.Selectable;
            printable = o.Printable;
            resizable = o.Resizable;
            reshapable = o.Reshapable;
            resizesRealtime = o.ResizesRealtime;
            shadowed = o.Shadowed;
            visible = o.Visible;
        }*/
        private bool? visible;

		#endregion Fields 

		#region Properties (10) 

        public bool? AutoRescales
        {
            get { return autoRescales; }
            set { autoRescales = value; }
        }

        public bool? Deletable
        {
            get { return deletable; }
            set { deletable = value; }
        }

        public bool? Movable
        {
            get { return movable; }
            set { movable = value; }
        }

        public bool? Printable
        {
            get { return printable; }
            set { printable = value; }
        }

        public bool? Reshapable
        {
            get { return reshapable; }
            set { reshapable = value; }
        }

        public bool? Resizable
        {
            get { return resizable; }
            set { resizable = value; }
        }

        public bool? ResizesRealtime
        {
            get { return resizesRealtime; }
            set { resizesRealtime = value; }
        }

        public bool? Selectable
        {
            get { return selectable; }
            set { selectable = value; }
        }

        public bool? Shadowed
        {
            get { return shadowed; }
            set { shadowed = value; }
        }

        public bool? Visible
        {
            get { return visible; }
            set { visible = value; }
        }

		#endregion Properties 

    }
}