#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ObjectHelper.cs
//

#endregion

using System.Collections.Generic;
using System.Data;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer.OldCode.Meta;
using MetaBuilder.Meta;

namespace MetaBuilder.BusinessFacade.MetaHelper
{
    /// <summary>
    /// Summary description for ObjectHelper.
    /// </summary>
    public class ObjectHelper
    {
        private bool Server = false;
        private string Provider { get { return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        public ObjectHelper(bool server)
        {
            //
            // TODO: Add constructor logic here
            //
            Server = server;
        }

        public static MetaBase ConvertToMetaBase(MetaObject obj, bool fromServer)
        {
            //string prevConstring = Variables.Instance.ConnectionString;
            //if (fromServer)
            //{
            //    Variables.Instance.ConnectionString = Variables.Instance.ServerConnectionString;
            //}
            MetaBase mbase = Loader.GetFromProvider(obj.pkid, obj.Machine, obj.Class, fromServer);
            //Variables.Instance.ConnectionString = prevConstring;
            return mbase;
        }
        public DataView ListObjects(string ClassName, int UserID)
        {
            ObjectAdapter objadapter = new ObjectAdapter();
            return objadapter.ListObjects(ClassName, UserID);
        }

        public DataView RetrieveObjects(string ClassName, int UserID)
        {
            ObjectAdapter objadapter = new ObjectAdapter();
            return objadapter.RetrieveObjects(ClassName, UserID);
        }
        public DataView GetObjectsFiltered(List<string> filters)
        {
            ObjectAdapter objadapter = new ObjectAdapter();
            return objadapter.GetObjectsFiltered(filters);
        }
        public DataView GetObjectsFiltered(List<string> filters, bool Server)
        {
            ObjectAdapter objadapter = new ObjectAdapter();
            return objadapter.GetObjectsFiltered(filters, Server);
        }

        public DataView GetObjects(string ClassName, int ObjectID, string ObjectMachine, int CAid, int UserID)
        {
            ObjectAdapter objadapter = new ObjectAdapter();
            return objadapter.GetObjects(ClassName, ObjectID, ObjectMachine, CAid, UserID);
        }

        public void AddObjectAssociation(BusinessLogic.ObjectAssociation assoc)
        {
            ObjectAdapter objadapter = new ObjectAdapter();
            objadapter.AddObjectAssociation(assoc);
        }

        public List<MetaObject> GetOrphanedObjects(GraphFileKey ignoreFileKey)
        {
            ObjectAdapter objadapter = new ObjectAdapter();
            return objadapter.GetOrphanedObjects(ignoreFileKey);
        }

        public DataSet GetArrayDecomposedObjects(string ClassName, int UserID)
        {
            ObjectAdapter objadapter = new ObjectAdapter();
            return objadapter.GetArrayDecomposedObjects(ClassName, UserID);
        }

        public void DeleteObject(int ObjectID, string MachineName)
        {
            ObjectAdapter objadapter = new ObjectAdapter();
            objadapter.DeleteObject(ObjectID, MachineName, Server);
        }

        public void DeleteObject(int ObjectID, string MachineName, bool doNotAlterProcedure)
        {
            ObjectAdapter objadapter = new ObjectAdapter();
            objadapter.DeleteObject(ObjectID, MachineName, Server, doNotAlterProcedure);
        }
    }
}