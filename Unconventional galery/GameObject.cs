using System;
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

namespace Unconventional_galery
{
    /// <summary>
    /// Object will get its texture by its type.
    /// NONE type is used for objects that will be defined by override, because they are not as common.
    /// </summary>
    public enum GameObjectType //to do load enum from file, so it can be modified
    {
        NONE = -1,
        OBJECT_FLOOR = 0,
        OBJECT_WALL = 1,
        OBJECT_DISPLAY_FRONT = 2,
        OBJECT_DISPLAY_BACK = 3,
        OBJECT_TEMPORARY =4,
        OBJECT_PILLAR=5,
        OBJECT_DISPLAY_DETAIL=6,
        OBJECT_PIPES=7,
    }

    internal class GameObject
    {
        public float[] _vertices { get; set; }
        
        public string _debugKey {  get; set; }

        public GameObjectType _gameObjectType {get;set;}

        public float[] _indices { get; set; }

        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        private Texture _texture;


        private Camera _camera { get; set; }

        public Vector3 _position { get; set; }
        public Vector3 _rotation { get; set; }
        public Vector3 _scale { get; set; }


       public  GameObject(Camera camera, float[] vertices ,string debugkey,GameObjectType gameObjectType, Vector3 worldSpaceCords, Vector3 worldSpaceRot, Vector3 scale, int textureOverride=-1,float[] indices = null, string[] shadersPath = null )
        {



         float[] DefaultIndices =
        {
            0, 1, 3,
            1, 2, 3
        };

         string[] DefaultShader =
        {
            "Shaders/shader.vert",
            "Shaders/shader.frag"
        };


        _camera = camera;
            _vertices = vertices;
            
            _debugKey = debugkey;
            _gameObjectType = gameObjectType;

            _position= worldSpaceCords;
            _rotation = worldSpaceRot;
            _scale= scale;
            

            if (indices == null) {
            _indices = DefaultIndices;
            } else
                _indices = indices;


            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            if (shadersPath == null)
            {
                _shader = new Shader(DefaultShader[0], DefaultShader[1]);
            }
            else
                _shader = new Shader(shadersPath[0], shadersPath[1]);
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

            if(textureOverride<0&&gameObjectType==GameObjectType.NONE)    { throw new Exception($"DEBUGKEY:{_debugKey} - Unable to set texture. GameObjectType is NONE and textureOverride is not set or bellow zero!"); }
            if(textureOverride> Directory.GetFiles("Resources").Length-1) { throw new Exception($"DEBUGKEY:{_debugKey} - textureOverride is higher then last texture ID!"); }
            if(Convert.ToInt32 (gameObjectType) > Directory.GetFiles("Resources").Length - 1) { throw new Exception($"DEBUGKEY:{_debugKey} - non existing GameObjectType is set!"); }

            if (textureOverride == -1)
            {
                _texture = Texture.LoadFromFile(Directory.GetFiles(Data.TexturesPath)[(int)_gameObjectType],gameObjectType);
            }else
                _texture = Texture.LoadFromFile(Directory.GetFiles(Data.TexturesPath)[textureOverride],gameObjectType);

            _texture.Use(TextureUnit.Texture0);



        }

        public void Render()
        {
          //  GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_vertexArrayObject);


            Matrix4 model = Matrix4.CreateScale(_scale)
                * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotation.Z)) 
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotation.Y))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_rotation.X))
                * Matrix4.CreateTranslation(_position)
                ;
            
            _shader.SetMatrix4("model", model); //worldspace
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());



            _texture.Use(TextureUnit.Texture0);
            _shader.Use();

            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length/5);
        }
    }
}
