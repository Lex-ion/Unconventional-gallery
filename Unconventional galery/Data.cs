using System.Globalization;
using System.IO;
using StbImageSharp;


namespace Unconventional_galery
{
    enum DataBridgeUsage
    {
        ADD_POINT_DATA = 1,
        EDITOR_PREVIEW = 2,
        EDITOR_CLEAR = 3,
    }

    internal class DataBridge
    {
        public static List<object> Data = new List<object>();
        public static bool IsReady = false;

        public (int, double) ti = (5, 4.89);

    }

    internal class Data
    {
        public static List<Texture> Textures = new List<Texture>();
        public static string TexturesPath = "Resources";

        public static List<GameObject> MapLoader(Camera camera)
        {
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            List<GameObject> objects = new List<GameObject>();

            string[] raw = File.ReadAllText("Data/map.txt").Replace("\n", "").Replace("\r", "").Split("NEW");
            raw = raw.Skip(1).ToArray();
            foreach (string data in raw)
            {
                string[] head = data.Split("VERTICES")[0].Split("INDICES")[0].Split(",");
                string[] vertData = data.Split("VERTICES")[1].Split(",");


                string[] wscRaw = head[2].Split(":")[1].Split("|");
                OpenTK.Mathematics.Vector3 wsc = new OpenTK.Mathematics.Vector3(float.Parse(wscRaw[0].Replace("f", ""), System.Globalization.NumberStyles.Any, ci), float.Parse(wscRaw[1].Replace("f", ""), System.Globalization.NumberStyles.Any, ci), float.Parse(wscRaw[2].Replace("f", ""), System.Globalization.NumberStyles.Any, ci));

                string[] wsrRaw = head[3].Split(":")[1].Split("|");
                OpenTK.Mathematics.Vector3 wsr = new OpenTK.Mathematics.Vector3(float.Parse(wsrRaw[0].Replace("f", ""), System.Globalization.NumberStyles.Any, ci), float.Parse(wsrRaw[1].Replace("f", ""), System.Globalization.NumberStyles.Any, ci), float.Parse(wsrRaw[2].Replace("f", ""), System.Globalization.NumberStyles.Any, ci));

                string[] scaleRaw = head[4].Split(":")[1].Split("|");
                OpenTK.Mathematics.Vector3 scale = new OpenTK.Mathematics.Vector3(float.Parse(scaleRaw[0].Replace("f", ""), System.Globalization.NumberStyles.Any, ci), float.Parse(scaleRaw[1].Replace("f", ""), System.Globalization.NumberStyles.Any, ci), float.Parse(scaleRaw[2].Replace("f", ""), System.Globalization.NumberStyles.Any, ci));

                int textureOverride = -1;
                if (head.Length > 5)
                {
                    textureOverride = Convert.ToInt32(head[5].Split(":")[1]);
                }







                float[] vertices = new float[vertData.Length];
                for (int i = 0; i < vertData.Length; i++)
                {
                    vertices[i] = float.Parse(vertData[i].Replace("f", ""), System.Globalization.NumberStyles.Any, ci);
                }
                objects.Add(new GameObject(camera, vertices, head[0].Split(":")[1], (GameObjectType)Convert.ToInt32(head[1].Split(":")[1]), wsc, wsr, scale, textureOverride));
            }

            return objects;
        }

