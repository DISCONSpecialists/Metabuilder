using System;
using System.Collections.Generic;
using System.Text;
using b = MetaBuilder.BusinessLogic;
namespace MetaBuilder.BusinessFacade.Storage
{
    /*
    public class ServerRepository : RepositoryBase
    {
        public ServerRepository()
            : base(RepositoryType.Server)
        {
            this.ConnectionString = Core.Variables.Instance.ServerConnectionString;
        }

        public ServerRepository(bool isAdmin)
            : base(RepositoryType.Server, isAdmin)
        {
            this.ConnectionString = Core.Variables.Instance.ServerConnectionString;
        }
        public void ReadState(b.IRepositoryItem item)
        {
            //item.ServerItemState = GetState(item);
        }
        public override CheckInResult PutItem(b.VCStatusList targetState, RepositoryActionSpecification spec)
        {
            b.IRepositoryItem item = spec.Item;
            bool error = false;
            switch (targetState)
            {
                case b.VCStatusList.None:
                    error = true;
                    break;
                case b.VCStatusList.CheckedOutRead:
                    error = true;
                    break;
            }

            if (error)
            {
                throw new Exception(item.Pkid.ToString() + " cannot put as " + targetState.ToString());
            }


            //item.ServerItemState = targetState;
            return base.PutItem(targetState, spec);
        }

        public override CheckOutResult GetItem(b.VCStatusList targetState, ref RepositoryActionSpecification spec, bool force)
        {
            b.IRepositoryItem item = spec.Item;
            b.VCStatusList serverState = GetState(item);
            bool error = false;
            switch (serverState)
            {
                case b.VCStatusList.None:
                    switch (targetState)
                    {
                        case b.VCStatusList.CheckedOut:
                            error = true;
                            break;
                        case b.VCStatusList.CheckedOutRead:
                            error = true;
                            break;
                        case b.VCStatusList.Locked:
                            error = true;
                            break;
                        case b.VCStatusList.Obsolete:
                            error = true;
                            break;
                    }
                    break;
                case b.VCStatusList.CheckedOut:
                    switch (targetState)
                    {
                        case b.VCStatusList.CheckedIn:
                            // can do

                            break;
                        case b.VCStatusList.CheckedOut:
                           /* if (!force)
                                error = true;
                            //TODO: Enable Enforcer!
                            break;
                    }

                    break;
                case b.VCStatusList.CheckedIn:
                    switch (targetState)
                    {
                        case b.VCStatusList.CheckedIn:
                           // if (item.ClientItemState == b.VCStatusList.CheckedOutRead)
                            {
                                return CheckOutResult.PartialSuccess;
                            }
                          if (!force)
                                error = true;
                            //TODO: Enable Enforcer!
                            break;
                    }
                    break;
            }
            if (error)
            {
                throw new Exception(item.Pkid.ToString() + item.MachineName + " Can't change state [" + serverState.ToString() + "] to " + targetState.ToString());
            }

            if (this.RepositoryDictionary.ContainsKey(item.Pkid.ToString() + item.MachineName))
            {
                if (item is b.GraphFile)
                {
                    b.GraphFile g = item as b.GraphFile;
                    b.GraphFile added = RepositoryDictionary[item.Pkid.ToString() + item.MachineName] as b.GraphFile;
                    if (g.ModifiedDate < added.ModifiedDate)
                        throw new Exception("Newer version in repository");

                }
            }

            // Set state on server itself
            // --------------------------
            error = false;
            switch (item.State)
            {
                case b.VCStatusList.CheckedOut:
                    //throw new Exception("already checked out");
                    break;
                case b.VCStatusList.Locked:
                    break;
                case b.VCStatusList.Obsolete:
                    break;
            }
            base.SaveState(spec, targetState);
           // item.ServerItemState = targetState;
            return base.GetItem(targetState, ref spec, force);
        }
        
        public override List<b.VCStatusList> GetAllowedStates(b.IRepositoryItem item)
        {
            List<b.VCStatusList> states = new List<b.VCStatusList>();
            switch (item.State)
            {
                case b.VCStatusList.CheckedIn:
                    states.Add(MetaBuilder.BusinessLogic.VCStatusList.CheckedOut);
                    states.Add(MetaBuilder.BusinessLogic.VCStatusList.CheckedOutRead);
                    if (IsAdmin)
                    {
                        states.Add(b.VCStatusList.Locked);
                        states.Add(b.VCStatusList.Obsolete);
                    }
                    break;
                case b.VCStatusList.CheckedOut:
                    states.Add(MetaBuilder.BusinessLogic.VCStatusList.CheckedOutRead);
                    if (IsAdmin)
                    {
                        states.Add(b.VCStatusList.CheckedIn);
                        states.Add(b.VCStatusList.Locked);
                        states.Add(b.VCStatusList.Obsolete);
                    }
                    break;
                case b.VCStatusList.Locked:
                    states.Add(b.VCStatusList.CheckedOutRead);
                    if (IsAdmin)
                    {
                        states.Add(b.VCStatusList.CheckedIn);
                        states.Add(b.VCStatusList.Locked);
                        states.Add(b.VCStatusList.Obsolete);
                    }
                    break;
                case b.VCStatusList.None:
                    states.Add(b.VCStatusList.CheckedIn);
                    break;
                case b.VCStatusList.Obsolete:
                    states.Add(b.VCStatusList.CheckedOutRead);
                    if (IsAdmin)
                    {
                        states.Add(b.VCStatusList.CheckedIn);
                        states.Add(b.VCStatusList.Locked);
                        states.Add(b.VCStatusList.Obsolete);
                    }
                    break;
            }
            return states;
        }

    }*/
}
