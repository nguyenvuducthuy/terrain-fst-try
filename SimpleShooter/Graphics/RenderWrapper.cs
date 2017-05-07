﻿using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SimpleShooter.Core;
using SimpleShooter.Graphics.ShaderLoad;

namespace SimpleShooter.Graphics
{
    public class RenderWrapper : IRenderWrapper
    {
        private GameObject _gameObject;

        private ShaderProgramDescriptor _descriptor;

        public RenderWrapper(GameObject gameObject)
        {
            _gameObject = gameObject;
            bool loaded = ShaderLoader.TryGet(_gameObject.ShaderKind, out _descriptor);
            if (!loaded)
            {
                throw new InvalidOperationException();
            }
            if (_gameObject.ShaderKind == ShadersNeeded.Line)
            {
                RenderType = PrimitiveType.Lines;
            }
        }

        public int VerticesCount
        {
            get
            {
                return (_gameObject as GameObject).Model.Vertices.Length;
            }
        }

        public ShadersNeeded ShaderKind { get { return _gameObject.ShaderKind; } }

        private PrimitiveType _renderType = PrimitiveType.Triangles;

        public PrimitiveType RenderType
        {
            get { return _renderType; }
            set { _renderType = value; }
        }

        public void Bind(Camera camera, Level level)
        {
            GL.UseProgram(_descriptor.ProgramId);
            BindUniforms(camera, level.LightPosition);
            BindBuffers(_gameObject.ShaderKind);
        }

        public void BindUniforms(Camera camera, Vector3 lightPos)
        {
            GL.UniformMatrix4(_descriptor.uniformMV, false, ref camera.ModelView);
            GL.UniformMatrix4(_descriptor.uniformMVP, false, ref camera.ModelViewProjection);
            GL.UniformMatrix4(_descriptor.uniformProjection, false, ref camera.Projection);

            switch (_gameObject.ShaderKind)
            {
                case ShadersNeeded.SimpleModel:
                    break;
                case ShadersNeeded.Line:
                    break;
                case ShadersNeeded.TextureLess:
                    GL.Uniform3(_descriptor.uniformLightPos, lightPos);

                    break;

                case ShadersNeeded.TextureLessNoLight:


                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_gameObject.ShaderKind), _gameObject.ShaderKind, null);
            }

        }

        private void BindBuffers(ShadersNeeded gameObjectShaderKind)
        {
            BindVertices();
            BindColors();

            switch (gameObjectShaderKind)
            {
                case ShadersNeeded.SimpleModel:
                    BindNormals();
                    break;
                case ShadersNeeded.TextureLess:
                    BindNormals();
                    break;
                case ShadersNeeded.Line:
                    break;
                case ShadersNeeded.TextureLessNoLight:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameObjectShaderKind), gameObjectShaderKind, null);
            }
        }

        public static void BindUniforms(ShaderProgramDescriptor _descriptor, Camera camera, Vector3 lightPos)
        {
            GL.UniformMatrix4(_descriptor.uniformMV, false, ref camera.ModelView);
            GL.UniformMatrix4(_descriptor.uniformMVP, false, ref camera.ModelViewProjection);
            GL.UniformMatrix4(_descriptor.uniformProjection, false, ref camera.Projection);

            switch (_descriptor.ShaderKind)
            {
                case ShadersNeeded.SimpleModel:
                    break;
                case ShadersNeeded.Line:
                    break;
                case ShadersNeeded.TextureLess:
                    GL.Uniform3(_descriptor.uniformLightPos, lightPos);

                    break;

                case ShadersNeeded.TextureLessNoLight:


                    break;
            }
        }


        public static void BindVertices(ShaderProgramDescriptor _descriptor, Vector3[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _descriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Vector3.SizeInBytes),
                data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_descriptor.AttribVerticesLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_descriptor.AttribVerticesLocation);
        }

        public static void BindColors(ShaderProgramDescriptor _descriptor, Vector3[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _descriptor.colorsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Vector3.SizeInBytes),
                data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_descriptor.AttribColorsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_descriptor.AttribColorsLocation);
        }

        public static void BindNormals(ShaderProgramDescriptor _descriptor, Vector3[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _descriptor.normalsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Vector3.SizeInBytes),
                data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_descriptor.AttribNormalsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_descriptor.AttribNormalsLocation);
        }



        private void BindVertices()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _descriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (_gameObject.Model.Vertices.Length * Vector3.SizeInBytes),
                _gameObject.Model.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_descriptor.AttribVerticesLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_descriptor.AttribVerticesLocation);
        }

        private void BindColors()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _descriptor.colorsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (_gameObject.Model.Colors.Length * Vector3.SizeInBytes),
                _gameObject.Model.Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_descriptor.AttribColorsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_descriptor.AttribColorsLocation);
        }

        private void BindNormals()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _descriptor.normalsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_gameObject.Model.Normals.Length * Vector3.SizeInBytes),
                _gameObject.Model.Normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_descriptor.AttribNormalsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_descriptor.AttribNormalsLocation);
        }

        public ShaderProgramDescriptor GetDescriptor()
        {
            return _descriptor;
        }
    }
}