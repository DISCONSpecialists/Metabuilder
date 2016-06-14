using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace MetaBuilder.Viewer
{
    [Serializable]
    class SingletonController : MarshalByRefObject
    {
        private static TcpChannel m_TCPChannel = null;
        private static Mutex m_Mutex = null;

        public delegate void ReceiveDelegate(string[] args);

        static private ReceiveDelegate m_Receive = null;
        static public ReceiveDelegate Receiver
        {
            get
            {
                return m_Receive;
            }
            set
            {
                m_Receive = value;
            }
        }

        public static bool IamFirst(ReceiveDelegate r)
        {
            if (IamFirst())
            {
                Receiver += r;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IamFirst()
        {
            try
            {
                string m_UniqueIdentifier;
                string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName(false).CodeBase;
                m_UniqueIdentifier = assemblyName.Replace("\\", "_");
                m_Mutex = new Mutex(false, m_UniqueIdentifier);
                if (m_Mutex.WaitOne(1, true))
                {  
                    try
                    {
                        CreateInstanceChannel();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    //Not the first instance!!!
                    m_Mutex.Close();
                    m_Mutex = null;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static void CreateInstanceChannel()
        {
            try
            {
                m_TCPChannel = new TcpChannel(9876);
                ChannelServices.RegisterChannel(m_TCPChannel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType("MetaBuilder.Viewer.SingletonController"), "SingletonController", WellKnownObjectMode.SingleCall);
            }
            catch
            {
                m_TCPChannel = new TcpChannel(6789);
                ChannelServices.RegisterChannel(m_TCPChannel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType("MetaBuilder.Viewer.SingletonController"), "SingletonController", WellKnownObjectMode.SingleCall);
            }
        }

        public static void Cleanup()
        {
            if (m_Mutex != null)
            {
                m_Mutex.Close();
            }

            if (m_TCPChannel != null)
            {
                m_TCPChannel.StopListening(null);
            }

            m_Mutex = null;
            m_TCPChannel = null;
        }

        public static void Send(string[] s)
        {
            SingletonController ctrl;
            try
            {
                TcpChannel channel = new TcpChannel();
                ChannelServices.RegisterChannel(channel, false);
            }
            catch (Exception ex)
            {
                //channel name 'tcp' already in use?
                ex.ToString();
            }
            try
            {
                ctrl = (SingletonController)Activator.GetObject(typeof(SingletonController), "tcp://localhost:9876/SingletonController");
            }
            catch (Exception exA)
            {
                //Core.Log.WriteLog(exA.ToString());
                try
                {
                    ctrl = (SingletonController)Activator.GetObject(typeof(SingletonController), "tcp://localhost:6789/SingletonController");
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog(ex.ToString());
                    // Console.WriteLine("Exception: " + e.Message);
                    //throw;
                    return;
                }
            }
            if (ctrl != null)
            {
                ctrl.Receive(s);
            }
            else
            {
                //Core.Log.WriteLog("SingletonController is null");
            }
        }

        public void Receive(string[] s)
        {
            if (m_Receive != null)
            {
                m_Receive(s);
            }
        }
    }
}
