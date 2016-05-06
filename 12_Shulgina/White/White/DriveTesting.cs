using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.Factory;
using TestStack.White.InputDevices;
using System.Diagnostics;
using TestStack.White.UIItems.Custom;
using System.Windows.Automation;


using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using TestStack.White.Configuration;
using TestStack.White.Drawing;
using TestStack.White.UIA;
using TestStack.White.UIItems.Actions;
using TestStack.White.WindowsAPI;
using Action = TestStack.White.UIItems.Actions.Action;
using TestStack.White.UIItems.ListBoxItems;




namespace WhiteNamespace
{
    [TestClass]
    class DriveTesting
    {
        [TestMethod]
        public void run(string pathToTestFile)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "chrome.exe",
                Arguments = "--force-renderer-accessibility --incognito https://accounts.google.com/ServiceLogin?service=wise&passive=1209600&continue=https://drive.google.com/?urp%3Dhttps://www.google.ru/_/chrome/newtab?espv%253D2%2526ie%253DUT%23&followup=https://drive.google.com/?urp%3Dhttps://www.google.ru/_/chrome/newtab?espv%253D2%2526ie%253DUT&ltmpl=drive&emr=1#identifier"
            };

            Application appChrome = Application.Launch(psi);
            appChrome.WaitWhileBusy();
            Thread.Sleep(2000);
            Window window = appChrome.GetWindows().FirstOrDefault();

            window.Keyboard.HoldKey(KeyboardInput.SpecialKeys.LWIN);
            window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.UP);
            window.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.LWIN);

            Thread.Sleep(1000);
            window.Keyboard.Enter("teststackwhitetest@gmail.com");
            window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            Thread.Sleep(1000);
            window.Keyboard.Enter("difficultpassword");
            window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            Thread.Sleep(4000);

            

            var firstAppExp = Application.Launch("explorer.exe");
            firstAppExp.Process.WaitForExit();
            Application appExp = Application.Attach("explorer");
            appExp.WaitWhileBusy();
            Thread.Sleep(1000);
            var windowExp = appExp.GetWindows().FirstOrDefault();
            Thread.Sleep(1000);
            window.Focus();
            Thread.Sleep(1000);
            windowExp.Focus();

            windowExp.Keyboard.HoldKey(KeyboardInput.SpecialKeys.LWIN);
            windowExp.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.LEFT);
            windowExp.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.LWIN);
            Thread.Sleep(1000);

            var addressPane = windowExp.Get<Panel>(SearchCriteria.ByAutomationId("41477"));
            addressPane.Click();
            Thread.Sleep(1000);
            windowExp.Keyboard.Enter(pathToTestFile);
            windowExp.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            Thread.Sleep(1000);

            var testfile = windowExp.GetElement(SearchCriteria.ByAutomationId("2"));
            Mouse.Instance.Location = testfile.GetClickablePoint();
            
            Point startPosition = testfile.GetClickablePoint();
            Point endPosition = new Point(window.Bounds.Center().X + 50, window.Bounds.Center().Y + 50);
            drag(startPosition, endPosition);

            Thread.Sleep(40000);
            window.Focus();
            window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.F5);
            Thread.Sleep(5000);
            try
            {
                var testFileItem = window.Get<ListItem>(SearchCriteria.ByText("testFile.zip Compressed Archive"));
                System.Windows.Forms.MessageBox.Show("File was successfully uploaded");
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("File was not uploaded");
            }

            
        }
        private void drag(Point startPosition, Point endPosition)
        {
            Mouse.LeftDown();
            var distanceX = endPosition.X - startPosition.X;
            var distanceY = endPosition.Y - startPosition.Y;

            var distance = Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));
            var stepCount = Math.Ceiling(distance / 10);

            var stepLengthX = distanceX / stepCount;
            var stepLengthY = distanceY / stepCount;

            var currentX = startPosition.X;
            var currentY = startPosition.Y;

            for (var i = 0; i < stepCount; i++)
            {
                currentX += stepLengthX;
                currentY += stepLengthY;
                Mouse.Instance.Location = new Point((int)currentX, (int)currentY);
                Thread.Sleep(30);
            }
            Thread.Sleep(1000);
            Mouse.LeftUp();
        }
    }
}
