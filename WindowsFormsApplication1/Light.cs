using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace abasilak
{
    public class Light
    {
        public struct Attenuation
        {
            #region Private Properties

            float _constant;
            float _linear;
            float _quadratic;

            #endregion

            public Attenuation(float c, float l, float q)
            {
                _constant = c;
                _linear = l;
                _quadratic = q;
            }
            public float constant
            {
                get { return _constant; }
                set { _constant = value; }
            }
            public float linear
            {
                get { return _linear; }
                set { _linear = value; }
            }
            public float quadratic
            {
                get { return _quadratic; }
                set { _quadratic = value; }
            }
        };
        
        public struct K
        {
            #region Private Properties

            float _a;
            float _d;
            float _s;

            #endregion

            public K(float aa, float dd, float ss)
            {
                _a = aa;
                _d = dd;
                _s = ss;
            }
            public float a
            {
                get { return _a; }
                set { _a = value; }
            }
            public float d
            {
                get { return _d; }
                set { _d = value; }
            }
            public float s
            {
                get { return _s; }
                set { _s = value; }
            }
        };

        public struct UniformBuffer
        {
            #region Private Properties

            Vector4 _position;
            Vector4 _ambient;
            Vector4 _diffuse;
            Vector4 _specular;

            #endregion

            public Attenuation _attenuation;
            public K           _k;
            public Vector4 position
            {
                get { return _position; }
                set { _position = value; }
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

            public UniformBuffer(Vector4 pos)
            {
                _position    = pos;
                _ambient     = new Vector4(1f, 1f, 1f, 1f);
                _diffuse     = new Vector4(1f, 1f, 1f, 1f);
                _specular    = new Vector4(1f, 1f, 1f, 1f);
                _k           = new K(0.2f, 0.7f, 0.4f);
                _attenuation = new Attenuation(1f, 0.0f, 0.000f);
            }
            public void spot() { _position.W = (_position.W == 0f) ? 1f : 0f; }
            public void set_color(int index, Vector4 c)
            {
                if      (index == 0) ambient    = new Vector4(c);
                else if (index == 1) diffuse    = new Vector4(c);
                else if (index == 2) specular   = new Vector4(c);
            }
        };

        #region Private Properties

        Buffer _buffer;
        bool   _changed;
        bool   _transform;
        Vector3 _up = Vector3.UnitY;

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
        public bool transform
        {
            get
            {
                return _transform;
            }
            set
            {
                _transform = value;
            }
        }
        #endregion

        public Light(Vector4 pos)
        {
            _ubo    = new UniformBuffer(pos);
            _buffer = new Buffer(BufferTarget.UniformBuffer, BufferUsageHint.DynamicDraw);
            _changed= true;
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
            _changed = true;
        }
        public void spot()
        {
            _ubo.spot();
            _changed = true;
        }

        public void mouse(int newX, int newY, MouseButtons mouse_button, ref Camera camera, int width, int height)
        {
            if (!camera.controlKey && !camera.shiftKey) return;
            if ((camera.center - _ubo.position.Xyz).Length == 0.0f) return;

            float DY = (float)(newY - camera.y) / (float)height;

            if (camera.controlKey && mouse_button == MouseButtons.Left)
                _ubo.position = new Vector4(camera.center + (_ubo.position.Xyz - camera.center) * (DY + 1f), _ubo.position.W);
            else if (camera.shiftKey && mouse_button == MouseButtons.Left)
            {
                float DX = (float)(newX - camera.x) / (float)width;
                Vector3 dirX = _up, dirY = camera.center - _ubo.position.Xyz;
                Vector3.Cross(ref _up, ref dirY, out dirY);
                dirY.Normalize();
                _ubo.position = new Vector4(rotatePoint(_ubo.position.Xyz, camera.center, dirX, (float)(-DX * Math.PI)), _ubo.position.W);
                _ubo.position = new Vector4(rotatePoint(_ubo.position.Xyz, camera.center, dirY, (float)(DY * Math.PI)), _ubo.position.W);
                _up = rotatePoint(camera.center + _up, camera.center, dirY, (float)(DY * Math.PI)) - camera.center;
                _up.Normalize();
            }
            camera.x = newX; 
            camera.y = newY;
            _changed = true;
        }
        public Vector3 rotatePoint(Vector3 Pos, Vector3 Origin, Vector3 Direction, float angle)
        {
            Vector3 vNewPos;
            Vector3 vPos = Pos - Origin;

            float x = Direction.X;
            float y = Direction.Y;
            float z = Direction.Z;

            float cosTheta = (float)Math.Cos(angle);
            float sinTheta = (float)Math.Sin(angle);

            vNewPos.X = (cosTheta + (1 - cosTheta) * x * x) * vPos.X;
            vNewPos.X += ((1 - cosTheta) * x * y - z * sinTheta) * vPos.Y;
            vNewPos.X += ((1 - cosTheta) * x * z + y * sinTheta) * vPos.Z;
            vNewPos.Y = ((1 - cosTheta) * x * y + z * sinTheta) * vPos.X;
            vNewPos.Y += (cosTheta + (1 - cosTheta) * y * y) * vPos.Y;
            vNewPos.Y += ((1 - cosTheta) * y * z - x * sinTheta) * vPos.Z;
            vNewPos.Z = ((1 - cosTheta) * x * z - y * sinTheta) * vPos.X;
            vNewPos.Z += ((1 - cosTheta) * y * z + x * sinTheta) * vPos.Y;
            vNewPos.Z += (cosTheta + (1 - cosTheta) * z * z) * vPos.Z;

            return (Origin + vNewPos);
        }
    }
}
