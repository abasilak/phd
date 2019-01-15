using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;

namespace abasilak
{
    public static class FPS
    {
        [DllImport("KernelBase.dll")]
        static extern bool QueryPerformanceCounter  (out long lpPerformanceCount);

        [DllImport("KernelBase.dll")]
        static extern bool QueryPerformanceFrequency(out long lpFrequency);

        #region Private Properties
        // CPU
#if FPS
        static int  _maxValue = 0;
        static long _startTimeLocal , _stopTimeLocal , _freqLocal;
        static long _startTimeGlobal, _stopTimeGlobal, _freqGlobal;
#endif
        // GPU
        static Query           _globalTime = new Query(QueryTarget.TimeElapsed);
        static Query           _localTime  = new Query(QueryTarget.TimeElapsed);
        static List<double>    _localPerf  = new List<double>();
        static List<string>    _localNames = new List<string>();

        static PerformanceForm _performanceForm = new PerformanceForm();

        #endregion

        #region Public Properties
#if FPS
        public static int maxValue
        {
            get { return _maxValue;  }
            set { _maxValue = value; }
        }
        public static double durationLocal
        {
            get
            {
                return (double)(_stopTimeLocal - _startTimeLocal) / (double)_freqLocal;
            }
        }
        public static double durationGlobal
        {
            get
            {
                return (double)(_stopTimeGlobal - _startTimeGlobal) / (double)_freqGlobal;
            }
        }
#endif
        #endregion

        #region Init Function
        public static void init()
        {
#if FPS
            _globalTime.use = false;
            beginGlobalCPU();
#endif
        }
        #endregion

        #region Begin Function
        public static void beginLocalCPU()
        {
#if FPS            
            try
            {
                _startTimeLocal = 0;
                _stopTimeLocal = 0;

                if (QueryPerformanceFrequency(out _freqLocal) == false)
                    throw new Exception("High-Performance counter not supported");

                QueryPerformanceCounter(out _startTimeLocal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
#endif
        }
        public static void beginGlobalCPU()
        {
#if FPS
            try
            {
                _startTimeGlobal = 0;
                _stopTimeGlobal = 0;

                if (QueryPerformanceFrequency(out _freqGlobal) == false)
                    throw new Exception("High-Performance counter not supported");

                QueryPerformanceCounter(out _startTimeGlobal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
#endif
        }
        public static void beginGlobal()
        {
#if FPS
            if (_globalTime.use)
            {
                _globalTime.restartTime();
                _globalTime.begin();
            }
#endif
        }
        public static void beginLocal()
        {
#if FPS
            if (_localTime.use)
            {
                _localTime.restartTime();
                _localTime.begin();
            }
#endif
        }
        #endregion

        #region End Function
        public static double endLocalCPU()
        {
#if FPS
            QueryPerformanceCounter(out _stopTimeLocal);

            return durationLocal;
#else
            return 0.0f;
#endif

        }
        public static void endGlobalCPU()
        {
#if FPS
            QueryPerformanceCounter(out _stopTimeGlobal);
#endif
        }
        public static void endGlobal()
        {
#if FPS
            if (_globalTime.use)
            {
                _globalTime.end();
                _globalTime.getTimeElapsed();
            }
#endif
        }
        public static void endLocal(string name)
        {
#if FPS
            if (_localTime.use)
            {
                _localTime.end();
                _localTime.getTimeElapsed();

                _localPerf.Add(_localTime.totalTimeElapsed / 1000000.0);
                _localNames.Add(name);
            }
#endif
        }
        #endregion

        #region Set Text Function
        public static string setText(string text)
        {
#if FPS
            text = "Total: (" + ((float)(1.0/durationGlobal)).ToString() + " fps)" + " - CPU: (" + ((float)durationGlobal).ToString() + " msec), GPU: (" + (_globalTime.totalTimeElapsed / 1000000.0f).ToString() + " msec)";
#endif
            return text;
        }
        #endregion

        #region Delete Function
        public static void delete()
        {
            _localTime.delete();
            _globalTime.delete();
        }
        #endregion

        #region ReValue Use Function
        public static void revalue_use()
        {
            _globalTime.use = !_globalTime.use;
            _localTime.use  = !_localTime.use;
        }
        #endregion

        #region Form Functions
        public static void showLocalForm()
        {
            _performanceForm.Show();
        }      
        public static void updateLocalForm()
        {
#if FPS
            if(_performanceForm.Visible)
                _performanceForm.writeChartErrorData(_localPerf, _localNames);
#endif
        }      
        public static void resetLocal()
        {
#if FPS
            if (_localTime.use)
            {
                _localPerf = new List<double>();
                _localNames = new List<string>();
            }
#endif
        }
        #endregion
    }
}