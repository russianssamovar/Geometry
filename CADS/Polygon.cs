using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CADS
{
    public struct PointT
    {
       public PointT(int x, int y, bool intersectPoint = false)
        {
            X = x;
            Y = y;
            IntersectPoint = intersectPoint;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public bool IntersectPoint { get; set; }
    }

    class Polygon
    {

        
        private PointT[] _points; //вершины нашего многоугольника
        private Triangle[] _triangles; //треугольники, на которые разбит наш многоугольник
        private bool[] _taken; //была ли рассмотрена i-ая вершина многоугольника

        public Polygon(PointT[] points) //points - х и y координаты
        {
            _points = points;


            _triangles = new Triangle[this._points.Length - 2];

            _taken = new bool[this._points.Length];

            _triangulate(); //триангуляция
        }



        private void _triangulate() //триангуляция
        {
            var trainPos = 0; //
            var leftPoints = _points.Length; //сколько осталось рассмотреть вершин

            //текущие вершины рассматриваемого треугольника
            var ai = _findNextNotTaken(0);
            var bi = _findNextNotTaken(ai + 1);
            var ci = _findNextNotTaken(bi + 1);

            var count = 0; //количество шагов

            while (leftPoints > 3) //пока не остался один треугольник
            {
                if ( _canBuildTriangle(ai, bi, ci)) //если можно построить треугольник
                {
                    _triangles[trainPos++] = new Triangle(_points[ai], _points[bi], _points[ci]); //новый треугольник
                    _taken[bi] = true; //исключаем вершину b
                    leftPoints--;
                    bi = ci;
                    ci = _findNextNotTaken(ci + 1); //берем следующую вершину
                }
                else
                { //берем следующие три вершины
                    ai = _findNextNotTaken(ai + 1);
                    bi = _findNextNotTaken(ai + 1);
                    ci = _findNextNotTaken(bi + 1);
                }

                if (count > _points.Length * _points.Length )
                { //если по какой-либо причине триангуляцию провести невозможно, выходим
                    _triangles = null;
                    break;
                }

                count++;
            }
            if (_points.Length == 3 )
            {
                _triangles[0] = new Triangle(_points[ai], _points[bi], _points[ci]);
            }
            if (_triangles != null) //если триангуляция была проведена успешно
                _triangles[trainPos] = new Triangle(_points[ai], _points[bi], _points[ci]);
        }

        private int _findNextNotTaken(int startPos) //найти следущую нерассмотренную вершину
        {
            startPos %= _points.Length;
            if (!_taken[startPos])
                return startPos;

            int i = (startPos + 1) % _points.Length;
            while (i != startPos)
            {
                if (!_taken[i])
                    return i;
                i = (i + 1) % _points.Length;
            }
            return -1;
        }


        private bool _isPointInside(PointT a, PointT b, PointT c, PointT p) //находится ли точка p внутри треугольника abc
        {
            float ab = (a.X - p.X) * (b.Y - a.Y) - (b.X - a.X) * (a.Y - p.Y);
            float bc = (b.X - p.X) * (c.Y - b.Y) - (c.X - b.X) * (b.Y - p.Y);
            float ca = (c.X - p.X) * (a.Y - c.Y) - (a.X - c.X) * (c.Y - p.Y);

            return (ab >= 0 && bc >= 0 && ca >= 0) || (ab <= 0 && bc <= 0 && ca <= 0);
        }

        private bool _canBuildTriangle(int ai, int bi, int ci) //false - если внутри есть вершина
        {
            for (int i = 0; i < _points.Length; i++) //рассмотрим все вершины многоугольника
                if (i != ai && i != bi && i != ci) //кроме троих вершин текущего треугольника
                    if (_isPointInside(_points[ai], _points[bi], _points[ci], _points[i]))
                        return false;
            return true;
        }


        public double GetArea() //подсчет площади
        {
            return _triangles?.Sum(t => t.GetArea()) ?? 0;
        }

        private PointT? _getIntersectionPoint(PointT l1P1, PointT l1P2, PointT l2P1, PointT l2P2)
        {
            var a1 = l1P2.Y - l1P1.Y;
            var b1 = l1P1.X - l1P2.X;
            var c1 = a1 * l1P1.X + b1 * l1P1.Y;

            var a2 = l2P2.Y - l2P1.Y;
            var b2 = l2P1.X - l2P2.X;
            var c2 = a2 * l2P1.X + b2 * l2P1.Y;

            var det = a1 * b2 - a2 * b1;

                var x = (b2 * c1 - b1 * c2) / det;
                var y = (a1 * c2 - a2 * c1) / det;
                var online1 = (Math.Min(l1P1.X, l1P2.X) < x || Math.Min(l1P1.X, l1P2.X) == x)
                                && (Math.Max(l1P1.X, l1P2.X) > x || Math.Max(l1P1.X, l1P2.X) == x)
                                && (Math.Min(l1P1.Y, l1P2.Y) < y || Math.Min(l1P1.Y, l1P2.Y) == y)
                                && (Math.Max(l1P1.Y, l1P2.Y) > y || Math.Max(l1P1.Y, l1P2.Y) == y);

                var online2 = ((Math.Min(l2P1.X, l2P2.X) < x || Math.Min(l2P1.X, l2P2.X)==x))
                                && (Math.Max(l2P1.X, l2P2.X) > x || Math.Max(l2P1.X, l2P2.X)== x)
                                && (Math.Min(l2P1.Y, l2P2.Y) < y || Math.Min(l2P1.Y, l2P2.Y)== y)
                                && (Math.Max(l2P1.Y, l2P2.Y) > y || Math.Max(l2P1.Y, l2P2.Y)== y);

                if (online1 && online2)
                    return new PointT(x, y);

            return null; 
        }

        public void GetUnion(Polygon secondPolygon)
        {
            var unionPoligon = new List<PointT>();
            foreach (var pt in secondPolygon._points)
            {
                if (!_pointInsertPoligon(_triangles,pt))
                {
                    unionPoligon.Add(pt);
                }
            }
            foreach (var pt in _points)
            {
                if (!_pointInsertPoligon(secondPolygon._triangles, pt))
                {
                    unionPoligon.Add(pt);
                }
            }


        }

        private bool _pointInsertPoligon(IEnumerable<Triangle> triangles, PointT pt)
        {
            foreach (var triangle in triangles)
            {
                if (_isPointInside(triangle.getA(),triangle.getB(),triangle.getC(),pt))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
