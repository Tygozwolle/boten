#region

using System.Reflection;

#endregion

namespace RoeiVerenigingWPF.Helpers;

public abstract class RemoveAllHandlers
{
    public static void RemoveAllhandlersFromOpject(object instance)
    {
        if (instance != null)
        {
            var eventsToClear = instance.GetType().GetEvents(
                BindingFlags.Public | BindingFlags.NonPublic
                                    | BindingFlags.Instance | BindingFlags.Static);

            foreach (EventInfo eventInfo in eventsToClear)
            {
                try
                {
                    FieldInfo? fieldInfo = instance.GetType().GetField(
                        eventInfo.Name,
                        BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                    if (fieldInfo != null)
                    {
                        if (fieldInfo.GetValue(instance) is Delegate eventHandler)
                            foreach (Delegate invocatedDelegate in eventHandler.GetInvocationList())
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