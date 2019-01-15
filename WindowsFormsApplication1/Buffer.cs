using System;

using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;

namespace abasilak
{  
    public class Buffer
    {
        private static DrawBuffersEnum[] buffers = new DrawBuffersEnum[8]
        {
            DrawBuffersEnum.ColorAttachment0,
            DrawBuffersEnum.ColorAttachment1,
            DrawBuffersEnum.ColorAttachment2,
            DrawBuffersEnum.ColorAttachment3,
            DrawBuffersEnum.ColorAttachment4,
            DrawBuffersEnum.ColorAttachment5,
            DrawBuffersEnum.ColorAttachment6,
            DrawBuffersEnum.ColorAttachment7
        };

        #region Private Properties

        int             _index;
        IntPtr          _size ;
        BufferTarget    _target;
        BufferUsageHint _usageHint;

        #endregion

        #region Public Properties
        public int index
        {
            get { return _index; }
            set { _index = value; }
        }      
        #endregion

        public Buffer(BufferTarget target, BufferUsageHint usageHint) {
            GL.GenBuffers(1, out _index);
            _target = target;
            _usageHint = usageHint;
        }
        public void delete() { GL.DeleteBuffers(1, ref _index); }

        public void bind    () { GL.BindBuffer(_target, _index); }
        public void unbind  () { GL.BindBuffer(_target,      0); }

        public void storage (IntPtr size, IntPtr data, BufferStorageFlags flags)
        {
            GL.BufferStorage(_target, size, data, flags);
        }

        #region Setting Data Functions
        public void data(IntPtr size) 
        {
            GL.BufferData(_target, size, IntPtr.Zero, _usageHint);
        }
        public void data<T>(ref T Input) where T: struct
        {
            _size = new IntPtr(Marshal.SizeOf(Input));
            GL.BufferData<T>(_target, _size, ref Input, _usageHint);
        }
        public void data<T>(ref T[] Input) where T: struct
        {
            _size = new IntPtr(Input.Length * Marshal.SizeOf(typeof(T)));
            GL.BufferData<T>(_target, _size, Input, _usageHint);
        }
        public void subdata<T>(ref T Input) where T: struct
        {
            _size = new IntPtr(Marshal.SizeOf(Input));
            GL.BufferSubData<T>(_target, IntPtr.Zero, _size, ref Input);
        }
        public void subdata<T>(ref T[] Input) where T : struct
        {
            _size = new IntPtr(Input.Length * Marshal.SizeOf(typeof(T)));
            GL.BufferSubData<T>(_target, IntPtr.Zero, _size, Input);
        }
        #endregion

        #region Clear Functions

        public void clear(PixelInternalFormat internalFormat, PixelFormat format, All type, IntPtr data)
        {
            GL.ClearBufferData(_target, internalFormat, format, type, IntPtr.Zero);
        }
        public void clearsubdata<T>(PixelInternalFormat internalFormat, PixelFormat format, All type, ref T Input) where T : struct
        {
            _size = new IntPtr(Marshal.SizeOf(Input));
            GL.ClearBufferSubData<T>(_target, internalFormat, IntPtr.Zero, _size, format, type, ref Input);
        }

        public void clearsubdata<T>(PixelInternalFormat internalFormat, PixelFormat format, All type, ref T[] Input) where T : struct
        {
            _size = new IntPtr(Input.Length * Marshal.SizeOf(typeof(T)));
            GL.ClearBufferSubData<T>(_target, internalFormat, IntPtr.Zero, _size, format, type, Input);
        }

