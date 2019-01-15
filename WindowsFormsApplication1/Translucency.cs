using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace abasilak
{
    public class Translucency
    {
        #region Private Properties

        int         _currID;
        const int   _multisamples = 2;

        Texture     _tex;
        FrameBuffer _fbo;
        Texture[]   _tex_f2b = new Texture[2];
        FrameBuffer _fbo_f2b;
        Texture[]   _tex_dual = new Texture[2];
        FrameBuffer _fbo_dual;
        Texture     _tex_k_buffer;
        FrameBuffer _fbo_k_buffer;
        
        #endregion

        #region Public Properties

        public Texture tex
        {
            get { return _tex; }
        }
        public Texture tex_f2b
        {
            get { return _tex_f2b[1 - _currID]; }
        }
        public Texture tex_dual
        {
            get { return _tex_dual[1 - _currID]; }
        }
        public Texture tex_k_buffer
        {
            get { return _tex_k_buffer; }
        }
        public FrameBuffer fbo_k_buffer
        {
            get { return _fbo_k_buffer; }
        }

        #endregion

        #region Constructor
        public  Translucency()
        {
            initBuffers();
        }
        #endregion 

        #region Delete Function
        public  void delete()
        {
            _tex.delete();
            _fbo.delete();
            _tex_f2b[0].delete();
            _tex_f2b[1].delete();
            _tex_dual[0].delete();
            _tex_dual[1].delete();
            _tex_k_buffer.delete();
            
            _fbo_f2b.delete();
            _fbo_dual.delete();
            _fbo_k_buffer.delete();
        }
        #endregion

        #region Buffer Functions
        private void initBuffers()
        {
            // Render
            {
#if multisample
                _tex = new Texture(TextureTarget.Texture2DMultisample);
#else
                _tex = new Texture(TextureTarget.TextureRectangle);
#endif
                _tex.bind();
                _tex.parameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                _tex.parameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                _tex.parameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                _tex.parameter(TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
#if multisample
                _tex.image2DMS(_multisamples, PixelInternalFormat.R32f, false);
#else
                _tex.image2D(0, PixelInternalFormat.R32f, 0, PixelFormat.Red, PixelType.Float, IntPtr.Zero);
#endif
                _tex.unbind();
                // Create FrameBuffer 
                _fbo = new FrameBuffer();
                _fbo.bind();
                _fbo.attachTexture2D(FramebufferAttachment.ColorAttachment0, ref _tex, 0);
                _fbo.checkStatus();
            }
            // F2B
            {
                // Create Color Texture
                for (int i = 0; i < 2; i++)
                {
#if multisample
                    _tex_f2b[i] = new Texture(TextureTarget.Texture2DMultisample);
#else
                    _tex_f2b[i] = new Texture(TextureTarget.TextureRectangle);
#endif
                    _tex_f2b[i].bind();
                    _tex_f2b[i].parameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                    _tex_f2b[i].parameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                    _tex_f2b[i].parameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                    _tex_f2b[i].parameter(TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
#if multisample
                    _tex_f2b[i].image2DMS(_multisamples, PixelInternalFormat.Rg32f, false);
#else
                    _tex_f2b[i].image2D(0, PixelInternalFormat.R32f, 0, PixelFormat.Red, PixelType.Float, IntPtr.Zero);
#endif
                    _tex_f2b[i].unbind();
                }
                // Create FrameBuffer 
                _fbo_f2b = new FrameBuffer();
                _fbo_f2b.bind();
                _fbo_f2b.attachTexture2D(FramebufferAttachment.ColorAttachment0, ref _tex_f2b[0], 0);
                _fbo_f2b.attachTexture2D(FramebufferAttachment.ColorAttachment1, ref _tex_f2b[1], 0);
                _fbo_f2b.checkStatus();
            }
            // DUAL
            {
                // Create Color Texture
                for (int i = 0; i < 2; i++)
                {
#if multisample
                    _tex_dual[i] = new Texture(TextureTarget.Texture2DMultisample);
#else
                    _tex_dual[i] = new Texture(TextureTarget.TextureRectangle);
#endif
                    _tex_dual[i].bind();
                    _tex_dual[i].parameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                    _tex_dual[i].parameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                    _tex_dual[i].parameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                    _tex_dual[i].parameter(TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
#if multisample
                    _tex_dual[i].image2DMS(_multisamples, PixelInternalFormat.Rgb32f, false);
#else
                    _tex_dual[i].image2D(0, PixelInternalFormat.Rgb32f, 0, PixelFormat.Rgb, PixelType.Float, IntPtr.Zero);
#endif
                    _tex_dual[i].unbind();
                }
                // Create FrameBuffer 
                _fbo_dual = new FrameBuffer();
                _fbo_dual.bind();
                _fbo_dual.attachTexture2D(FramebufferAttachment.ColorAttachment0, ref _tex_dual[0], 0);
                _fbo_dual.attachTexture2D(FramebufferAttachment.ColorAttachment1, ref _tex_dual[1], 0);
                _fbo_dual.checkStatus();
            }
            // K_Buffer
            {
                // Create Color Texture
#if multisample
                _tex_k_buffer = new Texture(TextureTarget.Texture2DMultisample);
#else
                _tex_k_buffer = new Texture(TextureTarget.TextureRectangle);
#endif
                _tex_k_buffer.bind();
                _tex_k_buffer.parameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                _tex_k_buffer.parameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                _tex_k_buffer.parameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                _tex_k_buffer.parameter(TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
#if multisample
                _tex_k_buffer.image2DMS(_multisamples, PixelInternalFormat.R32f, false);
#else
                _tex_k_buffer.image2D(0, PixelInternalFormat.R32f, 0, PixelFormat.Red, PixelType.Float, IntPtr.Zero);
#endif
                _tex_k_buffer.unbind();
                // Create FrameBuffer 
                _fbo_k_buffer = new FrameBuffer();
                _fbo_k_buffer.bind();
                _fbo_k_buffer.attachTexture2D(FramebufferAttachment.ColorAttachment0, ref _tex_k_buffer, 0);
                _fbo_k_buffer.checkStatus();
            }
            FrameBuffer.unbind();
        }
        #endregion
        
        #region Initing Functions
        public void initF2B()
        {
            _currID = 0;

            _fbo_f2b.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment1);
            Buffer.setMask(1, true, false, false, false);
            Buffer.clear(0, 0, 0, 0);
            Buffer.setMask(1, true, true, true, true);
        }
        public void initDUAL()
        {
            _currID = 0;

            _fbo_dual.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment1);
            Buffer.setMask(1, true, true, true, false);
            Buffer.clear(0, 0, 0, 0);
            Buffer.setMask(1, true, true, true, true);
        }
        public void initKB()
        {
            _fbo_k_buffer.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Buffer.setMask(0, true, false, false, false);
            Buffer.clear(0, 0, 0, 0);
            Buffer.setMask(0, true, true, true, true);
        }
        #endregion

        #region Compute Functions
        public void compute()
        {
            _fbo.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Buffer.setMask(0, true, false, false, false);
            Buffer.clear(0, 0, 0, 0);

            Depth.disableTest();
            Depth.setMask(false);
            Blend.equation(BlendEquationMode.FuncAdd);
            Blend.func(BlendingFactorSrc.One, BlendingFactorDest.One);
            Blend.enable();

            Example._scene.thickComputing.use();
            {
                Matrix4 tr = Example._scene.meshAnimation.pose.transformation_matrix;
                Example._scene.thickComputing.bindUniformMatrix4("transformation_matrix", false, ref tr);
                Example._scene.meshAnimation.pose.drawElements();
            }
            Shading.close();

            Blend.disable();
            Depth.enableTest();
            Depth.setMask(true);
            Buffer.setMask(0, true, true, true, true);

            FrameBuffer.unbind();
            Buffer.draw(DrawBufferMode.Back);
        }
        public void computeF2B(ref Texture tex_depth, ref Texture tex_front)
        {
            _fbo_f2b.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment0 + _currID);
            Buffer.setMask(_currID, true, false, false, false);
            Buffer.clear(0, 0, 0, 0);

            Example._scene.thickComputing_f2b.use();
            {
                Example._scene.thickComputing_f2b.bindUniform1("layers", _currID);
                Texture.active(TextureUnit.Texture0);  tex_depth.bind();
                Texture.active(TextureUnit.Texture1); _tex_f2b[1 - _currID].bind();
                Texture.active(TextureUnit.Texture2);  tex_front.bind();
                Texture.image.draw();
            }
            Shading.close();

            Buffer.setMask(_currID, true, true, true, true);
            _currID = 1 - _currID;
        }
        public void computeDUAL(ref Texture tex_depth)
        {
            _fbo_dual.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment0 + _currID);
            Buffer.setMask(_currID, true, true, true, false);
            Buffer.clear(0, 0, 0, 0);

            Example._scene.thickComputing_dual.use();
            {
                Example._scene.thickComputing_dual.bindUniform1("layers", _currID);
                Texture.active(TextureUnit.Texture0); tex_depth.bind();
                Texture.active(TextureUnit.Texture1); _tex_dual[1 - _currID].bind();
                Texture.image.draw();
            }
            Shading.close();

            Buffer.setMask(_currID, true, true, true, true);
            _currID = 1 - _currID;
        }
        public void computeK_Buffer(ref Texture tex_depth, bool stencil, bool k_multi)
        {
            if (stencil)
                Example._scene.thickComputing_k_stencil_buffer.use();
            else if (k_multi)
                Example._scene.thickComputing_k_multi_buffer_Z.use();
            else
                Example._scene.thickComputing_k_buffer.use();
            {
                Texture.active(TextureUnit.Texture0);
                tex_depth.bind();
                Texture.image.draw();
            }
            Shading.close();
        }
        #endregion
    }
}