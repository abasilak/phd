using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    public class Sampling
    {
        public static void enableMask    () { GL.Enable(EnableCap.SampleMask); }
        public static void enableMulti   () { GL.Enable(EnableCap.Multisample); }
        public static void enableShading () { GL.Enable((EnableCap)ArbSampleShading.SampleShadingArb); }
        public static void disableMask   () { GL.Disable(EnableCap.SampleMask); }
        public static void disableMulti  () { GL.Disable(EnableCap.Multisample); }
        public static void disableShading() { GL.Disable((EnableCap)ArbSampleShading.SampleShadingArb); }
        public static void minSampleShading(float value) { GL.MinSampleShading(value); }

        public static void setMask(int index, int mask)  {GL.SampleMask(index, mask);}
    }
}