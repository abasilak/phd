using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    public class Material
    {
        #region Private Properties

        public struct UniformBuffer
        {
            Vector4 _ambient;
            Vector4 _diffuse;
            Vector4 _specular;
            float   _shininess;
            float   _ni;
            float   _absorption;
            float   _gaussian_m;
            float   _gaussian_c;

            public UniformBuffer(float sh, float ni, float ab, float gm, float gc, Vector4 amb, Vector4 dif, Vector4 spec)
            {
                _ambient   = new Vector4(amb);
                _diffuse   = new Vector4(dif);
                _specular  = new Vector4(spec);
                _shininess = sh;
                _ni        = ni;
                _absorption= ab;
                _gaussian_m= gm;
                _gaussian_c= gc;
            }

            public Vector4 ambient
            {
                get { return _ambient; }
                set { _ambient = value; }
            }
            public Vector4 diffuse
            {
                get { return _diffuse; }
                set { _diffuse = value; }
            }
            public Vector4 specular
            {
                get { return _specular; }
                set { _specular = value; }
            }
            public float shininess
            {
                get { return _shininess; }
                set { _shininess = value; }
            }
            public float ni
            {
                get { return _ni; }
                set { _ni = value; }
            }
            public float absorption
            {
                get { return _absorption; }
                set { _absorption = value; }
            }
            public float gaussian_m
            {
                get { return _gaussian_m; }
                set { _gaussian_m = value; }
            }
            public float gaussian_c
            {
                get { return _gaussian_c; }
                set { _gaussian_c = value; }
            }
            public void set_color(int index, Vector4 c)
            {
                if      (index == 0) ambient    = new Vector4(c);
                else if (index == 1) diffuse    = new Vector4(c);
                else if (index == 2) specular   = new Vector4(c);
            }
        };

        Buffer  _buffer;
        bool    _changed;

        #endregion 

        #region Public Properties

        public UniformBuffer _ubo;

        public Buffer buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }
        public bool changed
        {
            set
            {
                _changed = value;
            }
        }

        #endregion 

        public Material()
        {
            _ubo = new UniformBuffer(64f, 0.2f, 20.0f, 0.5f, 1.0f, new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 1f, 1f, 1f));
            _buffer = new Buffer(BufferTarget.UniformBuffer, BufferUsageHint.DynamicDraw);
            _changed = true;
        }
        public Material(float sh, float ni, float ab, float gm, float gc, Vector4 amb, Vector4 dif, Vector4 spec)
        {
            _ubo = new UniformBuffer(sh, ni, ab, gm, gc, amb, dif, spec);
            _buffer = new Buffer(BufferTarget.UniformBuffer, BufferUsageHint.DynamicDraw);
            _changed = true;
        }    

        public void delete() { _buffer.delete(); }
        public void update()
        {
            if (_changed)
            {
                _buffer.bind();
                _buffer.data<UniformBuffer>(ref _ubo);
                _buffer.unbind();
                _changed = false;
            }
        }
        public void colorChanged(int index, Color color)
        {
            Vector4 new_color = new Vector4((float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f);
            _ubo.set_color(index, new_color);
        }
        public float shininess
        {
            get
            {
                return _ubo.shininess;
            }
            set
            {
                _ubo.shininess = value;
            }
        }
    }
}