        public static void clear(float r, float g, float b, float a) { GL.ClearColor(r, g, b, a); Buffer.clear(ClearBufferMask.ColorBufferBit); }
        public static void clear(Color color) { GL.ClearColor(color); Buffer.clear(ClearBufferMask.ColorBufferBit); }
        public static void clear(ClearBuffer buffer, int index, float[] value)
        {
            GL.ClearBuffer(buffer, index, value); 
            //Buffer.clear(ClearBufferMask.ColorBufferBit); 
        }
        public static void clear(ClearBuffer buffer, int index, uint[]  value)
        {   
            GL.ClearBuffer(buffer, index, value);
            //Buffer.clear(ClearBufferMask.ColorBufferBit);
        }
        public static void clear(ClearBufferMask mask) { GL.Clear(mask); }
        #endregion

        #region Masking Functions
        public static void setMask(bool r, bool g, bool b, bool a)            { GL.ColorMask(r, g, b, a); }
        public static void setMask(int index, bool r, bool g, bool b, bool a) { GL.ColorMask(index, r, g, b, a); }
        #endregion

        #region Drawing Functions
        public static void draw (DrawBufferMode mode)  { GL.DrawBuffer(mode); }
        public static void draw(int num, int start) { GL.DrawBuffers(num, ref buffers[start]); }
        #endregion
    }

    public class FrameBuffer
    {
        #region Private Properties

        public int _index;

        #endregion

        public FrameBuffer( ){ GL.GenFramebuffers(1, out _index); }

        #region Attaching Functions
        public void attachTexture(FramebufferAttachment attachment, ref Texture tex, int level)
        {
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attachment, tex.index, level);
        }
        public void attachTexture2D(FramebufferAttachment attachment, ref Texture tex, int level)
        {
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, tex.target, tex.index, level);
        }
        public void attachTexture3D(FramebufferAttachment attachment, ref Texture tex, int level, int zoffset)
        {
            GL.FramebufferTexture3D(FramebufferTarget.Framebuffer, attachment, tex.target, tex.index, level, zoffset);
        }
        public void attachTextureLayer(FramebufferAttachment attachment, ref Texture tex, int level, int layer)
        {
            GL.FramebufferTextureLayer(FramebufferTarget.Framebuffer, attachment, tex.index, level, layer);
        }
        #endregion

        public void          bind() { GL.BindFramebuffer(FramebufferTarget.Framebuffer, _index); }
        public static void unbind() { GL.BindFramebuffer(FramebufferTarget.Framebuffer,      0); }
        public void parameter(FramebufferDefaultParameter pname, int param) { GL.FramebufferParameter(FramebufferTarget.Framebuffer, pname, param); }
        public void delete() { GL.DeleteFramebuffers(1, ref _index); }
        public bool checkStatus()
        {
            switch(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer))
            {
                case FramebufferErrorCode.FramebufferComplete:
                    {
                        Trace.WriteLine("FBO: The framebuffer is complete and valid for rendering.");
                        return true;
                    }
                case FramebufferErrorCode.FramebufferIncompleteAttachment:
                    {
                        Trace.WriteLine("FBO: One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteMissingAttachment:
                    {
                        Trace.WriteLine("FBO: There are no attachments.");
                        break;
                    }
                /* case  FramebufferErrorCode.GL_FRAMEBUFFER_INCOMPLETE_DUPLICATE_ATTACHMENT_EXT: 
                     {
                         Trace.WriteLine("FBO: An object has been attached to more than one attachment point.");
                         break;
                     }*/
                case FramebufferErrorCode.FramebufferIncompleteDimensionsExt:
                    {
                        Trace.WriteLine("FBO: Attachments are of different size. All attachments must have the same width and height.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteFormatsExt:
                    {
                        Trace.WriteLine("FBO: The color attachments have different format. All color attachments must have the same format.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteDrawBuffer:
                    {
                        Trace.WriteLine("FBO: An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteReadBuffer:
                    {
                        Trace.WriteLine("FBO: The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferUnsupported:
                    {
                        Trace.WriteLine("FBO: This particular FBO configuration is not supported by the implementation.");
                        break;
                    }
                default:
                    {
                        Trace.WriteLine("FBO: Status unknown. (yes, this is really bad.)");
                        break;
                    }
            }
            return false;
        }
    }
}