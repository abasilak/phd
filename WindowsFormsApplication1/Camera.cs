using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    public class Camera
    {
        #region Private Properties

        struct UniformBuffer
        {
            public Matrix4 projection;
            public Matrix4 modelview;
            public Matrix4 normal;
        };

        int             _x, _y, _width, _height;
        View            _view;
        bool            _animation, _inverseZ, _clipping, _shiftKey, _controlKey;
        float           _aspect, _zNear, _zFar, _fov, _dis;
        Vector3         _angle;
        Vector3         _eye, _center, _up;
        Vector3         _u_eye, _u_center, _u_up;
        
        Buffer          _buffer;
        UniformBuffer   _ubo;
               
        #endregion

        #region Public Properties

        public enum View { User, Front, Top, Right };
        public View view
        {
            get { return _view; }
            set { _view = value;}
        }
        public bool clipping
        {
            get { return _clipping; }
            set { _clipping = value; }
        }
        public bool inverseZ
        {
            get { return _inverseZ; }
            set { _inverseZ = value; }
        }
        public bool animation
        {
            get { return _animation; }
            set { _animation = value; }
        }              
        public bool shiftKey
        {
            get { return _shiftKey; }
            set { _shiftKey = value; }
        }
        public bool controlKey
        {
            get { return _controlKey; }
            set { _controlKey = value; }
        }
        public float zNear
        {
            get { return _zNear; }
            set { _zNear = value; }
        }
        public float zFar
        {
            get { return _zFar; }
            set { _zFar = value; }
        }
        public float fov
        {
            get { return _fov; }
            set { _fov = value; }
        }
        public Buffer buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }
        public Vector3 eye
        {
            get { return _eye; }
            set { _eye= value; }
        }
        public Vector3 u_eye
        {
            get { return _u_eye; }
            set { _u_eye = value; }
        }
        
        public Vector3 center
        {
            get { return _center; }
        }

        public Vector3 angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        public int x
        {
            get { return _x; }
            set { _x = value; }
        }
        public int y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion

        public Camera(int width, int height, Vector3 eye, Vector3 center, Vector3 up, float dis)
        {
            _x = _y = -1;
            _view = View.User;
            _zNear = 0.001f;
            _zFar = 50f;
            _fov = MathHelper.PiOver4;
            _aspect = 0f;
            _angle = new Vector3(0.1f);
            _shiftKey = _controlKey = false;
            _width  = width;
            _height = height;
            _eye = _u_eye = eye;
            _center = _u_center = center;
            _up = _u_up = up;
            _dis = dis;
            _clipping = true;
            _inverseZ = false;
            _animation = false;
            _buffer = new Buffer(BufferTarget.UniformBuffer, BufferUsageHint.DynamicDraw);
        }
        
        public void delete()
        {
            _buffer.delete(); 
        }
        public void update()
        {
            _buffer.bind();
            _buffer.data<UniformBuffer>(ref _ubo);
        }
        public void reAlloc(Vector3 eye, Vector3 center, float dis)
        {
            _eye = _u_eye = eye;
            _center = _u_center = center;
            _dis = dis;
        }

        public void load_projection_matrix(Vector3 b1, Vector3 b2, bool z_fighting)
        {
            GL.Viewport(0, 0, _width, _height);
            _aspect = _width / (float)(_height);

            Vector3 dirEye = _center - _eye;
            dirEye.Normalize();

            if (_clipping)
            {
                _zNear = 0.4f * Math.Min(Vector3.Dot(b1 - _eye, dirEye), Vector3.Dot(b2 - _eye, dirEye));
                _zFar = 1.7f * Math.Max(Vector3.Dot(b1 - _eye, dirEye), Vector3.Dot(b2 - _eye, dirEye));
            }
            else
            {
                _zNear = 0.001f;
                _zFar = 50f;
            }

            if (_zNear < 0.0f)
                _zNear = 0.0001f;

            if(z_fighting)
                _zNear += 0.01f;

            Matrix4.CreatePerspectiveFieldOfView(_fov, _aspect, _zNear, _zFar, out _ubo.projection);
        }
        public void load_modelview_matrix()
        {
            _ubo.modelview = Matrix4.LookAt(_eye, _center, _up);
            _ubo.normal = Matrix4.Invert(_ubo.modelview);
            _ubo.normal.Transpose();
        }
        public void change_view()
        {
            if (_view == View.User)
            {
                _eye = _u_eye;
                _center = _u_center;
                _up = _u_up;
            }
            else
            {
                _u_eye = _eye;
                _u_center = _center;
                _u_up = _up;
                if (_view == View.Top)
                {
                    _eye = _center;
                    _eye.Z += 2 * _dis;
                    _up = Vector3.UnitY;
                }
                else if (_view == View.Front)
                {
                    _eye = _center;
                    _eye.Y += 2 * _dis;
                    _up = Vector3.UnitZ;
                }
                else if (_view == View.Right)
                {
                    _eye = _center;
                    _eye.X += 2 * _dis;
                    _up = Vector3.UnitZ;
                }
            }
        }
        public void mouse(int newX, int newY, MouseButtons mouse_button)
        {
            if (!_controlKey && !_shiftKey) return;

            if (_view != View.User) _view = View.User;

            float DY = (float)(newY - _y) / (float)_height;

            if (_controlKey && mouse_button == MouseButtons.Left)
                _eye = _center + (_eye - _center) * (DY + 1f);
            else if (_shiftKey)
            {
                float DX = (float)(newX - _x) / (float)_width;
                Vector3 dirX = _up, dirY = _center - _eye;
                Vector3.Cross(ref _up, ref dirY, out dirY);
                dirY.Normalize();
                if (mouse_button == MouseButtons.Left)
                {
                    _eye = rotatePoint(_eye, _center, dirX, (float)(-DX * Math.PI));
                    _eye = rotatePoint(_eye, _center, dirY, (float)( DY * Math.PI));
                    _up  = rotatePoint(_center + _up, _center, dirY, (float)(DY * Math.PI)) - _center;
                    _up.Normalize();
                }
                else if (mouse_button == MouseButtons.Right)
                {
                    Vector3 tmp = _eye - _center;
                    float length = 2f * (float)Math.Tan(_fov / 2f) * tmp.Length;
                    _center += dirY * DX * length * _aspect + dirX * DY * length;
                }
            }
            _x = newX; _y = newY;
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

