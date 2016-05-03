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

namespace WhiteNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            
            DriveTesting test = new DriveTesting();
            test.run("C:\\Users\\USER\\Desktop\\White");
        }
    }
}
