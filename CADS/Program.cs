using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CADS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество вершин многоугольника");
            var count = Convert.ToInt32(Console.ReadLine());


            var poligonT = new PointT[count];

            for (var i = 0; i < count; i++)
            {
                Console.WriteLine("X"+i+":");
                var x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Y" + i + ":");
                var y = Convert.ToInt32(Console.ReadLine());

                var point = new PointT(x,y);
                poligonT[i] = point;
            }

           



            var poligon = new Polygon(poligonT);
            Console.WriteLine("Площадь: ");
            Console.WriteLine(poligon.GetArea());
            Console.ReadLine();
        }
    }
}
