using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace Unconventional_galery
{
    internal class Data
    {

        public static List<GameObject> objectPoints = new List<GameObject>();

        public static List<GameObject> MapLoader(Camera camera)
        {
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            List<GameObject> objects = new List<GameObject>();

            string[] raw = File.ReadAllText("Data/map.txt").Replace("\n","").Replace("\r","").Split("NEW");
            raw = raw.Skip(1).ToArray();
            foreach (string data in raw) {
                string[] head = data.Split("VERTICES")[0].Split("INDICES")[0].Split(",");
                string[] vertData = data.Split("VERTICES")[1].Split(",");
                
                


                string[] wscRaw = head[2].Split(":")[1].Split("|");
               OpenTK.Mathematics.Vector3 wsc =new OpenTK.Mathematics.Vector3(float.Parse(wscRaw[0].Replace("f",""), System.Globalization.NumberStyles.Any, ci), float.Parse(wscRaw[1].Replace("f",""), System.Globalization.NumberStyles.Any, ci), float.Parse(wscRaw[2].Replace("f",""), System.Globalization.NumberStyles.Any, ci));

                string[] wsrRaw = head[3].Split(":")[1].Split("|");
                OpenTK.Mathematics.Vector3 wsr = new OpenTK.Mathematics.Vector3(float.Parse(wsrRaw[0].Replace("f",""), System.Globalization.NumberStyles.Any, ci), float.Parse(wsrRaw[1].Replace("f",""), System.Globalization.NumberStyles.Any, ci), float.Parse(wsrRaw[2].Replace("f",""), System.Globalization.NumberStyles.Any, ci));

                string[] scaleRaw= head[4].Split(":")[1].Split("|");
                OpenTK.Mathematics.Vector3 scale = new OpenTK.Mathematics.Vector3(float.Parse(scaleRaw[0].Replace("f",""), System.Globalization.NumberStyles.Any, ci), float.Parse(scaleRaw[1].Replace("f",""), System.Globalization.NumberStyles.Any, ci), float.Parse(scaleRaw[2].Replace("f",""), System.Globalization.NumberStyles.Any, ci));

                int textureOverride = -1;
                if (head.Length > 5) {
                     textureOverride =Convert.ToInt32( head[5].Split(":")[1]);
                }

                

               

                

                float[] vertices = new float[vertData.Length];
                for (int i = 0; i < vertData.Length; i++)
                {
                    vertices[i] = float.Parse(vertData[i].Replace("f",""),System.Globalization.NumberStyles.Any,ci);
                }
                objects.Add(new GameObject(camera,vertices, head[0].Split(":")[1], (GameObjectType)Convert.ToInt32( head[1].Split(":")[1]),wsc,wsr,scale,textureOverride));
            }

            return objects;
        }

        public static string Editor(Camera camera)
        {
            

                
                    string Error()
                    {
                        return "error";
                    }
                    string output = "";

                    OpenTK.Mathematics.Vector3 RoundVector(OpenTK.Mathematics.Vector3 vector3, int decimals)
                    {
                        return new OpenTK.Mathematics.Vector3((float)Math.Round(vector3.X, decimals,MidpointRounding.AwayFromZero), (float)Math.Round(vector3.Y, decimals, MidpointRounding.AwayFromZero), (float)Math.Round(vector3.Z,decimals, MidpointRounding.AwayFromZero));
                    }


            Console.WriteLine("Enter debug key:");
            string debugKey = Console.ReadLine();


            string inputFromConsole;

            int gameObjectType;
            do
            {
                Console.WriteLine("Enter valid gameObjectType");
                inputFromConsole = Console.ReadLine();
            } while (!int.TryParse(inputFromConsole, out gameObjectType)||!Enum.IsDefined(typeof(GameObjectType),gameObjectType)) ;

            int gameObjectTypeOverride;
            Console.WriteLine("Do you wish to override? [Y]es anything else is no");
            if(gameObjectType==-1)
            { Console.WriteLine("Override is needed"); }
            if (gameObjectType==-1||Console.ReadKey(true).KeyChar == 'y')
            {
                do
                {
                    Console.WriteLine("Enter valid gameObjectType for override");
                    inputFromConsole = Console.ReadLine();
                } while (!int.TryParse(inputFromConsole, out gameObjectTypeOverride) || !Enum.IsDefined(typeof(GameObjectType), gameObjectTypeOverride)|| gameObjectTypeOverride==-1);
            }


            int selection = 0;
            bool selecting = true;
            Console.WriteLine("Create [C]uboid or [P]lane?");
            do
            {
              
                switch (Console.ReadKey(true).KeyChar)
                {
                    case 'c':
                        selection = 1;
                        selecting = false;
                        Console.WriteLine("Selected cuboid");
                        break;

                    case 'p':
                        selection = 2;
                        selecting = false;
                        Console.WriteLine("Selected plane");
                        break;
                }


            } while (selecting);
                

                    

            int decimals;
            do
            {
                Console.WriteLine("Set rounding (0 - 15)");
                inputFromConsole = Console.ReadLine();
            } while (!int.TryParse(inputFromConsole,out decimals)||decimals<0||decimals>15);



                     Console.WriteLine("Set 2 or more vertexes. For adding fly to point and type to console add. When you are done press enter");
            
                    List < OpenTK.Mathematics.Vector3 > vertices = new List<OpenTK.Mathematics.Vector3>();
                   


                    Console.Write("Current position: ");
                    int[] cursorPos = { Console.CursorLeft, Console.CursorTop };
                    OpenTK.Mathematics.Vector3 lastDisplayedVector = new OpenTK.Mathematics.Vector3();
                    OpenTK.Mathematics.Vector3 midPoint = new OpenTK.Mathematics.Vector3();
            float[] cube = new float[]
                 {
                                //back
                                -1.0f, -1.0f, -1.0f,  1.0f, 0.0f,
                                 1.0f, -1.0f, -1.0f,  0.0f, 0.0f,
                                 1.0f,  1.0f, -1.0f,  0.0f, 1.0f,
                                 1.0f,  1.0f, -1.0f,  0.0f, 1.0f,
                                -1.0f,  1.0f, -1.0f,  1.0f, 1.0f,
                                -1.0f, -1.0f, -1.0f,  1.0f, 0.0f,

                                //front
                                -1.0f, -1.0f,  1.0f,  0.0f, 0.0f,
                                 1.0f, -1.0f,  1.0f,  1.0f, 0.0f,
                                 1.0f,  1.0f,  1.0f,  1.0f, 1.0f,
                                 1.0f,  1.0f,  1.0f,  1.0f, 1.0f,
                                -1.0f,  1.0f,  1.0f,  0.0f, 1.0f,
                                -1.0f, -1.0f,  1.0f,  0.0f, 0.0f,

                                //left
                                -1.0f,  1.0f,  1.0f,  1.0f, 1.0f,
                                -1.0f,  1.0f, -1.0f,  0.0f, 1.0f,
                                -1.0f, -1.0f, -1.0f,  0.0f, 0.0f,
                                -1.0f, -1.0f, -1.0f,  0.0f, 0.0f,
                                -1.0f, -1.0f,  1.0f,  1.0f, 0.0f,
                                -1.0f,  1.0f,  1.0f,  1.0f, 1.0f,

                                //right
                                 1.0f,  1.0f,  1.0f,  0.0f, 1.0f,
                                 1.0f,  1.0f, -1.0f,  1.0f, 1.0f,
                                 1.0f, -1.0f, -1.0f,  1.0f, 0.0f,
                                 1.0f, -1.0f, -1.0f,  1.0f, 0.0f,
                                 1.0f, -1.0f,  1.0f,  0.0f, 0.0f,
                                 1.0f,  1.0f,  1.0f,  0.0f, 1.0f,

                                 //bottom
                                -1.0f, -1.0f, -1.0f,  1.0f, 1.0f,
                                 1.0f, -1.0f, -1.0f,  0.0f, 1.0f,
                                 1.0f, -1.0f,  1.0f,  0.0f, 0.0f,
                                 1.0f, -1.0f,  1.0f,  0.0f, 0.0f,
                                -1.0f, -1.0f,  1.0f,  1.0f, 0.0f,
                                -1.0f, -1.0f, -1.0f,  1.0f, 1.0f,

                                //top
                                -1.0f,  1.0f, -1.0f,  0.0f, 1.0f,
                                 1.0f,  1.0f, -1.0f,  1.0f, 1.0f,
                                 1.0f,  1.0f,  1.0f,  1.0f, 0.0f,
                                 1.0f,  1.0f,  1.0f,  1.0f, 0.0f,
                                -1.0f,  1.0f,  1.0f,  0.0f, 0.0f,
                                -1.0f,  1.0f, -1.0f,  0.0f, 1.0f };
            while (true)
                    {
                        int[] lastPos = { Console.CursorLeft, Console.CursorTop };

                       

                        if (lastDisplayedVector != RoundVector(camera.Position, decimals))
                        {
                            lastDisplayedVector = RoundVector(camera.Position, decimals);
                            Console.SetCursorPosition(cursorPos[0], cursorPos[1]);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(cursorPos[0], cursorPos[1]);
                            Console.Write(RoundVector(camera.Position, decimals));
                            Console.SetCursorPosition(lastPos[0], lastPos[1]);
                        }

                       
                

                        if (Console.KeyAvailable)
                        {
                            if (Console.ReadLine().ToLower() == "add")
                            {
                                vertices.Add(RoundVector(camera.Position, decimals));

                        objectPoints.Add(new GameObject(camera, cube, "point", 0, vertices.Last(), new OpenTK.Mathematics.Vector3(45, 45, 45), new OpenTK.Mathematics.Vector3(0.1f, 0.1f, 0.1f)));

                                Console.WriteLine($"Succesfully added {vertices.Last()}");
                            }
                            else break;
                           
                        }
                        
                    }
                    Console.WriteLine($"Adding finished with total {vertices.Count} vertices");

                    List<float> vertexData = new List<float>();
                    void addToList(OpenTK.Mathematics.Vector3 vector3, float textureX, float textureY)
                    {
                        vertexData.Add(vector3.X);
                        vertexData.Add(vector3.Y);
                        vertexData.Add(vector3.Z);

                        vertexData.Add(textureX);
                        vertexData.Add(textureY);
                    }

                    if (vertices.Count == 2)
                    {

                        float length = Math.Abs(vertices[0].X - vertices[1].X) / 2;
                        float height = Math.Abs(vertices[0].Y - vertices[1].Y) / 2;
                float width = Math.Abs(vertices[0].Z - vertices[1].Z / 2);

                        float[] sample;

                        if (selection == 2)
                        {
                            sample = new float[]{
                                -1f, -1f, -1f,  0.0f, 0.0f,
                                 1f, -1f, 1f,  1.0f, 0.0f,
                                 1f,  1f, 1f,  1.0f, 1.0f,
                                 1f,  1f, 1f,  1.0f, 1.0f,
                                -1f,  1f, -1f,  0.0f, 1.0f,
                                -1f, -1f, -1f,  0.0f, 0.0f
                            };
                        }
                        else
                        {
                            sample = new float[]
                            {
                                //back
                                -1.0f, -1.0f, -1.0f,  1.0f, 0.0f,
                                 1.0f, -1.0f, -1.0f,  0.0f, 0.0f,
                                 1.0f,  1.0f, -1.0f,  0.0f, 1.0f,
                                 1.0f,  1.0f, -1.0f,  0.0f, 1.0f,
                                -1.0f,  1.0f, -1.0f,  1.0f, 1.0f,
                                -1.0f, -1.0f, -1.0f,  1.0f, 0.0f,

                                //front
                                -1.0f, -1.0f,  1.0f,  0.0f, 0.0f,
                                 1.0f, -1.0f,  1.0f,  1.0f, 0.0f,
                                 1.0f,  1.0f,  1.0f,  1.0f, 1.0f,
                                 1.0f,  1.0f,  1.0f,  1.0f, 1.0f,
                                -1.0f,  1.0f,  1.0f,  0.0f, 1.0f,
                                -1.0f, -1.0f,  1.0f,  0.0f, 0.0f,

                                //left
                                -1.0f,  1.0f,  1.0f,  1.0f, 1.0f,
                                -1.0f,  1.0f, -1.0f,  0.0f, 1.0f,
                                -1.0f, -1.0f, -1.0f,  0.0f, 0.0f,
                                -1.0f, -1.0f, -1.0f,  0.0f, 0.0f,
                                -1.0f, -1.0f,  1.0f,  1.0f, 0.0f,
                                -1.0f,  1.0f,  1.0f,  1.0f, 1.0f,

                                //right
                                 1.0f,  1.0f,  1.0f,  0.0f, 1.0f,
                                 1.0f,  1.0f, -1.0f,  1.0f, 1.0f,
                                 1.0f, -1.0f, -1.0f,  1.0f, 0.0f,
                                 1.0f, -1.0f, -1.0f,  1.0f, 0.0f,
                                 1.0f, -1.0f,  1.0f,  0.0f, 0.0f,
                                 1.0f,  1.0f,  1.0f,  0.0f, 1.0f,

                                 //bottom
                                -1.0f, -1.0f, -1.0f,  1.0f, 1.0f,
                                 1.0f, -1.0f, -1.0f,  0.0f, 1.0f,
                                 1.0f, -1.0f,  1.0f,  0.0f, 0.0f,
                                 1.0f, -1.0f,  1.0f,  0.0f, 0.0f,   
                                -1.0f, -1.0f,  1.0f,  1.0f, 0.0f,
                                -1.0f, -1.0f, -1.0f,  1.0f, 1.0f,

                                //top
                                -1.0f,  1.0f, -1.0f,  0.0f, 1.0f,
                                 1.0f,  1.0f, -1.0f,  1.0f, 1.0f,
                                 1.0f,  1.0f,  1.0f,  1.0f, 0.0f,
                                 1.0f,  1.0f,  1.0f,  1.0f, 0.0f,
                                -1.0f,  1.0f,  1.0f,  0.0f, 0.0f,
                                -1.0f,  1.0f, -1.0f,  0.0f, 1.0f

                            };

                        }


                midPoint.X = (vertices[0].X + vertices[1].X) / 2;
                midPoint.Y = (vertices[0].Y + vertices[1].Y) / 2;
                midPoint.Z = (vertices[0].Z + vertices[1].Z) / 2;
                Console.WriteLine($"WSC:{midPoint.X.ToString().Replace(",", ".")}|{midPoint.Y.ToString().Replace(",", ".")}|{midPoint.Z.ToString().Replace(",",".")}");

                int index = 0;
                        while (index < sample.Length)
                        {
                            sample[index] *= length;
                            sample[index + 1] *= height;
                            sample[index + 2] *= width;
                            index += 5;
                        }

                        foreach (float f in sample)
                            vertexData.Add(f);



                    }

                    foreach (float f in vertexData)
                        Console.Write(f.ToString().Replace(",", ".") + "f,");
                    Console.WriteLine();

                    return output;
                
            
                
            
            
        }
    }
}
