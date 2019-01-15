using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    public class sRGB
    {
        static bool _enabled = false;
        public static bool enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        public static void enable()
        {
            if (_enabled)
                GL.Enable(EnableCap.FramebufferSrgb);
        }
        public static void disable()
        {
            if (_enabled)
                GL.Disable(EnableCap.FramebufferSrgb);
        }
    }
}