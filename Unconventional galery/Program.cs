namespace Unconventional_galery
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            using (Gallery galery = new Gallery(1920, 1080, "Test"))
            {
                galery.Run();
            }
        }
    }
}