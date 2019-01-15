using System;
using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    public abstract class Transparency
    {
        #region Protected Properties
        
        protected const int _multisamples = 2;

        protected Texture _tex_blend;
        protected FrameBuffer _fbo_blend;

        protected Texture _tex_color;
        protected FrameBuffer _fbo_color;

        protected Shader _init, _render;
        protected Shading _initing, _rendering;

        #endregion

        #region Public Properties

        public Shading initing
        {
            get
            {
                return _initing;
            }
        }
        public FrameBuffer fbo_blend
        {
            get
            {
                return _fbo_blend;
            }
        }              

        #endregion

        #region Delete Functions

        public    virtual  void delete()
        {
            _tex_blend.delete();
            _fbo_blend.delete();

            _tex_color.delete();
            _fbo_color.delete();

            _render.delete();
            _rendering.delete();
        }

        #endregion 
        
        #region Buffer Functions
        protected virtual  void initBuffers()
        {
            // Create Color Texture 
#if multisample
            _tex_color = new Texture(TextureTarget.Texture2DMultisample);
#else
            _tex_color = new Texture(TextureTarget.TextureRectangle);
#endif
            _tex_color.bind();
            _tex_color.parameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            _tex_color.parameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            _tex_color.parameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            _tex_color.parameter(TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

#if multisample
            _tex_color.image2DMS(_multisamples, PixelInternalFormat.Rgba, false);
#else
            _tex_color.image2D(0, PixelInternalFormat.Rgba, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
#endif
            _tex_color.unbind();

            // Create Color FrameBuffer 
            _fbo_color = new FrameBuffer();
            _fbo_color.bind();
            _fbo_color.attachTexture2D(FramebufferAttachment.ColorAttachment0, ref _tex_color, 0);
            _fbo_color.checkStatus();
            FrameBuffer.unbind();        
        }
        #endregion 

        #region Abstract Functions

        public abstract void clearBuffers();
        public abstract void draw(bool useTexture);
        protected abstract void initShaders();

        #endregion
    }

    #region Transparency Inherited Classes

    public class AverageColors : Transparency
    {
        #region Private Properties
        Texture _tex_layers;
        #endregion

        #region Constructor
        public AverageColors()
        {
            initShaders();
            initBuffers();
        }
        #endregion 

        #region Delete Functions
        public new void delete()
        {
            base.delete();
            _init.delete();
            _initing.delete();
            _tex_layers.delete();
        }
        #endregion 

        #region Buffer Functions
        public override void clearBuffers()
        {
            _fbo_blend.bind();
            Buffer.draw(2, 0);
            Buffer.clear(0, 0, 0, 0);
           /* 
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Buffer.clear(0, 0, 0, 0);
            Buffer.draw(DrawBufferMode.ColorAttachment1);
            Buffer.clear(0, 0, 0, 0);
            Buffer.draw(2, 0);
            * */
        }
        protected new void initBuffers()
        {
            base.initBuffers();
            // Create Blend Texture
#if multisample
            _tex_blend = new Texture(TextureTarget.Texture2DMultisample);
#else
            _tex_blend = new Texture(TextureTarget.TextureRectangle);
#endif
            _tex_blend.bind();
            _tex_blend.parameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            _tex_blend.parameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            _tex_blend.parameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            _tex_blend.parameter(TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
#if multisample
            _tex_blend.image2DMS(_multisamples, PixelInternalFormat.Rgba32f, false);
#else
            _tex_blend.image2D(0, PixelInternalFormat.Rgba32f, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
#endif
            _tex_blend.unbind();

            // Create Layers Texture
#if multisample
            _tex_layers = new Texture(TextureTarget.Texture2DMultisample);
#else
            _tex_layers = new Texture(TextureTarget.TextureRectangle);
#endif
            _tex_layers.bind();
            _tex_layers.parameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            _tex_layers.parameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            _tex_layers.parameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            _tex_layers.parameter(TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
#if multisample
            _tex_layers.image2DMS(_multisamples, PixelInternalFormat.R32f, false);
#else
            _tex_layers.image2D(0, PixelInternalFormat.R32f, 0, PixelFormat.Red, PixelType.Float, IntPtr.Zero);
#endif
            _tex_layers.unbind();

            // Create FrameBuffer 
            _fbo_blend = new FrameBuffer();
            _fbo_blend.bind();
            _fbo_blend.attachTexture2D(FramebufferAttachment.ColorAttachment0, ref _tex_blend, 0);
            _fbo_blend.attachTexture2D(FramebufferAttachment.ColorAttachment1, ref _tex_layers, 0);
            _fbo_blend.checkStatus();
            FrameBuffer.unbind();
        }
        #endregion 

        #region Shader Functions
        protected override void initShaders()
        {
            _init = new Shader("rendering/transparency/average/init", ShaderType.FragmentShader);
            _init.complile(ShaderType.FragmentShader);

            _initing = new Shading();
            _initing.create();
            if (Example._scene.tessellation)
            {
                _initing.attachShader(Example._scene.renderVertexTess.id);
                _initing.attachShader(Example._scene.renderTessellationControl.id);
                _initing.attachShader(Example._scene.renderTessellationEvaluation.id);
            }
            else
                _initing.attachShader(Example._scene.renderVertex.id);
            _initing.attachShader(Example._scene.renderGeometry.id);
            _initing.attachShader(Example._scene.phongFragment.id);
            _initing.attachShader(Example._scene.computePixelColor.id);
            _initing.attachShader(_init.id);
            _initing.link();

            _initing.use();
            {
                _initing.bindBuffer(0, "Camera", Example._scene.camera.buffer.index, BufferRangeTarget.UniformBuffer);
                _initing.bindBuffer(1, "Light", Example._scene.light.buffer.index, BufferRangeTarget.UniformBuffer);
            }
            Shading.close();

            _render = new Shader("rendering/transparency/average/render", ShaderType.FragmentShader);
            _render.complile(ShaderType.FragmentShader);

            _rendering = new Shading();
            _rendering.create();
            _rendering.attachShader(Texture.vertex.id);
            _rendering.attachShader(_render.id);
            _rendering.link();

            _rendering.use();
            {
                _rendering.bindUniformMatrix4("modelview_matrix", false, ref Texture.modelview_matrix);

                _rendering.bindUniform1("width", Example._scene.width);
                _rendering.bindUniform1("height", Example._scene.height);
            }
            Shading.close();
        }
        #endregion 

        #region Drawing Functions
        public override void draw(bool useTexture)
        {
            _fbo_color.bind();
            Depth.clear();
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Buffer.clear(0, 0, 0, 0);

            _rendering.use();
            {
                Texture.active(TextureUnit.Texture0); _tex_blend.bind();
                Texture.active(TextureUnit.Texture1); _tex_layers.bind();
                if (useTexture)
                {
                    Texture.active(TextureUnit.Texture2); 
                    Example._scene.backgroundTexture.bind();
                    Texture.sampler.bind(2);
                }
                else
                    _rendering.bindUniform4("color_background", Example._scene.backgroundColor);
                _rendering.bindUniform1("useTexture", useTexture ? 1 : 0);
                Texture.image.draw();
            }
            Shading.close();

            FrameBuffer.unbind();
            Buffer.draw(DrawBufferMode.Back);

            Depth.clear();
            Buffer.clear(0, 0, 0, 0);
            _tex_color.draw();
        }
        #endregion 
    }

    public abstract class WeightSum : Transparency
    {
        #region Constructor

        public WeightSum()
        {
            initBuffers();
        }

        #endregion 

        #region Buffer Functions
        protected new void initBuffers()
        {
            base.initBuffers();
            // Create Blend Texture
#if multisample
            _tex_blend = new Texture(TextureTarget.Texture2DMultisample);
#else
            _tex_blend = new Texture(TextureTarget.TextureRectangle);
#endif
            _tex_blend.bind();
            _tex_blend.parameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            _tex_blend.parameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            _tex_blend.parameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            _tex_blend.parameter(TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
#if multisample
            _tex_blend.image2DMS(_multisamples, PixelInternalFormat.Rgba, false);
#else
            _tex_blend.image2D(0, PixelInternalFormat.Rgba, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
#endif
            _tex_blend.unbind();

            // Create Blend FrameBuffer 
            _fbo_blend = new FrameBuffer();
            _fbo_blend.bind();
            _fbo_blend.attachTexture2D(FramebufferAttachment.ColorAttachment0, ref _tex_blend, 0);
            _fbo_blend.checkStatus();
            FrameBuffer.unbind();
        }
        #endregion 

        #region Abstract Functions

        public abstract void clearBuffersBack(float r, float g, float b, float a);
        public abstract void compute(Texture tex_color);
        public abstract void compute(Texture tex_color_front, Texture tex_color_back);

        #endregion
    }

    #region WeightSum Inherited Classes

    public class WeightSumSimple : WeightSum
    {

        #region Constructor
        public WeightSumSimple() : base()
        {
            initShaders();
        }
        #endregion 

        #region Delete Functions
        public new void delete()
        {
            base.delete();
            _init.delete();
            _initing.delete();
        }
        #endregion 

        #region Shader Functions
        protected override void initShaders()
        {
            _init = new Shader("rendering/transparency/wsum/init", ShaderType.FragmentShader);
            _init.complile(ShaderType.FragmentShader);

            _initing = new Shading();
            _initing.create();
            if (Example._scene.tessellation)
            {
                _initing.attachShader(Example._scene.renderVertexTess.id);
                _initing.attachShader(Example._scene.renderTessellationControl.id);
                _initing.attachShader(Example._scene.renderTessellationEvaluation.id);
            }
            else
                _initing.attachShader(Example._scene.renderVertex.id);
            
            _initing.attachShader(Example._scene.renderGeometry.id);
            _initing.attachShader(Example._scene.phongFragment.id);
            _initing.attachShader(Example._scene.computePixelColor.id);
            _initing.attachShader(_init.id);
            _initing.link();

            _initing.use();
            {
                _initing.bindBuffer(0, "Camera", Example._scene.camera.buffer.index, BufferRangeTarget.UniformBuffer);
                _initing.bindBuffer(1, "Light", Example._scene.light.buffer.index, BufferRangeTarget.UniformBuffer);
            }
            Shading.close();

            _render = new Shader("rendering/transparency/wsum/render", ShaderType.FragmentShader);
            _render.complile(ShaderType.FragmentShader);

            _rendering = new Shading();
            _rendering.create();
            _rendering.attachShader(Texture.vertex.id);
            _rendering.attachShader(_render.id);
            _rendering.link();

            _rendering.use();
            {
                _rendering.bindUniformMatrix4("modelview_matrix", false, ref Texture.modelview_matrix);

                _rendering.bindUniform1("width", Example._scene.width);
                _rendering.bindUniform1("height", Example._scene.height);
            }
            Shading.close();
        }
        #endregion 

        #region Buffer Functions
        public override void clearBuffers()
        {
            _fbo_blend.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Buffer.clear(0, 0, 0, 0);
        }
        #endregion 

        #region Drawing Functions
        public override void draw(bool useTexture)
        {
            _fbo_color.bind();
            Depth.clear();
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Buffer.clear(0, 0, 0, 0);

            _rendering.use();
            {
                Texture.active(TextureUnit.Texture0); _tex_blend.bind();
                if (useTexture)
                {
                    Texture.active(TextureUnit.Texture1); 
                    Example._scene.backgroundTexture.bind();
                    Texture.sampler.bind(1);
                }
                else
                    _rendering.bindUniform4("color_background", Example._scene.backgroundColor);
                _rendering.bindUniform1("useTexture", useTexture ? 1 : 0);
                Texture.image.draw();
            }
            Shading.close();

            FrameBuffer.unbind();
            Buffer.draw(DrawBufferMode.Back);

            Depth.clear();
            Buffer.clear(0, 0, 0, 0);
            _tex_color.draw();
        }
        #endregion 

        #region Empty Functions

        public override void clearBuffersBack(float r, float g, float b, float a) { ;}
        public override void compute(Texture tex_color) { ;}
        public override void compute(Texture tex_color_front, Texture tex_color_back) { ;}

        #endregion
    }
    public class WeightSumF2B : WeightSum
    {
        #region Constructor
        public WeightSumF2B() : base()
        {
            initShaders();
        }
        #endregion 

        #region Shader Functions
        protected override void initShaders()
        {
            _render = new Shader("rendering/transparency/wsum/f2b/render", ShaderType.FragmentShader);
            _render.complile(ShaderType.FragmentShader);

            _rendering = new Shading();
            _rendering.create();
            _rendering.attachShader(Texture.vertex.id);
            _rendering.attachShader(_render.id);
            _rendering.link();

            _rendering.use();
            {
                _rendering.bindUniformMatrix4("modelview_matrix", false, ref Texture.modelview_matrix);
             
                _rendering.bindUniform1("width", Example._scene.width);
                _rendering.bindUniform1("height", Example._scene.height);
            }
            Shading.close();
        }
        #endregion 

        #region Buffer Functions
        public override void clearBuffers()
        {
            _fbo_blend.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Buffer.clear(0, 0, 0, 1);
        }
        #endregion

        #region Compute Functions
        public override void compute(Texture tex_color)
        {
            _fbo_blend.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment0);

            Depth.disableTest();
            Depth.setMask(false);
            Blend.enable();

            Blend.funcSeparate(BlendingFactorSrc.DstAlpha, BlendingFactorDest.One, BlendingFactorSrc.Zero, BlendingFactorDest.OneMinusSrcAlpha);
            Blend.equation(BlendEquationMode.FuncAdd);       
            {
                Texture.rendering.use();
                {
                    Texture.active(TextureUnit.Texture0); tex_color.bind();
                    Texture.image.draw();
                }
                Shading.close();
            }
            Blend.disable();
            
            Depth.enableTest();
            Depth.setMask(true);
        }
        #endregion 

        #region Drawing Functions
        public override void draw(bool useTexture)
        {
            _fbo_color.bind();
            Depth.clear();
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Buffer.clear(0, 0, 0, 0);

            _rendering.use();
            {
                Texture.active(TextureUnit.Texture0); _tex_blend.bind();
                if (useTexture)
                {
                    Texture.active(TextureUnit.Texture1); 
                    Example._scene.backgroundTexture.bind();
                    Texture.sampler.bind(1);
                }
                else
                    _rendering.bindUniform4("color_background", Example._scene.backgroundColor);
                _rendering.bindUniform1("useTexture", useTexture ? 1 : 0);
                Texture.image.draw();
            }
            Shading.close();
      
            FrameBuffer.unbind();
            Buffer.draw(DrawBufferMode.Back);

            Depth.clear();
            Buffer.clear(0, 0, 0, 0);
            _tex_color.draw();
        }
        #endregion 

        #region Empty Functions

        public override void clearBuffersBack(float r, float g, float b, float a) { ;}
        public override void compute(Texture tex_color_front, Texture tex_color_back) { ;}

        #endregion

    }
    public class WeightSumDual : WeightSum
    {
        #region Private Properties

        Texture _tex_back;

        #endregion 

        #region Constructor
        public WeightSumDual() : base()
        {
            initBuffers();
            initShaders();
        }
        #endregion 

        #region Shader Functions
        protected override void initShaders()
        {
            _render = new Shader("rendering/transparency/wsum/dual/render", ShaderType.FragmentShader);
            _render.complile(ShaderType.FragmentShader);

            _rendering = new Shading();
            _rendering.create();
            _rendering.attachShader(Texture.vertex.id);
            _rendering.attachShader(_render.id);
            _rendering.link();

            _rendering.use();
            {
                _rendering.bindUniformMatrix4("modelview_matrix", false, ref Texture.modelview_matrix);
            }
            Shading.close();
        }
        #endregion 

        #region Buffer Functions
        protected new void initBuffers()
        {
            base.initBuffers();

            // Create Back Color Texture
#if multisample
            _tex_back = new Texture(TextureTarget.Texture2DMultisample);
#else
            _tex_back = new Texture(TextureTarget.TextureRectangle);
#endif
            _tex_back.bind();
            _tex_back.parameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            _tex_back.parameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            _tex_back.parameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            _tex_back.parameter(TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
#if multisample
            _tex_back.image2DMS(_multisamples, PixelInternalFormat.Rgba, false);
#else
            _tex_back.image2D(0, PixelInternalFormat.Rgba, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
#endif
            _tex_back.unbind();

            _fbo_blend.bind();
            _fbo_blend.attachTexture2D(FramebufferAttachment.ColorAttachment1, ref _tex_back, 0);
            _fbo_blend.checkStatus();
            FrameBuffer.unbind();
        }
        public override void clearBuffers()
        {
            _fbo_blend.bind();  
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Buffer.clear(0, 0, 0, 1);
        }
        public override void clearBuffersBack(float r, float g, float b, float a)
        {
            _fbo_blend.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment1);
            Buffer.clear(r, g, b, a);
        }
        #endregion 

        #region Compute Functions
        public override void compute(Texture tex_color_front, Texture tex_color_back)
        {
            Depth.setMask(false);
            Depth.disableTest();
            Blend.enable();
            
            _fbo_blend.bind();
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Blend.equation(BlendEquationMode.FuncAdd);
            Blend.funcSeparate(BlendingFactorSrc.DstAlpha, BlendingFactorDest.One, BlendingFactorSrc.Zero, BlendingFactorDest.OneMinusSrcAlpha);
            {
                Texture.rendering.use();
                {
                    Texture.active(TextureUnit.Texture0); tex_color_front.bind();
                    Texture.image.draw();
                }
                Shading.close();
            }

            Buffer.draw(DrawBufferMode.ColorAttachment1);
            Blend.equation(BlendEquationMode.FuncAdd);
            Blend.func(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            {
                Texture.rendering.use();
                {
                    Texture.active(TextureUnit.Texture0); tex_color_back.bind();
                    Texture.image.draw();
                }
                Shading.close();
            }

            Blend.disable();
            Depth.enableTest();
            Depth.setMask(true);
        }
        #endregion 

        #region Drawing Functions
        public override void draw(bool useTexture)
        {
            _fbo_color.bind();
            Depth.clear();
            Buffer.draw(DrawBufferMode.ColorAttachment0);
            Buffer.clear(0, 0, 0, 0);

            _rendering.use();
            {
                Texture.active(TextureUnit.Texture0); _tex_blend.bind();
                Texture.active(TextureUnit.Texture1); _tex_back.bind();
                Texture.image.draw();
            }
            Shading.close();

            FrameBuffer.unbind();
            Buffer.draw(DrawBufferMode.Back);

            Depth.clear();
            Buffer.clear(0, 0, 0, 0);
            _tex_color.draw();
        }
        #endregion 

        #region Empty Functions
        public override void compute(Texture tex_color) {;}
        #endregion
    }
    #endregion 

    #endregion
}