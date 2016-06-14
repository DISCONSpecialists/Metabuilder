#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: DomainAttribute.cs
//

#endregion

using System;

namespace MetaBuilder.Meta.Editors
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class DomainAttribute : Attribute
	{
		public string DomainDefinition = "General";

		public DomainAttribute()
		{
		}

		public DomainAttribute(string domainDefinition)
		{
			DomainDefinition = domainDefinition;
		}

	}

}