
namespace abasilak
{
    public class Modes
    {
        public enum Coloring
        {
            NONE,
            VERTEX,
            FACET,
            MATERIAL,
            STRIPS,
            TEXTURE,
            NORMAL,
            DEPTH,
            LUMINANCE,
            TOON,
            XRAY,
            GOUCH,
            TEX_COORDS
        };

        public enum VertexColoring
        {
            NONE,
            RANDOM,
            GEODESIC_DISTANCES,
            DEFORMATION_GRADIENTS,
            CLUSTER,
            BONE,
            SKINNING_ERROR,
        };

        public enum DeformationGradient
        {
            REST_POSE,
            MEAN_POSE,
            POSE_TO_POSE,
        };

        public enum Weighting
        {
            RIGID,
            LBS,
        };

        public enum PeelingPerBucket
        {
            MAX,
            MEDIAN,
        };

        public enum Fitting
        {
            RP,
            MP,
            P2P_COR_COR,
            P2P_COR_APP,
            P2P_APP_APP,
            P2P_APP_APP_RPF, // P2P_RestPoseFormulation
        };

        public enum NormalApproximation
        {
            WEI_LIN_COM_INV,
            MAT_INV,
            RECOMPUTE,
        };

        public enum ClusteringVertexDistance
        {
            EUCLIDEAN,
            GEODESIC_ANGLE
        };

        public enum FittingError
        {
            MSE,
            RMSE,
            SME,
            KG,
            STED,
        };

        public enum FittingErrorVector
        {
            VERTEX,
            NORMAL,
        };

        public enum DeformationGradientComponents
        {
            VELOCITY,
            ACCELERATION,
            ROT_ANGLE,
            ROT_AXIS,
            SHEAR,
            SCALE,
            DG_FROBENIUS,
            FACET_AREA,
            ADJ_FROBENIUS
        };

        public enum Clustering
        {
            P_CENTER,
            K_MEANS,
            K_RG,
            MERGE_RG,
            DIVIDE_CONQUER,
            SPECTRAL,
            C_PCA
        };

        public enum ClusteringSpectralGraph
        {
            RANDOM_WALK,
            SYMMETRIC
        };

        public enum ClusteringSpectralDistance
        {
            NG02,
            NG02_TH07,
            NG02_DA08,
        };

        public enum ClusteringDistance
        {
            VERTEX,
            NORMAL,
            DEFORMATION_GRADIENT,
            SKINNING,
            CLUSTERING,
            MERGING,
            OVER_SEGMENTATION
        };

        public enum TexturingPar
        {
            OBJECT_PLANE,
            EYE_PLANE,
            SPHERICAL,
            REFLECTIVE,
            NORMAL
        };

        public enum TexturingApp
        {
            REPLACE,
            MODULATE,
            DECAL,
            BLEND,
            ADD
        };

        public enum Rendering
        {
            RENDER,
            PEELING,
            TRIMMING
        };

        public enum Illumination
        {
            PHONG,
            COOK
        };

        public enum Peeling
        {
//#if F2B
            F2B,
            F2B_2P,
            F2B_Z_3P,
            F2B_Z_3P_MIN_MAX,
            F2B_Z_2P,
            F2B_Z_2P_MIN_MAX,
            F2B_Z_K,
            F2B_Z_A,
            F2B_Z_LL,
//#endif
//#if DUAL
            DUAL,
            DUAL_Z_3P,
            DUAL_Z_2P,
            DUAL_Z_K,
            DUAL_Z_K_WS,
//#endif
//#if KB
            K_BUFFER,
            K_MULTI_BUFFER,
            K_MULTI_BUFFER_Z,
            K_STENCIL_BUFFER,
//#endif
//#if BUCKET
            BUCKET_UNIFORM,
            BUCKET_UNIFORM_Z,
            BUCKET_ADAPTIVE,
//#endif
//#if AB
            FREE_PIPE,
            LINKED_LISTS,
            S_BUFFER,
            PRECALC_OPENCL,
            PRECALC_FIXED,
//#endif
        };

        public enum Trimming
        {
            TRIMLESS_STATIC_F2B,
            TRIMLESS_STATIC_DUAL,
            TRIMLESS_STATIC_TWO_PASSES,
            TRIMLESS_DYNAMIC_TWO_PASSES,
            TRIMMING_STATIC,
            TRIMMING_DYNAMIC,
        };

        public enum Transparency
        {
            AVERAGE_COLORS,
            WEIGHT_SUM
        };

        public enum CSG_Operation
        {
            UNION,
            INTERSECTION,
            DIFFERENCE,
            NONE
        };
    }
}