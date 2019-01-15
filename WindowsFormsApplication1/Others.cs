using System.Collections;
using OpenTK;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using MathNet.Numerics.Statistics;
using System.Threading.Tasks;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace abasilak
{
    public class ColorFunctions
    {
        public static Color changeSaturation(Color color, double change)
        {
            const double Pr = 0.299;
            const double Pg = 0.587;
            const double Pb = 0.114;

            double P = Math.Sqrt(
            (color.R) * (color.R) * Pr +
            (color.G) * (color.G) * Pg +
            (color.B) * (color.B) * Pb);

            return Color.FromArgb(
                (int)(P + (color.R - P) * change),
                (int)(P + (color.G - P) * change),
                (int)(P + (color.B - P) * change));
        }

        static List<Color>  _colors;

        public static Color getColor(int colorID)
        {
            return _colors[colorID];
        }
        public static int   getColorsCount()
        {
            return _colors.Count;
        }
        public static void  addColor(Color color)
        {
            _colors.Add(color);
        }
        public static void  initColors()
        {
            _colors = new List<Color>();

            _colors.Add(Color.FromArgb(255, 255,   0));
            _colors.Add(Color.FromArgb(0  , 255,   0));
            _colors.Add(Color.FromArgb(0  , 255, 255));
            _colors.Add(Color.FromArgb(0  ,   0, 255));
            _colors.Add(Color.FromArgb(255,   0, 255));
            _colors.Add(Color.FromArgb(255, 0, 0)); // Red

            _colors.Add(Color.FromArgb(0  , 128, 128));
            _colors.Add(Color.FromArgb(0  ,   0, 128));
            _colors.Add(Color.FromArgb(128,   0, 128));
            _colors.Add(Color.FromArgb(128,   0,   0));
            _colors.Add(Color.FromArgb(128, 128,   0));
            _colors.Add(Color.FromArgb(0, 128, 0));

           // _colors.Add(Color.FromArgb(64 , 64 ,  64));
           // _colors.Add(Color.FromArgb(128, 128, 128));
           // _colors.Add(Color.FromArgb(192, 192, 192));
        }
    }

    public class GeometryFunctions
    {
        public static int calculateTriangles(int level)
        {
            if (level < 0)
                return 1;
            if (level == 0)
                return 0;
            return ((2 * level - 2) * 3) + calculateTriangles(level - 2);     // Recurse
        }

        public static double computeTriangleArea(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            double result = 0.0;
            DenseMatrix A = DenseMatrix.OfArray(new double[3,3]{{v1.X,v2.X,v3.X},{v1.Y,v2.Y,v3.Y},{1,1,1}});
            DenseMatrix B = DenseMatrix.OfArray(new double[3, 3]{{v1.Y,v2.Y,v3.Y},
            {v1.Z,v2.Z,v3.Z},{1,1,1}});
            DenseMatrix C = DenseMatrix.OfArray(new double[3, 3]{{v1.Z,v2.Z,v3.Z},
            {v1.X,v2.X,v3.X},{1,1,1}});

            result = 0.5 * Math.Sqrt(Math.Pow(A.Determinant(), 2.0) + Math.Pow(B.Determinant(), 2.0) + Math.Pow(C.Determinant(), 2.0));
            return result;
        }

        public static double computeVectorsAngle(Vector3 v1, Vector3 v2)
        {
            double Angle;
            Angle = Vector3.Dot(v1, v2) / (v1.Length * v2.Length);

            if (Angle > 1.0) Angle = 1.0;
            else if (Angle < -1.0) Angle = -1.0;

            return Math.Acos(Angle);
        }

        public static double computeVectorsAngle(Vector3d v1, Vector3d v2)
        {
            double Angle;
            Angle = Vector3d.Dot(v1, v2) / (v1.Length * v2.Length);

            if (Angle > 1.0) Angle = 1.0;
            else if (Angle < -1.0) Angle = -1.0;

            return Math.Acos(Angle);
        }
    }

    public class MatrixFunctions
    {
        public static double lengthData(double[] data)
        {
            double result = 0;
            for (int i = 0; i < data.Length; i++)
                result += Math.Pow(data[i], 2);
            return Math.Sqrt(result);
        }

        public static float [] normalizeData(float [] data)
        {
            float _minFacetData = float.MaxValue;
            float _maxFacetData = 0.0f;
            for (int i = 0; i < data.Length; i++)
            {
                if (_maxFacetData < data[i]) _maxFacetData = data[i];
                if (_minFacetData > data[i]) _minFacetData = data[i];
            }
            float _maxminFacetData = _maxFacetData - _minFacetData;

            float[] normalizedData = new float[data.Length];

#if CPU_PARALLEL
            Parallel.For(0, data.Length, i =>
#else
            for (int i = 0; i < data.Length; i++)
#endif
            {
                normalizedData[i] = _maxminFacetData == 0.0f ? 0.0f : (data[i] - _minFacetData) / _maxminFacetData;
            }
#if CPU_PARALLEL
);
#endif
            return normalizedData;
        }
        public static double[] normalizeData(double[] data)
        {
            double _minFacetData = double.MaxValue;
            double _maxFacetData = 0.0;
            for (int i = 0; i < data.Length; i++)
            {
                if (_maxFacetData < data[i]) _maxFacetData = data[i];
                if (_minFacetData > data[i]) _minFacetData = data[i];
            }
            double _maxminFacetData = _maxFacetData - _minFacetData;

            double[] normalizedData = new double[data.Length];

#if CPU_PARALLEL
            Parallel.For(0, data.Length, i =>
#else
            for (int i = 0; i < data.Length; i++)
#endif
            {
                normalizedData[i] = _maxminFacetData == 0.0 ? 0.0 : (data[i] - _minFacetData) / _maxminFacetData;
            }
#if CPU_PARALLEL
);
#endif
            return normalizedData;
        }

        public static T[] copyData<T>(T[] data) where T: struct
        {
            T[] copyData = new T[data.Length];
#if CPU_PARALLEL
            Parallel.For(0, data.Length, i =>
#else
            for (int i = 0; i < data.Length; i++)
#endif
            {
                copyData[i] = data[i];
            }
#if CPU_PARALLEL
);
#endif
            return copyData;
        }

        public static Matrix<double> calculateNipals(Matrix<double> data, int components)
        {
            //Set things up
            Matrix<double> X = data;
            double Threshold = 0.0000001;
            double TestValue;
            var E = X.Clone();
            var T = X.Column(0);
            var P = X.Column(0);
            var Temp = X.Column(0);
            Matrix Loadings = new DenseMatrix(X.ColumnCount, components);
            //Matrix Scores = new DenseMatrix(X.ColumnCount, components);
            double NewT = 0;

            //Loop over number of components
            for (int Iteration = 0; Iteration < components; Iteration++) //iterate until converged 
            {
                TestValue = Double.PositiveInfinity;
                while (TestValue > NewT * Threshold)
                {
                    //p=E't/t't
                    Temp = E.Transpose().Multiply(T);
                    Temp = Temp.Multiply(1 / T.DotProduct(T));
                    //p=p*(p'p)^-0.5
                    P = Temp.Multiply(Math.Pow(Temp.DotProduct(Temp), -0.5));
                    //get old eigenvalue
                    double oldT = T.DotProduct(T);
                    //Ep*(p'*p)
                    T = E.Multiply(P.ToColumnMatrix()).Multiply(P.DotProduct(P)).Column(0);
                    NewT = T.DotProduct(T);
                    TestValue = NewT - oldT;
                }
                //save the new component
                Loadings.SetColumn(Iteration, P);
                //Scores.SetColumn(Iteration, T);
                //deflate E=E-(t*p')
                var Deflate = T.ToColumnMatrix().Multiply(P.ToRowMatrix());
                E = E.Subtract(Deflate);
            }
            return Loadings;
        }
    }

    public class OpenTK_To_MathNET
    {
        public static void Vector3ToArray(Vector3 Vector, out int[] Array)
        {
            Array = new int[3] { (int)Vector.X, (int)Vector.Y, (int)Vector.Z };
        }

        public static void Vector4ToArray(Vector4 Vector, out int[] Array)
        {
            Array = new int[4] { (int)Vector.X, (int)Vector.Y, (int)Vector.Z, (int)Vector.W };
        }

        public static void Vector3ToArray(Vector3 Vector, out float[] Array)
        {
            Array = new float[3] { Vector.X, Vector.Y, Vector.Z };
        }

        public static void Vector4ToArray(Vector4 Vector, out float[] Array)
        {
            Array = new float[4] { Vector.X, Vector.Y, Vector.Z, Vector.W };
        }

        public static void ArrayToVector3(int[] Array, out Vector3 Vector)
        {
            Vector = new Vector3(Array[0], Array[1], Array[2]);
        }

        public static void ArrayToVector3(float[] Array, out Vector3 Vector)
        {
            Vector = new Vector3(Array[0], Array[1], Array[2]);
        }

        public static void ArrayToVector3(double[] Array, out Vector3 Vector)
        {
            Vector = new Vector3((float)Array[0], (float)Array[1], (float)Array[2]);
        }

        public static void ArrayToVector4(int[] Array, out Vector4 Vector)
        {
            Vector = new Vector4(Array[0], Array[1], Array[2], Array[3]);
        }

        public static void ArrayToVector4(float[] Array, out Vector4 Vector)
        {
            Vector = new Vector4(Array[0], Array[1], Array[2], Array[3]);
        }

        public static void ArrayToVector4(double[] Array, out Vector4 Vector)
        {
            Vector = new Vector4((float)Array[0], (float)Array[1], (float)Array[2], (float)Array[3]);
        }

        public static void ArrayToMatrix4(double[] Array, int offset, out Matrix4 matrix)
        {
            matrix = new Matrix4(
                (float)Array[offset + 0], (float)Array[offset + 1], (float)Array[offset + 2],
                (float)Array[offset + 3], (float)Array[offset + 4], (float)Array[offset + 5],
                (float)Array[offset + 6], (float)Array[offset + 7], (float)Array[offset + 8],
                (float)Array[offset + 9], (float)Array[offset + 10], (float)Array[offset + 11],
                0.0f, 0.0f, 0.0f, 1.0f);
        }

        public static DenseVector Vector3ToVector(Vector3d Vector)
        {
            return new DenseVector(new double[3] { Vector.X, Vector.Y, Vector.Z });
        }

        public static DenseVector Vector3ToVector(Vector3 Vector)
        {
            return new DenseVector(new double[3] { Vector.X, Vector.Y, Vector.Z });
        }

        public static DenseVector Vector4ToVector(Vector4d Vector)
        {
            return new DenseVector(new double[4] { Vector.X, Vector.Y, Vector.Z, Vector.W });
        }

        public static DenseVector Vector4ToVector(Vector4 Vector)
        {
            return new DenseVector(new double[4] { Vector.X, Vector.Y, Vector.Z, Vector.W });
        }

        public static Matrix4 DenseMatrixtoMatrix4(DenseMatrix matrixR, DenseVector matrixT)
        {
            Matrix4 M = new Matrix4();
            
            M.M11 = (float)matrixR.At(0, 0);
            M.M12 = (float)matrixR.At(0, 1);
            M.M13 = (float)matrixR.At(0, 2);
            M.M14 = (float)matrixT[0];
            
            M.M21 = (float)matrixR.At(1, 0);
            M.M22 = (float)matrixR.At(1, 1);
            M.M23 = (float)matrixR.At(1, 2);
            M.M24 = (float)matrixT[1];

            M.M31 = (float)matrixR.At(2, 0);
            M.M32 = (float)matrixR.At(2, 1);
            M.M33 = (float)matrixR.At(2, 2);
            M.M34 = (float)matrixT[2];

            M.M44 = 1.0f;

            return M;
        }

        public static DenseMatrix Matrix3ToDenseMatrix(Matrix3 matrix)
        {
            double[,] array = new double[3, 3];

            array[0, 0] = matrix.M11;
            array[0, 1] = matrix.M12;
            array[0, 2] = matrix.M13;

            array[1, 0] = matrix.M21;
            array[1, 1] = matrix.M22;
            array[1, 2] = matrix.M23;

            array[2, 0] = matrix.M31;
            array[2, 1] = matrix.M32;
            array[2, 2] = matrix.M33;

            return DenseMatrix.OfArray(array);
        }
        public static DenseMatrix Matrix4ToDenseMatrix(Matrix4 matrix)
        {
            double[,] array = new double[4, 4];

            array[0, 0] = matrix.M11;
            array[0, 1] = matrix.M12;
            array[0, 2] = matrix.M13;
            array[0, 3] = matrix.M14;

            array[1, 0] = matrix.M21;
            array[1, 1] = matrix.M22;
            array[1, 2] = matrix.M23;
            array[1, 3] = matrix.M24;

            array[2, 0] = matrix.M31;
            array[2, 1] = matrix.M32;
            array[2, 2] = matrix.M33;
            array[2, 3] = matrix.M34;

            array[3, 0] = matrix.M41;
            array[3, 1] = matrix.M42;
            array[3, 2] = matrix.M43;
            array[3, 3] = matrix.M44;

            return DenseMatrix.OfArray(array);
        }

        public static Quaterniond MatrixToQuaternion(DenseMatrix RotationMatrix)
        {
            Quaterniond result = Quaterniond.Identity;

            if (Math.Round(RotationMatrix.Determinant(), 12) != 1 || RotationMatrix.Trace() + 1 <= 0)
                return result;

            result.W = Math.Sqrt(1.0 + RotationMatrix[0, 0] + RotationMatrix[1, 1] + RotationMatrix[2, 2]) / 2.0;

            double w4 = 4 * result.W;
            result.X = (RotationMatrix[2, 1] - RotationMatrix[1, 2]) / w4;
            result.Y = (RotationMatrix[0, 2] - RotationMatrix[2, 0]) / w4;
            result.Z = (RotationMatrix[1, 0] - RotationMatrix[0, 1]) / w4;

            return result;
        }
    }

    public class Comparers
    {
        public class FloatKeyComparerDesc : IComparer<float>
        {
            int IComparer<float>.Compare(float x, float y)
            {
                int retVal = 0;

                if (x > y)
                    retVal = -1;
                if (x == y)
                    retVal = 0;
                if (x < y)
                    retVal = 1;

                return (retVal);
            }
        }
        public class DoubleKeyComparerDesc : IComparer<double>
        {
            int IComparer<double>.Compare(double x, double y)
            {

                int retVal = 0;

                if (x > y)
                    retVal = -1;
                if (x == y)
                    retVal = 0;
                if (x < y)
                    retVal = 1;

                return (retVal);
            }
        }
        public class DoubleKeyComparerAsc : IComparer<double>
        {
            int IComparer<double>.Compare(double x, double y)
            {

                int retVal = 0;

                if (x > y)
                    retVal = 1;
                if (x == y)
                    retVal = 0;
                if (x < y)
                    retVal = -1;

                return (retVal);
            }
        }
        public class IntKeyComparerAsc : IComparer<int>
        {
            int IComparer<int>.Compare(int x, int y)
            {

                int retVal = 0;

                if (x < y)
                    retVal = -1;
                if (x == y)
                    retVal = 0;
                if (x > y)
                    retVal = 1;

                return (retVal);
            }
        }
        public class StringKeyComparer : IComparer<string>
        {
            int IComparer<string>.Compare(string x, string y)
            {
                return (x.CompareTo(y));
            }
        }
        public class IntegerComparerAsc : IComparer
        {
            int IComparer.Compare(object item1, object item2)
            {

                int retVal = 0;
                int x = Convert.ToInt32(item1);
                int y = Convert.ToInt32(item2);

                if (x < y)
                    retVal = -1;
                if (x == y)
                    retVal = 0;
                if (x > y)
                    retVal = 1;

                return (retVal);
            }
        }
        public class StringComparer : IComparer
        {
            int IComparer.Compare(object item1, object item2)
            {
                return (item1.ToString().CompareTo(item2));
            }
        }
    }
}