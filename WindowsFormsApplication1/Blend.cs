using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    public class Blend
    {
        public static void enable()       { GL.Enable (EnableCap.Blend); }
        public static void enable(int i)  { GL.Enable (IndexedEnableCap.Blend, i); }    
        
        public static void disable()      { GL.Disable(EnableCap.Blend); }
        public static void disable(int i) { GL.Disable(IndexedEnableCap.Blend, i); }

        public static void equation(BlendEquationMode modeRGBA) { GL.BlendEquation(modeRGBA); }
        public static void equation(int i, BlendEquationMode modeRGBA) { GL.BlendEquation(i, modeRGBA); }
        public static void equationSeparate(BlendEquationMode modeRGB, BlendEquationMode modeA) { GL.BlendEquationSeparate(modeRGB, modeA); }
        
        public static void func(BlendingFactorSrc src, BlendingFactorDest dst) { GL.BlendFunc(src, dst); }
        public static void func(int i, BlendingFactorSrc src, BlendingFactorDest dst) { GL.BlendFunc(i, src, dst); }
        public static void funcSeparate(BlendingFactorSrc srcRGB, BlendingFactorDest  dstRGB,
                                        BlendingFactorSrc srcA  , BlendingFactorDest dstA) { GL.BlendFuncSeparate(srcRGB, dstRGB, srcA, dstA); }
    }
}