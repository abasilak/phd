/*==========================================================================;
 *
 *  File:          LSqrDotNet.cs
 *  Version:       1.0
 *  Desc:		   C# wrapper for LSQR DLL
 *  Author:        Miha Grcar 
 *  Created on:    Oct-2007
 *  Last modified: Oct-2007
 *  License:       Common Public License (CPL)
 *  Note:
 *      See ReadMe.txt for additional info and acknowledgements.
 * 
 ***************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace LSqrDotNet
{
    /* .-----------------------------------------------------------------------
       |		 
       |  Class LSqrDll
       |
       '-----------------------------------------------------------------------
    */
    public class LSqrDll
    {
    #if DEBUG
        private const string LSQR_DLL = "LSqrDebug.dll";
    #else
        private const string LSQR_DLL = "LSqr.dll";
    #endif
        // *** external functions ***
        [DllImport(LSQR_DLL)]
        public static extern int NewMatrix();
        [DllImport(LSQR_DLL)]
        public static extern void DeleteMatrix(int id);
        [DllImport(LSQR_DLL)]
        public static extern void NewRow(int mat_id);
        [DllImport(LSQR_DLL)]
        public static extern void InsertValue(int mat_id, int idx, double val);
        [DllImport(LSQR_DLL)]
        public static extern IntPtr DoLSqr(int mat_id, int mat_transp_id, double[] rhs, int max_iter);
        // *** wrapper for external DoLSqr ***
        public static double[] DoLSqr(int num_cols, LSqrSparseMatrix mat, LSqrSparseMatrix mat_transp, double[] rhs, int max_iter)
        {
            IntPtr sol_ptr = DoLSqr(mat.Id, mat_transp.Id, rhs, max_iter);
            double[] sol = new double[num_cols];
            Marshal.Copy(sol_ptr, sol, 0, sol.Length);
            Marshal.FreeHGlobal(sol_ptr);
            return sol;
        }
    }

    /* .-----------------------------------------------------------------------
       |		 
       |  Class LSqrSparseMatrix
       |
       '-----------------------------------------------------------------------
    */
    public class LSqrSparseMatrix
    {
        private int m_id;
        public LSqrSparseMatrix()
        {
            m_id = LSqrDll.NewMatrix();
        }
        public void Dispose()
        {
            if (m_id >= 0)
            {
                LSqrDll.DeleteMatrix(m_id);
                m_id = -1;
            }
        }
        ~LSqrSparseMatrix()
        {
            Dispose();
        }
        internal int Id
        {
            get { return m_id; }
        }
        public void NewRow()
        {
            LSqrDll.NewRow(m_id);
        }
        public void InsertValue(int idx, double val)
        {
            LSqrDll.InsertValue(m_id, idx, val);
        }
        public static LSqrSparseMatrix FromDenseMatrix(double[,] mat)
        {
            LSqrSparseMatrix lsqr_mat = new LSqrSparseMatrix();
            for (int row = 0; row < mat.GetLength(0); row++)
            {
                lsqr_mat.NewRow();
                for (int col = 0; col < mat.GetLength(1); col++)
                {
                    if (mat[row, col] != 0)
                    {
                        lsqr_mat.InsertValue(col, mat[row, col]);
                    }
                }
            }
            return lsqr_mat;
        }
        public static LSqrSparseMatrix TransposeFromDenseMatrix(double[,] mat)
        {
            LSqrSparseMatrix lsqr_mat = new LSqrSparseMatrix();
            for (int col = 0; col < mat.GetLength(1); col++)
            {
                lsqr_mat.NewRow();
                for (int row = 0; row < mat.GetLength(0); row++)
                {
                    if (mat[row, col] != 0)
                    {
                        lsqr_mat.InsertValue(row, mat[row, col]);
                    }
                }
            }
            return lsqr_mat;
        }
    }
}
