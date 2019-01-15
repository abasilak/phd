using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    public class Logic
    {
        public static void enable()              { GL.Enable (EnableCap.ColorLogicOp); }
        public static void disable()             { GL.Disable(EnableCap.ColorLogicOp); }
        public static void operation(LogicOp op) { GL.LogicOp(op); }
    }
}