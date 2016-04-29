using System;
using System.Windows;
using System.Windows.Automation;
using System.IO;
using System.Linq;

namespace yadisk_automation
{
    static class Utilities
    {
        public class CannotOpenExplorerException : Exception
        { }

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