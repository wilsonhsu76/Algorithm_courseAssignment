using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static hw2.commonStructure;

namespace hw2
{
    class DCFinder
    {
        const double testFieldWidth = 1000.0;
        const double testFieldHeight = 1000.0;
        List<xyPairs> orderX_Li = new List<xyPairs>();
        List<xyPairs> orderY_Li = new List<xyPairs>();
        closetTwoPoints ctpObj = new closetTwoPoints();

        public void InitialXandYLis(List<xyPairs> PointsLi)
        {
            this.InitialOrderX_Li(PointsLi);
            this.InitialOrderY_Li(PointsLi);
            this.ctpObj.dmin = testFieldWidth*10;
        }

        public closetTwoPoints FindClosetPairs()
        {
            if (orderX_Li.Count > 3)
            {
                DCFinder leftRecursiveDCFObj = new DCFinder();
                DCFinder rightRecursiveDCFObj = new DCFinder();

                //divide points into 2 sub-points
                List<xyPairs> leftSubOrderX_Li = orderX_Li.GetRange(0, (orderX_Li.Count + 1) / 2);
                List<xyPairs> rightSubOrderX_Li = orderX_Li.GetRange(((orderX_Li.Count + 1) / 2), orderX_Li.Count -  ((orderX_Li.Count + 1) / 2));
                double line_x = (double)(leftSubOrderX_Li[leftSubOrderX_Li.Count - 1].x + rightSubOrderX_Li[0].x)/2.0;
                leftRecursiveDCFObj.orderX_Li = leftSubOrderX_Li;
                rightRecursiveDCFObj.orderX_Li = rightSubOrderX_Li;


                //also produce y-points group for merge-consideration
                List<xyPairs> leftSubOrderY_Li = new List<xyPairs>();
                List<xyPairs> rightSubOrderY_Li = new List<xyPairs>();
                write_Y_PointsToSubPoints(leftSubOrderY_Li, rightSubOrderY_Li, leftSubOrderX_Li, rightSubOrderX_Li);
                leftRecursiveDCFObj.orderY_Li = leftSubOrderY_Li;
                rightRecursiveDCFObj.orderY_Li = rightSubOrderY_Li;

                //handle the sub-points
                closetTwoPoints leftCtpObj = leftRecursiveDCFObj.FindClosetPairs();
                closetTwoPoints rightCtpObj = rightRecursiveDCFObj.FindClosetPairs();

                //combine result
                //First, consider left and right group's result
                if (leftCtpObj.dmin <= rightCtpObj.dmin)
                    this.ctpObj = leftCtpObj;
                else
                    this.ctpObj = rightCtpObj;
                
                //Then, consider nearby 2d region near the x-line
                //the height should take care in 1.5d for each points
                double d_regionL = this.ctpObj.dmin;

                double x_leftBound = line_x - d_regionL;
                if (x_leftBound < 0.0)
                    x_leftBound = 0.0;

                double x_rightBound = line_x + d_regionL;
                if (x_rightBound > testFieldWidth)
                    x_rightBound = testFieldWidth;

                double y_offset = 1.5 * d_regionL;

                //take care left side first (from bottom to top)
                for (int i = 0; i < leftSubOrderY_Li.Count; i++)
                {
                    if (((double)(leftSubOrderY_Li[i].x) >= x_leftBound))
                    {
                        for(int j = 0; j< rightSubOrderY_Li.Count; j++)
                        {
                            bool condition1 = ((double)(rightSubOrderY_Li[j].x) <= x_rightBound);
                            bool condition2 = ((double)(rightSubOrderY_Li[j].y) >= leftSubOrderY_Li[i].y);
                            bool condition3 = ((double)(rightSubOrderY_Li[j].y) <= leftSubOrderY_Li[i].y + y_offset);
                            if (condition1 && condition2 && condition3)
                            {
                                double d = this.ComputeDistanceBetweenPoints(leftSubOrderY_Li[i], rightSubOrderY_Li[j]);
                                if (d < this.ctpObj.dmin)
                                {
                                    this.updateClosetTwoPoints(leftSubOrderY_Li[i], rightSubOrderY_Li[j], d);
                                    //Console.WriteLine("update case1 {0}", d);
                                }
                            }
                            else
                                continue;
                        }
                    }
                    else
                        continue;
                }

                //take care right side (from bottom to top)
                for (int i = 0; i < rightSubOrderY_Li.Count; i++)
                {
                    if ((double)(rightSubOrderY_Li[i].x) <= x_rightBound)
                    {
                        for (int j = 0; j < leftSubOrderY_Li.Count; j++)
                        {
                            bool condition1 = ((double)(leftSubOrderY_Li[j].x) >= x_leftBound);
                            bool condition2 = ((double)(leftSubOrderY_Li[j].y) >= rightSubOrderY_Li[i].y);
                            bool condition3 = ((double)(leftSubOrderY_Li[j].y) <= rightSubOrderY_Li[i].y + y_offset);
                            if (condition1 && condition2 && condition3)
                            {
                                double d = this.ComputeDistanceBetweenPoints(rightSubOrderY_Li[i], leftSubOrderY_Li[j]);
                                if (d < this.ctpObj.dmin)
                                {
                                    this.updateClosetTwoPoints(leftSubOrderY_Li[j], rightSubOrderY_Li[i], d);
                                    //Console.WriteLine("update case2 {0}", d);
                                }
                            }
                            else
                                continue;
                        }
                    }
                    else
                        continue;
                }


                //Console.WriteLine(this.ctpObj.dmin);
                return  this.ctpObj;
            }

            else
            {
                //points less than 3, directly use brute force to calculate
                this.ctpObj.dmin = testFieldWidth*10;   //max value
                for (int i = 0; i < orderX_Li.Count; i++)
                {
                    for (int j = 0; j < orderX_Li.Count; j++)
                    {
                        if (j == i)
                            continue;
                        else
                        {
                            double d = this.ComputeDistanceBetweenPoints(orderX_Li[i], orderX_Li[j]);
                            if (d < this.ctpObj.dmin)
                                this.updateClosetTwoPoints(orderX_Li[i], orderX_Li[j], d);

                        }
                    }
                }
                //Console.WriteLine(ctpObj.dmin);
                return this.ctpObj;
            }
        }

