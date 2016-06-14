using System;
using System.Collections.Generic;
using System.Text;
using b = MetaBuilder.BusinessLogic;
namespace MetaBuilder.BusinessFacade.Storage
{
    public class RepositoryPair
    {
        private ClientRepository client;
        public ClientRepository Client
        {
            get { return client; }
        }

        private ServerRepository server;
        public ServerRepository Server
        {
            get { return server; }
        }

        public RepositoryPair(ClientRepository client, ServerRepository server)
        {
            this.client = client;
            this.server = server;
            MetaBuilder.DataAccessLayer.DataRepository.AddConnection("Remote", Core.Variables.Instance.ServerConnectionString);
            MetaBuilder.DataAccessLayer.DataRepository.AddConnection("Local", Core.Variables.Instance.ConnectionString);
        }

        public void CheckOut(RepositoryActionSpecification spec)
        {
            client.PutItem(b.VCStatusList.CheckedOut, spec);
            server.GetItem(b.VCStatusList.CheckedOut, ref spec, server.IsAdmin);
        }

        public void CheckOutRead(RepositoryActionSpecification spec)
        {
            client.PutItem(b.VCStatusList.CheckedOutRead, spec);
        }

        public void CheckIn(RepositoryActionSpecification spec)
        {
            client.PutItem(b.VCStatusList.CheckedIn, spec);
            server.GetItem(b.VCStatusList.CheckedIn, ref spec, false);
        }
    }
}
