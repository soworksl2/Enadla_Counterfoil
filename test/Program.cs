using System;
using Enadla_Counterfoil;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            EnadlaCounterfoil counterfoil = new EnadlaCounterfoil("./test.ecf");

            counterfoil.SetData("test", null);

            Console.WriteLine("Hello World!");
        }
    }
}