        private void InitialOrderX_Li(List<xyPairs> PointsLi)
        {
            this.orderX_Li = PointsLi.ToList();
            this.orderX_Li = this.orderX_Li.OrderBy(element => element.x).ToList();
            return;
        }

        private void InitialOrderY_Li(List<xyPairs> PointsLi)
        {
            this.orderY_Li = PointsLi.ToList();
            this.orderY_Li = this.orderY_Li.OrderBy(element => element.y).ToList();
            return;
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
            this.ctpObj.dmin = d;
            this.ctpObj.point1_x = p1.x;
            this.ctpObj.point1_y = p1.y;
            this.ctpObj.point2_x = p2.x;
            this.ctpObj.point2_y = p2.y;
        }

        private void write_Y_PointsToSubPoints(List<xyPairs> leftSubOrderY_Li, List<xyPairs> rightSubOrderY_Li, List<xyPairs> leftSubOrderX_Li, List<xyPairs> rightSubOrderX_Li)
        {
            for (int i = 0; i < this.orderY_Li.Count; i++)
            {   //condition2 decides the same (x,y) points location.
                //# points in leftSubOrderX_Li = # points in leftSubOrderY_Li
                if ((leftSubOrderX_Li.Exists(point => (point.x == orderY_Li[i].x) && (point.y == orderY_Li[i].y)))
                    && (leftSubOrderX_Li.Count(point => (point.x == orderY_Li[i].x) && (point.y == orderY_Li[i].y)) > 
                    (leftSubOrderY_Li.Count(point => (point.x == orderY_Li[i].x) && (point.y == orderY_Li[i].y)))) )
                    leftSubOrderY_Li.Add(orderY_Li[i]);
                else
                    rightSubOrderY_Li.Add(orderY_Li[i]);
            }
            return;
        }

    }
}
