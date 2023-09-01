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
    }
}
