using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingWPF.helpers
{
    public abstract class RemoveAllHandlers
    {
        public static void RemoveAllhandlersFromOpject(object instance)
        {
            if (instance != null)
            {
                var eventsToClear = instance.GetType().GetEvents(
                    BindingFlags.Public | BindingFlags.NonPublic
                                        | BindingFlags.Instance | BindingFlags.Static);

                foreach (var eventInfo in eventsToClear)
                {
                    try
                    {
                        var fieldInfo = instance.GetType().GetField(
                            eventInfo.Name,
                            BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                        if (fieldInfo != null)
                        {
                            if (fieldInfo.GetValue(instance) is Delegate eventHandler)
                                foreach (var invocatedDelegate in eventHandler.GetInvocationList())
                                    eventInfo.GetRemoveMethod(fieldInfo.IsPrivate).Invoke(
                                        instance,
                                        new object[] { invocatedDelegate });
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
