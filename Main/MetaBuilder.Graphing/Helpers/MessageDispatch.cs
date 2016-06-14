using System;
using Northwoods.Go;

namespace MetaBuilder.Graphing
{
    public class DispatchMessageEventArgs : EventArgs
    {
        #region Fields (3)

        private string _message;
        private MessageType _messageType;
        private GoObject particularObject;
        private GoObject sender;

        public DispatchMessageEventArgs()
        {
        }

        #endregion Fields

        #region Constructors (1)

        public DispatchMessageEventArgs(MessageType messageType, string message)
        {
            _message = message;
            _messageType = messageType;
        }

        #endregion Constructors

        #region Properties (3)

        public GoObject ParticularObject
        {
            get { return particularObject; }
            set { particularObject = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public MessageType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        public GoObject Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        #endregion Properties
    }

    public delegate void DispatchMessageEventHandler(object sender, DispatchMessageEventArgs e);

    //public class MessageDispatch
    //{
    //    private static MessageDispatch Dispatcher;
    //}

    public enum MessageType
    {
        Info,
        Warning,
        Error,
        Status,
        ObjectRemovedFromILinkContainer
    }
}