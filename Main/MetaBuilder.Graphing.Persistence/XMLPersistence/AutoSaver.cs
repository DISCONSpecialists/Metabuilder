using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Containers;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence
{
    public class AutoSaver
    {

        #region Fields (2)

        private string autosaveFileName;
        private object dgm;

        #endregion Fields

        #region Properties (2)

        public string AutosaveFileName
        {
            get { return autosaveFileName; }
            set { autosaveFileName = value; }
        }

        public object Diagram
        {
            get { return dgm; }
            set { dgm = value; }
        }

        #endregion Properties

        #region Methods (1)


        // Public Methods (1) 

        public void Execute()
        {
            try
            {
#if DEBUG
                Console.WriteLine("AutoSaver::Execute::" + autosaveFileName + " " + DateTime.Now);
                return;
#endif
                FileUtil futil = new FileUtil();
                futil.Save(Diagram, autosaveFileName);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("AutoSaver::Execute" + Environment.NewLine + ex.ToString());
            }
        }


        #endregion Methods

    }
}
