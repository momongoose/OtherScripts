using System;
using System.Threading;

namespace Webcrawler
{

    class Programm
    {
        /// /////////////////////////////////////////////////////////////////////// ///
        ///                                                                         ///
        ///                            Webcrawler                                   ///
        ///                                                                         ///
        /// /////////////////////////////////////////////////////////////////////// /// 


        /// <summary>
        /// Main
        /// </summary>
        public static void Main(string[] args)
        {
            Webcrawler.WebcrwalManager();
            Console.WriteLine("Done");
            Thread.Sleep(15000);
        }
    }
}