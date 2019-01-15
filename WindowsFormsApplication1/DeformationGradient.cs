using OpenTK;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MathNet.Numerics;
using System.Windows.Forms;
using System.IO;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace abasilak
{
    public class DeformationGradient
    {
        #region Private Properties

        float[]     _FacetsData;
        float[]     _VerticesData;
        float[]     _VelocityData;
        float[]     _VerticesDataBackUp;
        Vector3[]   _colorsVerticesData;
        
        #endregion

        #region Public Properties
        public float[] FacetsData
        {
            get { return _FacetsData; }
        }
        public float[] VerticesData
        {
            get { return _VerticesData; }
        }
        public float[] VelocityData
        {
            get { return _VelocityData; }
        }
        #endregion

        #region Constructor
        public DeformationGradient(int numVertices, int numFacets)
        {
            _FacetsData = new float[numFacets];
            _VerticesData = new float[numVertices];
            _VelocityData = new float[numVertices];
            _VerticesDataBackUp = new float[numVertices];
            _colorsVerticesData = new Vector3[numVertices];
        }
        #endregion

        #region Calculate Functions
        
        private Matrix<double>  orientationMatrix(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            DenseMatrix result = new DenseMatrix(3, 3);

            //Edge 1 of the triangle
            Vector3 v21 = v2 - v1;
            //Edge 2 of the triangle
            Vector3 v31 = v3 - v1;
            //Normal vector of the triangle
            Vector3 n = Vector3.Cross(v21, v31);
            n.Normalize();

            result.SetColumn(0, OpenTK_To_MathNET.Vector3ToVector(v21).ToArray());
            result.SetColumn(1, OpenTK_To_MathNET.Vector3ToVector(v31).ToArray());
            result.SetColumn(2, OpenTK_To_MathNET.Vector3ToVector(n).ToArray());

            return result;
        }
        private void polarize(Svd<double> DG_SVD, out Vector3d RotationAxis, out double RotationAngle)
        {
            DenseMatrix RotationalMatrix = (DenseMatrix)DG_SVD.U.Multiply(DG_SVD.VT);
            Quaterniond RotationQ = OpenTK_To_MathNET.MatrixToQuaternion(RotationalMatrix);
            RotationQ.Normalize();
            RotationQ.ToAxisAngle(out RotationAxis, out RotationAngle);
        }
        private double          computeAreas(Vector3 restPosV1, Vector3 restPosV2, Vector3 restPosV3, Vector3 thisPosV1, Vector3 thisPosV2, Vector3 thisPosV3)
        {
            double restArea = GeometryFunctions.computeTriangleArea(restPosV1, restPosV2, restPosV3);
            double thisArea = GeometryFunctions.computeTriangleArea(thisPosV1, thisPosV2, thisPosV3);

            //Check the magnitude to scale the areas if they are very small
            double m = Precision.Magnitude(restArea);
            if (m < 0)
            {
                restArea *= Math.Pow(10, Math.Abs(m));
                thisArea *= Math.Pow(10, Math.Abs(m));
            }

            //If there was a deformation
            return (Math.Abs(thisArea - restArea) / (restArea));
        }

        public void calculate(Mesh3D restPose, Mesh3D thisPose, Modes.DeformationGradient dgMode, Modes.DeformationGradientComponents dgComponentsMode)
        {
            if (dgComponentsMode == Modes.DeformationGradientComponents.ACCELERATION ||
                dgComponentsMode == Modes.DeformationGradientComponents.VELOCITY 
                //|| dgComponentsMode == Modes.DeformationGradientComponents.ADJ_FROBENIUS
                )
            {
                int Time = thisPose.poseID - restPose.poseID;
#if CPU_PARALLEL
                Parallel.For(0, thisPose.verticesCount, VertexID =>
#else
                for (int VertexID = 0; VertexID < thisPose.verticesCount; VertexID++)
#endif
                {
                    _VelocityData[VertexID] = (thisPose.verticesData[VertexID] - restPose.verticesData[VertexID]).Length / restPose.aabb.diagonal;
                    _VelocityData[VertexID] /= (Time == 0) ? 1 : Time;

                    _VerticesData[VertexID]         = (dgComponentsMode == Modes.DeformationGradientComponents.VELOCITY) ? 
                                                        _VelocityData[VertexID] :
                                                        _VelocityData[VertexID] - restPose.dg[(int)dgMode].VelocityData[VertexID];
                    _VerticesDataBackUp[VertexID]   = _VerticesData[VertexID];
                }
#if CPU_PARALLEL
);
#endif
            }
            else
            {
#if CPU_PARALLEL
                Parallel.For(0, thisPose.facetsCount, FacetID =>
#else
                for (int FacetID = 0; FacetID < thisPose.facetsCount; FacetID++)
#endif
                {
                    Vector3 restPosV1, restPosV2, restPosV3;
                    Vector3 thisPosV1, thisPosV2, thisPosV3;

                    // RestPose 
                    restPosV1 = restPose.verticesData[restPose.indicesData[3 * FacetID]];
                    restPosV2 = restPose.verticesData[restPose.indicesData[3 * FacetID + 1]];
                    restPosV3 = restPose.verticesData[restPose.indicesData[3 * FacetID + 2]];

                    // ThisPose 
                    thisPosV1 = thisPose.verticesData[thisPose.indicesData[3 * FacetID]];
                    thisPosV2 = thisPose.verticesData[thisPose.indicesData[3 * FacetID + 1]];
                    thisPosV3 = thisPose.verticesData[thisPose.indicesData[3 * FacetID + 2]];

                    // Facet Area
                    if (dgComponentsMode == Modes.DeformationGradientComponents.FACET_AREA)
                        _FacetsData[FacetID] = (float)computeAreas(restPosV1, restPosV2, restPosV3, thisPosV1, thisPosV2, thisPosV3);
                    else
                    {
                        // 0. Compute Orientation Matrices
                        DenseMatrix restPosOrient = (DenseMatrix)orientationMatrix(restPosV1, restPosV2, restPosV3);
                        DenseMatrix thisPosOrient = (DenseMatrix)orientationMatrix(thisPosV1, thisPosV2, thisPosV3);

                        // 0.a Store Normal Facets
                        var K = thisPosOrient.Column(2);
                        Vector3d NormalsFacetsData = new Vector3d(K[0], K[1], K[2]);

                        // 1. Compute Deformation Gradient Matrix
                        DenseMatrix DG = thisPosOrient * (DenseMatrix)restPosOrient.Inverse();
                        // 2. Frobenius Norm
                        if (dgComponentsMode == Modes.DeformationGradientComponents.DG_FROBENIUS)
                            _FacetsData[FacetID] = (float)DG.FrobeniusNorm();
                        else
                        {
                            // 3. Polarize Deformation Gradient Matrix
                            var DG_SVD = DG.Svd(true);

                            if (dgComponentsMode == Modes.DeformationGradientComponents.ROT_ANGLE ||
                                dgComponentsMode == Modes.DeformationGradientComponents.ROT_AXIS)
                            {
                                // 3.1 Rotational Part
                                Vector3d RotAxis;
                                double RotAngle, RotAxisAngle;

                                polarize(DG_SVD, out RotAxis, out RotAngle);
                                if (dgComponentsMode == Modes.DeformationGradientComponents.ROT_ANGLE)
                                    _FacetsData[FacetID] = (float)RotAngle;
                                else
                                {
                                    Vector3d RotAxis_ = new Vector3d(RotAxis);
                                    Vector3d RotAxis_Negative = new Vector3d(-RotAxis);

                                    RotAxisAngle = Math.Min(
                                        GeometryFunctions.computeVectorsAngle(NormalsFacetsData, RotAxis_),
                                        GeometryFunctions.computeVectorsAngle(NormalsFacetsData, RotAxis_Negative));

                                    //_FacetsData[FacetID] = (float)RotAxisAngle * (float)RotAngle;
                                    _FacetsData[FacetID] = (float)Math.Sqrt((RotAxis.LengthSquared) + RotAngle * RotAngle);
                                }
                            }
                            else if (dgComponentsMode == Modes.DeformationGradientComponents.SCALE ||
                                     dgComponentsMode == Modes.DeformationGradientComponents.SHEAR)
                            {
                                // 3.2 Stretch Part
                                DenseMatrix _StretchMatrix = (DenseMatrix)(DG_SVD.VT.Transpose().Multiply(DG_SVD.W)).Multiply(DG_SVD.VT);

                                // 3.3 Shear Part                         
                                if (dgComponentsMode == Modes.DeformationGradientComponents.SHEAR)
                                {
                                    DenseMatrix _ShearMatrix = DenseMatrix.OfArray(_StretchMatrix.ToArray());
                                    _ShearMatrix.SetDiagonal(new double[3] { 0.0, 0.0, 0.0 });
                                    _FacetsData[FacetID] = (float)_ShearMatrix.FrobeniusNorm();
                                }
                                // 3.4 Scale Part
                                else
                                {
                                    DenseMatrix _ScaleMatrix = new DenseMatrix(3, 3);
                                    _ScaleMatrix.SetDiagonal(_StretchMatrix.Diagonal());
                                    _FacetsData[FacetID] = (float)_ScaleMatrix.FrobeniusNorm();
                                }
                            }
                        }
                    }

                    // Adjacent Deformation Metric
                    {
                        //_AdjData[FacetID] = 0.0f;
                        //for (int NeighborID = 0; NeighborID < thisPose.neighborsFacetsFacetsData[FacetID].Count; NeighborID++)
                        {
                            //int Neighbor = thisPose.neighborsFacetsFacetsData[FacetID][NeighborID];
                            //double FrobeniusDifFacets = (_DG[FacetID] - _DG[Neighbor]).FrobeniusNorm();
                            //double DistaceFacetCenters = (thisPose.centersFacetData[FacetID] - thisPose.centersFacetData[Neighbor]).Length;

                            //_AdjData[FacetID] = Math.Max(_AdjData[FacetID], (float)((FrobeniusDifFacets * FrobeniusDifFacets) / DistaceFacetCenters));
                            // _AdjData[FacetID] += (float) ((FrobeniusDifFacets * FrobeniusDifFacets) / DistaceFacetCenters);
                        }
                        //_AdjData[FacetID] /= thisPose.neighborsFacetsFacetsData[FacetID].Count;
                    }
                }
#if CPU_PARALLEL
);
#endif
            }
        }
        public void calculate(List<DeformationGradient> dgList, int verticesCount, int facetsCount)
        {
#if CPU_PARALLEL
            Parallel.For(0, facetsCount, FacetID =>
#else
            for (int FacetID = 0; FacetID < facetsCount; FacetID++)
#endif
            {
                _FacetsData[FacetID] = 0.0f;
                for (int DgID = 0; DgID < dgList.Count; DgID++)
                    _FacetsData[FacetID] += dgList[DgID].FacetsData[FacetID];
                //_AdjData[FacetID] = Math.Max(_AdjData[FacetID], dgList[DgID].AdjData[FacetID]);
                //...or use : _AdjData[FacetID] += dgList[DgID].AdjData[FacetID];                   
                _FacetsData[FacetID] /= dgList.Count;
            }

#if CPU_PARALLEL
);
#endif

#if CPU_PARALLEL
            Parallel.For(0, verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < verticesCount; VertexID++)
#endif
            {
                _VerticesData[VertexID] = 0.0f;
                for (int DgID = 0; DgID < dgList.Count; DgID++)
                    _VerticesData[VertexID] += dgList[DgID].VerticesData[VertexID];
                _VerticesData[VertexID] /= dgList.Count;

                /*
                 // to be removed
                _VelocityData[VertexID] = 0.0f;
                for (int DgID = 0; DgID < dgList.Count; DgID++)
                    _VelocityData[VertexID] += dgList[DgID].VelocityData[VertexID];
                _VelocityData[VertexID] /= dgList.Count;
                 */
            }
#if CPU_PARALLEL
);
#endif
        }
        public void calculateVariability(Mesh3DAnimationSequence mas, Mesh3D meanPose, int EigenCount)
        {
            Matrix<double>[] MatrixPose = new Matrix<double>[mas.poses.Count];

            for (int PoseID = 0; PoseID < mas.poses.Count; PoseID++)
            {
                MatrixPose[PoseID] = new DenseMatrix(mas.verticesCount * 3, 1);
#if CPU_PARALLEL
                Parallel.For(0, mas.verticesCount, VertexID =>
#else
                for (int VertexID = 0; VertexID < mas.verticesCount; VertexID++)
#endif
                {
                    MatrixPose[PoseID].At(VertexID * 3, 0, mas.poses[PoseID].verticesData[VertexID].X);
                    MatrixPose[PoseID].At(VertexID * 3 + 1, 0, mas.poses[PoseID].verticesData[VertexID].Y);
                    MatrixPose[PoseID].At(VertexID * 3 + 2, 0, mas.poses[PoseID].verticesData[VertexID].Z);
                }
#if CPU_PARALLEL
);
#endif
            }

            Matrix<double> MatrixMean = new DenseMatrix(mas.verticesCount * 3, 1);
#if CPU_PARALLEL
            Parallel.For(0, mas.verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < mas.verticesCount; VertexID++)
#endif
            {
                MatrixMean.At(VertexID * 3, 0, meanPose.verticesData[VertexID].X);
                MatrixMean.At(VertexID * 3 + 1, 0, meanPose.verticesData[VertexID].Y);
                MatrixMean.At(VertexID * 3 + 2, 0, meanPose.verticesData[VertexID].Z);
            }
#if CPU_PARALLEL
);
#endif
            Matrix<double> MatrixCovariance = new DenseMatrix(mas.verticesCount * 3, mas.verticesCount * 3);
            for (int PoseID = 0; PoseID < mas.poses.Count; PoseID++)
            {
                Matrix<double> MatrixTmp = MatrixPose[PoseID] - MatrixMean;
                MatrixCovariance += MatrixTmp.Multiply(MatrixTmp.Transpose());
            }
            MatrixCovariance = MatrixCovariance.Divide(mas.poses.Count);

            var MatrixCovarianceSvd = MatrixCovariance.Svd(true);

            Vector<double> EigenValues = MatrixCovarianceSvd.S.SubVector(0, EigenCount);
            Matrix<double> EigenVectors = MatrixCovarianceSvd.U.SubMatrix(0, MatrixCovariance.RowCount, 0, EigenCount);

#if CPU_PARALLEL
            Parallel.For(0, mas.verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < mas.verticesCount; VertexID++)
#endif
            {
                _VerticesData[VertexID] = 0.0f;
                for (int ColumnID = 0; ColumnID < EigenVectors.ColumnCount; ColumnID++)
                    _VerticesData[VertexID] += (float)(EigenValues[ColumnID] * EigenVectors.Column(ColumnID).SubVector(3 * VertexID, 3).Norm(2));
                _VerticesData[VertexID] /= (float)EigenValues.Sum();
                
                //_VariabilityData[VertexID] = 0.0f;
                //for (int ColumnID = 0; ColumnID < EigenVectors.ColumnCount; ColumnID++)
                  //  _VariabilityData[VertexID] += (float)(EigenValues[ColumnID] * EigenVectors.Column(ColumnID).SubVector(3 * VertexID, 3).Norm(2));
                //_VariabilityData[VertexID] /= (float)EigenValues.Sum();
            }
#if CPU_PARALLEL
);
#endif
        }

        #endregion

        #region Set Functions
        public void setVertexData(Mesh3D thisPose, Modes.DeformationGradientComponents dgComponentsMode, bool normalized)
        {
            if (dgComponentsMode == Modes.DeformationGradientComponents.ACCELERATION ||
                dgComponentsMode == Modes.DeformationGradientComponents.VELOCITY ||
                dgComponentsMode == Modes.DeformationGradientComponents.ADJ_FROBENIUS
                )
                _VerticesData = MatrixFunctions.copyData(_VerticesDataBackUp);
            else
            {
#if CPU_PARALLEL
                Parallel.For(0, thisPose.verticesCount, VertexID =>
#else
                for (int VertexID = 0; VertexID < thisPose.verticesCount; VertexID++)
#endif
                {
                    _VerticesData[VertexID] = 0.0f;
                    if (thisPose.neighborsFacetsData[VertexID].Count > 0)
                    {
                        for (int NeighborID = 0; NeighborID < thisPose.neighborsFacetsData[VertexID].Count; NeighborID++)
                            _VerticesData[VertexID] += _FacetsData[thisPose.neighborsFacetsData[VertexID][NeighborID]];
                        _VerticesData[VertexID] /= thisPose.neighborsFacetsData[VertexID].Count;
                    }
                }
#if CPU_PARALLEL
                );
#endif
            }

            if (normalized)
                _VerticesData = MatrixFunctions.normalizeData(_VerticesData);
        }
        public void setVertexData(Mesh3D thisPose, DeformationGradient dg, Modes.DeformationGradientComponents dgComponentsMode, bool normalized)
        {
            if (dgComponentsMode == Modes.DeformationGradientComponents.ACCELERATION || 
                dgComponentsMode == Modes.DeformationGradientComponents.VELOCITY
                )
                _VerticesData = MatrixFunctions.copyData(dg.VerticesData);
            else
            {
#if CPU_PARALLEL
                Parallel.For(0, thisPose.verticesCount, VertexID =>
#else
                for (int VertexID = 0; VertexID < thisPose.verticesCount; VertexID++)
#endif
                {
                    _VerticesData[VertexID] = 0.0f;
                    if (thisPose.neighborsFacetsData[VertexID].Count > 0)
                    {
                        for (int NeighborID = 0; NeighborID < thisPose.neighborsFacetsData[VertexID].Count; NeighborID++)
                            _VerticesData[VertexID] += dg.FacetsData[thisPose.neighborsFacetsData[VertexID][NeighborID]];
                        _VerticesData[VertexID] /= thisPose.neighborsFacetsData[VertexID].Count;
                    }
                }
#if CPU_PARALLEL
);
#endif
            }
            /*
            if (dgComponentsMode == Modes.DeformationGradientComponents.ADJ_FROBENIUS)
            {
                float[] VEL = new float[_VerticesData.Length];
                float[] ACC = new float[_VerticesData.Length];

                VEL = MatrixFunctions.normalizeData(MatrixFunctions.copyData(dg.VelocityData));
                ACC = MatrixFunctions.normalizeData(MatrixFunctions.copyData(dg.VerticesData));

                Parallel.For(0, _VerticesData.Length, VertexID =>
                {
                    _VerticesData[VertexID] = VEL[VertexID] * 0.5f + ACC[VertexID] * 0.5f;
                });
            }*/

            if (normalized)
                _VerticesData = MatrixFunctions.normalizeData(_VerticesData);
        }
        public void setColor(Mesh3D thisPose, bool normalized)
        {
            // ...NOT WORKING...
            /*
            // Prepare colors per faces TBO
#if CPU_PARALLEL
            Parallel.For(0, thisPose.facetsCount, FacetID =>
#else
            for (int FacetID = 0; FacetID < thisPose.facetsCount; FacetID++)
#endif
            {
                _colorsFacetsData[FacetID].X = _FacetsData[FacetID];
                _colorsFacetsData[FacetID].Y = 1.0f - _FacetsData[FacetID];
                _colorsFacetsData[FacetID].Z = 0.0f;
                _colorsFacetsData[FacetID].W = 1.0f;
            }
#if CPU_PARALLEL
            );
#endif
            Buffer TBO = new Buffer(BufferTarget.TextureBuffer, BufferUsageHint.StaticDraw);
            TBO.bind();
            TBO.data<Vector4>(ref _colorsFacetsData);

            thisPose.tex_colorsFacets = new Texture(TextureTarget.TextureBuffer);
            thisPose.tex_colorsFacets.bind();
            Texture.buffer(SizedInternalFormat.Rgba32f, TBO.index);
            TBO.unbind();
            TBO.delete();
            
            */

            float[] VerticesData = new float[thisPose.verticesCount];
            if (!normalized)
                VerticesData = MatrixFunctions.normalizeData(_VerticesData);
            else
                VerticesData = _VerticesData;

#if CPU_PARALLEL
            Parallel.For(0, thisPose.verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < thisPose.verticesCount; VertexID++)
#endif
            {
                _colorsVerticesData[VertexID].X = VerticesData[VertexID];
                _colorsVerticesData[VertexID].Y = 1.0f - VerticesData[VertexID];
                _colorsVerticesData[VertexID].Z = 0.0f;
            }
#if CPU_PARALLEL
            );
