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

namespace MetaBuilder.Meta.Editors
{
	public class RichTextEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			// We will use a window for property editing. 
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			RichTextEditForm frm = new RichTextEditForm();

			LongText lt;

			if (value != null)
			{
				lt = (LongText) value;
			}
			else
			{
				lt = new LongText();
			}

			frm.longText = lt;
			frm.ShowDialog();

			// Return the new value. 
			return frm.longText;

		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			// No special thumbnail will be shown for the grid. 
			return false;
			//return false;
		}
	}
}