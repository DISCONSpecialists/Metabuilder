using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MetaBuilder.Graphing.Containers;
using Northwoods.Go;
using TraceTool;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;


namespace MetaBuilder.Graphing.Controllers
{
    public class SerializationTester
    {
        public void TestSerialization(NormalDiagram diagram)
        {
            try
            {
                Stream ofile = File.Open("test.graph", FileMode.Create);
                IFormatter oformatter = new BinaryFormatter();
                oformatter.Serialize(ofile, diagram);
                ofile.Close();

                Stream ifile = File.Open("test.graph", FileMode.Open);
                IFormatter iformatter = new BinaryFormatter();
                ifile.Close();
            }
            catch (Exception x)
            {
#if DEBUG
                TTrace.Debug.Send(x.ToString());
                TTrace.Debug.Send(x.Message);
                TTrace.Debug.Send(x.Source);
                TTrace.Debug.Send(x.StackTrace);
#endif
            }
            //myView.Document = doc;
        }
    }
}