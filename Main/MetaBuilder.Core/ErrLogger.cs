using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using TraceTool;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MetaBuilder.Core
{
    public class ErrLogger
    {

        public static void Log()
        {
            Log(null);
        }

        public static void Log(Exception x)
        {
                LogEntry logEntry = new LogEntry();
                StringBuilder preamble = new StringBuilder();
                StackTrace stackTrace = new StackTrace();
                StackFrame stackFrame;
                MethodBase stackFrameMethod;
                string typeName;
                for (int i = 0; i < stackTrace.FrameCount; i++)
                {
                    stackFrame = stackTrace.GetFrame(i);
                    stackFrameMethod = stackFrame.GetMethod();
                    typeName = stackFrameMethod.ReflectedType.FullName;

                    //if ((typeName.StartsWith("System")))
                    {
                        preamble.Append("StackFrameMethod:" + typeName + "." + stackFrameMethod.Name);
                        preamble.Append("( ");

                        // log parameter types and names

                        ParameterInfo[] parameters =
                            stackFrameMethod.GetParameters();
                        int parameterIndex = 0;
                        while (parameterIndex < parameters.Length)
                        {
                            preamble.Append(parameters
                                                [parameterIndex].ParameterType.Name);
                            preamble.Append(" ");
                            preamble.Append(parameters[parameterIndex].Name);
                            parameterIndex++;
                            if (parameterIndex != parameters.Length)
                                preamble.Append(", ");
                        }
                        preamble.Append(" ): ");
                    }
                }

                //log DateTime, Namespace, Class and Method Name


                if (x != null)
                {
                    logEntry.Message = x.ToString() + x.Message;
                }
                else
                    logEntry.Message = "";
                logEntry.Message += preamble.ToString();
                Logger.Write(logEntry);
        }
    }
}
