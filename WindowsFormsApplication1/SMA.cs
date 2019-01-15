using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Windows.Forms;
using LSqrDotNet;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing;

namespace abasilak
{
    public static class Optimization_AxB
    {
        static double[,] _a;
        static double[ ] _b;

        #region Public Properties
        public static double[,] a
        {
            get { return _a; }
            set { _a = value; }
        }
        public static double [] b
        {
            get { return _b; }
            set { _b = value; }
        }
        #endregion
    }

    public abstract class SMA_ErrorData
    {
        #region Protected Properties
        private   int _numBones;
        protected int _numPoses;
        protected int _numVertices;

        protected double _totalErrorKG;
        protected double _totalErrorSME;
        protected double _totalErrorMSE;
        protected double _totalErrorRMSE;
        protected double _totalErrorSTED;
        protected double _totalErrorFrobenius;
        protected double _totalErrorMeanFrobenius;

        protected double[] _poseErrorMinValues;
        protected double[] _poseErrorMaxValues;
        protected double[] _poseErrorMaxMinValues;
        protected double[] _poseErrorFrobenius;
        protected double[] _poseErrorMSE;
        protected double[] _poseErrorRMSE;
        protected double[] _poseErrorKG;
        protected double[] _poseErrorSME;
        protected double[] _poseErrorSTED;

        protected double[] _poseErrorSTED_Spatial;
        protected double[] _poseErrorSTED_Temporal;

        protected DenseMatrix _errorMeanData;
        protected DenseMatrix _errorApproxData;
        protected DenseMatrix _telSTED;
        protected DenseMatrix _telApproxSTED;
        protected DenseMatrix _errorApproxDataSTED;
        protected DenseMatrix _errorApproxDataNormalized;

        // Approximation Data
        protected Vector3[][] _approxData;
        protected Vector3[][] _approxDataFinal;

        // Rest  Pose Corrections
        protected Vector3[] _restPoseCorrections;

        // EigenSkin
        protected bool _enableEigenSkin;
        protected int  _eigenSkinDisplacements;

        protected Buffer[]      _eigenSkinCorrectionsBuffer;
        protected Vector3[][]   _eigenSkinCorrections;
        protected Vector3[][]   _eigenSkinCorrectionsApprox;
        protected DenseMatrix   _eigenSkinCorrectionsData;

        // EigenWeights
        protected bool _enableEigenWeights;
        protected int  _eigenWeightsDisplacements;

        protected Buffer[]    _eigenWeightsCorrectionsBuffer;
        protected Vector4[][] _eigenWeightsCorrections;
        protected Vector4[][] _eigenWeightsCorrectionsApprox;
        protected DenseMatrix _eigenWeightsCorrectionsData;

        protected float  _smaStorage;
        protected string _fittingTime;

        protected Modes.Fitting           _fittingMode;
        protected Mesh3DAnimationSequence _mas;

        protected Texture[] _tex_matrices;
        #endregion

        #region Public Properties
        public string fittingTime
        {
            get { return _fittingTime; }
            set { _fittingTime = value; }
        }
        public Modes.Fitting fittingMode
        {
            get { return _fittingMode; }
            set { _fittingMode = value; }
        }

        public float smaStorage
        {
            get { return _smaStorage; }
        }

        public double totalErrorKG
        {
            get { return _totalErrorKG; }
        }
        public double totalErrorSTED
        {
            get { return _totalErrorSTED; }
        }
        public double totalErrorSME
        {
            get { return _totalErrorSME; }
        }
        public double totalErrorMSE
        {
            get { return _totalErrorMSE; }
        }
        public double totalErrorRMSE
        {
            get { return _totalErrorRMSE; }
        }
        public double[] poseErrorKG
        {
            get { return _poseErrorKG; }
        }
        public double[] poseErrorSME
        {
            get { return _poseErrorSME; }
        }
        public double[] poseErrorMSE
        {
            get { return _poseErrorMSE; }
        }
        public double[] poseErrorRMSE
        {
            get { return _poseErrorRMSE; }
        }
        public double[] poseErrorSTED
        {
            get { return _poseErrorSTED; }
        }
        public Vector3[][] approxData
        {
            get { return _approxData; }
            set { _approxData = value; }
        }
        public Vector3[] restPoseCorrections
        {
            get { return _restPoseCorrections; }
            set { _restPoseCorrections = value; }
        }

        public Vector3[][] approxDataFinal
        {
            get { return _approxDataFinal; }
            set { _approxDataFinal = value; }
        }

        public bool     enableEigenSkin
        {
            get { return _enableEigenSkin; }
            set { _enableEigenSkin = value; }
        }
        public int      eigenSkinDisplacements
        {
            get { return _eigenSkinDisplacements; }
            set { _eigenSkinDisplacements = value; }
        }
        public Buffer[] eigenSkinCorrectionsBuffer
        {
            get { return _eigenSkinCorrectionsBuffer; }
        }

        public bool     enableEigenWeights
        {
            get { return _enableEigenWeights; }
            set { _enableEigenWeights = value; }
        }
        public int      eigenWeightsDisplacements
        {
            get { return _eigenWeightsDisplacements; }
            set { _eigenWeightsDisplacements = value; }
        }
        public Buffer[] eigenWeightsCorrectionsBuffer
        {
            get { return _eigenWeightsCorrectionsBuffer; }
        }
        public Vector4[][] eigenWeightsCorrections
        {
            get { return _eigenWeightsCorrections; }
        }

        public Vector4[][] eigenWeightsCorrectionsApprox
        {
            get { return _eigenWeightsCorrectionsApprox; }
        }

        public Texture[] tex_matrices
        {
            get { return _tex_matrices; }
            set { _tex_matrices = value; }
        }
        #endregion

