using System;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace abasilak
{
    public class VertexArray
    {
        #region Private Properties

        int _index;
        
        #endregion

        public VertexArray(){ GL.GenVertexArrays(1, out _index); }
        public void delete(){ GL.DeleteVertexArrays(1, ref _index); }
        public void bind()  { GL.BindVertexArray(_index); }
        public void unbind(){ GL.BindVertexArray(0); }

        #region Attribute Functions
        public void enableAttrib (int i) { GL.EnableVertexAttribArray(i); }
        public void disableAttrib(int i) { GL.DisableVertexAttribArray(i); }
        public void setAttribPointer<T>(int i, VertexAttribPointerType Type, bool normalized, IntPtr pointer) where T: struct
        {
            int size   = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Length;
            int stride = Marshal.SizeOf(typeof(T));
            GL.VertexAttribPointer(i, size, Type, normalized, stride, pointer);
        }
        #endregion 
    }
}