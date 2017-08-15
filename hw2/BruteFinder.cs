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
    class BruteFinder
    {
        closetTwoPoints ctpObj = new closetTwoPoints();

        public closetTwoPoints BruteForceFindClosetPairs(List<xyPairs> PointsLi)
        {
            
            this.ctpObj.dmin = 10000.0;   //max value

            for (int i = 0; i < PointsLi.Count; i++)
            {
                for (int j = 0; j < PointsLi.Count; j++)
                {
                    if (j == i)
                        continue;
                    else
                    {
                        double d = this.ComputeDistanceBetweenPoints(PointsLi[i], PointsLi[j]);
                        if (d < this.ctpObj.dmin)
                            this.updateClosetTwoPoints(PointsLi[i], PointsLi[j], d);
                    }
                }
            }
            return ctpObj;
        }

        //compute d
        private double ComputeDistanceBetweenPoints(xyPairs p1, xyPairs p2)
        {
            double distance = 0;
            distance = Math.Sqrt((int)(Math.Pow((p1.x - p2.x), 2) + Math.Pow((p1.y - p2.y), 2)));
            return distance;
        }

        private void updateClosetTwoPoints(xyPairs p1, xyPairs p2, double d)
        {
            //Console.WriteLine(d);
            this.ctpObj.dmin = d;
            this.ctpObj.point1_x = p1.x;
            this.ctpObj.point1_y = p1.y;
            this.ctpObj.point2_x = p2.x;
            this.ctpObj.point2_y = p2.y;
        }
    }
}