        #region Constructor
        public SMA_ErrorData(int numV, int numP, int numB, ref Mesh3DAnimationSequence mas)
        {
            _mas = mas;

            _numPoses    = numP;
            _numVertices = numV;
            _numBones    = numB;

            _fittingMode = mas.sma.fittingMode;

            _totalErrorKG = 0.0;
            _totalErrorSME = 0.0;
            _totalErrorSTED = 0.0;
            _totalErrorFrobenius = 0.0;

            _enableEigenSkin = false;
            _eigenSkinDisplacements = 1;

            _enableEigenWeights = false;
            _eigenWeightsDisplacements = 1;

            _smaStorage = 3.0f * _numVertices + // Rest Pose
                          4.0f * _numVertices + // Weights
                          12 * _numPoses * _numBones; // Matrices

            if (_numPoses > 0)
            {
                _poseErrorMinValues = new double[_numPoses];
                _poseErrorMaxValues = new double[_numPoses];
                _poseErrorMaxMinValues = new double[_numPoses];
                _poseErrorFrobenius = new double[_numPoses];
                _poseErrorKG = new double[_numPoses];
                _poseErrorSME = new double[_numPoses];
                _poseErrorMSE = new double[_numPoses];
                _poseErrorRMSE = new double[_numPoses];
                _poseErrorSTED = new double[_numPoses];
                _poseErrorSTED_Spatial = new double[_numPoses];
                _poseErrorSTED_Temporal = new double[_numPoses];

                _errorApproxDataNormalized = new DenseMatrix(_numPoses, _numVertices);
                _errorApproxData = new DenseMatrix(_numPoses, _numVertices);
                _errorApproxDataSTED = new DenseMatrix(_numPoses, _numVertices);
                _telSTED        = new DenseMatrix(_numPoses, _numVertices);
                _telApproxSTED  = new DenseMatrix(_numPoses, _numVertices);
                _errorMeanData  = new DenseMatrix(_numPoses, _numVertices);

                _eigenSkinCorrectionsData   = new DenseMatrix(_numVertices * 3, _numPoses); // inverted dimensions !!
                _eigenSkinCorrections       = new Vector3[_numPoses][];
                _eigenSkinCorrectionsApprox = new Vector3[_numPoses][];
                _eigenSkinCorrectionsBuffer = new Buffer[_numPoses];

                _eigenWeightsCorrectionsData    = new DenseMatrix(_numVertices * 4, _numPoses); // inverted dimensions !!
                _eigenWeightsCorrections        = new Vector4[_numPoses][];
                _eigenWeightsCorrectionsApprox  = new Vector4[_numPoses][];
                _eigenWeightsCorrectionsBuffer  = new Buffer[_numPoses];

                _approxData = new Vector3[_numPoses][];
                _approxDataFinal = new Vector3[_numPoses][];
                for (int PoseID = 0; PoseID < _numPoses; PoseID++)
                {
                    _eigenSkinCorrections[PoseID]           = new Vector3[_numVertices];
                    _eigenSkinCorrectionsApprox[PoseID]     = new Vector3[_numVertices];
                    _eigenSkinCorrectionsBuffer[PoseID]     = new Buffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);

                    _eigenWeightsCorrections[PoseID]        = new Vector4[_numVertices];
                    _eigenWeightsCorrectionsApprox[PoseID]  = new Vector4[_numVertices];
                    _eigenWeightsCorrectionsBuffer[PoseID]  = new Buffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);

                    _approxData[PoseID] = new Vector3[_numVertices];
                    _approxDataFinal[PoseID] = new Vector3[_numVertices];
                }
                _restPoseCorrections = new Vector3[_numVertices];
            }
        }
        #endregion

        #region Get Fitting Mode Text Function
        public abstract string fittingModeText();
        #endregion

        #region Set Color Functions

        public void getValueBetweenTwoFixedColors(      float val, 
                                                        float aR,     float aG,     float aB,
                                                        float bR,     float bG,     float bB,
                                                    out float cR, out float cG, out float cB)
        {
            cR = (float)(bR - aR) * val + aR;      // Evaluated as -255*value + 255.
            cG = (float)(bG - aG) * val + aG;      // Evaluates as 0.
            cB = (float)(bB - aB) * val + aB;      // Evaluates as 255*value + 0.
        }

        public void getHeatMapColor(float val, out float cR, out float cG, out float cB)
        {
            const int     NUM_COLORS = 4;
            float   [,] colors_map = new float[NUM_COLORS,3] { {0,0,1}, {0,1,0}, {1,1,0}, {1,0,0} };

            // A static array of 4 colors:  (blue,   green,  yellow,  red) using {r,g,b} for each.
 
            int idx1;        // |-- Our desired color will be between these two indexes in "color".
            int idx2;        // |
            float fractBetween = 0;  // Fraction between "idx1" and "idx2" where our value is.
 
            if(val <= 0)      {  idx1 = idx2 = 0;            }    // accounts for an input <=0
            else if(val >= 1) {  idx1 = idx2 = NUM_COLORS-1; }    // accounts for an input >=0
            else
            {
                val = val * (NUM_COLORS-1);          // Will multiply value by 3.
                idx1  = (int)Math.Floor(val);        // Our desired color will be after this index.
                idx2  = idx1+1;                      // ... and before this index (inclusive).
                fractBetween = val - (float)idx1;    // Distance between the two indexes (0-1).
            }
 
            cR = (colors_map[idx2,0] - colors_map[idx1,0])*fractBetween + colors_map[idx1,0];
            cG = (colors_map[idx2,1] - colors_map[idx1,1])*fractBetween + colors_map[idx1,1];
            cB = (colors_map[idx2,2] - colors_map[idx1,2])*fractBetween + colors_map[idx1,2];
        }

        public void setVerticesColor()
        {
            Vector3[] colorsVerticesData = new Vector3[_numVertices];
            for (int VertexID = 0; VertexID < _numVertices; VertexID++)
            {
                float value = (float)_errorApproxDataNormalized.At(_mas.sma.selectedPose, VertexID);

                if (_mas.sma.colorHeatMap)
                    getHeatMapColor(value,
                        out colorsVerticesData[VertexID].X, out colorsVerticesData[VertexID].Y, out colorsVerticesData[VertexID].Z);
                else
                    getValueBetweenTwoFixedColors(value,
                        _mas.sma.colorMin.R / 255.0f, _mas.sma.colorMin.G / 255.0f, _mas.sma.colorMin.B / 255.0f,
                        _mas.sma.colorMax.R / 255.0f, _mas.sma.colorMax.G / 255.0f, _mas.sma.colorMax.B / 255.0f,
                        out colorsVerticesData[VertexID].X, out colorsVerticesData[VertexID].Y, out colorsVerticesData[VertexID].Z);
            }
            _mas.pose.setVerticesColor(colorsVerticesData);

            //if (_fittingMode == Modes.Fitting.RP)
              //  _mas.sma.poses[_mas.selectedPose].setVerticesColor(colorsVerticesData);
            //else
                _mas.sma.pose.setVerticesColor(colorsVerticesData);
        }
        #endregion

        #region EigenSkin Functions
        public abstract void computeEigenSkin();
        public          void setEigenSkinApproximation()
        {
            var E = _eigenSkinCorrectionsData.Svd(true);

            var D = E.U * E.W;
            DenseMatrix ApproxD = new DenseMatrix(3 * _numVertices, _eigenSkinDisplacements);
            ApproxD.SetSubMatrix(0, D.RowCount,   0, _eigenSkinDisplacements, D);

            var K = E.VT;
            DenseMatrix ApproxK = new DenseMatrix(_eigenSkinDisplacements, _numPoses);
            ApproxK.SetSubMatrix(0, _eigenSkinDisplacements, 0, D.ColumnCount, K);
            
            var ApproxE = ApproxD * ApproxK;
            
            for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
#if CPU_PARALLEL
                Parallel.For(0, _numVertices, VertexID =>
#else
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                {
                    _eigenSkinCorrectionsApprox[PoseID][VertexID].X = (float)ApproxE.At(3 * VertexID  , PoseID);
                    _eigenSkinCorrectionsApprox[PoseID][VertexID].Y = (float)ApproxE.At(3 * VertexID+1, PoseID);
                    _eigenSkinCorrectionsApprox[PoseID][VertexID].Z = (float)ApproxE.At(3 * VertexID+2, PoseID);
                }
#if CPU_PARALLEL
                );
#endif
        }
        public          void setEigenSkinBuffer()
        {
#if CPU_PARALLEL
            Parallel.For(0, _numPoses, PoseID =>
#else
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
#endif
            {
                _eigenSkinCorrectionsBuffer[PoseID].bind();
                if (!_enableEigenSkin || _mas.selectedRestPose == PoseID)
                {
                    Vector3[] ZERO = new Vector3[_numVertices];
                    _eigenSkinCorrectionsBuffer[PoseID].data<Vector3>(ref ZERO);
                }
                else
                    _eigenSkinCorrectionsBuffer[PoseID].data<Vector3>(ref _eigenSkinCorrectionsApprox[PoseID]);
                _eigenSkinCorrectionsBuffer[PoseID].unbind();
            }
#if CPU_PARALLEL
            );
#endif
        }
        #endregion

        #region EigenWeights Functions
        public abstract void computeEigenWeights();
        public          void setEigenWeightsApproximation()
        {
            return;
            
            var E = _eigenWeightsCorrectionsData.Svd(true);

            var D = E.U * E.W;
            DenseMatrix ApproxD = new DenseMatrix(4 * _numVertices, _eigenWeightsDisplacements);
            ApproxD.SetSubMatrix(0, D.RowCount, 0, _eigenWeightsDisplacements, D);

            var K = E.VT;
            DenseMatrix ApproxK = new DenseMatrix(_eigenWeightsDisplacements, _numPoses);
            ApproxK.SetSubMatrix(0, _eigenWeightsDisplacements, 0, D.ColumnCount, K);

            var ApproxE = ApproxD * ApproxK;

            for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                {
                    _eigenWeightsCorrectionsApprox[PoseID][VertexID].X = (float)ApproxE.At(4 * VertexID    , PoseID);
                    _eigenWeightsCorrectionsApprox[PoseID][VertexID].Y = (float)ApproxE.At(4 * VertexID + 1, PoseID);
                    _eigenWeightsCorrectionsApprox[PoseID][VertexID].Z = (float)ApproxE.At(4 * VertexID + 2, PoseID);
                    _eigenWeightsCorrectionsApprox[PoseID][VertexID].W = (float)ApproxE.At(4 * VertexID + 3, PoseID);
                }
        }
        public          void setEigenWeightsBuffer()
        {
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
                _eigenWeightsCorrectionsBuffer[PoseID].bind();
                if (!_enableEigenWeights || _mas.selectedRestPose == PoseID)
                {
                    Vector4[] ZERO = new Vector4[_numVertices];
                    _eigenWeightsCorrectionsBuffer[PoseID].data<Vector4>(ref ZERO);
                }
                else
                    //_eigenWeightsCorrectionsBuffer[PoseID].data<Vector4>(ref _eigenWeightsCorrectionsApprox[PoseID]);
                    _eigenWeightsCorrectionsBuffer[PoseID].data<Vector4>(ref _eigenWeightsCorrections[PoseID]);
                _eigenWeightsCorrectionsBuffer[PoseID].unbind();
            }
        }
        #endregion

        #region Error Functions
        public abstract void computeApproxData(int PoseID, int VertexID, Vector3 restPose, DenseMatrix transMatrix, bool final);
        public abstract void computeApproxDataError();
        public          void computeFittingError()
        {           
            _totalErrorFrobenius     = _errorApproxData.FrobeniusNorm();
            _totalErrorMeanFrobenius = _errorMeanData.FrobeniusNorm();

            _totalErrorMSE  = (_totalErrorFrobenius * _totalErrorFrobenius) / (_numVertices*_numPoses);
            _totalErrorRMSE = Math.Sqrt(_totalErrorMSE);

            _totalErrorKG  = _totalErrorFrobenius / _totalErrorMeanFrobenius;
            _totalErrorKG *= 100;

            _totalErrorSME  = _totalErrorFrobenius;
            _totalErrorSME /= Math.Sqrt(3.0 * _numVertices * _numPoses);
            _totalErrorSME *= 1000;

            perPoseError();
            normalizeError();

            computeFittingErrorSTED();
        }

        public void computeFittingErrorSTED()
        {
            // 1. Compute Spatial Error
            int     _neighborRing = 1;
            double  _c = 9.144 * Math.Pow(10, -5);
            double  _totalErrorSTED_Spatial = 0.0;
            for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
            {
                _poseErrorSTED_Spatial[PoseID] = 0.0;
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                {
                    // Compute Neighbor Vertices
                    List<int> TestNH        = new List<int>();
                    List<int> neighborhood  = new List<int>();

                    TestNH.Add(VertexID); neighborhood.Add(VertexID);
                    for (int NeighborRange = 0; NeighborRange < _neighborRing; ++NeighborRange)
                    {
                        int [] testNH = TestNH.ToArray(); TestNH.Clear();
                        foreach (var nVertexID in testNH)
                            foreach (int NeighborID in _mas.poses[PoseID].neighborsVerticesData[nVertexID])
                                if (!neighborhood.Contains(NeighborID) && !TestNH.Contains(NeighborID))
                                    TestNH.Add(NeighborID);

                        neighborhood.AddRange(TestNH);
                    }

                    // Compute Neighbor Edges
                    List<Tuple<int, int>> neighborsEdgesData = new List<Tuple<int, int>>();
                    foreach (var nVertexID in neighborhood)
                    {
                        foreach (int NeighborID in _mas.poses[PoseID].neighborsVerticesData[nVertexID])
                        {
                            Tuple<int, int> newEdge = Tuple.Create<int, int>(nVertexID, NeighborID);
                            Tuple<int, int> newEdgeInv = Tuple.Create<int, int>(NeighborID, nVertexID);

                            if (!neighborsEdgesData.Contains(newEdge) && !neighborsEdgesData.Contains(newEdgeInv))
                                neighborsEdgesData.Add(newEdge);
                        }
                    }

                    //Compute AvgED
                    double avgEl = 0.0;
                    double avgEdEl = 0.0;
                    foreach (var EdgeVertexIDs in neighborsEdgesData)
                    {
                        avgEl += computeEL(EdgeVertexIDs.Item1, EdgeVertexIDs.Item2, PoseID, false);
                        avgEdEl += computeED(EdgeVertexIDs.Item1, EdgeVertexIDs.Item2, PoseID) * computeEL(EdgeVertexIDs.Item1, EdgeVertexIDs.Item2, PoseID, false);
                    }
                    double avgEd = avgEdEl / avgEl;

                    //Compute DEV
                    double avg = 0.0;
                    foreach (var EdgeVertexIDs in neighborsEdgesData)
                        avg += Math.Pow(computeED(EdgeVertexIDs.Item1, EdgeVertexIDs.Item2, PoseID) - avgEd, 2) * computeEL(EdgeVertexIDs.Item1, EdgeVertexIDs.Item2, PoseID, false);

                    double ErrorSpatial = Math.Sqrt(avg / avgEl);
                    _errorApproxDataSTED[PoseID, VertexID]   = ErrorSpatial;
                    _poseErrorSTED_Spatial[PoseID]          += ErrorSpatial;
                }
                _totalErrorSTED_Spatial += _poseErrorSTED_Spatial[PoseID];
            }
            _totalErrorSTED_Spatial = Math.Sqrt(_totalErrorSTED_Spatial / (_numVertices * _numPoses));

            // 2. Compute Temporal Error
            int     _window = 5;
            double  _dt = 0.0003;
            float ld = 0; Vector3 Dif;
            for (int VertexID1 = 0; VertexID1 < _numVertices; ++VertexID1)
                for (int VertexID2 = VertexID1 + 1; VertexID2 < _numVertices; ++VertexID2)
                {
                    Vector3.Subtract(ref _mas.poses[0].verticesData[VertexID1], ref _mas.poses[0].verticesData[VertexID2], out Dif);
                    ld = Math.Max(ld, Dif.Length);
                }

            double _totalErrorSTED_Temporal = 0.0;
            for (int PoseID = 0; PoseID < _numPoses-1; ++PoseID)
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                {
                    Vector4 tel = new Vector4(
                        (_mas.poses[PoseID + 1].verticesData[VertexID].X - _mas.poses[PoseID].verticesData[VertexID].X) / ld,
                        (_mas.poses[PoseID + 1].verticesData[VertexID].Y - _mas.poses[PoseID].verticesData[VertexID].Y) / ld,
                        (_mas.poses[PoseID + 1].verticesData[VertexID].Z - _mas.poses[PoseID].verticesData[VertexID].Z) / ld,
                        (float)_dt);

                    _telSTED[PoseID, VertexID] = tel.Length;

                    Vector4 telApprox = new Vector4(
                        (_approxDataFinal[PoseID + 1][VertexID].X - _approxDataFinal[PoseID][VertexID].X) / ld,
                        (_approxDataFinal[PoseID + 1][VertexID].Y - _approxDataFinal[PoseID][VertexID].Y) / ld,
                        (_approxDataFinal[PoseID + 1][VertexID].Z - _approxDataFinal[PoseID][VertexID].Z) / ld,
                        (float)_dt);

                    _telApproxSTED[PoseID, VertexID] = telApprox.Length;
                }

            for (int PoseID = 0; PoseID < _numPoses-1; ++PoseID)
            {
                int minP = Math.Max(0             ,PoseID-_window);
                int maxP = Math.Min(PoseID+_window,_numPoses-1);
                double maxminP = (double)maxP - (double)minP;

                _poseErrorSTED_Temporal[PoseID] = 0.0;
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                {
                    double SS = 0.0;
                    for (int pID = minP; pID < maxP; ++pID)
                        SS += _telSTED[pID, VertexID];    
                    SS /= maxminP;

                    double ErrorTemporal = Math.Abs(_telSTED[PoseID, VertexID] - _telApproxSTED[PoseID, VertexID])/ SS;
                    _errorApproxDataSTED[PoseID, VertexID] =
                        Math.Sqrt(
                                    Math.Pow(_errorApproxDataSTED[PoseID, VertexID], 2) +
                                    Math.Pow(_c, 2) * Math.Pow(ErrorTemporal, 2)
                        );
                   _poseErrorSTED_Temporal[PoseID] += ErrorTemporal;
                }
                _totalErrorSTED_Temporal += _poseErrorSTED_Temporal[PoseID];
            }
            _totalErrorSTED_Temporal = Math.Sqrt(_totalErrorSTED_Temporal / (_numVertices * (_numPoses-1)));

            // 3. Compute Total Error
            _totalErrorSTED = Math.Sqrt(
                Math.Pow(_totalErrorSTED_Spatial, 2) +
                Math.Pow(_c, 2) * Math.Pow(_totalErrorSTED_Temporal, 2)
                );

            perPoseErrorSTED();
            normalizeErrorSTED();        
        }

        #region Per Pose Error Function
        protected void perPoseError()
        {
            DenseVector poseErrors;
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
                poseErrors = new DenseVector(_numVertices);
#if CPU_PARALLEL
                Parallel.For(0, _numVertices, VertexID =>
#else
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                {
                    poseErrors[VertexID] = _errorApproxData.At(PoseID, VertexID);
                }
#if CPU_PARALLEL
                );
#endif
                _poseErrorFrobenius[PoseID] = poseErrors.Norm(2);
                _poseErrorMSE[PoseID] = (_poseErrorFrobenius[PoseID] * _poseErrorFrobenius[PoseID]) / _numVertices;
                _poseErrorRMSE[PoseID] = Math.Sqrt(_poseErrorMSE[PoseID]);
                _poseErrorKG[PoseID] = (_poseErrorFrobenius[PoseID] / _totalErrorMeanFrobenius) * 100.0f;
                _poseErrorSME[PoseID] = (_poseErrorFrobenius[PoseID] / Math.Sqrt(3.0 * _numVertices * _numPoses)) * 1000.0f;
            }
        }
        protected void perPoseErrorSTED()
        {
#if CPU_PARALLEL
                Parallel.For(0, _numPoses, PoseID =>
#else
                for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
#endif
                {
                    _poseErrorSTED[PoseID] = Math.Sqrt(_poseErrorSTED_Spatial[PoseID] / _numVertices);    
                }
#if CPU_PARALLEL
);
#endif
        }
        #endregion

        #region Normalize Error Function
        public void normalizeError()
        {
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
                _poseErrorMinValues[PoseID] = double.MaxValue;
                _poseErrorMaxValues[PoseID] = 0.0;
                for (int VertexID = 0; VertexID < _numVertices; VertexID++)
                {
                    if (_poseErrorMaxValues[PoseID] < _errorApproxData[PoseID, VertexID]) _poseErrorMaxValues[PoseID] = _errorApproxData.At(PoseID, VertexID);
                    if (_poseErrorMinValues[PoseID] > _errorApproxData[PoseID, VertexID]) _poseErrorMinValues[PoseID] = _errorApproxData.At(PoseID, VertexID);
                }
                _poseErrorMaxMinValues[PoseID] = _poseErrorMaxValues[PoseID] - _poseErrorMinValues[PoseID];
            }

            double _minVertexData = double.MaxValue, _maxVertexData = 0.0;
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
                if (_poseErrorMinValues[PoseID] < _minVertexData)
                    _minVertexData = _poseErrorMinValues[PoseID];

                if (_poseErrorMaxValues[PoseID] > _maxVertexData)
                    _maxVertexData = _poseErrorMaxValues[PoseID];
            }

            double _maxminVertexData = _maxVertexData - _minVertexData;
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
#if CPU_PARALLEL
                Parallel.For(0, _numVertices, VertexID =>
#else
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                {
                    if(!_mas.sma.colorTemporalCoherence)
                    {
                        _minVertexData = _poseErrorMinValues[PoseID];
                        _maxVertexData = _poseErrorMaxValues[PoseID];

                        _maxminVertexData = _maxVertexData - _minVertexData;
                    }

                    double normalizedValue = _maxminVertexData == 0.0 ? 0.0 : (_errorApproxData.At(PoseID, VertexID) - _minVertexData) / _maxminVertexData;
                    _errorApproxDataNormalized[PoseID, VertexID] = normalizedValue;
                }
#if CPU_PARALLEL
);
#endif
            }
        }
        public void normalizeErrorSTED()
        {
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
                _poseErrorMinValues[PoseID] = double.MaxValue;
                _poseErrorMaxValues[PoseID] = 0.0;
                for (int VertexID = 0; VertexID < _numVertices; VertexID++)
                {
                    if (_poseErrorMaxValues[PoseID] < _errorApproxDataSTED[PoseID, VertexID]) _poseErrorMaxValues[PoseID] = _errorApproxDataSTED.At(PoseID, VertexID);
                    if (_poseErrorMinValues[PoseID] > _errorApproxDataSTED[PoseID, VertexID]) _poseErrorMinValues[PoseID] = _errorApproxDataSTED.At(PoseID, VertexID);
                }
                _poseErrorMaxMinValues[PoseID] = _poseErrorMaxValues[PoseID] - _poseErrorMinValues[PoseID];
            }

            double _minVertexData = double.MaxValue, _maxVertexData = 0.0;
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
                if (_poseErrorMinValues[PoseID] < _minVertexData)
                    _minVertexData = _poseErrorMinValues[PoseID];

                if (_poseErrorMaxValues[PoseID] > _maxVertexData)
                    _maxVertexData = _poseErrorMaxValues[PoseID];
            }

            double _maxminVertexData = _maxVertexData - _minVertexData;
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
#if CPU_PARALLEL
                Parallel.For(0, _numVertices, VertexID =>
#else
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                {
                    if (!_mas.sma.colorTemporalCoherence)
                    {
                        _minVertexData = _poseErrorMinValues[PoseID];
                        _maxVertexData = _poseErrorMaxValues[PoseID];
                        _maxminVertexData = _maxVertexData - _minVertexData;
                    }

                    double normalizedValue = _maxminVertexData == 0.0 ? 0.0 : (_errorApproxDataSTED.At(PoseID, VertexID) - _minVertexData) / _maxminVertexData;
                    _errorApproxDataNormalized[PoseID, VertexID] = normalizedValue;
                }
#if CPU_PARALLEL
);
#endif
            }
        }
        #endregion

        #endregion

        #region Compute Functions
        public abstract void computeFinalPositions();
        public abstract double computeEL(int VertexID1, int VertexID2, int PoseID, bool approx);
        public double computeED(int VertexID1, int VertexID2, int PoseID)
        {
            return Math.Abs((computeEL(VertexID1, VertexID2, PoseID, false) - computeEL(VertexID1, VertexID2, PoseID, true)) / computeEL(VertexID1, VertexID2, PoseID, false));
        }
        #endregion       
    }

    public class SMA_ErrorDataVertex : SMA_ErrorData
    {
        public SMA_ErrorDataVertex(int numV, int numP, int numB, ref Mesh3DAnimationSequence mas) : base(numV, numP, numB, ref mas)
        {
            _tex_matrices = new Texture[_numPoses];
        }
        public override string fittingModeText()
        {
            string FittingModeText = "";
            if      (_fittingMode == Modes.Fitting.RP               )   FittingModeText = "RP";
            else if (_fittingMode == Modes.Fitting.MP               )   FittingModeText = "MP";
            else if (_fittingMode == Modes.Fitting.P2P_APP_APP      )   FittingModeText = "P2P APP-APP";
            else if (_fittingMode == Modes.Fitting.P2P_APP_APP_RPF  )   FittingModeText = "P2P APP-APP-RPF";
            else if (_fittingMode == Modes.Fitting.P2P_COR_APP      )   FittingModeText = "P2P COR-APP";
            else if (_fittingMode == Modes.Fitting.P2P_COR_COR      )   FittingModeText = "P2P COR-COR";
            return FittingModeText + " VERTEX ";
        }
        public override void computeApproxData(int PoseID, int VertexID, Vector3 restPose, DenseMatrix transMatrix, bool final)
        {
            var newVertex = transMatrix * OpenTK_To_MathNET.Vector4ToVector(new Vector4(restPose, 1.0f));
            if (final)
                OpenTK_To_MathNET.ArrayToVector3(newVertex.ToArray(), out _approxDataFinal[PoseID][VertexID]);
            else
                OpenTK_To_MathNET.ArrayToVector3(newVertex.ToArray(), out _approxData[PoseID][VertexID]);
        }
        public override void computeApproxDataError()
        {
            int RestPose = (_fittingMode == Modes.Fitting.RP || _fittingMode == Modes.Fitting.P2P_APP_APP_RPF) ? _mas.selectedRestPose : 0;
            Mesh3D MeanPose = (_mas.addMeanPoseToTree) ? _mas.poses[_mas.poses.Count - 1] : _mas.meanPose;

            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
#if CPU_PARALLEL
                Parallel.For(0, _numVertices, VertexID =>
#else
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                {
                    // Approximation
                    Vector3 ApproxResult;
                    Vector3.Subtract(ref _mas.poses[PoseID].verticesData[VertexID], ref _approxDataFinal[PoseID][VertexID], out ApproxResult);
                    _errorApproxData[PoseID, VertexID] = ApproxResult.Length;

                    // Mean Pose
                    Vector3 MeanResult;
                    Vector3.Subtract(ref _mas.poses[PoseID].verticesData[VertexID], ref MeanPose.verticesData[VertexID], out MeanResult);
                    _errorMeanData[PoseID, VertexID] = MeanResult.Length;
                }
#if CPU_PARALLEL
                );
#endif
            }

            computeFittingError();
        }
        public override void computeEigenSkin()
        {
            int RestPose = (_fittingMode == Modes.Fitting.RP || _fittingMode == Modes.Fitting.P2P_APP_APP_RPF) ? _mas.selectedRestPose : 0;

            for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
            {
                if (PoseID != RestPose)
                {
#if CPU_PARALLEL
                    Parallel.For(0, _numVertices, VertexID =>
#else
                    for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                    {
                        DenseMatrix transMatrixV, transMatrixN;
                        _mas.sma.computeMatricesProduct(PoseID, VertexID, out transMatrixV, out transMatrixN);

                        var matrix = new DenseMatrix(3, 4);
                        var TransposeInverseMatrix = new DenseMatrix(4, 4);
                        TransposeInverseMatrix.SetDiagonal(new double[] { 0, 0, 0, 1 });
                        TransposeInverseMatrix.SetSubMatrix(0, 3, 0, 4, transMatrixV);

                        matrix.SetSubMatrix(0, 3, 0, 4, TransposeInverseMatrix.Inverse().SubMatrix(0, 3, 0, 4));

                        var vvvv = OpenTK_To_MathNET.Vector4ToVector(new Vector4(_mas.poses[PoseID].verticesData[VertexID], 1.0f));
                        var newE = (DenseMatrix)matrix * vvvv;

                        Vector3 newV;
                        OpenTK_To_MathNET.ArrayToVector3(newE.ToArray(), out newV);

                        Vector3 restPoseVertex = _mas.sma.getRestPoseVertex(PoseID, VertexID, _mas, false);
                        Vector3.Subtract(ref newV, ref restPoseVertex, out _eigenSkinCorrections[PoseID][VertexID]);
                    }
#if CPU_PARALLEL
);
#endif
                }
                else
                {
#if CPU_PARALLEL
                    Parallel.For(0, _numVertices, VertexID =>
#else
                    for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                    {
                        _eigenSkinCorrections[PoseID][VertexID] = Vector3.Zero;
                    }
#if CPU_PARALLEL
);
#endif
                }
#if CPU_PARALLEL
                Parallel.For(0, _numVertices, VertexID =>
#else
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                {
                    _eigenSkinCorrectionsData.At(VertexID * 3    , PoseID, _eigenSkinCorrections[PoseID][VertexID].X);
                    _eigenSkinCorrectionsData.At(VertexID * 3 + 1, PoseID, _eigenSkinCorrections[PoseID][VertexID].Y);
                    _eigenSkinCorrectionsData.At(VertexID * 3 + 2, PoseID, _eigenSkinCorrections[PoseID][VertexID].Z);
                }
#if CPU_PARALLEL
);
#endif
            }
        }
        public override void computeEigenWeights()
        {
            Mesh3DAnimationSequence mas = Example._scene.meshAnimation;
            SMA sma = mas.sma;

            int[]       bData = null;
            float[]     wData = null;

            int RestPose = (_fittingMode == Modes.Fitting.RP || _fittingMode == Modes.Fitting.P2P_APP_APP_RPF) ? _mas.selectedRestPose : 0;

            for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
            {
                if (PoseID != RestPose)
                {
                    for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                    {
                        // Initialize the Coefficient matrix of the System

                        //begin A

                        //Store the Rest Pose vertex coordinates
                        Vector3 restPoseVertex = sma.getRestPoseVertex(PoseID, VertexID, mas, true);
                        var     vRestPose = OpenTK_To_MathNET.Vector4ToVector(new Vector4(restPoseVertex, 1.0f));

                        OpenTK_To_MathNET.Vector4ToArray(sma.bonesData[VertexID], out bData);
                        OpenTK_To_MathNET.Vector4ToArray(sma.weightsData[VertexID], out wData);

                        int lastB = 4;
                        for (int BoneID = 0; BoneID < 4; BoneID++)
                            if (wData[BoneID] == 0.0f)
                            {
                                lastB = BoneID;
                                break;
                            }
                        lastB--;

                        if (lastB > 0)
                        {

                            // int A_rows = 3 * 3;
                            // int A_cols = 3 * 1;
                            double[,] A = new double[3, lastB];
                            double[] B = new double[3];

                            //Fill the appropriate Matrix elements
                            var M3 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(sma.matricesPoseBoneData[PoseID][bData[lastB]]); // oxi to 3 !!!
                            var V3 = M3 * vRestPose;

                            for (int BoneID = 0; BoneID < lastB; BoneID++)
                            {
                                var Mi = OpenTK_To_MathNET.Matrix4ToDenseMatrix(sma.matricesPoseBoneData[PoseID][bData[BoneID]]);
                                var Vi = Mi * vRestPose;

                                A[0, BoneID] = Vi[0] - V3[0];
                                A[1, BoneID] = Vi[1] - V3[1];
                                A[2, BoneID] = Vi[2] - V3[2];
                            }
                            //end A

                            //begin B
                            float[] vVertexError = null;
                            OpenTK_To_MathNET.Vector3ToArray(mas.poses[PoseID].verticesData[VertexID] - _approxDataFinal[PoseID][VertexID], out vVertexError);
                            for (int i = 0; i < 3; ++i) B[i] = vVertexError[i];
                            //end B

                            var matrixA = DenseMatrix.OfArray(A);
                            var vectorB = new DenseVector(B);

                            var resultX = matrixA.QR().Solve(vectorB);

                            float sum = 0.0f;
                            float[] X = new float[4];
                            for (int i = 0; i < lastB; ++i)
                            {
                                X[i] = (float)resultX[i];
                                sum += (float)resultX[i];
                            }

                            X[lastB] = -sum;

                            // Checking
                            bool k = true;
                            //float sumW = 0.0f;
                            for (int i = 0; i < 4; ++i)
                            {
                                //sumW += X[i] + wData[i];

                                if (wData[i] + X[i] < 0.0f)
                                {
                                    k = false;
                                }
                                //Console.WriteLine(""); // !!!!
                            }

                            if (!k)
                                for (int i = 0; i < 4; ++i)
                                    X[i] = 0.0f;

                            OpenTK_To_MathNET.ArrayToVector4(X, out _eigenWeightsCorrections[PoseID][VertexID]);
                        }
                        else
                            _eigenWeightsCorrections[PoseID][VertexID] = Vector4.Zero;
                    }
                }
                else
                {
                    for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                        _eigenWeightsCorrections[PoseID][VertexID] = Vector4.Zero;
                }

                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                {
                    _eigenWeightsCorrectionsData.At(VertexID * 4    , PoseID, _eigenWeightsCorrections[PoseID][VertexID].X);
                    _eigenWeightsCorrectionsData.At(VertexID * 4 + 1, PoseID, _eigenWeightsCorrections[PoseID][VertexID].Y);
                    _eigenWeightsCorrectionsData.At(VertexID * 4 + 2, PoseID, _eigenWeightsCorrections[PoseID][VertexID].Z);
                    _eigenWeightsCorrectionsData.At(VertexID * 4 + 3, PoseID, _eigenWeightsCorrections[PoseID][VertexID].W);
                }
            }              
        }
        public override void computeFinalPositions()
        {
            for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
#if !CPU_PARALLEL
                    Parallel.For(0, _numVertices, VertexID =>
#else
                    for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                    {
                        Vector3 RestPosePos = _mas.sma.getRestPoseVertex(PoseID, VertexID, _mas, false);

                        Vector3 CorrPos = (_enableEigenSkin) ? new Vector3(_eigenSkinCorrectionsApprox[PoseID][VertexID]) : Vector3.Zero;

                        Vector3 FinalPos;
                        Vector3.Add(ref RestPosePos, ref CorrPos, out FinalPos);

                        var transMatrixN = new DenseMatrix(3, 3);
                        var transMatrixV = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);

                        if (_enableEigenWeights) _mas.sma.computeMatricesProductEigenWeights(PoseID, VertexID, out transMatrixV, out transMatrixN);
                        else
                        {
                            if (_fittingMode == Modes.Fitting.P2P_APP_APP_RPF)
                            {
                                var transMatrixV44     = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                                var transMatrixV34_tmp = new DenseMatrix(3, 4);
                                var transMatrixV44_tmp = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                                for (int ID = 0; ID <= PoseID; ID++)
                                {
                                    _mas.sma.computeMatricesProduct(ID, VertexID, out transMatrixV34_tmp, out transMatrixN);
                                    transMatrixV44_tmp.SetSubMatrix(0, 3, 0, 4, transMatrixV34_tmp);
                                    transMatrixV44 = transMatrixV44_tmp * transMatrixV44;
                                }
                                transMatrixV.SetSubMatrix(0, 3, 0, 4, transMatrixV44);
                            }
                            else
                                _mas.sma.computeMatricesProduct(PoseID, VertexID, out transMatrixV, out transMatrixN);                                               
                        }

                        computeApproxData(PoseID, VertexID, FinalPos, transMatrixV, true);
                    }
#if !CPU_PARALLEL
                    );
