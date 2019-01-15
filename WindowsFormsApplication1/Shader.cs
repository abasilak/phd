using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Drawing;
using System.Windows.Forms;

namespace abasilak
{
    public class Shader
    {
        #region Private Properties

        int     _id;
        string  _path;
        static readonly string[] _paths = new string[] { "../../Shaders/Vertex/", "../../Shaders/Geometry/", "../../Shaders/Fragment/", "../../Shaders/Tessellation/", "../../Shaders/Tessellation/", "../../Shaders/Compute/" };
        static readonly string[] _exts  = new string[] { ".vert", ".geom", ".frag", ".tessCo", ".tessEv", ".comp" };

        #endregion

        #region Public Properties
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        #endregion

        public Shader(string name, ShaderType type) {
            int i;

            if      (type == ShaderType.VertexShader  ) i = 0;
            else if (type == ShaderType.GeometryShader) i = 1;
            else if (type == ShaderType.FragmentShader) i = 2;
            else if (type == ShaderType.TessControlShader)      i = 3;
            else if (type == ShaderType.TessEvaluationShader)   i = 4;
            else if (type == ShaderType.ComputeShader)  i = 5;
            else i = -1;

            _path = _paths[i] + name + _exts[i];
        }
        public void complile(ShaderType type)
        {
            _id = GL.CreateShader(type);
            StreamReader streamReader = new StreamReader(_path);

            string source = ""; 
            while (!streamReader.EndOfStream)
            {
                string Line = streamReader.ReadLine();
                string[] currentLine = Line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (currentLine.Length == 0) continue;

                if (currentLine[0].Equals("#include"))
                {
                    StreamReader sReader = new StreamReader("../../Shaders/" + currentLine[1].Substring(1,currentLine[1].Length-2));
                    source += sReader.ReadToEnd() + "\n";
                    sReader.Close();
                }
                else
                    source += Line + "\n";
            }            
            streamReader.Close();

            GL.ShaderSource(_id, source);
            GL.CompileShader(_id);
            Console.WriteLine(_path);
            Console.WriteLine(GL.GetShaderInfoLog(_id));

            int compileResult;
            GL.GetShader(_id, ShaderParameter.CompileStatus, out compileResult);
            if (compileResult != 1)
            {
                Console.WriteLine("Compile Error!");
                Application.Exit(); 
            }
        }
        public void delete() {GL.DeleteShader(_id);}     
    }

    public class Shading
    {
        #region Private Properties
        int     _program;
        #endregion

        public Shading() {;}
        public void attachShader(int shader) { GL.AttachShader(_program, shader); }
        public void detachShader(int shader) { GL.DetachShader(_program, shader); }

        public void create() {_program = GL.CreateProgram();}
        public void delete() {GL.DeleteShader(_program);}
        public void link()   {GL.LinkProgram(_program); Console.WriteLine(GL.GetProgramInfoLog(_program));}
        public void use()    {GL.UseProgram(_program);}
        public static void close()  {GL.UseProgram(0);}

        public int  getUniformLocation(string name)      {return GL.GetUniformLocation(_program, name);}
        public void bindAttribute(int index, string name){GL.BindAttribLocation  (_program, index, name);}
        public void bindFragData (int index, string name){GL.BindFragDataLocation(_program, index, name);}
        public void bindBuffer   (int index, string name, int uboIndex, BufferRangeTarget Target) 
        {
            int uniformBlockIndex = GL.GetUniformBlockIndex(_program, name);
            GL.BindBufferBase(Target, index, uboIndex);
            GL.UniformBlockBinding(_program, uniformBlockIndex, index);
        }

        #region BindUniform Functions

        public void bindUniform1(string name, uint value)
        {
            GL.Uniform1(getUniformLocation(name), value);
        }
        public void bindUniform1(string name, int  value){
            GL.Uniform1(getUniformLocation(name), value);
        }
        public void bindUniform1(string name, float value)
        {
            GL.Uniform1(getUniformLocation(name), value);
        }
        public void bindUniform1_64(string name, UInt64 value)
        {
            GL.Uniform1(getUniformLocation(name), value);
        }
        public void bindUniform1_64(string name, int count, UInt64[] value)
        {
            GL.NV.Uniform1( getUniformLocation(name), count, value);
        }
        public void bindUniform3(string name, Vector3 value)
        {
            GL.Uniform3(getUniformLocation(name), value);
        }
        public void bindUniform4(string name, Color value)
        {
            GL.Uniform4(getUniformLocation(name), value);
        }
        public void bindUniformMatrix4(string name, bool transpose, ref Matrix4 matrix)
        {
            GL.UniformMatrix4(getUniformLocation(name), transpose, ref matrix);
        }

        #endregion
    }
}
