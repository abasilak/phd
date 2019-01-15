using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace abasilak
{
    public class Scene
    {
        #region Private Properties

        AnimationGUI            _animationGUI;
        Mesh3DAnimationSequence _meshAnimation;

        MultiFragmentRendering  _multiFragmentRendering;

        //
        float    _gamma;
        float    _pointSize,_lineSize;
        int      _width, _height;
        Light    _light;
        Camera   _camera;
        //
        Color    _backgroundColor;
        Texture  _backgroundTexture;
        bool     _useBackgroundTexture;
        //
        int      _selectedTexture;
        int      _selectedRendering;
        //
#if peel
        bool     _instancing = true;
#else
        bool     _instancing = false;
#endif

        Vector3  _instance_translation = new Vector3(0.0f, 0.00001f, 0.0f);
        //Vector3 _instance_translation = new Vector3(0.0f, 0.0f, 0.0f);

        bool     _randomBias = false;
        float    _discardThreshold = 0.0f;

#if peel
        bool     _tessellation   = true;
#else
        bool     _tessellation   = false;
#endif
        float    _tessLevelInner = 1.0f;
        float    _tessLevelOuter = 1.0f;

        WeightSum     _weightSumSimple;
        WeightSum     _weightSumF2B;
        WeightSum     _weightSumDUAL;
        AverageColors _averageColors;

        // Modes
        Modes.Illumination _illuminationMode;
        Modes.Transparency _transparencyMode;
        Modes.Rendering _renderingMode;
        Modes.Peeling  _peelingMode;
        Modes.Trimming _trimmingMode;
        
        List<Texture>       _textures           = new List<Texture>();
        List<Rendering>[]   _renderingMethods   = new List<Rendering>[3];
        
        /////////////
        // Shaders //
        /////////////

        // render
        Shader   _renderVertex;
        Shader   _renderGeometry;
        Shader   _phongFragment,_computePixelColor,_renderFragment;
        // Tessellationn
        Shader _renderVertexTess;
        Shader _renderTessellationControl, _renderTessellationEvaluation;
        // AABB-ConvexHull
        Shader   _aabb_chVertex, _aabb_chFragment;
        Shading  _aabb_chRendering;
        // Sphere
        Shader   _sphereVertex, _sphereFragment;
        Shading  _sphereRendering;
        // Cluster Regions
        Shader   _renderVertexRegions, _renderGeometryRegions, _renderFragmentRegions;
        // Thickness

        Shader _thickVertex, _thickCompute, _thickCompute_f2b, _thickCompute_dual, _thickCompute_k_buffer, _thickCompute_k_multi_buffer_Z, _thickCompute_k_stencil_buffer;
        Shading _thickComputing, _thickComputing_f2b, _thickComputing_dual, _thickComputing_k_buffer, _thickComputing_k_multi_buffer_Z, _thickComputing_k_stencil_buffer;

        #endregion

        #region Public Properties

        public AnimationGUI animationGUI
        {
            get
            {
                return _animationGUI;
            }
            set
            {
                _animationGUI = value;
            }
        }  
        public Mesh3DAnimationSequence meshAnimation
        {
            get
            {
                return _meshAnimation;
            }
            set
            {
                _meshAnimation = value;
            }
        }
        public MultiFragmentRendering multiFragmentRendering
        {
            get { return _multiFragmentRendering; }
        }

        public int selectedTexture
        {
            get
            {
                return _selectedTexture;
            }
            set
            {
                _selectedTexture = value;
            }
        }
        public int selectedRendering
        {
            get
            {
                return _selectedRendering;
            }
            set
            {
                _selectedRendering = value;
            }
        }
        public float pointSize
        {
            get
            {
                return _pointSize;
            }
            set
            {
                _pointSize=value;
            }
        }
        public float lineSize
        {
            get
            {
                return _lineSize;
            }
            set
            {
                _lineSize=value;
            }
        }

        public int width
        {
            get { return _width; }
            set { _width = value; }
        }
        public int height
        {
            get { return _height; }
            set { _height = value; }
        }
        public float gamma
        {
            get
            {
                return _gamma;
            }
            set
            {
                _gamma=value;
            }
        }
        public bool useBackgroundTexture
        {
            get
            {
                return _useBackgroundTexture;
            }
            set
            {
                _useBackgroundTexture=value;
            }
        }

        public bool tessellation
        {
            get
            {
                return _tessellation;
            }
            set
            {
                _tessellation = value;
            }
        }

        public bool randomBias
        {
            get
            {
                return _randomBias;
            }
            set
            {
                _randomBias = value;
            }
        }
        public float discardThreshold
        {
            get
            {
                return _discardThreshold;
            }
            set
            {
                _discardThreshold = value;
            }
        }

        public float tessLevelInner
        {
            get
            {
                return _tessLevelInner;
            }
            set
            {
                _tessLevelInner = value;
            }
        }
        public float tessLevelOuter
        {
            get
            {
                return _tessLevelOuter;
            }
            set
            {
                _tessLevelOuter = value;
            }
        }

        public bool instancing
        {
            get
            {
                return _instancing;
            }
            set
            {
                _instancing = value;
            }
        }
        public Vector3 instance_translation
        {
            get
            {
                return _instance_translation;
            }
            set
            {
                _instance_translation = value;
            }
        }

        public List<Rendering> peelingMethods
        {
            get
            {
                return _renderingMethods[1];
            }
        }
        public List<Rendering> trimmingMethods
        {
            get
            {
                return _renderingMethods[2];
            }
        }
        
        public List<Texture> textures
        {
            get
            {
                return _textures;
            }
            set
            {
                _textures = value;
            }
        }

        public Light light
        {
            get { return _light; }
            set { _light = value; }
        }
        public Camera camera
        {
            get { return _camera; }
            set { _camera = value; }
        }
        public Shader renderVertex
        {
            get { return _renderVertex; }
            set { _renderVertex = value; }
        }
        public Shader renderVertexTess
        {
            get
            {
                return _renderVertexTess;
            }
        }
        public Shader renderTessellationControl
        {
            get
            {
                return _renderTessellationControl;
            }
        }
        public Shader renderTessellationEvaluation
        {
            get
            {
                return _renderTessellationEvaluation;
            }
        }

        public Shader renderVertexRegions
        {
            get { return _renderVertexRegions; }
        }
        public Shader renderGeometry
        {
            get { return _renderGeometry; }
            set { _renderGeometry = value; }
        }
        public Shader renderGeometryRegions
        {
            get { return _renderGeometryRegions; }
        }       
        public Shader phongFragment
        {
            get { return _phongFragment; }
            set { _phongFragment = value; }
        }
        public Shader computePixelColor
        {
            get { return _computePixelColor; }
            set { _computePixelColor = value; }
        }
        public Shader renderFragment
        {
            get { return _renderFragment; }
            set { _renderFragment = value; }
        }
        public Shader renderFragmentRegions
        {
            get { return _renderFragmentRegions; }
        }
        public Color   backgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }
        public Texture backgroundTexture
        {
            get { return _backgroundTexture; }
            set { _backgroundTexture = value; }
        }

        public Modes.Rendering renderingMode
        {
            get
            {
                return _renderingMode;
            }
            set
            {
                _renderingMode = value;
            }
        }
        public Modes.Illumination illuminationMode
        {
            get
            {
                return _illuminationMode;
            }
            set
            {
                _illuminationMode = value;
            }
        }
        public Modes.Transparency transparencyMode
        {
            get
            {
                return _transparencyMode;
            }
            set
            {
                _transparencyMode = value;
            }
        }
        public Modes.Peeling peelingMode
        {
            get
            {
                return _peelingMode;
            }
            set
            {
                _peelingMode = value;
            }
        }
        public Modes.Trimming trimmingMode
        {
            get
            {
                return _trimmingMode;
            }
            set
            {
                _trimmingMode = value;
            }
        }

        public WeightSum weightSumSimple
        {
            get
            {
                return _weightSumSimple;
            }
            set
            {
                _weightSumSimple = value;
            }
        }
        public WeightSum weightSumF2B
        {
            get
            {
                return _weightSumF2B;
            }
            set
            {
                _weightSumF2B = value;
            }
        }
        public WeightSum weightSumDUAL
        {
            get
            {
                return _weightSumDUAL;
            }
            set
            {
                _weightSumDUAL = value;
            }
        }
        public AverageColors averageColors
        {
            get
            {
                return _averageColors;
            }
            set
            {
                _averageColors = value;
            }
        }

        public Shading aabb_chRendering
        {
            get
            {
                return _aabb_chRendering;
            }
        }
        public Shading sphereRendering
        {
            get
            {
                return _sphereRendering;
            }
        }
        public Shading thickComputing
        {
            get
            {
                return _thickComputing;
            }
        }
        public Shading thickComputing_f2b
        {
            get
            {
                return _thickComputing_f2b;
            }
        }
        public Shading thickComputing_dual
        {
            get
            {
                return _thickComputing_dual;
            }
        }
        public Shading thickComputing_k_buffer
        {
            get
            {
                return _thickComputing_k_buffer;
            }
        }
        public Shading thickComputing_k_multi_buffer_Z
        {
            get
            {
                return _thickComputing_k_multi_buffer_Z;
            }
        }
        public Shading thickComputing_k_stencil_buffer
        {
            get
            {
                return _thickComputing_k_stencil_buffer;
            }
        }

        #endregion

        #region Constructor
        public  Scene(int w, int h)
        {
            _animationGUI = new AnimationGUI();
            _meshAnimation = new Mesh3DAnimationSequence();
            _multiFragmentRendering = new MultiFragmentRendering();
          
            _selectedTexture = -1;
            _selectedRendering = 0;

            _gamma = 1.0f;
            _pointSize = 1.0f;
            _lineSize = 1.0f;
            _width = w;
            _height = h;
            _backgroundColor = Color.Gray;
            _useBackgroundTexture = false;

            _transparencyMode = Modes.Transparency.WEIGHT_SUM;
            _illuminationMode = Modes.Illumination.PHONG;
#if peel
            _renderingMode = Modes.Rendering.PEELING;
#else
            _renderingMode = Modes.Rendering.RENDER;
#endif
            _peelingMode = Modes.Peeling.F2B;
            _trimmingMode = Modes.Trimming.TRIMLESS_STATIC_F2B;

            _camera = new Camera(_width, _height, Vector3.Zero, Vector3.Zero, Vector3.UnitZ, 0.0f);
            _light = new Light(new Vector4(Vector3.Zero, 1.0f));

            for (int RenderingID = 0; RenderingID < 3; RenderingID++)
                _renderingMethods[RenderingID] = new List<Rendering>();

            FPS.init();
        }
        #endregion 

        #region Delete Functions
        public void delete()
        {
            _meshAnimation.delete();

            _weightSumSimple.delete();
            _averageColors.delete();
#if peel
            _weightSumF2B.delete();
            _weightSumDUAL.delete();
#endif
            foreach (Texture tex in _textures)
                tex.delete();
            for (int j=0; j<3; j++)
                foreach (Rendering ren in _renderingMethods[j])
                    ren.delete();
            //Camera - Light
            _camera.delete();
            _light.delete();
            // Shaders
            _renderVertex.delete();
            _renderGeometry.delete();

            if (_tessellation)
            {
                _renderVertexTess.delete();
                _renderTessellationControl.delete();
                _renderTessellationEvaluation.delete();
            }
            _renderGeometryRegions.delete();
            _renderFragmentRegions.delete();

            _renderFragment.delete();
            _phongFragment.delete();
            _computePixelColor.delete();

            _aabb_chVertex.delete();
            _aabb_chFragment.delete();
            _aabb_chRendering.delete();

            _thickVertex.delete();
            _thickCompute.delete();
#if peel
            _thickCompute_f2b.delete();
            _thickComputing.delete();
            _thickComputing_f2b.delete();
            _thickCompute_dual.delete();
            _thickComputing_dual.delete();
            _thickCompute_k_buffer.delete();
            _thickComputing_k_buffer.delete();
            _thickCompute_k_multi_buffer_Z.delete();
            _thickComputing_k_multi_buffer_Z.delete();
#endif
            _multiFragmentRendering.delete();

            FPS.delete();
        }
        #endregion 
        
        #region Shader Functions
        public void initShaders()
        {
            // Render Shaders
            {
                _renderVertex = new Shader("rendering/render", ShaderType.VertexShader);
                _renderVertexRegions = new Shader("rendering/render_regions", ShaderType.VertexShader);
                _renderGeometry = new Shader("rendering/render", ShaderType.GeometryShader);
                _renderGeometryRegions = new Shader("rendering/render_regions", ShaderType.GeometryShader);
                _renderFragment = new Shader("rendering/render", ShaderType.FragmentShader);
                _renderFragmentRegions = new Shader("rendering/render_regions", ShaderType.FragmentShader);
                _phongFragment = new Shader("rendering/illumination", ShaderType.FragmentShader);
                _computePixelColor = new Shader("rendering/compute_pixel_color", ShaderType.FragmentShader);

                _renderVertex.complile(ShaderType.VertexShader);
                _renderVertexRegions.complile(ShaderType.VertexShader);
                _renderGeometry.complile(ShaderType.GeometryShader);
                _renderGeometryRegions.complile(ShaderType.GeometryShader);
                _phongFragment.complile(ShaderType.FragmentShader);
                _computePixelColor.complile(ShaderType.FragmentShader);
                _renderFragment.complile(ShaderType.FragmentShader);
                _renderFragmentRegions.complile(ShaderType.FragmentShader);
            }

            // Tessellated Render Shaders
            if(_tessellation)
            {
                GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
                //float PatchInnerLevel = 1.0f;
                //float PatchOuterLevel = 1.0f;
                //GL.PatchParameter(PatchParameterFloat.PatchDefaultInnerLevel, ref PatchInnerLevel);
                //GL.PatchParameter(PatchParameterFloat.PatchDefaultOuterLevel, ref PatchOuterLevel);

                _renderVertexTess= new Shader("rendering/render_tess", ShaderType.VertexShader);
                _renderVertexTess.complile(ShaderType.VertexShader);

                _renderTessellationControl = new Shader("Control/render", ShaderType.TessControlShader);
                _renderTessellationControl.complile(ShaderType.TessControlShader);

                _renderTessellationEvaluation = new Shader("Evaluation/render", ShaderType.TessEvaluationShader);
                _renderTessellationEvaluation.complile(ShaderType.TessEvaluationShader);
            }
            // AABB-ConvexHull Shaders
            {
                _aabb_chVertex = new Shader("aabb-ch/render", ShaderType.VertexShader);
                _aabb_chFragment = new Shader("aabb-ch/render", ShaderType.FragmentShader);

                _aabb_chVertex.complile(ShaderType.VertexShader);
                _aabb_chFragment.complile(ShaderType.FragmentShader);

                _aabb_chRendering = new Shading();
                _aabb_chRendering.create();
                _aabb_chRendering.attachShader(_aabb_chVertex.id);
                _aabb_chRendering.attachShader(_aabb_chFragment.id);
                _aabb_chRendering.link();
                _aabb_chRendering.use();
                {
                    _aabb_chRendering.bindBuffer(0, "Camera", _camera.buffer.index, BufferRangeTarget.UniformBuffer);
                }
                Shading.close();
            }
            // Sphere Shaders
            {
                _sphereVertex = new Shader("sphere/render", ShaderType.VertexShader);
                _sphereFragment = new Shader("sphere/render", ShaderType.FragmentShader);

                _sphereVertex.complile(ShaderType.VertexShader);
                _sphereFragment.complile(ShaderType.FragmentShader);

                _sphereRendering = new Shading();
                _sphereRendering.create();
                _sphereRendering.attachShader(_sphereVertex.id);
                _sphereRendering.attachShader(_sphereFragment.id);
                _sphereRendering.link();
                _sphereRendering.use();
                {
                    _sphereRendering.bindBuffer(0, "Camera", _camera.buffer.index, BufferRangeTarget.UniformBuffer);
                }
                Shading.close();
            }

            // Thickness Shaders
            {
                _thickVertex = new Shader("rendering/translucency/compute", ShaderType.VertexShader);
                _thickVertex.complile(ShaderType.VertexShader);
                _thickCompute = new Shader("rendering/translucency/compute", ShaderType.FragmentShader);
                _thickCompute.complile(ShaderType.FragmentShader);

                _thickComputing = new Shading();
                _thickComputing.create();
                _thickComputing.attachShader(_thickVertex.id);
                _thickComputing.attachShader(_thickCompute.id);
                _thickComputing.link();

                _thickComputing.use();
                {
                    _thickComputing.bindBuffer(0, "Camera", _camera.buffer.index, BufferRangeTarget.UniformBuffer);
                }
                Shading.close();
#if peel
                _thickCompute_f2b = new Shader("rendering/translucency/f2b/compute", ShaderType.FragmentShader);
                _thickCompute_f2b.complile(ShaderType.FragmentShader);

                _thickComputing_f2b = new Shading();
                _thickComputing_f2b.create();
                _thickComputing_f2b.attachShader(Texture.vertex.id);
                _thickComputing_f2b.attachShader(_thickCompute_f2b.id);
                _thickComputing_f2b.link();

                _thickComputing_f2b.use();
                {
                    _thickComputing_f2b.bindUniformMatrix4("modelview_matrix", false, ref Texture.modelview_matrix);
                }
                Shading.close();

                _thickCompute_dual = new Shader("rendering/translucency/dual/compute", ShaderType.FragmentShader);
                _thickCompute_dual.complile(ShaderType.FragmentShader);

                _thickComputing_dual = new Shading();
                _thickComputing_dual.create();
                _thickComputing_dual.attachShader(Texture.vertex.id);
                _thickComputing_dual.attachShader(_thickCompute_dual.id);
                _thickComputing_dual.link();

                _thickComputing_dual.use();
                {
                    _thickComputing_dual.bindUniformMatrix4("modelview_matrix", false, ref Texture.modelview_matrix);
                }
                Shading.close();

                _thickCompute_k_buffer = new Shader("rendering/translucency/k_buffer/compute", ShaderType.FragmentShader);
                _thickCompute_k_buffer.complile(ShaderType.FragmentShader);

                _thickComputing_k_buffer = new Shading();
                _thickComputing_k_buffer.create();
                _thickComputing_k_buffer.attachShader(Texture.vertex.id);
                _thickComputing_k_buffer.attachShader(_thickCompute_k_buffer.id);
                _thickComputing_k_buffer.link();

                _thickComputing_k_buffer.use();
                {
                    _thickComputing_k_buffer.bindUniformMatrix4("modelview_matrix", false, ref Texture.modelview_matrix);
                }
                Shading.close();

                _thickCompute_k_multi_buffer_Z = new Shader("rendering/translucency/k_multi_buffer_Z/compute", ShaderType.FragmentShader);
                _thickCompute_k_multi_buffer_Z.complile(ShaderType.FragmentShader);

                _thickComputing_k_multi_buffer_Z = new Shading();
                _thickComputing_k_multi_buffer_Z.create();
                _thickComputing_k_multi_buffer_Z.attachShader(Texture.vertex.id);
                _thickComputing_k_multi_buffer_Z.attachShader(_thickCompute_k_multi_buffer_Z.id);
                _thickComputing_k_multi_buffer_Z.link();

                _thickComputing_k_multi_buffer_Z.use();
                {
                    _thickComputing_k_multi_buffer_Z.bindUniformMatrix4("modelview_matrix", false, ref Texture.modelview_matrix);
                }
                Shading.close();

                _thickCompute_k_stencil_buffer = new Shader("rendering/translucency/k_stencil_buffer/compute", ShaderType.FragmentShader);
                _thickCompute_k_stencil_buffer.complile(ShaderType.FragmentShader);

                _thickComputing_k_stencil_buffer = new Shading();
                _thickComputing_k_stencil_buffer.create();
                _thickComputing_k_stencil_buffer.attachShader(Texture.vertex.id);
                _thickComputing_k_stencil_buffer.attachShader(_thickCompute_k_stencil_buffer.id);
                _thickComputing_k_stencil_buffer.link();

                _thickComputing_k_stencil_buffer.use();
                {
                    _thickComputing_k_stencil_buffer.bindUniformMatrix4("modelview_matrix", false, ref Texture.modelview_matrix);
                }
                Shading.close();
#endif
            }
        }
        #endregion       

        #region Transparency Initing
        public void initTransparency()
        {
            _weightSumSimple = new WeightSumSimple();
            _averageColors   = new AverageColors();
#if peel      
            _weightSumF2B    = new WeightSumF2B();
            _weightSumDUAL   = new WeightSumDual();
#endif
        }
        #endregion

        #region Add Functions
        public void addModel(TreeView tree, int count, ProgressBar pBar, ToolStripStatusLabel tLabel, ContextMenuStrip cMenuStrip)
        {
            if(_meshAnimation.addModel(tree, count, pBar, tLabel, cMenuStrip))
                return;
#if !peel
            if(_meshAnimation.ignoreGroups)
                _meshAnimation.addMeanPose(tree, cMenuStrip);
#endif
            // Setup Camera
            const float theta = (float)Math.PI / 8.0f;
            float dis = ((Math.Max(Math.Abs(_meshAnimation.max.X - _meshAnimation.min.X), Math.Abs(_meshAnimation.max.Z - _meshAnimation.min.Z))) / (float)Math.Sin(theta)) + 0.001f;
            Vector3 eye = new Vector3(_meshAnimation.center.X + dis, _meshAnimation.center.Y + dis, Math.Abs(_meshAnimation.center.Z));
            _camera.reAlloc(eye, _meshAnimation.center, dis);
            _camera.load_projection_matrix(_meshAnimation.min, _meshAnimation.max, false);

            // to be removed
            _camera.view = Camera.View.User;
            _camera.fov = ((float)(Math.PI)) * (45.0f) / 180f;
            _camera.change_view();
            _camera.load_projection_matrix(_meshAnimation.min, _meshAnimation.max, false);

            // Setup Light
            _light = new Light(new Vector4(0.0f, 0.0f, 1.0f, 1.0f));
        }
        public void addTexture(TreeView tree, ContextMenuStrip cMenuStrip)
        {
            Texture t = Texture.OpenFile();
            if (t == null)
                return;

            _textures.Add(t);
            int j = _textures.Count - 1;
            tree.Nodes[1].Nodes.Add(_textures[j].name);
            tree.Nodes[1].Nodes[j].Tag = j;
            tree.Nodes[1].Nodes[j].ContextMenuStrip = cMenuStrip;
            tree.ExpandAll();
        }
        public void addRenderingMethod(ref Rendering rendering, int j, ComboBox comboBox, string name)
        {
            _renderingMethods[j].Add(rendering);
            if(j>0) comboBox.Items.Add(name);
        }
        #endregion

        #region Remove Functions
        public void removeModel(TreeView tree)
        {
            if (meshAnimation.removeModel(tree))
                return;

            // Setup Camera
            const float theta = (float)Math.PI / 8.0f;
            float dis = ((Math.Max(Math.Abs(_meshAnimation.max.X - _meshAnimation.min.X), Math.Abs(_meshAnimation.max.Z - _meshAnimation.min.Z))) / (float)Math.Sin(theta)) + 0.001f;
            Vector3 eye = new Vector3(_meshAnimation.center.X + dis, _meshAnimation.center.Y + dis, Math.Abs(_meshAnimation.center.Z));
            _camera.reAlloc(eye, _meshAnimation.center, dis);
            _camera.load_projection_matrix(_meshAnimation.min, _meshAnimation.max, false);

            // Setup Light
            //_light = new Light(new Vector4(eye, 1.0f));
            _light = new Light(new Vector4(0.0f, 0.0f, 1.0f, 1.0f));
        }
        public void removeTexture(TreeView tree)
        {
            if (_selectedTexture == -1)
                return;
            int removedModelIndex = _selectedTexture;

            _meshAnimation.poses.RemoveAt(removedModelIndex);
            tree.Nodes[0].Nodes.RemoveAt(removedModelIndex);           
        }
        #endregion

        #region Draw  Functions
        public  void   draw()
        {
            update();

           // if (_translucent && _renderingMode == Modes.Rendering.RENDER)
             //   _renderingMethods[1][1].draw(ref _models);
            
            FPS.resetLocal();
            FPS.beginGlobal();
            {
                _renderingMethods[(int)_renderingMode][_selectedRendering].draw();
            }
            FPS.endGlobalCPU();
            FPS.updateLocalForm();
#if CSG
            _multiFragmentRendering.csgDraw();
#endif
            drawBackgroundTexture(); 
        }
        private void   drawBackgroundTexture()
        {
            if (_useBackgroundTexture && _backgroundTexture != null)
            {
                Depth.disableTest();
                Depth.setMask(false);
                Blend.equation(BlendEquationMode.FuncAdd);
                Blend.funcSeparate(BlendingFactorSrc.OneMinusDstAlpha, BlendingFactorDest.DstAlpha, BlendingFactorSrc.Zero, BlendingFactorDest.Zero);
                Blend.enable();
                {
                    _backgroundTexture.drawB();
                }
                Blend.disable();
                Depth.enableTest();
                Depth.setMask(true);
            }
        }
        #endregion

        #region Update Functions
        private void   update()
        {
            camera.load_projection_matrix(_meshAnimation.min, _meshAnimation.max, false);
            camera.load_modelview_matrix();
            camera.update();
            light.update();
        }
        #endregion 
                
        #region Animate Functions
        public void animate()
        {
            int numOfPoses = (_meshAnimation.addMeanPoseToTree) ? _meshAnimation.poses.Count - 1 : _meshAnimation.poses.Count;
            // Skinning Mesh Animation
            if (_meshAnimation.sma != null && _meshAnimation.sma.selectedPose > -1)
            {
                if (++_meshAnimation.sma.selectedPose == _meshAnimation.sma.numPoses)
                    _meshAnimation.sma.selectedPose = 0;
                if (_meshAnimation.sma.selectedPose == 0 && _animationGUI.animation_stop)
                    _animationGUI.play = false;
                _meshAnimation.selectedPose = _meshAnimation.sma.selectedPose;
            }
            // Mesh Animation
            else
            {
                if (++_meshAnimation.selectedPose == numOfPoses)
                {
                    _meshAnimation.selectedPose = _meshAnimation.selectedRestPose;
                    // Clustering Per Pose - Avoid rendering Rest Pose Clustering (has only 1 Cluster)
                    if (_meshAnimation.vColoringMode == Modes.VertexColoring.CLUSTER && _meshAnimation.clusteringPerPose == true)
                        _meshAnimation.selectedPose++;
                }
                if (_meshAnimation.selectedPose == _meshAnimation.selectedRestPose &&
                    _animationGUI.animation_stop)
                    _animationGUI.play = false;
            }
        }
        #endregion 

        #region Shader Functions
        public void updateShaders(ref Shading rendering)
        {
            rendering.use();
            {
                // Depth
             //   if (_camera.inverseZ)
                {
               //     rendering.bindUniform1("zNear", _camera.zFar);
                 //   rendering.bindUniform1("zFar", _camera.zNear);
                }
                //else
                {
                    rendering.bindUniform1("zNear", _camera.zNear);
                    rendering.bindUniform1("zFar", _camera.zFar);
                }
                // Clipping
                rendering.bindUniform1("cappingPlane", _multiFragmentRendering.cappingPlane);
                rendering.bindUniform1("cappingAngle", _multiFragmentRendering.cappingAngle);
                // Gamma
                rendering.bindUniform1("gamma", _gamma);
                // Light
                rendering.bindBuffer(1, "Light", _light.buffer.index, BufferRangeTarget.UniformBuffer);
                rendering.bindUniform1("IlluminationMode", (int)_illuminationMode);
            }
            Shading.close();
        }
        #endregion
    }
}