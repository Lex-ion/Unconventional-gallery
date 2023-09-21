using System.Collections;
using System.Globalization;
using System.Resources;

namespace Unconventional_galery
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            if(!Directory.Exists(Data.TexturesPath)||!Directory.Exists("Shaders")||!Directory.Exists("Data"))
                StartConfig();

            using (Gallery galery = new Gallery(1920, 1080, "Test"))
            {
                galery.Run();
            }
        }

        static void StartConfig()
        {
            

            Directory.CreateDirectory(Data.TexturesPath);
            Directory.CreateDirectory("Data");
            Directory.CreateDirectory("Shaders");

           // File.WriteAllBytes("Shaders/vertShader.vert", Properties.Resources.vertShader);
           // File.WriteAllBytes("Shaders/fragShader.frag", Properties.Resources.fragShader);

            ResourceManager MyResourceClass = new ResourceManager(typeof(Properties.Resources));

            ResourceSet resourceSet =  MyResourceClass.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                object resource = entry.Value;



                if (resourceKey.Contains("Shader"))
                {
                    File.WriteAllBytes($"Shaders/{resourceKey}.{resourceKey.Remove(4)}", (byte[])resource);
                }
                else if (resourceKey.Contains("_"))
                {
                    File.WriteAllBytes($"{Data.TexturesPath}/{resourceKey}.png", (byte[])resource);
                }
                else
                   if (resource is byte[])
                    File.WriteAllBytes($"Data/{resourceKey}.txt", (byte[])resource);
                else
                    File.WriteAllText($"Data/{resourceKey}.txt", (string)resource);

            }
        }
    }
}