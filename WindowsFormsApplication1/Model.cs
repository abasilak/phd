using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using System.Linq;
using MIConvexHull;

namespace abasilak
{
    public abstract class Model<T> where T : struct
    {       
        #region Private Properties

        // Buffers
        Buffer      _verticesBuffer;
        Buffer      _normalsBuffer;
        Buffer      _indicesBuffer;
        Buffer      _colorsBuffer;
        //Buffer    _textureIndicesBuffer;

        // VAO
        VertexArray _vao;
        
        #endregion

        #region Protected Properties

        // Instancing
        protected int _instancesCount = 1;

        // Shading
        protected Shading _rendering;

        //Points
        protected T[]       _verticesData;
        protected T[]       _normalsVerticesData;
        protected T[]       _colorsVerticesData;
        protected Vector2[] _texIndicesData;

        //Facets
        protected uint[]    _indicesData;

        // BeginMode
        protected PrimitiveType _primitiveMode;

        // Drawable
        protected bool _drawable = true;

        // Transformation Matrix
        protected Matrix4 _transformationMatrix = Matrix4.Identity;

        #endregion 
      
        #region Public Properties
        public T[]      verticesData
        {
            get
            {
                return _verticesData;
            }
        }
        public T[]      normalsVerticesData
        {
            get
            {
                return _normalsVerticesData;
            }
        }
        public uint[]   indicesData
        {
            get
            {
                return _indicesData;
            }
        }

        public int      verticesCount
        {
            get
            {
                return _verticesData.Length;
            }
        }
        public int      facetsCount
        {
            get
            {
                return _indicesData.Length / 3;
            }
        }
        public int      instancesCount
        {
            get
            {
                return _instancesCount;
            }
            set
            {
                _instancesCount = value;
            }
        }

        public bool     drawable
        {
            get
            {
                return _drawable;
            }
            set
            {
                _drawable = value;
            }
        }

        public Matrix4   transformation_matrix
        {
            get
            {
                return _transformationMatrix;
            }
            set
            {
                _transformationMatrix = value;
            }
        }

        public PrimitiveType primitiveMode
        {
            get
            {
                return _primitiveMode;
            }
            set
            {
                _primitiveMode = value;
            }

        }
        #endregion 

        #region Constructor
        public Model() {;}
        #endregion

        #region Delete Function
        public  virtual void delete()
        {
            if (_verticesData != null)          _verticesBuffer.delete();
            if (_normalsVerticesData != null)   _normalsBuffer.delete();
            if (_indicesData != null)           _indicesBuffer.delete();
//#if !peel
            if (_colorsVerticesData != null)    _colorsBuffer.delete();
            //if (_texIndicesData != null) _textureIndicesBuffer.delete();
//#endif
            _vao.delete();
        }
        #endregion

        #region Create Functions
        protected       void createBuffers()
        {
            if (_verticesData != null)
            {
                _verticesBuffer = new Buffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
                _verticesBuffer.bind();
                _verticesBuffer.data<T>(ref _verticesData);
            }

            if (_normalsVerticesData != null)
            {
                _normalsBuffer = new Buffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
                _normalsBuffer.bind();
                _normalsBuffer.data<T>(ref _normalsVerticesData);
            }
//#if !peel
            if (_colorsVerticesData!= null)
            {
                _colorsBuffer = new Buffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
                _colorsBuffer.bind();
                _colorsBuffer.data<T>(ref _colorsVerticesData);
            }
            //if (_texIndicesData != null)
            //{
            //    _textureIndicesBuffer = new Buffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            //    _textureIndicesBuffer.bind();
            //    _textureIndicesBuffer.data<Vector2>(ref _texIndicesData);
            //}
//#endif
            if (_indicesData != null)
            {
                _indicesBuffer = new Buffer(BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
                _indicesBuffer.bind();
                _indicesBuffer.data<uint>(ref _indicesData);
            }
        }
        public          void createVAO()
        {
            _vao = new VertexArray();
            _vao.bind();

            if (_verticesData != null)
            {
                _verticesBuffer.bind();
                _vao.enableAttrib(0);
                _vao.setAttribPointer<T>(0, VertexAttribPointerType.Float, false, IntPtr.Zero);
            }
            if (_normalsVerticesData != null)
            {
                _normalsBuffer.bind();
                _vao.enableAttrib(1);
                _vao.setAttribPointer<T>(1, VertexAttribPointerType.Float, true, IntPtr.Zero);
            }
//#if !peel
            if (_colorsVerticesData != null)
            {
                _colorsBuffer.bind();
                _vao.enableAttrib(2);
                _vao.setAttribPointer<T>(2, VertexAttribPointerType.Float, true, IntPtr.Zero);
            }
            //if (_texIndicesData != null)
            //{
            //    _textureIndicesBuffer.bind();
            //    _vao.enableAttrib(3);
            //    _vao.setAttribPointer<Vector2>(3, VertexAttribPointerType.Float, true, IntPtr.Zero);
            //}
//#endif
            _vao.unbind();
        }
        #endregion

        #region Set Vertices Color Function
        public void setVerticesColor()
        {
            _colorsBuffer = new Buffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            _colorsBuffer.bind();
            _colorsBuffer.data<T>(ref _colorsVerticesData);
            createVAO();
        }
        public void setVerticesColor(T[] colorData)
        {
            _colorsBuffer = new Buffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            _colorsBuffer.bind();
            _colorsBuffer.data<T>(ref colorData);
            createVAO();
        }
        #endregion

        #region Drawing Functions
        public abstract void draw();
        public          void drawElements()
        {
            _vao.bind();
            {
#if trim
                if (Example._scene.renderingMode == Modes.Rendering.TRIMMING &&
                    Example._scene.trimmingMode == Modes.Trimming.TRIMMING_DYNAMIC &&
                    Example._scene.multiFragmentRendering.csgOperation == Modes.CSG_Operation.NONE &&
                    Example._scene.multiFragmentRendering.passes > 0 &&
                    Example._scene.meshAnimation.selectedPose > 0)
                {
                    int M = Example._scene.multiFragmentRendering.getRestPose(Example._scene.meshAnimation.selectedPose);

                    Example._scene.meshAnimation.poses[M].verticesBuffer.bind();

                    _vao.enableAttrib(3);
                    _vao.setAttribPointer<T>(3, VertexAttribPointerType.Float, false, IntPtr.Zero);
                }
#endif
#if sma
                if (Example._scene.meshAnimation.selectedPose > -1 && Example._scene.meshAnimation.sma != null && Example._scene.meshAnimation.sma.selectedPose > -1)
                    Example._scene.meshAnimation.sma.draw(ref _vao);
#endif
                _indicesBuffer.bind();
                {
                   // GL.DrawElementsInstanced(_primitiveMode, _indicesData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero, _instancesCount);
                    GL.DrawElementsInstancedBaseVertex(_primitiveMode, _indicesData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero, _instancesCount, 0);
                }
                _indicesBuffer.unbind();
            }
            _vao.unbind();

            if (_verticesData        != null) _vao.disableAttrib(0);
            if (_normalsVerticesData != null) _vao.disableAttrib(1);
//#if !peel
            if (_colorsVerticesData  != null) _vao.disableAttrib(2);
//#endif

#if trim
            if (Example._scene.renderingMode == Modes.Rendering.TRIMMING &&
                Example._scene.trimmingMode == Modes.Trimming.TRIMMING_DYNAMIC &&
                Example._scene.multiFragmentRendering.csgOperation == Modes.CSG_Operation.NONE &&
                Example._scene.multiFragmentRendering.passes > 0 && Example._scene.meshAnimation.selectedPose > 0) _vao.disableAttrib(3);
#endif
#if sma
            if (Example._scene.meshAnimation.selectedPose > -1 && Example._scene.meshAnimation.sma != null && Example._scene.meshAnimation.sma.selectedPose > -1)
            {
                _vao.disableAttrib(4);
                _vao.disableAttrib(5);
                _vao.disableAttrib(6);
                _vao.disableAttrib(7);
               // _vao.disableAttrib(8);
            }
#endif
        }
        public          void drawElementsLite()
        {
            _vao.bind();
            {
                _indicesBuffer.bind();
                {
                    GL.DrawElementsInstancedBaseVertex(_primitiveMode, _indicesData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero, 1, 0);
                }
                _indicesBuffer.unbind();
            }
            _vao.unbind();
            _vao.disableAttrib(0);
        }
        #endregion
    }

    public class Image : Model<Vector2>
    {
        #region Static Readonly Properties
        static readonly Vector2[]   vertices = new Vector2[]{
            new Vector2( 0.0f, 0.0f ),
            new Vector2( 1.0f, 0.0f ),
            new Vector2( 1.0f, 1.0f ),
            new Vector2( 0.0f, 1.0f )};
        static readonly uint[]      indices = new uint[] { 0, 1, 2, 2, 3, 0 };
        #endregion

        #region Constructor
        public Image()
        {
            _primitiveMode= PrimitiveType.Triangles;
            _verticesData = vertices;
            _indicesData  = indices;
            createBuffers();
            createVAO();
        }
        #endregion 

        #region Drawing Functions
        public override void draw()
        {
            drawElementsLite();
        }
        #endregion
    }

    public class AABB : Model<Vector3>
    {
        #region Private Properties
        /* For Line drawing
        readonly uint[] indices = new uint[] { 
            0,1,1,2,2,3,3,0,
            4,5,5,6,6,7,7,4,
            1,5,2,6,0,4,3,7
        };
        */
        readonly uint[] indices = new uint[] { 
        0,1,2,
        2,3,0, 
        3,2,6, 
        6,7,3, 
        7,6,5, 
        5,4,7, 
        4,0,3,
        3,7,4,
        0,4,5,
        5,1,0,
        1,5,6,
        6,2,1
        };

        float _diagonal;
        #endregion

        #region Public Properties
        public float diagonal
        {
            get { return _diagonal; }
        }
        #endregion

        #region Constructor
        public AABB(Vector3 centerV, Vector3 minV, Vector3 maxV, Shading rendering)
        {
            _drawable = false;           
            _rendering      = rendering;
            _primitiveMode  = PrimitiveType.Triangles;

            _diagonal = (maxV - minV).Length;
            Vector3 R = (maxV - minV) * 0.5f;
            Vector3[] Vertices = new Vector3[]
            {
                centerV + new Vector3(-R.X,-R.Y, R.Z),
                centerV + new Vector3( R.X,-R.Y, R.Z),
                centerV + new Vector3( R.X, R.Y, R.Z),
                centerV + new Vector3(-R.X, R.Y, R.Z),
                centerV + new Vector3(-R.X,-R.Y,-R.Z),
                centerV + new Vector3( R.X,-R.Y,-R.Z),
                centerV + new Vector3( R.X, R.Y,-R.Z),
                centerV + new Vector3(-R.X, R.Y,-R.Z)
            };

            _verticesData   = Vertices;
            _indicesData    = indices;

            createBuffers();
            createVAO();
        }
        #endregion

        #region Drawing Functions
        public override void draw()
        {
            if (drawable)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                _rendering.use();
                {
                    _rendering.bindUniformMatrix4("transformation_matrix", false, ref _transformationMatrix);
                    drawElementsLite();
                }
                Shading.close();
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
        }
        #endregion
    }

    public class CH : Model<Vector3>
    {
        #region Private Properties

        //uint[] indices;

        #endregion

        #region Constructor
        public CH(Vector3[] vertices, Shading rendering)
        {
            _drawable = false;
            _rendering = rendering;
            _primitiveMode = PrimitiveType.Triangles;

            Vertex[] verticesMesh = new Vertex[vertices.Length];
#if CPU_PARALLEL
            Parallel.For(0, vertices.Length, VertexID =>
#else
            for(int VertexID = 0; VertexID < vertices.Length; VertexID++)
#endif
            {
                verticesMesh[VertexID] = new Vertex(vertices[VertexID].X, vertices[VertexID].Y, vertices[VertexID].Z);
            }
#if CPU_PARALLEL
);
#endif          
            // Compute Convex Hull
            var convexHullOutput   = ConvexHull.Create<Vertex, Face>(verticesMesh);
            var convexHullVertices = convexHullOutput.Points.ToList();
            var convexHullFacets   = convexHullOutput.Faces.ToList();

            _verticesData = new Vector3[convexHullVertices.Count];
#if CPU_PARALLEL
            Parallel.For(0, convexHullVertices.Count, VertexID =>
#else
            for (int VertexID = 0; VertexID < convexHullVertices.Count; VertexID++)
#endif
            {
                _verticesData[VertexID] = new Vector3(
                                        (float)(convexHullVertices[VertexID].Position[0]),
                                        (float)(convexHullVertices[VertexID].Position[1]),
                                        (float)(convexHullVertices[VertexID].Position[2]));
            }
#if CPU_PARALLEL
);
#endif          
            _indicesData = new uint [convexHullFacets.Count*3];
#if CPU_PARALLEL
            Parallel.For(0, convexHullFacets.Count, FacetID =>
#else
            for (int FacetID = 0; FacetID < convexHullFacets.Count; FacetID++)
#endif
            {
                var facet = convexHullFacets[FacetID];
                _indicesData[3 * FacetID    ] = (uint)convexHullVertices.IndexOf(facet.Vertices[0]);
                _indicesData[3 * FacetID + 1] = (uint)convexHullVertices.IndexOf(facet.Vertices[1]);
                _indicesData[3 * FacetID + 2] = (uint)convexHullVertices.IndexOf(facet.Vertices[2]);
            }
#if CPU_PARALLEL
);
#endif          
    
            createBuffers();
            createVAO();
        }
        #endregion

        #region Drawing Functions
        public override void draw()
        {
            if (drawable)
            {
                _rendering.use();
                {
                    _rendering.bindUniformMatrix4("transformation_matrix", false, ref _transformationMatrix);
                    drawElementsLite();
                }
                Shading.close();
            }
        }
        #endregion
    }

    public class Sphere : Model<Vector3>
    {
        #region Private Properties

        int     _colorID;
        Color   _color;
        Vector3 _center;

        Random  _randNum;

        static readonly uint[] _indices = new uint[] { 
        0,1,2,
        2,3,0, 
        3,2,6, 
        6,7,3, 
        7,6,5, 
        5,4,7, 
        4,0,3,
        3,7,4,
        0,4,5,
        5,1,0,
        1,5,6,
        6,2,1
        };
        #endregion

        #region Public Properties
        public int   colorID
        {
            get { return _colorID; }
            set { _colorID = value; }
        }
        public Color color
        {
            get { return _color; }
            set { _color = value; }
        }
        #endregion

        #region Constructor
        public Sphere(Vector3 center, float radius, int id)
        {
            _randNum        = new Random();
            _rendering      = Example._scene.sphereRendering;
            _drawable       = false;
            _primitiveMode  = PrimitiveType.Triangles;           
            _indicesData    = _indices;
            _colorID        = -1;
            _color          = Color.White;

            setCenter(center, radius);
        }
        #endregion

        #region Set Center
        public void setCenter(Vector3 center, float radius)
        {
            _center = center;

            Vector3     Radius = new Vector3(radius);
            Vector3[]   Vertices = new Vector3[]
            {
                _center + new Vector3(-Radius.X,-Radius.Y, Radius.Z),
                _center + new Vector3( Radius.X,-Radius.Y, Radius.Z),
                _center + new Vector3( Radius.X, Radius.Y, Radius.Z),
                _center + new Vector3(-Radius.X, Radius.Y, Radius.Z),
                _center + new Vector3(-Radius.X,-Radius.Y,-Radius.Z),
                _center + new Vector3( Radius.X,-Radius.Y,-Radius.Z),
                _center + new Vector3( Radius.X, Radius.Y,-Radius.Z),
                _center + new Vector3(-Radius.X, Radius.Y,-Radius.Z)
            };

            _verticesData = Vertices;
        }
        #endregion

        #region Set Buffers
        public void setBuffers()
        {
            createBuffers();
            createVAO();
        }
        #endregion

        #region Set Color
        // HSV values in [0..1] returns [r, g, b] values from 0 to 255
        private Color hsv_to_rgb(double h, double s, double v)
        {
            int    h_i = (int)(h*6);
            double f = h*6 - h_i;
            double p = v * (1 - s);
            double q = v * (1 - f * s);
            double t = v * (1 - (1 - f) * s);

            double R=0, G=0, B=0;
            if (h_i == 0) R = v; G = t; B = p;
            if (h_i == 1) R = q; G = v; B = p;
            if (h_i == 2) R = p; G = v; B = t;
            if (h_i == 3) R = p; G = q; B = v;
            if (h_i == 4) R = t; G = p; B = v;
            if (h_i == 5) R = v; G = p; B = q;

            return Color.FromArgb((int)(R * 256), (int)(G * 256), (int)(B * 256));
        }
        public void setRandomColor(int numOfSpheres, int id)
        {
            int C = 16777215 / numOfSpheres;
            int RGB_Bone1 = C * (id);
            int RGB_Bone2 = C * (id + 1);
            int RGB_Bone  = _randNum.Next(RGB_Bone1, RGB_Bone2);
            //int RGB_Bone  = (RGB_Bone1 + RGB_Bone2)/2;

            int R = ((RGB_Bone >> 16) & 0x0ff);
            int G = ((RGB_Bone >> 8) & 0x0ff);
            int B = ((RGB_Bone) & 0x0ff);

            _color = Color.FromArgb(R, G, B);
        }
        public void setFixedColor(int cID)
        {
            _color   = ColorFunctions.getColor(cID);
            _colorID = cID;
        }
        #endregion