#endif
            thisPose.setVerticesColor(_colorsVerticesData);
        }
        #endregion

        #region Load/Save Functions
        public bool load(String FilePath)
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
                                string[] NumNameCount = new string[2];

                                currentLine[0] = currentLine[0].Remove(0, 1);
                                NumNameCount = currentLine[0].Split('=');

                                switch (NumNameCount[0])
                                {
                                    case "NUM":                                       
                                        int VerticesCount = int.Parse(NumNameCount[1]);
                                        for (int VertexID = 0; VertexID < VerticesCount; VertexID++)
                                        {
                                            string[] ClusterLine = SReader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                            _VerticesData[VertexID] = float.Parse(ClusterLine[0]);
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
        public bool save(string name, string numOfPoses, string dgMode, string dgComponentsMode)
        {
            try
            {
                int VerticesCount = _VerticesData.Length;

                StreamWriter FileClustering = new System.IO.StreamWriter(@Properties.Settings.Default.DesktopPath + name + "(" + numOfPoses + ")^" + dgMode + "," + dgComponentsMode + ".fea");
                {
                    FileClustering.WriteLine("*NUM=" + VerticesCount.ToString());
                    for (int VertexID = 0; VertexID < VerticesCount; VertexID++)
                        FileClustering.Write(_VerticesData[VertexID].ToString() + "\n");
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
    }
}