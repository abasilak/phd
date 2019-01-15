using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    public class Stencil
    {
        public static void clear(int value) { GL.ClearStencil(value); Buffer.clear(ClearBufferMask.StencilBufferBit); }
        public static void enable() { GL.Enable(EnableCap.StencilTest); }
        public static void disable(){ GL.Disable(EnableCap.StencilTest); }
        public static void operation(StencilOp fail, StencilOp zfail, StencilOp zpass) { GL.StencilOp(fail, zfail, zpass); }
        public static void func(StencilFunction f, int @r, uint mask ) { GL.StencilFunc(f, r, mask); }
    }
}