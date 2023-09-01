namespace Unconventional_galery
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            using (Galery galery = new Galery(1920, 1080, "Test"))
            {
                galery.Run();
            }
        }
    }
}