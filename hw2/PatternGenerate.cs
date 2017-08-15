using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static hw2.commonStructure;

//sqaure 1000*1000
//20 points
namespace hw2
{
    public class PatternGenerate
    {
        const int testPointsNum = 400;
        List<xyPairs> PointsLi = new List<xyPairs>();

        public List<xyPairs> generateArray()
        {
            int xPairsParam = 0;
            int yPairsParam = 0;
            for (int i = 0; i < testPointsNum*2; i++)
            {
                if (i % 2 == 0)
                {
                    xPairsParam = IntUtil.Random(1, 1000);
                }
                else
                {
                    yPairsParam = IntUtil.Random(1, 1000);
                    //Console.WriteLine("({0}, {1})", xPairsParam, yPairsParam);
                    xyPairs xyStruct = new xyPairs();
                    xyStruct.x = xPairsParam;
                    xyStruct.y = yPairsParam;
                    PointsLi.Add(xyStruct);
                }
            }
            return this.PointsLi;
        }

        public static class IntUtil
        {
            private static Random random;

            private static void Init()
            {
                if (random == null) random = new Random();
            }

            public static int Random(int min, int max)
            {
                Init();
                return random.Next(min, max);
            }
        }

    }

}