        public static List<string> Editor(Camera camera, Gallery gallery)
        {
            List<string> output = new List<string>();

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

            float[] plane = {
             -1f, -1f, -1f,  0.0f, 0.0f,
              1f, -1f, 1f,  1.0f, 0.0f,
              1f,  1f, 1f,  1.0f, 1.0f,
              1f,  1f, 1f,  1.0f, 1.0f,
             -1f,  1f, -1f,  0.0f, 1.0f,
             -1f, -1f, -1f,  0.0f, 0.0f
                            };

            float[] sample= new float[plane.Length];

            string debugKey;
            int gameObjectType = 0;
            int selection = 0;
            int gameObjectTypeOverride = -1;
            int decimals;

            OpenTK.Mathematics.Vector3 midPoint = new OpenTK.Mathematics.Vector3();
            List<OpenTK.Mathematics.Vector3> vertices = new List<OpenTK.Mathematics.Vector3>();
            List<float> vertexData = new List<float>();


            float[,] textureCounts = new float[6, 2];
             //-------- code

            for(int i = 0; i < textureCounts.GetLength(0); i++)
                for (int j = 0; j < textureCounts.GetLength(1); j++)
                    textureCounts[i, j] = 1;
                


            SetUp();
            
            Creator();

            CreateOutput();

            return output;


            //---------
            //Methods
            string Error()
            {
                return "error";
            }

            OpenTK.Mathematics.Vector3 RoundVector(OpenTK.Mathematics.Vector3 vector3, int decimals)
            {
                return new OpenTK.Mathematics.Vector3((float)Math.Round(vector3.X, decimals, MidpointRounding.AwayFromZero), (float)Math.Round(vector3.Y, decimals, MidpointRounding.AwayFromZero), (float)Math.Round(vector3.Z, decimals, MidpointRounding.AwayFromZero));
            }

            void GenerateData()
            {
                if (vertices.Count == 2)
                {
                    


                    if (selection == 1)
                    {
                        sample = new float[cube.Length];
                        cube.CopyTo(sample, 0);
                    } else
                        plane.CopyTo(sample, 0);



                    float length = Math.Abs(vertices[0].X - vertices[1].X) / 2;
                    float height = Math.Abs(vertices[0].Y - vertices[1].Y) / 2;
                    float width = Math.Abs(vertices[0].Z - vertices[1].Z) / 2;




                    midPoint.X = (vertices[0].X + vertices[1].X) / 2;
                    midPoint.Y = (vertices[0].Y + vertices[1].Y) / 2;
                    midPoint.Z = (vertices[0].Z + vertices[1].Z) / 2;


                    for (int i=0; i < sample.Length;i+=5)
                    {
                        sample[i] *= length;
                        sample[i + 1] *= height;
                        sample[i + 2] *= width;
                        sample[i + 3] *= textureCounts[i/(5*6),0];
                        sample[i + 4] *= textureCounts[i/(5*6),1];
                        
                    }      
                        


                }
            }

            void AddToList(OpenTK.Mathematics.Vector3 vector3, float textureX, float textureY)
            {
                vertexData.Add(vector3.X);
                vertexData.Add(vector3.Y);
                vertexData.Add(vector3.Z);

                vertexData.Add(textureX);
                vertexData.Add(textureY);
            }
            
            void SetUp()
            {
                //debugKey
                Console.WriteLine("Enter debug key:");
                debugKey = Console.ReadLine();

                //gameObjectType
                do
                {
                    Console.WriteLine("Enter valid gameObjectType");
                } while (!int.TryParse(Console.ReadLine(), out gameObjectType) || !Enum.IsDefined(typeof(GameObjectType), gameObjectType));
                //override
                Console.WriteLine("Do you wish to override? [Y]es anything else is no");
                if (gameObjectType == -1)
                {
                    Console.WriteLine("Override is needed");
                }
                //override type
                if (gameObjectType == -1 || Console.ReadKey(true).KeyChar == 'y')
                {
                    do
                    {
                        Console.WriteLine("Enter valid gameObjectType for override");
                    } while (!int.TryParse(Console.ReadLine(), out gameObjectTypeOverride) || !Enum.IsDefined(typeof(GameObjectType), gameObjectTypeOverride) || gameObjectTypeOverride == -1);
                }
                //cuboid or plane
                Console.WriteLine("Create [C]uboid or [P]lane?");
                do
                {

                    switch (Console.ReadKey(true).KeyChar)
                    {
                        case 'c':
                            selection = 1;
                            Console.WriteLine("Selected cuboid");
                            break;

                        case 'p':
                            selection = 2;
                            Console.WriteLine("Selected plane");
                            break;
                    }


                } while (selection == 0);
                //rounding
                do
                {
                    Console.WriteLine("Set rounding (0 - 15)");
                } while (!int.TryParse(Console.ReadLine(), out decimals) || decimals < 0 || decimals > 15);
            }

            void Preview()
            {
                if (vertices.Count < 2)
                {
                    Console.WriteLine("not enough vertices");
                }
                else
                {
                    GenerateData();
                    DataBridge.Data.Add(sample);
                    DataBridge.Data.Add(gameObjectType);
                    DataBridge.Data.Add(midPoint);
                    DataBridge.Data.Add(gameObjectTypeOverride);
                    DataBridge.Data.Add(DataBridgeUsage.EDITOR_PREVIEW);
                    DataBridge.IsReady = true;
                }                              
            }

            void Creator()
            {
                Console.WriteLine("Set 2 or more vertexes. For adding fly to point and type to console add. When you are done press enter");
                Console.Write("Current position: ");
                int[] cursorPos = { Console.CursorLeft, Console.CursorTop };
                bool run = true;
                while (run)
                {

                    int[] lastPos = { Console.CursorLeft, Console.CursorTop };
                    OpenTK.Mathematics.Vector3 lastDisplayedVector = new OpenTK.Mathematics.Vector3();


                    //writes out coordinates
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
                        switch (Console.ReadLine().ToLower().Replace(" ",""))
                        {
                            default:
                                Console.WriteLine("Unknown command, if you wish to exit write \"Done\"");
                                break;

                            case "add":
                                vertices.Add(RoundVector(camera.Position, decimals));

                                DataBridge.Data.Add(cube);
                                DataBridge.Data.Add(vertices.Last());
                                DataBridge.Data.Add(DataBridgeUsage.ADD_POINT_DATA);
                                DataBridge.IsReady = true;

                                Console.WriteLine($"Succesfully added {vertices.Last()}");
                                break;

                            case "remove":
                                Console.WriteLine($"Removed {vertices.Last()}");
                                vertices.Remove(vertices.Last());
                                gallery._objectPoints.Remove(gallery._objectPoints.Last());
                                break;

                            case "preview":
                                Preview();
                                break;

                            case "clear":
                                DataBridge.Data.Add(DataBridgeUsage.EDITOR_CLEAR);
                                DataBridge.IsReady = true;
                                break;

                            case "settexturecount":
                                EditTextureCount();
                                break;

                            case "generatetexturecount":
                                GenerateTextureCount();
                                break;

                            case "save":
                                break;

                            case "done":
                                run = false;
                                break;

                        }
                    }
                }
                Console.WriteLine($"Adding finished with total {vertices.Count} vertices");
            }

