using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;

namespace yadisk_automation
{
    static class ElementActions
    {
        public static void ClickElement(AutomationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("ClickElement");

            var invokePattern = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            invokePattern.Invoke();
        }

        //public static void HardcoreClickElement(AutomationElement element)
        //{
        //    if (element == null)
        //        throw new ArgumentNullException("HardcoreClickelement");

        //    Cursor.Position = Utilities.ConvertPoint(element.Current.BoundingRectangle.TopLeft);
        //    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new IntPtr());
        //    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new IntPtr());
        //}

        public static void InsertTextWorkaround(AutomationElement element, string text)
        {
            if (element == null)
                throw new ArgumentNullException("InsertText");

            ClickElement(element);
            SendKeys.SendWait(text);
        }

        public static void InsertText(AutomationElement element, string text)
        {
            if (element == null)
                throw new ArgumentNullException("InsertText");

            var editValue = (ValuePattern)element.GetCurrentPattern(ValuePattern.Pattern);
            editValue.SetValue(text);
        }
        
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;
        private const UInt32 MOUSEEVENTF_MOVE = 0x0001;
        private const UInt32 MOUSEEVENTF_ABSOLUTE = 0x8000;

        [DllImport("user32.dll")]
        private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);

        public static void DragDrop(Point _from, Point _to)
        {
            var from = Utilities.ConvertPoint(_from);
            var to = Utilities.ConvertPoint(_to);

            @from.Offset(25, 25);
            to.Offset(25, 25);

            Cursor.Position = @from;
            Console.WriteLine(string.Format("Cursor is set to {0} {1}", @from.X, @from.Y));

            Thread.Sleep(1000);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new IntPtr());
            Console.WriteLine("Mouse down");
            Thread.Sleep(3000);

            Cursor.Position = to;
            Console.WriteLine(string.Format("Cursor is set to {0} {1}", to.X, to.Y));
            Thread.Sleep(200);
            to.Offset(5,5);
            Cursor.Position = to;
            Console.WriteLine(string.Format("Cursor is set to {0} {1}", to.X, to.Y));
            Thread.Sleep(3000);

            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new IntPtr());
            Console.WriteLine("Mouse up");
        }
    }
}