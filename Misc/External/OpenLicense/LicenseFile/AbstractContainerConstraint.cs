//--------------------------------------------------------------------------------
// Open License - A license manager for .NET software
// Copyright (C) 2004-2006 SP extreme (http://www.spextreme.com)
//
// This library is free software; you can redistribute it and/or modify it under 
// the terms of the GNU Lesser General Public License as published by the Free 
// Software Foundation; either version 2.1 of the License, or (at your option)  
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more 
// details.
//
// You should have received a copy of the GNU Lesser General Public License 
// along with this library; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//--------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

namespace OpenLicense.LicenseFile
{
	/// <summary>
	/// <p>This <see cref="AbstractContainerConstraint">AbstractContainerConstraint</see> 
	/// is used to define a container for other constraints.  This is used to provide
	/// a method to create grouping of constraints to provide bitwise operations.</p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	public abstract class AbstractContainerConstraint : AbstractConstraint
	{

		#region Fields (1) 

		private	List<IConstraint> constraints	= new List<IConstraint>( );

		#endregion Fields 


#region Properties		
		/// <summary>
		/// Gets or Sets the <c>ConstraintCollection</c> for this ContainerConstraint.
		/// </summary>
		/// <param>
		/// Sets the <c>ConstraintCollection</c> for this ContainerConstraint.
		/// </param>
		/// <returns>
		///	Gets the <c>ConstraintCollection</c> for this ContainerConstraint.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(null),
		Description( "Gets or Sets the ConstraintCollection for this ContainerConstraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		ReadOnly(true)
		]
		public List<IConstraint> Items
		{
			get 
			{
				return this.constraints;
			}
			set
			{
				this.constraints = value;
			}
		}
#endregion
	}
} /* NameSpace OpenLicense */
