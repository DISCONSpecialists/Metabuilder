#region Copyright © 2007 - DISCON Specialists
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ClassDropdown.cs
//
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using System.Collections.ObjectModel;

namespace MetaBuilder.UIControls.Common
{
    /// <summary>
    /// Summary description for ClassDropdown.
    /// </summary>
    public class ClassDropdown : ComboBox
    {

        #region Fields (2)

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private bool initializing;

        #endregion Fields

        #region Constructors (1)

        public ClassDropdown()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            //this.DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;
        }

        #endregion Constructors

        #region Properties (3)

        //public Collection<string> AvailableClasses
        //{
        //    get
        //    {
        //        Collection<string> retval = new Collection<string>();
        //        foreach (object item in Items)
        //        {
        //            retval.Add(item.ToString());
        //        }
        //        return retval;
        //    }
        //}

        public bool Initializing
        {
            get { return initializing; }
            set { initializing = value; }
        }

        public string SelectedClass
        {
            get { return Text; }
            set { Text = value; }
        }

        #endregion Properties

        #region Methods (3)

        // Public Methods (2) 
        public virtual void Init(string allowedClasses)
        {
            try
            {
                DropDownStyle = ComboBoxStyle.DropDownList;
                Initializing = true;
                DataView dv = Singletons.GetClassHelper().GetClasses(true);
                Items.Clear();
                foreach (DataRowView drv in dv)
                {
                    if (allowedClasses.Contains(drv["Name"].ToString()))
                        Items.Add(drv["Name"].ToString());
                }
                Text = Items[0].ToString();
                Initializing = false;
            }
            catch (Exception x)
            {
                Debug.WriteLine("Database Error:" + x.ToString());
            }
        }
        public virtual void Init()
        {
            try
            {
                DropDownStyle = ComboBoxStyle.DropDownList;
                Initializing = true;
                DataView dv = Singletons.GetClassHelper().GetClasses(true);
                Items.Clear();
                foreach (DataRowView drv in dv)
                {
                    Items.Add(drv["Name"].ToString());
                }
                Text = Items[0].ToString();
                Initializing = false;
            }
            catch (Exception x)
            {
                Debug.WriteLine("Database Error:" + x.ToString());
            }
        }
        public void RemoveClass(string itemToRemove)
        {
            Collection<object> toRemove = new Collection<object>();
            foreach (object item in Items)
            {
                if (item.ToString() == itemToRemove)
                {
                    toRemove.Add(item);
                }
            }
            for (int i = 0; i < toRemove.Count; i++)
            {
                Items.Remove(toRemove[i]);
            }
            if (Items.Count > 0)
            {
                SelectedIndex = 0;
            }
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