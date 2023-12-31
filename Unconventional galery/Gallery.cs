﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;
using System.Diagnostics;

using StbImageSharp;

namespace Unconventional_galery
{
    internal class Gallery : GameWindow
    {
        public Gallery(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
        }

        float[] _verts = {
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f
        };

        // Because we're adding a texture, we modify the vertex array to include texture coordinates.
        // Texture coordinates range from 0.0 to 1.0, with (0.0, 0.0) representing the bottom left, and (1.0, 1.0) representing the top right.
        // The new layout is three floats to create a vertex, then two floats to create the coordinates.
        float[] _vertices = {
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
};

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        // For documentation on this, check Texture.cs.
        private Texture _texture;

        Stopwatch Stopwatch;

        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        double _time = 0;

        string[] _files = Directory.GetFiles(Data.TexturesPath);
        int index = 0;

        List<GameObject> _objects = new List<GameObject>();
        public List<GameObject> _objectPoints = new List<GameObject>();
        public List<GameObject> _previewObjects = new List<GameObject>();

        Task Task;

        bool reloadingObjects = false;




        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.45f, 0.75f, 0.9f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // The shaders have been modified to include the texture coordinates, check them out after finishing the OnLoad function.
            _shader = new Shader("Shaders/vertShader.vert", "Shaders/fragShader.frag");
            _shader.Use();

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _texture = Texture.LoadFromFile("Resources/.0_floor.png", GameObjectType.NONE);
            _texture.Use(TextureUnit.Texture0);


            // We initialize the camera so that it is 3 units back from where the rectangle is.
            // We also give it the proper aspect ratio.
            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorState = CursorState.Grabbed;


            Action readAction = () => { ReadConsoleInput(); };
            Task.Run(readAction);

            _objects = Data.MapLoader(_camera);
            Stopwatch stopwatch = new Stopwatch();
            Stopwatch = stopwatch;
            Stopwatch.Start();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _time += 8 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_vertexArrayObject);


