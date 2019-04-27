using System;
using System.Diagnostics;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeachPatents
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();//监听耗时
            stopwatch.Start();
            IWebDriver driver = new ChromeDriver(Directory.GetCurrentDirectory());
            driver.Navigate().GoToUrl("https://patentscope.wipo.int/search/zh/search.jsf");
            driver.Manage().Window.Maximize();//窗口最大化，便于脚本执行
            //设置超时等待(隐式等待)时间设置10秒
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Console.Write("输入需要搜索的关键字:");
            string searchKeyWord = Console.ReadLine();//搜索查询值
            //输入
            driver.FindElement(By.Id("simpleSearchForm:fpSearch")).SendKeys(searchKeyWord);
            //搜索
            driver.FindElement(By.Id("simpleSearchForm:commandSimpleFPSearch")).Click();

            ShowResult(driver);

            do
            {
                Console.Write("是否继续？（yes/no）");
                if (Console.ReadLine().ToLower()=="no")
                {
                    break;
                }

                Console.Write("请输入需要跳转的页数:");
                string page = Console.ReadLine();
                ShowResult(driver, page);

            } while (true);
   
            

        }
        private static void ShowResult(IWebDriver driver,string page="")
        {
            if (!string.IsNullOrWhiteSpace(page))
            {
                int page_num;
                if (int.TryParse(page, out page_num))
                {

                    //填写值，并且跳转
                    driver.FindElement(By.Id("resultListFormTop:inputGoToPage")).Clear();//先清空
                    driver.FindElement(By.Id("resultListFormTop:inputGoToPage")).SendKeys(page);
                    driver.FindElement(By.Id("resultListFormTop:linkGoToPage")).Click();
                }
            }

     
            //显示结果
            Console.WriteLine(driver.FindElement(By.Id("resultListFormTop:resultPanel0")).Text);
            //显示当前页数
            Console.WriteLine($"当前页数:{driver.FindElement(By.Id("resultListFormTop:inputGoToPage")).GetAttribute("value")}");
            Console.WriteLine("***********************搜索结果***********************************");
            Console.WriteLine(driver.FindElement(By.Id("resultListForm:resultTable")).Text);
      


        }
    }
}
