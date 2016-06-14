using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class RefreshCollapsibleNodes : Form
    {

		#region Fields (1) 

        private GoView myView;

		#endregion Fields 

		#region Constructors (1) 

        public RefreshCollapsibleNodes()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Properties (2) 

        public GoView MyView
        {
            get { return myView; }
            set { myView = value; }
        }

        public List<MetaBase> SelectedObjects
        {
            get
            {
                List<MetaBase> retval = new List<MetaBase>();
                foreach (object o in checkedListBox1.CheckedItems)
                {
                    MetaBase mo = o as MetaBase;
                    retval.Add(mo);
                }
                return retval;
            }
        }

		#endregion Properties 

		#region Methods (3) 


		// Public Methods (1) 

        public void BindList(List<MetaBase> items)
        {
            checkedListBox1.BeginUpdate();

            List<MetaBase> itemsToAdd = new List<MetaBase>();
            foreach (MetaBase mb in items)
            {
                if (!(itemsToAdd.Contains(mb)))
                    if (mb.ToString()!=null)
                        itemsToAdd.Add(mb);
            }
           // checkedListBox1.Items.AddRange(itemsToAdd.ToArray());

   
            for (int i = 0; i < itemsToAdd.Count; i++)
            {
                checkedListBox1.Items.Add(itemsToAdd[i], true);
            }
            checkedListBox1.EndUpdate();
            this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
        }



		// Private Methods (2) 

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex>-1)
            {
                myView.Selection.Clear();
                MetaBase mb = checkedListBox1.SelectedItem as MetaBase;
                HighlightMetaBaseNodeInCollection(MyView.Document,mb);
                GoObject obj = myView.Selection.Primary;
                if (obj!=null)
                    myView.ScrollRectangleToVisible(obj.Bounds);
            }
        }

        private void HighlightMetaBaseNodeInCollection(IGoCollection collection, MetaBase mb)
        {
        foreach (GoObject obj in collection)
             {
                 if (obj is IMetaNode)
                 {
                     IMetaNode imnode = obj as IMetaNode;
                     if (imnode.MetaObject == mb)
                     {
                         MyView.Selection.Add(obj);
                     }
                 }
                 if (obj is IGoCollection)
                 {
                     HighlightMetaBaseNodeInCollection(obj as IGoCollection, mb);   
                 }
             }

        }


		#endregion Methods 

    }
}