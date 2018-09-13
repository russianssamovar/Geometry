using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CADS
{
    public class Triangle //треугольник
    {
        private PointT a, b, c;

        public Triangle(PointT a, PointT b, PointT c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public PointT getA()
        {
            return a;
        }

        public PointT getB()
        {
            return b;
        }

        public PointT getC()
        {
            return c;
        }

        public double GetArea() //подсчет площади треугольника
        {
            //длины трех сторон
            var lineA = _lineLeght(this.a, this.b);
            var lineB = _lineLeght(this.b, this.c);
            var lineC = _lineLeght(this.c, this.a);

            var p = (lineA + lineB + lineC) / 2;
            return Math.Sqrt(p * (p - lineA) * (p - lineB) * (p - lineC)); //площадь по трем сторонам
        }

        private double _lineLeght(PointT a, PointT b) //подсчет длины отрезка ab
        {
            float x = a.X - b.X;
            float y = a.Y - b.Y;
            return Math.Sqrt(x * x + y * y);
        }

    }
}
