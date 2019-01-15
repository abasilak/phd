using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    public class Depth
    {
        static double _depth = 1.0;
        public static double depth
        {
            get
            {
                return _depth;
            }
            set
            {
                _depth = value;
            }
        }

        public static void enableTest()  { GL.Enable(EnableCap.DepthTest);  }
        public static void enableClamp() { GL.Enable(EnableCap.DepthClamp); }
        public static void disableTest() { GL.Disable(EnableCap.DepthTest); }
        public static void disableClamp(){ GL.Disable(EnableCap.DepthClamp);}
        public static void setMask(bool mask)
        {
            GL.DepthMask(mask);
        }
        public static void clear()
        {
            float _d;
            //if (!Example._scene.camera.inverseZ)
            {
                _d = (float)_depth;
               // Depth.range(0.0, Depth.depth);
               // Depth.func(DepthFunction.Lequal);
            }
            //else
            {
                //_d = 0.0f;
               // Depth.range(Depth.depth, 0.0);
              //  Depth.func(DepthFunction.Gequal);
            }
            GL.ClearBuffer(ClearBuffer.Depth, 0, ref _d);
            //GL.ClearDepth(_d); GL.Clear(ClearBufferMask.DepthBufferBit);
        }
        public static void func(DepthFunction func) { GL.DepthFunc(func); }
        //public static void range(double near, double far) { GL.DepthRange(near,far); }
    }
}