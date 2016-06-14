#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: UniversalDropdownEditor.cs
//

#endregion

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace MetaBuilder.Meta.Editors
{
    public class FileCollectionEditor : CollectionEditor
    {
        public FileCollectionEditor(Type type)
            : base(type)
        {
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.Modal;
            }
            return UITypeEditorEditStyle.None;
        }
    
        

        protected override CollectionForm CreateCollectionForm()
        {
            return base.CreateCollectionForm();
        }

        protected override object CreateInstance(Type itemType)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.ShowDialog();
            if (ofd.FileName != null)
            {
                Attachment attachment = new Attachment();
                attachment.Load(ofd.FileName);
                return attachment;
            }
            return null;
        }


        private byte[] bytes;
        public byte[] Bytes
        {
            get { return bytes; }
            set { bytes = value; }
        }

    }
}