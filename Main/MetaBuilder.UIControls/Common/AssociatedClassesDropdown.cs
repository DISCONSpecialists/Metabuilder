#region Copyright © 2007 - DISCON Specialists
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: AssociatedClassesDropdown.cs
//
#endregion

using System.ComponentModel;
using System.Data;
using Janus.Windows.GridEX;
using Janus.Windows.GridEX.EditControls;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.Common
{
    /// <summary>
    /// Summary description for AssociatedClassesDropdown.
    /// </summary>
    public class AssociatedClassesDropdown : MultiColumnCombo
    {

		#region Fields (4) 

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private int limittoassociationtype;
        private string limittocaption;
        private string parentclassname;

		#endregion Fields 

		#region Constructors (1) 

        public AssociatedClassesDropdown()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

		#endregion Constructors 

		#region Properties (4) 

        public int LimitToAssociationType
        {
            get { return limittoassociationtype; }
            set { limittoassociationtype = value; }
        }

        public string LimitToCaption
        {
            get { return limittocaption; }
            set { limittocaption = value; }
        }

        public string ParentClassName
        {
            get { return parentclassname; }
            set
            {
                parentclassname = value;
                Init();
            }
        }

        public Association SelectedAssociation
        {
            get
            {
                Association ass = new Association();
                try
                {
                    ass.ID = (int) Value;
                    ass.ParentClass = parentclassname;
                    GridEXSelectedItem selitem = DropDownList.SelectedItems[0];
                    DataRowView drv = (DataRowView) selitem.GetRow().DataRow;
                    ass.ChildClass = drv["ChildClass"].ToString();
                    return ass;
                }
                catch
                {
                    return ass;
                }
            }
        }

		#endregion Properties 

		#region Methods (3) 


		// Public Methods (2) 

        public void Init()
        {
            if (parentclassname != null)
            {
                DataView dv;
                if ((limittocaption != string.Empty) && (limittocaption != null))
                {
                    dv = Singletons.GetClassHelper().GetAllowedAssociateClasses(ParentClassName, limittocaption,true);
                }
                else
                {
                    if (LimitToAssociationType > 0)
                    {
                        dv =
                            Singletons.GetClassHelper().GetAllowedAssociateClasses(ParentClassName,
                                                                                   LimitToAssociationType,true);
                    }
                    else
                    {
                        dv = Singletons.GetClassHelper().GetAllowedAssociateClasses(ParentClassName,true);
                    }
                }
                DropDownList.DataSource = dv;
                DropDownList.RetrieveStructure();
                DropDownList.Columns[0].Visible = false;
                DropDownList.Columns[1].Width = 200;
                DropDownList.Width = 600;
            }
        }

        public void Reset()
        {
            Text = "";
            //this.SelectedText = "";
        }



		// Protected Methods (1) 

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


		#endregion Methods 


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}