using System;
using System.Collections.Generic;
using System.Text;
using b = MetaBuilder.BusinessLogic;
namespace MetaBuilder.BusinessFacade.Storage
{
    /*
    public class ClientRepository : RepositoryBase
    {
        public ClientRepository()
            : base(RepositoryType.Client)
        {
            this.ConnectionString = Core.Variables.Instance.ConnectionString;
        }
        public override CheckInResult PutItem(b.VCStatusList targetState, RepositoryActionSpecification spec)
        {
            b.IRepositoryItem item = spec.Item;
            if (!this.IsAdmin)
            {
                switch (targetState)
                {
                    case b.VCStatusList.Locked:
                        throw new Exception("Cannot put Locked");
                        break;
                    case b.VCStatusList.Obsolete:
                        throw new Exception("Cannot put an obsolete item");
                        break;
                }
            }

            //item.ClientItemState = targetState;
            base.SaveState(spec, targetState);
            return base.PutItem(targetState, spec);
        }


        public override List<b.VCStatusList> GetAllowedStates(b.IRepositoryItem item)
        {
            List<b.VCStatusList> states = new List<b.VCStatusList>();
            switch (GetState(item))
            {
                case b.VCStatusList.CheckedOut:
                    states.Add(b.VCStatusList.CheckedIn);
                    break;
                case b.VCStatusList.CheckedOutRead:
                    break;
                case b.VCStatusList.None:
                    states.Add(b.VCStatusList.CheckedIn);
                    break;
            }
            return states;
        }

       
    }*/
}
