using System;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Elements;

namespace Winium.Desktop.Example
{
    class Program
    {
        private static string defaultFolder = @"W:\Education\02_ВУЗ\2014-2016_Магистр_ВМК\4_семестр\python";
        private static string testPageAddress = "https://disk.yandex.ru/";
        private static string fileName = "test.zip";

        #region Login/password to ya disk
        private static string login = "ya.test.py@yandex.ru";
        private static string password = "Qwerty123";
        #endregion

        static void Main(string[] args)
        {
            var chrome = OpenChrome(testPageAddress);
            OpenExplorerInFolder(defaultFolder);

            while (chrome.FindElementByName("Загрузка завершена") == null)
            {
                Thread.Sleep(100);
            }
            MessageBox.Show("Загрузка завершена");
        }

        private static CruciatusElement OpenChrome(string testPageAddress)
        {
            var chr = new Cruciatus.Application(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");
            chr.Start("--force-renderer-accessibility --start-maximized --incognito --app=" + testPageAddress);
            Thread.Sleep(3000);

            var chromeFinder = By.Name("Яндекс.Диск (инкогнито)").AndType(ControlType.Window);
            var chrome = Cruciatus.CruciatusFactory.Root.FindElement(chromeFinder);
            Thread.Sleep(100);

            // --start-maximized
            // mouseMove(chrome.FindElementByUid("TitleBar"), 1366 / 2, 0);

            // --app=" + testPageAddress
            //var address = chrome.FindElementByName("Адресная строка и строка поиска");
            //address.SetText(testPageAddress); Thread.Sleep(100);
            //Cruciatus.CruciatusFactory.Keyboard.SendText("{ENTER}"); Thread.Sleep(100);

            #region Login
            try
            {
                while (true)
                {
                    try
                    {
                        chrome.FindElementByName("Логин").SetText(login); Thread.Sleep(100);
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
            // Сохранить пароль?
            try { chrome.FindElementByName("Нет").SetText(login); Thread.Sleep(100); } catch (Exception) { }
            #endregion

            return chrome;
        }

        private static void OpenExplorerInFolder(string defaultFolder)
        {
            var expl = new Cruciatus.Application(@"C:\Windows\explorer.exe");
            expl.Start();
            Thread.Sleep(3000);

            var winFinder = By.Name("File Explorer").AndType(ControlType.Window);
            var explorer = Cruciatus.CruciatusFactory.Root.FindElement(winFinder);

            mouseMove(explorer.FindElementByUid("TitleBar"), 1300, 100);

            explorer.FindElementByName("All locations").SetText(defaultFolder);
            //explorer.FindElementByUid("1001").SetText(defaultFolder);
            Cruciatus.CruciatusFactory.Keyboard.SendText("{ENTER}");

            mouseMove(explorer.FindElementByName(fileName), 100, 384);
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