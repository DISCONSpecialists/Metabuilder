using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.CommonControls.Tree
{
	public interface IToolTipProvider
	{
		string GetToolTip(TreeNodeAdv node);
	}
}
