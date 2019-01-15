using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Windows.Forms;
using System.IO;

namespace abasilak
{
    public class Texture
    {
        #region Private Properties

        uint          _index;
        TextureTarget _target;
        string        _name;

        static Image   _image;
        static Shader  _vertex, _fragment, _fragment_background, _copy;
        static Shading _rendering, _rendering_background, _copying;
        static Sampler _sampler;

        #endregion
    
        #region Public Properties

        public uint index
        {
            get { return _index; }
            set { _index = value; }
        }
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public TextureTarget target
        {
            get { return _target; }
            set { _target = value; }
        }

        public static Image image
        {
            get { return _image; }
            set { _image = value; }
        }
        public static Sampler sampler
        {
            get { return _sampler; }
            set { _sampler = value; }
        }
        public static Shader vertex
        {
            get { return _vertex; }
        }
        public static Shading rendering
        {
            get { return _rendering; }
        }
        public static Shading copying
        {
            get { return _copying; }
        }
        public static Matrix4 modelview_matrix;

        #endregion

        static Texture()
        {
            _image = new Image();
            _sampler = new Sampler();

            //--- final pass Shader
            _vertex   = new Shader("image/render", ShaderType.VertexShader);
            _fragment = new Shader("image/render", ShaderType.FragmentShader);

            _vertex.complile    (ShaderType.VertexShader);
            _fragment.complile  (ShaderType.FragmentShader);

            _rendering = new Shading();
            _rendering.create();
            _rendering.attachShader(_vertex.id);
            _rendering.attachShader(_fragment.id);
            _rendering.link();

            modelview_matrix = Matrix4.CreateOrthographicOffCenter(0, 1, 0, 1, -1, 1);

            _rendering.use();
            {
                _rendering.bindUniformMatrix4("modelview_matrix", false, ref modelview_matrix);
                int _multisamples = 8;
                //GL.GetInteger(GetPName.MaxSamples, out _multisamples);
                _rendering.bindUniform1("samples", _multisamples);
                
            }
            Shading.close();

            _fragment_background = new Shader("image/render_background", ShaderType.FragmentShader);
            _fragment_background.complile(ShaderType.FragmentShader);

            _rendering_background = new Shading();
            _rendering_background.create();
            _rendering_background.attachShader(_vertex.id);
            _rendering_background.attachShader(_fragment_background.id);
            _rendering_background.link();

            _rendering_background.use();
            {               
                _rendering_background.bindUniformMatrix4("modelview_matrix", false, ref modelview_matrix);

                _rendering_background.bindUniform1("width", Example._scene.width);
                _rendering_background.bindUniform1("height", Example._scene.height);
            }
            Shading.close();

            _copy = new Shader("image/copy", ShaderType.FragmentShader);
            _copy.complile(ShaderType.FragmentShader);

            _copying = new Shading();
            _copying.create();
            _copying.attachShader(_vertex.id);
            _copying.attachShader(_copy.id);
            _copying.link();

            _copying.use();
            {
                _copying.bindUniformMatrix4("modelview_matrix", false, ref modelview_matrix);
            }
            Shading.close();

        }
        public Texture(TextureTarget t)
        {
            GL.GenTextures(1, out _index);
            _target = t;
        }

        private static Texture LoadFile(string FilePath)
        {
            Bitmap tData = new Bitmap(FilePath);
            System.Drawing.Imaging.BitmapData data = tData.LockBits(new System.Drawing.Rectangle(0, 0, tData.Width, tData.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Texture _tex = new Texture(TextureTarget.Texture2D);
            _tex.name = Path.GetFileName(FilePath);
            _tex.bind();
            _tex.image2D(0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            _tex.unbind();

            tData.UnlockBits(data);
            tData.Dispose();

            return _tex;
        }
        public static Texture OpenFile()
        {
            Texture result = null;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @Properties.Settings.Default.TexturePath;
            ofd.Filter = "BMP .bmp|*.bmp|JPG .jpg|*.jpg";
            ofd.Multiselect = false;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();

            Application.DoEvents();

            if (ofd.FileName.Trim() == "")
                return result;

            switch (ofd.FilterIndex)
            {
                case 1:
                case 2:
                    {
                        result = LoadFile(ofd.FileName.Trim());
                        break;
                    }
                default: break;
            }

            return result;
        }
        
        public static void enable2D()
        {
            GL.Enable(EnableCap.Texture2D);
        }
        public static void disable2D()
        {
            GL.Disable(EnableCap.Texture2D);
        }
        public static void deleteAll()
        {
            _image.delete();
            _vertex.delete();
            _fragment.delete();
            _rendering.delete();
            _fragment_background.delete();
            _rendering_background.delete();
        }
        public static void active(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
        }
        public static void unbind(TextureTarget t) { GL.BindTexture(t, 0); }
        public static void buffer(SizedInternalFormat format, int buffer)
        {
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, format, buffer);
        }

        #region Image?D Functions

        public void image2D(int level, PixelInternalFormat internalformat, int width, int height, int border, PixelFormat format, PixelType type, IntPtr pixels)
        { GL.TexImage2D(_target, level, internalformat, width, height, border, format, type, pixels); }
        public void image2D(int level, PixelInternalFormat internalformat, int border, PixelFormat format, PixelType type, IntPtr pixels)
        { GL.TexImage2D(_target, level, internalformat, Example._scene.width, Example._scene.height, border, format, type, pixels); }
        public void image3D(int level, PixelInternalFormat internalformat, int depth, int border, PixelFormat format, PixelType type, IntPtr pixels)
        { GL.TexImage3D(_target, level, internalformat, Example._scene.width, Example._scene.height, depth, border, format, type, pixels); }

        public void image3Dsub(int level, int depth, int border, PixelFormat format, PixelType type, IntPtr pixels)
        { GL.TexSubImage3D(_target, level, 0, 0, 0, Example._scene.width, Example._scene.height, depth, format, type, pixels); }
                
        public void image2DMS(int samples, PixelInternalFormat internalformat, bool fixed_p)
        {GL.TexImage2DMultisample((TextureTargetMultisample)_target, samples, internalformat, Example._scene.width, Example._scene.height, fixed_p);}
        public void image3DMS(int samples, PixelInternalFormat internalformat, int depth,  bool fixed_p)
        { GL.TexImage3DMultisample((TextureTargetMultisample)_target, samples, internalformat, Example._scene.width, Example._scene.height, depth, fixed_p); }
        
        #endregion 

        public void clear   (PixelFormat format, PixelType type, IntPtr data)
        {
            GL.ClearTexImage(_index, 0, format, type, data);
        }
        public void clear<T>(PixelFormat format, PixelType type, T[] data) where T : struct
        {
            GL.ClearTexImage<T>(_index, 0, format, type, data);
        }

        public void delete() { GL.DeleteTexture((int)_index); }
        public void bind()   { GL.BindTexture(_target, _index); }
        public void unbind() { GL.BindTexture(_target, 0); }
        public void parameter(TextureParameterName name, int parameter) { GL.TexParameter(_target, name, parameter); }

        #region Drawing Functions
        public void draw()
        {           
            Texture._rendering.use();
            {
                Texture.active(TextureUnit.Texture0); bind();
                Texture._image.draw();
                unbind();
            }
            Shading.close();
        }
        public void drawB()
        {
            Texture._rendering_background.use();
            {
                Texture.active(TextureUnit.Texture0);
                bind();
                Texture.sampler.bind(0);
                Texture._image.draw();
                unbind();
            }
            Shading.close();
        }
        #endregion 
    }
}