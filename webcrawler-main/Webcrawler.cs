using System.Diagnostics;
using System.Net.Mime;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Threading;
using OpenQA.Selenium; // NuGet: Selenium.WebDriver
using OpenQA.Selenium.Firefox;
using NPOI.XSSF.UserModel; // NuGet: NPOI
using NPOI.SS.UserModel; // NuGet: NPOI
using OpenQA.Selenium.Interactions;

namespace Webcrawler
{
    public static class Webcrawler
    {
        public static string path = Environment.CurrentDirectory;//Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static bool headless = true;

        /// <summary>
        /// Führt das Webcrawling durch
        /// </summary>
        public static void WebcrwalManager()
        {
            var fireOptions = new FirefoxOptions();
            if (headless) fireOptions.AddArgument("--headless");
            fireOptions.SetPreference("browser.download.folderList", 2);
            fireOptions.SetPreference("browser.download.manager.showWhenStarting", false);
            fireOptions.SetPreference("browser.download.dir", path);
            fireOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", true);
            //fireOptions.BrowserExecutableLocation = (@"path\firefox.exe");
            var fire = FirefoxDriverService.CreateDefaultService(Environment.CurrentDirectory);
            using (WebDriver browser = new FirefoxDriver(fire, fireOptions, new TimeSpan(2, 45, 0)))
            {
                //browser.Manage().Window.Maximize();
                browser.Url = "https://url";//write URL here
                browser.Navigate();
                Console.WriteLine("Browser geöffnet");
                var n = 0;
                while (true)
                {
                    var count = 2;
                    MySqlCommand cmd = new MySqlCommand();
                    browser.Navigate().Refresh();
                    Thread.Sleep(7000);
                    while (true)
                    {
                        string pos = string.Format("/html/body/app/layout/div[4]/div[1]/div/auctiongallery/gallery/div/div/div/div[2]/div[2]/div/div[{0}]/lotsmallcontainer/div/div/div[3]/div[1]", count);
                        string pri = string.Format("/html/body/app/layout/div[4]/div[1]/div/auctiongallery/gallery/div/div/div/div[2]/div[2]/div/div[{0}]/lotsmallcontainer/div/div/div[4]/div[2]/div[2]/div[2]/span", count);
                        int price;
                        int position;
                        count++;
                        try
                        {
                            string temp = browser.FindElement(By.XPath(pos)).Text;
                            string temp2 = browser.FindElement(By.XPath(pri)).Text.Substring(2).ToString();
                            price = Int32.Parse(temp2);
                            position = Int32.Parse(temp);
                            cmd.CommandText = string.Format("INSERT INTO table (position, price) VALUES({0}, {1}) AS new ON DUPLICATE KEY UPDATE price=new.price", position, price);
                            UpdatInsert(cmd);
                        }
                        catch
                        {
                            if(count > 100)
                            {
                                break;
                            } else
                            {
                                continue;
                            }
                            
                        }
                    }
                    Console.WriteLine("Iteration " + n.ToString() + " finished.");
                    n++;
                }
            }
        }

        public static MySqlConnection? OpenConnection()
        {
            string realBank;
            realBank = "Database";
            try
            {
                MySqlConnectionStringBuilder c = new MySqlConnectionStringBuilder();
                c.Server = "Server";
                c.Database = realBank;
                c.UserID = "UserID";
                c.Password = "Password";
                c.Port = 3306;
                c.SslMode = MySqlSslMode.Disabled;
                c.AllowPublicKeyRetrieval = true;
                MySqlConnection conn = new MySqlConnection(c.ToString());
                conn.Open();
                return conn;
            }
            catch (MySqlException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return null;
        }

        public static bool UpdatInsert(MySqlCommand cmd)
        {
            try
            {
                var conn = OpenConnection();
                if (conn != null)
                {
                    cmd.Connection = conn;
                    var x = cmd.ExecuteNonQuery();
                    conn.Close();
                    return x != 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return false;
        }
    }
}