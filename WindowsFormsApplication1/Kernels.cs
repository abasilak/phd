using System;
using System.IO;
using Cloo;
using Cloo.Bindings;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace abasilak
{
    public class Kernel
    {
        #region Private Properties

        string _path;
        static readonly string _paths = "../../Kernels/";
        static readonly string _exts  = ".cl";

        ComputeProgram      _program;
        ComputeEventList    _eventList;
        ComputeCommandQueue _commands;
        List<ComputeKernel> _kernels = new List<ComputeKernel>();
       
        #endregion

        #region Public Properties
        public ComputeCommandQueue commands
        {
            get { return _commands; }
        }       
        #endregion

        #region Constructor
        public Kernel(string name, string compile_options, string [] kernel_names)
        {
            _path = _paths + name + _exts;
            StreamReader streamReader = new StreamReader(_path);
            string source = streamReader.ReadToEnd();
            streamReader.Close();

            // Create and build the opencl program.
            _program = new ComputeProgram(Example.context, source);
            _program.Build(Example.context.Devices, compile_options, null, IntPtr.Zero);

            // Create the kernel function and set its arguments.
            ComputeKernel kernel;
            for (int i = 0; i < kernel_names.Length; i++)
            {
                kernel = _program.CreateKernel(kernel_names[i]);
                _kernels.Add(kernel);
            }
            _eventList = new ComputeEventList();
            // Create the command queue. This is used to control kernel execution and manage read/write/copy operations.
            _commands = new ComputeCommandQueue(Example.context, Example.context.Devices[0], ComputeCommandQueueFlags.None);

            Console.WriteLine(_path);
        }
        #endregion

        #region Memory Functions
        public void setMemoryArgument(int k, int index, ComputeMemory memObj)
        {
            _kernels[k].SetMemoryArgument(index, memObj);
        }
        public void setArgument(int k, int index, IntPtr dataSize, IntPtr dataAddr)
        {
            _kernels[k].SetArgument(index, dataSize, dataAddr);
        }
        #endregion

        #region Command Functions
        public void finish()
        {
            _commands.Finish();
        }
        public void acquireGLObjects(List<ComputeMemory> _memObjs)
        {
            _commands.AcquireGLObjects(_memObjs, null);
        }
        public void releaseGLObjects(List<ComputeMemory> _memObjs)
        {
            _commands.ReleaseGLObjects(_memObjs, null);
        }
        #endregion

        #region Execute Functions
        public void execute(int k, long[] workersGlobal, long[] workersLocal)
        {
            _commands.Execute(_kernels[k], null, workersGlobal, workersLocal, null);
        }
        #endregion
     }
}
