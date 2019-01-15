using OpenTK.Graphics.OpenGL;
using System;

namespace abasilak
{
    public class Query
    {
        #region Private Properties

        bool        _use;
        uint        _index;
        uint        _samples;
        Int64       _timeElapsed;
        Int64       _totalTimeElapsed;
        QueryTarget _target;

        #endregion

        #region Public Properties

        public bool use
        {
            get
            {
                return _use;
            }
            set
            {
                _use = value;
            }
        }
        public uint samples
        {
            get { return _samples; }
        }
        public Int64 timeElapsed
        {
            get { return _timeElapsed; }
        }
        public Int64 totalTimeElapsed
        {
            get { return _totalTimeElapsed; }
        }

        #endregion

        public Query(QueryTarget t)
        {
            GL.GenQueries(1, out _index);
            _target = t;
            _use    = true;
            _totalTimeElapsed = 0;
        }

        public void delete()
        {
            GL.DeleteQueries(1, ref _index);
        }
        public void begin()
        {
            GL.BeginQuery(_target, _index);
        }
        public void end()
        {
            GL.EndQuery(_target);
        }

        public void restartTime()
        {
            _totalTimeElapsed = 0;
        }
        public void getResult()
        {
            GL.GetQueryObject(_index, GetQueryObjectParam.QueryResult, out _samples);
        }
        public void getTimeElapsed()
        {
            int available=0;
            while (available == 0)
                GL.GetQueryObject(_index, GetQueryObjectParam.QueryResultAvailable, out available); 
            GL.GetQueryObject(_index, GetQueryObjectParam.QueryResult, out _timeElapsed);
            _totalTimeElapsed += _timeElapsed;
        }
        public bool isResultZero(){return (_samples == 0) ? true : false;}

        #region Conditional Rendering Functions
        public void beginConditionalRender()
        {
            GL.BeginConditionalRender(_index, ConditionalRenderType.QueryByRegionWait);       
        }
        public void endConditionalRender()
        {
            GL.EndConditionalRender();
        }
        #endregion
    }
}