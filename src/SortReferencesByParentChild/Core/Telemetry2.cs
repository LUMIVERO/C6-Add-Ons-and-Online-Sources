using SwissAcademic.ApplicationInsights;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SwissAcademic.Addons.SortReferencesByParentChildAddon
{
    internal static class Telemetry2
    {
        public static void Warning(Exception exception, [CallerFilePath] string callerFilePath = null, [CallerMemberName] string callerMemberName = null)
        {
            var method = typeof(Telemetry).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m => m.Name.Equals("Warning") && m.GetParameters() is ParameterInfo[] parameters && parameters.Length != 0 && parameters[0].ParameterType.Equals(typeof(Exception)));
            if (method != null)
            {
                method.Invoke(null, new object[] { exception, string.Empty, exception.Message, string.Empty, null });
                return;
            }

            method = typeof(Telemetry).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m => m.Name.Equals("TrackException") && m.GetParameters() is ParameterInfo[] parameters && parameters.Length != 0 && parameters[0].ParameterType.Equals(typeof(Exception)));
            if (method != null)
            {
                method.Invoke(null, new object[] { exception, SeverityLevel.Warning, ExceptionFlow.Eat, null, null, null, callerFilePath, callerMemberName });
            }
        }
    }
}