        #region Drawing Functions
        public override void draw()
        {
            _rendering.use();
            {
                _rendering.bindUniform4("color", _color);
                _rendering.bindUniformMatrix4("transformation_matrix", false, ref _transformationMatrix);
                drawElementsLite();
            }
            Shading.close();
        }
        #endregion
    }

    public class Mesh3D : Model<Vector3>
    {
        #region Private Properties

        // MultiFragment Rendering
        int         _maxLayers;
        Query []    _samplesAnyQuery = new Query[2];

        // AABB
        AABB _aabb;
        // Convex Hull
        CH   _convexHull;

        // Faces per Vertex
        List<int>[] _facetsVerticesData;

        // Neighbors per Vertex
        List<int>[] _neighborsVerticesData;
        List<int>[] _neighborsFacetsData;

        // Neighbors per Facet
        List<int>[] _neighborsFacetsFacetsData;
        
        // Geodesic Distances per Vertex
        double[,]   _distancesGeodesic;
        SortedDictionary<double, List<int>> [] _distancesQueue;

        // Normals/Centers
        Vector3 [] _centersFacetData;
        Vector3 [] _normalsFacetsData;
        Vector4 [] _colorsFacetsData;
        double  [] _areasFacetData;
        double _averageAreasFacet;

        // Deformation Gradients
        DeformationGradient [] _dg = new DeformationGradient[3];

        // Clustering
        Clustering []    _clusteringMethods = new Clustering[7];
        Modes.Clustering _clusteringMode;
        Cluster[]        _clustersVerticesData;
        Cluster[]        _clustersFacetsData;

        // Properties
        string  _name;
        int     _poseID;
        bool    _changed;
        bool    _wireframe;
        // PrimitiveID
        int     _init_PrimitiveID;

        float _radius;
        float _volume;
        float _translation_factor;
        
        Vector3 _center;
        Vector3 _min, _max;

        Modes.Coloring      _coloringMode;
        Modes.TexturingApp  _texturingApp;
        Modes.TexturingPar  _texturingPar;

        // Transparency Shading
        bool _transparent;
        bool _translucent;
        float _transparency;

        // Strips Shading
        bool    _stripXY;
        float   _stripSize;
        Color   _stripColor;

        // Xray Shading
        float _edgeFalloff;

        // Gouch Shading
        float _diffuseWarm;
        float _diffuseCool;
               
        // Texture
        Texture _texture;
        Texture _tex_colorsFacets;

        // Material
        Material _material;

        // Thickness
        Translucency _thickness;

        #endregion

        #region Public Properties

        public int maxLayers
        {
            get
            {
                return _maxLayers;
            }
            set
            {
                _maxLayers = value;
            }
        }
        public Query [] samplesAnyQuery
        {
            get
            {
                return _samplesAnyQuery;
            }
            set
            {
                _samplesAnyQuery = value;
            }
        }

        public DeformationGradient [] dg
        {
            get { return _dg; }
        }
        public DeformationGradient dgRP
        {
            get { return _dg[0]; }
            set { _dg[0] = value; }
        }
        public DeformationGradient dgMP
        {
            get { return _dg[1]; }
            set { _dg[1] = value; }
        }
        public DeformationGradient dgP2P
        {
            get { return _dg[2]; }
            set { _dg[2] = value; }
        }
        public P_Center_Clustering pCenter
        {
            get { return (P_Center_Clustering)_clusteringMethods[0]; }
        }
        public K_Means_Clustering kMeans
        {
            get { return (K_Means_Clustering)_clusteringMethods[1]; }
        }
        public K_RG_Clustering kRG
        {
            get { return (K_RG_Clustering)_clusteringMethods[2]; }
        }
        public Merge_RG_Clustering mergeRG
        {
            get { return (Merge_RG_Clustering)_clusteringMethods[3]; }
        }
        public Divide_Conquer_Clustering dConquer
        {
            get { return (Divide_Conquer_Clustering)_clusteringMethods[4]; }
        }
        public K_Spectral_Clustering kSpectral
        {
            get { return (K_Spectral_Clustering)_clusteringMethods[5]; }
        }
        public C_PCA_Clustering cPCA
        {
            get { return (C_PCA_Clustering)_clusteringMethods[6]; }
        }
        public Clustering [] clusteringMethods
        {
            get { return _clusteringMethods; }
        }
        public Clustering clusteringMethod
        {
            get { return _clusteringMethods[(int)_clusteringMode]; }
        }
        public Modes.Clustering clusteringMode
        {
            get { return _clusteringMode; }
            set { _clusteringMode = value; }
        }

        public Vector3  [] centersFacetData
        {
            get { return _centersFacetData; }
        }
        public double[] areasFacetData
        {
            get { return _areasFacetData; }
        }
        public double averageAreasFacet
        {
            get
            {
                return _averageAreasFacet;
            }
            set
            {
                _averageAreasFacet = value;
            }
        }
        public Cluster[] clustersFacetsData
        {
            get { return _clustersFacetsData; }
        }
        public Cluster[] clustersVerticesData
        {
            get { return _clustersVerticesData; }
        }
        public List<int>[] facetsVerticesData
        {
            get { return _facetsVerticesData; }
        }
        public List<int>[] neighborsFacetsData
        {
            get { return _neighborsFacetsData; }
        }
        public List<int>[] neighborsVerticesData
        {
            get { return _neighborsVerticesData; }
        }
        public List<int>[] neighborsFacetsFacetsData
        {
            get { return _neighborsFacetsFacetsData; }
        }

        public AABB aabb
        {
            get { return _aabb; }
            set { _aabb = value; }
        }

        public CH convexHull
        {
            get { return _convexHull; }
            set { _convexHull = value; }
        }

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }

        }
        public bool changed
        {
            get { return _changed; }
            set { _changed = value; }
        }
        public bool wireframe
        {
            get { return _wireframe; }
            set { _wireframe = value; }
        }
        
        public float stripSize
        {
            get
            {
                return _stripSize;
            }
            set
            {
                _stripSize = value;
            }
        }
        public Color stripColor
        {
            get
            {
                return _stripColor;
            }
            set
            {
                _stripColor = value;
            }

        }
        public bool stripXY
        {
            get
            {
                return _stripXY;
            }
            set
            {
                _stripXY = value;
            }
        }
        
        public float edgeFalloff
        {
            get
            {
                return _edgeFalloff;
            }
            set
            {
                _edgeFalloff = value;
            }
        }

        public float diffuseWarm
        {
            get
            {
                return _diffuseWarm;
            }
            set
            {
                _diffuseWarm = value;
            }
        }
        public float diffuseCool
        {
            get
            {
                return _diffuseCool;
            }
            set
            {
                _diffuseCool = value;
            }
        }

        public Modes.Coloring coloringMode
        {
            get
            {
                return _coloringMode;
            }
            set
            {
                _coloringMode = value;
            }
        }
        public Modes.TexturingApp texturingApp
        {
            get
            {
                return _texturingApp;
            }
            set
            {
                _texturingApp = value;
            }
        }
        public Modes.TexturingPar texturingPar
        {
            get
            {
                return _texturingPar;
            }
            set
            {
                _texturingPar = value;
            }
        }

        public float volume
        {
            get
            {
                return _volume;
            }
        }
        public float radius
        {
            get { return _radius; }
        }
        public float translation_factor
        {
            get { return _translation_factor; }
            set { _translation_factor = value; }
        }

