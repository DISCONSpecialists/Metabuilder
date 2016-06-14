using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MetaBuilder.BusinessLogic;
using MetaBuilder.BusinessFacade.Storage;
using MetaBuilder.Meta;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UnitTests
{

        [TestFixture]
        public class GeneralTests
        {
            List<GraphFile> graphs;
            List<MetaBase> objects;
            GraphFile graph;
            GraphFile graph2;
            MetaBase gobj;
            MetaBase gobj2;
            MetaBase gobj3;
            MetaBase gobj4;
            ClientRepository client;
            ClientRepository client2;
            ServerRepository server;
            RepositoryPair repositoryPair;
            RepositoryPair repositoryPair2;
            ObjectAssociation oassoc;
            ObjectAssociation oassoc2;
            // 190	0	5	4/24/2007 4:30:43 PM	NULL	1	True	False	1.2.0.29693	<Binary data>	8	1	NULL	E:\Program Files\Discon Specialists\MetaBuilder\MetaData\Diagrams\add1.dgm	NULL	2	NULL	NULL
            // 191	0	2	4/25/2007 11:34:35 AM	NULL	1	True	False	1.2.1.20823	<Binary data>	8	1	NULL	E:\Program Files\Discon Specialists\MetaBuilder\MetaData\Diagrams\F1 Test.dgm	NULL	2	NULL	NULL

            /// <summary>
            /// Changes the test data's VCStatus to [None]
            /// </summary>
            private void ResetRepositorySettings()
            {
               // repositoryPair.Server.IsAdmin = true;
                graph.VCStatusID = (int)VCStatusList.None;
                graph2.VCStatusID = (int)VCStatusList.None;

                MetaObject mo = d.DataRepository.MetaObjectProvider.GetByPkidMachine(gobj.pkid, gobj.machineName);
                mo.VCStatusID = (int)VCStatusList.None;
                d.DataRepository.MetaObjectProvider.Update(mo);

                mo = d.DataRepository.MetaObjectProvider.GetByPkidMachine(gobj2.pkid, gobj2.machineName);
                mo.VCStatusID = (int)VCStatusList.None;
                d.DataRepository.MetaObjectProvider.Update(mo);

                mo = d.DataRepository.MetaObjectProvider.GetByPkidMachine(gobj3.pkid, gobj3.machineName);
                mo.VCStatusID = (int)VCStatusList.None;
                d.DataRepository.MetaObjectProvider.Update(mo);

                mo = d.DataRepository.MetaObjectProvider.GetByPkidMachine(gobj4.pkid, gobj4.machineName);
                mo.VCStatusID = (int)VCStatusList.None;
                d.DataRepository.MetaObjectProvider.Update(mo);

                d.DataRepository.GraphFileProvider.Update(graph);
                d.DataRepository.GraphFileProvider.Update(graph2);
                
                oassoc.VCStatusID = (int)VCStatusList.None;
                oassoc2.VCStatusID = (int)VCStatusList.None;
                d.DataRepository.ObjectAssociationProvider.Update(oassoc);
                d.DataRepository.ObjectAssociationProvider.Update(oassoc2);

                Assert.AreEqual(graph.ClientItemState, VCStatusList.None);
                Assert.AreEqual(graph2.ClientItemState, VCStatusList.None);
                Assert.AreEqual(graph.ServerItemState, VCStatusList.None);
                Assert.AreEqual(graph2.ServerItemState, VCStatusList.None);
                Assert.AreEqual(gobj.ClientItemState, VCStatusList.None);
                Assert.AreEqual(gobj.ServerItemState, VCStatusList.None);
                Assert.AreEqual(gobj2.ClientItemState, VCStatusList.None);
                Assert.AreEqual(gobj2.ServerItemState, VCStatusList.None);
                Assert.AreEqual(gobj3.ClientItemState, VCStatusList.None);
                Assert.AreEqual(gobj3.ServerItemState, VCStatusList.None);
                Assert.AreEqual(gobj4.ClientItemState, VCStatusList.None);
                Assert.AreEqual(gobj4.ServerItemState, VCStatusList.None);

                

            }
            [SetUp]
            public void DoSetup()
            {
                client = new ClientRepository();
                client2 = new ClientRepository();
                server = new ServerRepository(true);
                repositoryPair = new RepositoryPair(client, server);
                graphs = new List<GraphFile>();
                objects = new List<MetaBase>();
                graph = d.DataRepository.GraphFileProvider.GetByPkidMachine(190, "KNERSIS");
                graph2 = d.DataRepository.GraphFileProvider.GetByPkidMachine(191, "KNERSIS");

                gobj = Meta.Loader.CreateInstance("Function");
                gobj2 = Meta.Loader.CreateInstance("Function");
                gobj3 = Meta.Loader.CreateInstance("Function");
                gobj4 = Meta.Loader.CreateInstance("Function");
                gobj.LoadFromID(6951, "KNERSIS");
                gobj2.LoadFromID(6952, "KNERSIS");
                gobj3.LoadFromID(6953, "KNERSIS");
                gobj4.LoadFromID(6954, "KNERSIS");
                oassoc = d.DataRepository.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(
                    2341,7375,7376,"KNERSIS","KNERSIS");
                oassoc2 = d.DataRepository.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(
                    2413, 7373, 7375, "KNERSIS", "KNERSIS");

                graph.Contents.Add(oassoc);
                ResetRepositorySettings();
            }
           

            [Test]
            public void AssociationStartsWithStatusNone()
            {
                GraphFile file = new GraphFile();
                ObjectAssociation oassoc = new ObjectAssociation();
                oassoc.ChildObjectID = 7376;
                oassoc.CAid = 2341;
                oassoc.ObjectID = 7375;
                oassoc.ChildObjectMachine = "KNERSIS";
                oassoc.ObjectMachine = "KNERSIS";
                VCStatusList state = server.GetState(oassoc);
                file.Contents.Add(oassoc);
                RepositoryActionSpecification spec1 = new RepositoryActionSpecification();
                spec1.Item = file;
                spec1.LoadWorkspace(true);
                repositoryPair.CheckOut(spec1);
                Console.WriteLine("Graphfile:" + oassoc.ClientItemState);
            }


            [Test]
            public void CheckingOutMarksItemsThatHaveBeenCheckedOut()
            {
                RepositoryActionSpecification spec1 = new RepositoryActionSpecification();
                spec1.Item = graph2;
                spec1.LoadWorkspace(false);
                
                repositoryPair.CheckIn(spec1);
                repositoryPair.CheckOut(spec1);
                Assert.AreEqual(graph2.ClientItemState, VCStatusList.CheckedOut);
                Assert.AreEqual(gobj2.ClientItemState, VCStatusList.CheckedOut);
                Assert.AreEqual(gobj3.ClientItemState, VCStatusList.CheckedOut);
                Assert.AreEqual(gobj3.ClientItemState, VCStatusList.CheckedOut);

                RepositoryActionSpecification specForObject = new RepositoryActionSpecification();
                specForObject.Item = gobj3;
                specForObject.LoadWorkspace(true);
                repositoryPair.CheckIn(specForObject);
                Assert.AreEqual(VCStatusList.CheckedIn, gobj3.ServerItemState);
                Assert.AreEqual(VCStatusList.CheckedIn, gobj3.ClientItemState);
                Assert.AreEqual(true, server.IsAdmin);
                repositoryPair2.CheckOut(specForObject);
            }

            [Test]
            public void CheckingInACheckedInObjectRaisesAnError()
            {
                RepositoryActionSpecification specForObject = new RepositoryActionSpecification();
                specForObject.Item = gobj3;
                specForObject.LoadWorkspace(true);
                repositoryPair.CheckIn(gobj3);
                repositoryPair.CheckIn(gobj3);
            }

            [Test]
            public void TestPossibleActions()
            {
                List<VCStatusList> allowedStates = server.GetAllowedStates(graph);
                foreach (VCStatusList status in allowedStates)
                {
                    switch (status)
                    {
                        case VCStatusList.CheckedIn:
                            repositoryPair.CheckIn(graph);
                            break;
                        case VCStatusList.CheckedOut:
                            repositoryPair.CheckOut(graph);
                            break;
                        case VCStatusList.CheckedOutRead:
                            repositoryPair.CheckOutRead(graph);
                            break;
                        case VCStatusList.Locked:
                            repositoryPair.CheckOutRead(graph);
                            break;
                        case VCStatusList.None:
                            break;
                        case VCStatusList.Obsolete:
                            repositoryPair.CheckOutRead(graph);
                            break;
                        case VCStatusList.PartiallyCheckedIn:
                            repositoryPair.CheckOutRead(graph);
                            break;


                    }
                }

            }

            [Test]
            public void CheckPossibleTargetStatesForNone()
            {
                GraphFile g = new GraphFile();
                g.ServerItemState = VCStatusList.None;
                g.ClientItemState = VCStatusList.None;
                List<VCStatusList> allowedStates = server.GetAllowedStates(g);
                List<VCStatusList> expected = new List<VCStatusList>();
                expected.Add(VCStatusList.CheckedIn);
                Assert.AreEqual(expected.Count, allowedStates.Count);
            }

            [Test]
            public void CheckPossibleTargetStatesForCheckedIn()
            {
                GraphFile g = new GraphFile();
                g.ServerItemState = VCStatusList.CheckedIn;
                g.ClientItemState = VCStatusList.CheckedIn;
                List<VCStatusList> allowedStates = server.GetAllowedStates(g);
                List<VCStatusList> expected = new List<VCStatusList>();
                expected.Add(VCStatusList.CheckedOut);
                expected.Add(VCStatusList.CheckedOutRead);
                if (server.IsAdmin)
                {
                    expected.Add(VCStatusList.Locked);
                    expected.Add(VCStatusList.Obsolete);
                }
                Assert.AreEqual(expected.Count, allowedStates.Count);
            }
            [Test]
            public void TestVersionClash()
            {
                server.RepositoryDictionary.Add(graph.Pkid.ToString() + graph.MachineName, graph);
                GraphFile gCopy = graph.Copy();
                graph.ModifiedDate = DateTime.Now.AddDays(50);
                repositoryPair.CheckIn(gCopy);
            }

            [Test]
            public void CheckOutFile()
            {
                repositoryPair.CheckIn(graph);
                repositoryPair.CheckOut(graph);
                Assert.AreEqual(VCStatusList.CheckedOut, oassoc.ServerItemState);
                Assert.AreEqual(VCStatusList.CheckedOut, graph.ServerItemState);
                repositoryPair.CheckIn(gobj4);
                Console.WriteLine("Gobj4Server:" + gobj4.ServerItemState.ToString());
                List<VCStatusList> possibleStates = repositoryPair.Server.GetAllowedStates(gobj4);
                
     
                foreach (VCStatusList state in possibleStates)
                {
                    Console.WriteLine(state.ToString());
                }
                
            }
            private void Other()
            {
                client.PutItem(VCStatusList.None, gobj);
                client.PutItem(VCStatusList.None, gobj2);
                client.PutItem(VCStatusList.None, gobj3);
                client.PutItem(VCStatusList.None, gobj4);

                RepositoryPair pair = new RepositoryPair(client, server);
                RepositoryPair pair2 = new RepositoryPair(client2, server);

                Inspect();
                pair.CheckIn(graph);
                Inspect();
                pair.CheckIn(gobj4);
                Inspect();
                pair.CheckOut(gobj4);
                Inspect();
                pair.CheckIn(graph2);
                Inspect();
                pair.CheckOut(graph2);
                Inspect();
                pair.CheckIn(gobj2);
                Inspect();

                server.Name = "Server";
                client.Name = "Client";
                client2.Name = "Client 2";

                Console.WriteLine("xxxxxxxxxxxxxxxxxxxxx");
                
                pair2.CheckOut(graph2);
            }
            private static void ListAllowedStates(MetaBase gobj4, IRepositoryService service)
            {
                List<VCStatusList> allowedStates = service.GetAllowedStates(gobj4);
                foreach (VCStatusList state in allowedStates)
                {
                    Console.WriteLine("AllowedStatesChange " + service.Name + " :" + state.ToString());
                }
            }

            public void Inspect()
            {
                Console.WriteLine("---------------------------------");
                foreach (GraphFile g in graphs)
                {
                    Console.WriteLine(g.Pkid.ToString() + g.MachineName + " Server: " + g.ServerItemState.ToString() + " Client:" + g.ClientItemState.ToString());
                    foreach (IRepositoryItem item in g.Contents)
                    {
                        Console.WriteLine("\t" + item.Pkid.ToString() + item.MachineName + " Server: " + item.ServerItemState.ToString() + " Client:" + item.ClientItemState.ToString());
                    }

                }
            }



        }

}
