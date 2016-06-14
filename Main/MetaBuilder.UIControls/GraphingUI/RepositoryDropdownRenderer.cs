using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.BusinessLogic;
using XPTable.Renderers;

namespace MetaBuilder.UIControls.GraphingUI
{
    public class RepositoryDropdownRenderer:DropDownCellRenderer 
    {

		#region Methods (1) 


		// Public Methods (1) 

        public override void OnPaintCell(XPTable.Events.PaintCellEventArgs e)
        {
            if (e.Cell.Row != null)
            {
                //// Console.WriteLine(e.Cell.Row.ToString());
            }
            base.OnPaintCell(e);
        }


		#endregion Methods 

    }
}
