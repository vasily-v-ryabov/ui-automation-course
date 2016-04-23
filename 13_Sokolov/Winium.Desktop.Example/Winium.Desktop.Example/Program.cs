using System;
using System.IO;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Elements;

namespace Winium.Desktop.Example
{
    class Program
    {
        private static string defaultFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private static string testPageAddress = "https://disk.yandex.ru/";
        private static string fileName = "test.zip";

        #region Login/password to ya disk
        private static string login = "ya.test.py@yandex.ru";
        private static string password = "Qwerty123";
        #endregion

        /// <summary>
        /// Before run:
        /// 1) If you have 'test.zip' in https://disk.yandex.ru/ test account - remove it.
        /// 2) Log out from all accounts from https://disk.yandex.ru/
        /// 3) Close all google chrome windows
        /// </summary>
        static void Main(string[] args)
        {
            var chrome = OpenChrome(testPageAddress);
            OpenExplorerInFolder(defaultFolder);

            while (chrome.FindElementByName("Загрузка завершена") == null)
            {
                Thread.Sleep(100);
            }
            MessageBox.Show("Загрузка завершена");

            Thread.Sleep(1000);
            chrome.SetFocus();
            Thread.Sleep(1000);
            chrome.FindElementByName("System").DoubleClick();
        }

        private static Cruciatus.Application chromeWindow = null;
        private static CruciatusElement OpenChrome(string testPageAddress)
        {
            var chromeWindow = new Cruciatus.Application(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");
            chromeWindow.Start("--force-renderer-accessibility --start-maximized --app=" + testPageAddress);
            Thread.Sleep(3000);

            var chromeFinder = By.Name("Яндекс.Диск").AndType(ControlType.Window);
            var chrome = Cruciatus.CruciatusFactory.Root.FindElement(chromeFinder);
            Thread.Sleep(100);

            #region Login
            try
            {
                while (true)
                {
                    try
                    {
                        CruciatusElement ce = chrome.FindElementByName("Логин");
                        while (!ce.Text().Equals("Логин"))
                        {
                            ce.Click();
                            String backspace = "{BACKSPACE}";
                            String delete = "{DELETE}";
                            String clearstr = "";
                            for (int i = 0; i < ce.Text().Length; i++) clearstr += backspace + delete;

                            Cruciatus.CruciatusFactory.Keyboard.SendText(clearstr);
                        }
                        ce.SetText(login); Thread.Sleep(100);
                        break;
                    }
                    catch (Exception) { }
                }
                chrome.FindElementByName("Пароль").SetText(password); Thread.Sleep(100);
                chrome.FindElementByName("Войти ").Click(); 
                Thread.Sleep(3000);
            } catch (Exception) { Console.WriteLine("Logged"); }
            #endregion

            #region Close helpers
            // Save password?
            try { chrome.FindElementByName("Нет").Click(); Thread.Sleep(100); } catch (Exception) { }
            // Install yandex disk on your computer
            // try { chrome.FindElementByName("Имя ").Click(); Thread.Sleep(100); } catch (Exception) { }
            #endregion

            return chrome;
        }

        private static void OpenExplorerInFolder(string defaultFolder)
        {
            Cruciatus.Application expl = new Cruciatus.Application(@"C:\Windows\explorer.exe");
            expl.Start(defaultFolder);
            Thread.Sleep(1000);

            var winFinder = By.Name("Winium.Desktop.Example").AndType(ControlType.Window);
            var explorer = Cruciatus.CruciatusFactory.Root.FindElement(winFinder);

            mouseMove(explorer.FindElementByUid("TitleBar"), 1300, 100);

            CruciatusElement items = explorer.FindElementByName("Items View");
            mouseMove(items.FindElementByName(fileName), 100, 384);
            Thread.Sleep(1000);

            explorer.SetFocus();
            Thread.Sleep(1000);
            explorer.FindElementByName("System").DoubleClick();
        }

        private static void mouseMove(CruciatusElement ci, double x1, double y1)
        {
            var rect = ci.Properties.BoundingRectangle;
            double x0 = rect.X + rect.Width / 2;
            double y0 = rect.Y + rect.Height / 2;
            mouseMove(x0, y0, x1, y1);
        }
        private static void mouseMove(double x0, double y0, double x1, double y1)
        {
            Cruciatus.CruciatusFactory.Mouse.SetCursorPos(x0, y0);
            Thread.Sleep(500);
            MouseSimulator.LeftMouseButtonDown();
            Thread.Sleep(1000);

            int steps = 5;
            for (int i = 0; i<= steps; i++)
            {
                Cruciatus.CruciatusFactory.Mouse.SetCursorPos(x0 + (x1 - x0) * i / steps, y0 + (y1 - y0) * i / steps);
            }
            Thread.Sleep(1000);
            MouseSimulator.LeftMouseButtonUp();
            Thread.Sleep(500);
        }
    }
}