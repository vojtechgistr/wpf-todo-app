using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace ToDoListApp.Utils
{
    public static class TraceUtil
    {
        [Conditional("TRACE")]
        public static void LogMethodCall(params object[] callingMethodParamValues)
        {
            var method = new StackFrame(skipFrames: 1).GetMethod();
            var methodParams = method.GetParameters();
            var methodCalledBy = new StackFrame(skipFrames: 2).GetMethod();

            var methodCaller = "";
            if (methodCalledBy != null)
            {
                methodCaller = $"{methodCalledBy.DeclaringType?.Name}.{methodCalledBy.Name}()";
            }

            if (methodParams.Length == callingMethodParamValues.Length)
            {
                List<string> paramList = new List<string>();
                foreach (var param in methodParams)
                {
                    paramList.Add($"{param.Name}={callingMethodParamValues[param.Position]}");
                }

                Log(method.Name, string.Join(", ", paramList), methodCaller);

            }
            else
            {
                Log(method.Name, "/* No parameters passed */", methodCaller);
            }


        }

        public static void LogWelcomeMessage()
        {
            Console.WriteLine("  _       ____    _____   _____  ______  _____  \r\n | |     / __ \\  / ____| / ____||  ____||  __ \\ \r\n | |    | |  | || |  __ | |  __ | |__   | |__) |\r\n | |    | |  | || | |_ || | |_ ||  __|  |  _  / \r\n | |____| |__| || |__| || |__| || |____ | | \\ \\ \r\n |______|\\____/  \\_____| \\_____||______||_|  \\_\\\r\n                                                \r\n                                                ");
        }

        private static void Log(string methodName, string parameterList, string methodCaller)
        {
            try
            {
                Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss.fffff")}\t{methodCaller} -> {methodName}({parameterList})");
            } catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

    }
}