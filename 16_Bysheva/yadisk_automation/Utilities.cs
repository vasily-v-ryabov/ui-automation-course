using System;
using System.Windows;
using System.Windows.Automation;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace yadisk_automation
{
    static class Utilities
    {
        public class CannotOpenExplorerException : Exception
        { }

        static private class KeyboardSend
        {
            [DllImport("user32.dll")]
            private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

            private const int KEYEVENTF_EXTENDEDKEY = 1;
            private const int KEYEVENTF_KEYUP = 2;

            public static void KeyDown(Keys vKey)
            {
                keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
            }

            public static void KeyUp(Keys vKey)
            {
                keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            }
        }

        public static void PressWinAnd(Keys key) 
        {
            KeyboardSend.KeyDown(Keys.LWin);
            KeyboardSend.KeyDown(key);
            KeyboardSend.KeyUp(key);
            KeyboardSend.KeyUp(Keys.LWin);
        }

        public static void AlignWindowToRight()
        {
            Console.WriteLine("Sending WIN+RIGHT to align window to right");
            Utilities.PressWinAnd(Keys.Right);
        }

        public static void AlignWindowToLeft()
        {
            Console.WriteLine("Sending WIN+LEFT to align window to left");
            Utilities.PressWinAnd(Keys.Left);
        }

        public static void RecursiveGoRound(AutomationElement parent, TreeScope treeScope, int recursionLevel = 0)
        {
            if (parent == null)
                throw new ArgumentNullException("RecursiveGoRound");

            for (var i = 0; i < recursionLevel; i++)
                Console.Write("   ");

            Console.WriteLine(string.Format("'{0}' --- '{1}'", parent.Current.LocalizedControlType, parent.Current.Name));

            var children = parent.FindAll(treeScope, Condition.TrueCondition);

            recursionLevel++;
            foreach (AutomationElement element in children)
                RecursiveGoRound(element, treeScope, recursionLevel);
        }

        public static System.Drawing.Point ConvertPoint(Point point)
        {
            return new System.Drawing.Point((int)point.X, (int)point.Y);
        }

        public static string GetExplorerWindowTitle(string filePath)
        {
            filePath = Path.GetFullPath(filePath);

            var explorerWindowTitle = Path.GetDirectoryName(filePath).Split(Path.DirectorySeparatorChar).Last();
            var root = Path.GetPathRoot(filePath);
            if (explorerWindowTitle == root || explorerWindowTitle == string.Empty)
            {
                var driveInfo = DriveInfo.GetDrives();
                var drive = driveInfo.Single(d => d.Name.Equals(root, StringComparison.InvariantCultureIgnoreCase));
                explorerWindowTitle = string.Format("{0} ({1})", drive.VolumeLabel, drive.Name.Split(Path.DirectorySeparatorChar).First());
            }

            return explorerWindowTitle;
        }
    }
}