            Matrix4 model = Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_time));
            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800 / 800, 0.1f, 100.0f);

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());



            _texture.Use(TextureUnit.Texture0);
            _shader.Use();

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            _objects[0]._scale += Vector3.One * 0.0001f * (float)Math.Sin(_time / 10);
            _objects[0]._position += new Vector3(-1, 0, 0) * 0.0001f * (float)Math.Sin(_time / 10);
            foreach (GameObject obj in _objects)
            {
                obj.Render();
            }

            foreach (GameObject point in _objectPoints)
            {
                point.Render();
                point._rotation += new Vector3((float)Math.Sin(-_time / 10), 0, (float)Math.Sin(_time / 7)) * 0.01f * (float)Math.Sin(_time / 10);
            }

            foreach (GameObject obj in _previewObjects)
            {
                obj.Render();
            }

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (reloadingObjects)
            {
                foreach (Texture texture in Data.Textures)
                    GL.DeleteTexture(texture.Handle);

                Data.Textures.Clear();
                _objects.Clear();
                _objectPoints.Clear();
                _previewObjects.Clear();

                _files = Directory.GetFiles(Data.TexturesPath);

                reloadingObjects = false;
                _objects = Data.MapLoader(_camera);
            }

            if (DataBridge.IsReady)
                ReadDataBridge();

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }


            if (input.IsKeyPressed(Keys.E))
            {
                if (index >= _files.Length - 1)
                    index = 0;
                else
                    index++;

                ChangeTexture();
            }

            if (input.IsKeyPressed(Keys.Q))
            {
                if (index <= 0)
                {
                    index = _files.Length - 1;
                }
                else
                    index--;
                ChangeTexture();
            }

            void ChangeTexture()
            {
                _texture = Texture.LoadFromFile(_files[index], GameObjectType.NONE);
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;



            if (input.IsKeyDown(Keys.W))
            {
                Vector3 destination =  Vector3.Normalize(Vector3.Cross(Vector3.UnitY, _camera.Right)) * cameraSpeed * (float)e.Time;
                if(!IsColliding(_camera.Position, _camera.Position+destination))
                    _camera.Position += destination;
               else
                   _camera.Position += destination -new Vector3(destination.X,0,0);

                // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= Vector3.Normalize(Vector3.Cross(Vector3.UnitY, _camera.Right)) * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += Vector3.UnitY * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= Vector3.UnitY * cameraSpeed * (float)e.Time; // Down
            }

            if (input.IsKeyPressed(Keys.F5))
                reloadingObjects = true;

            // Get the mouse state
            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }



            //was broken because on top is if statment that requires to be focused on game or it will return d
            //  if (Console.KeyAvailable)
            //  {
            //      Action action = ()=>{ 
            //              ReadConsoleInput();
            //          };
            //      
            //      if (Task.Status != TaskStatus.Running)
            //      {
            //          Task.Run(action);
            //      }
            //      
            //  }

        }

        void ReadDataBridge()
        {
            DataBridge.IsReady = false;


            if ((DataBridgeUsage)DataBridge.Data.Last() == DataBridgeUsage.ADD_POINT_DATA)
            {
                _objectPoints.Add(new GameObject(_camera, (float[])DataBridge.Data[0], "objectPoint", GameObjectType.OBJECT_TEMPORARY, (OpenTK.Mathematics.Vector3)DataBridge.Data[1], new OpenTK.Mathematics.Vector3(45, 0, 45), new OpenTK.Mathematics.Vector3(0.05f, 0.05f, 0.05f)));
                DataBridge.Data.Clear();
            }
            else if ((DataBridgeUsage)DataBridge.Data.Last() == DataBridgeUsage.EDITOR_PREVIEW)
            {
                _previewObjects.Add(new GameObject(_camera, (float[])DataBridge.Data[0], "preview", (GameObjectType)DataBridge.Data[1], (OpenTK.Mathematics.Vector3)DataBridge.Data[2], new OpenTK.Mathematics.Vector3(0, 0, 0), new OpenTK.Mathematics.Vector3(1f, 1f, 1f), (int)DataBridge.Data[3]));

            } else if ((DataBridgeUsage)DataBridge.Data.Last() == DataBridgeUsage.EDITOR_CLEAR)
            {
                _previewObjects.Clear();
                _objectPoints.Clear();
            }

            DataBridge.Data.Clear(); //just in case i forgot

        }



        void ReadConsoleInput()
        {


            Dictionary<string, Action> commands = new Dictionary<string, Action>
            {
                { "teleport", Teleport },
                { "reload", Reload },
                { "editor", Editor }
            };

            while (true)
            {
                try
                {
                    commands[Console.ReadLine().ToLower()].DynamicInvoke();
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Unknown command!");
                }
            }
        }

        void Editor()
        {
            foreach (string line in Data.Editor(_camera, this))
                Console.WriteLine(line);
        }

        void Reload()
        {
            reloadingObjects = true;
        }


        void Teleport()
        {
            string[] nums= new string[3];
            OpenTK.Mathematics.Vector3 pos = new OpenTK.Mathematics.Vector3();
            do
            {
                Console.WriteLine("Write coordinates to teleport. Format: X,xf|Y,yf|Z,zf");
                nums = Console.ReadLine().Replace(";","|").Split("|");
                foreach (string num in nums)
                {
                    num.Replace("f", "").Replace(".",",");
                }

            } while (!float.TryParse(nums[0], out pos.X)|| !float.TryParse(nums[1], out pos.Y)|| !float.TryParse(nums[2], out pos.Z));
            _camera.Position = pos;
        }
        

        // In the mouse wheel function, we manage all the zooming of the camera.
        // This is simply done by changing the FOV of the camera.
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY*4;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            // We need to update the aspect ratio once the window has been resized.
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        bool IsColliding(Vector3 currentPos, Vector3 targetPos)
        {
            

            foreach (GameObject gameObject in _objects)
            {
                Vector3 WSP = gameObject._position;

                if (Vector3.Distance(WSP, currentPos) > 15)
                    continue;

                List<Vector3> points = new List<Vector3>();
                for (int i = 0; i < gameObject._vertices.Count()/5; i+=5)
                {
                    points.Add(new Vector3( gameObject._vertices[i], gameObject._vertices[i+1], gameObject._vertices[i+2]));
                }
                for (int i = 0; i < points.Count / 3; i += 3)
                {
                    Vector3[] currentPoints =
                    {
                        points[i],points[i+1],points[i+2]
                    };

                    

                    var top = currentPoints[currentPoints.Select ((value, index) => new {Value = value,Index = index}).Aggregate((a,b)=>(a.Value.Y>b.Value.Y)?a:b).Index]+WSP;
                    var left = currentPoints[currentPoints.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value.X > b.Value.X&&a.Value.Y!=top.Y) ? a : b).Index]+WSP;
                    var right = currentPoints[currentPoints.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value.X < b.Value.X && a.Value.Y != top.Y) ? a : b).Index] + WSP;
                    
                    
                    
                    float x = top.X;
                    float z = top.Z;
                    float y = top.Y;

                    if (currentPos.X <= x && x <= targetPos.X || targetPos.X <= x && x <= currentPos.X)
                    {
                        return true;
                    }

                    if (currentPos.Z <= z && z <= targetPos.Z || targetPos.Z <= z && z <= currentPos.Z)
                    {
                        return true;
                    }
                    if (currentPos.Y <= y && y <= targetPos.Y || targetPos.Z <= y && y <= currentPos.Y)
                    {
                        return true;
                    }
                }

 
            }


            return false;
        }
    }
}
