using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MetaBuilder.UIControls.GraphingUI.Tools.Help
{
    public class HelpItem
    {
        public HelpItem(string words, string topic, string description)
        {

        }
        public HelpItem(string words, string topic, string description, Bitmap image)
        {

        }

        private string keyWords;
        public string KeyWords { get { return keyWords; } set { keyWords = value; } }

        private string topicName;
        public string TopicName { get { return topicName; } set { topicName = value; } }

        private string description;
        public string Descripition { get { return description; } set { description = value; } }

        private Bitmap image;
        public Bitmap Image { get { return image; } set { image = value; } }
    }
}