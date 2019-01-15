using System.Collections.Generic;
using OpenTK;
using System;
using System.Collections;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace abasilak
{
    public abstract class Cluster
    {
        /// <summary>
        /// Function defined in min_cut.dll to perform maximum flow/minimum cut on a weighted graph
        /// </summary>
        [DllImport("min_cut", CallingConvention = CallingConvention.StdCall)]
        extern unsafe static bool* minimumCut(double** capacities, int nodesTotalCount, int edgesCount);

        #region Private Properties
        Sphere  _sphere;

        int     _id;
        int     _poseID;
        int     _posesCount;

        int     [] _CAV;

        double  _area;
        SortedDictionary<double, List<int>> _areas;

        bool _toBeCleaned;
        bool _toBeChecked;

        Cluster     _clusterThis;
        Cluster     _clusterPrev;
        List<int>   _clusterPrevIDs;

        List<int>       _elementsF;
        List<Cluster>   _neighbors;
        List<List<int>> _bordersV;

        DeformationGradient _DG;

        double          _maxDistance;

        double []       _dg;
        Vector3[]       _center;
        Vector3[]       _normal;
        Vector<double>  _centroid;
        Dictionary<int, double> _centroidDict;
        
        Matrix4[]       _transformationMatrices;

        #endregion 

        #region Protected Properties
        protected int           _head;
        protected double        _error;
        protected List<int>     _elementsV;
        #endregion

        #region Public Properties
        public SortedDictionary<double, List<int>> areas
        {
            get { return _areas; }
            set { _areas = value; }        
        }
        public Sphere sphere
        {
            get { return _sphere; }
        }
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        public int head
        {
            get { return _head; }
            set { _head = value; }
        }
        public double error
        {
            get { return _error; }
            set { _error = value; }
        }
        public double maxDistance
        {
            get
            {
                return _maxDistance;
            }
            set
            {
                _maxDistance = value;
            }
        }
        public Cluster clusterThis
        {
            get
            {
                return _clusterThis;
            }
            set
            {
                _clusterThis = value;
            }
        }
        public Cluster clusterPrev
        {
            get
            {
                return _clusterPrev;
            }
            set
            {
                _clusterPrev = value;
            }
        }
        public List<int> clusterPrevIDs
        {
            get
            {
                return _clusterPrevIDs;
            }
            set
            {
                _clusterPrevIDs = value;
            }
        }
        public double area
        {
            get { return _area; }
            set { _area = value; }
        }
        public bool toBeCleaned
        {
            get { return _toBeCleaned; }
            set { _toBeCleaned = value; }
        }
        public bool toBeChecked
        {
            get { return _toBeChecked; }
            set { _toBeChecked = value; }
        }
        public Vector<double> centroid
        {
            get { return _centroid; }
            set { _centroid = value;}
        }

        public int [] CAV
        {
            get
            {
                return _CAV;
            }
            set
            {
                _CAV = value;
            }
        }
        public List<int> elementsV
        {
            get { return _elementsV; }
        }
        public List<List<int>> bordersV
        {
            get { return _bordersV; }
            set { _bordersV = value; }
        }
        public List<int> elementsF
        {
            get { return _elementsF; }
        }
        public List<Cluster> neighbors
        {
            get { return _neighbors; }
            set { _neighbors = value; }
        }
        public Matrix4 [] transformationMatrices
        {
            get { return _transformationMatrices; }
            set { _transformationMatrices = value; }
        }
        #endregion

        #region Constructor
        public Cluster(Mesh3D pose, int headIndex, int idIndex, int numOfPoses, Modes.DeformationGradient dgMode)
        {
            Vector3 Center = pose.verticesData[headIndex];
            float   Radius = pose.radius / 50.0f;

            _id         = idIndex;
            _head       = headIndex;
            _posesCount = numOfPoses;
            _poseID     = pose.poseID;
            _neighbors  = new List<Cluster>();
            _error      = 0.0f;

            _areas       = new SortedDictionary<double, List<int>>(new Comparers.DoubleKeyComparerDesc());
            _area        = 0.0;
            _toBeCleaned = false;
            _toBeChecked = false;

            _clusterPrevIDs = new List<int>();

            if      (dgMode == Modes.DeformationGradient.REST_POSE) _DG = pose.dgRP;
            else if (dgMode == Modes.DeformationGradient.MEAN_POSE) _DG = pose.dgMP;
            else                                                    _DG = pose.dgP2P;

            _dg         = new double[_posesCount];
            _normal     = new Vector3[_posesCount];
            _center     = new Vector3[_posesCount];
            _sphere     = new Sphere(Center, Radius, idIndex);
            _elementsV  = new List<int>();
            _elementsF  = new List<int>();
            _bordersV   = new List<List<int>>();
            _transformationMatrices = new Matrix4[_posesCount];
        }
        #endregion

        #region Get Area Functions
        public List<int> getAreaList(double area)
        {
            List<int> result;
            _areas.TryGetValue(area, out result);
            return result;
        }
        public int       getMaxClusterAreaID()
        {
            foreach (KeyValuePair<double, List<int>> pivot in _areas)
                return pivot.Value[0];
            return -1;
        }
        public int       getMaxClusterAreaNextID()
        {
            int Count = 0;
            foreach (KeyValuePair<double, List<int>> Pivot in _areas)
            {
                if (Count == 0 && Pivot.Value.Count > 1)
                    return Pivot.Value[1];
                else if (Count == 1)
                    return Pivot.Value[0];
                else
                    Count++;
            }
            return -1;
        }
        public double    getMaxClusterArea()
        {
            foreach (KeyValuePair<double, List<int>> pivot in _areas)
                return pivot.Key;
            return 0.0;
        }
        public double    getMaxClusterAreaNext()
        {
            int Count = 0;
            foreach (KeyValuePair<double, List<int>> Pivot in _areas)
            {
                if (Count == 0 && Pivot.Value.Count > 1 || Count == 1)
                    return Pivot.Key;
                else
                    Count++;
            }
            return 0.0;
        }
        #endregion

        #region Add Elements Functions
        public void addElementArea(int clusterID, double area)
        {
            //Check if a node with the same distance already exists
            List<int> tList = getAreaList(area);

            if (tList == null)//If not create the list and add to the new node
            {
                tList = new List<int>();
                tList.Add(clusterID);
                _areas.Add(area, tList);
            }
            else//If yes add the node to the list of the existing
            {
                //But first check to see if we are attempting to insert the same element
                if (!tList.Contains(clusterID))
                    tList.Add(clusterID);
            }
        }

        public void addElementV(int vertexID)
        {
            _elementsV.Add(vertexID);
        }
        public void addElementF(int facetID)
        {
            _elementsF.Add(facetID);
        }
        public void addRangeElementV(List<int> verticesID)
        {
            _elementsV.AddRange(verticesID);
        }
        public void addRangeElementF(List<int> facetsID)
        {
            _elementsF.AddRange(facetsID);
        }
        #endregion

        #region Remove Elements Functions
        public void removeElementV(int vertexID)
        {
            _elementsV.Remove(vertexID);
        }
        public void removeElementF(int facetID)
        {
            _elementsF.Remove(facetID);
        }
        #endregion

        #region IsEmpty Function
        public bool isEmpty()
        {
            return (_elementsV.Count == 0) ? true : false;
        }
        #endregion

        #region Compute Head
        public void computeHead(Mesh3DAnimationSequence mas)
        {
            double[] Errors = new double[_elementsV.Count];
#if CPU_PARALLEL
            Parallel.For(0, _elementsV.Count, ElementID =>
#else
            for (int ElementID=0; ElementID < _elementsV.Count; ElementID++)
#endif
            {
                Errors[ElementID] = mas.clusteringPerPose ? computeErrorFromVertexP2P(mas, mas.poses[_poseID], _elementsV[ElementID]) : computeErrorFromVertices(mas, _elementsV[ElementID]);
            }
#if CPU_PARALLEL
);
#endif
            double ErrorMin = double.MaxValue;
            for (int ElementID = 0; ElementID < _elementsV.Count; ElementID++)
            {
                if (Errors[ElementID] < ErrorMin)
                {
                    ErrorMin = Errors[ElementID];
                    _head = _elementsV[ElementID];
                }
            }
        }
        #endregion

        #region Compute Centers

        public void     computeCenter(Mesh3DAnimationSequence mas, bool scaling, Dictionary<int, double> [] matrix)
        {
            if      (mas.clusteringDistanceMode == Modes.ClusteringDistance.NORMAL)                computeNormals(mas);
            else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.DEFORMATION_GRADIENT)  computeDGs(mas);
            else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.SKINNING)              computeTransformationMatrices(mas, scaling);
            else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING)            computeMatrix(matrix);

            computeCentersPos(mas);
            if (mas.clusteringVertexDistanceMode == Modes.ClusteringVertexDistance.GEODESIC_ANGLE)
                computeHead(mas);
        }

        public Vector3  computeCenterPos (Mesh3D pose)
        {
            Vector3 Center = new Vector3();
            foreach (int ElementID in _elementsV)
                Center += pose.verticesData[ElementID];
            Center /= _elementsV.Count;
            return Center;
        }
        public void     computeCentersPos(Mesh3DAnimationSequence mas)
        {
            if (mas.clusteringPerPose)
            {
                _center[0] = computeCenterPos(mas.poses[_poseID]);
                _sphere.setCenter(_center[0], mas.restPose.radius / 50.0f);
            }
            else
            {
#if CPU_PARALLEL
                Parallel.For(0, _posesCount, PoseID =>
#else
                for (int PoseID = 0; PoseID < _posesCount; PoseID++)
#endif
                {
                    _center[PoseID] = computeCenterPos(mas.poses[PoseID]);
                }
#if CPU_PARALLEL
);
#endif
                _sphere.setCenter(_center[mas.restPose.poseID], mas.restPose.radius / 50.0f);
            }
        }

        public Vector3  computeNormal (Mesh3D pose)
        {
            Vector3 Normal = Vector3.Zero;
            foreach (int ElementID in _elementsV)
                Normal += pose.normalsVerticesData[ElementID];
            Normal.Normalize();

            return Normal;
        }
        public void     computeNormals(Mesh3DAnimationSequence mas)
        {
            if (mas.clusteringPerPose)
                _normal[0] = computeNormal(mas.poses[_poseID]);
            else
            {
#if CPU_PARALLEL
                Parallel.For(0, _posesCount, PoseID =>
#else
                for (int PoseID = 0; PoseID < _posesCount; PoseID++)
#endif
                {
                    _normal[PoseID] = computeNormal(mas.poses[PoseID]);
                }
#if CPU_PARALLEL
);
#endif
            }
        }

        public double   computeDG  (Mesh3D pose)
        {
            double dg = 0.0;
            foreach (int ElementID in _elementsV)
                dg += (double)_DG.VerticesData[ElementID];
            dg /= (double)_elementsV.Count;

            return dg;
        }
        public void     computeDGs (Mesh3DAnimationSequence mas)
        {
            if (mas.clusteringPerPose)
                _dg[0] = computeDG(mas.poses[_poseID]);
            else
            {
                _dg[mas.selectedRestPose] = 0.0f;
#if CPU_PARALLEL
                Parallel.For(0, _posesCount, PoseID =>
#else
                for (int PoseID = 0; PoseID < _posesCount; PoseID++)
#endif
                {
                    if (PoseID != mas.selectedRestPose)
                        _dg[PoseID] = computeDG(mas.poses[PoseID]);
                }
#if CPU_PARALLEL
);
#endif
            }
        }

        public Matrix4  computeTransformationMatrix(Mesh3D restPose, Mesh3D thisPose, bool scaling)
        {
            // If Cluster Contains only One (1) Vertex
            if (_elementsV.Count == 1)
                return OpenTK_To_MathNET.DenseMatrixtoMatrix4(
                        DenseMatrix.CreateIdentity(3),
                        OpenTK_To_MathNET.Vector3ToVector(thisPose.verticesData[_elementsV[0]]) -
                        OpenTK_To_MathNET.Vector3ToVector(restPose.verticesData[_elementsV[0]]));
            // Otherwise...

            DenseVector Pcenter = new DenseVector(3);                 
            DenseVector Qcenter = new DenseVector(3);                   
            DenseMatrix Pmatrix = new DenseMatrix(3, _elementsV.Count);
            DenseMatrix Qmatrix = new DenseMatrix(3, _elementsV.Count);

            for (int ElementID = 0; ElementID < _elementsV.Count; ElementID++)
            {
                // Init Arrays
                var Pvertex = OpenTK_To_MathNET.Vector3ToVector(restPose.verticesData[_elementsV[ElementID]]);
                var Qvertex = OpenTK_To_MathNET.Vector3ToVector(thisPose.verticesData[_elementsV[ElementID]]);
                Pmatrix.SetColumn(ElementID, (Vector)Pvertex);
                Qmatrix.SetColumn(ElementID, (Vector)Qvertex);

                // Compute Center
                Pcenter = Pcenter + Pvertex;
                Qcenter = Qcenter + Qvertex;
            }
            Pcenter /= _elementsV.Count;
            Qcenter /= _elementsV.Count;

            // Translation
            for (int ElementID = 0; ElementID < _elementsV.Count; ElementID++)
            {
                Pmatrix.SetColumn(ElementID, Pmatrix.Column(ElementID) - Pcenter);
                Qmatrix.SetColumn(ElementID, Qmatrix.Column(ElementID) - Qcenter);
            }
            // Covarianve 'A' Matrix
            DenseMatrix Amatrix = (DenseMatrix)Pmatrix.TransposeAndMultiply(Qmatrix);
            var AmatrixSVD = Amatrix.Svd(true);

            // 'd' sign
            DenseMatrix Rmatrix = (DenseMatrix)((DenseMatrix)AmatrixSVD.U * (DenseMatrix)AmatrixSVD.VT).Transpose();
            int Rsign = Math.Sign(Rmatrix.Determinant());
            DenseMatrix RmatrixSigned = DenseMatrix.CreateIdentity(3); RmatrixSigned.At(2, 2, Rsign);
            // Optimal Rotational Matrix
            DenseMatrix RmatrixOptimal = (DenseMatrix)AmatrixSVD.VT.Transpose() * RmatrixSigned * (DenseMatrix)AmatrixSVD.U;

            // Optimal Uniform Scale Matrix
            DenseMatrix SmatrixOptimal = DenseMatrix.CreateIdentity(3);

            if (scaling)
            {
                double Plengths = 0.0, Qlenghts = 0.0;
                for (int ElementID = 0; ElementID < _elementsV.Count; ElementID++)
                {
                    Plengths += Math.Pow(Pmatrix.Column(ElementID).Norm(2), 2.0);
                    Qlenghts += Math.Pow(Qmatrix.Column(ElementID).Norm(2), 2.0);
                }
                double s = Math.Sqrt(Qlenghts / Plengths);
                SmatrixOptimal.At(0, 0, s);
                SmatrixOptimal.At(1, 1, s);
                SmatrixOptimal.At(2, 2, s);
            }
            // Optimal Affine Matrix 
            DenseMatrix AmatrixOptimal = SmatrixOptimal * RmatrixOptimal;
            // Optimal Translation Matrix (= cy - s * U * cx)
            DenseVector TmatrixOptimal = Qcenter - AmatrixOptimal * Pcenter;

            return OpenTK_To_MathNET.DenseMatrixtoMatrix4(AmatrixOptimal, TmatrixOptimal);
        }
        public void     computeTransformationMatricesEditing(Mesh3DAnimationSequence mas, bool scaling)
        {
            Matrix4 matrixRPtoEP = computeTransformationMatrix(mas.restPose, mas.editedPose, scaling);
#if CPU_PARALLEL
            Parallel.For(0, _posesCount, PoseID =>
#else
            for (int PoseID = 0; PoseID < _posesCount; PoseID++)
#endif
            {
                _transformationMatrices[PoseID] = Matrix4.Mult(_transformationMatrices[PoseID], matrixRPtoEP);
            }
#if CPU_PARALLEL
);
#endif
        }
        public void     computeTransformationMatrices(Mesh3DAnimationSequence mas, bool scaling)
        {
            if (mas.clusteringPerPose)
                _transformationMatrices[0] = (_poseID == mas.selectedRestPose) ? Matrix4.Identity : computeTransformationMatrix(mas.dgMode == Modes.DeformationGradient.POSE_TO_POSE ? mas.poses[_poseID - 1] : mas.restPose, mas.poses[_poseID], scaling);
            else
            {
                _transformationMatrices[mas.selectedRestPose] = Matrix4.Identity;
#if CPU_PARALLEL
                Parallel.For(0, _posesCount, PoseID =>
#else
                for (int PoseID = 0; PoseID < _posesCount; PoseID++)
#endif
                {
                    if (PoseID != mas.selectedRestPose)
                        _transformationMatrices[PoseID] = computeTransformationMatrix(mas.dgMode == Modes.DeformationGradient.POSE_TO_POSE ? mas.poses[PoseID - 1] : mas.restPose, mas.poses[PoseID], scaling);
                }
#if CPU_PARALLEL
);
#endif
            }
        }

        public void     computeMatrix(Matrix    <double>         matrix)
        {
            _centroid = new DenseVector(matrix.ColumnCount);
            foreach (int ElementID in _elementsV)
                _centroid += matrix.Row(ElementID);
            _centroid.Divide(_elementsV.Count, _centroid);
        }
        public void     computeMatrix(Dictionary<int, double> [] matrix)
        {
            var listOfKeyValuesToModify = new List<KeyValuePair<int, double>>();

            _centroidDict = new Dictionary<int, double>();
            foreach (int ElementID in _elementsV)
                foreach (KeyValuePair<int, double> KeyValues in matrix[ElementID])
                {
                    if (_centroidDict.ContainsKey(KeyValues.Key))
                        listOfKeyValuesToModify.Add(new KeyValuePair<int, double>(KeyValues.Key, KeyValues.Value));
                    else
                        _centroidDict.Add(KeyValues.Key, KeyValues.Value);
                }

            foreach (var KeyValue in listOfKeyValuesToModify)
                _centroidDict[KeyValue.Key] += KeyValue.Value;

            var listOfKeysToModify = new List<int>();
            foreach (KeyValuePair<int, double> KeyValues in _centroidDict)
                listOfKeysToModify.Add(KeyValues.Key);
            foreach (var Key in listOfKeysToModify)
                _centroidDict[Key] /= _elementsV.Count;
        }

        #endregion

        #region Compute Error Functions

        public double computeErrorFromHead (Mesh3DAnimationSequence mas, Mesh3D pose, int vertexID)
        {
            return pose.computeVectorsEuclideanDistance(vertexID, _head);
            //return (mas.clusteringVertexDistanceMode == Modes.ClusteringVertexDistance.EUCLIDEAN) ?
                //pose.computeVectorsEuclideanDistance(vertexID, _head) :
              //  pose.computeVectorsGeodesicDistance (vertexID, _head) ;
        }
        public double computeErrorFromHeads(Mesh3DAnimationSequence mas, int vertexID)
        {
            double ErrorTotal = 0.0;
            foreach (Mesh3D Pose in mas.poses)
            {
                double Error = computeErrorFromHead(mas, Pose, vertexID);
                ErrorTotal += Error * Error;
            }
            return ErrorTotal;
        }

        public double computeErrorFromVertex    (Mesh3DAnimationSequence mas, Mesh3D pose, int vertexID)
        {
            return (mas.clusteringVertexDistanceMode == Modes.ClusteringVertexDistance.EUCLIDEAN) ?
                (pose.verticesData[vertexID] - _center[pose.poseID]).Length :
                computeErrorFromHead(mas, pose, vertexID);
        }
        public double computeErrorFromVertexP2P(Mesh3DAnimationSequence mas, Mesh3D pose, int vertexID)
        {
            return (mas.clusteringVertexDistanceMode == Modes.ClusteringVertexDistance.EUCLIDEAN) ?
                (pose.verticesData[vertexID] - _center[0]).Length :
                computeErrorFromHead(mas, pose, vertexID);
        }
        public double computeErrorFromVertices  (Mesh3DAnimationSequence mas, int vertexID)
        {
            double ErrorTotal = 0.0;
            foreach (Mesh3D Pose in mas.poses)
            {
                double Error = computeErrorFromVertex(mas, Pose, vertexID);
                ErrorTotal += Error * Error;
            }
            return ErrorTotal;
        }

        public double computeErrorFromNormal(Mesh3DAnimationSequence mas, Mesh3D pose, int vertexID)
        {
            return (mas.clusteringVertexDistanceMode == Modes.ClusteringVertexDistance.EUCLIDEAN) ?
                (pose.normalsVerticesData[vertexID] - _normal[pose.poseID]).Length :
                GeometryFunctions.computeVectorsAngle(pose.normalsVerticesData[vertexID], _normal[pose.poseID]);
        }
        public double computeErrorFromNormalP2P(Mesh3DAnimationSequence mas, Mesh3D pose, int vertexID)
        {
            return (mas.clusteringVertexDistanceMode == Modes.ClusteringVertexDistance.EUCLIDEAN) ?
                (pose.normalsVerticesData[vertexID] - _normal[0]).Length :
                GeometryFunctions.computeVectorsAngle(pose.normalsVerticesData[vertexID], _normal[0]);
        }
        public double computeErrorFromNormals(Mesh3DAnimationSequence mas, int vertexID)
        {
            double ErrorTotal = 0.0;
            foreach (Mesh3D Pose in mas.poses)
            {
                double Error = computeErrorFromNormal(mas, Pose, vertexID);
                ErrorTotal += Error * Error;
            }
            return ErrorTotal;
        }

        public double computeErrorFromDG(Mesh3D pose, int vertexID)
        {
            return Math.Abs((double)_DG.VerticesData[vertexID] - _dg[pose.poseID]);
            //return (double)_DG.verticesData[vertexID] - _dg[pose.poseID];
        }
        public double computeErrorFromDGP2P(Mesh3DAnimationSequence mas, Mesh3D pose, int vertexID)
        {
            return Math.Abs((double)_DG.VerticesData[vertexID] - _dg[0]);
            //return (double)_DG.verticesData[vertexID] - _dg[0];
        }
        public double computeErrorFromDGs(Mesh3DAnimationSequence mas, int vertexID)
        {
            double ErrorTotal = 0.0;
            foreach (Mesh3D Pose in mas.poses)
                if (Pose.poseID != mas.selectedRestPose)
                {
                    double Error = computeErrorFromDG(Pose, vertexID);
                    ErrorTotal += Error * Error;
                }
            return ErrorTotal;
        }

        public double computeErrorFromTransformation(Mesh3D restPose, Mesh3D thisPose, int vertexID)
        {
            var BoneMatrix44 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(_transformationMatrices[thisPose.poseID]);
            var BoneMatrix34 = new DenseMatrix(3, 4); BoneMatrix34.SetSubMatrix(0, 3, 0, 4, BoneMatrix44);

            Vector3 ApproxVertex;
            Vector4 RestPoseVertex = new Vector4(restPose.verticesData[vertexID], 1.0f);
            var NewVertex = BoneMatrix34 * OpenTK_To_MathNET.Vector4ToVector(RestPoseVertex);
            OpenTK_To_MathNET.ArrayToVector3(NewVertex.ToArray(), out ApproxVertex);

            return (thisPose.verticesData[vertexID] - ApproxVertex).Length;
        }
        public double computeErrorFromTransformationP2P(Mesh3D restPose, Mesh3D thisPose, int vertexID)
        {
            var BoneMatrix44 = OpenTK_To_MathNET.Matrix4ToDenseMatrix(_transformationMatrices[0]);
            var BoneMatrix34 = new DenseMatrix(3, 4); BoneMatrix34.SetSubMatrix(0, 3, 0, 4, BoneMatrix44);

            Vector3 ApproxVertex;
            Vector4 RestPoseVertex = new Vector4(restPose.verticesData[vertexID], 1.0f);
            var NewVertex = BoneMatrix34 * OpenTK_To_MathNET.Vector4ToVector(RestPoseVertex);
            OpenTK_To_MathNET.ArrayToVector3(NewVertex.ToArray(), out ApproxVertex);

            return (thisPose.verticesData[vertexID] - ApproxVertex).Length;
        }
        public double computeErrorFromTransformations(Mesh3DAnimationSequence mas, int vertexID)
        {
            double ErrorTotal = 0.0;
            foreach (Mesh3D Pose in mas.poses)
                if (Pose.poseID != mas.selectedRestPose)
                {
                    double Error = computeErrorFromTransformation(mas.dgMode == Modes.DeformationGradient.POSE_TO_POSE ? mas.poses[Pose.poseID - 1] : mas.restPose, Pose, vertexID);
                    ErrorTotal += Error * Error;
                }
            return ErrorTotal;
        }

        public double computeErrorFromCentroid(Matrix<double> matrix, int vertexID)
        {
            return (new DenseVector((matrix.Row(vertexID) - _centroid).ToArray())).Norm(2);
        }
        public double computeErrorFromCentroid(Dictionary<int, double>[] matrix, int vertexID)
        {
            Double Error = 0.0f;
            foreach (KeyValuePair<int, double> KeyValues in _centroidDict)
                Error += matrix[vertexID].ContainsKey(KeyValues.Key) ? Math.Pow(KeyValues.Value - matrix[vertexID][KeyValues.Key], 2) : Math.Pow(KeyValues.Value, 2);
            foreach (KeyValuePair<int, double> KeyValues in matrix[vertexID])
                if (!_centroidDict.ContainsKey(KeyValues.Key))
                    Error += Math.Pow(KeyValues.Value, 2);
            return Math.Sqrt(Error);
        }

        public double computeError(Matrix       <double>        matrix)
        {
            double[] Errors = new double[_elementsV.Count];
#if CPU_PARALLEL
            Parallel.For(0, _elementsV.Count, ElementID =>
#else
            for (int ElementID=0; ElementID < _elementsV.Count; ElementID++)
#endif
            {
                Errors[ElementID] = computeErrorFromCentroid(matrix, _elementsV[ElementID]);
            }
#if CPU_PARALLEL
);
#endif
            _error = 0.0;
            for (int ElementID = 0; ElementID < _elementsV.Count; ElementID++)
                _error += Errors[ElementID] * Errors[ElementID];
            return _error;
        }
        public double computeError(Dictionary   <int, double>[] matrix)
        {
            double[] Errors = new double[_elementsV.Count];
#if CPU_PARALLEL
            Parallel.For(0, _elementsV.Count, ElementID =>
#else
            for (int ElementID=0; ElementID < _elementsV.Count; ElementID++)
#endif
            {
                Errors[ElementID] = computeErrorFromCentroid(matrix, _elementsV[ElementID]);
            }
#if CPU_PARALLEL
);
#endif
            _error = 0.0;
            for (int ElementID = 0; ElementID < _elementsV.Count; ElementID++)
                _error += Errors[ElementID] * Errors[ElementID];
            return _error;
        }

        public double computeError(Mesh3DAnimationSequence mas, Mesh3D pose, int vertexID)
        {
            if      (mas.clusteringDistanceMode == Modes.ClusteringDistance.VERTEX)                 return computeErrorFromVertexP2P(mas, pose, vertexID);
            else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.NORMAL)                 return computeErrorFromNormalP2P(mas, pose, vertexID);
            else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.DEFORMATION_GRADIENT)   return computeErrorFromDGP2P    (mas, pose, vertexID);
            else                                                                                    return
                pose.poseID == mas.selectedRestPose ? 0.0 :
                computeErrorFromTransformationP2P(mas.dgMode == Modes.DeformationGradient.POSE_TO_POSE ? mas.poses[pose.poseID - 1] : mas.restPose, pose, vertexID);
        }
        public double computeError(Mesh3DAnimationSequence mas, int vertexID)
        { 
            if      (mas.clusteringDistanceMode == Modes.ClusteringDistance.VERTEX)                 return computeErrorFromVertices         (mas, vertexID);
            else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.NORMAL)                 return computeErrorFromNormals          (mas, vertexID);
            else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.DEFORMATION_GRADIENT)   return computeErrorFromDGs              (mas, vertexID);
            else                                                                                    return computeErrorFromTransformations  (mas, vertexID);
        }
        public double computeError(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            double[] Errors = new double[_elementsV.Count];
#if CPU_PARALLEL
            Parallel.For(0, _elementsV.Count, ElementID =>
#else
            for (int ElementID=0; ElementID < _elementsV.Count; ElementID++)
#endif
            {
                Errors[ElementID] = mas.clusteringPerPose ? computeError(mas, pose, _elementsV[ElementID]) : computeError(mas, _elementsV[ElementID]);
            }
#if CPU_PARALLEL
);
#endif
            _error = 0.0;
            double ErrorMin = double.MaxValue;
            for (int ElementID = 0; ElementID < _elementsV.Count; ElementID++)
            {
                if (Errors[ElementID] < ErrorMin)
                {
                    ErrorMin = Errors[ElementID];
                    _head = _elementsV[ElementID];
                }
                _error += Errors[ElementID];
            }
            return _error;
        }

        #endregion

        #region Compute Neighborhood
        public void computeNeighborhood(Mesh3D pose)
        {
            _bordersV   = new List<List<int>>();
            _neighbors  = new List<Cluster>();

            foreach (int ElementID in _elementsV)
            {
               // bool Added = false;
                foreach (int NeighborID in pose.neighborsVerticesData[ElementID])
                    if (pose.clustersVerticesData[ElementID].id != pose.clustersVerticesData[NeighborID].id)
                    {
                        int Index = _neighbors.IndexOf(pose.clustersVerticesData[NeighborID]);
                        if (Index == -1)
                        {
                            _neighbors.Add(pose.clustersVerticesData[NeighborID]);

                            //Added = true;
                            _bordersV.Add(new List<int>());
                            _bordersV[_bordersV.Count - 1].Add(ElementID);
                        }
                        else //if (!Added)
                        {
                            //Added = true;
                            _bordersV[Index].Add(ElementID);
                        }
                    }
            }
        }
        #endregion

        #region Cleaning Functions
        public void is_hCleanable(double areaPrev, double areaThis, double h_cleaning)
        {
            _toBeCleaned = false;
            if (_area < areaThis)
            {
                double AreaTolerancePrev = areaPrev * h_cleaning;
                double AreaToleranceThis = areaThis * h_cleaning;
                if (_area < AreaTolerancePrev && _area < AreaToleranceThis)
                    _toBeCleaned = true;
            }
        }
        public void clean(Mesh3D pose)
        {
            // 1. Move Vertices
            foreach (int ElementID_C in _elementsV)
            {
                int MinNeighborID = -1;
                double MinNeighborDistance = double.MaxValue;

                for (int NeighborID = 0; NeighborID < _neighbors.Count; NeighborID++)
                {
                    foreach (int ClusterPrevID in _clusterPrevIDs)
                        if (_elementsV.Count == _clusterPrev.elementsV.Count || // If has no neighbors with same Prev Cluster
                            _neighbors[NeighborID].clusterPrevIDs.Contains(ClusterPrevID))
                        {
                            int Index = _neighbors[NeighborID].neighbors.IndexOf(this);
                            List<int> Borders = _neighbors[NeighborID].bordersV[Index];

                            foreach (int ElementID_N in Borders)
                            {
                                double NeighborDistance = pose.computeVectorsEuclideanDistance(ElementID_C, ElementID_N);
                                if (NeighborDistance < MinNeighborDistance)
                                {
                                    MinNeighborID = NeighborID;
                                    MinNeighborDistance = NeighborDistance;
                                }
                            }
                            break;
                        }
                }

                if (MinNeighborID == -1)
                    MinNeighborID = 0;

                Cluster ClusterSelected = _neighbors[MinNeighborID];
                ClusterSelected.addElementV(ElementID_C);

                foreach (int ClusterPrevID in _clusterPrevIDs)
                    if (!ClusterSelected.clusterPrevIDs.Contains(ClusterPrevID))
                        ClusterSelected.clusterPrevIDs.Add(ClusterPrevID);

                pose.clustersVerticesData[ElementID_C] = ClusterSelected;
            }

            // 2. Move Facets
            foreach (int FacetID in _elementsF)
            {
                int[] Indices = new int[3];
                for (int I = 0; I < 3; I++)
                    Indices[I] = (int)pose.indicesData[3 * FacetID + I];

                if (pose.clustersVerticesData[Indices[0]].id == pose.clustersVerticesData[Indices[1]].id &&
                    pose.clustersVerticesData[Indices[1]].id == pose.clustersVerticesData[Indices[2]].id)
                {
                    pose.clustersVerticesData[Indices[0]].addElementF(FacetID);
                    pose.clustersVerticesData[Indices[0]].area += pose.areasFacetData[FacetID];
                    pose.clustersFacetsData[FacetID] = pose.clustersVerticesData[Indices[0]];
                }
            }

            // 3. Recompute Neighborhood
            Parallel.ForEach(_neighbors, ClusterN =>
            {
                ClusterN.computeNeighborhood(pose);
            });
        }
        #endregion

        #region Compute Connected Components
        public List<Cluster> computeConnectedComponents(Mesh3DAnimationSequence mas, Mesh3D pose, bool merged, double h_cleaning)
        {
            List<Cluster>   ClustersNew         = new List<Cluster>();
            List<List<int>> ConnectedComponents = new List<List<int>>();

            List<int> Queue = new List<int>();
            bool[]    Used  = new bool[pose.verticesCount];
            foreach (int ElementID in _elementsV) Used[ElementID] = true;

            // Compute Connected Components using BFS traversal
            int TotalCount = 0;
            while (TotalCount < _elementsV.Count)
            {
                List<int> ComponentElements = new List<int>();

                // Find Head of Component 
                int HeadID=-1;
                foreach (int ElementID in _elementsV)
                    if (Used[ElementID])
                    {
                        HeadID = ElementID;
                        Used[ElementID] = false;
                        break;
                    }               
                Queue.Add(HeadID);

                // Add Elements
                while (Queue.Count > 0)
                {
                    int ElementID = Queue[0];
                    foreach (int ElementID_N in pose.neighborsVerticesData[ElementID])
                        if(Used[ElementID_N])
                        {
                            Used[ElementID_N] = false;
                            Queue.Add(ElementID_N);
                        }
                    Queue.RemoveAt(0);

                    ComponentElements.Add(ElementID);
                }
                ConnectedComponents.Add(ComponentElements);

                TotalCount += ComponentElements.Count;
            }

            if (ConnectedComponents.Count > 1)
            {
                // 1. Compute Facets for each new Cluster 
                List<int>[] Facets = new List<int>[ConnectedComponents.Count];
                for (int ComponentID = 0; ComponentID < ConnectedComponents.Count; ComponentID++)
                    Facets[ComponentID] = new List<int>();
                foreach (int FacetID in _elementsF)
                {
                    int[] Indices = new int[3];
                    for (int I = 0; I < 3; I++)
                        Indices[I] = (int)pose.indicesData[3 * FacetID + I];

                    for (int ComponentID = 0; ComponentID < ConnectedComponents.Count; ComponentID++)
                        if (ConnectedComponents[ComponentID].Contains(Indices[0]) &&
                            ConnectedComponents[ComponentID].Contains(Indices[1]) &&
                            ConnectedComponents[ComponentID].Contains(Indices[2]))
                        {
                            Facets[ComponentID].Add(FacetID);
                            break;
                        }
                }
                
                // 2. Find largest Component
                int     MaxClusterID       = -1;
                double  MaxClusterElements =  0.0;
                for (int ClusterID = 0; ClusterID < ConnectedComponents.Count; ClusterID++)
                    if (ConnectedComponents[ClusterID].Count >= MaxClusterElements)
                    {
                        MaxClusterID       = ClusterID;
                        MaxClusterElements = ConnectedComponents[ClusterID].Count;
                    }

                // 3. Create Rest Components
                for (int ClusterID = 0; ClusterID < ConnectedComponents.Count; ClusterID++)
                    if (ClusterID != MaxClusterID)
                    {
                        Cluster ClusterNew = new Simple_Cluster(pose, ConnectedComponents[ClusterID][0], pose.clusteringMethod.clustersMerged.Count + ClusterID, 1, mas.dgMode);
                        // 3.1 Set Color
                        ClusterNew.clusterThis    = _clusterThis;
                        ClusterNew.clusterPrev    = _clusterPrev;
                        ClusterNew.clusterPrevIDs = new List<int>(_clusterPrevIDs);
                        ClusterNew.CAV            = _CAV;
                        ClusterNew.sphere.color   = _sphere.color;
                        ClusterNew.sphere.colorID = _sphere.colorID;

                        // 3.3 Add/Remove Vertex Elements
                        removeElementV(ConnectedComponents[ClusterID][0]);
                        pose.clustersVerticesData[ConnectedComponents[ClusterID][0]] = ClusterNew;
                        for (int ElementID = 1; ElementID < ConnectedComponents[ClusterID].Count; ElementID++)
                        {
                            ClusterNew.addElementV(ConnectedComponents[ClusterID][ElementID]);
                            removeElementV(ConnectedComponents[ClusterID][ElementID]);
                            pose.clustersVerticesData[ConnectedComponents[ClusterID][ElementID]] = ClusterNew;
                        }

                        // 3.4 Add/Remove Facet Elements
                        ClusterNew.addRangeElementF(Facets[ClusterID]);
                        foreach (int FacetID in Facets[ClusterID])
                        {
                            pose.clustersFacetsData[FacetID] = ClusterNew;
                            removeElementF(FacetID);
                        }

                        // 3.5 Add new Cluster to List
                        ClustersNew.Add(ClusterNew);
                    }
            }

            return ClustersNew;
        }
        #endregion 

        #region Compute MaxDistance for Weighting
        public void     addElement(ref SortedDictionary<double, List<int>> distances, int vertexID, double distance)
        {
            //Check if a node with the same distance already exists
            List<int> tList;
            distances.TryGetValue(distance, out tList);

            if (tList == null)//If not create the list and add to the new node
            {
                tList = new List<int>();
                tList.Add(vertexID);
                distances.Add(distance, tList);
            }
            else//If yes add the node to the list of the existing
            {
                //But first check to see if we are attempting to insert the same element
                if (!tList.Contains(vertexID))
                    tList.Add(vertexID);
            }
        }
        public double   getMax(ref SortedDictionary<double, List<int>> distances)
        {
            foreach (KeyValuePair<double, List<int>> pivot in distances)
                return pivot.Key;
            return 0.0;
        }
        public void     computeMax(Mesh3DAnimationSequence mas)
        {
            SortedDictionary<double, List<int>> distances = new SortedDictionary<double, List<int>>(new Comparers.DoubleKeyComparerDesc());

            foreach (int ElementID in _elementsV)
            {
                double Error = mas.clusteringPerPose ? computeErrorFromVertexP2P(mas, mas.pose, ElementID) : computeErrorFromVertex(mas, mas.pose, ElementID);
                addElement(ref distances, ElementID, Error);
            }
            _maxDistance = getMax(ref distances);
        }
        #endregion 

        #region Smooth Boundaries
        public void smoothBoundaries(Mesh3D pose, Cluster clusterN)
        {
            int EdgesCount = 0;
            List<int> ElementsFuzzy = new List<int>();

            // Source
            List<int> ElementsFuzzySourceB = new List<int>();
            List<int> ElementsFuzzySourceN = new List<int>();

            // Hop-1
            int IndexSourse = _neighbors.IndexOf(clusterN);
            if (IndexSourse == -1)
                return;
            ElementsFuzzySourceB.AddRange(_bordersV[IndexSourse]);
            // Hop-2
            foreach (int ElementID in ElementsFuzzySourceB)
                foreach (int NeighborID in pose.neighborsVerticesData[ElementID])
                    if (!ElementsFuzzySourceB.Contains(NeighborID) && !ElementsFuzzySourceN.Contains(NeighborID) && _elementsV.Contains(NeighborID))
                        ElementsFuzzySourceN.Add(NeighborID);
            if (ElementsFuzzySourceN.Count == 0)
                return;

            // Add Elements to Fuzzy
            ElementsFuzzy.AddRange(ElementsFuzzySourceN);
            ElementsFuzzy.AddRange(ElementsFuzzySourceB);
            int ElementsSourceCount = ElementsFuzzy.Count;

            // Sink
            List<int> ElementsFuzzySinkB = new List<int>();
            List<int> ElementsFuzzySinkN = new List<int>();
            // Hop-1
            int IndexSink = clusterN.neighbors.IndexOf(this);
            if (IndexSink == -1)
                return;
            ElementsFuzzySinkB.AddRange(clusterN.bordersV[IndexSink]);
            // Hop-2
            foreach (int ElementID in ElementsFuzzySinkB)
                foreach (int NeighborID in pose.neighborsVerticesData[ElementID])
                    if (!ElementsFuzzySinkB.Contains(NeighborID) && !ElementsFuzzySinkN.Contains(NeighborID) && clusterN.elementsV.Contains(NeighborID))
                        ElementsFuzzySinkN.Add(NeighborID);
            if (ElementsFuzzySinkN.Count == 0)
                return;

            // Add Elements to Fuzzy
            ElementsFuzzy.AddRange(ElementsFuzzySinkB);
            ElementsFuzzy.AddRange(ElementsFuzzySinkN);
            int ElementsTotalCount = ElementsFuzzy.Count;

            // Compute Capacities
            double  DistaceEdgeAverage = 0.0;

            int   [ ] IDs_Element       = new int   [ElementsTotalCount];
            bool  [ ] ElementType       = new bool  [ElementsTotalCount];
            double[,] CapacitiesEdge    = new double[ElementsTotalCount, ElementsTotalCount];
            double[,] CapacitiesTotal   = new double[ElementsTotalCount, ElementsTotalCount+2];

#if CPU_PARALLEL
            Parallel.For(0, ElementsTotalCount, ElementIndex =>
#else
            for (int ElementIndex = 0; ElementIndex < ElementsTotalCount; ElementIndex++)
#endif
            {
                int ElementID = ElementsFuzzy[ElementIndex];

                if      (ElementIndex < ElementsFuzzySourceN.Count)
                {
                    IDs_Element[ElementIndex] = 0;

                    CapacitiesTotal[ElementIndex, ElementsTotalCount    ] = 0.0;
                    CapacitiesTotal[ElementIndex, ElementsTotalCount + 1] = Double.MaxValue;
                }
                else if (ElementIndex < ElementsSourceCount)
                {
                    IDs_Element[ElementIndex] = 1;

                    double DistanceSource   = pose.computeVectorsEuclideanDistance(ElementID, _head);
                    double DistanceSink     = pose.computeVectorsEuclideanDistance(ElementID, clusterN.head);
                    double DistanceSS       = DistanceSource + DistanceSink;

                    CapacitiesTotal[ElementIndex, ElementsTotalCount    ] = DistanceSource / DistanceSS;
                    CapacitiesTotal[ElementIndex, ElementsTotalCount + 1] = DistanceSink   / DistanceSS;
                }
                else if (ElementIndex < ElementsSourceCount + ElementsFuzzySinkB.Count)
                {
                    IDs_Element[ElementIndex] = 2;

                    double DistanceSource   = pose.computeVectorsEuclideanDistance(ElementID, _head);
                    double DistanceSink     = pose.computeVectorsEuclideanDistance(ElementID, clusterN.head);
                    double DistanceSS       = DistanceSource + DistanceSink;

                    CapacitiesTotal[ElementIndex, ElementsTotalCount    ] = DistanceSink   / DistanceSS;
                    CapacitiesTotal[ElementIndex, ElementsTotalCount + 1] = DistanceSource / DistanceSS;
                }
                else
                {
                    IDs_Element[ElementIndex] = 3;

                    CapacitiesTotal[ElementIndex, ElementsTotalCount    ] = Double.MaxValue;
                    CapacitiesTotal[ElementIndex, ElementsTotalCount + 1] = 0.0;
                }
            }
#if CPU_PARALLEL
);
#endif
            for (int ElementIndex = 0; ElementIndex < ElementsTotalCount; ElementIndex++)
            {
                int ID_Element = IDs_Element[ElementIndex];
                int ElementID  = ElementsFuzzy[ElementIndex];

                foreach (int NeighborID in pose.neighborsVerticesData[ElementID])
                {
                    int NeighborIndex = ElementsFuzzy.IndexOf(NeighborID);

                    if (NeighborIndex != -1)
                    {
                        int      ID_Neighbor = -1;
                        if      (NeighborIndex < ElementsFuzzySourceN.Count)                     ID_Neighbor = 0;
                        else if (NeighborIndex < ElementsSourceCount)                            ID_Neighbor = 1;
                        else if (NeighborIndex < ElementsSourceCount + ElementsFuzzySinkB.Count) ID_Neighbor = 2;
                        else                                                                     ID_Neighbor = 3;

                        double DistanceEdge = 0.0;

                        for (int FacetIndex = 0; FacetIndex < pose.facetsVerticesData[ElementID].Count; FacetIndex++)
                        {
                            int FacetID = pose.facetsVerticesData[ElementID][FacetIndex];
                            int VertexElement=-1, VertexNeighbor=-1, VertexOther=-1;

                            int[] Indices = new int[3];
                            for (int I = 0; I < 3; I++)
                            {
                                Indices[I] = (int)pose.indicesData[3 * FacetID + I];

                                if     (Indices[I] == ElementID ) VertexElement  = Indices[I];
                                else if(Indices[I] == NeighborID) VertexNeighbor = Indices[I];
                                else                              VertexOther    = Indices[I];
                            }

                            if (VertexElement != -1 && VertexNeighbor != -1 && ElementsFuzzy.Contains(VertexOther))
                            {
                                Vector3 P1 = new Vector3();
                                Vector3 P2 = new Vector3();

                                P1 = (ID_Element == ID_Neighbor) ? pose.verticesData[VertexElement] + pose.verticesData[VertexOther] : pose.verticesData[VertexElement] + pose.verticesData[VertexNeighbor];
                                P1 = Vector3.Divide(P1, 2.0f);
                                P2 = pose.verticesData[VertexNeighbor] + pose.verticesData[VertexOther];
                                P2 = Vector3.Divide(P2, 2.0f);

                                DistanceEdge += (P1 - P2).Length;
                            }
                        }

                        EdgesCount++;
                        DistaceEdgeAverage += DistanceEdge;
                        CapacitiesEdge [ElementIndex, NeighborIndex] = DistanceEdge;
                    }
                }
            }
            DistaceEdgeAverage /= EdgesCount;

            // Scale Values
#if CPU_PARALLEL
          Parallel.For(0, ElementsTotalCount, ElementIndex_I =>
#else
            for (int ElementIndex_I = 0; ElementIndex_I < ElementsTotalCount; ElementIndex_I++)
#endif
            {
                for (int ElementIndex_J = ElementIndex_I + 1; ElementIndex_J < ElementsTotalCount; ElementIndex_J++)
                    if (CapacitiesEdge[ElementIndex_I, ElementIndex_J] > 0.0)
                        CapacitiesTotal[ElementIndex_I, ElementIndex_J] = CapacitiesEdge[ElementIndex_I, ElementIndex_J] / (CapacitiesEdge[ElementIndex_I, ElementIndex_J] + DistaceEdgeAverage);
            }
#if CPU_PARALLEL
);
#endif
            // Call Min Cut Algorithm (c++ code)
            unsafe
            {
                fixed (double* Junk = &CapacitiesTotal[0, 0])
                {
                    double*[] CapacitiesTmp = new double*[CapacitiesTotal.GetLength(0)];
                    for (int Index = 0; Index < CapacitiesTotal.GetLength(0); Index++)
                        fixed (double* ptr = &CapacitiesTotal[Index, 0])
                        {
                            CapacitiesTmp[Index] = ptr;
                        }

                    fixed (double** CapacitiesPtr = &CapacitiesTmp[0])
                    {
                        bool* Components = minimumCut(CapacitiesPtr, ElementsTotalCount, EdgesCount);
#if CPU_PARALLEL
                        Parallel.For(0, ElementsTotalCount, ElementIndex =>
#else
                        for (int ElementIndex = 0; ElementIndex < ElementsTotalCount; ElementIndex++)
#endif
                        {
                            ElementType[ElementIndex] = Components[ElementIndex];
                        }
#if CPU_PARALLEL
);
#endif
                        for (int ElementIndex = 0; ElementIndex < ElementsSourceCount; ElementIndex++)
                        {
                            if (ElementType[ElementIndex])
                            {
                                int ElementID = ElementsFuzzy[ElementIndex];

                                removeElementV(ElementID);
                                clusterN.addElementV(ElementID);
                                pose.clustersVerticesData[ElementID] = clusterN;
                            } 
                        }

                        for (int ElementIndex = ElementsSourceCount; ElementIndex < ElementsTotalCount; ElementIndex++)
                        {
                            if (!ElementType[ElementIndex])
                            {
                                int ElementID = ElementsFuzzy[ElementIndex];

                                addElementV(ElementID);
                                clusterN.removeElementV(ElementID);
                                pose.clustersVerticesData[ElementID] = this;
                            }
                        }
                    }
                }
            }

            computeNeighborhood(pose);
        }
        #endregion

        #region Draw Function
        public void draw()
        {
             _sphere.draw();
        }
        #endregion
    }

    public class Simple_Cluster : Cluster
    {
        #region Constructor
        public Simple_Cluster(Mesh3D pose, int headIndex, int id, int numOfPoses, Modes.DeformationGradient dgMode)
            : base(pose, headIndex, id, numOfPoses, dgMode)
        {
            addElementV(_head);
        }
        #endregion
    }

    public class P_Center_Cluster : Cluster
    {
        #region Private Properties
        SortedDictionary<double, List<int>> _distances;
        #endregion

        #region Public Properties
        public SortedDictionary<double, List<int>> distances
        {
            get { return _distances; }
        }
        #endregion

        #region Constructor
        public P_Center_Cluster(Mesh3D pose, int headIndex, int id, int numOfPoses, Modes.DeformationGradient dgMode)
            : base(pose, headIndex, id, numOfPoses, dgMode)
        {          
            _elementsV.Add(_head);

            List<int> hList = new List<int>();
            hList.Add(_head);
            _distances = new SortedDictionary<double, List<int>>(new Comparers.DoubleKeyComparerDesc());
            _distances.Add(0.0f, hList);
        }
        #endregion

        #region Get Functions
        public List<int>   getList(double distance)
        {
            List<int> result;
            _distances.TryGetValue(distance, out result);
            return result;
        }
        public double      getMaximalDistance()
        {
            foreach (KeyValuePair<double, List<int>> pivot in _distances) 
                return pivot.Key;
            return 0.0;
        }
        #endregion

        #region Add Elements Function
        public void addElement(int vertexID, double distance)
        {
            _elementsV.Add(vertexID);

            //Check if a node with the same distance already exists
            List<int> tList = getList(distance);

            if (tList == null)//If not create the list and add to the new node
            {
                tList = new List<int>();
                tList.Add(vertexID);
                _distances.Add(distance, tList);
            }
            else//If yes add the node to the list of the existing
            {
                //But first check to see if we are attempting to insert the same element
                if (!tList.Contains(vertexID))
                    tList.Add(vertexID);
            }
        }
        #endregion

        #region Remove Elements Function
        public void removeElement(int vertexID, double distance)
        {
            List<int> vertexList = _distances[distance];
            if (vertexList != null)
            {
                vertexList.Remove(vertexID);
                if (vertexList.Count == 0)
                    _distances.Remove(distance);
            }
            _elementsV.Remove(vertexID);
        }
        #endregion
    }

    public class Divide_Conquer_Cluster : Cluster
    {
        #region Private Properties
        int _tail;
        #endregion

        #region Public Properties
        public int tail
        {
            get { return _tail; }
        }
        #endregion
        
        #region Constructor
        public Divide_Conquer_Cluster(Mesh3D pose, int headIndex, int numOfClusters, int id, int numOfPoses, Modes.DeformationGradient dgMode) : 
            base(pose, headIndex, id, numOfPoses, dgMode) { ;}
        #endregion

        #region Compute Closest Element to Error Function
        public void findClosestElement(Mesh3DAnimationSequence mas, Mesh3D pose, Dictionary<int, double> [] matrix, out double errorMax, out double errorTotal)
        {
            double  ErrorMin = double.MaxValue;
            errorMax    = 0.0;
            errorTotal  = 0.0;

            double[] Errors = new double[_elementsV.Count];
#if CPU_PARALLEL
            Parallel.For(0, _elementsV.Count, ElementID =>
#else
            for (int ElementID=0; ElementID < _elementsV.Count; ElementID++)
#endif
            {
                Errors[ElementID] = mas.clusteringPerPose                                               ? computeError(mas, pose, _elementsV[ElementID]) :
                                    mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING   ? computeErrorFromCentroid(matrix, _elementsV[ElementID]) : computeError(mas, _elementsV[ElementID]);
            }
#if CPU_PARALLEL
);
#endif
            for (int ElementID = 0; ElementID < _elementsV.Count; ElementID++)
            {
                if (Errors[ElementID] <= ErrorMin)
                {
                    ErrorMin = Errors[ElementID];
                    _head = _elementsV[ElementID];
                }

                if (Errors[ElementID] >= errorMax)
                {
                    errorMax = Errors[ElementID];
                    _tail = _elementsV[ElementID];
                }
                errorTotal += Errors[ElementID];
            }
            _error = errorTotal;
        }
        #endregion
    }

    public class C_PCA_Cluster : Cluster
    {
        #region Private Properties
        Vector<double> _centroid3;
        Matrix<double> _basisVectors;
        #endregion

        #region Public Properties
        public Vector<double> centroid3
        {
            get { return _centroid3; }
        }
        public Matrix<double> basisVectors
        {
            get { return _basisVectors; }
            set { _basisVectors = value; }
        }
        #endregion

        #region Constructor
        public C_PCA_Cluster(Mesh3D pose, int headIndex, int id, int numOfPoses, Modes.DeformationGradient dgMode) :
            base(pose, headIndex, id, numOfPoses, dgMode)
        {
            ;
        }
        #endregion

        #region Compute Center
        public void computeCenter(Matrix<double> matrix)
        {
            _centroid3 = new DenseVector(matrix.RowCount);
            foreach (int ElementID in _elementsV)
                _centroid3 += matrix.Column(ElementID);
            _centroid3 /= _elementsV.Count;
        }
        #endregion

        #region Compute Basis Vectors
        public void computeBasisVectors(Matrix<double> matrix,int BasisVectorsCount, bool nipals)
        {
            Matrix<double> MatrixData = new DenseMatrix(matrix.RowCount, _elementsV.Count);
            int Column = 0;
            foreach (int ElementID in _elementsV)
                MatrixData.SetColumn(Column++, matrix.Column(ElementID));

            Matrix<double> BasisVectors     = nipals ? MatrixFunctions.calculateNipals(MatrixData.Transpose(), BasisVectorsCount) : MatrixData.Svd(true).U;
            Matrix<double> SelBasisVectors  = BasisVectors.SubMatrix(0, BasisVectors.RowCount, 0, BasisVectorsCount);

            _basisVectors = new DenseMatrix(BasisVectorsCount, BasisVectors.RowCount);
            _basisVectors.SetSubMatrix(0, BasisVectorsCount, 0, BasisVectors.RowCount, SelBasisVectors.Transpose());
        }
        #endregion

        #region Compute Error
        public double computeError(Matrix<double> matrix    , int BasisVectorCount)
        {
            double[] Errors = new double[_elementsV.Count];
#if CPU_PARALLEL
            Parallel.For(0, _elementsV.Count, ElementID =>
#else
            for (int ElementID=0; ElementID < _elementsV.Count; ElementID++)
#endif
            {
                Errors[ElementID] = computeError(matrix.Column(_elementsV[ElementID]), BasisVectorCount);
            }
#if CPU_PARALLEL
);
#endif
            double Error = 0;
            foreach (int ElementID in _elementsV)
                Error += Errors[_elementsV[ElementID]];
            return Error;
        }
        public double computeError(Vector<double> vertexData, int BasisVectorCount)
        {
            Vector<double> Center3   = vertexData - _centroid3;
            Vector<double> SumEigen3 = new DenseVector(_centroid3.Count);

            for (int BasisID = 0; BasisID < BasisVectorCount; ++BasisID)
                SumEigen3 += Center3.DotProduct(_basisVectors.Row(BasisID)) * _basisVectors.Row(BasisID);

            return (Center3 - SumEigen3).Norm(2);
        }
        #endregion
    }

    public abstract class Clustering
    {
        #region Private Properties

        int  _selectedCluster;

        bool _drawSpheres;
        bool _drawRegions;
        bool _drawNeighbors;

        double        _h_cleaning;
        List<Cluster> _clustersMerged;

        int[][]       _CAVs;

        #endregion

        #region Protected Properties
        protected float     _pFactor;
        protected int       _count;

        protected bool      _nipals;
        protected bool      _randomSeeding;
        protected bool      _scalingFactor;
        
        protected int       _iterations;
        protected int       _maxIterations;

        protected double    _errorTotal;
        protected double    _errorTolerance;
        protected double    _errorTotalTolerance;

        protected Dictionary<int, double>[] _similarityMatrix;

        protected List<Cluster> _clusters;
        protected List<Cluster> _clustersBackUp;

        protected bool[]        _used;
        protected SortedDictionary<double, List<int[,]    >> _queue;
        protected SortedDictionary<double, List<Cluster[,]>> _queueClusters;
        protected SortedDictionary<double, List<Cluster[,]>> _queueClustersUp;

        protected Random        _randNum;
        #endregion

        #region Public Properties
        public float pFactor
        {
            get
            {
                return _pFactor;
            }
            set
            {
                _pFactor = value;
            }
        }
        public bool randomSeeding
        {
            get { return _randomSeeding; }
            set { _randomSeeding = value; }
        }
        public bool scalingFactor
        {
            get { return _scalingFactor; }
            set { _scalingFactor = value; }
        }
        public bool nipals
        {
            get { return _nipals; }
            set { _nipals = value; }
        }
        public int  count
        {
            get { return _count; }
            set { _count = value; }
        }
        public int iterations
        {
            get { return _iterations; }
        }
        public int maxIterations
        {
            set { _maxIterations = value; }
        }
        public double h_cleaning
        {
            get { return _h_cleaning; }
            set
            {
                _h_cleaning = value / 100.0;
            }
        }
        public double errorTolerance
        {
            get { return _errorTolerance; }
            set { _errorTolerance = value; }
        }
        public double errorTotalTolerance
        {
            get { return _errorTotalTolerance; }
            set { _errorTotalTolerance = value; }
        }
        public double errorTotal
        {
            get { return _errorTotal; }
        }
        public int  selectedCluster
        {
            get { return _selectedCluster; }
            set { _selectedCluster = value; }
        }
        public bool drawSpheres
        {
            get { return _drawSpheres; }
            set { _drawSpheres = value; }
        }
        public bool drawRegions
        {
            get { return _drawRegions; }
            set { _drawRegions = value; }
        }
        public bool drawNeighbors
        {
            get { return _drawNeighbors; }
            set { _drawNeighbors = value; }
        }
        public List<Cluster> clusters
        {
            get { return _clusters; }
        }
        public List<Cluster> clustersMerged
        {
            get { return _clustersMerged; }
            set
            {
                _clustersMerged = value;
            }
        }
        #endregion

        #region Constructor
        public Clustering()
        {
            _randNum        = new Random();
            _nipals         = false;
            _randomSeeding  = false;
            _scalingFactor  = false;
            _count          = 0;
            _iterations     = 0;
            _errorTotal     = double.MaxValue;
            _selectedCluster= -1;
            _drawSpheres    = true;
            _drawRegions    = true;
            _drawNeighbors  = false;
            _clusters       = new List<Cluster>();
            _clustersBackUp = new List<Cluster>();
            _clustersMerged = new List<Cluster>();
            
            _h_cleaning = 0.0;
            _pFactor    = 1.8f;
        }
        #endregion

        #region Abstract Function
        public abstract void initialSeeding (Mesh3DAnimationSequence mas, Mesh3D pose);
        public abstract void compute        (Mesh3DAnimationSequence mas, Mesh3D pose);
        #endregion

        #region Queue Functions

        #region Get Functions
        private List<int[,]>     getList(double error)
        {
            List<int[,]> result;
            _queue.TryGetValue(error, out result);
            return result;
        }
        private List<Cluster[,]> getListCluster(double error)
        {
            List<Cluster[,]> result;
            _queueClusters.TryGetValue(error, out result);
            return result;
        }
        private List<Cluster[,]> getListClusterUp(double error)
        {
            List<Cluster[,]> result;
            _queueClustersUp.TryGetValue(error, out result);
            return result;
        }
        #endregion

        #region Add Elements Function
        protected void addElement2Queue(double error, int listA, int listB)
        {
            //Check if a node with the same distance already exists
            List<int[,]> tList = getList(error);

            if (tList == null)  //If not, create the list and add to the new node
            {
                tList = new List<int[,]>();
                tList.Add(new int[1, 2] { { listA, listB } });

                _queue.Add(error, tList);
            }
            else //If yes add the node to the list of the existing
            {
                int[,] NewElement = new int[1, 2] { { listA, listB } };
                //But first check to see if we are attempting to insert the same element
                if (!tList.Contains(NewElement))
                    tList.Add(NewElement);
            }
        }
        protected void addElement2Queue(double error, Cluster clusterA, Cluster clusterB)
        {
            //Check if a node with the same distance already exists
            List<Cluster[,]> tList = getListCluster(error);

            if (tList == null)  //If not, create the list and add to the new node
            {
                tList = new List<Cluster[,]>();
                tList.Add(new Cluster[1, 2] { { clusterA, clusterB } });

                _queueClusters.Add(error, tList);
            }
            else //If yes, add the node to the list of the existing
            {
                Cluster[,] NewElement = new Cluster[1, 2] { { clusterA, clusterB } };
                //But first check to see if we are attempting to insert the same element
                if (!tList.Contains(NewElement))
                    tList.Add(NewElement);
            }
        }
        protected void addElement2QueueUp(double error, Cluster clusterA, Cluster clusterB)
        {
            //Check if a node with the same distance already exists
            List<Cluster[,]> tList = getListClusterUp(error);

            if (tList == null)  //If not, create the list and add to the new node
            {
                tList = new List<Cluster[,]>();
                tList.Add(new Cluster[1, 2] { { clusterA, clusterB } });

                _queueClustersUp.Add(error, tList);
            }
            else //If yes, add the node to the list of the existing
            {
                Cluster[,] NewElement = new Cluster[1, 2] { { clusterA, clusterB } };
                //But first check to see if we are attempting to insert the same element
                if (!tList.Contains(NewElement))
                    tList.Add(NewElement);
            }
        }
        #endregion

        #region Add Adjacent Vertices Functions
        public void addAdjacentVertices(Mesh3DAnimationSequence mas, Mesh3D pose, int vertexID, int clusterID)
        {
            for (int NeighborID = 0; NeighborID < pose.neighborsVerticesData[vertexID].Count; NeighborID++)
            {
                int vID = pose.neighborsVerticesData[vertexID][NeighborID];
                if (!_used[vID])
                {
                    double Error = mas.clusteringPerPose ? _clusters[clusterID].computeError(mas, pose, vID) : 
                        (mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING) ? _clusters[clusterID].computeError(_similarityMatrix) : _clusters[clusterID].computeError(mas, vID);
                    addElement2Queue(Error, vID, clusterID);
                }
            }
        }
        #endregion

        #endregion

        #region Compute Centers Functions
        public void computeCenters(Mesh3DAnimationSequence mas)
        {
            foreach (Cluster ClusterC in _clusters)
                if (!ClusterC.isEmpty())
                    ClusterC.computeCenter(mas, _scalingFactor, _similarityMatrix);
        }
        public void computeCenters(Matrix<double> matrix)
        {
#if CPU_PARALLEL
            Parallel.ForEach(_clusters, ClusterC =>
#else
            foreach (Cluster ClusterC in _clusters)
#endif
            {
                if (!ClusterC.isEmpty())
                    ClusterC.computeMatrix(matrix);
            }
#if CPU_PARALLEL
);
#endif
        }
        #endregion

        #region Compute Transformation Matrices  
        public void computeTransformationMatrices(Mesh3DAnimationSequence mas, bool merged)
        {
            List<Cluster> Clusters = (merged) ? _clustersMerged : _clusters;

            foreach (Cluster ClusterC in Clusters)
            {
                ClusterC.computeCentersPos(mas);
                ClusterC.sphere.setBuffers();
            }

            if (mas.clusteringDistanceMode != Modes.ClusteringDistance.SKINNING)
                foreach (Cluster ClusterC in Clusters)
                    ClusterC.computeTransformationMatrices(mas, _scalingFactor);

            // P2P Clustering: Compute Transformation Matrices from RP (Sphere Animation)
            if (!mas.clusteringPerPose && mas.dgMode == Modes.DeformationGradient.POSE_TO_POSE)
                foreach (Cluster ClusterC in Clusters)
                    for (int PoseID = 1; PoseID < mas.poses.Count; PoseID++)
                        ClusterC.transformationMatrices[PoseID] = ClusterC.transformationMatrices[PoseID] * ClusterC.transformationMatrices[PoseID - 1];
        }
        public void computeTransformationMatricesEditing(Mesh3DAnimationSequence mas)
        {
            foreach (Cluster ClusterC in _clusters)
                ClusterC.computeTransformationMatricesEditing(mas, _scalingFactor);
        }
        #endregion

        #region Compute Error Functions
        public double computeErrorFromMatrix(int vertexID_1, int vertexID_2, Dictionary<int, double>[] matrix)
        {
            Double Error = 0.0f;
            foreach (KeyValuePair<int, double> KeyValues in matrix[vertexID_1])
                Error += matrix[vertexID_2].ContainsKey(KeyValues.Key) ? Math.Pow(KeyValues.Value - matrix[vertexID_2][KeyValues.Key], 2) : Math.Pow(KeyValues.Value, 2);
            foreach (KeyValuePair<int, double> KeyValues in matrix[vertexID_2])
                if (!matrix[vertexID_1].ContainsKey(KeyValues.Key))
                    Error += Math.Pow(KeyValues.Value, 2);
            return Math.Sqrt(Error);
        }
        public double computeError(Mesh3DAnimationSequence mas, Mesh3D pose, int vertexID_1, int vertexID_2)
        {
            double Error = 0.0;
            if      (mas.clusteringDistanceMode == Modes.ClusteringDistance.VERTEX)                 Error = pose.computeErrorFromVertex(mas, vertexID_1, vertexID_2);
            else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.NORMAL)                 Error = pose.computeErrorFromNormal(mas, vertexID_1, vertexID_2);
            else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.DEFORMATION_GRADIENT)   Error = pose.computeErrorFromDG(mas, vertexID_1, vertexID_2);
            else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING)             Error = computeErrorFromMatrix(vertexID_1, vertexID_2, _similarityMatrix);
            else Error = 0.0f; //computeErrorFromTransformations(mas, vertexID_1, vertexID_2); 
                
            return Error * Error;
        }
        public double computeError(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            double NewErrorTotal = 0.0;
            foreach (Cluster ClusterC in _clusters)
                NewErrorTotal += (mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING) ? ClusterC.computeError(_similarityMatrix) : ClusterC.computeError(mas, pose);
            return NewErrorTotal;
        }
        public double computeError(Matrix<double> matrix)
        {
            double NewErrorTotal = 0.0;
            foreach (Cluster ClusterC in _clusters)
                NewErrorTotal += ClusterC.computeError(matrix);
            return NewErrorTotal;
        }
        #endregion

        #region Compute Similarity Matrix Functions
        public void computeSimilarityMatrix(Mesh3DAnimationSequence mas)
        {
            _similarityMatrix = new Dictionary<int, double> [mas.verticesCount];

#if CPU_PARALLEL
            Parallel.For(0, mas.verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < mas.verticesCount; VertexID++)
#endif
            {
                _similarityMatrix[VertexID] = new Dictionary<int, double>();

                int TotalClusters = 0;
                foreach (Mesh3D Pose in mas.poses)
                {
                    if (Pose.poseID == mas.selectedRestPose)
                        continue;

                    /* Add Boundary Clusters
                    List<int> NeighborClusters = new List<int>();
                    NeighborClusters.Add(Pose.clustersVerticesData[VertexID].id);
                    
                    foreach (int NeighborID in Pose.neighborsVerticesData[VertexID])
                        if (Pose.clustersVerticesData[VertexID].id != Pose.clustersVerticesData[NeighborID].id)
                            if (!NeighborClusters.Contains(Pose.clustersVerticesData[NeighborID].id))
                                NeighborClusters.Add(Pose.clustersVerticesData[NeighborID].id);
                    

                    double Weight = 1.0 / (double)NeighborClusters.Count;
                    foreach (int NeighborClusterID in NeighborClusters)
                        _similarityMatrix[VertexID].Add(TotalClusters + NeighborClusterID, Weight);
                    */

                    _similarityMatrix[VertexID].Add(TotalClusters + Pose.clustersVerticesData[VertexID].id, 1.0);
                    
                    //TotalClusters += mas.pose.clusteringMode == Modes.Clustering.MERGE_RG ? mas.verticesCount : Pose.clusteringMethod.clusters.Count;
                    
                    TotalClusters += mas.clusteringPerPoseMerging ? Pose.clusteringMethod.clustersMerged.Count : Pose.clusteringMethod.clusters.Count;
                }
            }
#if CPU_PARALLEL
);
#endif
        }
        #endregion 

        #region Set IDs
        public void setIDs(bool merged)
        {
            List<Cluster> Clusters = (merged) ? _clustersMerged : _clusters;
#if CPU_PARALLEL
            Parallel.For(0, Clusters.Count, ClusterID =>
#else
            for (int ClusterID = 0; ClusterID < Clusters.Count; ClusterID++)
#endif
            {
                Clusters[ClusterID].id = ClusterID;
            }
#if CPU_PARALLEL
);
#endif
        }
        #endregion

        #region Compute Neighborhood
        public void computeNeighborhood(Mesh3D pose, bool merged)
        {
            List<Cluster> Clusters = (merged) ? _clustersMerged : _clusters;

            // Set Cluster ID for each Vertex
            foreach (Cluster ClusterC in Clusters)
#if CPU_PARALLEL
                Parallel.ForEach(ClusterC.elementsV, ElementID =>
#else
                foreach (int ElementID in _clusters[ClusterC.id].elementsV)
#endif
                {
                    pose.clustersVerticesData[ElementID] = ClusterC;
                }
#if CPU_PARALLEL
);
#endif
            // Compute Cluster Neighbors for each Cluster
#if CPU_PARALLEL
            Parallel.ForEach(Clusters, ClusterC =>
#else
            foreach (Cluster ClusterC in _clusters)
#endif
            {
                ClusterC.computeNeighborhood(pose);
            }
#if CPU_PARALLEL
);
#endif
        }
        #endregion

        #region Compute Colors
        public void setColor(bool fixedColors, bool merged, bool twoRingColoring)
        {
            if (fixedColors) setFixedColor(merged, twoRingColoring);
            else             setRandomColor(merged);
        }
        public void setRandomColor(bool merged)
        {
            List<Cluster> Clusters = (merged) ? _clustersMerged : _clusters;
#if CPU_PARALLEL
            Parallel.For(0, Clusters.Count, ClusterID =>
#else
            for (int ClusterID = 0; ClusterID < Clusters.Count; ClusterID++)
#endif
            {
                Clusters[ClusterID].sphere.setRandomColor(Clusters.Count, ClusterID);
            }
#if CPU_PARALLEL
);
#endif
        }
        public void setFixedColor(bool merged, bool twoRingColoring)
        {
            //setRandomColor(merged);
            //return;

            ColorFunctions.initColors();

            List<Cluster> Clusters = (merged) ? _clustersMerged : _clusters;

            if (Clusters.Count == 0)
                return;

            // Sort List of Clusters before seting colots to them
            {
                Clusters.Sort(delegate(Cluster c1, Cluster c2)
                {
                    return c1.area.CompareTo(c2.area);
                });
                Clusters.Reverse();
                setIDs(merged);
            }

            Parallel.For(0, Clusters.Count, ClusterID =>
            {
                Clusters[ClusterID].sphere.colorID = -1;
                Clusters[ClusterID].sphere.color = Color.LightGray;
            });

            int ClustersID = 0;
            Random Rand = new Random();

            List<Cluster> ClustersSorted = new List<Cluster>();
            ClustersSorted.Add(Clusters[0]);

            // BFS
            while (ClustersSorted.Count < Clusters.Count)
            {
                if (ClustersID == ClustersSorted.Count)
                {
                    bool exit = false;
                    foreach (Cluster ClusterC in Clusters)
                        if (!ClustersSorted.Contains(ClusterC))
                        {
                            ClustersSorted.Add(ClusterC);
                            exit = true;
                            break;
                        }

                    if (exit)
                        continue;
                }
                else
                {
                    Cluster ClusterC = ClustersSorted[ClustersID++];
                    foreach (Cluster ClusterN in ClusterC.neighbors)
                        if (!ClustersSorted.Contains(ClusterN))
                            ClustersSorted.Add(ClusterN);
                }
            }

            List<int> ColorIDs = new List<int>();
            int ColorIDsMaxCount = ColorFunctions.getColorsCount();

            foreach (Cluster ClusterC in ClustersSorted)
            {
                List<int> NeighborColorIDs = new List<int>();

                // !! {1-Ring Neighbors} !!
                foreach (Cluster ClusterN in ClusterC.neighbors)
                    if (ClusterN.sphere.colorID != -1)
                    {
                        NeighborColorIDs.Add(ClusterN.sphere.colorID);

                        // Complement Color
                        if (ClusterN.sphere.colorID + 3 < ColorIDsMaxCount)
                            NeighborColorIDs.Add(ClusterN.sphere.colorID + 3);
                        else if (ClusterN.sphere.colorID - 3 > 0)
                            NeighborColorIDs.Add(ClusterN.sphere.colorID - 3);

                        if (ClusterN.sphere.colorID == 0)       // Color == Yellow
                            NeighborColorIDs.Add(10);           // Color == Olive
                        else if (ClusterN.sphere.colorID == 1)  // Color == Green
                            NeighborColorIDs.Add(11);           // Color == ?
                        else if (ClusterN.sphere.colorID > 1 && ClusterN.sphere.colorID < 6)
                            NeighborColorIDs.Add(ClusterN.sphere.colorID + 4);

                        // !! {2-Ring Neighbors} !!
                        if (twoRingColoring)
                            foreach (Cluster ClusterN_N in ClusterN.neighbors)
                                if (ClusterN_N.sphere.colorID != -1)
                                    NeighborColorIDs.Add(ClusterN_N.sphere.colorID);
                    }

                for (int ColorID = 0; ColorID < ColorIDsMaxCount; ColorID++)
                    if (!NeighborColorIDs.Contains(ColorID) && !ColorIDs.Contains(ColorID))
                    {
                        ColorIDs.Add(ColorID);
                        if (ColorIDs.Count == ColorIDsMaxCount)
                            ColorIDs.Clear();

                        ClusterC.sphere.setFixedColor(ColorID);
                        break;
                    }

                if (ClusterC.sphere.colorID == -1)
                {
                    for (int ColorID = 0; ColorID < ColorIDsMaxCount; ColorID++)
                        if (!NeighborColorIDs.Contains(ColorID))
                        {
                            ClusterC.sphere.setFixedColor(ColorID);
                            break;
                        }
                }
            }
        }
        #endregion

        #region Compute Weights
        public void computeRigidWeights(Mesh3D pose, bool merged, out Vector4[] bones, out Vector4[] weights)
        {
            bones = new Vector4[pose.verticesCount];
            weights = new Vector4[pose.verticesCount];

            List<Cluster> Clusters = (merged) ? _clustersMerged : _clusters;
            for (int VertexID = 0; VertexID < pose.verticesCount; ++VertexID)
            {
                int[] bData = new int[4] { pose.clustersVerticesData[VertexID].id, 0, 0, 0 };
                float[] wData = new float[4] { 1, 0, 0, 0 };

                OpenTK_To_MathNET.ArrayToVector4(bData, out bones[VertexID]);
                OpenTK_To_MathNET.ArrayToVector4(wData, out weights[VertexID]);
            }
        }
        public virtual  void computeLinearWeights(Mesh3DAnimationSequence mas, Mesh3D pose, bool merged, out Vector4[] bones, out Vector4[] weights)
        {
            List<Cluster> Clusters = (merged) ? _clustersMerged : _clusters;

            int BoneID;
            int[] bData;
            float[] wData;
            double w, distance, clusterRange;

            bones = new Vector4[pose.verticesCount];
            weights = new Vector4[pose.verticesCount];

            //Get maximum distance
            Parallel.ForEach(Clusters, ClusterR =>
            {
                ClusterR.computeMax(mas);
            });

            for (int VertexID = 0; VertexID < pose.verticesCount; ++VertexID)
            {
                SortedDictionary<double, List<KeyValuePair<int, double>>> Distance_Bone_Weight = new SortedDictionary<double, List<KeyValuePair<int, double>>>();
                foreach (Cluster ClusterC in Clusters)
                {/*
                    bool exit = true;
                    if (ClusterC.id != pose.clustersVerticesData[VertexID].id)
                        foreach (Cluster ClusterN in ClusterC.neighbors)
                        {
                            if (ClusterN.id == pose.clustersVerticesData[VertexID].id)
                                exit = false;
                        }
                    else
                        exit = false;

                    if (exit)
                        continue;*/

                    //We check to see if the Vertex is within range of it, according to the Area Factor
                    //First we compute the eucledian distance from the cluster head.
                    distance = (mas.clusteringPerPose) ? ClusterC.computeErrorFromVertexP2P(mas, pose, VertexID) : ClusterC.computeErrorFromVertex(mas, pose, VertexID);
                    //distance = ClusterC.computeErrorFromHead(mas, pose, VertexID);

                    //Then we compare this distance
                    clusterRange = _pFactor * ClusterC.maxDistance;
                    if (ClusterC.maxDistance > pose.clustersVerticesData[VertexID].maxDistance)
                        clusterRange /= ClusterC.maxDistance/pose.clustersVerticesData[VertexID].maxDistance;

                    //if it isn't then proceed to the next cluster
                    //if (distance <= clusterRange)
                    {
                        if (clusterRange > 0.0)
                        {
                            if (distance <= clusterRange)
                                w = 1.0f - (distance / clusterRange);
                            else
                                w = 0.000001;    // for extra bone IDs

                            if (w == 0.0f)
                                w = 0.000001;
                        }
                        else
                            w = 1.0f;

                        List<KeyValuePair<int, double>> result = null;
                        Distance_Bone_Weight.TryGetValue(distance, out result);
                        if (result == null)
                        {
                            List<KeyValuePair<int, double>> tList = new List<KeyValuePair<int, double>>();
                            tList.Add(new KeyValuePair<int, double>(ClusterC.id, w));
                            Distance_Bone_Weight.Add(distance, tList);
                        }
                        else
                            Distance_Bone_Weight[distance].Add(new KeyValuePair<int, double>(ClusterC.id, w));
                    }
                }

                bData = new int[4] { 0, 0, 0, 0 };
                wData = new float[4] { 0, 0, 0, 0 };

                BoneID = 0;
                foreach (List<KeyValuePair<int, double>> List in Distance_Bone_Weight.Values)
                {
                    foreach (KeyValuePair<int, double> values in List)
                    {
                        bData[BoneID] = values.Key;
                        wData[BoneID] = (float)values.Value;
                        if (++BoneID == 4)
                            break;
                    }
                    if (BoneID == 4)
                        break;
                }

                OpenTK_To_MathNET.ArrayToVector4(bData, out bones[VertexID]);
                OpenTK_To_MathNET.ArrayToVector4(wData, out weights[VertexID]);
            }
        }
        
        #endregion

        #region Combine Clusterings
        public  void  combineClusterings(Mesh3DAnimationSequence mas, Mesh3D prevPose, Mesh3D thisPose)
        {
            _clustersMerged = new List<Cluster>();
            if (prevPose.poseID == mas.selectedRestPose)
            {
                _clustersMerged.AddRange(thisPose.clusteringMethod.clusters);
                computeNeighborhood(thisPose, true);
            }
            else
            {
                List<Cluster> ClustersP = (mas.clusteringIncremental) ? prevPose.clusteringMethod.clustersMerged : prevPose.clusteringMethod.clusters;
                List<Cluster> ClustersT = thisPose.clusteringMethod.clusters;

                // 1. Set Cluster ID for each Vertex ~(alleviate problems when recomputationing merging)
                foreach (Cluster ClusterT in ClustersT)
                    Parallel.ForEach(ClusterT.elementsV, ElementID => { thisPose.clustersVerticesData[ElementID] = ClusterT; });

                // 2. Split 'Previous' Clusters to 'New' Clusters using 'This' Clusters
                foreach (Cluster ClusterP in ClustersP)
                {
                    List<int>       ClusterMergedIDs = new List<int>();
                    List<Cluster>   ClustersMerged   = new List<Cluster>();

                    // 2.1 Compute Merging
                    foreach (int ElementID in ClusterP.elementsV)
                    {
                        int ClusterID = ClusterMergedIDs.IndexOf(thisPose.clustersVerticesData[ElementID].id);

                        if (ClusterID != -1)
                        {
                            ClustersMerged[ClusterID].addElementV(ElementID);
                            thisPose.clustersVerticesData[ElementID] = ClustersMerged[ClusterID];
                        }
                        else
                        {
                            Cluster NewCluster = new Simple_Cluster(thisPose, ElementID, ClustersMerged.Count + _clustersMerged.Count, 1, mas.dgMode);

                            NewCluster.clusterThis = thisPose.clustersVerticesData[ElementID];
                            NewCluster.clusterPrev = ClusterP;
                            NewCluster.clusterPrevIDs.Add(ClusterP.id);

                            NewCluster.sphere.color   = thisPose.clustersVerticesData[ElementID].sphere.color;
                            NewCluster.sphere.colorID = thisPose.clustersVerticesData[ElementID].sphere.colorID;
                            
                            ClustersMerged.Add(NewCluster);
                            ClusterMergedIDs.Add(thisPose.clustersVerticesData[ElementID].id);

                            thisPose.clustersVerticesData[ElementID] = NewCluster;
                        }
                    }
                    
                    // 2.2 Check if has disjoint components
                    Parallel.For(0, ClustersMerged.Count, ClusterID =>
                    {
                        if (ClustersMerged[ClusterID].elementsV.Count < ClustersT[ClusterMergedIDs[ClusterID]].elementsV.Count)
                            ClustersMerged[ClusterID].toBeChecked = true;
                    });

                    // 2.3 Add new clusters to List
                    _clustersMerged.AddRange(ClustersMerged);
                }
                // 3. Compute Cluster Neighbors for each Cluster
                Parallel.ForEach(_clustersMerged, ClusterC => { ClusterC.computeNeighborhood(thisPose); });
                // 4. Set Cluster IDs
                setIDs(true);
            }
        }
        #endregion

        #region Compute Over-Segmentation
        private void computeCAVs(Mesh3DAnimationSequence mas)
        {
            _CAVs = new int[mas.verticesCount][];
            Parallel.For(0, mas.verticesCount, VertexID =>
            {
                _CAVs[VertexID] = new int[mas.poses.Count];
                foreach (Mesh3D Pose in mas.poses)
                    _CAVs[VertexID][Pose.poseID] = (Pose.poseID == mas.selectedRestPose) ? 0 : Pose.clustersVerticesData[VertexID].id;
            });
        }
        public  void computeOverSegmentation(Mesh3DAnimationSequence mas, Mesh3D thisPose)
        {
            // Compute CA vectors
            computeCAVs(mas);

            // Compute Over-segmentation 
            _clusters = new List<Cluster>();
            _queueClusters = null;

            List<int[]> ClusterCAVs = new List<int[]>();
            for (int VertexID = 0; VertexID < mas.verticesCount; VertexID++)
            {
                int ClusterID = -1;
                foreach (Cluster ClusterC in _clusters)
                {
                    if (_CAVs[VertexID].SequenceEqual(ClusterC.CAV))
                    {
                        ClusterID = ClusterC.id;
                        break;
                    }
                }
                if (ClusterID != -1)
                {
                    _clusters[ClusterID].addElementV(VertexID);
                    thisPose.clustersVerticesData[VertexID] = _clusters[ClusterID];
                }
                else
                {
                    Cluster NewCluster = new Simple_Cluster(thisPose, VertexID, _clusters.Count, mas.poses.Count, mas.dgMode);

                    NewCluster.toBeChecked = true;
                    NewCluster.CAV = _CAVs[VertexID];
                    ClusterCAVs.Add(_CAVs[VertexID]);

                    _clusters.Add(NewCluster);

                    thisPose.clustersVerticesData[VertexID] = NewCluster;
                }
            }
            // 3. Compute Cluster Neighbors for each Cluster
            Parallel.ForEach(_clusters, ClusterC =>
            {
                ClusterC.computeNeighborhood(thisPose);
            });
            // 4. Set Cluster IDs
            setIDs(false);
        }
        #endregion 

        #region Perform h-Cleaning
        public void computeConnectedComponents(Mesh3DAnimationSequence mas, Mesh3D pose, bool merged)
        {
            List<Cluster> Clusters = (merged) ? _clustersMerged : _clusters;

            // 1. Compute Connected Components per Cluster
            List<Cluster>[] ClustersNew = new List<Cluster>[Clusters.Count];

            Parallel.For(0, Clusters.Count, ClusterID =>
            {
                // 1.1 check if is h-cleanable
                //if (pose.poseID - 1 != mas.selectedRestPose && merged)
                  //  Clusters[ClusterID].is_hCleanable(Clusters[ClusterID].clusterPrev.area, Clusters[ClusterID].clusterThis.area, _h_cleaning);
                
                // 1.2 if not then compute its connected components
                if (Clusters[ClusterID].toBeChecked)
                    //&& (!Clusters[ClusterID].toBeCleaned || computeAll))
                    ClustersNew[ClusterID] = Clusters[ClusterID].computeConnectedComponents(mas, pose, merged, _h_cleaning);
                else
                    ClustersNew[ClusterID] = new List<Cluster>();
            });

            // 2. Pack New Clusters
            List<Cluster> ClustersNewTotal = new List<Cluster>();
            for (int ClusterID = 0; ClusterID < Clusters.Count; ClusterID++)
                ClustersNewTotal.AddRange(ClustersNew[ClusterID]);

            // 3. Pack New Clusters
            if (ClustersNewTotal.Count > 0)
            {
                Clusters.AddRange(ClustersNewTotal);
                setIDs(merged);

                // Find Neighbors of New Clusters
                List<Cluster> ClustersNeighborhoodChange = new List<Cluster>();
                ClustersNeighborhoodChange.AddRange(ClustersNewTotal);

                int CountPrev = Clusters.Count - ClustersNewTotal.Count;
                bool[] Used = new bool[Clusters.Count];
                Parallel.For(0, Clusters.Count, ClusterID => { Used[ClusterID] = (ClusterID < CountPrev) ? false : true; });
                
                for (int ClusterID = 0; ClusterID < CountPrev; ClusterID++)
                    if (ClustersNew[ClusterID].Count > 0)
                    {
                        if (!Used[Clusters[ClusterID].id])
                        {
                            Used[Clusters[ClusterID].id] = true;
                            ClustersNeighborhoodChange.Add(Clusters[ClusterID]);
                        }

                        foreach (Cluster ClusterC_N in Clusters[ClusterID].neighbors)
                            if (!Used[ClusterC_N.id])
                            {
                                Used[ClusterC_N.id] = true;
                                ClustersNeighborhoodChange.Add(ClusterC_N);
                            }
                    }

                Parallel.ForEach(ClustersNeighborhoodChange, ClusterM => { ClusterM.computeNeighborhood(pose); });
            }
        }
        public void performCleaning(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            List<Cluster> ClustersToBeCleaned = new List<Cluster>();
            foreach (Cluster ClusterC in _clustersMerged)
            {
                if (pose.poseID - 1 != mas.selectedRestPose)
                    ClusterC.is_hCleanable(ClusterC.clusterPrev.area, ClusterC.clusterThis.area, _h_cleaning);

                if (ClusterC.toBeCleaned)
                    ClustersToBeCleaned.Add(ClusterC);
            }
            // 1. Sort Cleanable Clusters
            ClustersToBeCleaned.Sort(delegate(Cluster c1, Cluster c2)
            {
                return c1.elementsV.Count.CompareTo(c2.elementsV.Count);
            });

            // 2. Remove Cleanable Clusters
            foreach (Cluster ClusterC in ClustersToBeCleaned)
            {
                // 2.1 Clean Cluster
                ClusterC.clean(pose);
                // 2.2 Delete Cluster
                _clustersMerged.Remove(ClusterC);
            }

            // 3. Set IDs
            setIDs(true);
        }
        public void performCleaningOverSegmentation(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            Parallel.ForEach(_clusters, ClusterC =>
            {
                if (ClusterC.area == 0.0)
                    ClusterC.area = pose.averageAreasFacet * ClusterC.elementsV.Count;
            });
            
            if (mas.clusteringIncremental)
            {
                _used = new bool[_clusters.Count];
                Parallel.For(0, _clusters.Count, ClusterID => { _used[ClusterID] = false; });

                int[] ClustersCount = new int[mas.poses.Count];
                Parallel.For(0, mas.poses.Count, PoseID =>    { ClustersCount[PoseID] = mas.poses[PoseID].clusteringMethod.clusters.Count; });
                int         ClustersCountMax = ClustersCount.Max();

                double[][] AreaThis = new double[mas.poses.Count][];
                for (int PoseID = 0; PoseID < mas.poses.Count; PoseID++)
                {
                    AreaThis[PoseID] = new double[ClustersCountMax];
                    Parallel.ForEach(mas.poses[PoseID].clusteringMethod.clusters, ClusterC => { AreaThis[PoseID][ClusterC.id] = ClusterC.area; });
                }

                _clusters.Sort(delegate(Cluster c1, Cluster c2)
                {
                    return c1.area.CompareTo(c2.area);
                });

                for (int PoseID = mas.poses.Count-1; PoseID >= 1; PoseID--)
                {
                    List<Cluster> ToBeDeleted = new List<Cluster>();
                   
                    foreach (Cluster ClusterA in _clusters)
                        if (!_used[ClusterA.id])
                            foreach (Cluster ClusterB in ClusterA.neighbors)
                                if (!_used[ClusterB.id] &&  ClusterA.CAV[PoseID] != ClusterB.CAV[PoseID] &&
                                    ClusterA.CAV.Take(PoseID - 1).SequenceEqual(ClusterB.CAV.Take(PoseID - 1)))
                                {
                                    // [A] -> [B]
                                    {
                                        double PrevAreaOfClusterB = 0.0;
                                        foreach (Cluster ClusterX in _clusters)
                                        {
                                            if (!_used[ClusterX.id] && ClusterB.CAV[PoseID] != ClusterX.CAV[PoseID] &&
                                                    ClusterB.CAV.Take(PoseID - 1).SequenceEqual(ClusterX.CAV.Take(PoseID - 1)))
                                                PrevAreaOfClusterB += ClusterX.area;
                                        }

                                        //double PrevAreaOfClusterB = ClusterB.area;
                                        //foreach (Cluster ClusterB_N in ClusterB.neighbors)
                                          //  if (!_used[ClusterB_N.id] && ClusterB.CAV[PoseID] != ClusterB_N.CAV[PoseID] &&
                                            //                             ClusterB.CAV.Take(PoseID - 1).SequenceEqual(ClusterB_N.CAV.Take(PoseID - 1)))
                                              //  PrevAreaOfClusterB += ClusterB_N.area;

                                        ClusterA.is_hCleanable(
                                            PrevAreaOfClusterB,
                                            AreaThis[PoseID     ][ClusterA.CAV[PoseID    ]],
                                            _h_cleaning);

                                        if (ClusterA.toBeCleaned)
                                        {
                                            _used[ClusterA.id] = true;
                                            ToBeDeleted.Add(ClusterA);

                                            // 1. Copy Neighborhood
                                            foreach (Cluster ClusterN in ClusterA.neighbors)
                                                if (!_used[ClusterN.id])
                                                {
                                                    if (ClusterN.id != ClusterB.id)
                                                    {
                                                        if (!ClusterB.neighbors.Contains(ClusterN))
                                                            ClusterB.neighbors.Add(ClusterN);
                                                        if (!ClusterN.neighbors.Contains(ClusterB))
                                                            ClusterN.neighbors.Add(ClusterB);
                                                    }
                                                    ClusterN.neighbors.Remove(ClusterA);
                                                }

                                            // 2. Copy Data

                                            // 2.1 Vertices
                                            ClusterB.addRangeElementV(ClusterA.elementsV);
                                            Parallel.ForEach(ClusterA.elementsV, ElementID =>
                                            {
                                                pose.clustersVerticesData[ElementID] = ClusterB;
                                            });

                                            // 2.2 Facets
                                            ClusterB.addRangeElementF(ClusterA.elementsF);
                                            Parallel.ForEach(ClusterA.elementsF, FacetID =>
                                            {
                                                pose.clustersFacetsData[FacetID] = ClusterB;
                                            });

                                            // 2.3 Area
                                            ClusterB.area += ClusterA.area;

                                            break;
                                        }
                                    }
                                }
                    foreach (Cluster ClusterD in ToBeDeleted)
                        _clusters.Remove(ClusterD);
                }
            }
            else
            {
                if (_queueClusters == null)
                {             
                    _queueClusters = new SortedDictionary<double, List<Cluster[,]>>();
                    List<Cluster> ClustersToBeCleaned = new List<Cluster>();

                    foreach (Cluster ClusterC in _clusters)
                        foreach (Cluster ClusterN in ClusterC.neighbors)
                            addElement2Queue(ClusterC.area / (ClusterC.area + ClusterN.area), ClusterC, ClusterN);

                    _used = new bool[_clusters.Count];
                    Parallel.For(0, _clusters.Count, ClusterID =>
                    {
                        _used[ClusterID] = false;
                    });
                }

                while (_queueClusters.Count > 0 && _clusters.Count > _count)
                {
                    KeyValuePair<double, List<Cluster[,]>> MergingPair = _queueClusters.First();

                    double MergingError = MergingPair.Key;
                    List<Cluster[,]> MergingClusters = MergingPair.Value;

                    if (MergingError <= _h_cleaning)
                        while (MergingClusters.Count > 0)
                        {
                            // 2.1 Clean Cluster

                            // Move [A] --> [B]
                            Cluster ClusterA = MergingClusters[0][0, 0];
                            Cluster ClusterB = MergingClusters[0][0, 1];
                            MergingClusters.RemoveAt(0);

                            if (!_used[ClusterA.id] && !_used[ClusterB.id])
                            {
                                _used[ClusterA.id] = true;

                                // 1. Copy Neighborhood
                                foreach (Cluster ClusterN in ClusterA.neighbors)
                                    if (!_used[ClusterN.id])
                                    {
                                        if (ClusterN.id != ClusterB.id)
                                        {
                                            if (!ClusterB.neighbors.Contains(ClusterN))
                                                ClusterB.neighbors.Add(ClusterN);
                                            if (!ClusterN.neighbors.Contains(ClusterB))
                                                ClusterN.neighbors.Add(ClusterB);
                                        }
                                        ClusterN.neighbors.Remove(ClusterA);
                                    }

                                // 2. Copy Data

                                // 2.1 Vertices
                                ClusterB.addRangeElementV(ClusterA.elementsV);
                                Parallel.ForEach(ClusterA.elementsV, ElementID =>
                                {
                                    pose.clustersVerticesData[ElementID] = ClusterB;
                                });

                                // 2.2 Facets
                                ClusterB.addRangeElementF(ClusterA.elementsF);
                                Parallel.ForEach(ClusterA.elementsF, FacetID =>
                                {
                                    pose.clustersFacetsData[FacetID] = ClusterB;
                                });

                                // 2.3 Area
                                ClusterB.area += ClusterA.area;

                                // 3. Recompute Pairs
                                double AreaPrev = 0.0;
                                foreach (Cluster ClusterN in ClusterB.neighbors)
                                    if (!_used[ClusterN.id])
                                    {
                                        AreaPrev = ClusterN.area + ClusterB.area;
                                        // [N] -> [B]
                                        addElement2Queue(ClusterN.area / AreaPrev, ClusterN, ClusterB);
                                        // [B] -> [N]
                                        addElement2Queue(ClusterB.area / AreaPrev, ClusterB, ClusterN);
                                    }
                                // 4. Delete cluster
                                _clusters.Remove(ClusterA);
                            }
                        }
                    else
                        break;
                    _queueClusters.Remove(MergingError);
                }
            }


            // 2. Set IDs
            setIDs(false);
        }
        public void mergeErrorClusters(Mesh3DAnimationSequence mas, Mesh3D pose, bool merged)
        {
            List<Cluster> Clusters = (merged) ? _clustersMerged : _clusters;
            bool[] ClusterToBeRemoved = new bool[Clusters.Count];

            // Compute smallest cluster error
            double ErrorMinCluster = Double.MaxValue;
            foreach (Cluster ClusterC in Clusters)
                if (ClusterC.error < ErrorMinCluster)
                    ErrorMinCluster = ClusterC.error;
            ErrorMinCluster /= 2.0;

            // Check For Merging
            Cluster ClusterA, ClusterB;
            for (int ClusterID = 0; ClusterID < Clusters.Count; ++ClusterID) // B Cluster
            {
                ClusterB = Clusters[ClusterID];
                if (ClusterToBeRemoved[ClusterB.id])
                    continue;

                for (int NeighborID = 0; NeighborID < ClusterB.neighbors.Count; ++NeighborID) // A Cluster
                {
                    ClusterA = ClusterB.neighbors[NeighborID];
                    if (ClusterToBeRemoved[ClusterA.id])
                        continue;

                    // Error A-->B
                    double ErrorClustersAB_Old = ClusterA.error;
                    double ErrorClustersAB_New = 0.0;
                    foreach (int ElementID in ClusterA.elementsV)
                        ErrorClustersAB_New +=
                            mas.clusteringPerPose ? ClusterB.computeError(mas, pose, ElementID) :
                            mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING ?
                            ClusterB.computeErrorFromCentroid(_similarityMatrix, ElementID) : ClusterB.computeError(mas, ElementID);

                    if (ErrorClustersAB_New - ErrorClustersAB_Old < ErrorMinCluster)
                    {
                        // 3.1 Copy 'A' Cluster Elements to 'B' Cluster
                        ClusterB.addRangeElementV(ClusterA.elementsV);
#if CPU_PARALLEL
                        Parallel.ForEach(ClusterA.elementsV, ElementID =>
#else
                        foreach (int ElementID in ClusterA.elementsV)
#endif
                        {
                            pose.clustersVerticesData[ElementID] = ClusterB;
                        }
#if CPU_PARALLEL
);
#endif
                        // 3.2 Re-compute Center of 'B' Cluster
                        ClusterB.computeCenter(mas, _scalingFactor, _similarityMatrix);
                        // 3.3 Copy 'A' Cluster Neighbors to 'B' Cluster and Reverse
                        foreach (Cluster ClusterN in ClusterA.neighbors)
                        {
                            if (ClusterN.id != ClusterB.id)
                            {
                                // 3.3.1.1 Add 'B' to 'A's Neighbor
                                ClusterN.neighbors.Remove(ClusterA);
                                ClusterN.neighbors.Add(ClusterB);
                                // 3.3.2.1 Add 'A's Neighbor to 'B'
                                if (!ClusterB.neighbors.Contains(ClusterN))
                                    ClusterB.neighbors.Add(ClusterN);
                            }
                        }
                        // 3.4 Compute Error
                        _errorTotal -= ClusterA.error;
                        _errorTotal -= ClusterB.error;
                        ClusterB.error = ErrorClustersAB_New;
                        _errorTotal += ClusterB.error;

                        ClusterToBeRemoved[ClusterA.id] = true;
                    }
                }
            }

            List<Cluster> ClustersNew = new List<Cluster>();
            for (int ClusterID = 0; ClusterID < Clusters.Count; ++ClusterID)
                if (!ClusterToBeRemoved[ClusterID])
                    ClustersNew.Add(Clusters[ClusterID]);

            _clusters = new List<Cluster>();
            _clusters.AddRange(ClustersNew);

            setIDs(merged);
        }
        #endregion

        #region Smooth Boundaries
        public void smoothBoundaries(Mesh3D pose, bool merged)
        {
            List<Cluster> Clusters = (merged) ? _clustersMerged : _clusters;

            bool[,] Smoothed = new bool[Clusters.Count, Clusters.Count];

            foreach (Cluster ClusterC in Clusters)
                foreach (Cluster ClusterN in ClusterC.neighbors)
                {
                    if (Smoothed[ClusterC.id,ClusterN.id] || Smoothed[ClusterN.id, ClusterC.id])
                        continue;

                    Smoothed[ClusterC.id, ClusterN.id] = true;
                    Smoothed[ClusterN.id, ClusterC.id] = true;

                    ClusterC.smoothBoundaries(pose, ClusterN);
                }
        }
        #endregion

        #region Compute Rest Pose Clustering
        public void computeRestPose(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            //Add all the Vertices to the first cluster
            Cluster NewCluster = new Simple_Cluster(pose, 0, 0, 1, mas.dgMode);
            NewCluster.addRangeElementV(Enumerable.Range(1, pose.verticesCount-1).ToList());

            _clusters = new List<Cluster>();
            _clusters.Add(NewCluster);

            _clustersMerged = new List<Cluster>();
            _clustersMerged.Add(NewCluster);

            // 6.1 Keep the final tolerance
            _errorTotal     = 0.0;
            _errorTolerance = 0.0;
            // 6.2 Compute Centers
            foreach (Cluster ClusterC in _clusters)
            {
                ClusterC.computeCentersPos(mas);
                ClusterC.sphere.setBuffers();
            }
        }
        #endregion

        #region Load/Save Functions
        public bool load(Mesh3DAnimationSequence mas, Mesh3D pose, String FilePath)
        {
            try
            {
                using (StreamReader SReader = new StreamReader(FilePath, Encoding.Default))
                {
                    while (!SReader.EndOfStream)
                    {
                        string Line = SReader.ReadLine();
                        string[] currentLine = Line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (currentLine.Length == 0) continue;

                        switch (currentLine[0][0])
                        {
                            case '*':
                                string[] ClusteringNameCount = new string[2];

                                currentLine[0] = currentLine[0].Remove(0, 1);
                                ClusteringNameCount = currentLine[0].Split('=');
                                
                                switch (ClusteringNameCount[0])
                                {
                                    case "CLUSTERS":
                                        _clusters = new List<Cluster>();

                                        int ClustersCount = int.Parse(ClusteringNameCount[1]); 
                                        for (int ClusterID = 0; ClusterID < ClustersCount; ClusterID++)
                                        {
                                            string[] ClusterLine = SReader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                            int ElementsCount   = int.Parse(ClusterLine[0]);
                                            int HeadID          = int.Parse(ClusterLine[1]);

                                            Cluster NewCluster = new Simple_Cluster(pose, HeadID, ClusterID, mas.clusteringPerPose ? 1 : mas.poses.Count, mas.dgMode);
                                            for (int ElementID = 2; ElementID < ElementsCount+1; ++ElementID)
                                                NewCluster.addElementV(int.Parse(ClusterLine[ElementID]));

                                            _clusters.Add(NewCluster);
                                        }
                                        
                                        _count = ClustersCount;
                                        break;

                                    case "CLUSTERS_MERGED":
                                        _clustersMerged = new List<Cluster>();

                                        ClustersCount = int.Parse(ClusteringNameCount[1]);

                                        for (int ClusterID = 0; ClusterID < ClustersCount; ClusterID++)
                                        {
                                            string[] ClusterLine = SReader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                            int ElementsCount   = int.Parse(ClusterLine[0]);
                                            int HeadID          = int.Parse(ClusterLine[1]);

                                            Cluster NewCluster = new Simple_Cluster(pose, HeadID, ClusterID, mas.clusteringPerPose ? 1 : mas.poses.Count, mas.dgMode);
                                            for (int ElementID = 2; ElementID < ElementsCount+1; ++ElementID)
                                                NewCluster.addElementV(int.Parse(ClusterLine[ElementID]));

                                            _clustersMerged.Add(NewCluster);
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        public bool save(string name, string numOfPoses, string ClusteringMode)
        {
            try
            {
                int ClustersCount       = _clusters.Count;
                int ClustersMergedCount = _clustersMerged.Count;

                StreamWriter FileClustering = new System.IO.StreamWriter(@Properties.Settings.Default.DesktopPath + name + "(" + numOfPoses + ")^" + ClusteringMode + "{" + ClustersCount.ToString() + "," + ClustersMergedCount.ToString() + "}.clu");
                {
                    if (ClustersCount > 0)
                    {
                        FileClustering.WriteLine("*CLUSTERS=" + ClustersCount.ToString());
                        foreach (Cluster ClusterC in _clusters)
                        {
                            FileClustering.Write(ClusterC.elementsV.Count.ToString() + " ");
                            foreach (int ElementID in ClusterC.elementsV)
                                FileClustering.Write(ElementID.ToString() + " ");
                            FileClustering.Write("\n");
                        }
                        FileClustering.Write("\n");
                    }

                    if (ClustersMergedCount > 0)
                    {
                        FileClustering.WriteLine("*CLUSTERS_MERGED=" + ClustersMergedCount.ToString());
                        foreach (Cluster ClusterC in _clustersMerged)
                        {
                            FileClustering.Write(ClusterC.elementsV.Count.ToString() + " ");
                            foreach (int ElementID in ClusterC.elementsV)
                                FileClustering.Write(ElementID.ToString() + " ");
                            FileClustering.Write("\n");
                        }
                        FileClustering.Write("\n");
                    }
                }
                FileClustering.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Draw Function
        public void draw(Mesh3DAnimationSequence mas, ref Shading rendering)
        {
            // Draw Cluster Regions
            if (_drawRegions)
            {
                GL.LineWidth(Example._scene.lineSize);

                Example._scene.updateShaders(ref rendering);
                rendering.use();
                {
                    mas.drawModels(ref rendering, false, false);
                }
                Shading.close();
                GL.LineWidth(1.0f);
            }

            // Draw Cluster Centers
            if (_drawSpheres && mas.sma != null)
            {
                int SelectedPoseID = (mas.sma.selectedPose == -1) ?
                    mas.clusteringPerPose ? 0 : mas.selectedPose
                    : 
                    mas.sma.selectedPose;
#if CPU_PARALLEL
                Parallel.For(0, _clusters.Count, ClusterID =>
#else
                for (int ClusterID = 0; ClusterID < _clusters.Count; ClusterID++)
#endif
                {
                    _clusters[ClusterID].sphere.transformation_matrix = Matrix4.Transpose(mas.clusteringPerPose ? Matrix4.Identity : _clusters[ClusterID].transformationMatrices[SelectedPoseID]);
                    /*
                    if(mas.sma.initWeights)
                    {
                        if (mas.sma.fittingMode == Modes.Fitting.RP || mas.sma.fittingMode == Modes.Fitting.P2P_APP_APP_RPF)
                        {
                            //if (mas.editedPose != null && SelectedPoseID == 0)
                            _clusters[ClusterID].sphere.transformation_matrix = Matrix4.Transpose(mas.sma.matricesPoseBoneData[SelectedPoseID][ClusterID] * _clusters[ClusterID].transformationMatrices[mas.selectedRestPose]);
                            //else
                              //  _clusters[ClusterID].sphere.transformation_matrix = Matrix4.Transpose(

                                //Matrix4.Invert(_clusters[ClusterID].transformationMatrices[0]) *
                                //mas.sma.matricesPoseBoneData[SelectedPoseID][ClusterID] *
                                //_clusters[ClusterID].transformationMatrices[0]
                                //)
                                //;
                        }
                        else 
                        {
                            _clusters[ClusterID].sphere.transformation_matrix = Matrix4.Identity;
                            for (int PoseID = 1; PoseID <= SelectedPoseID; PoseID++)
                                _clusters[ClusterID].sphere.transformation_matrix *= Matrix4.Transpose(mas.sma.matricesPoseBoneData[PoseID][ClusterID]);
                        }
                    }
                    else
                        _clusters[ClusterID].sphere.transformation_matrix = Matrix4.Transpose(mas.clusteringPerPose ? Matrix4.Identity : _clusters[ClusterID].transformationMatrices[SelectedPoseID]);
                     */
                }
#if CPU_PARALLEL
);
#endif
                for (int ClusterID = 0; ClusterID < _clusters.Count; ClusterID++)
                    _clusters[ClusterID].draw();
            }
        }
        #endregion
    }

    public class P_Center_Clustering : Clustering
    {
        #region Constructor
        public P_Center_Clustering() : base()
        {
            
        }
        #endregion

        #region Compute Function
        public override void compute(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            int PosesCount = mas.clusteringPerPose ? 1 : mas.poses.Count;

            _clusters = new List<Cluster>();
                
            // Initial Cluster
            int      SeedCluster = _randNum.Next(pose.verticesCount);
            P_Center_Cluster NewCluster = new P_Center_Cluster(pose, SeedCluster, _clusters.Count, PosesCount, mas.dgMode);

            //Add all the Vertices to the first cluster
            for (int VertexID = 0; VertexID < pose.verticesCount; ++VertexID)
                if (VertexID != NewCluster.head)
                    NewCluster.addElement(VertexID, mas.clusteringPerPose ? NewCluster.computeErrorFromHead(mas, pose, VertexID) : NewCluster.computeErrorFromHeads(mas, VertexID));
            _clusters.Add(NewCluster);

            while (_clusters.Count != _count)
            {
                //Find the head of the new Cluster
                int    MaxClusterID   = 0;
                double MaxClusterDist = 0.0;
                for (int ClusterID = 0; ClusterID < _clusters.Count; ++ClusterID)
                {
                    //Find the cluster with the maximal distance
                    double MaxDist = ((P_Center_Cluster)_clusters[ClusterID]).getMaximalDistance();
                    if (MaxClusterDist  < MaxDist)
                    {
                        MaxClusterDist  = MaxDist;
                        MaxClusterID    = ClusterID;
                    }
                }

                //Take steps to remove the new Head from the cluster it belonged
                P_Center_Cluster RemCluster = (P_Center_Cluster)_clusters[MaxClusterID];
                //The vertex id of the node to become the head of the new cluster
                int NewHeadID = RemCluster.getList(MaxClusterDist)[0]; 

                //Pick it up and remove from the cluster
                RemCluster.removeElement(NewHeadID, MaxClusterDist);

                NewCluster = new P_Center_Cluster(pose, NewHeadID, _clusters.Count, PosesCount, mas.dgMode);

                //Now we must get all the nodes, from all the existing clusters whose distance from their head
                //is bigger than the one from the head of the new cluster. These nodes will be added to the new cluster
                foreach (P_Center_Cluster ClusterC in _clusters)
                {
                    var NodesToDelete = new ArrayList();

                    //For each distance recorded in the cluster
                    foreach (KeyValuePair<double, List<int>> Distances in ClusterC.distances)
                    {
                        //For each node that has that distance
                        for (int Node = 0; Node < Distances.Value.Count; ++Node)
                        {
                            int ElementID = Distances.Value[Node];

                            //Check if the distance of the node from the new Cluster Head is 
                            //smaller than its distance from the head of the cluster it belongs to.
                            //Compute the Euclidian distance from the new head.

                            //If the distance is smaller, the node is transferred to the new cluster
                            double Error = mas.clusteringPerPose ? NewCluster.computeErrorFromHead(mas, pose, ElementID) : NewCluster.computeErrorFromHeads(mas, ElementID);
                            if (Error < Distances.Key)
                            {
                                NewCluster.addElement(ElementID, Error);
                                NodesToDelete.Add(new KeyValuePair<double, int>(Distances.Key, ElementID));
                            }
                        } 
                    }

                    foreach (KeyValuePair<double, int> RemPair in NodesToDelete)
                        ClusterC.removeElement(RemPair.Value, RemPair.Key);
                } 

                //Add the cluster to the list
                _clusters.Add(NewCluster);
            }

            // Compute Final Center Positions
            foreach (Cluster ClusterC in _clusters)
            {
                ClusterC.computeCentersPos(mas);
                ClusterC.sphere.setBuffers();
            }
        }
        #endregion
        /*
        #region Compute Skinning Weights Function
        public override void computeLinearWeights(Mesh3DAnimationSequence mas, Mesh3D pose, bool merged, out Vector4[] bones, out Vector4[] weights)
        {
            int BoneID;
            int[] bData;
            float[] wData;
            double w, distance, clusterRange;
            P_Center_Cluster ClusterC;

            bones = new Vector4[pose.verticesCount];
            weights = new Vector4[pose.verticesCount];

            for (int VertexID = 0; VertexID < pose.verticesCount; ++VertexID)
            {
                SortedDictionary<double, List<KeyValuePair<int, double>>> Distance_Bone_Weight = new SortedDictionary<double, List<KeyValuePair<int, double>>>();
                for (int ClusterID = 0; ClusterID < _count; ClusterID++)
                {
                    ClusterC = (P_Center_Cluster)_clusters[ClusterID];
                    //We check to see if the Vertex is within range of it, according to the Area Factor
                    //First we compute the eucledian distance from the cluster head.
                    distance = ClusterC.computeErrorFromHead(mas, pose, VertexID);
                    //Then we compare this distance
                    clusterRange = _pFactor * ClusterC.getMaximalDistance();
                    //if it isn't then proceed to the next cluster
                    if (distance <= clusterRange)
                    {
                        w = 1.0f - (distance / clusterRange);
                        if (w == 0.0f) w = double.Epsilon;

                        List<KeyValuePair<int, double>> result = null;
                        Distance_Bone_Weight.TryGetValue(distance, out result);
                        if (result == null)
                        {
                            List<KeyValuePair<int, double>> tList = new List<KeyValuePair<int, double>>();
                            tList.Add(new KeyValuePair<int, double>(ClusterID, w));
                            Distance_Bone_Weight.Add(distance, tList);
                        }
                        else
                            Distance_Bone_Weight[distance].Add(new KeyValuePair<int, double>(ClusterID, w));
                    }
                }

                bData = new int[4] { 0, 0, 0, 0 };
                wData = new float[4] { 0, 0, 0, 0 };

                BoneID = 0;
                foreach (List<KeyValuePair<int, double>> List in Distance_Bone_Weight.Values)
                {
                    foreach (KeyValuePair<int, double> values in List)
                    {
                        bData[BoneID] = values.Key;
                        wData[BoneID] = (float)values.Value;
                        if (++BoneID == 4)
                            break;
                    }
                    if (BoneID == 4)
                        break;
                }

                OpenTK_To_MathNET.ArrayToVector4(bData, out bones[VertexID]);
                OpenTK_To_MathNET.ArrayToVector4(wData, out weights[VertexID]);
            }
        }
        #endregion
        */
        #region Empty Functions
        public override void initialSeeding(Mesh3DAnimationSequence mas, Mesh3D pose) { ;}
        #endregion
    }

    public class K_Means_Clustering : Clustering
    {
        #region Constructor
        public K_Means_Clustering() : base() { ;}
        #endregion

        #region Seeding Function
        public override void initialSeeding(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            int PosesCount = mas.clusteringPerPose ? 1 : mas.poses.Count;

            // 0.1 Initial Clusters
            _clusters = new List<Cluster>();

            // Random Seeding
            if (_randomSeeding)
            {
                for (int ClusterID = 0; ClusterID < _count; ++ClusterID)
                {
                    int SeedCluster = _randNum.Next(pose.verticesCount);
                    Cluster NewCluster = new Simple_Cluster(pose, SeedCluster, _clusters.Count, PosesCount, mas.dgMode);
                    _clusters.Add(NewCluster);
                }
            }
            // P2P/P-Center Seeding
            else
            {
                if (mas.clusteringPerPose)
                {
                    List<Cluster> Clusters;
                    if (pose.poseID - 1 == mas.selectedRestPose)
                    {
                        pose.pCenter.compute(mas, pose);
                        Clusters = pose.pCenter.clusters;
                    }
                    else
                    {
                        Clusters = mas.poses[pose.poseID - 1].kMeans.clusters;
                        foreach (Cluster ClusterC in Clusters)
                            ClusterC.computeHead(mas);
                    }

                    foreach (Cluster ClusterC in Clusters)
                    {
                        Cluster NewCluster = new Simple_Cluster(pose, ClusterC.head, _clusters.Count, PosesCount, mas.dgMode);
                        _clusters.Add(NewCluster);
                    }
                }
                else
                {
                    mas.pCenter.compute(mas, pose);
                    foreach (P_Center_Cluster ClusterC in mas.pCenter.clusters)
                    {
                        Cluster NewCluster = new Simple_Cluster(pose, ClusterC.head, _clusters.Count, PosesCount, mas.dgMode);
                        _clusters.Add(NewCluster);
                    }
                }
            }
        }
        #endregion

        #region Compute Function
        public override void compute(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            // 0.1 Init Seeding
            initialSeeding(mas, pose);
            // 0.2 Init Cluster Centers
            computeCenters(mas);

            _iterations = 0;
            _errorTotal = double.MaxValue;
            double NewErrorTotal     = double.MaxValue;
            double NewErrorTolerance = 0.0;

            do
            {
                // 1. Clear Elements
                _errorTotal = NewErrorTotal;
                foreach (Cluster ClusterC in _clusters)
                    ClusterC.elementsV.Clear();

                // 2. Add Vertices to the Closest Cluster
                int[] MinIDs = new int[pose.verticesCount];
#if CPU_PARALLEL
                Parallel.For(0, pose.verticesCount, VertexID =>
#else
                for (int VertexID = 0; VertexID < pose.verticesCount; ++VertexID)
#endif
                {
                    int     MinID   = -1;
                    double  MinError = double.MaxValue;
                    for (int ClusterID = 0; ClusterID < _clusters.Count; ++ClusterID)
                    {
                        double Error =
                                mas.clusteringPerPose ? _clusters[ClusterID].computeError(mas, pose, VertexID) :
                                mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING ? Math.Pow(_clusters[ClusterID].computeErrorFromCentroid(_similarityMatrix, VertexID), 2)
                                                                                                  : _clusters[ClusterID].computeError(mas, VertexID);
                        if (MinError > Error)
                        {
                            MinID = ClusterID;
                            MinError = Error;
                        }
                    }
                    MinIDs[VertexID] = MinID;
                }
#if CPU_PARALLEL
);
#endif
                for (int VertexID = 0; VertexID < pose.verticesCount; ++VertexID)
                    _clusters[MinIDs[VertexID]].addElementV(VertexID);

                // 3. Compute Centers 
                computeCenters(mas);
                // 4. Compute Error
                NewErrorTotal = computeError(mas, pose);
                NewErrorTolerance = (_iterations == 0) ? _errorTolerance + 1 : Math.Abs(NewErrorTotal - _errorTotal);

                // 5. BackUp
                if (NewErrorTotal <= _errorTotal)
                {
                    _errorTotal = NewErrorTotal;
                    _clustersBackUp = new List<Cluster>();
                    _clustersBackUp.AddRange(_clusters);
                }
                else
                    break;

            }
            while (++_iterations < _maxIterations && NewErrorTolerance > _errorTolerance);

            // 6. Finally...

            // 6.1 Keep the best clusters
            _clusters = new List<Cluster>();
            _clusters.AddRange(_clustersBackUp);
            // 6.2 Keep the final tolerance
            _errorTolerance = NewErrorTolerance;
            // 6.3 Compute Centers
            foreach (Cluster ClusterC in _clusters)
                ClusterC.sphere.setBuffers();
        }
        #endregion
    }

    public class K_RG_Clustering : Clustering
    {
        #region Constructor
        public K_RG_Clustering() : base() { ;}
        #endregion

        #region Seeding Function
        public override void initialSeeding(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            _used = new bool[pose.verticesCount];
#if CPU_PARALLEL
            Parallel.For(0, pose.verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < pose.verticesCount; VertexID++)
#endif
            {
                _used[VertexID] = false;
            }
#if CPU_PARALLEL
);
#endif
            int SeedCluster = 0;
            List<int> PrevClusterHeadIDs = new List<int>();

            // First Iteration
            if (_iterations == 1)
            {
                // Random Seeding
                if (_randomSeeding)
                    SeedCluster = _randNum.Next(0, pose.verticesCount);
                // P2P/P-Center Seeding
                else
                {
                    if (mas.clusteringPerPose)
                    {
                        List<Cluster> Clusters;
                        if (pose.poseID - 1 == mas.selectedRestPose)
                        {
                            pose.pCenter.compute(mas, pose);
                            Clusters = pose.pCenter.clusters;
                        }
                        else
                        {
                            Clusters = mas.poses[pose.poseID - 1].kRG.clusters;
                            foreach (Cluster ClusterC in Clusters)
                                ClusterC.computeHead(mas);
                        }

                        foreach (Cluster ClusterC in Clusters)
                            PrevClusterHeadIDs.Add(ClusterC.head);
                    }
                    else
                    {
                        mas.pCenter.compute(mas, pose);
                        foreach (P_Center_Cluster ClusterC in mas.pCenter.clusters)
                            PrevClusterHeadIDs.Add(ClusterC.head);
                    }
                }
            }
            // Use previous iteration clustering heads for Seeding
            else
                foreach (Cluster ClusterC in _clusters)
                    PrevClusterHeadIDs.Add(ClusterC.head);

            int PosesCount = mas.clusteringPerPose ? 1 : mas.poses.Count;

            _clusters = new List<Cluster>();
            for (int ClusterID = 0; ClusterID < _count; ClusterID++)
            {
                if (_iterations == 1 && _randomSeeding)
                    while (_used[SeedCluster])
                        SeedCluster = _randNum.Next(0, pose.verticesCount);
                else
                    SeedCluster = PrevClusterHeadIDs[ClusterID];

                _used[SeedCluster] = true;

                Cluster NewCluster = new Simple_Cluster(pose, SeedCluster, _clusters.Count, PosesCount, mas.dgMode);
                _clusters.Add(NewCluster);
            }

            computeCenters(mas);

            for (int ClusterID = 0; ClusterID < _count; ClusterID++)
                addAdjacentVertices(mas, pose, _clusters[ClusterID].head, ClusterID);
        }
        #endregion

        #region Compute Function
        public override void compute(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            _iterations = 0;
            _errorTotal = double.MaxValue;

            double NewErrorTotal;
            double NewErrorTolerance = double.MaxValue;

            while (_iterations++ < _maxIterations && NewErrorTolerance > _errorTolerance)
            {
                _queue = new SortedDictionary<double, List<int[,]>>();

                initialSeeding(mas, pose);

                while (_queue.Count > 0)
                {
                    KeyValuePair<double, List<int[,]>> PopPair = _queue.First();

                    double       PopError    = PopPair.Key;
                    List<int[,]> PopElements = PopPair.Value;
                    while (PopElements.Count > 0)
                    {
                        int VertexID  = PopElements[0][0, 0];
                        int ClusterID = PopElements[0][0, 1];
                        PopElements.RemoveAt(0);

                        if (!_used[VertexID])
                        {
                            _used[VertexID] = true;
                            _clusters[ClusterID].addElementV(VertexID);
                            addAdjacentVertices(mas, pose, VertexID, ClusterID);
                        }
                    }
                    _queue.Remove(PopError);
                }

                // 3.1 Compute Cluster Center
                computeCenters(mas);
                // 3.2 Compute Error
                NewErrorTotal     = computeError(mas, pose);
                NewErrorTolerance = (_iterations == 1) ? _errorTolerance + 1 : Math.Abs(NewErrorTotal - _errorTotal);
                // 3.3 BackUp
                if (NewErrorTotal <= _errorTotal)
                {
                    _errorTotal = NewErrorTotal;
                    _clustersBackUp = new List<Cluster>();
                    _clustersBackUp.AddRange(_clusters);
                }
                else
                    break;
            }
            // 4.1 Keep the best clusters
            _clusters = new List<Cluster>();
            _clusters.AddRange(_clustersBackUp);
            // 4.2 Keep the final tolerance
            _errorTolerance = NewErrorTolerance;
            // 4.3 Compute Cluster Centers
            foreach (Cluster ClusterC in _clusters)
                ClusterC.sphere.setBuffers();
        }
        #endregion
    }

    public class Merge_RG_Clustering : Clustering
    {
        #region Constructor
        public Merge_RG_Clustering() : base() { ;}
        #endregion

        #region Seeding Function
        public override void initialSeeding(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            // 0.1 Init Used 
            _used                   = new bool[pose.verticesCount];
            Cluster[] ClustersNew   = new Cluster[pose.verticesCount];
#if CPU_PARALLEL
            Parallel.For(0, pose.verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < pose.verticesCount; ++VertexID)
#endif
            {
                _used[VertexID]       = false;
                ClustersNew[VertexID] = new Simple_Cluster(pose, VertexID, VertexID, mas.clusteringPerPose ? 1 : mas.poses.Count, mas.dgMode);
            }
#if CPU_PARALLEL
);
#endif
            // 0.2 Initial Clusters
            _clusters = new List<Cluster>();
            _clusters.AddRange(ClustersNew);
        }
        #endregion

        #region Compute Function
        public override void compute(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            // 0.1 Init Seeding
            initialSeeding(mas, pose);
            // 0.2 Init Cluster Centers
            computeCenters(mas);
            // 0.3 Init Cluster Neighborhood
            computeNeighborhood(pose, false);
            // 0.4 Init Error
            _errorTotal = computeError(mas, pose);
            // 0.5 Init Queue
            _queueClusters = new SortedDictionary<double, List<Cluster[,]>>();

            // 2. Find Clusters A-->B to be merged
            Cluster ClusterA, ClusterB;
            for (int ClusterID = 0; ClusterID < _clusters.Count; ++ClusterID) // B Cluster
            {
                ClusterB = _clusters[ClusterID];
                for (int NeighborID = 0; NeighborID < ClusterB.neighbors.Count; ++NeighborID) // A Cluster
                {
                    ClusterA = ClusterB.neighbors[NeighborID];

                    // Error A-->B
                    double ClustersAB_Error = 0.0;
                    foreach (int ElementID in ClusterA.elementsV)
                        ClustersAB_Error += 
                            mas.clusteringPerPose                                               ? ClusterB.computeError(mas, pose, ElementID) :
                            mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING   ? ClusterB.computeErrorFromCentroid(_similarityMatrix, ElementID) : ClusterB.computeError(mas, ElementID);
                    
                    // Add to List
                    addElement2Queue(ClustersAB_Error, ClusterA, ClusterB);
                }
            }

            int NumClustersToBeRemoved =  pose.verticesCount - _count;
            while (_queueClusters.Count > 0 && NumClustersToBeRemoved > 0 && _errorTotal < _errorTotalTolerance)
            {
                KeyValuePair<double, List<Cluster[,]>> PopPair = _queueClusters.First();
                double           PopError    = PopPair.Key;
                List<Cluster[,]> PopElements = PopPair.Value;

                while (PopElements.Count > 0 && NumClustersToBeRemoved > 0 && _errorTotal < _errorTotalTolerance)
                {
                    ClusterA = PopElements[0][0, 0];
                    ClusterB = PopElements[0][0, 1];

                    if (!_used[ClusterA.id] && !_used[ClusterB.id])
                    {
                        NumClustersToBeRemoved--;
                        _used[ClusterA.id] = true;

                        // 3.1 Copy 'A' Cluster Elements to 'B' Cluster
                        ClusterB.addRangeElementV(ClusterA.elementsV);
#if CPU_PARALLEL
                        Parallel.ForEach(ClusterA.elementsV, ElementID =>
#else
                        foreach (int ElementID in ClusterA.elementsV)
#endif
                        {
                            pose.clustersVerticesData[ElementID] = ClusterB;
                        }
#if CPU_PARALLEL
);
#endif
                        // 3.2 Re-compute Center of 'B' Cluster
                        ClusterB.computeCenter(mas, _scalingFactor, _similarityMatrix);
                        // 3.3 Copy 'A' Cluster Neighbors to 'B' Cluster and Reverse
                        foreach (Cluster ClusterN in ClusterA.neighbors)
                        {
                            if (ClusterN.id != ClusterB.id && !_used[ClusterN.id])
                            {
                                // 3.3.1.1 Add 'B' to 'A's Neighbor
                                ClusterN.neighbors.Remove(ClusterA);
                                ClusterN.neighbors.Add(ClusterB);
                                // 3.3.1.2 Error B-->N(A)
                                double ClustersAB_Error = 0.0;
                                foreach (int ElementID in ClusterB.elementsV)
                                    ClustersAB_Error += 
                                        mas.clusteringPerPose                                               ? ClusterN.computeError(mas, pose, ElementID) :
                                        mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING   ? ClusterN.computeErrorFromCentroid(_similarityMatrix, ElementID)
                                                                                                            : ClusterN.computeError(mas, ElementID);
                                addElement2Queue(ClustersAB_Error, ClusterB, ClusterN);

                                // 3.3.2.1 Add 'A's Neighbor to 'B'
                                if (!ClusterB.neighbors.Contains(ClusterN))
                                    ClusterB.neighbors.Add(ClusterN);
                                // 3.3.2.2 Error N(A)-->B
                                ClustersAB_Error = 0.0;
                                foreach (int ElementID in ClusterN.elementsV)
                                    ClustersAB_Error +=
                                        mas.clusteringPerPose                                               ? ClusterB.computeError(mas, pose, ElementID) :
                                        mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING   ? ClusterB.computeErrorFromCentroid(_similarityMatrix, ElementID)
                                                                                                            : ClusterB.computeError(mas, ElementID);
                                addElement2Queue(ClustersAB_Error, ClusterN, ClusterB);
                            }
                        }
                        // 3.4 Compute Error
                        _errorTotal -= ClusterA.error;
                        _errorTotal -= ClusterB.error;
                        _errorTotal += mas.clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING ? ClusterB.computeError(_similarityMatrix) : ClusterB.computeError(mas, pose);

                        _clusters.Remove(ClusterA);
                    }
                    PopElements.RemoveAt(0);
                }
                _queueClusters.Remove(PopError);
            }

            // 5. Finally
            foreach (Cluster ClusterC in _clusters)
            {
                ClusterC.computeCentersPos(mas);
                ClusterC.sphere.setBuffers();
            }

            setIDs(false);
            setColor(false, false, false);
        }
        #endregion
    }

    public class Divide_Conquer_Clustering : Clustering
    {
        #region Private Properties
        int _tail;
        #endregion

        #region Constructor
        public Divide_Conquer_Clustering() : base() { ;}
        #endregion

        #region Seeding Function
        public override void initialSeeding(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            double ErrorMax, ErrorTotal;

            _used = new bool[pose.verticesCount];

            int SeedCluster = _randNum.Next(0, pose.verticesCount);
            int PosesCount  = mas.clusteringPerPose ? 1 : mas.poses.Count;

            Divide_Conquer_Cluster NewCluster = new Divide_Conquer_Cluster(pose, SeedCluster, _count, _clusters.Count, PosesCount, mas.dgMode);
            NewCluster.addRangeElementV(Enumerable.Range(0, pose.verticesCount).ToList());

            NewCluster.computeCenter(mas, _scalingFactor, _similarityMatrix);
            NewCluster.findClosestElement(mas, pose, _similarityMatrix, out ErrorMax, out ErrorTotal);
            
            _tail       = NewCluster.tail;
            _errorTotal = ErrorTotal;

            _clusters = new List<Cluster>();
            _clusters.Add(NewCluster);
        }
        #endregion

        #region Compute Maximum Tail Function
        private void computeMaxTail_Error(Mesh3DAnimationSequence mas, Mesh3D pose, out int tail, out double errorTotal)
        {
            double ErrorMax = 0.0;
            double ErrorClusterMax, ErrorClusterTotal;

            tail = -1;
            errorTotal = 0.0;
            foreach (Divide_Conquer_Cluster ClusterC in _clusters)
            {
                ClusterC.findClosestElement(mas, pose, _similarityMatrix, out ErrorClusterMax, out ErrorClusterTotal);
                if (ErrorClusterMax >= ErrorMax)
                {
                    ErrorMax = ErrorClusterMax;
                    tail = ClusterC.tail;
                }
                errorTotal += ErrorClusterTotal;
            }
        }
        #endregion 

        #region Compute Function
        public override void compute(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            initialSeeding(mas, pose);

            int     NewTail=-1;
            double  NewErrorTotal;
            double  NewErrorTolerance = double.MaxValue;
            int     PosesCount = mas.clusteringPerPose ? 1 : mas.poses.Count;

            while (_clusters.Count < _count && _errorTotal > _errorTotalTolerance)
            {
                // Add New Cluster
                Cluster NewCluster = new Divide_Conquer_Cluster(pose, _tail, _count, _clusters.Count, PosesCount, mas.dgMode);
                NewCluster.addElementV(_tail);
                NewCluster.computeCenter(mas, _scalingFactor,_similarityMatrix);
                _clusters.Add(NewCluster);

                _iterations = 0;
                _errorTotal = double.MaxValue;
                NewErrorTolerance = double.MaxValue;

                do
                {
                    _queue = new SortedDictionary<double, List<int[,]>>();
#if CPU_PARALLEL
                    Parallel.For(0, pose.verticesCount, VertexID =>
#else
                    for (int VertexID = 0; VertexID < pose.verticesCount; VertexID++)
#endif
                    {
                        _used[VertexID] = false;
                    }
#if CPU_PARALLEL
);
#endif
                    for (int ClusterID = 0; ClusterID < _clusters.Count; ClusterID++)
                    {
                        _clusters[ClusterID].elementsV.Clear();
                        _clusters[ClusterID].addElementV(_clusters[ClusterID].head);

                        _used[_clusters[ClusterID].head] = true;
                        addAdjacentVertices(mas, pose, _clusters[ClusterID].head, ClusterID);
                    }

                    while (_queue.Count > 0)
                    {
                        KeyValuePair<double, List<int[,]>> PopPair = _queue.First();

                        double PopError = PopPair.Key;
                        List<int[,]> PopElements = PopPair.Value;
                        while (PopElements.Count > 0)
                        {
                            int VertexID = PopElements[0][0, 0];
                            int ClusterID = PopElements[0][0, 1];
                            PopElements.RemoveAt(0);

                            if (!_used[VertexID])
                            {
                                _used[VertexID] = true;
                                _clusters[ClusterID].addElementV(VertexID);
                                addAdjacentVertices(mas, pose, VertexID, ClusterID);
                            }
                        }
                        _queue.Remove(PopError);
                    }

                    // 3.1 Compute Cluster Centers
                    computeCenters(mas);
                    // 3.2 Compute Max Tail & Total Error
                    computeMaxTail_Error(mas, pose, out NewTail, out NewErrorTotal);

                    NewErrorTolerance = (_iterations == 0) ? _errorTolerance + 1 : Math.Abs(NewErrorTotal - _errorTotal);
                    // 3.3 Back Up
                    if (NewErrorTotal <= _errorTotal)
                    {
                        _tail = NewTail;
                        _errorTotal = NewErrorTotal;
                        _clustersBackUp = new List<Cluster>();
                        _clustersBackUp.AddRange(_clusters);
                    }
                    else
                        break;
                }
                while (++_iterations < _maxIterations && NewErrorTolerance > _errorTolerance);
            }

            // 4.1 Keep the best clusters
            _clusters = new List<Cluster>();
            _clusters.AddRange(_clustersBackUp);
            // 4.2 Keep the final tolerance
            _errorTolerance = NewErrorTolerance;

            foreach (Cluster ClusterC in _clusters)
            {
                ClusterC.computeCentersPos(mas);
                ClusterC.sphere.setBuffers();
            }
            setColor(false, false, false);
        }
        #endregion
    }

    public class K_Spectral_Clustering : Clustering
    {
        #region Private Properties
        bool            _nearestNeighborGraph;
        double          _eigenGap;
        int             _countInit;
        int             _percentageInit;
        List<Cluster>   _clustersInit;
        #endregion

        #region Public Properties
        public int percentageInit
        {
            get { return _percentageInit; }
            set { _percentageInit = value; }
        }
        public bool nearestNeighborGraph
        {
            get { return _nearestNeighborGraph; }
            set { _nearestNeighborGraph = value; }
        }
        public double eigenGap
        {
            get { return _eigenGap; }
            set { _eigenGap = value; }
        }
        public List<Cluster> clustersInit
        {
            get { return _clustersInit; }
        }
        #endregion

        #region Constructor
        public K_Spectral_Clustering() : base() { _percentageInit = 1; _nearestNeighborGraph = false; _eigenGap = 0.0; }
        #endregion

        #region Seeding Function
        public override void initialSeeding(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            _used = new bool[pose.verticesCount];
#if CPU_PARALLEL
            Parallel.For(0, pose.verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < pose.verticesCount; VertexID++)
#endif
            {
                _used[VertexID] = false;
            }
#if CPU_PARALLEL
);
#endif
            // P-Center Initialization - [or use Curvature-based Segmentation]
            _countInit    = (pose.verticesCount * _percentageInit)/100; // use 10% of vertices
            _clustersInit = new List<Cluster>();

            if (mas.clusteringPerPose)
            {
                List<Cluster> Clusters;
                if (pose.poseID - 1 == mas.selectedRestPose)
                {
                    pose.pCenter.count = _countInit;
                    pose.pCenter.compute(mas, pose);
                    pose.pCenter.computeNeighborhood(pose, false);
                    
                    Clusters = pose.pCenter.clusters;
                }
                else
                {
                    Clusters = mas.poses[pose.poseID - 1].kSpectral.clustersInit;
                    foreach (Cluster ClusterC in Clusters)
                        ClusterC.computeHead(mas);
                }

                foreach (Cluster ClusterC in Clusters)
                {
                    Cluster NewCluster      = new Simple_Cluster(pose, ClusterC.head, _clustersInit.Count, 1, mas.dgMode);
                    NewCluster.neighbors    = new List<Cluster>(ClusterC.neighbors);
                    _clustersInit.Add(NewCluster);
                    _used[ClusterC.head] = true;
                }
            }
            else
            {
                mas.pCenter.count = _countInit;
                mas.pCenter.compute(mas, pose);
                mas.pCenter.computeNeighborhood(pose, false);
                foreach (P_Center_Cluster ClusterC in mas.pCenter.clusters)
                {
                    Cluster NewCluster = new Simple_Cluster(pose, ClusterC.head, _clustersInit.Count, mas.poses.Count, mas.dgMode);
                    NewCluster.neighbors = new List<Cluster>(ClusterC.neighbors);

                    _clustersInit.Add(NewCluster);
                    _used[ClusterC.head] = true;
                }
            }
        }
        #endregion

        #region Spectral Analysis
        private Matrix<double>  calculateAffinityMatrix  (Mesh3DAnimationSequence mas, Mesh3D pose) // Parallel??
        {
            int PosesCount = mas.clusteringPerPose ? 1 : mas.poses.Count;

            // Compute Average Vertex Distances
            int TotalElements = 0;
            Matrix<double> A = new DenseMatrix(_countInit, _countInit);

            double S = 0.0;
            TotalElements=0;
            for (int ElementsID_i = 0; ElementsID_i < _countInit; ElementsID_i++)
                for (int ElementsID_j = 0; ElementsID_j < _countInit; ElementsID_j++)
                {
                    bool isNotNeighbor = false;
                    if (_nearestNeighborGraph)
                        foreach(Cluster ClusterC in _clustersInit[ElementsID_i].neighbors)
                            if (ClusterC.head != _clustersInit[ElementsID_j].head)
                            {
                                isNotNeighbor = true;
                                break;
                            }
                    if(isNotNeighbor)
                        continue;

                    double      AverageDistance = 0.0;
                    double []   Distances = new double[PosesCount];
                    for (int PoseID = 0; PoseID < PosesCount; PoseID++)
                    {
                        Distances[PoseID] = computeError(mas, mas.clusteringPerPose ? pose : mas.poses[PoseID], _clustersInit[ElementsID_i].head, _clustersInit[ElementsID_j].head);
                        AverageDistance += Distances[PoseID];
                    }

                    if (mas.clusteringSpectralDistanceMode == Modes.ClusteringSpectralDistance.NG02_TH07) // Theobalt07 - Animation Collage
                        S += AverageDistance; 

                    AverageDistance /= PosesCount;

                    // Compute Standard Deviation
                    double StandardDeviation = 0.0;
                    if (mas.clusteringSpectralDistanceMode == Modes.ClusteringSpectralDistance.NG02_TH07 ||
                        mas.clusteringSpectralDistanceMode == Modes.ClusteringSpectralDistance.NG02_DA08)
                    {
                        for (int PoseID = 0; PoseID < PosesCount; PoseID++)
                            StandardDeviation += Math.Pow(Distances[PoseID] - AverageDistance, 2);
                        StandardDeviation = Math.Sqrt(StandardDeviation / PosesCount);
                    }

                    if (mas.clusteringSpectralDistanceMode == Modes.ClusteringSpectralDistance.NG02_TH07) // Theobalt07 - Animation Collage
                        A[ElementsID_i, ElementsID_j] = StandardDeviation;
                    else if (mas.clusteringSpectralDistanceMode == Modes.ClusteringSpectralDistance.NG02_DA08) // DeAguiar08 - Automatic Conversion of MA into SA
                    {
                        A[ElementsID_i, ElementsID_j] = StandardDeviation + Math.Sqrt(AverageDistance);
                        S += A[ElementsID_i, ElementsID_j];
                    }
                    else if (mas.clusteringSpectralDistanceMode == Modes.ClusteringSpectralDistance.NG02) // Ng02 - On Spectral Clustering - Analysis and an algorithm
                    {
                        A[ElementsID_i, ElementsID_j] = AverageDistance;
                        S += A[ElementsID_i, ElementsID_j];
                    }

                    TotalElements++;
                }

            if (mas.clusteringSpectralDistanceMode == Modes.ClusteringSpectralDistance.NG02) // Ng02 - On Spectral Clustering - Analysis and an algorithm
            {
                S /= TotalElements; S *= S; S *= 2;
            }
            else
            {
                S /= (PosesCount * PosesCount); S *= S;
            }

#if CPU_PARALLEL
            Parallel.For(0, _countInit, ElementsID_i =>
#else
            for (int ElementsID_i = 0; ElementsID_i < _countInit; ElementsID_i++)
#endif
            {
                for (int ElementsID_j = 0; ElementsID_j < _countInit; ElementsID_j++)
                    A[ElementsID_i, ElementsID_j] = Math.Exp(-A[ElementsID_i, ElementsID_j] / S);               
            }
#if CPU_PARALLEL
);
#endif
            return A;
        }
        private Matrix<double>  calculateSpectralAnalysis(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            // 1. Affinity matrix
            //mas.clusteringDistanceMode = Modes.ClusteringDistance.VERTEX;
            Matrix<double> AffinityMatrix = calculateAffinityMatrix(mas, pose);
            //mas.clusteringDistanceMode = Modes.ClusteringDistance.DEFORMATION_GRADIENT;
            //Matrix<double> AffinityMatrixD = calculateAffinityMatrix(mas);

            //Matrix<double> AffinityMatrix = new DenseMatrix(_countInit, _countInit, 0.0);
            //for (int ElementsID_i = 0; ElementsID_i < _countInit; ElementsID_i++)
                //for (int ElementsID_j = 0; ElementsID_j < _countInit; ElementsID_j++)
                    //AffinityMatrix[ElementsID_i, ElementsID_j] = AffinityMatrixD[ElementsID_i, ElementsID_j] *AffinityMatrixV[ElementsID_i, ElementsID_j];

            // 2. Degree matrix
            double[] DiagMatrix = new double[AffinityMatrix.RowCount];
#if CPU_PARALLEL
            Parallel.For(0, AffinityMatrix.RowCount, Row =>
#else
            for (int Row = 0; Row < AffinityMatrix.RowCount; Row++)
#endif
            {
                DiagMatrix[Row] = (mas.clusteringSpectralGraphMode == Modes.ClusteringSpectralGraph.RANDOM_WALK) ? Math.Pow(AffinityMatrix.Row(Row).Sum(), -1) : Math.Pow(AffinityMatrix.Row(Row).Sum(), -0.5);
            }
#if CPU_PARALLEL
);
#endif
            Matrix<double> DModMatrix = new DenseMatrix(AffinityMatrix.RowCount, AffinityMatrix.RowCount);
            DModMatrix.SetDiagonal(DiagMatrix);
            // L Matrix
            Matrix<double> L_Matrix = (mas.clusteringSpectralGraphMode == Modes.ClusteringSpectralGraph.RANDOM_WALK) ?
                DModMatrix.Multiply(AffinityMatrix) : 
                DModMatrix.Multiply(AffinityMatrix.Multiply(DModMatrix));

            // 3. Compute PCA - EigenValues-EigenVectors
            Matrix<double> EigenSelected;
            //L_Matrix = MatrixFunctions.standardisedMatrix(L_Matrix);
            if (_nipals)
                EigenSelected = MatrixFunctions.calculateNipals((Matrix)L_Matrix, _count);
            else
            {
                var L_MatrixSVD  = L_Matrix.Svd(true);
                var EigenValues  = L_MatrixSVD.S;
                var EigenVectors = L_MatrixSVD.U;

                if(_eigenGap > 0.0)
                    for(int EigenValueID=0; EigenValueID<EigenValues.Count-1; EigenValueID++)
                        if (EigenValues[EigenValueID] - EigenValues[EigenValueID + 1] < _eigenGap)
                        {
                            _count = EigenValueID;
                            break;
                        }
                // Get required eigenvectors
                EigenSelected = EigenVectors.SubMatrix(0, EigenVectors.RowCount, 0, _count);
            }

            // 4. Normalize the selected matrix
            if(mas.clusteringSpectralGraphMode == Modes.ClusteringSpectralGraph.SYMMETRIC)
#if CPU_PARALLEL
                Parallel.For(0, EigenSelected.RowCount, Row =>
#else
                for (int Row = 0; Row < EigenSelected.RowCount; Row++)
#endif
                {
                    double[] RowData = EigenSelected.Row(Row).ToArray();
                    EigenSelected.SetRow(Row, EigenSelected.Row(Row).Divide(MatrixFunctions.lengthData(RowData)));
                }
#if CPU_PARALLEL
);
#endif
            return EigenSelected;
        }
        #endregion

        #region Compute Function
        public override void compute(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            // 0. Init Seeding
            initialSeeding(mas, pose);
            
            // 1. Calculate Spectral Analysis
            Matrix<double> SelectedEigen = calculateSpectralAnalysis(mas, pose);

            // 2. Compute K-means 

            // 2.1 Init Seeding
            _clusters = new List<Cluster>();
            for (int ClusterID = 0; ClusterID < _count; ++ClusterID)
            {
                int SeedCluster = ClusterID; // randNum.Next(_countInit);
                Cluster NewCluster = new Simple_Cluster(pose, SeedCluster, _clusters.Count, mas.clusteringPerPose ? 1 : mas.poses.Count, mas.dgMode);
                _clusters.Add(NewCluster);
            }

            // 2.2 Init Cluster Centers
            computeCenters(SelectedEigen);

            _iterations = 0;
            _errorTotal = double.MaxValue;
            double NewErrorTotal = double.MaxValue;
            double NewErrorTolerance = 0.0;

            do
            {
                // 1. Clear Elements
                _errorTotal = NewErrorTotal;
                foreach (Cluster ClusterC in _clusters)
                    ClusterC.elementsV.Clear();

                // 2. Add Vertices to the Closest Cluster
                int[] MinIDs = new int[_countInit];
#if CPU_PARALLEL
                Parallel.For(0, _countInit, ElementID =>
#else
                for (int ElementID = 0; ElementID < _countInit; ++ElementID)
#endif
                {
                    int MinID = -1;
                    double MinError = double.MaxValue;
                    for (int ClusterID = 0; ClusterID < _clusters.Count; ++ClusterID)
                    {
                        double Error = Math.Pow(_clusters[ClusterID].computeErrorFromCentroid(SelectedEigen, ElementID), 2);
                        if (MinError > Error)
                        {
                            MinID = ClusterID;
                            MinError = Error;
                        }
                    }
                    MinIDs[ElementID] = MinID;
                }
#if CPU_PARALLEL
);
#endif
                for (int ElementID = 0; ElementID < _countInit; ++ElementID)
                    _clusters[MinIDs[ElementID]].addElementV(ElementID);

                // 3. Compute Centers
                computeCenters(SelectedEigen);
                // 4. Compute Error
                NewErrorTotal       = computeError(SelectedEigen);

                NewErrorTolerance   = (_iterations == 0) ? _errorTolerance + 1 : Math.Abs(NewErrorTotal - _errorTotal);
                if (NewErrorTotal <= _errorTotal)
                {
                    _errorTotal = NewErrorTotal;
                    _clustersBackUp = new List<Cluster>();
                    foreach (Cluster ClusterC in _clusters)
                        _clustersBackUp.Add(ClusterC);
                }
                else
                    break;

            }
            while (++_iterations < _maxIterations && NewErrorTolerance > _errorTolerance);
            
            // 6.0 Keep the Final Tolerance
            _errorTolerance = NewErrorTolerance;
            
            // 6.1 Keep the Best Clusters
            _clusters = new List<Cluster>();
            _clusters.AddRange(_clustersBackUp);

            // 6.2 Compute Correct Head (Global)
            int[] ClusterParent = new int[_countInit];
            for (int ClusterID = 0; ClusterID < _clusters.Count; ++ClusterID)
            {
                Cluster ClusterC = _clusters[ClusterID];
                int Head = ClusterC.head;
                ClusterC.head = _clustersInit[Head].head;
#if CPU_PARALLEL
                Parallel.For(0, ClusterC.elementsV.Count, ElementID =>
#else
                for (int ElementID = 0; ElementID < ClusterC.elementsV.Count; ++ElementID)
#endif               
                {
                    int El = ClusterC.elementsV[ElementID];
                    ClusterC.elementsV[ElementID] = _clustersInit[El].head;
                    ClusterParent[El] = ClusterID;
                }
#if CPU_PARALLEL
);
#endif
            }

            // 6.3 Remove Empty Clusters from Clusters List
            List<int> ClustersToBeRemoved = new List<int>();
            for (int ClusterID = 0; ClusterID < _clusters.Count; ++ClusterID)
                if (_clusters[ClusterID].isEmpty())
                    ClustersToBeRemoved.Add(ClusterID);
            ClustersToBeRemoved.Reverse();
            foreach (int ClusterID in ClustersToBeRemoved)
                _clusters.RemoveAt(ClusterID);

            // 6.4 Assign remaining vertices to the optimal Clusters
            int[] MinParentIDs = new int[pose.verticesCount];
#if CPU_PARALLEL
            Parallel.For(0, pose.verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < pose.verticesCount; VertexID++)
#endif
            {
                if (!_used[VertexID])
                {
                    int MinID = -1;
                    double MinError = double.MaxValue;
                    for (int ClusterID = 0; ClusterID < _clustersInit.Count; ++ClusterID)
                    {
                        double Error = mas.clusteringPerPose ? _clustersInit[ClusterID].computeErrorFromHead(mas, pose, VertexID) : _clustersInit[ClusterID].computeErrorFromHeads(mas, VertexID);
                        if (MinError > Error)
                        {
                            MinID = ClusterID;
                            MinError = Error;
                        }
                    }
                    MinParentIDs[VertexID] = MinID;
                }
            }
#if CPU_PARALLEL
);
#endif
            for (int VertexID = 0; VertexID < pose.verticesCount; ++VertexID)
                if (!_used[VertexID])
                    _clusters[ClusterParent[MinParentIDs[VertexID]]].addElementV(VertexID);

            // 6.5 Compute Cluster Centers
            foreach (Cluster ClusterC in _clusters)
            {
                ClusterC.computeCentersPos(mas);
                ClusterC.sphere.setBuffers();
            }
        }
        #endregion
    }

    public class C_PCA_Clustering : Clustering
    {
        #region Private Properties
        int _basisVectorsCount;
        #endregion

        #region Public Properties
        public int basisVectorsCount
        {
            get { return _basisVectorsCount; }
            set { _basisVectorsCount = value; }
        }
        #endregion

        #region Constructor
        public C_PCA_Clustering() : base() { _basisVectorsCount = 1; }
        #endregion

        #region Vertex Trajectories Matrix
        private Matrix<double> calculateMatrixData(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            int RowLength = mas.poses.Count;
            if (mas.clusteringDistanceMode != Modes.ClusteringDistance.DEFORMATION_GRADIENT)
                RowLength *= 3;

            Matrix<double> VertexTrajectories = new DenseMatrix(RowLength, mas.verticesCount);
#if CPU_PARALLEL
            Parallel.For(0, mas.verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < mas.verticesCount; VertexID++)
#endif
            {
                for (int PoseID = 0; PoseID < mas.poses.Count; PoseID++)
                {
                    if      (mas.clusteringDistanceMode == Modes.ClusteringDistance.VERTEX)
                    {
                        VertexTrajectories.At(PoseID * 3    , VertexID, mas.poses[PoseID].verticesData[VertexID].X);
                        VertexTrajectories.At(PoseID * 3 + 1, VertexID, mas.poses[PoseID].verticesData[VertexID].Y);
                        VertexTrajectories.At(PoseID * 3 + 2, VertexID, mas.poses[PoseID].verticesData[VertexID].Z);
                    }
                    else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.NORMAL)
                    {
                        VertexTrajectories.At(PoseID * 3    , VertexID, mas.poses[PoseID].normalsVerticesData[VertexID].X);
                        VertexTrajectories.At(PoseID * 3 + 1, VertexID, mas.poses[PoseID].normalsVerticesData[VertexID].Y);
                        VertexTrajectories.At(PoseID * 3 + 2, VertexID, mas.poses[PoseID].normalsVerticesData[VertexID].Z);
                    }
                    else if (mas.clusteringDistanceMode == Modes.ClusteringDistance.DEFORMATION_GRADIENT)
                    {
                        DeformationGradient DG;
                        if      (mas.dgMode == Modes.DeformationGradient.REST_POSE) DG = mas.poses[PoseID].dgRP;
                        else if (mas.dgMode == Modes.DeformationGradient.MEAN_POSE) DG = mas.poses[PoseID].dgMP;
                        else                                                        DG = mas.poses[PoseID].dgP2P;
                        VertexTrajectories.At(PoseID, VertexID, DG.VerticesData[VertexID]);
                    }
                }
            }
#if CPU_PARALLEL
);
#endif
            return VertexTrajectories;
        }
        #endregion 

        #region Seeding Function
        public override void initialSeeding(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            int PosesCount = mas.clusteringPerPose ? 1 : mas.poses.Count;

            // 0.1 Initial Clusters
            _clusters = new List<Cluster>();

            // Random Seeding
            if (_randomSeeding)
            {
                for (int ClusterID = 0; ClusterID < _count; ++ClusterID)
                {
                    int SeedCluster = _randNum.Next(pose.verticesCount);
                    C_PCA_Cluster NewCluster = new C_PCA_Cluster(pose, SeedCluster, _clusters.Count, PosesCount, mas.dgMode);
                    
                    NewCluster.addElementV(SeedCluster);
                    NewCluster.basisVectors = new DenseMatrix(_basisVectorsCount, PosesCount);
                    NewCluster.basisVectors.SetSubMatrix(0, _basisVectorsCount, 0, PosesCount, DenseMatrix.CreateIdentity(PosesCount));

                    _clusters.Add(NewCluster);
                }
            }
            // P-Center Seeding
            else
            {
                if (mas.clusteringPerPose)
                {
                    List<Cluster> Clusters;
                    if (pose.poseID - 1 == mas.selectedRestPose)
                    {
                        pose.pCenter.compute(mas, pose);
                        Clusters = pose.pCenter.clusters;
                    }
                    else
                    {
                        Clusters = mas.poses[pose.poseID - 1].cPCA.clusters;
                        foreach (Cluster ClusterC in Clusters)
                            ClusterC.computeHead(mas);
                    }

                    foreach (Cluster ClusterC in Clusters)
                    {
                        Cluster NewCluster = new C_PCA_Cluster(pose, ClusterC.head, _clusters.Count, PosesCount, mas.dgMode);
                        NewCluster.addRangeElementV(ClusterC.elementsV);
                        _clusters.Add(NewCluster);
                    }
                }
                else
                {
                    mas.pCenter.compute(mas, pose);
                    foreach (P_Center_Cluster ClusterC in mas.pCenter.clusters)
                    {
                        Cluster NewCluster = new C_PCA_Cluster(pose, ClusterC.head, _clusters.Count, PosesCount, mas.dgMode);
                        NewCluster.addRangeElementV(ClusterC.elementsV);
                        _clusters.Add(NewCluster);
                    }
                }
            }
        }
        #endregion

        #region Compute Centers
        public new void computeCenters(Matrix<double> matrix)
        {
#if CPU_PARALLEL
            Parallel.ForEach(_clusters, ClusterC =>
#else
            foreach (Cluster ClusterC in _clusters)
#endif
            {
                if (!ClusterC.isEmpty())
                    ((C_PCA_Cluster)ClusterC).computeCenter(matrix);
            }
#if CPU_PARALLEL
);
#endif
        }
        #endregion

        #region Compute Basis Vectors
        public void computeBasisVectors(Mesh3DAnimationSequence mas, Matrix<double> matrix)
        {
#if CPU_PARALLEL
            Parallel.ForEach(_clusters, ClusterC =>
#else
            foreach (Cluster ClusterC in _clusters)
#endif
            {
                if (!ClusterC.isEmpty())
                    ((C_PCA_Cluster)ClusterC).computeBasisVectors(matrix, _basisVectorsCount, _nipals);
            }
#if CPU_PARALLEL
);
#endif
        }
        #endregion

        #region Compute Error
        public new double computeError(Matrix<double> matrix)
        {
            double ErrorTotal = 0.0;
            foreach (C_PCA_Cluster ClusterC in _clusters)
                ErrorTotal += ClusterC.computeError(matrix, _basisVectorsCount);
            return ErrorTotal;
        }
        #endregion

        #region Compute Function
        public override void compute(Mesh3DAnimationSequence mas, Mesh3D pose)
        {
            // 0. Init Seeding
            initialSeeding(mas, pose);

            // 1. Calculate Vertex Trajectories Matrix 
            Matrix<double> VertexTrajectories = calculateMatrixData(mas, pose);

            // 2. Compute K-means

            // 2.1 Init Cluster Centers
            computeCenters(VertexTrajectories);
            // 2.2 Init Cluster Basis Vectors
            if(!_randomSeeding)
                computeBasisVectors(mas, VertexTrajectories);

            _iterations = 0;
            _errorTotal = double.MaxValue;
            double NewErrorTotal = double.MaxValue;
            double NewErrorTolerance = 0.0;

            do
            {
                // 1. Clear Elements
                _errorTotal = NewErrorTotal;
                foreach (Cluster ClusterC in _clusters)
                    ClusterC.elementsV.Clear();

                // 2. Add Vertices to the Closest Cluster
                int[] MinIDs = new int[pose.verticesCount];
#if CPU_PARALLEL
                Parallel.For(0, pose.verticesCount, VertexID =>
#else
                for (int VertexID = 0; VertexID < pose.verticesCount; ++VertexID)
#endif
                {
                    int MinID = -1;
                    double MinError = double.MaxValue;
                    for (int ClusterID = 0; ClusterID < _clusters.Count; ++ClusterID)
                    {
                        double Error = ((C_PCA_Cluster)_clusters[ClusterID]).computeError(VertexTrajectories.Column(VertexID), _basisVectorsCount);
                        if (MinError > Error)
                        {
                            MinID = ClusterID;
                            MinError = Error;
                        }
                    }
                    MinIDs[VertexID] = MinID;
                }
#if CPU_PARALLEL
);
#endif
                for (int VertexID = 0; VertexID < pose.verticesCount; ++VertexID)
                    _clusters[MinIDs[VertexID]].addElementV(VertexID);
                // 3.1 Compute Centers
                computeCenters(VertexTrajectories);
                // 3.2 Compute Basis Vectors
                computeBasisVectors(mas, VertexTrajectories);
                // 4. Compute Error
                NewErrorTotal = computeError(VertexTrajectories);

                NewErrorTolerance = (_iterations == 0) ? _errorTolerance + 1 : Math.Abs(NewErrorTotal - _errorTotal);
                if (NewErrorTotal <= _errorTotal)
                {
                    _errorTotal = NewErrorTotal;
                    _clustersBackUp = new List<Cluster>();
                    _clustersBackUp.AddRange(_clusters);
                }
                else
                    break;
            }
            while (++_iterations < _maxIterations && NewErrorTolerance > _errorTolerance);

            // 5.1 Keep the Best Clusters
            _clusters = new List<Cluster>();
            _clusters.AddRange(_clustersBackUp);
            // 5.2 Keep the Final Tolerance
            _errorTolerance = NewErrorTolerance;
            // 5.3 Compute Sphere Buffers
            foreach (Cluster ClusterC in _clusters)
                ClusterC.sphere.setBuffers();
        }
        #endregion
    }
}