        public float transparency
        {
            get { return _transparency; }
            set { _transparency = value; }
        }
        public Material material
        {
            get { return _material; }
            set { _material = value; }
        }
        public Texture texture
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
            }
        }
        public Texture tex_colorsFacets
        {
            get { return _tex_colorsFacets; }
            set { _tex_colorsFacets = value; }
        }
        public Translucency thickness
        {
            get
            {
                return _thickness;
            }
            set
            {
                _thickness = value;
            }
        }

        public bool transparent
        {
            get
            {
                return _transparent;
            }
            set
            {
                _transparent = value;
            }
        }
        public bool translucent
        {
            get
            {
                return _translucent;
            }
            set
            {
                _translucent = value;
            }
        }

        public int poseID
        {
            get
            {
                return _poseID;
            }
            set
            {
                _poseID = value;
            }
        }
        public int init_PrimitiveID
        {
            get
            {
                return _init_PrimitiveID;
            }
            set
            {
                _init_PrimitiveID = value;
            }
        }

        public Vector3 max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }
        }
        public Vector3 min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
            }
        }
        public Vector3 center
        {
            get
            {
                return _center;
            }
            set
            {
                _center = value;
            }
        }

        #endregion

        #region Constructor
        public Mesh3D(Vector3[] vertices, Vector3[] normals, Vector3[] colors, uint[] indices)
        {
            _wireframe          = false;
            _primitiveMode      = PrimitiveType.Triangles;
#if peel
            _coloringMode       = Modes.Coloring.MATERIAL;
           // _thickness          = new Translucency();
#else
            _coloringMode       = Modes.Coloring.VERTEX;
#endif
            // Transparency Shading
            _transparent    = false;
            _translucent    = false;
            _transparency   = 0.3f;
            // Strips Shading
            _stripSize      = 1.0f;
            _stripXY        = false;
            _stripColor     = Color.SpringGreen;
            // Xray Shading
            _edgeFalloff = 1.0f;
            // Gooch Shading
            _diffuseWarm = 0.45f;
            _diffuseCool = 0.45f;
            // Texture
            _texturingPar = Modes.TexturingPar.REFLECTIVE;
            _texturingApp = Modes.TexturingApp.MODULATE;

            _verticesData        = vertices;
            _indicesData         = indices;
            _normalsVerticesData = normals;

            _material               = new Material();
            _transformationMatrix   = Matrix4.Identity;

            calculateVerticesColors(0);
                                 
            createBuffers();
            createVAO();
        }
        public Mesh3D(int poseID, string name, Vector3[] vertices, Vector3[] normals, Vector3[] colors, Vector2[] tex_indices, uint[] indices, List<int>[] neighborsVertices, List<int>[] neighborsFacets)
        {
            _name = name;
            _wireframe = false;
            _primitiveMode = (Example._scene.tessellation) ? PrimitiveType.Patches : PrimitiveType.Triangles;
#if peel
            _coloringMode = Modes.Coloring.MATERIAL;
            //_thickness          = new Translucency();
#else
            _coloringMode = Modes.Coloring.VERTEX;
#endif
            // Transparency Shading
            _transparent = false;
            _translucent = false;
            _transparency = 0.3f;
            // Strips Shading
            _stripSize = 1.0f;
            _stripXY = false;
            _stripColor = Color.SpringGreen;
            // Xray Shading
            _edgeFalloff = 1.0f;
            // Gooch Shading
            _diffuseWarm = 0.45f;
            _diffuseCool = 0.45f;
            // Texture
            _texturingPar = Modes.TexturingPar.REFLECTIVE;
            _texturingApp = Modes.TexturingApp.MODULATE;

            _verticesData = vertices;
            _texIndicesData = tex_indices;
            _indicesData = indices;
            _neighborsVerticesData = neighborsVertices;
            _neighborsFacetsData = neighborsFacets;

            _material = new Material();
            _transformationMatrix = Matrix4.Identity;

            // Clustering
#if !peel    
            _clusteringMethods[0] = new P_Center_Clustering();
            _clusteringMethods[1] = new K_Means_Clustering();
            _clusteringMethods[2] = new K_RG_Clustering();
            _clusteringMethods[3] = new Merge_RG_Clustering();
            _clusteringMethods[4] = new Divide_Conquer_Clustering();
            _clusteringMethods[5] = new K_Spectral_Clustering();
            _clusteringMethods[6] = new C_PCA_Clustering();

            _clusteringMode = Modes.Clustering.P_CENTER;
            _clustersVerticesData = new Cluster[verticesCount];
            _clustersFacetsData = new Cluster[facetsCount];
#endif
            // Boundaries
            calculateMinMaxCenter();

            // Normals
            if (Example._scene.meshAnimation.recomputeNormals || normals.Length == 0 || vertices.Length != normals.Length)
            {
                calculateFacetsNormals();
                calculateVerticesNormals();
            }
            else
                _normalsVerticesData = normals;

            // For AdjDeformationGradient
            {
                // Facet Centers
                // calculateFacetsCenters();
                // Neighbors Facets-Facets
                // calculateNeighborFacetsFacets();
            }

            // Areas
            calculateFacetsAreas();
            // Colors
#if peel            
            calculateFacetsColors();
#endif
            if (Example._scene.meshAnimation.recomputeNormals)
                calculateVerticesColors(poseID);
            else
                _colorsVerticesData = colors;
            // Volume
            calculateVolume();

            // Facets-Vertices
            calculateFacetsPerVertex();

            // AABB
            _aabb = new AABB(_center, _min, _max, Example._scene.aabb_chRendering);

            // Convex Hull
            _convexHull = new CH(_verticesData, Example._scene.aabb_chRendering);

#if peel
            // Prepare colors per faces TBO
            //Buffer TBO = new Buffer(BufferTarget.TextureBuffer, BufferUsageHint.StaticDraw);
            //TBO.bind();
            //TBO.data<Vector4>(ref _colorsFacetsData);

            //_tex_colorsFacets = new Texture(TextureTarget.TextureBuffer);
            //_tex_colorsFacets.bind();
            //Texture.buffer(SizedInternalFormat.Rgba32f, TBO.index);
            //TBO.unbind();
            //TBO.delete();

            // Occlusion Query
            _samplesAnyQuery[0] = new Query(QueryTarget.AnySamplesPassed);
            _samplesAnyQuery[1] = new Query(QueryTarget.AnySamplesPassed);
#endif
            createBuffers();
            createVAO();
        }
        #endregion

        #region Copy Functions
        public Mesh3D ShallowCopy()
        {
            return (Mesh3D)this.MemberwiseClone();
        }
        #endregion

        #region Delete Function
        public new void delete()
        {
            base.delete();
            
            _aabb.delete();
            _convexHull.delete();
            _material.delete();
#if peel
            //_thickness.delete();
            //_tex_colorsFacets.delete();
            _samplesAnyQuery[0].delete();
            _samplesAnyQuery[1].delete();
#endif
        }
        #endregion

        #region Update Shaders Functions
        public void updateShaders(ref Shading rendering, bool illumination)
        {
            if (illumination)
            {
                // Material
                _material.update();
                rendering.bindBuffer(2, "Material", _material.buffer.index, BufferRangeTarget.UniformBuffer);
                // Strips
                rendering.bindUniform1("stripXY", _stripXY ? 1 : 0);
                rendering.bindUniform1("stripSize", _stripSize);
                rendering.bindUniform4("stripColor", _stripColor);
                // Xray Edge Falloff
                rendering.bindUniform1("edgeFalloff", _edgeFalloff);
                // Gouch Diffuse 
                rendering.bindUniform1("diffuseWarm", _diffuseWarm);
                rendering.bindUniform1("diffuseCool", _diffuseCool);
                // Texturing
                rendering.bindUniform1("TexturingPar", (int)_texturingPar);
                rendering.bindUniform1("TexturingApp", (int)_texturingApp);

                rendering.bindUniform1("ColoringMode", (int)_coloringMode);
                rendering.bindUniform1("useTransparency", _transparent ? 1 : 0);
                rendering.bindUniform1("useTranslucency", _translucent ? 1 : 0);
                rendering.bindUniform1("transparency", _transparency);
            }
            // Transforming
            Matrix4 tr = _transformationMatrix;
            rendering.bindUniformMatrix4("transformation_matrix", false, ref tr);

            // Primitive ID
            rendering.bindUniform1("init_PrimitiveID", _init_PrimitiveID);
            rendering.bindUniform1("instancesCount"  , _instancesCount);

            //Instancing
            rendering.bindUniform1("Instancing"          , Example._scene.instancing ? 1 : 0);
            rendering.bindUniform3("instance_translation", Example._scene.instance_translation * _radius);
        }
        #endregion

        #region Drawing Functions
        public override void draw()
        {
            // Coloring Mode
            if (_coloringMode == Modes.Coloring.FACET || _coloringMode == Modes.Coloring.TEXTURE)
            {
                Texture.active(TextureUnit.Texture2);
#if peel
                if (_coloringMode == Modes.Coloring.FACET)
                    _tex_colorsFacets.bind();
                else
#endif
                {
                    if (_texture != null)
                    {
                        _texture.bind();
                        Texture.sampler.bind(2);
                    }
                }
            }
#if peel
            // Translucency Mode
            if (Example._scene.multiFragmentRendering.translucent && _translucent && Example._scene.renderingMode == Modes.Rendering.RENDER)
            {
                Texture.active(TextureUnit.Texture3);
                /*if (Example._scene.peelingMode == Modes.Peeling.F2B || Example._scene.peelingMode == Modes.Peeling.F2B_2P || Example._scene.peelingMode == Modes.Peeling.F2B_Z_2P || Example._scene.peelingMode == Modes.Peeling.F2B_Z_3P || Example._scene.peelingMode == Modes.Peeling.F2B_Z_3P_MIN_MAX || Example._scene.peelingMode == Modes.Peeling.F2B_Z_K || Example._scene.peelingMode == Modes.Peeling.F2B_Z_2P_MIN_MAX || Example._scene.peelingMode == Modes.Peeling.F2B_Z_A || Example._scene.peelingMode == Modes.Peeling.F2B_Z_LL)
                    _thickness.tex_f2b.bind();
                else if (Example._scene.peelingMode == Modes.Peeling.DUAL || Example._scene.peelingMode == Modes.Peeling.DUAL_Z_2P || Example._scene.peelingMode == Modes.Peeling.DUAL_Z_3P || Example._scene.peelingMode == Modes.Peeling.DUAL_Z_K || Example._scene.peelingMode == Modes.Peeling.DUAL_Z_K_WS)
                    _thickness.tex_dual.bind();
                else if (Example._scene.peelingMode == Modes.Peeling.K_BUFFER || Example._scene.peelingMode == Modes.Peeling.K_MULTI_BUFFER || Example._scene.peelingMode == Modes.Peeling.K_MULTI_BUFFER_Z || Example._scene.peelingMode == Modes.Peeling.K_STENCIL_BUFFER || Example._scene.peelingMode == Modes.Peeling.BUCKET_UNIFORM)
                    _thickness.tex_k_buffer.bind();
                else if (Example._scene.peelingMode == Modes.Peeling.FREE_PIPE || Example._scene.peelingMode == Modes.Peeling.LINKED_LISTS)
                    ;
                else
                    _thickness.tex.bind();
                */
                _thickness.tex.bind();
            }
#endif
            drawElements();
        }
        #endregion

        #region Calculate Functions
        public void calculateMinMaxCenter()
        {
            _min    = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            _max    = new Vector3(0.0f, 0.0f, 0.0f);
            for (int i = 0; i < verticesCount; i++)
            {
                if (_verticesData[i].X < _min.X) _min.X = _verticesData[i].X;
                if (_verticesData[i].Y < _min.Y) _min.Y = _verticesData[i].Y;
                if (_verticesData[i].Z < _min.Z) _min.Z = _verticesData[i].Z;

                if (_verticesData[i].X > _max.X) _max.X = _verticesData[i].X;
                if (_verticesData[i].Y > _max.Y) _max.Y = _verticesData[i].Y;
                if (_verticesData[i].Z > _max.Z) _max.Z = _verticesData[i].Z;
            }

            _center = (_min + _max)*0.5f;
            Vector3 R = (_max - _min) * 0.5f;
            _radius = Math.Max(R.X, Math.Max(R.Y, R.Z));
            _translation_factor = 0.5f;
        }
        public void calculateVerticesColors(int poseID)
        {
            Vector3 color;
            if (!Example._scene.meshAnimation.ignoreGroups)
            {
                Random RandNum = new Random(poseID);
                Color c = Color.FromArgb(RandNum.Next());
                color.X = c.R / 255.0f;
                color.Y = c.G / 255.0f;
                color.Z = c.B / 255.0f;
            }
            else
                color = new Vector3(0.41f);

            _colorsVerticesData = new Vector3[verticesCount];
#if CPU_PARALLEL
            Parallel.For(0, verticesCount, VertexID =>
#else
            for(int VertexID=0; VertexID<verticesCount; VertexID++)
#endif
            {
                _colorsVerticesData[VertexID] = new Vector3(color);
            }
#if CPU_PARALLEL
);
#endif
        }
        public void calculateFacetsColors()
        {
            Random RandNum = new Random();

            _colorsFacetsData = new Vector4[facetsCount];
#if CPU_PARALLEL
            Parallel.For(0, facetsCount, FacetID =>
#else
            for(int FacetID=0; FacetID<facetsCount; FacetID++)
#endif
            {
                Color c = Color.FromArgb(RandNum.Next());
                _colorsFacetsData[FacetID].X=c.R/255.0f;
                _colorsFacetsData[FacetID].Y=c.G/255.0f;
                _colorsFacetsData[FacetID].Z=c.B/255.0f;
                _colorsFacetsData[FacetID].W=1.0f;
            }
#if CPU_PARALLEL
            );
#endif
        }
        public void calculateFacetsCenters()
        {
            _centersFacetData = new Vector3[facetsCount];
#if CPU_PARALLEL
            Parallel.For(0, facetsCount, FacetID =>
#else
            for(int FacetID=0; FacetID<facetsCount; FacetID++)
#endif
            {
                Vector3 v0, v1, v2;
                v0 = _verticesData[_indicesData[3 * FacetID]];
                v1 = _verticesData[_indicesData[3 * FacetID + 1]];
                v2 = _verticesData[_indicesData[3 * FacetID + 2]];

                _centersFacetData[FacetID] = (v0 + v1 + v2) / 3.0f;
            }
#if CPU_PARALLEL
);
#endif        
        }
        public void calculateFacetsAreas()
        {
            _areasFacetData = new double[facetsCount];
#if CPU_PARALLEL
            Parallel.For(0, facetsCount, FacetID =>
#else
            for(int FacetID=0; FacetID<facetsCount; FacetID++)
#endif
            {
                Vector3 V0, V1, V2;
                V0 = _verticesData[_indicesData[3 * FacetID]];
                V1 = _verticesData[_indicesData[3 * FacetID + 1]];
                V2 = _verticesData[_indicesData[3 * FacetID + 2]];

                _areasFacetData[FacetID] = GeometryFunctions.computeTriangleArea(V0, V1, V2);
            }
#if CPU_PARALLEL
);
#endif
            if (facetsCount>0)
                _averageAreasFacet = areasFacetData.Average();
        }
        public void calculateFacetsNormals()
        {
            _normalsFacetsData = new Vector3[facetsCount];

#if CPU_PARALLEL
            Parallel.For(0, facetsCount, FacetID =>
#else
            for(int FacetID=0; FacetID<facetsCount; FacetID++)
#endif
            {
                Vector3 v0, v1, v2;
                v0 = _verticesData[_indicesData[3*FacetID]];
                v1 = _verticesData[_indicesData[3*FacetID+1]];
                v2 = _verticesData[_indicesData[3*FacetID+2]];

                _normalsFacetsData[FacetID]=Vector3.Cross(Vector3.Subtract(v1, v0), Vector3.Subtract(v2, v0));
                _normalsFacetsData[FacetID].Normalize();
            }
#if CPU_PARALLEL
            );
#endif
		}
        public void calculateVerticesNormals()
        {
            _normalsVerticesData = new Vector3[verticesCount];

#if CPU_PARALLEL
            Parallel.For(0, verticesCount, VertexID =>
#else
            for(int VertexID=0; VertexID<verticesCount; VertexID++)
#endif
            {
                float   weight;
                Vector3 v0, v10, v20;

                v0 = _verticesData[VertexID];

                for(int NeighborID=0; NeighborID<_neighborsFacetsData[VertexID].Count; NeighborID++)
                {
                    int k;
                    int o = 3*_neighborsFacetsData[VertexID][NeighborID];

                    for(k=0; k<3; k++)
                        if(VertexID==_indicesData[o+k])
                            break;
                    v10 = _verticesData[_indicesData[o+(k+1)%3]] - v0;
                    v20 = _verticesData[_indicesData[o+(k+2)%3]] - v0;
                    weight = (float) Math.Acos(Vector3.Dot(v10, v20) / (v10.Length * v20.Length));
                    _normalsVerticesData[VertexID]+=weight*_normalsFacetsData[_neighborsFacetsData[VertexID][NeighborID]];
                }
                _normalsVerticesData[VertexID].Normalize();
            }
#if CPU_PARALLEL
            );
#endif
        }
		public void calculateVolume()
        {
#if CPU_PARALLEL
            object lockObject = new object();

            Parallel.For<float>(0, facetsCount, () => 0.0f, (FacetID, loop, partialResult) =>
            {
                Vector3 v0, v1, v2;
                v0 = _verticesData[_indicesData[3 * FacetID]];
                v1 = _verticesData[_indicesData[3 * FacetID + 1]];
                v2 = _verticesData[_indicesData[3 * FacetID + 2]];
                partialResult += Math.Abs(Vector3.Dot(v0, Vector3.Cross(v1, v2)));

                return partialResult;
            },
                (localPartialSum) =>
                {
                    // Enforce serial access to single, shared result
                    lock (lockObject)
                    {
                        _volume += localPartialSum;
                    }
                }
            );
#else
            Vector3 v0, v1, v2;

            _volume=0.0f;
            for(int FacetID=0; FacetID<facetsCount; FacetID++)
            {
                v0=_verticesData[_indicesData[3*FacetID]];
                v1=_verticesData[_indicesData[3*FacetID+1]];
                v2=_verticesData[_indicesData[3*FacetID+2]];
				_volume += Math.Abs(Vector3.Dot(v0,Vector3.Cross(v1,v2)));
			}
#endif
            _volume /= 6.0f;
        }
        public void calculateNeighborFacetsFacets()
        {
            _neighborsFacetsFacetsData = new List<int> [facetsCount];

#if CPU_PARALLEL
            Parallel.For(0, facetsCount, FacetID =>
#else
            for(int FacetID=0; FacetID<facetsCount; FacetID++)
#endif
            {
                _neighborsFacetsFacetsData[FacetID] = new List<int>();

                for(int VertexID=0; VertexID<3; VertexID++)
                {
                    uint V = _indicesData[3 * FacetID + VertexID];
                    for (int NeighborID = 0; NeighborID < _neighborsFacetsData[V].Count; NeighborID++)
                    {
                        int Neighbor = _neighborsFacetsData[V][NeighborID];
                        if (Neighbor != FacetID && !_neighborsFacetsFacetsData[FacetID].Contains(Neighbor))
                            _neighborsFacetsFacetsData[FacetID].Add(Neighbor);
                    }
                }               
            }
#if CPU_PARALLEL
);
#endif
        }
        public void calculateFacetsPerVertex()
        {
            _facetsVerticesData = new List<int>[verticesCount];
            for(int FacetID=0; FacetID<facetsCount; FacetID++)
                for (int VertexIndex = 0; VertexIndex < 3; VertexIndex++)
                {
                    int VertexID = (int)_indicesData[3 * FacetID + VertexIndex];

                    if( _facetsVerticesData[VertexID] == null)
                        _facetsVerticesData[VertexID] = new List<int>();
                    _facetsVerticesData[VertexID].Add(FacetID);
                }
        }
        public void calculateVectorsGeodesicDistances()
        {
            //Dijkstra’s Algorithm - Finds the shortest path between v1 and v2 in the pose

            _distancesQueue     = new SortedDictionary<double, List<int>>[verticesCount];
            _distancesGeodesic  = new double[verticesCount, verticesCount];

#if CPU_PARALLEL
            Parallel.For(0, verticesCount, VertexStart =>
#else 
            for (int VertexStart = 0; VertexStart < verticesCount; VertexStart++)
#endif
            {
                // 1.1
                _distancesQueue[VertexStart] = new SortedDictionary<double, List<int>>();
                addElement(VertexStart, VertexStart, 0.0);

                // 1.2           
                bool[] Used = new bool[verticesCount];
                for (int VertexID = 0; VertexID < verticesCount; VertexID++)
                    Used[VertexID] = false;

                // 2.
                int TotalUsed = 0;
                while (_distancesQueue[VertexStart].Count > 0 && TotalUsed < verticesCount)
                {
                    KeyValuePair<double, List<int>> GeodesicPopPair = getMinimumDistance(VertexStart);
                    double      GeodesicDistance = GeodesicPopPair.Key;
                    List<int>   GeodesicElements = GeodesicPopPair.Value;

                    while (GeodesicElements.Count > 0)
                    {
                        int VertexID = GeodesicElements[0];
                        GeodesicElements.RemoveAt(0);

                        if (!Used[VertexID])
                        {
                            TotalUsed++;
                            Used[VertexID] = true;
                            _distancesGeodesic[VertexStart, VertexID] = GeodesicDistance;
                            
                            foreach (int NeighborID in _neighborsVerticesData[VertexID])
                                if (!Used[NeighborID])
                                {
                                    double EuclideanDistance = (_verticesData[VertexID] - _verticesData[NeighborID]).Length;
                                    double Distance = GeodesicDistance + EuclideanDistance;

                                    addElement(VertexStart, NeighborID, Distance);
                                }
                        }
                    }
                    _distancesQueue[VertexStart].Remove(GeodesicDistance);
                }
            }
#if CPU_PARALLEL
);
#endif
        }
        #endregion

        #region Translate Function
        public void translate(Keys keys)
        {
            Matrix4 TranslationMatrix = Matrix4.Identity;
            float   TranslationFactor = _radius * _translation_factor;

            if      (keys == Keys.A) TranslationMatrix = Matrix4.CreateTranslation(TranslationFactor, 0.0f, 0.0f);
            else if (keys == Keys.D) TranslationMatrix = Matrix4.CreateTranslation(-TranslationFactor, 0.0f, 0.0f);
            else if (keys == Keys.Q) TranslationMatrix = Matrix4.CreateTranslation(0.0f, TranslationFactor, 0.0f);
            else if (keys == Keys.E) TranslationMatrix = Matrix4.CreateTranslation(0.0f, -TranslationFactor, 0.0f);
            else if (keys == Keys.W) TranslationMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, TranslationFactor);
            else if (keys == Keys.S) TranslationMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, -TranslationFactor);

            _transformationMatrix       *= TranslationMatrix;
            _aabb.transformation_matrix  = _transformationMatrix;
            _convexHull.transformation_matrix = _transformationMatrix;
        }
        #endregion

        #region Deformation Gradient Functions
        public void setDeformationGradients(Modes.DeformationGradient dgMode, Modes.DeformationGradientComponents dgComponentsMode, bool dgPerPose, DeformationGradient[] masDG, bool dgNormalize)
        {
            DeformationGradient DG = _dg[(int)dgMode];

            if (dgPerPose) DG.setVertexData(this, dgComponentsMode, dgNormalize);
            else           DG.setVertexData(this, masDG[(int)dgMode], dgComponentsMode, dgNormalize);
            DG.setColor(this, dgNormalize);
        }
        #endregion

        #region Euclidean-Geodesic Distance Functions

        #region Queue Functions

        #region Get Functions
        public List<int> getList(int vertexStart, double distance)
        {
            List<int> Result;
            _distancesQueue[vertexStart].TryGetValue(distance, out Result);
            return Result;
        }
        public KeyValuePair<double, List<int>> getMinimumDistance(int vertexStart)
        {
            foreach (KeyValuePair<double, List<int>> Pivot in _distancesQueue[vertexStart])
                return Pivot;
            return new KeyValuePair<double, List<int>>();
        }
        #endregion
  
        #region Add Elements Function
        public void addElement(int vertexStart, int vertexID, double distance)
        {
            //Check if a node with the same distance already exists
            List<int> tList = getList(vertexStart, distance);

            if (tList == null)//If not create the list and add to the new node
            {
                tList = new List<int>();
                tList.Add(vertexID);
                _distancesQueue[vertexStart].Add(distance, tList);
            }
            else//If yes add the node to the list of the existing
            {
                //But first check to see if we are attempting to insert the same element
                if (!tList.Contains(vertexID))
                    tList.Add(vertexID);
            }
        }
        #endregion

        #endregion

        #region Compute Function
        public double computeVectorsEuclideanDistance(int vertexID_1, int vertexID_2)
        {
            return (double)(_verticesData[vertexID_1] - _verticesData[vertexID_2]).Length;
        }
        public double computeNormalsEuclideanDistance(int vertexID_1, int vertexID_2)
        {
            return (double)(_normalsVerticesData[vertexID_1] - _normalsVerticesData[vertexID_2]).Length;
        }
        public double computeVectorsGeodesicDistance (int vertexID_1, int vertexID_2)
        {
            return _distancesGeodesic[vertexID_1,vertexID_2];
        }
        #endregion

        #region Get Vertices Color Function
        public Vector3[] getGeodesicDistancesVerticesColor(int selectedVertex)
        {
            double [] Distances          = new double[verticesCount];
            Vector3[] ColorsVerticesData = new Vector3[verticesCount];

            if (selectedVertex == -1)
            {
                if (_distancesGeodesic != null)
                {
#if CPU_PARALLEL
                    Parallel.For(0, verticesCount, VertexID_I =>
#else
                    for (int VertexID_I = 0; VertexID_I < verticesCount; VertexID_I++)
#endif
                    {
                        Distances[VertexID_I] = 0.0;
                        for (int VertexID_J = 0; VertexID_J < verticesCount; VertexID_J++)
                            Distances[VertexID_I] += _distancesGeodesic[VertexID_I, VertexID_J];
                        Distances[VertexID_I] /= verticesCount;
                    }
#if CPU_PARALLEL
);
#endif
                    Distances = MatrixFunctions.normalizeData(Distances);
                }
#if CPU_PARALLEL
                Parallel.For(0, verticesCount, VertexID =>
#else
                for (int VertexID = 0; VertexID < verticesCount; VertexID++)
#endif
                {
                    ColorsVerticesData[VertexID].X = (float)Distances[VertexID];
                    ColorsVerticesData[VertexID].Y = 1.0f - (float)Distances[VertexID];
                    ColorsVerticesData[VertexID].Z = 0.0f;
                }
#if CPU_PARALLEL
);
#endif
            }
            else
            {
                if (_distancesGeodesic != null)
                {
#if CPU_PARALLEL
                    Parallel.For(0, verticesCount, VertexID =>
#else
                    for (int VertexID = 0; VertexID < verticesCount; VertexID++)
#endif
                    {
                        Distances[VertexID] = _distancesGeodesic[selectedVertex, VertexID];
                    }
#if CPU_PARALLEL
);
#endif
                    Distances = MatrixFunctions.normalizeData(Distances);
                }
                
#if CPU_PARALLEL
                Parallel.For(0, verticesCount, VertexID =>
#else
                for (int VertexID = 0; VertexID < verticesCount; VertexID++)
#endif
                {
                    if (VertexID == selectedVertex)
                        ColorsVerticesData[VertexID] = new Vector3(1.0f);
                    else
                    {
                        ColorsVerticesData[VertexID].X = (float)Distances[VertexID];
                        ColorsVerticesData[VertexID].Y = 1.0f - (float)Distances[VertexID];
                        ColorsVerticesData[VertexID].Z = 0.0f;
                    }
                }
#if CPU_PARALLEL
);
#endif
            }
            return ColorsVerticesData;
        }
        #endregion

        #endregion

        #region Compute Triangle Elements per Cluster
        public void computeTriangleElements(bool merged)
        {
            List<Cluster> Clusters = (merged) ? clusteringMethod.clustersMerged : clusteringMethod.clusters;
#if CPU_PARALLEL
            Parallel.ForEach(Clusters, ClusterC =>
#else
            foreach (Cluster ClusterC in Clusters)
#endif
            {
                ClusterC.area = 0.0;
                ClusterC.elementsF.Clear();
            }
#if CPU_PARALLEL
);
#endif
            for (int FacetID = 0; FacetID < facetsCount; FacetID++)
            {
                uint [] Indices = new uint[3];
                for (int I = 0; I < 3; I++)
                    Indices[I] = _indicesData[3 * FacetID + I];

                if (_clustersVerticesData[Indices[0]].id == _clustersVerticesData[Indices[1]].id && _clustersVerticesData[Indices[1]].id == _clustersVerticesData[Indices[2]].id)
                {
                    _clustersVerticesData[Indices[0]].addElementF(FacetID);
                    _clustersVerticesData[Indices[0]].area += _areasFacetData[FacetID];
                    _clustersFacetsData[FacetID] = _clustersVerticesData[Indices[0]];
                }
            }

            // dEN DOUlevei !!@$!@$#!$
            Parallel.ForEach(Clusters, ClusterC =>
            {
                if (ClusterC.area <= 0.0)
                    ClusterC.area = _averageAreasFacet * ClusterC.elementsV.Count;
            });
        }
        #endregion

        #region Propagate Clustering Colors
        public void propagateColoring(Mesh3D prevPose, List<Cluster> ClustersP, List<Cluster> ClustersT, bool merged, bool twoRingColoring)
        {
            // Compute Covering Areas
            foreach (Cluster ClusterT in ClustersT)
            {
                double TotalCount = 0;
                List<int>       ClustersP_ID    = new List<int>();
                List<double>    ClustersP_Area  = new List<double>();
                // Facets
                if (ClusterT.elementsF.Count > 0)
                {
                    foreach (int FacetID in ClusterT.elementsF)
                    {
                        Cluster ClusterP = prevPose.clustersFacetsData[FacetID];
                        if (ClusterP != null)
                        {
                            // if belong to a removed cluster... ??? (to be removed)
                            //if (ClusterP.id >= ClustersP.Count)
                              //  continue;

                            int Index = ClustersP_ID.IndexOf(ClusterP.id);
                            if (Index == -1)
                            {
                                ClustersP_ID.Add(ClusterP.id);
                                ClustersP_Area.Add(_areasFacetData[FacetID]);
                            }
                            else
                                ClustersP_Area[Index] += _areasFacetData[FacetID];
                            TotalCount += 1;
                        }
                    }

                   for (int ClusterID = 0; ClusterID < ClustersP_ID.Count; ClusterID++)
                       ClusterT.addElementArea(ClustersP_ID[ClusterID], ClustersP_Area[ClusterID]);
                }
                // Vertices
                if (TotalCount == 0)
                {
                    foreach (int ElementID in ClusterT.elementsV)
                    {
                        int Index = ClustersP_ID.IndexOf(prevPose.clustersVerticesData[ElementID].id);
                        if (Index == -1)
                        {
                            ClustersP_ID.Add(prevPose.clustersVerticesData[ElementID].id);
                            ClustersP_Area.Add(0.0000001);
                        }
                        else
                            ClustersP_Area[Index] += 0.0000001;
                    }

                    for (int ClusterID = 0; ClusterID < ClustersP_ID.Count; ClusterID++)
                        ClusterT.addElementArea(ClustersP_ID[ClusterID], ClustersP_Area[ClusterID]);
                }
            }

            // BFS Search-Coloring
            int ClustersID = 0;
            bool[] Used = new bool[ClustersT.Count];
            Parallel.For(0, ClustersT.Count, ClusterID => { Used[ClusterID] = false; });
            List<Cluster> ClustersSortedFinal = new List<Cluster>();

            while (ClustersSortedFinal.Count < ClustersT.Count)
            {
                if (ClustersID == ClustersSortedFinal.Count)
                {
                    int    BestClusterMaxAreaID =  -1;
                    double BestClusterMaxArea   = 0.0;
                    foreach (Cluster ClusterT in ClustersT)
                    {
                        if (Used[ClusterT.id])
                            continue;

                        ClusterT.sphere.colorID = -1;
                        double ClusterMaxArea = ClusterT.getMaxClusterArea();

                        if (ClusterMaxArea > BestClusterMaxArea)
                        {
                            BestClusterMaxArea   = ClusterMaxArea;
                            BestClusterMaxAreaID = ClusterT.id;
                        }
                    }
                    Used[BestClusterMaxAreaID] = true;
                    ClustersSortedFinal.Add(ClustersT[BestClusterMaxAreaID]);
                }

                Cluster ClusterC = ClustersSortedFinal[ClustersID++];

                List<Cluster> ClustersSortedN = new List<Cluster>();
                foreach (Cluster ClusterN in ClusterC.neighbors)
                    if (!Used[ClusterN.id])
                        ClustersSortedN.Add(ClusterN);

                ClustersSortedN.Sort(delegate(Cluster c1, Cluster c2)
                {
                    return  (c1.getMaxClusterArea()).CompareTo(c2.getMaxClusterArea());
                });
                ClustersSortedN.Reverse();

                foreach (Cluster ClusterN in ClustersSortedN)
                {
                    Used[ClusterN.id] = true;
                    ClustersSortedFinal.Add(ClusterN);
                }
            }

            // Chech if cluster can take the color from one of its covering clustersP
            List<Cluster>     ClustersNoMapping = new List<Cluster>();
  
            foreach (Cluster ClusterS in ClustersSortedFinal)
            {
                // Find Neighbor Colors
                List<Color> NeighborColor = new List<Color>();
                foreach (Cluster ClusterN in ClusterS.neighbors)
                {
                    if (ClusterN.sphere.colorID != -1 && !NeighborColor.Contains(ClusterN.sphere.color))
                        NeighborColor.Add(ClusterN.sphere.color);

                    if(twoRingColoring)
                        foreach (Cluster ClusterN_N in ClusterN.neighbors)
                            if (ClusterN_N.sphere.colorID != -1 && !NeighborColor.Contains(ClusterN_N.sphere.color))
                                NeighborColor.Add(ClusterN_N.sphere.color);
                }
                
                // Search if cluster can take a color from its covering clusters from previous frame
                bool ColorFound=false;
                foreach (KeyValuePair<double, List<int>> AreasS in ClusterS.areas)
                {
                    foreach (int ClusterS_ID in AreasS.Value)
                    {
                        ColorFound = true;
                        foreach (Cluster ClusterS_N in ClusterS.neighbors)
                            if (ClusterS_N.sphere.colorID == -1)
                            {
                                foreach (KeyValuePair<double, List<int>> AreasS_N in ClusterS_N.areas)
                                {
                                    foreach (int ClusterS_N_ID in AreasS_N.Value)
                                    {
                                        if (ClusterS_N_ID == ClusterS_ID && AreasS.Key < AreasS_N.Key)
                                        {
                                            ColorFound = false;
                                            break;
                                        }
                                    }
                                    break;
                                }
                                if (!ColorFound)
                                    break;
                            }

                        Cluster ClusterP = ClustersP[ClusterS_ID];
                        if (ColorFound && !NeighborColor.Contains(ClusterP.sphere.color))
                        {
                            ClusterS.sphere.color   = ClusterP.sphere.color;
                            ClusterS.sphere.colorID = ClusterP.sphere.colorID;
                            break;
                        }
                    }
                    if (ClusterS.sphere.colorID != -1)
                        break;
                }

                if (ClusterS.sphere.colorID != -1)
                    continue;
                // else Create Weighted Color

                double WeightC   = 0.5;
                double WeightC_1 = 1.0 - WeightC;
                if (ClusterS.areas.Count == 1)
                {
/*                  if (merged && ClusterS.neighbors.Count > 1 )
                    {
                        Color ColorNew = Color.FromArgb(
                            (int)((double)ClustersP[ClusterS.getMaxClusterAreaID()].sphere.color.R * WeightC + (double)ClusterS.sphere.color.R * (1.0 - WeightC)),
                            (int)((double)ClustersP[ClusterS.getMaxClusterAreaID()].sphere.color.G * WeightC + (double)ClusterS.sphere.color.G * (1.0 - WeightC)),
                            (int)((double)ClustersP[ClusterS.getMaxClusterAreaID()].sphere.color.B * WeightC + (double)ClusterS.sphere.color.B * (1.0 - WeightC)));

                        ClusterS.sphere.color = ColorNew;
                        ClusterS.sphere.colorID = ColorFunctions.getColorsCount();
                        ColorFunctions.addColor(ColorNew);
                    }
                    else
  */                
                    ClustersNoMapping.Add(ClusterS);
                }
                else
                {
                    Color ColorNew = Color.FromArgb(
                        (int)((double)ClustersP[ClusterS.getMaxClusterAreaID()].sphere.color.R * WeightC + (double)ClustersP[ClusterS.getMaxClusterAreaNextID()].sphere.color.R * WeightC_1),
                        (int)((double)ClustersP[ClusterS.getMaxClusterAreaID()].sphere.color.G * WeightC + (double)ClustersP[ClusterS.getMaxClusterAreaNextID()].sphere.color.G * WeightC_1),
                        (int)((double)ClustersP[ClusterS.getMaxClusterAreaID()].sphere.color.B * WeightC + (double)ClustersP[ClusterS.getMaxClusterAreaNextID()].sphere.color.B * WeightC_1));

                    ClusterS.sphere.color   = ColorNew;
                    ClusterS.sphere.colorID = ColorFunctions.getColorsCount();
                    ColorFunctions.addColor(ColorNew);
                }
            }

            // No mapped Clusters
            int MaxColorIDs = ColorFunctions.getColorsCount(); // or = 12
            foreach (Cluster ClusterS in ClustersNoMapping)
            {
                List<Color> NeighborColor = new List<Color>();
                foreach (Cluster ClusterN in ClusterS.neighbors)
                {
                    if (ClusterN.sphere.colorID != -1 && !NeighborColor.Contains(ClusterN.sphere.color))
                        NeighborColor.Add(ClusterN.sphere.color);

                    if (twoRingColoring)
                        foreach (Cluster ClusterN_N in ClusterN.neighbors)
                            if (ClusterN_N.sphere.colorID != -1 && !NeighborColor.Contains(ClusterN_N.sphere.color))
                                NeighborColor.Add(ClusterN_N.sphere.color);
                }

                int C_ID = (ClustersP[ClusterS.getMaxClusterAreaID()].sphere.colorID + 1 >= MaxColorIDs) ? 0 : ClustersP[ClusterS.getMaxClusterAreaID()].sphere.colorID + 1;
                for (int ColorID = C_ID; ColorID < MaxColorIDs; ColorID++)
                    if (!NeighborColor.Contains(ColorFunctions.getColor(ColorID)) && (C_ID + 2 != ColorID))
                    {
                        ClusterS.sphere.setFixedColor(ColorID);
                        break;
                    }
            }

            ColorFunctions.initColors();
        }
        #endregion 

        #region Compute Error Functions
        public double computeErrorFromVertex(Mesh3DAnimationSequence mas, int vertexID_1, int vertexID_2)
        {
            return (mas.clusteringVertexDistanceMode == Modes.ClusteringVertexDistance.EUCLIDEAN) ? 
                computeVectorsEuclideanDistance(vertexID_1, vertexID_2) :
                computeVectorsGeodesicDistance(vertexID_1, vertexID_2);
        }
        public double computeErrorFromNormal(Mesh3DAnimationSequence mas, int vertexID_1, int vertexID_2)
        {
            return (mas.clusteringVertexDistanceMode == Modes.ClusteringVertexDistance.EUCLIDEAN) ?
                computeNormalsEuclideanDistance(vertexID_1, vertexID_2) :
                GeometryFunctions.computeVectorsAngle(_normalsVerticesData[vertexID_1], _normalsVerticesData[vertexID_2]);
        }
        public double computeErrorFromDG    (Mesh3DAnimationSequence mas, int vertexID_1, int vertexID_2)
        {
            DeformationGradient DG;
            if      (mas.dgMode == Modes.DeformationGradient.REST_POSE) DG = dgRP;
            else if (mas.dgMode == Modes.DeformationGradient.MEAN_POSE) DG = dgMP;
            else                                                        DG = dgP2P;

            if (mas.clusteringVertexDistanceMode == Modes.ClusteringVertexDistance.EUCLIDEAN)
                return Math.Abs((double)DG.VerticesData[vertexID_1] - (double)DG.VerticesData[vertexID_2]);
                //return (double)DG.verticesData[vertexID_1] - (double)DG.verticesData[vertexID_2];
            else
            {
                // ** Hasler2010 **
                /*
                DenseMatrix RotationalMatrix_1 = new DenseMatrix(3, 3, 0);
                foreach (int NeighborID in _neighborsFacetsData[vertexID_1])
                {
                    var DG_SVD_1 = DG.DG[NeighborID].Svd(true);
                    RotationalMatrix_1 += (DenseMatrix)DG_SVD_1.U().Multiply(DG_SVD_1.VT());
                    break;
                }
                //RotationalMatrix_1 = (DenseMatrix)RotationalMatrix_1.Divide((double)pose.neighborsFacetsData[vertexID_1].Count);

                DenseMatrix RotationalMatrix_2 = new DenseMatrix(3, 3, 0);
                foreach (int NeighborID in _neighborsFacetsData[vertexID_2])
                {
                    var DG_SVD_2 = DG.DG[NeighborID].Svd(true);
                    RotationalMatrix_2 += (DenseMatrix)DG_SVD_2.U().Multiply(DG_SVD_2.VT());
                    break;
                }
                //RotationalMatrix_2 = (DenseMatrix)RotationalMatrix_2.Divide((double)pose.neighborsFacetsData[vertexID_2].Count);

                Quaterniond RotationQ_1 = OpenTK_To_MathNET.MatrixToQuaternion(RotationalMatrix_1);
                Quaterniond RotationQ_2 = OpenTK_To_MathNET.MatrixToQuaternion(RotationalMatrix_2);
                Quaterniond RotationQ_1_INV = Quaterniond.Invert(RotationQ_1);
                Quaterniond RotationQ = Quaterniond.Multiply(RotationQ_1_INV, RotationQ_2);
                
                Vector3d Axis; double Angle;
                RotationQ.ToAxisAngle(out Axis, out Angle);

                return Angle;
                 * */
                return 0.0;
            }
        }
        #endregion

        #region Translucency Functions
        public void initTranslucency()
        {
            if (!(_drawable && _translucent))
                return;
            /*
            if      (Example._scene.peelingMode == Modes.Peeling.F2B || Example._scene.peelingMode == Modes.Peeling.F2B_2P || Example._scene.peelingMode == Modes.Peeling.F2B_Z_2P || Example._scene.peelingMode == Modes.Peeling.F2B_Z_3P || Example._scene.peelingMode == Modes.Peeling.F2B_Z_3P_MIN_MAX || Example._scene.peelingMode == Modes.Peeling.F2B_Z_K || Example._scene.peelingMode == Modes.Peeling.F2B_Z_2P_MIN_MAX || Example._scene.peelingMode == Modes.Peeling.F2B_Z_A || Example._scene.peelingMode == Modes.Peeling.F2B_Z_LL)
                _thickness.initF2B();
            else if (Example._scene.peelingMode == Modes.Peeling.DUAL || Example._scene.peelingMode == Modes.Peeling.DUAL_Z_2P || Example._scene.peelingMode == Modes.Peeling.DUAL_Z_3P || Example._scene.peelingMode == Modes.Peeling.DUAL_Z_K || Example._scene.peelingMode == Modes.Peeling.DUAL_Z_K_WS)
                _thickness.initDUAL();
            else if (Example._scene.peelingMode == Modes.Peeling.K_BUFFER ||  Example._scene.peelingMode == Modes.Peeling.K_MULTI_BUFFER || Example._scene.peelingMode == Modes.Peeling.K_MULTI_BUFFER_Z || Example._scene.peelingMode == Modes.Peeling.K_STENCIL_BUFFER)
                _thickness.initKB();
            */
            _thickness.initDUAL();
        }
        public void computeTranslucency()
        {
            if (_drawable && _translucent)
                _thickness.compute();
        }
        public void computeF2BTranslucency(ref Texture tex_depth, ref Texture tex_color)
        {
            if (_drawable && _translucent)
                _thickness.computeF2B(ref tex_depth, ref tex_color);
        }
        public void computeDUALTranslucency(ref Texture tex_depth)
        {
            if (_drawable && _translucent)
                _thickness.computeDUAL(ref tex_depth);
        }
        #endregion
    }

    public class Mesh3DAnimationSequence
    {
        #region Private Properties

        // MultiFragment Rendering
        int          _maxLayers;

        // Model Sequence
        int          _facetsCount;
        int          _verticesCount;
        List<Mesh3D> _poses = new List<Mesh3D>();
       
        // Mean Pose
        Mesh3D _meanPose;
        // Edited Pose
        Mesh3D _editedPose;

        // Skinning Mesh Animation Data
        SMA _sma;

        // Primitive ID
        int _max_PrimitiveID;

        // Spatial attributes
        Vector3 _center;
        Vector3 _min, _max;

        // Mesh Loading Properties
        bool _rotation90;
        bool _ignoreGroups;
        bool _recomputeNormals;

        // Selection Properties
        bool _addMeanPoseToTree;
        bool _selectedPoseDrawable;
        int  _selectedPose;
        int  _selectedRestPose;
        int  _selectedVertex;

        PolygonMode  _polygonMode;
        MaterialFace _materialFace;

        Modes.VertexColoring _vColoringMode;

        // Deformation Gradient Properties
        bool                                _dgPerPose;
        bool                                _dgNormalize;
        bool                                _dgVariability;
        int                                 _dgVariabilityEigenCount;
        DeformationGradient []              _dg = new DeformationGradient[3];
        Modes.DeformationGradient           _dgMode;
        Modes.DeformationGradientComponents _dgComponentsMode;

        // Clustering
        bool                                _clusteringClean;
        bool                                _clusteringPerPose;
        bool                                _clusteringPerPoseMerging;
        bool                                _clustering2RingColoring;
        bool                                _clusteringIncremental;
        Clustering[]                        _clusteringMethods = new Clustering[7];
        Modes.Clustering                    _clusteringMode;
        Modes.ClusteringDistance            _clusteringDistanceMode;
        Modes.ClusteringVertexDistance      _clusteringVertexDistanceMode;
        Modes.ClusteringSpectralGraph       _clusteringSpectralGraphMode;
        Modes.ClusteringSpectralDistance    _clusteringSpectralDistanceMode;

        #endregion

        #region Public Properties

        public SMA sma
        {
            get { return _sma; }
            set { _sma = value; }
        }

        public Mesh3D pose
        {
            get
            {
                return _poses[_selectedPose];
            }
        }
        public Mesh3D meanPose
        {
            get { return _meanPose; }
        }
        public Mesh3D restPose
        {
            get
            {
                return _poses[_selectedRestPose];
            }
        }
        public Mesh3D editedPose
        {
            get { return _editedPose; }
        }
        public List<Mesh3D> poses
        {
            get
            {
                return _poses;
            }
        }

        public bool dgPerPose
        {
            get
            {
                return _dgPerPose;
            }
            set
            {
                _dgPerPose = value;
            }
        }
        public bool dgNormalize
        {
            get
            {
                return _dgNormalize;
            }
            set
            {
                _dgNormalize = value;
            }
        }
        public bool dgVariability
        {
            get
            {
                return _dgVariability;
            }
            set
            {
                _dgVariability = value;
            }
        }
        public int  dgVariabilityEigenCount
        {
            get
            {
                return _dgVariabilityEigenCount;
            }
            set
            {
                _dgVariabilityEigenCount = value;
            }
        }
        public DeformationGradient [] dg
        {
            get
            {
                return _dg;
            }
        }
        public DeformationGradient dgRP
        {
            get
            {
                return _dg[0];
            }
            set
            {
                _dg[0] = value;
            } 
        }
        public DeformationGradient dgMP
        {
            get
            {
                return _dg[1];
            }
            set
            {
                _dg[1] = value;
            }
        }
        public DeformationGradient dgP2P
        {
            get
            {
                return _dg[2];
            }
            set
            {
                _dg[2] = value;
            }
        }
        public Modes.DeformationGradient dgMode
        {
            get
            {
                return _dgMode;
            }
            set
            {
                _dgMode = value;
            }
        }
        public Modes.DeformationGradientComponents dgComponentsMode
        {
            get
            {
                return _dgComponentsMode;
            }
            set
            {
                _dgComponentsMode = value;
            }
        }

        public bool clusteringPerPose
        {
            get
            {
                return _clusteringPerPose;
            }
            set
            {
                _clusteringPerPose = value;
            }
        }
        public bool clusteringPerPoseMerging
        {
            get
            {
                return _clusteringPerPoseMerging;
            }
            set
            {
                _clusteringPerPoseMerging = value;
            }
        }
        public bool clustering2RingColoring
        {
            get
            {
                return _clustering2RingColoring;
            }
            set
            {
                _clustering2RingColoring = value;
            }
        }

        public bool clusteringClean
        {
            get
            {
                return _clusteringClean;
            }
            set
            {
                _clusteringClean = value;
            }
        }
        public bool clusteringIncremental
        {
            get
            {
                return _clusteringIncremental;
            }
            set
            {
                _clusteringIncremental = value;
            }
        }
        public P_Center_Clustering pCenter
        {
            get { return (P_Center_Clustering)_clusteringMethods[0]; }
        }
        public K_Means_Clustering kMeans
        {
            get { return (K_Means_Clustering)_clusteringMethods[1]; }
        }
        public C_PCA_Clustering cPCA
        {
            get { return (C_PCA_Clustering)_clusteringMethods[6]; }
        }
        public K_RG_Clustering kRG
        {
            get { return (K_RG_Clustering)_clusteringMethods[2]; }
        }
        public Merge_RG_Clustering mergeRG
        {
            get { return (Merge_RG_Clustering)_clusteringMethods[3]; }
        }
        public Divide_Conquer_Clustering dConquer
        {
            get { return (Divide_Conquer_Clustering)_clusteringMethods[4]; }
        }
        public K_Spectral_Clustering kSpectral
        {
            get { return (K_Spectral_Clustering)_clusteringMethods[5]; }
        }
        public Clustering clusteringMethod
        {
            get { return _clusteringMethods[(int)_clusteringMode]; }
        }
        public Clustering[] clusteringMethods
        {
            get { return _clusteringMethods; }
        }
        public Modes.Clustering clusteringMode
        {
            get { return _clusteringMode; }
            set { _clusteringMode = value; }
        }
        public Modes.ClusteringSpectralDistance clusteringSpectralDistanceMode
        {
            get { return _clusteringSpectralDistanceMode; }
            set { _clusteringSpectralDistanceMode = value; }
        }
        public Modes.ClusteringSpectralGraph clusteringSpectralGraphMode
        {
            get { return _clusteringSpectralGraphMode; }
            set { _clusteringSpectralGraphMode = value; }
        }
        public Modes.ClusteringVertexDistance clusteringVertexDistanceMode
        {
            get { return _clusteringVertexDistanceMode; }
            set { _clusteringVertexDistanceMode = value; }
        }
        public Modes.ClusteringDistance clusteringDistanceMode
        {
            get { return _clusteringDistanceMode; }
            set { _clusteringDistanceMode = value; }
        }

        public int max_PrimitiveID
        {
            get
            {
                return _max_PrimitiveID;
            }
            set
            {
                _max_PrimitiveID = value;
            }
        }
        public int selectedRestPose
        {
            get
            {
                return _selectedRestPose;
            }
            set
            {
                _selectedRestPose = value;
            }
        }
        public int selectedPose
        {
            get
            {
                return _selectedPose;
            }
            set
            {
                _selectedPose = value;
            }
        }
        public int selectedVertex
        {
            get
            {
                return _selectedVertex;
            }
            set
            {
                _selectedVertex = value;
            }
        }
        public int verticesCount
        {
            get
            {
                return _verticesCount;
            }
        }
        public int facetsCount
        {
            get
            {
                return _facetsCount;
            }
        }  
        public bool selectedPoseDrawable
        {
            get
            {
                return _selectedPoseDrawable;
            }
            set
            {
                _selectedPoseDrawable = value;
            }
        }
        public bool addMeanPoseToTree
        {
            get { return _addMeanPoseToTree; } 
            set { _addMeanPoseToTree = value; }
        }
        public bool rotation90
        {
            get
            {
                return _rotation90;
            }
            set
            {
                _rotation90 = value;
            }
        }
        public bool recomputeNormals
        {
            get
            {
                return _recomputeNormals;
            }
            set
            {
                _recomputeNormals = value;
            }
        }
        public bool ignoreGroups
        {
            get
            {
                return _ignoreGroups;
            }
            set
            {
                _ignoreGroups = value;
            }
        }
        public Vector3 max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }
        }
        public Vector3 min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
            }
        }
        public Vector3 center
        {
            get
            {
                return _center;
            }
            set
            {
                _center = value;
            }
        }
        public MaterialFace materialFace
        {
            get
            {
                return _materialFace;
            }
            set
            {
                _materialFace = value;
            }
        }
        public PolygonMode polygonMode
        {
            get
            {
                return _polygonMode;
            }
            set
            {
                _polygonMode = value;
            }
        }
        public Modes.VertexColoring vColoringMode
        {
            get { return _vColoringMode; }
            set { _vColoringMode = value; }
        }
        public int maxLayers
        {
            get
            {
                return _maxLayers;
            }
            set
            {
                _maxLayers = value;
            }
        }
        #endregion

        #region Constructor
        public Mesh3DAnimationSequence()
        {
            _ignoreGroups           = true;
            _rotation90             = false;
            _recomputeNormals       = false;
            _selectedPoseDrawable   = true;
            _addMeanPoseToTree      = false;

            _max_PrimitiveID = 0;

            _min    = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            _max    = Vector3.Zero;
            _center = Vector3.Zero;

            _selectedPose       = -1;
            _selectedRestPose   =  0;
            _selectedVertex     = -1;

            _dgPerPose = false;
            _dgNormalize = true;
            _dgVariability = false;
            _dgVariabilityEigenCount = 1;
            _dgMode = Modes.DeformationGradient.REST_POSE;
            _dgComponentsMode = Modes.DeformationGradientComponents.VELOCITY;

            _polygonMode    = PolygonMode.Fill;
            _materialFace   = MaterialFace.FrontAndBack;
            _vColoringMode  = Modes.VertexColoring.BONE;

            // Clustering
            _clusteringMethods[0]   = new P_Center_Clustering();
            _clusteringMethods[1]   = new K_Means_Clustering();
            _clusteringMethods[2]   = new K_RG_Clustering();
            _clusteringMethods[3]   = new Merge_RG_Clustering();
            _clusteringMethods[4]   = new Divide_Conquer_Clustering();
            _clusteringMethods[5]   = new K_Spectral_Clustering();
            _clusteringMethods[6]   = new C_PCA_Clustering();
            
            _clusteringMode                 = Modes.Clustering.P_CENTER;
            _clusteringSpectralDistanceMode = Modes.ClusteringSpectralDistance.NG02_TH07;
            _clusteringSpectralGraphMode    = Modes.ClusteringSpectralGraph.RANDOM_WALK;
            _clusteringDistanceMode         = Modes.ClusteringDistance.VERTEX;
            _clusteringVertexDistanceMode   = Modes.ClusteringVertexDistance.EUCLIDEAN;
         
            _clusteringClean            = false;
            _clusteringPerPose          = false;
            _clustering2RingColoring    = false;
            _clusteringIncremental      = false;
            _clusteringPerPoseMerging   = false;
        }
        #endregion

        #region Update Shaders Functions
        public void updateShaders(ref Shading rendering, Mesh3D pose, bool illumination)
        {
            pose.updateShaders(ref rendering, illumination);
            // Pose
            if (_sma != null)
            {
                rendering.bindUniform1("Pose", _sma.selectedPose);              
                if (_sma.selectedPose > -1)
                {
                    rendering.bindUniform1("enableEigenSkinVertex"   , _sma.errorDataVertex.enableEigenSkin ? 1 : 0);
                    rendering.bindUniform1("enableEigenSkinNormal"   , _sma.errorDataNormal.enableEigenSkin ? 1 : 0);
                   // rendering.bindUniform1("enableEigenWeightsVertex", _sma.errorDataVertex.enableEigenWeights ? 1 : 0);

                    rendering.bindUniform1("ApproximatingNormalsMode", (int)_sma.approximatingNormalsMode);

                    rendering.bindUniform1("tex_matrices", 1);
                    Texture.active(TextureUnit.Texture1);
                    _sma.errorDataVertex.tex_matrices[_sma.selectedPose].bind();
                }
            }
            else
            {
                rendering.bindUniform1("Pose", -1);
            }
        }
        #endregion

        #region Drawing Functions
        public void drawModels(ref Shading rendering, bool illumination, bool transparent)
        {
            List<Mesh3D> Poses = (_sma != null && _sma.poses != null && _sma.selectedPose != -1)
                ? _sma.poses : _poses;

            bool t = false;
            if (_selectedPoseDrawable && (Example._scene.animationGUI.play || _selectedPose != -1))
            {
                if (!Poses[_selectedPose].drawable)
                    return;

                GL.PolygonMode(_materialFace, _polygonMode);
                if (illumination)
                {
                    if (transparent)
                    {
                        t = Poses[_selectedPose].transparent;
                        Poses[_selectedPose].transparent = false;
                    }

                    updateShaders(ref rendering, Poses[_selectedPose], true);
                    Poses[_selectedPose].draw();
                    
                    if (transparent)
                        Poses[_selectedPose].transparent = t;
                }
                else
                {
                    updateShaders(ref rendering, Poses[_selectedPose], false);
                    Poses[_selectedPose].drawElements();
                }

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
            else
            {
                foreach (Mesh3D Pose in Poses)
                    if (Pose.drawable)
                    {
                        GL.PolygonMode(_materialFace, _polygonMode);
                        if (illumination)
                        {
                            if (transparent)
                            {
                                t = Pose.transparent;
                                Pose.transparent = false;
                            }

                            // Conditional Rendering
                            //Pose.samplesAnyQuery.beginConditionalRender();
                            {
                                updateShaders(ref rendering, Pose, true);
                                Pose.draw();
                            }
                            //Pose.samplesAnyQuery.endConditionalRender();

                            if (transparent)
                                Pose.transparent = t;
                        }
                        else
                        {
                            GL.PolygonMode(_materialFace, _polygonMode);
                            updateShaders(ref rendering, Pose, false);
                            Pose.drawElements();
                        }
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    }
            }

            // 3. Wireframe Rendering
            drawLines(ref rendering, illumination);
        }
        public void drawLines(ref Shading rendering, bool illumination)
        {
            List<Mesh3D> Poses = (_sma != null && _sma.poses != null && _sma.selectedPose != -1) ? _sma.poses : _poses;

            if (_selectedPoseDrawable && (Example._scene.animationGUI.play || _selectedPose != -1))
            {
                if (!Poses[_selectedPose].wireframe)
                    return;

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                updateShaders(ref rendering, Poses[_selectedPose], illumination);
                if (illumination)
                {
                    rendering.bindUniform1("ColoringMode", 0);
                    Poses[_selectedPose].draw();
                }
                else
                    Poses[_selectedPose].drawElements();
                GL.PolygonMode(MaterialFace.FrontAndBack, _polygonMode);
            }
            else
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                foreach (Mesh3D Pose in Poses)
                    if (Pose.drawable && Pose.wireframe)
                    {
                        updateShaders(ref rendering, Pose, illumination);
                        if (illumination)
                        {
                            rendering.bindUniform1("ColoringMode", 0);
                            Pose.draw();
                        }
                        else
                        {
                            Pose.drawElements();
                        }
                    }
                GL.PolygonMode(MaterialFace.FrontAndBack, _polygonMode);
            }
        }
        
        public void drawAABBs(ref Shading rendering)
        {
            List<Mesh3D> Poses = (_sma != null && _sma.poses != null && _sma.selectedPose != -1) ? _sma.poses : _poses;

            if (_selectedPoseDrawable && (Example._scene.animationGUI.play || _selectedPose != -1))
            {
                if (Poses[_selectedPose].drawable)
                {
                    updateShaders(ref rendering, Poses[_selectedPose], false);
                    Poses[_selectedPose].aabb.drawElementsLite();
                }
            }
            else
            {
                foreach (Mesh3D Pose in Poses)
                    if (Pose.drawable)
                    {
                        updateShaders(ref rendering, Pose, false);
                        Pose.aabb.drawElementsLite();
                    }
            }
        }
        public void drawAABBs()
        {
            if (_selectedPoseDrawable && (Example._scene.animationGUI.play || _selectedPose != -1))
                _poses[_selectedPose].aabb.draw();
            else
                foreach (Mesh3D Pose in _poses)
                    Pose.aabb.draw();
        }

        public void drawConvexHulls(ref Shading rendering)
        {
            List<Mesh3D> Poses = (_sma != null && _sma.poses != null && _sma.selectedPose != -1) ? _sma.poses : _poses;

            if (_selectedPoseDrawable && (Example._scene.animationGUI.play || _selectedPose != -1))
            {
                if (Poses[_selectedPose].drawable)
                {
                    updateShaders(ref rendering, Poses[_selectedPose], false);
                    Poses[_selectedPose].convexHull.drawElementsLite();
                }
            }
            else
            {
                foreach (Mesh3D Pose in Poses)
                    if (Pose.drawable)
                    {
                        updateShaders(ref rendering, Pose, false);
                        Pose.convexHull.drawElementsLite();
                    }
            }
        }
        public void drawConvexHulls()
        {
            if (_selectedPoseDrawable && (Example._scene.animationGUI.play || _selectedPose != -1))
                _poses[_selectedPose].convexHull.draw();
            else
                foreach (Mesh3D Pose in _poses)
                    Pose.convexHull.draw();
        }

        public void drawClustering(ref Shading rendering)
        {
            if (_selectedPose != -1 && _vColoringMode == Modes.VertexColoring.CLUSTER)
            {
                if (_clusteringPerPose)
                    pose.clusteringMethod.draw(this, ref rendering);
                else
                    clusteringMethod.draw(this, ref rendering);
            }
        }
        #endregion

        #region Get Vertices Color Functions
        private Vector3[] getClusterVerticesColorPerPose(Clustering clustering, bool merged)
        {
            Vector3[] colorsData = new Vector3[_verticesCount];

#if CPU_PARALLEL
            Parallel.For(0, _verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < _verticesCount; VertexID++)
#endif
            {
                colorsData[VertexID] = new Vector3(0.41f);
            }
#if CPU_PARALLEL
);
#endif
            List<Cluster> Clusters = merged ? clustering.clustersMerged : clustering.clusters;
            int SelectedCluster = clustering.selectedCluster;

            if (SelectedCluster == -1)
            {
                for (int ClusterID = 0; ClusterID < Clusters.Count; ClusterID++)
                {
                    Color C = Clusters[ClusterID].sphere.color;
#if CPU_PARALLEL
                    Parallel.ForEach(Clusters[ClusterID].elementsV, ElementID =>
#else
                    for (int ElementID = 0; ElementID < Clusters[ClusterID].elementsV.Count; ElementID++)
#endif
                    {
                        colorsData[ElementID].X = C.R / 255.0f;
                        colorsData[ElementID].Y = C.G / 255.0f;
                        colorsData[ElementID].Z = C.B / 255.0f;
                    }
#if CPU_PARALLEL
);
#endif
//                    foreach (List<int> Borders in Clusters[ClusterID].bordersV)
//                    {
//#if CPU_PARALLEL
//                        Parallel.ForEach(Borders, ElementID =>
//#else
//                        for (int ElementID = 0; ElementID < Clusters[ClusterID].elements.Count; ElementID++)
//#endif
//                        {
//                            colorsData[ElementID].X = 255.0f;
//                            colorsData[ElementID].Y = 255.0f;
//                            colorsData[ElementID].Z = 255.0f;
//                        }
//#if CPU_PARALLEL
//);
//#endif
//                    }
                }
            }
            else
            {
                Color C = Clusters[SelectedCluster].sphere.color;
#if CPU_PARALLEL
                Parallel.For(0, Clusters[SelectedCluster].elementsV.Count, VertexID =>
#else
                for (int VertexID = 0; VertexID < Clusters[SelectedCluster].elementsV.Count; VertexID++)
#endif
                {
                    int jj = Clusters[SelectedCluster].elementsV[VertexID];
                    colorsData[jj].X = C.R / 255.0f;
                    colorsData[jj].Y = C.G / 255.0f;
                    colorsData[jj].Z = C.B / 255.0f;
                }
#if CPU_PARALLEL
);
#endif
//                foreach (List<int> Borders in Clusters[SelectedCluster].bordersV)
//                {
//#if CPU_PARALLEL
//                    Parallel.ForEach(Borders, ElementID =>
//#else
//                        for (int ElementID = 0; ElementID < Clusters[ClusterID].elements.Count; ElementID++)
//#endif
//                    {
//                        colorsData[ElementID].X = 255.0f;
//                        colorsData[ElementID].Y = 255.0f;
//                        colorsData[ElementID].Z = 255.0f;
//                    }
//#if CPU_PARALLEL
//);
//#endif
//                }

                if (clustering.drawNeighbors)
                    for (int ClusterID = 0; ClusterID < Clusters[SelectedCluster].neighbors.Count; ClusterID++)
                    {
                        Cluster ClusterC = Clusters[SelectedCluster].neighbors[ClusterID];
                        C = Color.FromArgb(Color.WhiteSmoke.ToArgb() - ClusterID);
#if CPU_PARALLEL
                        Parallel.For(0, ClusterC.elementsV.Count, VertexID =>
#else
                        for (int VertexID = 0; VertexID < ClusterC.elementsV.Count; VertexID++)
#endif
                        {
                            int jj = ClusterC.elementsV[VertexID];
                            colorsData[jj].X = C.R / 255.0f;
                            colorsData[jj].Y = C.G / 255.0f;
                            colorsData[jj].Z = C.B / 255.0f;
                        }
#if CPU_PARALLEL
);
#endif
                    }
            }
            return colorsData;
        }
        #endregion 

        #region Set Vertices Color Functions
        private void setNoneVerticesColor()
        {
            foreach (Mesh3D Pose in _poses)
                Pose.setVerticesColor();
        }
        private void setRandomVerticesColor()
        {
            Random RandNum = new Random();

            Vector3[] ColorsVerticesData = new Vector3[_verticesCount];
#if CPU_PARALLEL
            Parallel.For(0, _verticesCount, VertexID =>
#else
            for (int VertexID = 0; VertexID < _verticesCount; VertexID++)
#endif
            {

                int[] k = new int[3];
                k[0] = RandNum.Next(255);
                k[1] = RandNum.Next(255);
                k[2] = RandNum.Next(255);

                Color c = Color.FromArgb(k[0], k[1], k[2]);
                ColorsVerticesData[VertexID].X = c.R / 255.0f;
                ColorsVerticesData[VertexID].Y = c.G / 255.0f;
                ColorsVerticesData[VertexID].Z = c.B / 255.0f;
            }
#if CPU_PARALLEL
            );
#endif
            foreach (Mesh3D Pose in _poses)
                Pose.setVerticesColor(ColorsVerticesData);
        }
        private void setGeodesicDistancesVerticesColor()
        {
            foreach (Mesh3D Pose in _poses)
                Pose.setVerticesColor(Pose.getGeodesicDistancesVerticesColor(_selectedVertex));
        }
        private void setClusterVerticesColor()
        {
            pose.setVerticesColor(  _clusteringPerPose ? 
                                    getClusterVerticesColorPerPose(pose.clusteringMethod, _clusteringPerPoseMerging) :
                                    getClusterVerticesColorPerPose(clusteringMethod     , _clusteringPerPoseMerging));
        }
        public  void setVerticesColor()
        {
            if      (_vColoringMode == Modes.VertexColoring.DEFORMATION_GRADIENTS)  setDeformationGradientsVerticesColor();
            else if (_vColoringMode == Modes.VertexColoring.GEODESIC_DISTANCES)     setGeodesicDistancesVerticesColor();
            else if (_vColoringMode == Modes.VertexColoring.RANDOM) setRandomVerticesColor();
            else if (_vColoringMode == Modes.VertexColoring.CLUSTER) { 
                if (sma != null)
                    sma.setVerticesColor(this);
                
                setClusterVerticesColor();
            }
            else if (_vColoringMode == Modes.VertexColoring.NONE) setNoneVerticesColor();
            else if (sma != null && (_vColoringMode == Modes.VertexColoring.BONE || _vColoringMode == Modes.VertexColoring.SKINNING_ERROR)) sma.setVerticesColor(this);
        }
        #endregion

        #region Deformation Gradient Functions

        #region Load-Save Functions
        public void loadDGs(Mesh3D Pose, String FilePath)
        {
            // LOAD
            {
                if      (FilePath.Contains("REST_POSE"))    _dgMode = Modes.DeformationGradient.REST_POSE;
                else if (FilePath.Contains("MEAN_POSE"))    _dgMode = Modes.DeformationGradient.MEAN_POSE;
                else if (FilePath.Contains("POSE_TO_POSE")) _dgMode = Modes.DeformationGradient.POSE_TO_POSE;
                else // ERROR
                {
                    return;
                }

                if      (FilePath.Contains("ACCELERATION"))     _dgComponentsMode = Modes.DeformationGradientComponents.ACCELERATION;
                else if (FilePath.Contains("ADJ_FROBENIUS"))    _dgComponentsMode = Modes.DeformationGradientComponents.ADJ_FROBENIUS;
                else if (FilePath.Contains("DG_FROBENIUS"))     _dgComponentsMode = Modes.DeformationGradientComponents.DG_FROBENIUS;
                else if (FilePath.Contains("FACET_AREA"))       _dgComponentsMode = Modes.DeformationGradientComponents.FACET_AREA;
                else if (FilePath.Contains("ROT_ANGLE"))        _dgComponentsMode = Modes.DeformationGradientComponents.ROT_ANGLE;
                else if (FilePath.Contains("ROT_AXIS"))         _dgComponentsMode = Modes.DeformationGradientComponents.ROT_AXIS;
                else if (FilePath.Contains("SCALE"))            _dgComponentsMode = Modes.DeformationGradientComponents.SCALE;
                else if (FilePath.Contains("SHEAR"))            _dgComponentsMode = Modes.DeformationGradientComponents.SHEAR;
                else if (FilePath.Contains("VELOCITY"))         _dgComponentsMode = Modes.DeformationGradientComponents.VELOCITY;
                else // ERROR
                {
                    return;
                }
            }

            if(_dgPerPose)
            {
                Pose.dg[(int)_dgMode] = new DeformationGradient(_verticesCount, _facetsCount);
                Pose.dg[(int)_dgMode].load(FilePath);
                Pose.dg[(int)_dgMode].setColor(Pose, _dgNormalize);
            }
            else
            {
                _dg[(int)_dgMode] = new DeformationGradient(_verticesCount, _facetsCount);
                _dg[(int)_dgMode].load(FilePath);
                _dg[(int)_dgMode].setColor(Pose, _dgNormalize);
            }            
        }
        public void saveDGs(bool AllPoses)
        {
            string DgMode = "";
            if      (_dgMode == Modes.DeformationGradient.REST_POSE) DgMode = "REST_POSE";
            else if (_dgMode == Modes.DeformationGradient.MEAN_POSE) DgMode = "MEAN_POSE";
            else                                                     DgMode = "POSE_TO_POSE";

            string DgComponentsMode = "";
            if      (_dgComponentsMode == Modes.DeformationGradientComponents.ACCELERATION)     DgComponentsMode = "ACCELERATION";
            else if (_dgComponentsMode == Modes.DeformationGradientComponents.ADJ_FROBENIUS)    DgComponentsMode = "ADJ_FROBENIUS";
            else if (_dgComponentsMode == Modes.DeformationGradientComponents.DG_FROBENIUS)     DgComponentsMode = "DG_FROBENIUS";
            else if (_dgComponentsMode == Modes.DeformationGradientComponents.FACET_AREA)       DgComponentsMode = "FACET_AREA";
            else if (_dgComponentsMode == Modes.DeformationGradientComponents.ROT_ANGLE)        DgComponentsMode = "ROT_ANGLE";
            else if (_dgComponentsMode == Modes.DeformationGradientComponents.ROT_AXIS)         DgComponentsMode = "ROT_AXIS";
            else if (_dgComponentsMode == Modes.DeformationGradientComponents.SCALE)            DgComponentsMode = "SCALE";
            else if (_dgComponentsMode == Modes.DeformationGradientComponents.SHEAR)            DgComponentsMode = "SHEAR";
            else if (_dgComponentsMode == Modes.DeformationGradientComponents.VELOCITY)         DgComponentsMode = "VELOCITY";
            else                                                                                DgComponentsMode = "ALL_WEIGHTED";

            if (!_dgPerPose)
                restPose.dg[(int)_dgMode].save(restPose.name, _poses.Count.ToString(), DgMode, DgComponentsMode);
            else
            {
                if (AllPoses)
#if CPU_PARALLEL
                    Parallel.ForEach(_poses, Pose =>
#else
                    foreach(Mesh3D Pose in _poses)
#endif
                    {
                        Pose.dg[(int)_dgMode].save(Pose.name, 1.ToString(), DgMode, DgComponentsMode);
                    }
#if CPU_PARALLEL
);
#endif
                else
                    pose.dg[(int)_dgMode].save(pose.name, 1.ToString(), DgMode, DgComponentsMode);
            }
        }
        #endregion

        public void calculateDeformationGradients(ProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            if (_poses.Count == 0)
                return;

            ///////

            Mesh3D MeanPose = (_addMeanPoseToTree) ? _poses[_poses.Count - 1] : _meanPose;

            progressBar.Value   = 0;
            progressBar.Maximum = _poses.Count;

            float Time_calculateDG_P2P = 0.0f;

            FPS.beginLocalCPU();

            if      (_dgMode == Modes.DeformationGradient.REST_POSE)
            {
                for (int PoseID = 0; PoseID < _poses.Count; ++PoseID)
                {
                    _poses[PoseID].dgRP = new DeformationGradient(_verticesCount, facetsCount);
                    _poses[PoseID].dgRP.calculate(restPose, _poses[PoseID], _dgMode, _dgComponentsMode);
                    progressBar.Increment(1);
                }
            }
            else if (_dgMode == Modes.DeformationGradient.MEAN_POSE)
            {
                for (int PoseID = 0; PoseID < _poses.Count; ++PoseID)
                {
                    _poses[PoseID].dgMP = new DeformationGradient(_verticesCount, facetsCount);
                    _poses[PoseID].dgMP.calculate(MeanPose, _poses[PoseID], _dgMode, _dgComponentsMode);
                    progressBar.Increment(1);
                }            
            }
            else
            {
                _poses[0].dgP2P = new DeformationGradient(_verticesCount, facetsCount);
                _poses[0].dgP2P.calculate(_poses[0], _poses[0], _dgMode, _dgComponentsMode);
                progressBar.Increment(1);
                for (int PoseID = 1; PoseID < _poses.Count; ++PoseID)
                {
                    _poses[PoseID].dgP2P = new DeformationGradient(_verticesCount, facetsCount);
                    _poses[PoseID].dgP2P.calculate(_poses[PoseID - 1], _poses[PoseID], _dgMode, _dgComponentsMode);
                    progressBar.Increment(1);
                }
            }

            Time_calculateDG_P2P += (float)FPS.endLocalCPU();

            //GLOBAL - AVERAGE
            {
                List<DeformationGradient> DgList = null;

                if (_dgMode == Modes.DeformationGradient.REST_POSE)
                {
                    DgList = new List<DeformationGradient>();
                    for (int PoseID = 0; PoseID < _poses.Count; ++PoseID)
                        if (PoseID != _selectedRestPose)
                            DgList.Add(_poses[PoseID].dgRP);

                    _dg[0] = new DeformationGradient(_verticesCount, _facetsCount);
                    _dg[0].calculate(DgList, _verticesCount, _facetsCount);
                }
                else if (_dgMode == Modes.DeformationGradient.MEAN_POSE)
                {
                    DgList = new List<DeformationGradient>();
                    for (int PoseID = 0; PoseID < _poses.Count; ++PoseID)
                        DgList.Add(_poses[PoseID].dgMP);
                    _dg[1] = new DeformationGradient(_verticesCount, _facetsCount);
                    _dg[1].calculate(DgList, _verticesCount, _facetsCount);               
                }
                else
                {
                    DgList = new List<DeformationGradient>();
                    for (int PoseID = 1; PoseID < _poses.Count; ++PoseID) DgList.Add(_poses[PoseID].dgP2P);
                    _dg[2] = new DeformationGradient(_verticesCount, _facetsCount);
                    _dg[2].calculate(DgList, _verticesCount, _facetsCount);               
                }

                if (_dgVariability)
                    _dg[(int)_dgMode].calculateVariability(this, MeanPose, _dgVariabilityEigenCount);
            }

            statusLabel.Text = "DG_P2P: (" + (Time_calculateDG_P2P / (_poses.Count - 1)).ToString() + ") - ";
        }
        public void setDeformationGradientsVerticesColor()
        {
            for (int PoseID = 0; PoseID < _poses.Count; ++PoseID)
                _poses[PoseID].setDeformationGradients(_dgMode, _dgComponentsMode, _dgPerPose, _dg, _dgNormalize);
        }
        #endregion

        #region Clustering Functions

        #region Load-Save Functions
        public void loadClustering(Mesh3D Pose, String FilePath)
        {
            Clustering CL = (_clusteringPerPose) ? Pose.clusteringMethod : clusteringMethod;

            CL.load(this, Pose, FilePath);
            CL.computeNeighborhood(Pose, false);
            Pose.computeTriangleElements(false);
            CL.computeTransformationMatrices(this, false);
        }
        public void saveClustering(bool AllPoses)
        {
            string ClusteringMode = "";

            if (!_clusteringPerPose)
            {
                if (_clusteringMode == Modes.Clustering.P_CENTER)
                    ClusteringMode = "P_CENTER";
                else if (_clusteringMode == Modes.Clustering.K_MEANS)
                    ClusteringMode = "K_MEANS";
                else if (_clusteringMode == Modes.Clustering.K_RG)
                    ClusteringMode = "K_RG";
                else if (_clusteringMode == Modes.Clustering.MERGE_RG)
                    ClusteringMode = "MERGE_RG";
                else if (_clusteringMode == Modes.Clustering.DIVIDE_CONQUER)
                    ClusteringMode = "DIVIDE_CONQUER";
                else if (_clusteringMode == Modes.Clustering.SPECTRAL)
                    ClusteringMode = "SPECTRAL";
                else if (_clusteringMode == Modes.Clustering.C_PCA)
                    ClusteringMode = "C_PCA";

                clusteringMethod.save(restPose.name, _poses.Count.ToString(), ClusteringMode);
            }
            else
            {
                if      (pose.clusteringMode == Modes.Clustering.P_CENTER)
                    ClusteringMode = "P_CENTER";
                else if (pose.clusteringMode == Modes.Clustering.K_MEANS)
                    ClusteringMode = "K_MEANS";
                else if (pose.clusteringMode == Modes.Clustering.K_RG)
                    ClusteringMode = "K_RG";
                else if (pose.clusteringMode == Modes.Clustering.MERGE_RG)
                    ClusteringMode = "MERGE_RG";
                else if (pose.clusteringMode == Modes.Clustering.DIVIDE_CONQUER)
                    ClusteringMode = "DIVIDE_CONQUER";
                else if (pose.clusteringMode == Modes.Clustering.SPECTRAL)
                    ClusteringMode = "SPECTRAL";
                else if (pose.clusteringMode == Modes.Clustering.C_PCA)
                    ClusteringMode = "C_PCA";

                if (AllPoses)
#if CPU_PARALLEL
                    Parallel.ForEach(_poses, Pose =>
#else
                    foreach(Mesh3D Pose in _poses)
#endif
                    {
                        Pose.clusteringMethod.save(Pose.name, 1.ToString(), ClusteringMode);
                    }
#if CPU_PARALLEL
);
#endif
                else
                    pose.clusteringMethod.save(pose.name, 1.ToString(), ClusteringMode);
            }
        }
        #endregion

        public void computeClustering(ProgressBar progressBar, ToolStripStatusLabel statusLabel, OpenTK.GLControl renderWindow)
        {           
            float Time_total = 0.0f;
            float Time_computeClustering = 0.0f;
            float Time_combineClusterings = 0.0f;
            float Time_computeNeighborhood = 0.0f;
            float Time_computeConnectedComponents = 0.0f;
            float Time_performCleaning = 0.0f;
            float Time_coloring = 0.0f;
            float Time_smoothBoundaries = 0.0f;
            float Time_computeTriangleElements = 0.0f;
            float Time_computeTransformationMatrices = 0.0f;
            {
                progressBar.Value = 0;
                vColoringMode     = Modes.VertexColoring.CLUSTER;

                if (_clusteringPerPose)
                {
                    progressBar.Maximum = _poses.Count;
                    foreach (Mesh3D Pose in _poses)
                    {
                        // 1. Compute Clusterings
                        FPS.beginLocalCPU();
                        {
                            if (Pose.poseID == _selectedRestPose)
                                Pose.clusteringMethod.computeRestPose(this, Pose);
                            else
                                Pose.clusteringMethod.compute(this, Pose);
                        }
                        Time_computeClustering += (float)FPS.endLocalCPU();

                        // 2. Compute Neighborhood
                        FPS.beginLocalCPU();
                        {
                            Pose.clusteringMethod.computeNeighborhood(Pose, false);
                        }
                        Time_computeNeighborhood += (float)FPS.endLocalCPU();

                        // 3. Compute Tringle Elements
                        FPS.beginLocalCPU();
                        {
                            Pose.computeTriangleElements(false);
                        }
                        Time_computeTriangleElements += (float)FPS.endLocalCPU();

                        // 4. Compute Transformations
                        FPS.beginLocalCPU();
                        {
                            Pose.clusteringMethod.computeTransformationMatrices(this, false);
                        }
                        Time_computeTransformationMatrices += (float)FPS.endLocalCPU();

                        // 5. Set Coloring
                        FPS.beginLocalCPU();
                        {
                            // 5.1 Random Coloring 
                            //Pose.clusteringMethod.setColor(true, false, _clustering2RingColoring);

                            // 5.2 Propagate Coloring
                            if (Pose.poseID == _selectedRestPose || Pose.poseID-1 == _selectedRestPose)
                                Pose.clusteringMethod.setColor(true, false, _clustering2RingColoring);
                            else
                                Pose.propagateColoring(_poses[Pose.poseID - 1], _poses[Pose.poseID - 1].clusteringMethod.clusters, Pose.clusteringMethod.clusters, false, _clustering2RingColoring);
                        }
                        Time_coloring += (float)FPS.endLocalCPU();

                        // 6. Interactive Rendering
                        renderWindow.MakeCurrent();
                        {
                            _selectedPose = Pose.poseID;
                            setVerticesColor();

                            Example._scene.draw();
                        }
                        renderWindow.SwapBuffers();

                        progressBar.Increment(1);
                    }

                    Time_total += (Time_computeClustering / (_poses.Count));
                    Time_total += (Time_computeNeighborhood / (_poses.Count));
                    Time_total += (Time_computeTriangleElements / (_poses.Count));
                    Time_total += (Time_computeTransformationMatrices / (_poses.Count));
                    Time_total += (Time_coloring / (_poses.Count));

                    statusLabel.Text  = "Clustering: (" + (Time_computeClustering / (_poses.Count)).ToString() + ") - ";
                    statusLabel.Text += "Smoothing: (" + (Time_smoothBoundaries / (_poses.Count)).ToString() + ") - ";
                    statusLabel.Text += "Triangles: (" + (Time_computeTriangleElements / (_poses.Count)).ToString() + ") - ";
                    statusLabel.Text += "Neighborhood: (" + (Time_computeNeighborhood / (_poses.Count)).ToString() + ") - ";
                    statusLabel.Text += "Coloring: (" + (Time_coloring/ (_poses.Count)).ToString() + ") - ";
                    statusLabel.Text += "Transformation: (" + (Time_computeTransformationMatrices / (_poses.Count)).ToString() + ") - ";
                    statusLabel.Text += "Total: (" + Time_total.ToString() + ")";
                }
                else
                {
                    if (_clusteringDistanceMode == Modes.ClusteringDistance.MERGING)
                    {
                        int      PosesCount = _poses.Count-1;
                        progressBar.Maximum = PosesCount;

                        // A. Subsequent Merging
                        if (_clusteringIncremental)
                        {
                            // 1. First, Compute P2P Merging
                            for (int PoseID = 0; PoseID < _poses.Count; PoseID++)
                                if (PoseID != _selectedRestPose)
                                {
                                    Mesh3D PrevPose = _poses[PoseID - 1];
                                    Mesh3D ThisPose = _poses[PoseID];

                                    // 1.1 Combine Clusterings
                                    FPS.beginLocalCPU();
                                    {
                                        ThisPose.clusteringMethod.combineClusterings(this, PrevPose, ThisPose);
                                    }
                                    Time_combineClusterings += (float)FPS.endLocalCPU();

                                    // 1.2 Compute Connected Components
                                    FPS.beginLocalCPU();
                                    {
                                        if (PoseID - 1 != _selectedRestPose)
                                            ThisPose.clusteringMethod.computeConnectedComponents(this, ThisPose, true);
                                    }
                                    Time_computeConnectedComponents += (float)FPS.endLocalCPU();

                                    // 1.3 Compute Tringle Elements
                                    FPS.beginLocalCPU();
                                    {
                                        ThisPose.computeTriangleElements(true);
                                    }
                                    Time_computeTriangleElements += (float)FPS.endLocalCPU();

                                    // 1.4 Perform h-cleaning
                                    if (_clusteringClean && PoseID-1 != _selectedRestPose)
                                    {
                                        FPS.beginLocalCPU();
                                        {
                                            ThisPose.clusteringMethod.performCleaning(this, ThisPose);
                                        }
                                        Time_performCleaning += (float)FPS.endLocalCPU();
                                    }

                                    // 1.5 Color Propagation
                                    FPS.beginLocalCPU();
                                    {
                                        if(PoseID-1 == _selectedRestPose)
                                            ThisPose.clusteringMethod.setColor(true, true, _clustering2RingColoring);
                                        //else
                                            //ThisPose.propagateColoring(PrevPose, PrevPose.clusteringMethod.clustersMerged, ThisPose.clusteringMethod.clustersMerged, true, _clustering2RingColoring);
                                    }
                                    Time_coloring += (float)FPS.endLocalCPU();
                                    
                                    progressBar.Increment(1);
                                }
                                else
                                {
                                    // 1.1 Compute Neighborhood
                                    FPS.beginLocalCPU();
                                    {
                                        restPose.clusteringMethod.computeNeighborhood(restPose, true);
                                    }
                                    Time_computeNeighborhood += (float)FPS.endLocalCPU();

                                    // 1.2 Compute Tringle Elements
                                    FPS.beginLocalCPU();
                                    {
                                        restPose.computeTriangleElements(true);
                                    }
                                    Time_computeTriangleElements += (float)FPS.endLocalCPU();

                                    // 1.3 Set Coloring 
                                    FPS.beginLocalCPU();
                                    {
                                        restPose.clusteringMethod.setColor(true, true, _clustering2RingColoring);
                                    }
                                    Time_coloring += (float)FPS.endLocalCPU();
                                }
                            // 2. Copy Clustering of Final Pose to MAS Clustering
                            clusteringMethod.clusters.Clear();
                            clusteringMethod.clusters.AddRange(_poses[PosesCount].clusteringMethod.clustersMerged);

                            Time_total += Time_coloring;
                        }
                        // B. Pairwise Merging
                        else
                        {
                            int PrevPoseID, ThisPoseID;
                            Mesh3D PrevPose, ThisPose;

                            double MergingDepthMax = Math.Log((double)(PosesCount), 2.0);
                            if (MergingDepthMax - Math.Truncate(MergingDepthMax) > 0.0)
                                MergingDepthMax++;
                            else
                                progressBar.Maximum--;

                            double MergingLengthMax = PosesCount;

                            // 1. First, Perform Pose Pair Clusterings from Leaves to Root
                            for (int MergingDepth = 0; MergingDepth < Math.Floor(MergingDepthMax); MergingDepth++)
                            {
                                MergingLengthMax = Math.Ceiling(MergingLengthMax / 2);
                                for (int MergingLength = 0; MergingLength < Math.Floor(MergingLengthMax); MergingLength++)
                                {
                                    progressBar.Increment(1);

                                    if (MergingDepth == 0)
                                        PrevPoseID = 2 * MergingLength + 1;
                                    else
                                        PrevPoseID = (int)Math.Pow(2.0, (double)(MergingDepth + 1)) * MergingLength + (int)Math.Pow(2.0, (double)(MergingDepth));

                                    ThisPoseID = (int)Math.Pow(2.0, (double)(MergingDepth)) + PrevPoseID;

                                    if ((PrevPoseID == _poses.Count-1) || (PrevPoseID >= _poses.Count && ThisPoseID >= _poses.Count))
                                        continue;
                                    else if (ThisPoseID >= _poses.Count)
                                        ThisPoseID = PosesCount;

                                    PrevPose = this.poses[PrevPoseID];
                                    ThisPose = this.poses[ThisPoseID];

                                    // 2.1 Combine Clusterings
                                    FPS.beginLocalCPU();
                                    {
                                        ThisPose.clusteringMethod.combineClusterings(this, PrevPose, ThisPose);
                                    }
                                    Time_combineClusterings += (float)FPS.endLocalCPU();

                                    // 2.2 Compute Connected Components
                                    FPS.beginLocalCPU();
                                    {
                                        ThisPose.clusteringMethod.computeConnectedComponents(this, ThisPose, true);
                                    }
                                    Time_computeConnectedComponents += (float)FPS.endLocalCPU();

                                    // 2.3 Compute Tringle Elements
                                    FPS.beginLocalCPU();
                                    {
                                        ThisPose.computeTriangleElements(true);
                                    }
                                    Time_computeTriangleElements += (float)FPS.endLocalCPU();

                                    // 2.4 Perform Cleaning
                                    if (_clusteringClean)
                                    {
                                        FPS.beginLocalCPU();
                                        {
                                            ThisPose.clusteringMethod.performCleaning(this, ThisPose);
                                        }
                                        Time_performCleaning += (float)FPS.endLocalCPU();
                                    }

                                    ThisPose.clusteringMethod.clusters.Clear();
                                    ThisPose.clusteringMethod.clusters.AddRange(ThisPose.clusteringMethod.clustersMerged);
                                }
                            }
                            // 3. Copy Clustering of Final Pose to MAS Clustering
                            clusteringMethod.clusters.Clear();
                            clusteringMethod.clusters.AddRange(_poses[PosesCount].clusteringMethod.clusters);
                            // 4. Set Coloring
                            FPS.beginLocalCPU();
                            {
                                clusteringMethod.setColor(true, false, _clustering2RingColoring);
                            }
                            Time_coloring += (float)FPS.endLocalCPU();
                            Time_total += Time_coloring;
                        }

                        clusteringMethod.computeNeighborhood(restPose, false);
                        clusteringMethod.computeTransformationMatrices(this, false);

                        // C. Performance
                        {
                            Time_total += Time_combineClusterings;
                            Time_total += Time_computeTriangleElements;
                            Time_total += Time_computeConnectedComponents;
                            Time_total += Time_performCleaning ;

                            statusLabel.Text  = "Combine: (" + (Time_combineClusterings ).ToString() + ") - ";
                            statusLabel.Text += "Triangles: (" + (Time_computeTriangleElements ).ToString() + ") - ";
                            statusLabel.Text += "ConnectedComponents: (" + (Time_computeConnectedComponents ).ToString() + ") - ";
                            if (_clusteringClean)
                            statusLabel.Text += "Cleaning: (" + (Time_performCleaning ).ToString() + ") - ";
                            statusLabel.Text += "Coloring: (" + Time_coloring.ToString() + ") - ";
                            statusLabel.Text += "Total: (" + Time_total.ToString() + ")";
                        }
                    }
                    else if (_clusteringDistanceMode == Modes.ClusteringDistance.OVER_SEGMENTATION)
                    {
                        progressBar.Maximum = 1;
                        {
                            // 1. Compute Over-Segmentation
                            FPS.beginLocalCPU();
                            {
                                clusteringMethod.computeOverSegmentation(this, restPose);
                            }
                            Time_combineClusterings += (float)FPS.endLocalCPU();

                            // 2. Compute Connected Components
                            FPS.beginLocalCPU();
                            {
                                clusteringMethod.computeConnectedComponents(this, restPose, false);
                            }
                            Time_computeConnectedComponents += (float)FPS.endLocalCPU();

                            // 3. Compute Triangle Elements
                            FPS.beginLocalCPU();
                            {
                                restPose.computeTriangleElements(false);
                            }
                            Time_computeTriangleElements += (float)FPS.endLocalCPU();

                            // 4. Set Coloring
                            FPS.beginLocalCPU();
                            {
                                clusteringMethod.setColor(true, false, _clustering2RingColoring);
                            }
                            Time_coloring += (float)FPS.endLocalCPU();

                            // 5. Compute Transformation Matrices
                            FPS.beginLocalCPU();
                            {
                                clusteringMethod.computeTransformationMatrices(this, false);
                            }
                            Time_computeTransformationMatrices += (float)FPS.endLocalCPU();

                            // 6. Performance
                            {
                                Time_total += Time_combineClusterings;
                                Time_total += Time_computeConnectedComponents;
                                Time_total += Time_computeTriangleElements;

                                statusLabel.Text  = "Combine: (" + (Time_combineClusterings).ToString() + ") - ";
                                statusLabel.Text += "Triangles: (" + (Time_computeTriangleElements).ToString() + ") - ";
                                statusLabel.Text += "ConnectedComponents: (" + (Time_computeConnectedComponents).ToString() + ") - ";
                                statusLabel.Text += "Coloring: (" + Time_coloring.ToString() + ") - ";
                                statusLabel.Text += "Total: (" + Time_total.ToString() + ")";
                            }
                        }
                        progressBar.Increment(1);
                    }
                    else
                    {
                        if (_clusteringDistanceMode == Modes.ClusteringDistance.CLUSTERING)
                            clusteringMethod.computeSimilarityMatrix(this);

                        progressBar.Maximum = 1;
                        
                        FPS.beginLocalCPU();
                        {
                            clusteringMethod.compute(this, restPose);
                            clusteringMethod.computeNeighborhood(restPose, false);

                            if (_clusteringMode == Modes.Clustering.K_MEANS || _clusteringMode == Modes.Clustering.SPECTRAL)
                            {
                                foreach (Cluster ClusterC in clusteringMethod.clusters)
                                    ClusterC.toBeChecked = true;
                                clusteringMethod.computeConnectedComponents(this, restPose, false);
                            }
                            restPose.computeTriangleElements(false);
                            clusteringMethod.setColor(true, false, _clustering2RingColoring);
                            clusteringMethod.computeTransformationMatrices(this, false);                            
                        }
                        Time_total += (float)FPS.endLocalCPU();
                        statusLabel.Text += "Total: (" + Time_total.ToString() + ")";
                        progressBar.Increment(1);
                    }
                }
            }
        }
        public void propagateClusteringColors(ProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            progressBar.Value   = 0;
            progressBar.Maximum = _poses.Count - 2;

            float Time = 0.0f;
            for (int PoseID = 0; PoseID < _poses.Count; PoseID++)
                if (PoseID != _selectedRestPose)
                {
                    Mesh3D PrevPose = _poses[PoseID - 1];
                    Mesh3D ThisPose = _poses[PoseID];

                    FPS.beginLocalCPU();
                    {
                        if (PoseID - 1 != _selectedRestPose)
                        {
                            if (!_clusteringPerPoseMerging) // Original Segmentation
                                ThisPose.propagateColoring(PrevPose, PrevPose.clusteringMethod.clusters, ThisPose.clusteringMethod.clusters, false, _clustering2RingColoring);
                            else                            // Variable Segmentation
                                ThisPose.propagateColoring(PrevPose, PrevPose.clusteringMethod.clustersMerged, ThisPose.clusteringMethod.clustersMerged, true, _clustering2RingColoring);
                        }
                    }
                    Time += (float)FPS.endLocalCPU();

                    progressBar.Increment(1);
                }
                else
                {
                    FPS.beginLocalCPU();
                    {
                        _poses[PoseID].clusteringMethod.setColor(true, _clusteringPerPoseMerging, _clustering2RingColoring);
                    }
                    Time += (float)FPS.endLocalCPU();
                }

            statusLabel.Text = "Average Color Propagation: (" + (Time / (_poses.Count - 2)).ToString() + ") - ";
        }
        public void computeVariableSegmentation(ProgressBar progressBar, ToolStripStatusLabel statusLabel, OpenTK.GLControl renderWindow)
        {
            int PosesCount = _poses.Count - 1;

            progressBar.Value = 0;
            progressBar.Maximum = PosesCount;

            float Time_mergeClusterings = 0.0f;
            float Time_computeConnectedComponents = 0.0f;
            float Time_performCleaning = 0.0f;
            float Time_computeTriangleElements = 0.0f;
            float Time_computeTransformationMatrices = 0.0f;
            float Time_coloring = 0.0f;
            float Time_total = 0.0f;
            
            // Variable or Incremental Segmentation
            for (int PoseID = 0; PoseID < _poses.Count; PoseID++)
                if (PoseID != _selectedRestPose)
                {
                    Mesh3D PrevPose = _poses[PoseID - 1];
                    Mesh3D ThisPose = _poses[PoseID];

                    // 1. Combine Clusterings
                    FPS.beginLocalCPU();
                    {
                        ThisPose.clusteringMethod.combineClusterings(this, PrevPose, ThisPose);
                    }
                    Time_mergeClusterings += (float)FPS.endLocalCPU();

                    // 2. Compute Connected Components
                    if (_clusteringClean)
                    {
                        FPS.beginLocalCPU();
                        {
                            ThisPose.clusteringMethod.computeConnectedComponents(this, ThisPose, true);
                        }
                        Time_computeConnectedComponents += (float)FPS.endLocalCPU();
                    }

                    // 3. Compute Tringle Elements
                    FPS.beginLocalCPU();
                    {
                        ThisPose.computeTriangleElements(true);
                    }
                    Time_computeTriangleElements += (float)FPS.endLocalCPU();

                    // 4. Perform Cleaning
                    if (_clusteringClean)
                    {
                        FPS.beginLocalCPU();
                        {
                            ThisPose.clusteringMethod.performCleaning(this, ThisPose);
                        }
                        Time_performCleaning += (float)FPS.endLocalCPU();
                    }

                    // 5. Compute Transformations
                    FPS.beginLocalCPU();
                    {
                        ThisPose.clusteringMethod.computeTransformationMatrices(this, true);
                    }
                    Time_computeTransformationMatrices += (float)FPS.endLocalCPU();

                    // 6. Set Coloring
                    FPS.beginLocalCPU();
                    {
                        if (PoseID - 1 == _selectedRestPose) // 6.1 Random Coloring
                            ThisPose.clusteringMethod.setColor(true, true, _clustering2RingColoring);
                        else                                 // 6.2 Propagate Coloring
                            ThisPose.propagateColoring(PrevPose, PrevPose.clusteringMethod.clustersMerged, ThisPose.clusteringMethod.clustersMerged, true, _clustering2RingColoring);
                    }
                    Time_coloring += (float)FPS.endLocalCPU();

                    // 7. Interactive Rendering
                    renderWindow.MakeCurrent();
                    {
                        _selectedPose = PoseID;
                        setVerticesColor();

                        Example._scene.draw();
                        System.Threading.Thread.Sleep(500);
                    }
                    renderWindow.SwapBuffers();

                    progressBar.Increment(1);
                }
                else
                {
                    FPS.beginLocalCPU();
                    {
                        _poses[PoseID].clusteringMethod.setColor(true, true, _clustering2RingColoring);
                    }
                    Time_coloring += (float)FPS.endLocalCPU();
                }

            Time_total += (Time_mergeClusterings / PosesCount);
            Time_total += (Time_computeTriangleElements / PosesCount);
            Time_total += (Time_computeConnectedComponents / PosesCount);
            Time_total += (Time_performCleaning / PosesCount);
            Time_total += (Time_coloring / (_poses.Count));
            Time_total += (Time_computeTransformationMatrices / PosesCount);

            statusLabel.Text = "Merge: (" + (Time_mergeClusterings / PosesCount).ToString() + ") - ";
            statusLabel.Text += "Triangles: (" + (Time_computeTriangleElements / PosesCount).ToString() + ") - ";
            if (_clusteringClean)
            {
                statusLabel.Text += "ConnectedComponents: (" + (Time_computeConnectedComponents / PosesCount).ToString() + ") - ";
                statusLabel.Text += "Clean: (" + (Time_performCleaning / PosesCount).ToString() + ") - ";
            }
            statusLabel.Text += "Coloring: (" + (Time_coloring / (_poses.Count)).ToString() + ") - ";
            statusLabel.Text += "Transformation: (" + (Time_computeTransformationMatrices / PosesCount).ToString() + ") - ";
            statusLabel.Text += "Total: (" + Time_total.ToString() + ") -";
        }

        public void performCleaning(ProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            float Time_coloring = 0.0f;
            float Time_performCleaning = 0.0f;
            float Time_computeTransformationMatrices = 0.0f;
            {
                //progressBar.Value = 0;
                //progressBar.Maximum = 1;

                // 2. Perform Cleaning
                FPS.beginLocalCPU();
                {
                    clusteringMethod.performCleaningOverSegmentation(this, restPose);
                }
                Time_performCleaning += (float)FPS.endLocalCPU();

                // 3. Set Coloring
                FPS.beginLocalCPU();
                {
                    clusteringMethod.setColor(true, false, _clustering2RingColoring);
                }
                Time_coloring += (float)FPS.endLocalCPU();

                // 4. Compute Transformation Matrices
                FPS.beginLocalCPU();
                {
                    clusteringMethod.computeTransformationMatrices(this, false);
                }
                Time_computeTransformationMatrices += (float)FPS.endLocalCPU();

                //progressBar.Increment(1);
            }

            statusLabel.Text  = "Cleaning: (" + Time_performCleaning.ToString() + ")";
            statusLabel.Text += "Coloring: (" + Time_coloring.ToString() + ") - ";
        }
        public void smoothBoundaries(ProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            float Time_smoothBoundaries = 0.0f;

            progressBar.Value = 0;
            if (_clusteringPerPose)
            {
                progressBar.Maximum = _poses.Count-1;
                foreach (Mesh3D Pose in _poses)
                    if (Pose.poseID != _selectedRestPose)
                    {
                        Pose.clusteringMethod.smoothBoundaries(Pose, _clusteringPerPoseMerging);
                        progressBar.Increment(1);
                    }
            }
            else
            {
                progressBar.Maximum = 1;
                FPS.beginLocalCPU();
                {
                    clusteringMethod.smoothBoundaries(restPose, false);
                }
                Time_smoothBoundaries += (float)FPS.endLocalCPU();

                progressBar.Increment(1);
            }

            statusLabel.Text += "Smoothing: " + (Time_smoothBoundaries / progressBar.Maximum).ToString();
        }
        #endregion

        #region Delete Functions
        public void delete()
        {
            foreach (Mesh3D pose in _poses)
                pose.delete();
            if (_sma != null) _sma.delete();
        }
        #endregion

        #region Add Functions
        public bool addModel(TreeView tree, int count, ProgressBar pBar, ToolStripStatusLabel tLabel, ContextMenuStrip cMenuStrip)
        {
            int i, j, k, p;

            Mesh3D mesh_clone = null;
            List<List<Mesh3D>> meshes = WaveFront_OBJ_File.OpenOBJFile(pBar, tLabel, _rotation90, _ignoreGroups);

            if (meshes == null || meshes[0] == null)
                return true;

            for (k = 0; k < meshes.Count; k++)
            {
                List<Mesh3D> M = meshes[k];
                for (p = 0; p < M.Count; p++)
                {
                    mesh_clone = null;
                    for (i = 0; i < count; i++)
                    {
                        mesh_clone = M[p].ShallowCopy();
                        mesh_clone.poseID = _poses.Count;

                        mesh_clone.name += (count == 1) ? "" : "_" + i.ToString();

                        mesh_clone.init_PrimitiveID = _max_PrimitiveID + 1;
                        _poses.Add(mesh_clone);

                        j = _poses.Count - 1;

                        _max_PrimitiveID += _poses[j].facetsCount; // Primitive ID

                        if (j == _selectedRestPose)
                            tree.Nodes[0].Nodes.Add(_poses[j].name + '*');
                        else
                            tree.Nodes[0].Nodes.Add(_poses[j].name);

                        tree.Nodes[0].Nodes[_poses[j].poseID].Tag = _poses[j].poseID;
                        tree.Nodes[0].Nodes[_poses[j].poseID].ContextMenuStrip = cMenuStrip;

                        if (j == 0)
                        {
                            _max = poses[0].max;
                            _min = poses[0].min;
                            _center = poses[0].center;
                        }
                        else
                        {
                            if (_max.X < _poses[j].max.X) _max.X = _poses[j].max.X;
                            if (_max.Y < _poses[j].max.Y) _max.Y = _poses[j].max.Y;
                            if (_max.Z < _poses[j].max.Z) _max.Z = _poses[j].max.Z;

                            if (_min.X > _poses[j].min.X) _min.X = _poses[j].min.X;
                            if (_min.Y > _poses[j].min.Y) _min.Y = _poses[j].min.Y;
                            if (-min.Z > _poses[j].min.Z) _min.Z = _poses[j].min.Z;

                            _center = (_max + _min) * 0.5f;
                        }
                    }
                }
            }
            _selectedPose   = 0;
            _verticesCount  = _poses[0].verticesCount;
            _facetsCount    = _poses[0].facetsCount;

            tLabel.Text = "Scene consists of " + _poses.Count.ToString() + " models!";
            tree.ExpandAll();

            return false;
        }
        public void addMeanPose(TreeView tree, ContextMenuStrip cMenuStrip)
        {
            Vector3[] meanVertices = new Vector3[_poses[0].verticesCount];
            for (int VertexID = 0; VertexID < meanVertices.Length; VertexID++)
            {
                meanVertices[VertexID] = Vector3.Zero;
                for (int PoseID = 0; PoseID < _poses.Count; PoseID++)
                    meanVertices[VertexID] += _poses[PoseID].verticesData[VertexID];
                meanVertices[VertexID] /= (float)_poses.Count;
            }

            _meanPose = new Mesh3D(_poses.Count, _poses[0].name + "#", meanVertices, new Vector3[] { }, null, null, _poses[0].indicesData, _poses[0].neighborsVerticesData, _poses[0].neighborsFacetsData);
            _meanPose.poseID = _poses.Count;

            if (_addMeanPoseToTree)
            {
                _poses.Add(_meanPose);

                tree.Nodes[0].Nodes.Add(_meanPose.name);
                tree.Nodes[0].Nodes[_meanPose.poseID].Tag = _meanPose.poseID;
                tree.Nodes[0].Nodes[_meanPose.poseID].ContextMenuStrip = cMenuStrip;
                tree.ExpandAll();
            }
        }
        #endregion

        #region Edited Pose
        public void addEditedPose(TreeView tree, ProgressBar progressBar, ToolStripStatusLabel _statusLabel)
        {
            List<List<Mesh3D>> EditedMeshes = WaveFront_OBJ_File.OpenOBJFile(progressBar, _statusLabel, false, true);
            if (EditedMeshes == null || EditedMeshes[0] == null)
                return;

            _editedPose = EditedMeshes[0][0];
            _editedPose.poseID = _selectedPose;
            _editedPose.name += " - Edited";

            tree.Nodes[0].Nodes[_editedPose.poseID].Text = _editedPose.name;

            if (_editedPose.poseID == _selectedRestPose)
                tree.Nodes[0].Nodes[_editedPose.poseID].Text += '*';
            else
                _sma.computeFittingMatrices(this, _selectedPose);

            //clusteringMethod.computeTransformationMatricesEditing(this);
            
    //        _sma.computeMatricesEditing(this);
            _sma.computeFinalPositions();
            _sma.computeApproxModels(this);
        }
        public void resetEditedPose(TreeView tree)
        {
            tree.Nodes[0].Nodes[_editedPose.poseID].Text = _poses[_editedPose.poseID].name;

            if (_editedPose.poseID == _selectedRestPose)
                tree.Nodes[0].Nodes[_editedPose.poseID].Text += '*';

            _editedPose = null;
            _sma.computeFinalPositions();
            _sma.computeApproxModels(this);
        }
        #endregion

        #region Remove Functions
        public bool removeModel(TreeView tree)
        {
            if (_selectedPose == -1) return true;

            int removedModelIndex = _selectedPose;
            tree.Nodes[0].Nodes.RemoveAt(removedModelIndex);

            _poses.RemoveAt(removedModelIndex);
            _min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            _max = Vector3.Zero;
            _center = Vector3.Zero;

            foreach (Mesh3D pose in _poses)
            {
                if (pose.poseID >= removedModelIndex)
                {
                    pose.poseID--;
                    tree.Nodes[0].Nodes[pose.poseID].Tag = pose.poseID;
                }

                if (_max.X < pose.max.X) _max.X = pose.max.X;
                if (_max.Y < pose.max.Y) _max.Y = pose.max.Y;
                if (_max.Z < pose.max.Z) _max.Z = pose.max.Z;

                if (_min.X > pose.min.X) _min.X = pose.min.X;
                if (_min.Y > pose.min.Y) _min.Y = pose.min.Y;
                if (_min.Z > pose.min.Z) _min.Z = pose.min.Z;
            }
            _center = (_max + _min) * 0.5f;

            return false;
        }
        #endregion

        #region Translate Function
        public void translate(Keys keys)
        {
            if (_selectedPose != -1)
                pose.translate(keys);
        }
        #endregion
    }
}