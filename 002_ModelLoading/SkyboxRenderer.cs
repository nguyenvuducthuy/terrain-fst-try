﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ModelLoading
{
    class SkyboxRenderer
    {
        public int SkyboxTextureId { get; set; }

        public ShaderManager ShaderManager { get; set; }
        public RenderEngine RenderEngine { get; set; }

        public SkyboxRenderer(ShaderManager mgr, RenderEngine eng)
        {
            this.ShaderManager = mgr;
            this.RenderEngine = eng;
            SkyboxTextureId = LoadCubeMapForSkybox(skyboxPaths);
        }


        public void Render()
        {
            GL.UseProgram(ShaderManager.ProgramIdForSky);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(ShaderManager.UniformSkySampler, 0);
            GL.BindTexture(TextureTarget.TextureCubeMap, SkyboxTextureId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, ShaderManager.sky_texcoord_buffer_address);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(VERTICES.Length * Vector3.SizeInBytes),
              VERTICES, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(ShaderManager.AttribPositionSkybox, 
                3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindVertexArray(ShaderManager.AttribPositionSkybox);
            GL.EnableVertexAttribArray(ShaderManager.AttribPositionSkybox);

            var translation = Matrix4.CreateTranslation(RenderEngine.Player.Position.X, 0, RenderEngine.Player.Position.Z);
            var view = translation * RenderEngine.ModelView;

            GL.UniformMatrix4(ShaderManager.UniformViewSkybox, false, ref view);
            GL.UniformMatrix4(ShaderManager.UniformProjectionSkybox, false, ref RenderEngine.Projection);

            GL.DrawArrays(PrimitiveType.Triangles, 0, VERTICES.Length);
        }

        public int LoadCubeMapForSkybox(string[] paths)
        {

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.GenTextures(1, out int texureId);

            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];

                var png = new Bitmap(path);

                var width = png.Width;

                var height = png.Height;

                var rect = new Rectangle(0, 0, width, height);

                var bitmap_data = png.LockBits(rect,
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.BindTexture(TextureTarget.TextureCubeMap, texureId);


                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, 
                    PixelInternalFormat.Rgba,
                    width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                    PixelType.UnsignedByte, IntPtr.Zero);


                GL.TexSubImage2D(TextureTarget.TextureCubeMapPositiveX + i,
                    level: 0, xoffset: 0, yoffset: 0,
                 width: width, height: height, 
                 format: OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                 type: PixelType.UnsignedByte,
                 pixels: bitmap_data.Scan0);

                png.UnlockBits(bitmap_data);

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)All.Linear);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)All.Linear);

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)All.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);
            }

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            return texureId;
        }


        private const float SIZE = 80f;

        public static Vector3[] VERTICES = {        
	   new Vector3(-SIZE,  SIZE, -SIZE),
	   new Vector3(-SIZE, -SIZE, -SIZE),
	   new Vector3(SIZE, -SIZE, -SIZE),
	   new Vector3(SIZE, -SIZE, -SIZE),
	   new Vector3(SIZE,  SIZE, -SIZE),
	   new Vector3(-SIZE,  SIZE, -SIZE),

	    new Vector3(-SIZE, -SIZE,  SIZE),
	    new Vector3(-SIZE, -SIZE, -SIZE),
	    new Vector3(-SIZE,  SIZE, -SIZE),
	    new Vector3(-SIZE,  SIZE, -SIZE),
	    new Vector3(-SIZE,  SIZE,  SIZE),
	    new Vector3(-SIZE, -SIZE,  SIZE),

	     new Vector3(SIZE, -SIZE, -SIZE),
	     new Vector3(SIZE, -SIZE,  SIZE),
	     new Vector3(SIZE,  SIZE,  SIZE),
	     new Vector3(SIZE,  SIZE,  SIZE),
	     new Vector3(SIZE,  SIZE, -SIZE),
	     new Vector3(SIZE, -SIZE, -SIZE),

	    new Vector3(-SIZE, -SIZE,  SIZE),
	    new Vector3(-SIZE,  SIZE,  SIZE),
	    new Vector3( SIZE,  SIZE,  SIZE),
	    new Vector3( SIZE,  SIZE,  SIZE),
	    new Vector3( SIZE, -SIZE,  SIZE),
	    new Vector3(-SIZE, -SIZE,  SIZE),

	    new Vector3(-SIZE,  SIZE, -SIZE),
	    new Vector3( SIZE,  SIZE, -SIZE),
	    new Vector3( SIZE,  SIZE,  SIZE),
	    new Vector3( SIZE,  SIZE,  SIZE),
	    new Vector3(-SIZE,  SIZE,  SIZE),
	    new Vector3(-SIZE,  SIZE, -SIZE),

	    new Vector3(-SIZE, -SIZE, -SIZE),
	    new Vector3(-SIZE, -SIZE,  SIZE),
	    new Vector3( SIZE, -SIZE, -SIZE),
	    new Vector3( SIZE, -SIZE, -SIZE),
	    new Vector3(-SIZE, -SIZE,  SIZE),
	    new Vector3( SIZE, -SIZE,  SIZE)
	};

        public static string[] skyboxPaths = new string[] { 
                                                    @"Assets\\Textures\\Skybox\\right.png", 
                                                    @"Assets\\Textures\\Skybox\\left.png", 
                                                    @"Assets\\Textures\\Skybox\\top.png",
                                                    @"Assets\\Textures\\Skybox\\bottom.png", 
                                                    @"Assets\\Textures\\Skybox\\back.png", 
                                                    @"Assets\\Textures\\Skybox\\front.png"};

    }
}
