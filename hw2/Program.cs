using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static hw2.commonStructure;
using System.IO;
using System.Diagnostics;

namespace hw2
{
    class Program
    {
        static void Main(string[] args)
        {
            PatternGenerate PGObj = new PatternGenerate();
            List<xyPairs> testPointLi = PGObj.generateArray();
            /*List<xyPairs> testPointLi = new List<xyPairs>();
            xyPairs u1 = new xyPairs(); u1.x = 906; u1.y = 39; testPointLi.Add(u1);
            xyPairs u2 = new xyPairs(); u2.x = 559; u2.y = 132; testPointLi.Add(u2);
            xyPairs u3 = new xyPairs(); u3.x = 97; u3.y = 39; testPointLi.Add(u3);
            xyPairs u4 = new xyPairs(); u4.x = 703; u4.y = 868; testPointLi.Add(u4);
            xyPairs u5 = new xyPairs(); u5.x = 416; u5.y = 936; testPointLi.Add(u5);
            xyPairs u6 = new xyPairs(); u6.x = 279; u6.y = 289; testPointLi.Add(u6);
            xyPairs u7 = new xyPairs(); u7.x = 543; u7.y = 261; testPointLi.Add(u7);
            xyPairs u8 = new xyPairs(); u8.x = 461; u8.y = 308; testPointLi.Add(u8);
            xyPairs u9 = new xyPairs(); u9.x = 244; u9.y = 639; testPointLi.Add(u9);
            xyPairs u10 = new xyPairs();u10.x = 667; u10.y = 768; testPointLi.Add(u10);*/

            Stopwatch sw = new Stopwatch(); //using System.Diagnostics;
            sw.Reset(); //initial
            sw.Start();

            BruteFinder BFObj = new BruteFinder();
            closetTwoPoints testCptObj = BFObj.BruteForceFindClosetPairs(testPointLi);
            Console.WriteLine("({0},{1}) and ({2},{3}), BruteForce method Result: dmin={4})",
                testCptObj.point1_x, testCptObj.point1_y, testCptObj.point2_x, testCptObj.point2_y, testCptObj.dmin);

            sw.Stop();
            Console.WriteLine("Brute Mathod : {0} ms", sw.Elapsed.TotalMilliseconds);


            /*StreamWriter sw = new StreamWriter(@"D:\testPattern.txt");
            for(int i = 0; i< testPointLi.Count; i++)
            {
                sw.WriteLine("({0},{1})", testPointLi[i].x, testPointLi[i].y);
            }
            sw.Close();*/

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Below is DCF log:");



            Stopwatch sw2 = new Stopwatch(); //using System.Diagnostics;
            sw2.Reset(); //initial
            sw2.Start();

            DCFinder DCFObj = new DCFinder();
            DCFObj.InitialXandYLis(testPointLi);
            closetTwoPoints DCF_CptObj = DCFObj.FindClosetPairs();
            Console.WriteLine("({0},{1}) and ({2},{3}), divide and Conquer method Result: dmin={4})", 
                DCF_CptObj.point1_x, DCF_CptObj.point1_y, DCF_CptObj.point2_x, DCF_CptObj.point2_y, DCF_CptObj.dmin);

            sw2.Stop();
            Console.WriteLine("DCF Mathod : {0} ms", sw2.Elapsed.TotalMilliseconds);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
