using System;
using System.Threading;
using System.Windows.Automation;

namespace yadisk_automation
{
    static class ElementFinder
    {
        public static AutomationElement FindElementByNameAndType(AutomationElement mainWindow, string name, string type, TreeScope treeScope = TreeScope.Descendants)
        {
            if (mainWindow == null)
                throw new ArgumentNullException("FindElementByNameAndType");

            return mainWindow.FindFirst(treeScope,
                                        new AndCondition(
                                            new PropertyCondition(AutomationElement.NameProperty, name, PropertyConditionFlags.IgnoreCase),
                                            new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, type, PropertyConditionFlags.IgnoreCase)));
        }

        public static AutomationElement TryFindElement(AutomationElement where, string name, string type, TimeSpan timeout, TreeScope treeScope = TreeScope.Descendants)
        {
            if (@where == null || timeout == null)
                throw new ArgumentNullException("TryFindElement");

            var elapsedTime = TimeSpan.FromSeconds(0);
            var result = FindElementByNameAndType(@where, name, type, treeScope);
            
            while (result == null)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
                Console.WriteLine(string.Format("...searching for {0} '{1}'...", type, name));

                if (elapsedTime > timeout)
                {
                    Console.WriteLine(string.Format("Cannot find {2} '{0}'. Failed by timeout '{1}'", name, timeout.Seconds, type));
                    return null;
                }

                result = FindElementByNameAndType(@where, name, type, treeScope);
            }
            Console.WriteLine(string.Format("{1} '{0}' found", name, type));

            return result;
        }
    }
}