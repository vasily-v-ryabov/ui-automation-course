using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;

namespace yadisk_automation
{
    static class YadiskActions
    {
        public static TimeSpan DefaultTimeout
        {
            get { return TimeSpan.FromSeconds(10); }
        }

        public static void Login(AutomationElement mainWindow, string logine, string passworde)
        {
            if (mainWindow == null)
                throw new ArgumentNullException("Login");

            ElementActions.InsertTextWorkaround(ElementFinder.TryFindElement(mainWindow, "логин", "edit", DefaultTimeout), logine);
            ElementActions.InsertTextWorkaround(ElementFinder.TryFindElement(mainWindow, "пароль", "edit", DefaultTimeout), passworde);
            ElementActions.ClickElement(ElementFinder.TryFindElement(mainWindow, "войти ", "button", DefaultTimeout));

            Thread.Sleep(DefaultTimeout);
        }

        public static void ChooseFile(AutomationElement mainWindow, string path)
        {
            if (mainWindow == null)
                throw new ArgumentNullException("ChooseFile");

            ElementActions.ClickElement(ElementFinder.TryFindElement(mainWindow, "Choose Files Загрузить", "button", DefaultTimeout.Add(DefaultTimeout)));

            var uploadDialog = ElementFinder.TryFindElement(mainWindow, "Open", "dialog", DefaultTimeout, TreeScope.Children);
            var address = ElementFinder.TryFindElement(uploadDialog, "file name:", "edit", DefaultTimeout);

            //InsertText(address, path);
            //WORKAROUND:
            SendKeys.SendWait(path);

            ElementActions.ClickElement(ElementFinder.TryFindElement(uploadDialog, "Open", "button", DefaultTimeout));
        }



        public static void DragFileFromExplorer(string filePath, Point dropTarget)
        {
            var explorerProcess = Process.Start("explorer.exe", Path.GetDirectoryName(filePath));
            if (explorerProcess == null)
                throw new Utilities.CannotOpenExplorerException();

            Thread.Sleep(DefaultTimeout);

            AutomationElement explorerWindow = null;

            try
            {
                explorerWindow = ElementFinder.TryFindElement(AutomationElement.RootElement, Utilities.GetExplorerWindowTitle(filePath), "window", DefaultTimeout);
                Utilities.AlignWindowToRight();

                ElementActions.InsertText(ElementFinder.TryFindElement(explorerWindow, "search box", "edit", DefaultTimeout), Path.GetFileName(filePath));
                Thread.Sleep(1000);
                var file = ElementFinder.TryFindElement(explorerWindow, Path.GetFileName(filePath), "list item", DefaultTimeout);

                var filePoint = file.Current.BoundingRectangle.TopLeft;
                filePoint.Offset(file.Current.BoundingRectangle.Width/2, file.Current.BoundingRectangle.Height/2);
                ElementActions.DragDrop(filePoint, dropTarget);

                ElementActions.ClickElement(ElementFinder.TryFindElement(explorerWindow, "close", "button", DefaultTimeout));
            }
            catch (Exception e)
            {
                ElementActions.ClickElement(ElementFinder.TryFindElement(explorerWindow, "close", "button", DefaultTimeout));
                throw e;
            }
        }

        public static void MonitorDownload(AutomationElement browserWindow)
        {
            if (browserWindow == null)
                throw new ArgumentNullException("MonitorDownload");

            Console.WriteLine("...Please wait ~2 minutes. Checking if file was uploaded before...");

            var replaceButton = ElementFinder.TryFindElement(browserWindow, "заменить ", "button", TimeSpan.FromSeconds(0));
            if (replaceButton != null)
                ElementActions.ClickElement(replaceButton);

            ElementFinder.TryFindElement(browserWindow, "загрузка завершена", "text", DefaultTimeout);

            Console.WriteLine("\nDone!");
        }
    }
}