using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Press any key to request the Sort service...");
        Console.ReadKey();
        Service1 service = new Service1();
        var resut = service.Sort(new int[] {5, 1, 10});
        Array.ForEach(resut, Console.WriteLine);
        Console.ReadKey();
    }
}