#endif
            computeApproxDataError();
        }
        public override double computeEL(int VertexID1, int VertexID2, int PoseID, bool approx)
        {
            Vector3 Dif;
            if (approx)
                Vector3.Subtract(ref _approxDataFinal[PoseID][VertexID1], ref _approxDataFinal[PoseID][VertexID2], out Dif);
            else
                Vector3.Subtract(ref _mas.poses[PoseID].verticesData[VertexID1], ref _mas.poses[PoseID].verticesData[VertexID2], out Dif);
            
            return Dif.Length;
        }
    }

    public class SMA_ErrorDataNormal : SMA_ErrorData
    {
               Modes.NormalApproximation _approximatingMode;
        public Modes.NormalApproximation  approximatingMode
        {
            get { return _approximatingMode; }
            set { _approximatingMode = value; }
        }

        public SMA_ErrorDataNormal(int numV, int numP, int numB, ref Mesh3DAnimationSequence mas, Modes.NormalApproximation _approximatingNormalsMode) : base(numV, numP, numB, ref mas)
        { _approximatingMode = _approximatingNormalsMode; }
        public override string fittingModeText()
        {
            string FittingModeText = "";
            if      (_fittingMode == Modes.Fitting.RP               ) FittingModeText = "RP";
            else if (_fittingMode == Modes.Fitting.MP               ) FittingModeText = "MP";
            else if (_fittingMode == Modes.Fitting.P2P_APP_APP      ) FittingModeText = "P2P APP-APP";
            else if (_fittingMode == Modes.Fitting.P2P_APP_APP_RPF  ) FittingModeText = "P2P APP-APP-RPF";
            else if (_fittingMode == Modes.Fitting.P2P_COR_APP      ) FittingModeText = "P2P COR-APP";
            else if (_fittingMode == Modes.Fitting.P2P_COR_COR      ) FittingModeText = "P2P COR-COR";
            return FittingModeText + " NORMAL ";
        }
        public override void computeApproxData(int PoseID, int VertexID, Vector3 restPose, DenseMatrix transMatrix, bool final)
        {
            // Compute Approximated Vertex Normal
            var newNormal = transMatrix * OpenTK_To_MathNET.Vector3ToVector(restPose);
            newNormal = newNormal / newNormal.Norm(2);

            // Set computed value...
            if (final)
                OpenTK_To_MathNET.ArrayToVector3(newNormal.ToArray(), out _approxDataFinal[PoseID][VertexID]);
            else
                OpenTK_To_MathNET.ArrayToVector3(newNormal.ToArray(), out _approxData[PoseID][VertexID]);
        }
        public override void computeApproxDataError()
        {
            Mesh3D MeanPose = (_mas.addMeanPoseToTree) ? _mas.poses[_mas.poses.Count - 1] : _mas.meanPose;

            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
#if CPU_PARALLEL
                Parallel.For(0, _numVertices, VertexID =>
#else
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                {
                    // Approximation
                    Vector3 ApproxResult;
                    Vector3.Subtract(ref _mas.poses[PoseID].normalsVerticesData[VertexID], ref _approxDataFinal[PoseID][VertexID], out ApproxResult);
                    _errorApproxData[PoseID, VertexID] = ApproxResult.Length;

                    // Mean Pose
                    Vector3 MeanResult;
                    Vector3.Subtract(ref _mas.poses[PoseID].normalsVerticesData[VertexID], ref MeanPose.normalsVerticesData[VertexID], out MeanResult);
                    _errorMeanData[PoseID, VertexID] = MeanResult.Length;
                }
#if CPU_PARALLEL
                );
#endif
            }

            //computeFittingError();
        }
        public override void computeEigenSkin()
        {
            int RestPose = (_fittingMode == Modes.Fitting.RP || _fittingMode == Modes.Fitting.P2P_APP_APP_RPF) ? _mas.selectedRestPose : 0;

            for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
            {
                if (PoseID != RestPose)
                {
#if CPU_PARALLEL
                    Parallel.For(0, _numVertices, VertexID =>
#else
                    for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                    {
                        Vector3 newV;
                        var matrix = new DenseMatrix(3, 3);

                        if (_approximatingMode != Modes.NormalApproximation.RECOMPUTE)
                        {
                            DenseMatrix transMatrixV, transMatrixN;
                            _mas.sma.computeMatricesProduct(PoseID, VertexID, out transMatrixV, out transMatrixN);

                            if (_approximatingMode == Modes.NormalApproximation.WEI_LIN_COM_INV)
                                matrix.SetSubMatrix(0, 3, 0, 3, transMatrixV.SubMatrix(0, 3, 0, 3).Transpose());
                            else if (_approximatingMode == Modes.NormalApproximation.MAT_INV)
                                matrix.SetSubMatrix(0, 3, 0, 3, transMatrixN.Inverse());

                            var vvvv = OpenTK_To_MathNET.Vector3ToVector(new Vector3(_mas.poses[PoseID].normalsVerticesData[VertexID]));
                            var newE = (DenseMatrix)matrix * vvvv;

                            OpenTK_To_MathNET.ArrayToVector3(newE.ToArray(), out newV);
                        }
                        else
                            newV = new Vector3(_mas.sma.errorDataNormal.approxDataFinal[PoseID][VertexID]);

                        Vector3 restPosePos = _mas.sma.getRestPoseNormal(PoseID, VertexID, _mas, true);
                        Vector3.Subtract(ref newV, ref restPosePos, out _eigenSkinCorrections[PoseID][VertexID]);
                    }
#if CPU_PARALLEL
                    );
#endif
                }
                else
#if CPU_PARALLEL
                    Parallel.For(0, _numVertices, VertexID =>
#else
                    for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                    {
                        _eigenSkinCorrections[PoseID][VertexID] = Vector3.Zero;
                    }
#if CPU_PARALLEL
                    );
                
                Parallel.For(0, _numVertices, VertexID =>
#else
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                {
                    _eigenSkinCorrectionsData.At(VertexID * 3, PoseID, _eigenSkinCorrections[PoseID][VertexID].X);
                    _eigenSkinCorrectionsData.At(VertexID * 3 + 1, PoseID, _eigenSkinCorrections[PoseID][VertexID].Y);
                    _eigenSkinCorrectionsData.At(VertexID * 3 + 2, PoseID, _eigenSkinCorrections[PoseID][VertexID].Z);
                }
#if CPU_PARALLEL
                );
#endif
            }
        }
        public override void computeEigenWeights() {;}
        public override void computeFinalPositions()
        {
            if (_approximatingMode == Modes.NormalApproximation.RECOMPUTE)
                recomputeNormalVectors();
            else
            {
                int RestPose = (_fittingMode == Modes.Fitting.RP || _fittingMode == Modes.Fitting.P2P_APP_APP_RPF) ? _mas.selectedRestPose : 0;

                for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
                    if (PoseID != RestPose)
#if CPU_PARALLEL
                        Parallel.For(0, _numVertices, VertexID =>
#else
                        for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                        {
                            // Rest Pose - EigenSkin
                            Vector3 RestPosePos = _mas.sma.getRestPoseNormal(PoseID, VertexID, _mas, false);
                            
                            Vector3 CorrPos = (_enableEigenSkin) ? new Vector3(_eigenSkinCorrectionsApprox[PoseID][VertexID]) : Vector3.Zero;
                            
                            Vector3 FinalPos;
                            Vector3.Add(ref RestPosePos, ref CorrPos, out FinalPos);

                            // Martix Transformation - EigenWeights
                            DenseMatrix transMatrixV, transMatrixN;
                            if (_enableEigenWeights) _mas.sma.computeMatricesProductEigenWeights(PoseID, VertexID, out transMatrixV, out transMatrixN);
                            else                     _mas.sma.computeMatricesProduct(PoseID, VertexID, out transMatrixV, out transMatrixN);

                            computeApproxData(PoseID, VertexID, FinalPos, transMatrixN, true);
                        }
#if CPU_PARALLEL
);
#endif        
            }

            computeApproxDataError();
        }
        public          void recomputeNormalVectors()
        {
                for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
                {
                    Vector3[] normalsFacetsData = new Vector3[_mas.poses[0].facetsCount];
#if CPU_PARALLEL
                    Parallel.For(0, _mas.poses[0].facetsCount, FacetID =>
#else
                    for (int FacetID = 0; FacetID < _mas.poses[0].facetsCount; FacetID++)
#endif
                    {
                        Vector3 v0 = _mas.sma.errorDataVertex.approxDataFinal[PoseID][_mas.poses[0].indicesData[3 * FacetID]];
                        Vector3 v1 = _mas.sma.errorDataVertex.approxDataFinal[PoseID][_mas.poses[0].indicesData[3 * FacetID + 1]];
                        Vector3 v2 = _mas.sma.errorDataVertex.approxDataFinal[PoseID][_mas.poses[0].indicesData[3 * FacetID + 2]];

                        normalsFacetsData[FacetID] = Vector3.Cross(Vector3.Subtract(v1, v0), Vector3.Subtract(v2, v0));
                        normalsFacetsData[FacetID].Normalize();
                    }
#if CPU_PARALLEL
                    );
                    Parallel.For(0, _numVertices, VertexID =>
#else
                    for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
                    {
                        _approxDataFinal[PoseID][VertexID] = Vector3.Zero;

                        Vector3 v0 = _mas.poses[PoseID].verticesData[VertexID];

                        for (int NeighborID = 0; NeighborID < _mas.poses[0].neighborsFacetsData[VertexID].Count; NeighborID++)
                        {
                            int k;
                            int o = 3 * _mas.poses[0].neighborsFacetsData[VertexID][NeighborID];

                            for (k = 0; k < 3; k++)
                                if (VertexID == _mas.poses[0].indicesData[o + k])
                                    break;

                            Vector3 v10     = _mas.sma.errorDataVertex.approxDataFinal[PoseID][_mas.poses[0].indicesData[o + (k + 1) % 3]] - v0;
                            Vector3 v20     = _mas.sma.errorDataVertex.approxDataFinal[PoseID][_mas.poses[0].indicesData[o + (k + 2) % 3]] - v0;
                            float   weight  = (float)Math.Acos(Vector3.Dot(v10, v20) / (v10.Length * v20.Length));
                            _approxDataFinal[PoseID][VertexID] += weight * normalsFacetsData[_mas.poses[0].neighborsFacetsData[VertexID][NeighborID]];
                        }
                        _approxDataFinal[PoseID][VertexID].Normalize();
                    }
#if CPU_PARALLEL
                    );
#endif
                }
        }

        public override double computeEL(int VertexID1, int VertexID2, int PoseID, bool approx)
        {
            if (approx)
                return (_approxDataFinal[PoseID][VertexID1] - _approxDataFinal[PoseID][VertexID2]).Length;
            else
                return (_mas.poses[PoseID].normalsVerticesData[VertexID1] - _mas.poses[PoseID].normalsVerticesData[VertexID2]).Length;
        }
    }
    
    public class SMA
    {
        #region Private Properties
        
        int _selectedBone;
        int _selectedPose;
        int _selectedErrorData;
        int _numBones;
        int _numPoses;
        int _numVertices;

        int _fitWeightsIter;
        int _fitMatricesIter;
        int _fitRestPoseIter;

        bool _initWeights;
        bool _initMatrices;
        bool _initRestPose;

        Color _colorMin;
        Color _colorMax;
        bool  _colorHeatMap;
        bool  _colorTemporalCoherence;
        Vector4[]   _bonesData;
        Vector4[]   _weightsData;
        Vector3[]   _colorsBoneData;
        Vector3[]   _colorsVerticesData;

        Matrix4[][] _matricesBonePoseData;
        Matrix4[][] _matricesPoseBoneData;

        Buffer      _bonesBuffer;
        Buffer      _weightsBuffer;

        // Initial Weighting
        Modes.Weighting             _weightingMode;

        // Fitting
        double                      _fittingThreshold;
        int                         _fittingIterations;
        int                         _fittingLSQRIterations;
        Modes.Fitting               _fittingMode;
        Modes.FittingError          _fittingErrorMode;
        Modes.FittingErrorVector    _fittingErrorVectorMode;
        
        // Normal Computation Mode
        Modes.NormalApproximation   _approximatingNormalsMode;

        // Approximation Seequence
        List<Mesh3D>        _poses  = null;
        List<SMA_ErrorData> _errorDataList = new List<SMA_ErrorData>();

        // Form
        ResultsForm         _resultsForm;

        #endregion

        #region Public Properties
        public Color colorMin
        {
            get { return _colorMin; }
            set { _colorMin = value; }
        }
        public Color colorMax
        {
            get { return _colorMax; }
            set { _colorMax = value; }
        }
        public bool colorHeatMap
        {
            get { return _colorHeatMap; }
            set { _colorHeatMap = value; }
        }

        public bool colorTemporalCoherence
        {
            get { return _colorTemporalCoherence; }
            set { _colorTemporalCoherence = value; }
        }

        public bool initWeights
        {
            get { return _initWeights; }
            set { _initWeights = value; }
        }
        public bool initMatrices
        {
            get { return _initMatrices; }
            set { _initMatrices = value; }
        }
        public bool initRestPose
        {
            get { return _initRestPose; }
            set { _initRestPose = value; }
        }

        public int fitWeightsIter
        {
            get { return _fitWeightsIter; }
            set { _fitWeightsIter = value; }
        }
        public int fitMatricesIter
        {
            get { return _fitMatricesIter; }
            set { _fitMatricesIter = value; }
        }
        public int fitRestPoseIter
        {
            get { return _fitRestPoseIter; }
            set { _fitRestPoseIter = value; }
        }  

        public Mesh3D pose
        {
            get
            {
                return _poses[_selectedPose];
            }
        }
        public List<Mesh3D> poses
        {
            get
            {
                return _poses;
            }
            set
            {
                _poses = value;
            }
        }
        public int selectedPose
        {
            get { return _selectedPose; }
            set { _selectedPose = value; }
        }
        public int selectedBone
        {
            get { return _selectedBone; }
            set { _selectedBone = value; }
        }
        public int selectedErrorData
        {
            get { return _selectedErrorData; }
            set { _selectedErrorData = value; }
        }
        public int numPoses
        {
            get { return _numPoses; }
            set { _numPoses = value; }
        }
        public int numBones
        {
            get { return _numBones; }
        }
        public int numVertices
        {
            get { return _numVertices; }
            set { _numVertices = value; }
        }
        public Matrix4[][] matricesPoseBoneData
        {
            get { return _matricesPoseBoneData; }
        }
        public Vector4[] bonesData
        {
            get { return _bonesData; }
        }
        public Vector4[] weightsData
        {
            get { return _weightsData; }
        }
        public Vector3[] colorsVerticesData
        {
            get { return _colorsVerticesData; }
        }
        public Buffer bonesBuffer
        {
            get { return _bonesBuffer; }
        }
        public Buffer weightsBuffer
        {
            get { return _weightsBuffer; }
        }
        public int fittingIterations
        {
            get { return _fittingIterations;  }
            set { _fittingIterations = value; }
        }
        public double fittingThreshold
        {
            get { return _fittingThreshold; }
            set { _fittingThreshold = value; }
        }
        public int fittingLSQRIterations
        {
            get { return _fittingLSQRIterations; }
            set { _fittingLSQRIterations = value; }
        }

        public Modes.Weighting weightingMode
        {
            get
            {
                return _weightingMode;
            }
            set
            {
                _weightingMode = value;
            }
        }
        public Modes.Fitting fittingMode
        {
            get { return _fittingMode; }
            set { _fittingMode = value; }
        }
        public Modes.FittingError fittingErrorMode
        {
            get { return _fittingErrorMode; }
            set { _fittingErrorMode = value; }
        }
        public Modes.FittingErrorVector fittingErrorVectorMode
        {
            get { return _fittingErrorVectorMode;}
            set { _fittingErrorVectorMode = value; }
        }
        public Modes.NormalApproximation approximatingNormalsMode
        {
            get { return _approximatingNormalsMode; }
            set { _approximatingNormalsMode = value; }
        }
        public ResultsForm resultsForm
        {
            get { return _resultsForm;}
        }
        public SMA_ErrorData errorDataVertex
        {
            get { return _errorDataList[_selectedErrorData*2]; } 
        }
        public SMA_ErrorData errorDataNormal
        {
            get { return _errorDataList[_selectedErrorData*2+1]; }
        }
        #endregion

        #region Constructor
        public SMA(int NumVertices, int NumPoses)
        {
            _numBones       = 0;
            _numPoses       = NumPoses;
            _numVertices    = NumVertices;
            _selectedPose   = -1;
            _selectedBone   = -1;

            _colorMin = Color.Lime;
            _colorMax = Color.Red;
            _colorHeatMap = true;
            _colorTemporalCoherence = true;

            _initWeights    = false;
            _initRestPose   = false;
            _initMatrices   = false;

            _fitWeightsIter     = 0;
            _fitRestPoseIter    = 0;
            _fitMatricesIter    = 0;

            _weightingMode  = Modes.Weighting.LBS;

            _fittingThreshold       = 1.0e-6;
            _fittingIterations      = 1;
            _fittingLSQRIterations  = 15;
            _fittingMode            = Modes.Fitting.RP;
            _fittingErrorMode       = Modes.FittingError.MSE;
            _fittingErrorVectorMode = Modes.FittingErrorVector.VERTEX;
            _approximatingNormalsMode = Modes.NormalApproximation.RECOMPUTE;
        }
        #endregion
             
        #region Draw Function
        public void draw(ref VertexArray vao)
        {
            if (_selectedPose > -1)
            {
                _bonesBuffer.bind();
                vao.enableAttrib(4);
                vao.setAttribPointer<Vector4>(4, VertexAttribPointerType.Float, false, IntPtr.Zero);

                _weightsBuffer.bind();
                vao.enableAttrib(5);
                vao.setAttribPointer<Vector4>(5, VertexAttribPointerType.Float, false, IntPtr.Zero);
                
                // EigenSkin Vertex
                if (errorDataVertex.enableEigenSkin)
                {
                    errorDataVertex.eigenSkinCorrectionsBuffer[_selectedPose].bind();
                    vao.enableAttrib(6);
                    vao.setAttribPointer<Vector3>(6, VertexAttribPointerType.Float, false, IntPtr.Zero);
                }
                // EigenSkin Normal
                if (errorDataNormal.enableEigenSkin)
                {
                    errorDataNormal.eigenSkinCorrectionsBuffer[_selectedPose].bind();
                    vao.enableAttrib(7);
                    vao.setAttribPointer<Vector3>(7, VertexAttribPointerType.Float, false, IntPtr.Zero);
                }
                // EigenWeights Vertex
                /*
                if (errorDataVertex.enableEigenWeights)
                {
                    errorDataVertex.eigenWeightsCorrectionsBuffer[_selectedPose].bind();
                    vao.enableAttrib(8);
                    vao.setAttribPointer<Vector4>(8, VertexAttribPointerType.Float, false, IntPtr.Zero);
                }*/
            }
        }
        #endregion

        #region Load Functions
        public bool loadSMA(string FilePath, ref Mesh3DAnimationSequence mas)
        {
            List<List<Matrix4>> Matrices = new List<List<Matrix4>>();
            List<Vector4> Bones = new List<Vector4>();
            List<Vector4> Weights = new List<Vector4>();
            try
            {
                using (StreamReader sreader = new StreamReader(FilePath, Encoding.Default))
                {
                    while (!sreader.EndOfStream)
                    {
                        string Line = sreader.ReadLine();
                        string[] currentLine = Line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (currentLine.Length == 0) continue;

                        switch (currentLine[0][0])
                        {
                            case '*':
                                currentLine[0] = currentLine[0].Remove(0, 1);
                                currentLine[0] = currentLine[0].Trim(new char[] { ',' });
                                switch (currentLine[0])
                                {
                                    case "BONEANIMATION":

                                        Matrices.Add(new List<Matrix4>());

                                        currentLine[2] = currentLine[2].Split('=')[1];
                                        _numPoses = int.Parse(currentLine[2]);
                                        for (int i = 0; i < _numPoses; i++)
                                        {
                                            string[] tmp = sreader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                            {
                                                Matrix4 mat = new Matrix4();

                                                mat.M11 = float.Parse(tmp[1]);
                                                mat.M12 = float.Parse(tmp[2]);
                                                mat.M13 = float.Parse(tmp[3]);
                                                mat.M14 = float.Parse(tmp[4]);

                                                mat.M21 = float.Parse(tmp[5]);
                                                mat.M22 = float.Parse(tmp[6]);
                                                mat.M23 = float.Parse(tmp[7]);
                                                mat.M24 = float.Parse(tmp[8]);

                                                mat.M31 = float.Parse(tmp[9]);
                                                mat.M32 = float.Parse(tmp[10]);
                                                mat.M33 = float.Parse(tmp[11]);
                                                mat.M34 = float.Parse(tmp[12]);

                                                mat.M41 = float.Parse(tmp[13]);
                                                mat.M42 = float.Parse(tmp[14]);
                                                mat.M43 = float.Parse(tmp[15]);
                                                mat.M44 = float.Parse(tmp[16]);

                                                Matrices[_numBones].Add(mat);
                                            }
                                        }
                                        _numBones++;
                                        break;

                                    case "VERTEXWEIGHTS":
                                        currentLine[1] = currentLine[1].Split('=')[1];
                                        currentLine[1] = currentLine[1].Split('#')[0];
                                        int numVerts = int.Parse(currentLine[1]);

                                        float[] bData = new float[4];
                                        float[] wData = new float[4];
                                        for (int i = 0; i < numVerts; i++)
                                        {
                                            string[] tmp = sreader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                            {
                                                int count = tmp.Length / 2;
                                                for (int j = 0; j < count; j++)
                                                {
                                                    bData[j] = float.Parse(tmp[j * 2 + 1]);
                                                    wData[j] = float.Parse(tmp[j * 2 + 2]);
                                                }
                                                Bones.Add(new Vector4(bData[0], bData[1], bData[2], bData[3]));
                                                Weights.Add(new Vector4(wData[0], wData[1], wData[2], wData[3]));
                                            }
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }

                Mesh3DAnimationSequence Mas = mas;
                _errorDataList.Add(new SMA_ErrorDataVertex(_numVertices, _numPoses, _numBones, ref Mas));
                _errorDataList.Add(new SMA_ErrorDataNormal(_numVertices, _numPoses, _numBones, ref Mas, Modes.NormalApproximation.RECOMPUTE));

                _matricesBonePoseData = new Matrix4[_numBones][];
                for (int BoneID = 0; BoneID < _numBones; BoneID++)
                    _matricesBonePoseData[BoneID] = Matrices[BoneID].ToArray();

                reorderMatrices();
                setMatrices();

                _bonesData = Bones.ToArray();
                setBones();

                _weightsData = Weights.ToArray();
                setWeights();

                setRandomColor();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public bool loadRMA(Mesh3DAnimationSequence mas, ProgressBar pBar, ToolStripStatusLabel tLabel)
        {
            List<List<Mesh3D>> meshes = WaveFront_OBJ_File.OpenOBJFile(pBar, tLabel, false, false);

            if (meshes == null || meshes[0] == null)
                return true;

            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
                meshes[PoseID][0].verticesData.CopyTo(errorDataVertex.approxDataFinal[PoseID], 0);
                meshes[PoseID][0].normalsVerticesData.CopyTo(errorDataNormal.approxDataFinal[PoseID], 0);
            }
            _selectedPose = 0;

            // Init stuff 
            {
                // A. Matrices
                _matricesPoseBoneData = new Matrix4[_numPoses][];
                for (int PoseID = 0; PoseID < _numPoses; PoseID++)
                {
                    _matricesPoseBoneData[PoseID] = new Matrix4[_numBones];
                    for (int BoneID = 0; BoneID < _numBones; BoneID++)
                        _matricesPoseBoneData[PoseID][BoneID] = Matrix4.Identity;
                }
                setMatrices();

                // B. Weights
                _bonesData = new Vector4[_numVertices];
                _weightsData = new Vector4[_numVertices];
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                {
                    int[] bData = new int[4] { 0, 0, 0, 0 };
                    float[] wData = new float[4] { 1, 0, 0, 0 };

                    OpenTK_To_MathNET.ArrayToVector4(bData, out _bonesData[VertexID]);
                    OpenTK_To_MathNET.ArrayToVector4(wData, out _weightsData[VertexID]);
                }
                setBones();
                setWeights();
            }

            tLabel.Text = "RMA loaded!";
            return false;
        }
        #endregion

        #region Compute/Set Color Functions
        public void setFixedColor(List<Cluster> clusters)
        {
            _colorsBoneData = new Vector3[_numBones];
            for (int boneID = 0; boneID < _numBones; boneID++)
            {
                int ClusterID = clusters[boneID].id;

                _colorsBoneData[ClusterID].X = clusters[ClusterID].sphere.color.R / 255.0f;
                _colorsBoneData[ClusterID].Y = clusters[ClusterID].sphere.color.G / 255.0f;
                _colorsBoneData[ClusterID].Z = clusters[ClusterID].sphere.color.B / 255.0f;
            }
        }
        public void setRandomColor()
        {
            Random RandNum = new Random();

            int rgb_bone, rgb_bone1, rgb_bone2;
            int C = 16777215 / _numBones;

            _colorsBoneData = new Vector3[_numBones];
            for (int boneID = 0; boneID < _numBones; boneID++)
            {
                rgb_bone1 = C * boneID;
                rgb_bone2 = C * (boneID + 1);
                rgb_bone = RandNum.Next(rgb_bone1, rgb_bone2);

                _colorsBoneData[boneID].X = (float)((rgb_bone >> 16 ) & 0x0ff) / 255.0f;
                _colorsBoneData[boneID].Y = (float)((rgb_bone >> 8  ) & 0x0ff) / 255.0f;
                _colorsBoneData[boneID].Z = (float)((rgb_bone       ) & 0x0ff) / 255.0f;
            }
        }
        public void setVerticesColor(Mesh3DAnimationSequence mas)
        {
            if (_bonesData == null)
                return;

            if (mas.vColoringMode == Modes.VertexColoring.BONE)
            {
                _colorsVerticesData = new Vector3[_numVertices];
                if (_selectedBone > -1)
                {
#if CPU_PARALLEL
                    Parallel.For(0, _numVertices, VertexID =>
#else
                    for (int VertexID = 0; VertexID < _numVertices; VertexID++)
#endif
                    {
                        int[] bData = null;
                        float[] wData = null;
                        OpenTK_To_MathNET.Vector4ToArray(_bonesData[VertexID], out bData);
                        OpenTK_To_MathNET.Vector4ToArray(_weightsData[VertexID], out wData);

                        Vector3 rgb_comp = new Vector3(0.41f); // Gray Color
                        for (int boneID = 0; boneID < 4; boneID++)
                            if (_selectedBone == bData[boneID] && wData[boneID] > 0.0f)
                            {
                                rgb_comp = _colorsBoneData[_selectedBone];
                                break;
                            }
                        _colorsVerticesData[VertexID] = rgb_comp;
                    }
#if CPU_PARALLEL
);
#endif
                }
                else
                {
#if CPU_PARALLEL
                    Parallel.For(0, _numVertices, VertexID =>
#else
                    for (int VertexID = 0; VertexID < _numVertices; VertexID++)
#endif
                    {
                        int[] bData = null;
                        float[] wData = null;
                        OpenTK_To_MathNET.Vector4ToArray(_bonesData[VertexID], out bData);
                        OpenTK_To_MathNET.Vector4ToArray(_weightsData[VertexID], out wData);

                        _colorsVerticesData[VertexID] = Vector3.Zero;
                        for (int BoneID = 0; BoneID < 4; BoneID++)
                            _colorsVerticesData[VertexID] += _colorsBoneData[bData[BoneID]] * wData[BoneID];
                    }
#if CPU_PARALLEL
);
#endif
                }

                foreach (Mesh3D Pose in  mas.poses)
                    Pose.setVerticesColor(_colorsVerticesData);

                if(mas.sma.poses != null)
                foreach (Mesh3D Pose in mas.sma.poses)
                    Pose.setVerticesColor(_colorsVerticesData);
            }
            else if (mas.vColoringMode == Modes.VertexColoring.CLUSTER)
            {
                _colorsVerticesData = new Vector3[_numVertices];

                List<Cluster> Clusters = mas.clusteringMethod.clusters;
                for (int ClusterID = 0; ClusterID < Clusters.Count; ClusterID++)
                {
                    Color C = Clusters[ClusterID].sphere.color;
#if CPU_PARALLEL
                    Parallel.ForEach(Clusters[ClusterID].elementsV, ElementID =>
#else
                    for (int ElementID = 0; ElementID < Clusters[ClusterID].elementsV.Count; ElementID++)
#endif
                    {
                        _colorsVerticesData[ElementID].X = C.R / 255.0f;
                        _colorsVerticesData[ElementID].Y = C.G / 255.0f;
                        _colorsVerticesData[ElementID].Z = C.B / 255.0f;
                    }
#if CPU_PARALLEL
);
#endif
                }

                if (mas.sma.poses != null)
                    foreach (Mesh3D Pose in mas.sma.poses)
                        Pose.setVerticesColor(_colorsVerticesData);
            }
            else
            {
                if (mas.sma.selectedPose == -1)
                    return;

                if (_fittingErrorVectorMode == Modes.FittingErrorVector.VERTEX)
                    errorDataVertex.setVerticesColor();
                else
                    errorDataNormal.setVerticesColor();
            }
        }
        #endregion

        #region Get Rest Pose Functions
        public Vector3 getRestPoseVertex(int PoseID, int VertexID, Mesh3DAnimationSequence mas, bool fit)
        {
            Vector3 restPose = new Vector3();
            if (mas.editedPose == null)
            {
                if      (errorDataVertex.fittingMode == Modes.Fitting.RP)
                    restPose = 
                        new Vector3(mas.poses[mas.selectedRestPose].verticesData[VertexID] + errorDataVertex.restPoseCorrections[VertexID]);
                else if (errorDataVertex.fittingMode == Modes.Fitting.MP)
                    restPose =
                        new Vector3(mas.meanPose.verticesData[VertexID] + errorDataVertex.restPoseCorrections[VertexID]);
                else if (errorDataVertex.fittingMode == Modes.Fitting.P2P_APP_APP_RPF)
                {
                    restPose = (fit && PoseID > 0) ?
                        new Vector3(errorDataVertex.approxData[Math.Max(PoseID - 1, 0)][VertexID]) :
                        new Vector3(mas.poses[mas.selectedRestPose].verticesData[VertexID] + errorDataVertex.restPoseCorrections[VertexID]);
                }
                else if (errorDataVertex.fittingMode == Modes.Fitting.P2P_APP_APP)
                    restPose =
                        new Vector3(errorDataVertex.approxData[Math.Max(PoseID - 1, 0)][VertexID] + errorDataVertex.restPoseCorrections[VertexID]);
                else if (errorDataVertex.fittingMode == Modes.Fitting.P2P_COR_COR)
                    restPose =
                        new Vector3(mas.poses[Math.Max(PoseID - 1, 0)].verticesData[VertexID] + errorDataVertex.restPoseCorrections[VertexID]);
                else if (errorDataVertex.fittingMode == Modes.Fitting.P2P_COR_APP)
                {
                    restPose = (fit) ? new Vector3(mas.poses[Math.Max(PoseID - 1, 0)].verticesData[VertexID]) :
                                       new Vector3(errorDataVertex.approxData[Math.Max(PoseID - 1, 0)][VertexID]);
                    restPose += errorDataVertex.restPoseCorrections[VertexID];
                }
            }
            else
            {
                if (errorDataVertex.fittingMode == Modes.Fitting.RP) 
                    restPose = new Vector3(mas.editedPose.verticesData[VertexID]);
                else if (errorDataVertex.fittingMode == Modes.Fitting.P2P_APP_APP_RPF)
                    restPose = (fit && PoseID > 0) ?
                        new Vector3(errorDataVertex.approxData[Math.Max(PoseID - 1, 0)][VertexID]) :
                        new Vector3(mas.poses[mas.selectedRestPose].verticesData[VertexID] + errorDataVertex.restPoseCorrections[VertexID]);
            }
                    
            return restPose;
        }
        public Vector3 getRestPoseNormal(int PoseID, int VertexID, Mesh3DAnimationSequence mas, bool fit)
        {
            Vector3 restPose;
            if      (errorDataVertex.fittingMode == Modes.Fitting.RP)               restPose = new Vector3(errorDataNormal.approxData[mas.selectedRestPose][VertexID]);
            else if (errorDataVertex.fittingMode == Modes.Fitting.MP)               restPose = new Vector3(mas.meanPose.normalsVerticesData[VertexID]);
            else if (errorDataVertex.fittingMode == Modes.Fitting.P2P_APP_APP_RPF)
            {
                restPose = (fit) ? new Vector3(errorDataNormal.approxData[Math.Max(PoseID - 1, 0)][VertexID]) : new Vector3(mas.poses[mas.selectedRestPose].normalsVerticesData[VertexID]);
            }
            else if (errorDataVertex.fittingMode == Modes.Fitting.P2P_APP_APP)      restPose = new Vector3(errorDataNormal.approxData[Math.Max(PoseID - 1, 0)][VertexID]);
            else if (errorDataVertex.fittingMode == Modes.Fitting.P2P_COR_COR)      restPose = new Vector3(mas.poses[Math.Max(PoseID - 1, 0)].normalsVerticesData[VertexID]);
            else //if (errorDataVertex.fittingMode  == Modes.Fitting.P2P_COR_APP)
            {
                restPose = (fit) ? new Vector3(mas.poses[Math.Max(PoseID - 1, 0)].normalsVerticesData[VertexID]) : new Vector3(errorDataNormal.approxData[Math.Max(PoseID - 1,0)][VertexID]);
            }

            return restPose;
        }
        #endregion

        #region Set Functions
        private void setBones()
        {
            _bonesBuffer = new Buffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            _bonesBuffer.bind();
            _bonesBuffer.data<Vector4>(ref _bonesData);
        }
        private void setWeights()
        {
            _weightsBuffer = new Buffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            _weightsBuffer.bind();
            _weightsBuffer.data<Vector4>(ref _weightsData);
        }
        private void setMatrices()
        {
            Buffer TBO;
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
                Matrix4[] matricesTranspose = new Matrix4[_numBones];
                for (int BoneID = 0; BoneID < _numBones; BoneID++)
                    Matrix4.Transpose(ref _matricesPoseBoneData[PoseID][BoneID], out matricesTranspose[BoneID]);

            
                TBO = new Buffer(BufferTarget.TextureBuffer, BufferUsageHint.StaticDraw);
                TBO.bind();
                TBO.data<Matrix4>(ref matricesTranspose);

                errorDataVertex.tex_matrices[PoseID] = new Texture(TextureTarget.TextureBuffer);
                errorDataVertex.tex_matrices[PoseID].bind();
                Texture.buffer(SizedInternalFormat.Rgba32f, TBO.index);
                errorDataVertex.tex_matrices[PoseID].unbind();
                TBO.unbind();
            }
        }
        private void reorderMatrices()
        {
            _matricesPoseBoneData = new Matrix4[_numPoses][];
#if CPU_PARALLEL
            Parallel.For(0, _numPoses, PoseID =>
#else
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
#endif
            {
                _matricesPoseBoneData[PoseID] = new Matrix4[_numBones];
                for (int BoneID = 0; BoneID < _numBones; BoneID++)
                    _matricesPoseBoneData[PoseID][BoneID] = _matricesBonePoseData[BoneID][PoseID];
            }
#if CPU_PARALLEL
);
#endif
        }
        #endregion

        #region Compute Functions

        #region Init Functions
        public void computeInitWeigths (Mesh3DAnimationSequence mas)
        {
            Stopwatch Timer = new Stopwatch();
            Timer.Start();
            {
                Clustering ClusteringMethod = mas.clusteringPerPose ? mas.pose.clusteringMethod : mas.clusteringMethod;

                List<Cluster> Clusters = mas.clusteringPerPoseMerging ? ClusteringMethod.clustersMerged : ClusteringMethod.clusters;

                _numBones = Clusters.Count;

                if(_weightingMode == Modes.Weighting.RIGID)
                    ClusteringMethod.computeRigidWeights(mas.pose, mas.clusteringPerPoseMerging, out _bonesData, out _weightsData);
                else
                    ClusteringMethod.computeLinearWeights(mas, mas.pose, mas.clusteringPerPoseMerging, out _bonesData, out _weightsData);

                setFixedColor(Clusters);
                setBones();

                normalizeWeights();
                setWeights();

                _initWeights = true;
            }
            Timer.Stop();
            Console.WriteLine("Init Weights Computations Time elapsed: {0}", Timer.Elapsed);
        }
        public void computeInitMatrices(Mesh3DAnimationSequence mas)
        {
            _matricesPoseBoneData = new Matrix4[_numPoses][];

#if CPU_PARALLEL
            Parallel.For(0, _numPoses, PoseID =>
#else
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
#endif
            {
                _matricesPoseBoneData[PoseID] = new Matrix4[_numBones];
                for (int BoneID = 0; BoneID < _numBones; BoneID++)
                    _matricesPoseBoneData[PoseID][BoneID] = mas.clusteringMethod.clusters[BoneID].transformationMatrices[PoseID];
            }
#if CPU_PARALLEL
);
#endif
            setMatrices();
        }
        #endregion 

        #region Fitting Functions
        public void computeFitting        (Mesh3DAnimationSequence mas, ProgressBar progressBar)
        {
            Stopwatch Timer = new Stopwatch();
            Timer.Start();
            {
                _selectedErrorData = _errorDataList.Count/2;
                _errorDataList.Add(new SMA_ErrorDataVertex(_numVertices, _numPoses, _numBones, ref mas));
                _errorDataList.Add(new SMA_ErrorDataNormal(_numVertices, _numPoses, _numBones, ref mas, _approximatingNormalsMode));

                for (int Iter = 0; Iter < _fittingIterations; Iter++)
                {
                    if (Iter + 1 <= _fitMatricesIter)
                        computeFittingMatrices(mas, Iter, progressBar);
                    if (Iter + 1 <= _fitRestPoseIter)
                        computeFittingRestPose(mas, progressBar);
                    if (Iter + 1 <= _fitWeightsIter)
                        computeFittingWeights(mas, progressBar);
                }
            }
            Timer.Stop();

            errorDataVertex.fittingTime = "Time: " + Timer.Elapsed.ToString() + " Poses(" + _numPoses.ToString() + ") - Bones(" + _numBones.ToString() + ")";
            errorDataNormal.fittingTime = "Time: " + Timer.Elapsed.ToString() + " Poses(" + _numPoses.ToString() + ") - Bones(" + _numBones.ToString() + ")";
            Console.WriteLine("Fitting Computation Time Elapsed: {0} - Poses({1}) - Bones({2})", Timer.Elapsed, _numPoses, _numBones);

            if (_resultsForm == null)
                _resultsForm = new ResultsForm();
        }

        public void func_skinning_2(double[] x, ref double func, object obj)
        {
            func = 0.0;
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
                for (int I = 0; I < 3; I++)
                    func += System.Math.Pow(
                        Optimization_AxB.a[3 * PoseID + I, 0] * x[0] +
                        Optimization_AxB.a[3 * PoseID + I, 1] * x[1] -
                        Optimization_AxB.b[3 * PoseID + I], 2);
        }
        public void func_skinning_3(double[] x, ref double func, object obj)
        {
            func = 0.0;
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
                for (int I = 0; I < 3; I++)
                    func += System.Math.Pow(
                        Optimization_AxB.a[3 * PoseID + I, 0] * x[0] +
                        Optimization_AxB.a[3 * PoseID + I, 1] * x[1] +
                        Optimization_AxB.a[3 * PoseID + I, 2] * x[2] -
                        Optimization_AxB.b[3 * PoseID + I], 2);
        }
        public void func_skinning_4(double[] x, ref double func, object obj)
        {
            func = 0.0;
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
                for (int I = 0; I < 3; I++)
                    func += System.Math.Pow(
                        Optimization_AxB.a[3 * PoseID + I, 0] * x[0] +
                        Optimization_AxB.a[3 * PoseID + I, 1] * x[1] +
                        Optimization_AxB.a[3 * PoseID + I, 2] * x[2] +
                        Optimization_AxB.a[3 * PoseID + I, 3] * x[3] -
                        Optimization_AxB.b[3 * PoseID + I   ], 2);
        }

        public void computeFittingWeights (Mesh3DAnimationSequence mas, ProgressBar progressBar)
        {
            progressBar.Value = 0;
            progressBar.Maximum = _numVertices;
            {
                int[] bData = null;
                float[] wData = null;
                float[] vPoseArray = null;

                //Call the work function in a new thread. For each of the vertices in the chunk
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                {
                    OpenTK_To_MathNET.Vector4ToArray(_bonesData[VertexID], out bData);
                    OpenTK_To_MathNET.Vector4ToArray(_weightsData[VertexID], out wData);

                    int numOfWeights = 0;
                    for (int BoneID = 0; BoneID < 4; BoneID++)
                        if (wData[BoneID] > 0.0f)
                            numOfWeights++;

                    if (numOfWeights == 1)
                        continue;

                    //Initialize the Coefficient matrix of the System
                    int A_rows = 3 * _numPoses;
                    int A_cols = numOfWeights;

                    double[,] AA = new double[A_rows, A_cols];
                    double[] B = new double[A_rows];

                    for (int PoseID = 0; PoseID < _numPoses; PoseID++)
                    {
                        //begin A
                        //var newVertexTT4;
                        Vector3 restPoseVertex = getRestPoseVertex(PoseID, VertexID, mas, true);

                        //Fill the appropriate Matrix elements
                       // if (_fittingMode == Modes.Fitting.P2P_APP_APP_RPF)
                        {
                            /*
                            var transMatrixN = new DenseMatrix(3, 3);
                            var transMatrixV44 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                            var transMatrixV34_tmp = new DenseMatrix(3, 4);
                            var transMatrixV44_tmp = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                            for (int ID = 0; ID <= PoseID; ID++)
                            {
                                computeMatricesProduct(ID, VertexID, out transMatrixV34_tmp, out transMatrixN);
                                transMatrixV44_tmp.SetSubMatrix(0, 3, 0, 4, transMatrixV34_tmp);
                                transMatrixV44 = transMatrixV44_tmp * transMatrixV44;
                            }
                            TransMatrixV.SetSubMatrix(0, 3, 0, 3, transMatrixV44);
                             */
                        }
                      //  else
                       // {
                            //var boneMatrix44 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(_matricesPoseBoneData[PoseID][bData[3]]);
                            //var boneMatrix34 = new DenseMatrix(3, 4);
                            //boneMatrix34.SetSubMatrix(0, 3, 0, 4, boneMatrix44);

                            //var newVertexTT4 = boneMatrix34 * OpenTK_To_MathNET.Vector4ToVector(new Vector4(restPoseVertex, 1.0f));

                            for (int BoneID = 0; BoneID < numOfWeights; BoneID++)
                                //if (wData[BoneID] > 0.0f)
                                {
                                    var boneMatrix44 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(_matricesPoseBoneData[PoseID][bData[BoneID]]);
                                    var boneMatrix34 = new DenseMatrix(3, 4);
                                    boneMatrix34.SetSubMatrix(0, 3, 0, 4, boneMatrix44);
                                    
                                    var newVertexTT = boneMatrix34 * OpenTK_To_MathNET.Vector4ToVector(new Vector4(restPoseVertex, 1.0f));
                                    //newVertexTT.Subtract(newVertexTT4,newVertexTT);

                                    for (int i = 0; i < 3; i++)
                                        AA[3 * PoseID + i, BoneID] = newVertexTT[i];
                                }
                      //  }

                        //end A

                        //begin B
                      //  OpenTK_To_MathNET.Vector3ToArray(mas.poses[PoseID].verticesData[VertexID], out vPoseArray);
                        OpenTK_To_MathNET.Vector3ToArray(mas.poses[PoseID].verticesData[VertexID] - errorDataVertex.approxData[PoseID][VertexID], out vPoseArray);
                        for (int i = 0; i < 3; ++i)
                            //B[3 * PoseID + i] = vPoseArray[i];// - newVertexTT4[i];
                            B[3 * PoseID + i] = vPoseArray[i];
                        //end B
                    }

                    Optimization_AxB.a = AA;
                    Optimization_AxB.b = B;

                    // perform the Non-Negative Least-squares Computation
                    //double[] X = new double[] { _weightsData[VertexID][0], _weightsData[VertexID][1], _weightsData[VertexID][2], _weightsData[VertexID][3] };
                    //double[,] c = new double[,] { { 1, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 1, 0 }, { 1, 1, 1, 1, 1 } };
                    //int[] ct = new int[] { 1, 1, 1, 1, 0 };

                    //double  [ ] X  = new double  [numOfWeights];
                    //double  [,] c  = new double[numOfWeights+1, numOfWeights+1];
                    //int     [ ] ct = new int[numOfWeights + 1];

                    //for (int BoneID = 0; BoneID < numOfWeights; BoneID++)
                    //{
                    //    X[BoneID] = 0;
                    //    ct[BoneID] = 1;
                    //    for (int BoneID_2 = 0; BoneID_2 < numOfWeights; BoneID_2++)
                    //        c[BoneID, BoneID_2] = (BoneID == BoneID_2) ? 1 : 0;
                    //    c[BoneID, numOfWeights] = -_weightsData[VertexID][BoneID];
                    //}
                    //ct[numOfWeights] = 0;

                    //for (int BoneID = 0; BoneID < numOfWeights; BoneID++)
                    //    c[numOfWeights, BoneID] = 1;
                    //c[numOfWeights, numOfWeights] = 0;

                    double[] X = new double[] { 0, 0, 0, 0 };
                    double[,] c = new double[,] {   { 1, 0, 0, 0, -_weightsData[VertexID][0] }, { 0, 1, 0, 0, -_weightsData[VertexID][1] }, 
                                                  { 0, 0, 1, 0, -_weightsData[VertexID][2] }, { 0, 0, 0, 1, -_weightsData[VertexID][3] }, { 1, 1, 1, 1, 0 } };
                    int[] ct = new int[] { 1, 1, 1, 1, 0 };

                    //double[] X = new double[] { _weightsData[VertexID][0], _weightsData[VertexID][1], _weightsData[VertexID][2] };
                    //double[,] c = new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 } };
                    //int[] ct = new int[] { 1, 1, 1 };

                    int     maxits = 0;
                    double  epsf = 0;
                    double  epsx = 0;
                    double  diffirentiation = 1.0e-6;
                    
                    alglib.minbleicstate state;
                    alglib.minbleicreport rep;
                    alglib.minbleiccreatef(X, diffirentiation, out state);
                    alglib.minbleicsetlc(state, c, ct);
                    alglib.minbleicsetcond(state, _fittingThreshold, epsf, epsx, maxits);
                    if(numOfWeights==2)
                        alglib.minbleicoptimize(state, func_skinning_2, null, null);
                    else if (numOfWeights == 3)
                        alglib.minbleicoptimize(state, func_skinning_3, null, null);
                    else// (numOfWeights == 4)
                        alglib.minbleicoptimize(state, func_skinning_4, null, null);
                    alglib.minbleicresults(state, out X, out rep);

                    // extract weights data from X
                    for (int BoneID = 0; BoneID < numOfWeights; ++BoneID)
                        //if (wData[BoneID] > 0.0f)
                        //_weightsData[VertexID][BoneID] = (float)X[BoneID];
                        _weightsData[VertexID][BoneID] += (float)X[BoneID];
                   
                    //_weightsData[VertexID][3] = 1.0f - (_weightsData[VertexID][0] + _weightsData[VertexID][1] + _weightsData[VertexID][2]);

                    progressBar.Increment(1);
                }

                for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
                    for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                    {
                        Vector3 restPoseVertex = getRestPoseVertex(PoseID, VertexID, mas, false);
                        Vector3 restPoseNormal = getRestPoseNormal(PoseID, VertexID, mas, false);

                        var transMatrixN = new DenseMatrix(3, 3);
                        var transMatrixV = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);

                        if (_fittingMode == Modes.Fitting.P2P_APP_APP_RPF)
                        {
                            var transMatrixV44 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                            var transMatrixV34_tmp = new DenseMatrix(3, 4);
                            var transMatrixV44_tmp = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                            for (int ID = 0; ID <= PoseID; ID++)
                            {
                                computeMatricesProduct(ID, VertexID, out transMatrixV34_tmp, out transMatrixN);
                                transMatrixV44_tmp.SetSubMatrix(0, 3, 0, 4, transMatrixV34_tmp);
                                transMatrixV44 = transMatrixV44_tmp * transMatrixV44;
                            }
                            transMatrixV.SetSubMatrix(0, 3, 0, 4, transMatrixV44);
                        }
                        else
                            computeMatricesProduct(PoseID, VertexID, out transMatrixV, out transMatrixN);

                        errorDataVertex.computeApproxData(PoseID, VertexID, restPoseVertex, transMatrixV, false);
                        errorDataNormal.computeApproxData(PoseID, VertexID, restPoseNormal, transMatrixN, false);
                    }
            }
        }

        public void computeFittingRestPose(Mesh3DAnimationSequence mas, ProgressBar progressBar)
        {
            progressBar.Value = 0;
            progressBar.Maximum = _numVertices;
            {
                //Initialize the Coefficient matrix of the System
                int A_rows = 3 * _numPoses;
                int A_cols = 3;

                double[,] AA = new double[A_rows, A_cols];
                double[ ] B  = new double[A_rows];

                int[] bData = null;
                float[] wData = null;
                float[] vPoseArray = null;

                //Call the work function in a new thread. For each of the vertices in the chunk
                for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                {
                    OpenTK_To_MathNET.Vector4ToArray(_bonesData[VertexID], out bData);
                    OpenTK_To_MathNET.Vector4ToArray(_weightsData[VertexID], out wData);

                    for (int PoseID = 0; PoseID < _numPoses; PoseID++)
                    {
                        //begin A

                        //Fill the appropriate Matrix elements
                        var TransMatrixV = new DenseMatrix(3, 3);
                        if (_fittingMode == Modes.Fitting.P2P_APP_APP_RPF)
                        {
                            var transMatrixN        = new DenseMatrix(3, 3);
                            var transMatrixV44      = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                            var transMatrixV34_tmp  = new DenseMatrix(3, 4);
                            var transMatrixV44_tmp  = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                            for (int ID = 0; ID <= PoseID; ID++)
                            {
                                computeMatricesProduct(ID, VertexID, out transMatrixV34_tmp, out transMatrixN);
                                transMatrixV44_tmp.SetSubMatrix(0, 3, 0, 4, transMatrixV34_tmp);
                                transMatrixV44 = transMatrixV44_tmp * transMatrixV44;
                            }
                            TransMatrixV.SetSubMatrix(0, 3, 0, 3, transMatrixV44);
                        }
                        else
                        {
                            for (int BoneID = 0; BoneID < 4; BoneID++)
                                if (wData[BoneID] > 0.0f)
                                {
                                    var boneMatrix44 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(_matricesPoseBoneData[PoseID][bData[BoneID]]);
                                    var boneMatrix33 = new DenseMatrix(3, 3);
                                    boneMatrix33.SetSubMatrix(0, 3, 0, 3, boneMatrix44);
                                    //Multiply the matrix by the infuence weight
                                    boneMatrix33 *= wData[BoneID];
                                    //Add the resulting matrix to the sum
                                    TransMatrixV += boneMatrix33;
                                }
                        }

                        for (int i = 0; i < 3; ++i)
                            for (int j = 0; j < 3; ++j)
                                AA[3 * PoseID + i, j] = TransMatrixV[i,j];
                        //end A

                        //begin B
                        OpenTK_To_MathNET.Vector3ToArray(                          
                            mas.poses                 [PoseID].verticesData[VertexID] - errorDataVertex.approxData[PoseID][VertexID]
                            , out vPoseArray);
                        for (int i = 0; i < 3; ++i)
                            B[3 * PoseID + i] = vPoseArray[i];
                        //end B
                    }

                    Vector3 restPoseVertexCorrections;
                    // perform the Sparse Least-squares Computation

                    int info;
                    double[] X;
                    alglib.densesolverlsreport rep;
                    alglib.rmatrixsolvels(AA, A_rows, A_cols, B, _fittingThreshold, out info, out rep, out X);

                    //LSqrSparseMatrix A = LSqrSparseMatrix.FromDenseMatrix(AA);
                    //LSqrSparseMatrix A_T = LSqrSparseMatrix.TransposeFromDenseMatrix(AA);
                    //double[] X = LSqrDll.DoLSqr(A_cols, A, A_T, B, A_rows + A_cols + _fittingLSQRIterations);

                    //A.Dispose();
                    //A_T.Dispose();

    
                    OpenTK_To_MathNET.ArrayToVector3(X, out restPoseVertexCorrections);
                    errorDataVertex.restPoseCorrections[VertexID] += restPoseVertexCorrections;

                    //Kavan Approximation:
                    //var K_A = DenseMatrix.OfArray(AA);
                    //var K_B = DenseVector.OfEnumerable(B);
                    //var K_X = K_A.Transpose().Multiply(K_A).Inverse().Multiply(K_A.Transpose().Multiply(K_B));

                    //// extract restPose corrections data from X
                    //OpenTK_To_MathNET.ArrayToVector3(K_X.ToArray(), out restPoseVertexCorrections);
                    //errorDataVertex.restPoseCorrections[VertexID] += restPoseVertexCorrections;

                    progressBar.Increment(1);
                }

                for (int PoseID = 0; PoseID < _numPoses; ++PoseID)
                    for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
                    {
                        Vector3 restPoseVertex = getRestPoseVertex(PoseID, VertexID, mas, false);
                        Vector3 restPoseNormal = getRestPoseNormal(PoseID, VertexID, mas, false);

                        var transMatrixN = new DenseMatrix(3, 3);
                        var transMatrixV = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);

                        if (_fittingMode == Modes.Fitting.P2P_APP_APP_RPF)
                        {
                            var transMatrixV44 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                            var transMatrixV34_tmp = new DenseMatrix(3, 4);
                            var transMatrixV44_tmp = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                            for (int ID = 0; ID <= PoseID; ID++)
                            {
                                computeMatricesProduct(ID, VertexID, out transMatrixV34_tmp, out transMatrixN);
                                transMatrixV44_tmp.SetSubMatrix(0, 3, 0, 4, transMatrixV34_tmp);
                                transMatrixV44 = transMatrixV44_tmp * transMatrixV44;
                            }
                            transMatrixV.SetSubMatrix(0, 3, 0, 4, transMatrixV44);
                        }
                        else
                            computeMatricesProduct(PoseID, VertexID, out transMatrixV, out transMatrixN);

                        errorDataVertex.computeApproxData(PoseID, VertexID, restPoseVertex, transMatrixV, false);
                        errorDataNormal.computeApproxData(PoseID, VertexID, restPoseNormal, transMatrixN, false);
                    }
            }
        }

        public void computeFittingMatrices(Mesh3DAnimationSequence mas, int Iter, ProgressBar progressBar)
        {
            progressBar.Value = 0;
            progressBar.Maximum = _numPoses;

            int RestPose = (errorDataVertex.fittingMode == Modes.Fitting.RP) ? mas.selectedRestPose : 0;

            _matricesPoseBoneData = new Matrix4[_numPoses][];
            
#if CPU_PARALLEL
            Parallel.For(0, _numPoses, PoseID =>
#else
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
#endif
            {
                _matricesPoseBoneData[PoseID] = new Matrix4[_numBones];
            }
#if CPU_PARALLEL
            );
            Parallel.For(0, _numBones, BoneID =>
#else
            for (int BoneID = 0; BoneID < _numBones; BoneID++)
#endif
            {            
                _matricesPoseBoneData[RestPose][BoneID] = Matrix4.Identity;
            }
#if CPU_PARALLEL
            );
#endif

            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            {
                if (Iter == 0 && PoseID == RestPose && errorDataVertex.fittingMode != Modes.Fitting.MP)
                {
                    mas.poses[RestPose].verticesData.CopyTo(errorDataVertex.approxData[RestPose], 0);
                    mas.poses[RestPose].normalsVerticesData.CopyTo(errorDataNormal.approxData[RestPose], 0);
                }
                else
                    computeFittingMatrices(mas, PoseID);
                progressBar.Increment(1);
            }
           
            setMatrices();
        }
        public void computeFittingMatrices(Mesh3DAnimationSequence mas, int PoseID)
        {
            //Initialize the Coefficient matrix of the System
            int A_rows = 3 * _numVertices;
            int A_cols = 12 * _numBones;

            double[,] AA = new double[A_rows, A_cols];
            double[] B = new double[A_rows];

            int bOffset;
            int[] bData = null;
            float[] wData = null;
            float[] vPoseArray = null;
            float[] vRestPoseArray = null;

            //Call the work function in a new thread. For each of the vertices in the chunk
            for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
            {
                //begin A

                //Store the Rest Pose vertex coordinates
                Vector3 restPoseVertex = getRestPoseVertex(PoseID, VertexID, mas, true);

                OpenTK_To_MathNET.Vector4ToArray(new Vector4(restPoseVertex, 1.0f), out vRestPoseArray);
                OpenTK_To_MathNET.Vector4ToArray(_bonesData[VertexID], out bData);
                OpenTK_To_MathNET.Vector4ToArray(_weightsData[VertexID], out wData);

                //Fill the appropriate Matrix elements
                for (int BoneID = 0; BoneID < 4; BoneID++)
                {
                    bOffset = 12 * bData[BoneID];
                    if (wData[BoneID] > 0.0f)
                        for (int i = 0; i < 4; ++i)
                        {
                            AA[3 * VertexID, bOffset + i] =
                            AA[3 * VertexID + 1, bOffset + 4 + i] =
                            AA[3 * VertexID + 2, bOffset + 8 + i] = wData[BoneID] * vRestPoseArray[i];
                        }
                }
                //end A

                //begin B
                if (mas.editedPose == null)
                    OpenTK_To_MathNET.Vector3ToArray(mas.poses[PoseID].verticesData[VertexID], out vPoseArray);
                else
                    OpenTK_To_MathNET.Vector3ToArray(mas.editedPose.verticesData[VertexID], out vPoseArray);
                for (int i = 0; i < 3; ++i)
                    B[3 * VertexID + i] = vPoseArray[i];
                //end B               
            }

            int info;
            double[] X;
            alglib.densesolverlsreport rep;
            alglib.rmatrixsolvels(AA, A_rows, A_cols, B, _fittingThreshold, out info, out rep, out X);

            // perform the Sparse Least-squares Computation
            //LSqrSparseMatrix A = LSqrSparseMatrix.FromDenseMatrix(AA);
            //LSqrSparseMatrix A_T = LSqrSparseMatrix.TransposeFromDenseMatrix(AA);
            // X = LSqrDll.DoLSqr(A_cols, A, A_T, B, A_rows + A_cols + _fittingLSQRIterations);

            // Kavan's Approximation
            //var K_A = DenseMatrix.OfArray(AA);
            //var K_B = DenseVector.OfEnumerable(B);
            //var K_X = K_A.Transpose().Multiply(K_A).Inverse().Multiply(K_A.Transpose().Multiply(K_B));

            // extract matrices data from X
            for (int BoneID = 0; BoneID < _numBones; BoneID++)
            {
                bOffset = 12 * BoneID;
              //  OpenTK_To_MathNET.ArrayToMatrix4(K_X.ToArray(), bOffset, out _matricesPoseBoneData[PoseID][BoneID]);
                OpenTK_To_MathNET.ArrayToMatrix4(X, bOffset, out _matricesPoseBoneData[PoseID][BoneID]);
            }
            
            // compute Approximation Data [V,N]
            for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
            {
                Vector3 restPoseVertex = getRestPoseVertex(PoseID, VertexID, mas, false);
                Vector3 restPoseNormal = getRestPoseNormal(PoseID, VertexID, mas, false);

                var transMatrixN = new DenseMatrix(3, 3);
                var transMatrixV = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);

                if (_fittingMode == Modes.Fitting.P2P_APP_APP_RPF)
                {
                    var transMatrixV44     = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                    var transMatrixV34_tmp = new DenseMatrix(3, 4);
                    var transMatrixV44_tmp = OpenTK_To_MathNET.Matrix4ToDenseMatrix(Matrix4.Identity);
                    for (int ID = 0; ID <= PoseID; ID++)
                    {
                        computeMatricesProduct(ID, VertexID, out transMatrixV34_tmp, out transMatrixN);
                        transMatrixV44_tmp.SetSubMatrix(0, 3, 0, 4, transMatrixV34_tmp);
                        transMatrixV44 = transMatrixV44_tmp * transMatrixV44;
                    }
                    transMatrixV.SetSubMatrix(0, 3, 0, 4, transMatrixV44);
                }
                else
                    computeMatricesProduct(PoseID, VertexID, out transMatrixV, out transMatrixN);

                errorDataVertex.computeApproxData(PoseID, VertexID, restPoseVertex, transMatrixV, false);
                errorDataNormal.computeApproxData(PoseID, VertexID, restPoseNormal, transMatrixN, false);
            }
            //A.Dispose();
            //A_T.Dispose();
        }

        public void computeMatricesProduct            (int PoseID, int VertexID, out DenseMatrix TransMatrixV, out DenseMatrix TransMatrixN)
        {
            int[] bData = null;
            float[] wData = null;
            OpenTK_To_MathNET.Vector4ToArray(_bonesData[VertexID], out bData);
            OpenTK_To_MathNET.Vector4ToArray(_weightsData[VertexID], out wData);

            TransMatrixV = new DenseMatrix(3, 4);
            TransMatrixN = new DenseMatrix(3, 3);

            for (int BoneID = 0; BoneID < 4; BoneID++)
                if (wData[BoneID] > 0.0f)
                {
                    var boneMatrix44 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(_matricesPoseBoneData[PoseID][bData[BoneID]]);
                    var boneMatrix34 = new DenseMatrix(3, 4);
                    boneMatrix34.SetSubMatrix(0, 3, 0, 4, boneMatrix44);
                    //Multiply the matrix by the infuence weight
                    boneMatrix34 *= wData[BoneID];
                    //Add the resulting matrix to the sum
                    TransMatrixV += boneMatrix34;
/*
                    if (((SMA_ErrorDataNormal)errorDataNormal).approximatingMode == Modes.NormalApproximation.MAT_INV)
                    {
                        var boneMatrix33 = new DenseMatrix(3, 3);
                        boneMatrix34.SubMatrix(0, 3, 0, 3).Inverse().Transpose().CopyTo(boneMatrix33);
                        boneMatrix33 *= wData[BoneID];
                        TransMatrixN += boneMatrix33;
                    }
 */
                }

        //    if (((SMA_ErrorDataNormal)errorDataNormal).approximatingMode == Modes.NormalApproximation.WEI_LIN_COM_INV)
             //   TransMatrixV.SubMatrix(0, 3, 0, 3).Inverse().Transpose().CopyTo(TransMatrixN);
        }
        public void computeMatricesProductEigenWeights(int PoseID, int VertexID, out DenseMatrix TransMatrixV, out DenseMatrix TransMatrixN)
        {
            int  [] bData = null;
            float[] wData = null;
            OpenTK_To_MathNET.Vector4ToArray(_bonesData[VertexID], out bData);
            OpenTK_To_MathNET.Vector4ToArray(_weightsData[VertexID] + ((SMA_ErrorDataVertex)errorDataVertex).eigenWeightsCorrections[PoseID][VertexID], out wData);

            TransMatrixV = new DenseMatrix(3, 4);
            TransMatrixN = new DenseMatrix(3, 3);

            for (int BoneID = 0; BoneID < 4; BoneID++)
                if (wData[BoneID] > 0.0f)
                {
                    var boneMatrix44 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(_matricesPoseBoneData[PoseID][bData[BoneID]]);
                    var boneMatrix34 = new DenseMatrix(3, 4);
                    boneMatrix34.SetSubMatrix(0, 3, 0, 4, boneMatrix44);
                    //Multiply the matrix by the infuence weight
                    boneMatrix34 *= wData[BoneID];
                    //Add the resulting matrix to the sum
                    TransMatrixV += boneMatrix34;

                    if (((SMA_ErrorDataNormal)errorDataNormal).approximatingMode == Modes.NormalApproximation.MAT_INV)
                    {
                        var boneMatrix33 = new DenseMatrix(3, 3);
                        boneMatrix34.SubMatrix(0, 3, 0, 3).Inverse().Transpose().CopyTo(boneMatrix33);
                        boneMatrix33 *= wData[BoneID];
                        TransMatrixN += boneMatrix33;
                    }
                }      
        }

        public void computeMatricesEditing(Mesh3DAnimationSequence mas)
        {
           // for (int PoseID = 0; PoseID < _numPoses; PoseID++)
            //    for (int BoneID = 0; BoneID < _numBones; BoneID++)
              //  {
                  //  Matrix4 matrixRPtoEP = mas.clusteringMethod.clusters[BoneID].computeTransformationMatrix(mas.restPose, mas.editedPose, mas.clusteringMethod.scalingFactor);
                  //  Matrix4 matrixEPtoRP = mas.clusteringMethod.clusters[BoneID].computeTransformationMatrix(mas.editedPose, mas.restPose, mas.clusteringMethod.scalingFactor);

                  //  _matricesPoseBoneData[PoseID][BoneID] =
                        //matrixRPtoEP *
                    //    _matricesPoseBoneData[PoseID][BoneID] * matrixRPtoEP;
              //  }
        }

        #region Normalize Weights Functions
        private void normalizeWeights()
        {
#if CPU_PARALLEL
            Parallel.For(0, _numVertices, VertexID =>
#else
            for (int VertexID = 0; VertexID < _numVertices; ++VertexID)
#endif
            {
                normalizeWeights(VertexID);
            }
#if CPU_PARALLEL
);
#endif
        }
        private void normalizeWeights(int VertexID)
        {
            float[] wData = null;
            OpenTK_To_MathNET.Vector4ToArray(_weightsData[VertexID], out wData);

            float sumW = 0.0f;
            for (int j = 0; j < 4; ++j) sumW += wData[j];
            for (int j = 0; j < 4; ++j) wData[j] /= sumW;

            OpenTK_To_MathNET.ArrayToVector4(wData, out _weightsData[VertexID]);
        }
        #endregion

        #endregion

        #region EigenSkin Functions
        public void computeEigenSkin()
        {
            errorDataVertex.computeEigenSkin();
            errorDataNormal.computeEigenSkin();
        }
        #endregion

        #region EigenWeights Functions
        public void computeEigenWeights()
        {
            errorDataVertex.computeEigenWeights();
            errorDataNormal.computeEigenWeights();
        }
        #endregion

        #endregion

        #region Compute Approximation Models
        public void computeFinalPositions()
        {
            errorDataVertex.computeFinalPositions();
            errorDataNormal.computeFinalPositions();
        }
        public void computeApproxModels(Mesh3DAnimationSequence mas)
        {
            _poses = new List<Mesh3D>();
            for (int PoseID = 0; PoseID < _numPoses; PoseID++)
                _poses.Add(new Mesh3D(errorDataVertex.approxDataFinal[PoseID], errorDataNormal.approxDataFinal[PoseID], null, mas.poses[0].indicesData));
            //{
                //if (mas.editedPose != null && PoseID == mas.editedPose.poseID)
                  //  _poses.Add(new Mesh3D(mas.editedPose.verticesData, mas.editedPose.normalsVerticesData, null, mas.editedPose.indicesData));
                //else
                //if(((SMA_ErrorDataNormal)errorDataNormal).approximatingMode == Modes.NormalApproximation.RECOMPUTE) // [for GPU-Implementation]
                //  _poses.Add(new Mesh3D(errorDataVertex.approxDataFinal[PoseID], errorDataNormal.approxDataFinal[PoseID], null, mas.poses[0].indicesData));
                //else
                //  _poses.Add(new Mesh3D(errorDataVertex.approxData[PoseID], errorDataNormal.approxData[PoseID], null, mas.poses[0].indicesData));
            //}
        }
        #endregion

        #region Delete Function
        public void delete()
        {
            if (_bonesData != null) _bonesBuffer.delete();
            if (_weightsData != null) _weightsBuffer.delete();
        }
        #endregion

        #region Results Form Function
        public void createForm()
        {
            _resultsForm.Show();
        }
        public void createChartError()
        {
            _resultsForm.createChartError();
        }
        #endregion
    }
}