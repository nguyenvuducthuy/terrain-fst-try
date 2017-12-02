﻿using System;
using System.Collections.Generic;
using System.IO;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics
{
    partial class Shaders
    {
        private static TexturelessNoLight _texturelessNoLightDescriptor;
        private static TexturedNoLight _texturedNoLightDescriptor;

        class TexturelessNoLight
        {
            public int uniformMVP = 0;
            public int uniformMV = 0;
            public int uniformProjection = 0;
            public int ProgramId = 0;
            public int AttribVerticesLocation = 0;
            public int AttribNormalsLocation = 0;
            public int AttribColorsLocation = 0;
            public int verticesBuffer = 0;
            public int colorsBuffer = 0;
            public int normalsBuffer = 0;
        }

        class TexturedNoLight
        {
            public int uniformMVP = 0;
            public int uniformMV = 0;
            public int uniformProjection = 0;
            public int ProgramId = 0;
            public int AttribVerticesLocation = 0;
            public int AttribNormalsLocation = 0;
            public int AttribColorsLocation = 0;
            public int TexCoordsLocation = 0;
            public int verticesBuffer = 0;
            public int colorsBuffer = 0;
            public int normalsBuffer = 0;
            public int texCoordsBuffer = 0;
            public int uniformTexture1 = 0;
        }

        public static void InitTexturelessNoLight()
        {
            var textureLessProgId = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);
            var vertText = File.ReadAllText(@"Assets\Shaders\texturelessNoLight.vert");
            GL.ShaderSource(vert, vertText);
            GL.CompileShader(vert);
            GL.AttachShader(textureLessProgId, vert);

            int statusCode;
            GL.GetShader(vert, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vert, out info);
                throw new Exception("vertex shader: " + info);
            }

            var frag = GL.CreateShader(ShaderType.FragmentShader);
            var fragText = File.ReadAllText(@"Assets\Shaders\texturelessNoLight.frag");
            GL.ShaderSource(frag, fragText);
            GL.CompileShader(frag);
            GL.AttachShader(textureLessProgId, frag);

            GL.LinkProgram(textureLessProgId);

            GL.GetShader(frag, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(frag, out info);
                throw new Exception("fragment shader: " + info);
            }

            _texturelessNoLightDescriptor = new TexturelessNoLight();

            _texturelessNoLightDescriptor.uniformMVP = GL.GetUniformLocation(textureLessProgId, "uMVP");
            _texturelessNoLightDescriptor.uniformMV = GL.GetUniformLocation(textureLessProgId, "uMV");
            _texturelessNoLightDescriptor.uniformProjection = GL.GetUniformLocation(textureLessProgId, "uP");

            _texturelessNoLightDescriptor.ProgramId = textureLessProgId;

            _texturelessNoLightDescriptor.AttribVerticesLocation = GL.GetAttribLocation(textureLessProgId, "vPosition");
            _texturelessNoLightDescriptor.AttribNormalsLocation = GL.GetAttribLocation(textureLessProgId, "vNormal");
            _texturelessNoLightDescriptor.AttribColorsLocation = GL.GetAttribLocation(textureLessProgId, "vColor");

            GL.GenBuffers(1, out _texturelessNoLightDescriptor.verticesBuffer);
            GL.GenBuffers(1, out _texturelessNoLightDescriptor.colorsBuffer);
            GL.GenBuffers(1, out _texturelessNoLightDescriptor.normalsBuffer);
        }

        public static void InitTexturedNoLight()
        {
            var texturedProgId = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);

            var vertText = File.ReadAllText(@"Assets\Shaders\texturedNoLight.vert");
            GL.ShaderSource(vert, vertText);
            GL.CompileShader(vert);
            GL.AttachShader(texturedProgId, vert);


            int statusCode;
            GL.GetShader(vert, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vert, out info);
                throw new Exception("vertex shader: " + info);
            }

            var frag = GL.CreateShader(ShaderType.FragmentShader);
            var fragText = File.ReadAllText(@"Assets\Shaders\texturedNoLight.frag");
            GL.ShaderSource(frag, fragText);
            GL.CompileShader(frag);
            GL.AttachShader(texturedProgId, frag);

            GL.LinkProgram(texturedProgId);

            GL.GetShader(frag, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(frag, out info);
                throw new Exception("fragment shader: " + info);
            }

            _texturedNoLightDescriptor = new TexturedNoLight();

            _texturedNoLightDescriptor.uniformMVP = GL.GetUniformLocation(texturedProgId, "uMVP");
            _texturedNoLightDescriptor.uniformMV = GL.GetUniformLocation(texturedProgId, "uMV");
            _texturedNoLightDescriptor.uniformProjection = GL.GetUniformLocation(texturedProgId, "uP");
            _texturedNoLightDescriptor.uniformTexture1 = GL.GetUniformLocation(texturedProgId, "u_Texture");

            _texturedNoLightDescriptor.ProgramId = texturedProgId;

           _texturedNoLightDescriptor.AttribVerticesLocation = GL.GetAttribLocation(texturedProgId, "vPosition");
           _texturedNoLightDescriptor.AttribNormalsLocation = GL.GetAttribLocation(texturedProgId, "vNormal");
            _texturedNoLightDescriptor.AttribColorsLocation = GL.GetAttribLocation(texturedProgId, "vColor");
            _texturedNoLightDescriptor.TexCoordsLocation = GL.GetAttribLocation(texturedProgId, "a_TexCoordinate");

            GL.GenBuffers(1, out _texturedNoLightDescriptor.verticesBuffer);
            GL.GenBuffers(1, out _texturedNoLightDescriptor.colorsBuffer);
            GL.GenBuffers(1, out _texturedNoLightDescriptor.normalsBuffer);
            GL.GenBuffers(1, out _texturedNoLightDescriptor.texCoordsBuffer);
        }





    }
}