            void CreateOutput()
            {
                GenerateData();

                foreach (float f in sample)
                    vertexData.Add(f);

                string constraint = "VERTICES\n";

                for (int i = 0; i < vertexData.Count; i++)
                {
                    constraint += vertexData[i].ToString().Replace(",", ".") + "f";
                    if (i < vertexData.Count - 1)
                        constraint += ",";
                }

                output.Add("NEW");
                output.Add($"DebugKey:{debugKey},");
                output.Add($"GameObjectType:{gameObjectType},");
                output.Add($"WSC:{midPoint.X.ToString().Replace(",", ".")}|{midPoint.Y.ToString().Replace(",", ".")}|{midPoint.Z.ToString().Replace(",", ".")},");
                output.Add("WSR:0|0|0,");
                output.Add("Scale:1|1|1");
                

                if (gameObjectTypeOverride > -1)
                {
                    output.Add($",Override:{gameObjectTypeOverride}");
                }

                output.Add(constraint);
            }

            void EditTextureCount()
            {
                CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                float[] floats = new float[2];

                bool passed=false;
                do
                {
                    Console.WriteLine("Repeat texture along X axis times: [N.n]");
                    passed = float.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Any, ci, out floats[0]);
                } while (!passed);
                passed = false;
                do
                {
                    Console.WriteLine("Repeat texture along Y axis times: [N.n]");
                    passed = float.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Any, ci, out floats[1]);
                } while (!passed);

                for (int i = 0; i < textureCounts.GetLength(0); i++)
                    for (int j = 0; j < textureCounts.GetLength(1); j++)
                        textureCounts[i, j] = floats[j];
            }

            void GenerateTextureCount()
            {
                if (vertices.Count != 2)
                    return; 



                using (Stream stream = File.OpenRead(Directory.GetFiles(TexturesPath)[gameObjectTypeOverride>-1?gameObjectTypeOverride:gameObjectType]))
                {
                    ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                    float length = Math.Abs(vertices[0].X - vertices[1].X);
                    float height = Math.Abs(vertices[0].Y - vertices[1].Y);
                    float width  = Math.Abs(vertices[0].Z - vertices[1].Z);

                    Console.WriteLine($"Length: {length}");
                    Console.WriteLine($"Height: {height}");
                    Console.WriteLine($"width:  {width}");
                    Console.WriteLine($"Texture resolution: {image.Width}x{image.Height}");


                    CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    ci.NumberFormat.CurrencyDecimalSeparator = ".";

                    float detailLevel = 0;
                    bool passed = false;
                    do
                    {
                        Console.WriteLine("Set detail level (best around 200-750)");
                        passed = float.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Any, ci, out detailLevel);
                    } while (!passed);

                    length *= detailLevel;
                    height *= detailLevel;
                    width  *= detailLevel;


                    for (int i = 0; i < 2; i++) //back&front
                    {
                        textureCounts[i, 0] = length / image.Width;
                        textureCounts[i, 1] = height / image.Height;
                    }
                    for (int i = 2; i < 4; i++)
                    {
                        textureCounts[i, 0] = width / image.Width;
                        textureCounts[i, 1] = height / image.Height;
                    }
                    for (int i = 4; i < 6; i++)
                    {
                        textureCounts[i, 0] = length / image.Width;
                        textureCounts[i, 1] = width / image.Height;
                    }
                };
            }

        }
    }
}
