#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: RichTextEditor.cs
//

#endregion

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace MetaBuilder.Meta.Editors
{
    public class FileEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            // We will use a window for property editing. 
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            AttachmentEditForm frm = new AttachmentEditForm();

            Attachment lt;

            if (value != null)
            {
                lt = (Attachment)value;
            }
            else
            {
                lt = new Attachment();
            }

            frm.myAttachment = lt;
            DialogResult res = frm.ShowDialog();
            if (res == DialogResult.OK)
            {
                return frm.myAttachment;
            }
            return lt;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            // No special thumbnail will be shown for the grid. 
            return false;
            //return false;
        }
    }
}