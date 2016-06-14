#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: IMetaBase.cs
//

#endregion

using System;

namespace MetaBuilder.Meta
{
	/// <summary>
	/// Summary description for IMetaBase.
	/// </summary>
	public interface IMetaBase
	{
		void LoadEmbeddedObjects(int CurrentLevel, int TargetLevel);
		void Set(string PropertyName, object value);
		object Get(string PropertyName);
		void Save(Guid unique);
	}
}