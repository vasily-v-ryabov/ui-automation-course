using System;
using System.Threading;
using System.Diagnostics;
using System.Windows.Automation;
using System.IO;

namespace yadisk_automation
{
    class Program
    {
        /// <summary>
        /// Before run:
        /// 1) If you have 'test.zip' in https://disk.yandex.ru/ test account please remove it
        /// 2) Close all google chrome windows
        /// 3) Run the application with parameter: yadisk_automation.exe path/to/file/test.zip
        /// </summary>

        private const string LOGIN = "sir.testowyj";
        private const string Password = "TestTest123!";

        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please pass path of the file to upload as command line parameter");
                return 3;
            }

            var filePath = Path.GetFullPath(args[0]);

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist");
                return 4;
            }

            var browserProcess = Process.Start("chrome.exe", "https://disk.yandex.ru/ -incognito --force-renderer-accessibility");
            if (browserProcess == null)
                return 1;

            Thread.Sleep(YadiskActions.DefaultTimeout);

            AutomationElement browserWindow;

            try
            {
                browserWindow = AutomationElement.FromHandle(browserProcess.MainWindowHandle);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Close all browser windows and relaunch the programm");
                Console.ReadKey();
                return 2;
            }

            try
            {
                YadiskActions.Login(browserWindow, LOGIN, Password);

                //ChooseFile(browserWindow, filePath);
                YadiskActions.DragFileFromExplorer(filePath, ElementFinder.TryFindElement(browserWindow, "последние файлы", "text", YadiskActions.DefaultTimeout).Current.BoundingRectangle.TopLeft);

                YadiskActions.MonitorDownload(browserWindow);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Utilities.CannotOpenExplorerException e)
            {
                Console.WriteLine("Cannot open explorer!");
            }


            Console.WriteLine("Press any key to close the program...");
            Console.ReadKey();
            
            browserProcess.Kill();
            return 0;
        }


    }
}
