using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.Graphing.Helpers
{
    public class PluginProgressEventArgs : EventArgs
    {
        private float percent;

        public float Percent
        {
            get { return percent; }
            set { percent = value; }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

    }

    public delegate void PluginProgressEventHandler(object sender, PluginProgressEventArgs e);
}
