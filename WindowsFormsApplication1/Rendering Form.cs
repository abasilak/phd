using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.ComponentModel;
using Cloo;
using OpenTK.Graphics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace abasilak
{
    public partial class Example : Form
    {
        // required for OpenCL-OpenGL interop
        [DllImport("opengl32.dll")]
        extern static IntPtr wglGetCurrentDC();

        bool loaded = false;
        bool _modelSelectionChanged;     
        public static Scene _scene;
        public static ComputeContext context;
        int _dedicated, _dedicated_free, _total;

        #region Execute Commands
        public static int ExecuteCommand(string Command, int Timeout)
        {
            int ExitCode;
            ProcessStartInfo ProcessInfo;
            Process Process;
            ProcessInfo = new ProcessStartInfo("cmd.exe /C " + Command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;
            Process = Process.Start(ProcessInfo);
            Process.WaitForExit();
            ExitCode = Process.ExitCode;
            Process.Close();

            return ExitCode;
        }
        public static int ExecuteCommand(string ExecutablePath, string arguments, int Timeout)
        {
            int ExitCode = 1;
            try
            {
                ProcessStartInfo ProcessInfo;
                Process Process;
                ProcessInfo = new ProcessStartInfo(ExecutablePath, arguments);
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = false;
                Process = Process.Start(ProcessInfo);
                Process.WaitForExit();
                ExitCode = Process.ExitCode;
                Process.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            return ExitCode;
        }
        #endregion
        private void RenderWindow_Load(object sender, EventArgs e)
        {
            loaded = true;
            Application.Idle += new EventHandler(RenderWindow_Idle);

#if peel
            //---------------------------------------------------------------------------------
            Console.WriteLine(" ---------- OpenGL ---------- ");
            Console.WriteLine("Version:   " + GL.GetString(StringName.Version));
            Console.WriteLine("GLSL:      " + GL.GetString(StringName.ShadingLanguageVersion));
            Console.WriteLine("Renderer:  " + GL.GetString(StringName.Renderer));
            Console.WriteLine("Vendor:    " + GL.GetString(StringName.Vendor));
            //Console.WriteLine("Extensions:" + GL.GetString(StringName.Extensions));
            //---------------------------------------------------------------------------------        

            GL.GetInteger((GetPName)0x9047, out _dedicated);
            GL.GetInteger((GetPName)0x9048, out _total);
            GL.GetInteger((GetPName)0x9049, out _dedicated_free);

            Console.WriteLine();
            Console.WriteLine(" ---------- GPU Memory ---------- ");
            Console.WriteLine("Dedicated:           " + ((float)_dedicated/1024.0f).ToString() + "MB");
            Console.WriteLine("Dedicated (Free):    " + ((float)_dedicated_free / 1024.0f).ToString() + "MB");
            Console.WriteLine("Total:               " + ((float)_total/ 1024.0f).ToString() + "MB");

            //---------------------------------------------------------------------------------        
            ComputePlatform platform = ComputePlatform.Platforms[0];
            Console.WriteLine();
            Console.WriteLine(" ----------  OpenCL  ---------- ");
            Console.WriteLine(" ---------- platform ---------- ");
            Console.WriteLine("Name:      " + platform.Name);
            Console.WriteLine("Version:   " + platform.Version);
            Console.WriteLine("Vendor :   " + platform.Vendor);
            Console.WriteLine("Profile:   " + platform.Profile);
            Console.WriteLine(" ----------  device  ---------- ");
            Console.WriteLine("Profile:   " + platform.Devices[0].Type.ToString());

            IntPtr                      DC  = wglGetCurrentDC();        
            IGraphicsContextInternal    Ctx = (OpenTK.Graphics.IGraphicsContextInternal)OpenTK.Graphics.GraphicsContext.CurrentContext;
            IntPtr RawContextHandle         = Ctx.Context.Handle;

            // Create the context property list and populate it.
            ComputeContextProperty p1 = new ComputeContextProperty(ComputeContextPropertyName.CL_GL_CONTEXT_KHR, RawContextHandle);
            ComputeContextProperty p2 = new ComputeContextProperty(ComputeContextPropertyName.CL_WGL_HDC_KHR, DC);
            ComputeContextProperty p3 = new ComputeContextProperty(ComputeContextPropertyName.Platform, platform.Handle.Value);
            ComputeContextPropertyList properties = new ComputeContextPropertyList(new ComputeContextProperty[] { p1, p2, p3 });

            context = new ComputeContext(platform.Devices, properties, null, IntPtr.Zero);
            //context = new ComputeContext( ComputeDeviceTypes.Gpu, properties, null, IntPtr.Zero);
#else
            GL.GetInteger((GetPName)0x9049, out _dedicated_free);
#endif
            //---------------------------------------------------------------------------
            _scene = new Scene(RenderWindow.Width, RenderWindow.Height);
            _scene.initShaders();
            _scene.initTransparency();

#if csg
            _scene.multiFragmentRendering.initTexRule_CopyRules();
#endif
            // Create Tree Node
            treeView_models.Nodes.Add("Mesh Animation Sequence");
            treeView_models.Nodes[0].Tag = -1;
            treeView_models.Nodes.Add("Textures");
            treeView_models.Nodes[1].Tag = -2;

            comboBoxRenderingType.Items.Add("Render");
            Rendering illumination = new Illumination();
            _scene.addRenderingMethod(ref illumination, 0, null, null);

#if peel
            /// 
            /// Peeling 
            ///
            comboBoxRenderingType.Items.Add("Peeling");
#if F2B
            // F2B Methods
            Rendering f2b = new F2B(); _scene.addRenderingMethod(ref f2b, 1, comboBox2, "F2B");
            //Rendering f2b2 = new F2B2(); _scene.addRenderingMethod(ref f2b2, 1, comboBox2, "F2B2");
            //Rendering f2b_3P_max = new F2B_3P_Max(); _scene.addRenderingMethod(ref f2b_3P_max, 1, comboBox2, "F2B-3P-MAX");
            //Rendering f2b_3P_min_max = new F2B_3P_MinMax(); _scene.addRenderingMethod(ref f2b_3P_min_max, 1, comboBox2, "F2B-3P-MIN/MAX");
            //Rendering f2b_2P_max = new F2B_2P_Max(); _scene.addRenderingMethod(ref f2b_2P_max, 1, comboBox2, "F2B-2P-MAX");
            //Rendering f2b_2P_min_max = new F2B_2P_MinMax(); _scene.addRenderingMethod(ref f2b_2P_min_max, 1, comboBox2, "F2B-2P-MIN/MAX");
            //Rendering f2b_KB = new F2B_KB(); _scene.addRenderingMethod(ref f2b_KB, 1, comboBox2, "F2B-KB");
            //Rendering f2b_FreePipe = new F2B_FreePipe(); _scene.addRenderingMethod(ref f2b_FreePipe, 1, comboBox2, "F2B-FreePipe");
            //Rendering f2b_LL= new F2B_LL(); _scene.addRenderingMethod(ref f2b_LL, 1, comboBox2, "F2B-LL");
#endif

#if DUAL
            // DUAL Methods
            Rendering dual = new DUAL(); _scene.addRenderingMethod(ref dual, 1, comboBox2, "DUAL");
            //Rendering dual_3P_max = new DUAL_3P_Max(); _scene.addRenderingMethod(ref dual_3P_max, 1, comboBox2, "DUAL-3P-MAX");
            //Rendering dual_3P_min_max = new DUAL_3P_MinMax(); _scene.addRenderingMethod(ref dual_3P_min_max, 1, comboBox2, "DUAL-3P-MIN/MAX");
            //Rendering dual_2P_max = new DUAL_2P_Max(); _scene.addRenderingMethod(ref dual_2P_max, 1, comboBox2, "DUAL-2P-MAX");
            //Rendering dual_2P_min_max = new DUAL_2P_MinMax(); _scene.addRenderingMethod(ref dual_2P_min_max, 1, comboBox2, "DUAL-2P-MIN/MAX");
            //Rendering dual_K_1b = new DUAL_KB_1B(); _scene.addRenderingMethod(ref dual_K_1b, 1, comboBox2, "DUAL-K-1B");
            //Rendering dual_K_2b = new DUAL_KB_2B(); _scene.addRenderingMethod(ref dual_K_2b, 1, comboBox2, "DUAL-K-2B");
            //Rendering dual_FreePipe = new DUAL_FreePipe(); _scene.addRenderingMethod(ref dual_FreePipe, 1, comboBox2, "DUAL-FreePipe");
            //Rendering dual_LL= new DUAL_LL(); _scene.addRenderingMethod(ref dual_LL, 1, comboBox2, "DUAL-LL");
#endif
#if KB
            // K Methods
            //Rendering kb = new KB();                                _scene.addRenderingMethod(ref kb, 1, comboBox2, "KB");
            //Rendering kb_Faster = new KB_Faster();                  _scene.addRenderingMethod(ref kb_Faster, 1, comboBox2, "KB-Faster"); // AtomicBarrier(); ???
            //Rendering kb_Multi = new KB_Multi();                    _scene.addRenderingMethod(ref kb_Multi, 1, comboBox2, "KB-Multi");
            //Rendering kb_SR = new KB_SR();                          _scene.addRenderingMethod(ref kb_SR, 1, comboBox2, "KB-SR");

            //Rendering kb_Array_Max = new KB_Array_Max();            _scene.addRenderingMethod(ref kb_Array_Max, 1, comboBox2, "K+B-Array-Max");
            //Rendering kb_Array_Max_V2 = new KB_Array_Max_V2();      _scene.addRenderingMethod(ref kb_Array_Max_V2, 1, comboBox2, "K+B-Array-Max-V2");
            //Rendering kb_Heap_Max = new KB_Heap_Max();              _scene.addRenderingMethod(ref kb_Heap_Max, 1, comboBox2, "K+B-Heap-Max");
            //Rendering kb_Array_Ins = new KB_Array_Ins();            _scene.addRenderingMethod(ref kb_Array_Ins, 1, comboBox2, "K+B-Array-Ins");

            //Rendering kb_MDT_32 = new KB_MDT_32();                  _scene.addRenderingMethod(ref kb_MDT_32, 1, comboBox2, "KB-MDT-32");
            //Rendering kb_MDT_64 = new KB_MDT_64();                  _scene.addRenderingMethod(ref kb_MDT_64, 1, comboBox2, "KB-MDT-64");

            //Rendering kb_AB_Array = new KB_AB_Array();              _scene.addRenderingMethod(ref kb_AB_Array, 1, comboBox2, "KB-AB-Array");
            //Rendering kb_AB_LL = new KB_AB_LL();                    _scene.addRenderingMethod(ref kb_AB_LL, 1, comboBox2, "KB-AB-LL");
            //Rendering kb_AB_SB = new KB_AB_SB();                    _scene.addRenderingMethod(ref kb_AB_SB, 1, comboBox2, "KB-AB-SB");
            //Rendering kb_LL = new KB_LL();                          _scene.addRenderingMethod(ref kb_LL, 1, comboBox2, "KB-LL");
            //Rendering kb_LL_sync = new KB_LL_Sync();                  _scene.addRenderingMethod(ref kb_LL_sync, 1, comboBox2, "KB-LL-Sync");
            //Rendering kb_SB      = new KB_SB();                          _scene.addRenderingMethod(ref kb_SB, 1, comboBox2, "K+B-SB");

            //Rendering kb_Array_Max_DynΚ = new KB_Array_Max_DynK();      _scene.addRenderingMethod(ref kb_Array_Max_DynΚ, 1, comboBox2, "K+B-Array-Max-DynK");
            Rendering kb_SB_DynΚ = new KB_SB_DynK();                    _scene.addRenderingMethod(ref kb_SB_DynΚ, 1, comboBox2, "K+B-SB-DynK");

            /*
             *                  CLIPPING ENABLED
             */

            //Rendering kb_CL = new KB_Clipping();                      _scene.addRenderingMethod(ref kb_CL, 1, comboBox2, "KB-Clipping");
            //Rendering kb_SR_CL = new KB_SR_Clipping();                _scene.addRenderingMethod(ref kb_SR_CL, 1, comboBox2, "KB-SR-Clipping");
            //Rendering kb_Array_Max_CL = new KB_Array_Max_Clipping();  _scene.addRenderingMethod(ref kb_Array_Max_CL, 1, comboBox2, "K+B-Array-Max-Clipping");
            //Rendering kb_Array_Ins_CL = new KB_Array_Ins_Clipping();  _scene.addRenderingMethod(ref kb_Array_Ins_CL, 1, comboBox2, "KB-Array-Ins-Clipping");
            //Rendering kb_MDT_32_CL = new KB_MDT_32_Clipping();        _scene.addRenderingMethod(ref kb_MDT_32_CL, 1, comboBox2, "KB-MDT-32-Clipping");
            //Rendering kb_MDT_64_CL = new KB_MDT_64_Clipping();        _scene.addRenderingMethod(ref kb_MDT_64_CL, 1, comboBox2, "KB-MDT-64-Clipping");
            //Rendering kb_AB_LL_CL = new KB_AB_LL_Clipping();          _scene.addRenderingMethod(ref kb_AB_LL_CL, 1, comboBox2, "KB-AB-LL-Clipping");
            //Rendering kb_LL_CL = new KB_LL_Clipping();                _scene.addRenderingMethod(ref kb_LL_CL, 1, comboBox2, "KB-LL-Clipping");
            //Rendering kb_SB_CL = new KB_SB_Clipping();                  _scene.addRenderingMethod(ref kb_SB_CL, 1, comboBox2, "K+B-SB-Clipping");
#endif
#if BUCKET
            // Bucket Methods
            //Rendering bun    = new BUN();    _scene.addRenderingMethod(ref bun, 1, comboBox2, "BUN"); 
            //Rendering bun_3P = new BUN_3P(); _scene.addRenderingMethod(ref bun_3P, 1, comboBox2, "BUN-3P");
            //Rendering bad = new BAD(); _scene.addRenderingMethod(ref bad, 1, comboBox2, "BAD");
#endif
#if AB
                                    //      --- A-Buffer Methods ---      //
            //Rendering ab_Array      = new AB_Array();           _scene.addRenderingMethod(ref ab_Array    , 1, comboBox2, "AB-Array");
            //Rendering ab_LL         = new AB_LL();              _scene.addRenderingMethod(ref ab_LL   , 1, comboBox2, "AB-LL");
            //Rendering ab_LL_s         = new AB_LL_Storage();      _scene.addRenderingMethod(ref ab_LL_s , 1, comboBox2, "AB-LL-S");
            //Rendering ab_LL_p       = new AB_LL_Paged();        _scene.addRenderingMethod(ref ab_LL_p , 1, comboBox2, "AB-LL-P");
            //Rendering ab_ll_bun       = new AB_LL_Bucket();         _scene.addRenderingMethod(ref ab_ll_bun, 1, comboBox2, "AB-LL-BUN");
            //Rendering ab_LL_sync      = new AB_LL_Sync();         _scene.addRenderingMethod(ref ab_LL_sync, 1, comboBox2, "AB-LL-Sync");

            // 2 Passes
            //Rendering ab_sb              = new AB_SB();             _scene.addRenderingMethod(ref ab_sb, 1, comboBox2, "AB-SB");
            //Rendering ab_precalc_opencl  = new AB_PreCalc_OpenCL(); _scene.addRenderingMethod(ref ab_precalc_opencl, 1, comboBox2, "AB-PreCalc-OpenCL");
            //Rendering ab_precalc_fixed   = new AB_PreCalc_Fixed();  _scene.addRenderingMethod(ref ab_precalc_fixed , 1, comboBox2, "AB-PreCalc-Fixed"); 
#endif
#if AB_Β2
            // 2 Buckets - Color & Depth
            //Rendering ab_Array_b2 = new AB_Array_B2(); _scene.addRenderingMethod(ref ab_Array_b2, 1, comboBox2, "AB-Array-B2");
            //Rendering ab_LL_b2      = new AB_LL_B2();           _scene.addRenderingMethod(ref ab_LL_b2, 1, comboBox2, "AB-LL-B2");
            //Rendering ab_LL_p_b2    = new AB_LL_Paged_B2();     _scene.addRenderingMethod(ref ab_LL_p_b2, 1, comboBox2, "AB-LL-Paged-B2");
            //Rendering ab_sb_b2      = new AB_SB_B2();           _scene.addRenderingMethod(ref ab_sb_b2, 1, comboBox2, "AB-SB-B2");
#endif
#endif

#if trim
            /// 
            /// Trimming - CSG
            ///
            comboBoxRenderingType.Items.Add("Trimming");
#if TRIMLESS
            // Trimless Methods
            Rendering trimless_static_f2b   = new Trimless_Static_F2B(); _scene.addRenderingMethod(ref trimless_static_f2b, 2, comboBox11, "TRIMLESS STATIC-F2B");
            Rendering trimless_static_dual = new Trimless_Static_DUAL(); _scene.addRenderingMethod(ref trimless_static_dual, 2, comboBox11, "TRIMLESS STATIC-DUAL");
            Rendering trimless_static_2p = new Trimless_Static_2Passes(); _scene.addRenderingMethod(ref trimless_static_2p, 2, comboBox11, "TRIMLESS STATIC-2PASSES");
            Rendering trimless_dynamic_2p = new Trimless_Dynamic_2Passes(); _scene.addRenderingMethod(ref trimless_dynamic_2p, 2, comboBox11, "TRIMLESS DYNAMIC-2PASSES");
#endif
#if TRIMMING
            // Trimming Methods
            Rendering trimming_static = new Trimming_Static(); _scene.addRenderingMethod(ref trimming_static, 2, comboBox11, "TRIMMING STATIC");
            Rendering trimming_dynamic = new Trimming_Dynamic(); _scene.addRenderingMethod(ref trimming_dynamic, 2, comboBox11, "TRIMMING DYNAMIC");
#endif
#if CSG
            // CSG Methods
            _scene.multiFragmentRendering.trimming_csg   = new Trimming_CSG();
            _scene.multiFragmentRendering.trimming_csg_Z = new Trimming_CSG_Z();
#endif
#endif
            //---------------------------------------------------------------------------------------
            Depth.enableTest();
            Depth.setMask(true);
            Depth.enableClamp();
            GL.Enable(EnableCap.Dither); // Dither.Enable();

            initGUI();
        }
        private void RenderWindow_Draw(object sender, PaintEventArgs e)
        {
            if (!loaded) return;
            RenderWindow.MakeCurrent();

            // Scene Draw
            _scene.draw();

            if (_scene.meshAnimation.selectedPose > -1)
                _modelSelectionChanged = false;

#if peel
            textBoxTotalPasses.Text = _scene.multiFragmentRendering.total_passes.ToString();

            if (_scene.renderingMode == Modes.Rendering.PEELING)
            {
                //if (_scene.peelingMode == Modes.Peeling.LINKED_LISTS || _scene.peelingMode == Modes.Peeling.S_BUFFER || _scene.peelingMode == Modes.Peeling.PRECALC_OPENCL)
                textBoxMemorySize.Text = _scene.peelingMethods[_scene.selectedRendering].memory.ToString("F") + " MB";
                textBoxKValue.Text     = _scene.peelingMethods[_scene.selectedRendering].sizeArrayHeap.ToString();
                textBoxKPercentage.Text = _scene.peelingMethods[_scene.selectedRendering].peelingPercentage.ToString("F");

                if (_scene.peelingMode == Modes.Peeling.F2B)
                { 
                    if (_scene.meshAnimation.selectedPose == -1)
                        _scene.meshAnimation.maxLayers      = _scene.multiFragmentRendering.total_passes;
                    else
                        _scene.meshAnimation.pose.maxLayers = _scene.multiFragmentRendering.total_passes;
                }
            }

            if (_scene.multiFragmentRendering.enablePeelingError)
            {
                textBox9.Text = _scene.multiFragmentRendering.samples.ToString();
                textBox10.Text = _scene.multiFragmentRendering.totalSamples.ToString();
                textBox11.Text = (_scene.multiFragmentRendering.samples / _scene.multiFragmentRendering.totalSamples).ToString();
            }
#endif

            // Swap
            RenderWindow.SwapBuffers();
        }
        private void RenderWindow_Delete() 
        {
            _scene.delete();
            Texture.deleteAll();
        }

/******
 * 
 *          INPUT
 * 
 * *****/
        private void RenderWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (!loaded) return;

            if (e.KeyCode == Keys.Escape)
            {
                RenderWindow_Delete();
                Application.Exit();
            }
            else if (e.KeyCode == Keys.A || e.KeyCode == Keys.D || e.KeyCode == Keys.W || e.KeyCode == Keys.S || e.KeyCode == Keys.Q || e.KeyCode == Keys.E)
                                                   _scene.meshAnimation.translate(e.KeyCode);
            else if (e.KeyCode == Keys.ControlKey) _scene.camera.controlKey = true;
            else if (e.KeyCode == Keys.ShiftKey)   _scene.camera.shiftKey   = true;
        }
        private void RenderWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (!loaded) return;

            if (e.KeyCode == Keys.ControlKey)    _scene.camera.controlKey = false;
            else if (e.KeyCode == Keys.ShiftKey) _scene.camera.shiftKey   = false;
        }
        private void RenderWindow_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!_scene.light.transform)
                _scene.camera.mouse(e.X, e.Y, e.Button);
            else
            {
                Camera camera = _scene.camera;
                _scene.light.mouse(e.X, e.Y, e.Button, ref camera, _scene.width, _scene.height);
            }
        }
        private void RenderWindow_Idle(object sender, EventArgs e)
        {
            if (_scene.animationGUI.play && !_scene.animationGUI.pause)
            {
                if (_scene.camera.animation)
                    button19_Click(sender, e);
                else
                {
                    if (_scene.meshAnimation.sma != null && _scene.meshAnimation.sma.selectedPose > -1)
                        treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes[_scene.meshAnimation.sma.selectedPose].BackColor = System.Drawing.Color.White;
                    else
                        treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].BackColor = System.Drawing.Color.White;
                    {
                        _scene.animate();
                    }
                    if (_scene.meshAnimation.sma != null && _scene.meshAnimation.sma.selectedPose > -1)
                        treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes[_scene.meshAnimation.sma.selectedPose].BackColor = System.Drawing.Color.Orange;
                    else
                        treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].BackColor = System.Drawing.Color.Red;
                }
            }
#if FPS
            while (true)
            {
                FPS.endGlobalCPU();

                int CurrentFPS = (int)(1.0 / FPS.durationGlobal);
                if (FPS.maxValue != 0 && CurrentFPS > FPS.maxValue)
                    continue;

                this.Text = FPS.setText(this.Text);
                FPS.beginGlobalCPU();
                break;
            }
#endif
            RenderWindow.Invalidate();
        }
        private void RenderWindow_Resize(object sender, EventArgs e)
        {
            if (!loaded) return;

            _scene.camera.load_projection_matrix(_scene.meshAnimation.min, _scene.meshAnimation.max, false);
            RenderWindow.Invalidate();
        }
        public Example() { InitializeComponent(); }

/******
 * 
 *          GUI
 * 
 * *****/

        private void initGUI()
        {
            textBox1.Text = _scene.camera.zNear.ToString();
            textBox2.Text = _scene.camera.zFar.ToString();

            numericUpDown_gamma.Value=(decimal)_scene.gamma;
            numericUpDown1.Value = (decimal)(180f * ((float)_scene.camera.fov) / (float)(Math.PI));
            numericUpDown5.Value = (decimal)_scene.light._ubo._attenuation.constant;
            numericUpDown6.Value = (decimal)_scene.light._ubo._attenuation.linear;
            numericUpDown7.Value = (decimal)_scene.light._ubo._attenuation.quadratic;
            numericUpDown8.Value = (decimal)_scene.light._ubo._k.a;
            numericUpDown9.Value = (decimal)_scene.light._ubo._k.d;
            numericUpDown10.Value = (decimal)_scene.light._ubo._k.s;
            numericUpDown10.Value = (decimal)_scene.light._ubo._k.s;
            numericUpDownTessLevelInner.Value = (decimal)1;
            numericUpDownTessLevelOuter.Value = (decimal)1;
            numericUpDownMergingPrevTolerance.Value = (decimal)0;
            numericUpDownPercentageSpectral.Value = (decimal)1;
            numericUpDown21.Value = (decimal)_scene.multiFragmentRendering.restPose;
            numericUpDown22.Value = (decimal)_scene.camera.angle.X;
            numericUpDown23.Value = (decimal)_scene.camera.angle.Y;
            numericUpDown24.Value = (decimal)_scene.camera.angle.Z;
            numericUpDown32.Value = (decimal)_scene.discardThreshold;
            numericUpDown34.Value = (decimal)_scene.multiFragmentRendering.maxK;
            numericUpDown35.Value = (decimal)_scene.multiFragmentRendering.maxKerror;
            numericUpDown36.Value = (decimal)_scene.multiFragmentRendering.maxKmemory;
            numericUpDown36.Maximum = (decimal)((float)_dedicated_free / 1024.0f);
            numericUpDownClustersCount.Value = (decimal)35;
            numericUpDownClusteringError.Value = (decimal)0.0;
            numericUpDownClusteringIterations.Value = (decimal)1;
            numericUpDownClusteringTolerance.Value = (decimal)0.001;
            numericUpDownPFactor.Value = (decimal)_scene.meshAnimation.pCenter.pFactor;
            numericUpDownSelectedVertex.Value = (decimal) -1;
            numericUpDownFittingIterations.Value = (decimal)1;
#if peel
            comboBox2.SelectedIndex = (int)_scene.peelingMode;
#endif 
            comboBoxRenderingType.SelectedIndex = (int)_scene.renderingMode;
            comboBoxClusteringDistanceMode.SelectedIndex = 2;
            comboBoxDistanceMode.SelectedIndex = 0;
            comboBox3.SelectedIndex = 1;
            comboBox5.SelectedIndex = (int)_scene.illuminationMode;
            comboBox6.SelectedIndex = (int)_scene.transparencyMode;
            comboBox10.SelectedIndex = 2;
            comboBox12.SelectedIndex = 0;
            comboBoxVertexColoringMode.SelectedIndex = 0;
            comboBox13.SelectedIndex = 2;
            comboBoxDGmode.SelectedIndex = 0;
            comboBoxDGComponentsMode.SelectedIndex = 0;
            comboBoxClusteringMode.SelectedIndex = 0;
            comboBox17.SelectedIndex = 0;
            comboBox18.SelectedIndex = 2;
            comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.CLUSTER;

            comboBoxClusteringSpectralGraphMode.SelectedIndex = 0;
#if trim       
            comboBox11.SelectedIndex = (int)_scene.trimmingMode;
#endif
            _statusLabel.Text = "Scene is Empty...";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _scene.camera.clipping = !_scene.camera.clipping;
            _scene.camera.load_projection_matrix(_scene.meshAnimation.min, _scene.meshAnimation.max, false);

            textBox1.Text = _scene.camera.zNear.ToString();
            textBox2.Text = _scene.camera.zFar.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _scene.camera.view = (Camera.View)comboBox1.SelectedIndex;
            _scene.camera.change_view();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _scene.camera.fov = ((float)(Math.PI)) * ((float)numericUpDown1.Value) / 180f;
            _scene.camera.load_projection_matrix(_scene.meshAnimation.min, _scene.meshAnimation.max, false);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            _scene.light.spot();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
                _scene.light.colorChanged(0, colorDialog1.Color);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
                _scene.light.colorChanged(1, colorDialog1.Color);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
                _scene.light.colorChanged(2, colorDialog1.Color);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.pose.material.changed = true;
            _scene.meshAnimation.pose.material.shininess = (float)numericUpDown2.Value;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                _scene.meshAnimation.pose.material.changed = true;
                _scene.meshAnimation.pose.material.colorChanged(0, colorDialog1.Color);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                _scene.meshAnimation.pose.material.changed = true;
                _scene.meshAnimation.pose.material.colorChanged(1, colorDialog1.Color);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                _scene.meshAnimation.pose.material.changed = true;
                _scene.meshAnimation.pose.material.colorChanged(2, colorDialog1.Color);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_scene.renderingMode != Modes.Rendering.RENDER)
            {
                _scene.peelingMode = (Modes.Peeling)comboBox2.SelectedIndex;
                _scene.selectedRendering = (int)_scene.peelingMode;

                //checkBox6.Enabled=(_scene.peelingMode==Modes.Peeling.DUAL||
                //                   _scene.peelingMode==Modes.Peeling.DUAL_Z_2P||
                //                   _scene.peelingMode == Modes.Peeling.DUAL_Z_3P ||
                //                   _scene.peelingMode == Modes.Peeling.DUAL_Z_K ||
                //                   _scene.peelingMode == Modes.Peeling.DUAL_Z_K_WS
                //                   ) ? true : false;
                textBoxMemorySize.Text = _scene.peelingMethods[_scene.selectedRendering].memory.ToString("F") + " MB";
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.layers = (int)numericUpDown3.Value;
        }

        private void button_backGrColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                _scene.backgroundColor = colorDialog1.Color;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            _scene.renderingMode = (Modes.Rendering)comboBoxRenderingType.SelectedIndex;

            if(_scene.renderingMode==Modes.Rendering.RENDER)
                _scene.selectedRendering = 0;
            else if(_scene.renderingMode==Modes.Rendering.PEELING)
                _scene.selectedRendering = (int)_scene.peelingMode;
            else if (_scene.renderingMode == Modes.Rendering.TRIMMING)
                _scene.selectedRendering = (int)_scene.trimmingMode;

            comboBox6.Enabled=(_scene.renderingMode==Modes.Rendering.RENDER)?true:false;
            //groupBox2.Enabled=(_scene.renderingMode==Modes.Rendering.PEELING)?true:false;
            {
#if peel
                comboBox2.SelectedIndex = (int)_scene.peelingMode;
#endif
#if trim
                comboBox11.SelectedIndex= (int)_scene.trimmingMode;
#endif
                numericUpDown3.Value = _scene.multiFragmentRendering.layers;
                numericUpDown19.Value = (decimal)_scene.multiFragmentRendering.cappingPlane;
                numericUpDown20.Value = (decimal)_scene.multiFragmentRendering.cappingAngle;
                textBoxTotalPasses.Text = _scene.multiFragmentRendering.passes.ToString();

                if (_scene.renderingMode == Modes.Rendering.PEELING)
                    textBoxMemorySize.Text = _scene.peelingMethods[_scene.selectedRendering].memory.ToString("F") + " MB";
                else if (_scene.renderingMode == Modes.Rendering.TRIMMING)
                    textBoxMemorySize.Text = _scene.trimmingMethods[_scene.selectedRendering].memory.ToString("F") + " MB";
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.coloringMode = (Modes.Coloring)comboBox4.SelectedIndex;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.transparency = (float)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            _scene.light.changed = true;
            _scene.light._ubo._attenuation.constant = (float)numericUpDown5.Value;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            _scene.light.changed = true;
            _scene.light._ubo._attenuation.linear = (float)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            _scene.light.changed = true;
            _scene.light._ubo._attenuation.quadratic = (float)numericUpDown7.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            _scene.light.changed = true;
            _scene.light._ubo._k.a = (float)numericUpDown8.Value;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            _scene.light.changed = true;
            _scene.light._ubo._k.d = (float)numericUpDown9.Value;
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            _scene.light.changed = true;
            _scene.light._ubo._k.s = (float)numericUpDown10.Value;
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.pose.material.changed = true;
            _scene.meshAnimation.pose.material._ubo.ni = (float)numericUpDown11.Value;
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.pose.material.changed = true;
            _scene.meshAnimation.pose.material._ubo.absorption = (float)numericUpDown12.Value;
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.pose.material.changed = true;
            _scene.meshAnimation.pose.material._ubo.gaussian_m = (float)numericUpDown13.Value;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.translucent = !_scene.meshAnimation.pose.translucent;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            _scene.illuminationMode=(Modes.Illumination)comboBox5.SelectedIndex;
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.pose.material.changed = true;
            _scene.meshAnimation.pose.material._ubo.gaussian_c = (float)numericUpDown14.Value;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            _scene.transparencyMode=(Modes.Transparency)comboBox6.SelectedIndex;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.samplesQuery.use = !_scene.multiFragmentRendering.samplesQuery.use;
            _scene.multiFragmentRendering.samplesAnyQuery.use = !_scene.multiFragmentRendering.samplesAnyQuery.use;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _scene.useBackgroundTexture = !_scene.useBackgroundTexture;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.useFront = !_scene.multiFragmentRendering.useFront;
        }

        private void numericUpDown_gamma_ValueChanged(object sender, EventArgs e)
        {
            _scene.gamma = (float)numericUpDown_gamma.Value;
        }

        private void treeView_models_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Model
            if ((int)e.Node.Tag == -1)
            {
                if (_scene.meshAnimation.selectedPose != -1 && _scene.meshAnimation.poses.Count > 0)
                    _scene.meshAnimation.pose.wireframe = false;

                _scene.meshAnimation.selectedPose = -1;
                foreach(TreeNode node in e.Node.Nodes)
                    node.BackColor = System.Drawing.Color.White;
            }
            else if ((int)e.Node.Tag == -2)
            {
                _scene.selectedTexture = -1;
                foreach (TreeNode node in e.Node.Nodes)
                    node.BackColor = System.Drawing.Color.White;
            }
            else if ((int)e.Node.Parent.Tag == -1 && (int)e.Node.Tag > -1)
            {
                foreach (TreeNode node in e.Node.Parent.Nodes)
                    node.BackColor = System.Drawing.Color.White;

                e.Node.BackColor = System.Drawing.Color.Red;

                if (_scene.meshAnimation.selectedPoseDrawable == false && _scene.meshAnimation.selectedPose != -1 && _scene.meshAnimation.selectedPose < _scene.meshAnimation.poses.Count)
                    _scene.meshAnimation.pose.aabb.drawable = false;

                _scene.meshAnimation.selectedPose = (int)e.Node.Tag;
                _modelSelectionChanged = true;

                if (_scene.meshAnimation.selectedPoseDrawable == false)
                    _scene.meshAnimation.pose.aabb.drawable = true;

                textBox3.Text = _scene.meshAnimation.pose.name;
                textBox4.Text = _scene.meshAnimation.pose.poseID.ToString();
                textBox5.Text = _scene.meshAnimation.pose.verticesCount.ToString();

                int TotalTriangles = _scene.meshAnimation.pose.facetsCount;
                if (_scene.tessellation)
                    TotalTriangles *= GeometryFunctions.calculateTriangles((int)_scene.tessLevelInner);
                textBox6.Text = TotalTriangles.ToString();
                textBox7.Text = _scene.meshAnimation.pose.volume.ToString();

                checkBox5.Checked = _scene.meshAnimation.pose.translucent;
                checkBox3.Checked = _scene.meshAnimation.pose.drawable;
                checkBox7.Checked = _scene.meshAnimation.pose.aabb.drawable;
                checkBox8.Checked = _scene.meshAnimation.pose.stripXY;
                checkBox12.Checked = _scene.meshAnimation.pose.transparent;
                checkBox21.Checked = _scene.meshAnimation.pose.wireframe;

                comboBox4.SelectedIndex = (int)_scene.meshAnimation.pose.coloringMode;
                comboBox7.SelectedIndex = _scene.meshAnimation.pose.primitiveMode == PrimitiveType.Triangles ? 0 : 1;
                comboBox8.SelectedIndex = (int)_scene.meshAnimation.pose.texturingApp;
                comboBox9.SelectedIndex = (int)_scene.meshAnimation.pose.texturingPar;
                comboBoxVertexColoringMode.SelectedIndex = (int)_scene.meshAnimation.vColoringMode;

                numericUpDown2.Value = (decimal)_scene.meshAnimation.pose.material.shininess;
                numericUpDown4.Value = (decimal)_scene.meshAnimation.pose.transparency;
                numericUpDown11.Value = (decimal)_scene.meshAnimation.pose.material._ubo.ni;
                numericUpDown12.Value = (decimal)_scene.meshAnimation.pose.material._ubo.absorption;
                numericUpDown13.Value = (decimal)_scene.meshAnimation.pose.material._ubo.gaussian_m;
                numericUpDown14.Value = (decimal)_scene.meshAnimation.pose.material._ubo.gaussian_c;
                if (_scene.meshAnimation.sma != null)
                {
                    numericUpDownBoneSelection.Value = (decimal)_scene.meshAnimation.sma.selectedBone;
                    numericUpDownBoneSelection.Maximum = _scene.meshAnimation.sma.numBones - 1;
                }
                numericUpDown16.Value = (decimal)_scene.meshAnimation.pose.stripSize;
                numericUpDown18.Value = (decimal)_scene.meshAnimation.pose.translation_factor;
                numericUpDown27.Value = (decimal)_scene.meshAnimation.pose.edgeFalloff;
                numericUpDown28.Value = (decimal)_scene.meshAnimation.pose.diffuseWarm;
                numericUpDown29.Value = (decimal)_scene.meshAnimation.pose.diffuseCool;

                if (_scene.meshAnimation.clusteringPerPose)
                {
                    int Count = _scene.meshAnimation.clusteringPerPoseMerging ? _scene.meshAnimation.pose.clusteringMethod.clustersMerged.Count : _scene.meshAnimation.pose.clusteringMethod.clusters.Count;

                    textBoxClustersCount.Text = Count.ToString();
                    numericUpDownClusterSelection.Maximum = Count - 1;

                    if (_scene.meshAnimation.pose.clusteringMode >= Modes.Clustering.K_MEANS)
                    {
                        textBoxClusteringIterationsCount.Text = _scene.meshAnimation.pose.clusteringMethod.iterations.ToString();
                        textBoxClusteringErrorTolerance.Text = _scene.meshAnimation.pose.clusteringMethod.errorTolerance.ToString();
                        textBoxClusteringErrorTotal.Text = _scene.meshAnimation.pose.clusteringMethod.errorTotal.ToString();
                    }
                }
                else
                {
                    int Count = _scene.meshAnimation.clusteringPerPoseMerging ? _scene.meshAnimation.clusteringMethod.clustersMerged.Count : _scene.meshAnimation.clusteringMethod.clusters.Count;

                    textBoxClustersCount.Text = Count.ToString();
                    numericUpDownClusterSelection.Maximum = Count - 1;

                    if (_scene.meshAnimation.pose.clusteringMode >= Modes.Clustering.K_MEANS)
                    {
                        textBoxClusteringIterationsCount.Text = _scene.meshAnimation.clusteringMethod.iterations.ToString();
                        textBoxClusteringErrorTolerance.Text = _scene.meshAnimation.clusteringMethod.errorTolerance.ToString();
                        textBoxClusteringErrorTotal.Text = _scene.meshAnimation.clusteringMethod.errorTotal.ToString();
                    }
                }
                _scene.meshAnimation.setVerticesColor();

                if (_scene.meshAnimation.sma != null)
                    _scene.meshAnimation.sma.selectedPose = -1;
            }
            else if ((int)e.Node.Parent.Tag > -1 && (int)e.Node.Tag > -1)
            {
                _scene.meshAnimation.selectedPose     = (int)e.Node.Parent.Tag;
                _scene.meshAnimation.sma.selectedPose = (int)e.Node.Tag;

                // RP-P2P Pose Scheme
               // if (_scene.meshAnimation.sma.errorDataVertex.fittingMode == Modes.Fitting.RP)
                 //   _scene.meshAnimation.selectedPose = _scene.meshAnimation.selectedRestPose;
                //else 
                  //  _scene.meshAnimation.selectedPose = (_scene.meshAnimation.sma.selectedPose == 0) ? 0 : _scene.meshAnimation.sma.selectedPose - 1;

                // Normal Vectors Recompute
                //if (((SMA_ErrorDataNormal)_scene.meshAnimation.sma.errorDataNormal).approximatingMode == Modes.NormalApproximation.RECOMPUTE)
                    _scene.meshAnimation.selectedPose = _scene.meshAnimation.sma.selectedPose;

                if(_scene.meshAnimation.vColoringMode == Modes.VertexColoring.SKINNING_ERROR)
                    _scene.meshAnimation.setVerticesColor();

                foreach (TreeNode node in e.Node.Parent.Nodes)
                    node.BackColor = System.Drawing.Color.White;
                e.Node.BackColor = System.Drawing.Color.Orange;
            }
            else if ((int)e.Node.Parent.Tag == -2 && (int)e.Node.Tag > -1)
            {
                _scene.selectedTexture = (int)e.Node.Tag;
                
                if (_scene.meshAnimation.sma != null)
                    _scene.meshAnimation.sma.selectedPose = -1;

                foreach (TreeNode node in e.Node.Parent.Nodes)
                    node.BackColor = System.Drawing.Color.White;
                e.Node.BackColor = System.Drawing.Color.Blue;
            }
        }

        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            if(!_modelSelectionChanged)
                _scene.meshAnimation.pose.drawable = !_scene.meshAnimation.pose.drawable;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
            {
                PrimitiveType mode;
                if (comboBox7.SelectedIndex == 0)
                    mode = PrimitiveType.Triangles;
                else
                    mode = PrimitiveType.Points;
                _scene.meshAnimation.pose.primitiveMode = mode;
            }
        }

        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.stripSize = (float)numericUpDown16.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
                _scene.meshAnimation.pose.stripColor = colorDialog1.Color;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.stripXY = !_scene.meshAnimation.pose.stripXY;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
            {
            }
        }

        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
            {
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.aabb.drawable = !_scene.meshAnimation.pose.aabb.drawable;
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.texturingPar = (Modes.TexturingPar)comboBox9.SelectedIndex;
        }      

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.texturingApp = (Modes.TexturingApp)comboBox8.SelectedIndex;
        }

        private void buttonAddModel_Click(object sender, EventArgs e)
        {
            _scene.addModel(treeView_models, 1, _progressBar, _statusLabel, contextMenuStrip2);

            _scene.meshAnimation.sma = new SMA(_scene.meshAnimation.verticesCount, _scene.meshAnimation.poses.Count);
        }

        private void buttonAddTexture_Click(object sender, EventArgs e)
        {
            _scene.addTexture(treeView_models, contextMenuStrip1);
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.transparent = !_scene.multiFragmentRendering.transparent;
        }

        private void checkBox9_CheckedChanged_1(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.correctAlpha = !_scene.multiFragmentRendering.correctAlpha;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.translucent = !_scene.multiFragmentRendering.translucent;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.transparent = !_scene.meshAnimation.pose.transparent;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            _scene.removeModel(treeView_models);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            _scene.removeTexture(treeView_models);
        }

        private void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.translation_factor = (float)numericUpDown18.Value;
        }

        private void numericUpDown19_ValueChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.cappingPlane = (float)numericUpDown19.Value;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.poses.Count > 0 && _scene.meshAnimation.selectedPose != -1)
            {
                _scene.animationGUI.play = true;
                if (!_scene.animationGUI.pause)
                {
                    treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].BackColor = System.Drawing.Color.White;
                    _scene.meshAnimation.selectedPose = (_scene.meshAnimation.sma != null && _scene.meshAnimation.sma.selectedPose > -1) ? _scene.meshAnimation.sma.selectedPose : _scene.meshAnimation.selectedRestPose;
                    treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].BackColor = System.Drawing.Color.Red;
                }
                _scene.animationGUI.pause = false;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            _scene.animationGUI.pause = true;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.selectedPose == -1)
                return;

            _scene.animationGUI.play = false;
            treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].BackColor = System.Drawing.Color.White;
            _scene.meshAnimation.selectedPose = _scene.meshAnimation.selectedRestPose;
        }

        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.cappingAngle = (float)numericUpDown20.Value;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            RenderWindow.VSync = !RenderWindow.VSync;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int numOfPoses = (_scene.meshAnimation.sma     != null) ? _scene.meshAnimation.sma.numPoses :
                             (_scene.meshAnimation.addMeanPoseToTree) ? _scene.meshAnimation.poses.Count - 1 : _scene.meshAnimation.poses.Count;

            if (_scene.meshAnimation.sma != null && _scene.meshAnimation.sma.selectedPose > -1 && _scene.meshAnimation.sma.selectedPose + 1 < numOfPoses)
            {
                treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes[_scene.meshAnimation.sma.selectedPose].BackColor
                = System.Drawing.Color.White;
                _scene.meshAnimation.sma.selectedPose++;
                treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes[_scene.meshAnimation.sma.selectedPose].BackColor
                = System.Drawing.Color.Orange;
            }
            else if (_scene.meshAnimation.selectedPose     > -1 &&
                     _scene.meshAnimation.selectedPose + 1 < numOfPoses)
            {                             
                treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].BackColor = System.Drawing.Color.White;
                _scene.meshAnimation.selectedPose++;
                treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].BackColor = System.Drawing.Color.Red;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.selectedPose > 0)
            {
                treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].BackColor = System.Drawing.Color.White;
                _scene.meshAnimation.selectedPose--;
                treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].BackColor = System.Drawing.Color.Red;
            }
        }

        private void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.restPose = (int)numericUpDown21.Value;
        }

        private void meshLabButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Mesh File(s) to Edit with MeshLab";
            ofd.Filter = "Wavefront .obj|*.obj|OFF .off|*.off|PLY .ply|*.ply";
            ofd.InitialDirectory = @Properties.Settings.Default.MeshLabLoadPath;
            ofd.Multiselect = true;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();
            Application.DoEvents();
            Application.UseWaitCursor = true;

            string filter = "";
            if (checkBox18.Checked)
                filter = @"\RemoveDuplicatedVertex.mlx";
            else if (checkBox19.Checked)
                filter = @"\computeAngleWeightedNormals_Normalize.mlx";

            progressBar2.Value = 0;
            progressBar2.Maximum = ofd.FileNames.Length;
            for (int i = 0; i < ofd.FileNames.Length; ++i)
            {
                string LoadFile = ofd.FileNames[i];
                string FileType = "";
                switch (ofd.FilterIndex)
                {
                    case 1:
                        FileType = ".obj";
                        break;
                    case 2:
                        FileType = ".off";
                        break;
                    case 3:
                        FileType = ".ply";
                        break;
                }

                string SaveFile = ofd.FileNames[i].Replace(FileType, ".obj");

                string ExecLine  = "\""   + @Properties.Settings.Default.MeshLabServerPath + "\"";
                string Arguments = " -i " + "\"" + LoadFile + "\"" + 
                                   " -o " + "\"" + SaveFile + "\"" +
                                   " -s " + "\"" + @Properties.Settings.Default.MeshLabExtensionsPath + filter + "\"";
                if (checkBox16.Checked)
                    Arguments += " -om vn";

                Example.ExecuteCommand(ExecLine, Arguments, 0);
                progressBar2.Increment(1);
            }
            Application.UseWaitCursor = false;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.rotation90 = !_scene.meshAnimation.rotation90;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.recomputeNormals = !_scene.meshAnimation.recomputeNormals;
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            _scene.light.transform = !_scene.light.transform;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            _scene.light._ubo.position = new Vector4(_scene.meshAnimation.center, _scene.light._ubo.position.W);
            _scene.light.changed = true;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            setToModelToolStripMenuItem.Enabled = (_scene.selectedTexture >= 0 && _scene.meshAnimation.selectedPose >= 0) ? true : false;
            setToBackgroundToolStripMenuItem.Enabled = (_scene.selectedTexture >= 0) ? true : false;
        }

        private void setToBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.backgroundTexture = _scene.textures[_scene.selectedTexture];
        }

        private void setToModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.texture = _scene.textures[_scene.selectedTexture];
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox10.SelectedIndex)
            {
                case 0:
                    _scene.meshAnimation.polygonMode = PolygonMode.Point;
                    break;
                case 1:
                    _scene.meshAnimation.polygonMode = PolygonMode.Line;
                    break;
                case 2:
                    _scene.meshAnimation.polygonMode = PolygonMode.Fill;
                    break;
            }
            //GL.PolygonMode(_scene.materialFace, _scene.polygonMode);
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.wireframe = !_scene.meshAnimation.pose.wireframe;
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            CSGtoolStripMenuItem.Enabled = (_scene.renderingMode == Modes.Rendering.TRIMMING && _scene.trimmingMode == Modes.Trimming.TRIMMING_DYNAMIC) ? true : false;
            setModelToolStripMenuItem.Enabled = _scene.multiFragmentRendering.csgModeling;
            setOperationToolStripMenuItem.Enabled = (_scene.multiFragmentRendering.csgModel_1 == -1 || _scene.multiFragmentRendering.csgModel_2 == -1) ? false : true;
            enableToolStripMenuItem.Enabled = !_scene.multiFragmentRendering.csgModeling;
            disableToolStripMenuItem.Enabled = _scene.multiFragmentRendering.csgModeling;
        }

        private void csgModel1toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.selectedPose != _scene.multiFragmentRendering.csgModel_2)
            {
                _scene.multiFragmentRendering.csgModel_1 = _scene.meshAnimation.selectedPose;
                _statusLabel.Text = "CSG Model 1 is " + _scene.meshAnimation.poses[_scene.multiFragmentRendering.csgModel_1].name;
            }
        }

        private void csgModel2toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.selectedPose != _scene.multiFragmentRendering.csgModel_1)
            {
                _scene.multiFragmentRendering.csgModel_2 = _scene.meshAnimation.selectedPose;
                _statusLabel.Text = "CSG Model 2 is " + _scene.meshAnimation.poses[_scene.multiFragmentRendering.csgModel_2].name;
            }
        }

        private void unionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.csgOperation = Modes.CSG_Operation.UNION;
            _statusLabel.Text = "CSG Operation is UNION";
        }

        private void intersectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.csgOperation = Modes.CSG_Operation.INTERSECTION;
            _statusLabel.Text = "CSG Operation is INTERSECTION";
        }

        private void differenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.csgOperation = Modes.CSG_Operation.DIFFERENCE;
            _statusLabel.Text = "CSG Operation is DIFFERENCE";
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.csgOperation = Modes.CSG_Operation.NONE;
            _statusLabel.Text = "CSG Operation is NONE";
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.csgModeling = true;
            _statusLabel.Text = "CSG Modeling Enabled !!";
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.csgModeling = false;
            _statusLabel.Text = "CSG Modeling Disabled !!";
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.csgModel_1 = _scene.multiFragmentRendering.csgModel_2 = -1;
            _scene.multiFragmentRendering.csgOperation = Modes.CSG_Operation.NONE;
            _statusLabel.Text = "CSG Properties cleared !!";

        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.redrawFather = !_scene.multiFragmentRendering.redrawFather;
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.zFighting = !_scene.multiFragmentRendering.zFighting;
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            _scene.camera.inverseZ = !_scene.camera.inverseZ;
            //_scene.camera.load_projection_matrix(_scene.min, _scene.max);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Matrix4 rotX = Matrix4.CreateRotationX(_scene.camera.angle.X);
            Matrix4 rotY = Matrix4.CreateRotationY(_scene.camera.angle.Y);
            Matrix4 rotZ = Matrix4.CreateRotationZ(_scene.camera.angle.Z);

            _scene.camera.eye -= _scene.meshAnimation.center;
            _scene.camera.eye = Vector3.TransformPosition(_scene.camera.eye, rotX);
            _scene.camera.eye = Vector3.TransformPosition(_scene.camera.eye, rotY);
            _scene.camera.eye = Vector3.TransformPosition(_scene.camera.eye, rotZ);
            _scene.camera.eye += _scene.meshAnimation.center;
        }

        private void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            _scene.camera.angle = new Vector3((float)numericUpDown22.Value, _scene.camera.angle.Y, _scene.camera.angle.Z);
        }

        private void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            _scene.camera.angle = new Vector3(_scene.camera.angle.X, (float)numericUpDown23.Value, _scene.camera.angle.Z);
        }

        private void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            _scene.camera.angle = new Vector3(_scene.camera.angle.X, _scene.camera.angle.Y, (float)numericUpDown24.Value);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            _scene.camera.animation = !_scene.camera.animation;
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_scene.renderingMode != Modes.Rendering.RENDER)
            {
                _scene.trimmingMode = (Modes.Trimming)comboBox11.SelectedIndex;
                _scene.selectedRendering = (int)_scene.trimmingMode;

                if (_scene.renderingMode == Modes.Rendering.PEELING)
                    textBoxMemorySize.Text = _scene.peelingMethods[_scene.selectedRendering].memory.ToString("F") + " MB";
                else if (_scene.renderingMode == Modes.Rendering.TRIMMING)
                    textBoxMemorySize.Text = _scene.trimmingMethods[_scene.selectedRendering].memory.ToString("F") + " MB";
            }
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            FPS.revalue_use();
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.useMax = !_scene.multiFragmentRendering.useMax;
        }

        private void comboBox13_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (comboBox13.SelectedIndex)
            {
                case 0:
                    _scene.meshAnimation.materialFace = MaterialFace.Back;
                    break;
                case 1:
                    _scene.meshAnimation.materialFace = MaterialFace.Front;
                    break;
                case 2:
                    _scene.meshAnimation.materialFace = MaterialFace.FrontAndBack;
                    break;
            }
            //GL.PolygonMode(_scene.materialFace, _scene.polygonMode);
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.useOcclusionCulling = !_scene.multiFragmentRendering.useOcclusionCulling;
        }

        private void numericUpDown25_ValueChanged(object sender, EventArgs e)
        {
            _scene.lineSize = (float)numericUpDown25.Value;
            GL.LineWidth(_scene.lineSize);
        }

        private void numericUpDown26_ValueChanged(object sender, EventArgs e)
        {
            _scene.pointSize = (float)numericUpDown26.Value;
            GL.PointSize(_scene.pointSize);
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.enablePeelingError = !_scene.multiFragmentRendering.enablePeelingError;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            _scene.animationGUI.animation_stop = !_scene.animationGUI.animation_stop;
        }

        private void drawSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.closest = !_scene.multiFragmentRendering.closest;
        }

        private void numericUpDown27_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.edgeFalloff = (float)numericUpDown27.Value;
        }

        private void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.diffuseWarm = (float)numericUpDown28.Value;
        }
        
        private void numericUpDown29_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.diffuseCool = (float)numericUpDown29.Value;
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.ignoreGroups = !_scene.meshAnimation.ignoreGroups;
        }

        private void loadSMA_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TXT .txt|*.txt|Skin .skin|*.skin|SMA .sma|*.sma";
            ofd.Title = "Select Skinning Mesh Animation File";
            ofd.InitialDirectory = @Properties.Settings.Default.SMALoadPath;
            ofd.Multiselect = false;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();
            Application.DoEvents();

            if (ofd.FileName.Trim() == "")
                return;
            
            Application.UseWaitCursor = true;  
            switch (ofd.FilterIndex)
            {
                case 1:
                case 2:
                case 3:
                    {
                        _scene.meshAnimation.sma = new SMA(_scene.meshAnimation.pose.verticesCount, 0);
                        Mesh3DAnimationSequence Mas = _scene.meshAnimation;
                        if (_scene.meshAnimation.sma.loadSMA(ofd.FileName.Trim(), ref Mas))
                        {
                            _statusLabel.Text = ofd.FileName.Trim() + " loaded!!";
                            for (int i = 0; i < _scene.meshAnimation.sma.numPoses; i++)
                            {
                                treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].Nodes.Add("Pose_" + i.ToString());
                                treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].Nodes[i].Tag = i;
                            }
                            treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].Expand();
                        }
                        break;
                    }
                default:
                    break;
            }
            Application.UseWaitCursor = false;
        }

        private void numericUpDownBoneSelection_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged && _scene.meshAnimation.sma != null)
            {
                _scene.meshAnimation.sma.selectedBone = (int)numericUpDownBoneSelection.Value;
                _scene.meshAnimation.setVerticesColor();
            }
        }

        private void comboBoxVertexColoringMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.vColoringMode = (Modes.VertexColoring)comboBoxVertexColoringMode.SelectedIndex;
            if (_scene.meshAnimation.poses.Count > 0)
                _scene.meshAnimation.setVerticesColor();
            
            if (_scene.meshAnimation.sma != null && _scene.meshAnimation.sma.poses != null && _scene.meshAnimation.selectedPose == _scene.meshAnimation.selectedRestPose)
                    _scene.meshAnimation.sma.poses[_scene.meshAnimation.selectedRestPose].setVerticesColor();
        }

        private void buttonCalculateDefGradients_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                _scene.meshAnimation.calculateDeformationGradients(_progressBar, _statusLabel);
                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.DEFORMATION_GRADIENTS;
            }
            Application.UseWaitCursor = false;
        }

        private void comboBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.dgMode = (Modes.DeformationGradient)comboBoxDGmode.SelectedIndex;
            _scene.meshAnimation.setVerticesColor();
        }

        private void setColoringModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                Pose.coloringMode = _scene.meshAnimation.pose.coloringMode;
        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.dgComponentsMode = (Modes.DeformationGradientComponents)comboBoxDGComponentsMode.SelectedIndex;
            _scene.meshAnimation.setVerticesColor();
        }

        private void drawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.meshAnimation.selectedPoseDrawable = !_scene.meshAnimation.selectedPoseDrawable;

            if (_scene.meshAnimation.selectedPoseDrawable == true && _scene.meshAnimation.selectedPose != -1)
                _scene.meshAnimation.pose.aabb.drawable = false;
        }

        private void propagateColoringModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
            {
                Pose.coloringMode = _scene.meshAnimation.pose.coloringMode;
                Pose.transparent = _scene.meshAnimation.pose.transparent;
            }
        }

        private void setAsRestPoseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Text =
            treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Text.Remove(treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Text.Length - 1);
            _scene.meshAnimation.selectedRestPose = _scene.meshAnimation.selectedPose;
            treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedPose].Text += '*';
        }

        private void checkBoxDGperPose_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.dgPerPose = !_scene.meshAnimation.dgPerPose;
            _scene.meshAnimation.setVerticesColor();
        }

        private void checkBoxDrawSpheres_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                Pose.clusteringMethod.drawSpheres = checkBoxDrawSpheres.Checked;
            _scene.meshAnimation.clusteringMethod.drawSpheres = checkBoxDrawSpheres.Checked;
        }

        private void buttonComputeClustering_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.NONE;

                _scene.meshAnimation.computeClustering(_progressBar, _statusLabel, RenderWindow);

                if (_scene.meshAnimation.clusteringPerPose)
                {
                    int Count = _scene.meshAnimation.clusteringPerPoseMerging ? _scene.meshAnimation.pose.clusteringMethod.clustersMerged.Count : _scene.meshAnimation.pose.clusteringMethod.clusters.Count;

                    textBoxClustersCount.Text = Count.ToString();
                    numericUpDownClusterSelection.Maximum = Count - 1;

                    if (_scene.meshAnimation.pose.clusteringMode >= Modes.Clustering.K_MEANS)
                    {
                        textBoxClusteringIterationsCount.Text = _scene.meshAnimation.pose.clusteringMethod.iterations.ToString();
                        textBoxClusteringErrorTolerance.Text = _scene.meshAnimation.pose.clusteringMethod.errorTolerance.ToString();
                        textBoxClusteringErrorTotal.Text = _scene.meshAnimation.pose.clusteringMethod.errorTotal.ToString();
                    }
                }
                else
                {
                    int Count = _scene.meshAnimation.clusteringPerPoseMerging ? _scene.meshAnimation.clusteringMethod.clustersMerged.Count : _scene.meshAnimation.clusteringMethod.clusters.Count;
                    textBoxClustersCount.Text = Count.ToString();
                    numericUpDownClusterSelection.Maximum = Count - 1;

                    if (_scene.meshAnimation.clusteringMode >= Modes.Clustering.K_MEANS)
                    {
                        textBoxClusteringIterationsCount.Text = _scene.meshAnimation.clusteringMethod.iterations.ToString();
                        textBoxClusteringErrorTolerance.Text = _scene.meshAnimation.clusteringMethod.errorTolerance.ToString();
                        textBoxClusteringErrorTotal.Text = _scene.meshAnimation.clusteringMethod.errorTotal.ToString();
                    }
                }
                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.CLUSTER;
            }
            Application.UseWaitCursor = false;
        }

        private void checkBoxClusteringPerPose_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.clusteringPerPose = !_scene.meshAnimation.clusteringPerPose;
            
            comboBoxClusteringMode_SelectedIndexChanged(sender, e);
        }

        private void numericUpDownClusterCount_ValueChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
            {
                if (!Pose.clusteringMethod.randomSeeding)
                    Pose.pCenter.count = (int)numericUpDownClustersCount.Value;
                Pose.clusteringMethod.count = (int)numericUpDownClustersCount.Value;
            }
            if (!_scene.meshAnimation.clusteringMethod.randomSeeding)
                _scene.meshAnimation.pCenter.count = (int)numericUpDownClustersCount.Value;
            _scene.meshAnimation.clusteringMethod.count = (int)numericUpDownClustersCount.Value;
        }

        private void numericUpDownClusterSelection_ValueChanged(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.clusteringPerPose)
                _scene.meshAnimation.pose.clusteringMethod.selectedCluster = (int)numericUpDownClusterSelection.Value;
            else
                _scene.meshAnimation.clusteringMethod.selectedCluster = (int)numericUpDownClusterSelection.Value;

            _scene.meshAnimation.setVerticesColor();
        }

        private void comboBoxClusteringMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.clusteringPerPose)
            {
                foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                {
                    Pose.clusteringMode = (Modes.Clustering)comboBoxClusteringMode.SelectedIndex;
                    Pose.clusteringMethod.count = (int)numericUpDownClustersCount.Value;
                    Pose.clusteringMethod.scalingFactor = checkBoxSkinningScale.Checked;
                    Pose.clusteringMethod.randomSeeding = checkBoxRandomSeeding.Checked;
                    Pose.clusteringMethod.drawSpheres   = checkBoxDrawSpheres.Checked;
                    Pose.clusteringMethod.drawRegions   = checkBoxDrawRegions.Checked;
                    Pose.clusteringMethod.drawNeighbors = checkBoxDrawNeighbors.Checked;

                    Pose.clusteringMethod.maxIterations       = (int)   numericUpDownClusteringIterations.Value;
                    Pose.clusteringMethod.errorTolerance      = (double)numericUpDownClusteringTolerance.Value;
                    Pose.clusteringMethod.errorTotalTolerance = (double)numericUpDownClusteringError.Value;

                    if (!Pose.clusteringMethod.randomSeeding)
                        Pose.pCenter.count = (int)numericUpDownClustersCount.Value;
                    if (Pose.clusteringMode == Modes.Clustering.MERGE_RG)
                    {
                        Pose.mergeRG.errorTotalTolerance = (double)numericUpDownClusteringError.Maximum;
                        numericUpDownClusteringError.Value = numericUpDownClusteringError.Maximum;
                    }
                    else
                        numericUpDownClusteringError.Value = numericUpDownClusteringError.Minimum;

                    if (Pose.clusteringMode == Modes.Clustering.SPECTRAL)
                        Pose.kSpectral.percentageInit = (int)numericUpDownPercentageSpectral.Value;
                    if (Pose.clusteringMode == Modes.Clustering.C_PCA)
                    {
                        numericUpDownBasisVectorsCount.Maximum = 1;
                        Pose.cPCA.basisVectorsCount = (int)numericUpDownBasisVectorsCount.Value;
                    }
                }
            }
            else
            {
                _scene.meshAnimation.clusteringMode                         = (Modes.Clustering)comboBoxClusteringMode.SelectedIndex;
                _scene.meshAnimation.clusteringMethod.count                 = (int)numericUpDownClustersCount.Value;
                _scene.meshAnimation.clusteringMethod.scalingFactor         = checkBoxSkinningScale.Checked;
                _scene.meshAnimation.clusteringMethod.randomSeeding         = checkBoxRandomSeeding.Checked;
                _scene.meshAnimation.clusteringMethod.drawSpheres           = checkBoxDrawSpheres.Checked;
                _scene.meshAnimation.clusteringMethod.drawRegions           = checkBoxDrawRegions.Checked;
                _scene.meshAnimation.clusteringMethod.drawNeighbors         = checkBoxDrawNeighbors.Checked;

                _scene.meshAnimation.clusteringMethod.maxIterations         = (int)numericUpDownClusteringIterations.Value;
                _scene.meshAnimation.clusteringMethod.errorTolerance        = (double)numericUpDownClusteringTolerance.Value;
                _scene.meshAnimation.clusteringMethod.errorTotalTolerance   = (double)numericUpDownClusteringError.Value;

                if (!_scene.meshAnimation.clusteringMethod.randomSeeding)
                    _scene.meshAnimation.pCenter.count = (int)numericUpDownClustersCount.Value;

                if (_scene.meshAnimation.clusteringMode == Modes.Clustering.MERGE_RG)
                {
                    _scene.meshAnimation.mergeRG.errorTotalTolerance = (double)numericUpDownClusteringError.Maximum;
                    numericUpDownClusteringError.Value = numericUpDownClusteringError.Maximum;
                }
                if (_scene.meshAnimation.clusteringMode == Modes.Clustering.SPECTRAL)
                    _scene.meshAnimation.kSpectral.percentageInit = (int)numericUpDownPercentageSpectral.Value;
                if (_scene.meshAnimation.clusteringMode == Modes.Clustering.C_PCA)
                {
                    numericUpDownBasisVectorsCount.Maximum = _scene.meshAnimation.poses.Count;
                    _scene.meshAnimation.cPCA.basisVectorsCount = (int)numericUpDownBasisVectorsCount.Value;
                }
            }
        }

        private void buttonComputeWeights_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.NONE;

                _scene.meshAnimation.sma.computeInitWeigths(_scene.meshAnimation);
                numericUpDownBoneSelection.Maximum = _scene.meshAnimation.sma.numBones - 1;

                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.BONE;
                _statusLabel.Text = "Weights Calculation Completed!!";
            }
            Application.UseWaitCursor = false;
        }

        private void numericUpDownPFactor_ValueChanged(object sender, EventArgs e)
        {
            if(!_scene.meshAnimation.clusteringPerPose)
                _scene.meshAnimation.clusteringMethod.pFactor = (float)numericUpDownPFactor.Value;
            else
                _scene.meshAnimation.pose.clusteringMethod.pFactor = (float)numericUpDownPFactor.Value;
        }

        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.sma != null)
                _scene.meshAnimation.sma.fittingMode = (Modes.Fitting)comboBox17.SelectedIndex;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                _scene.meshAnimation.sma.computeFitting(_scene.meshAnimation, _progressBar);
                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.NONE;
                _statusLabel.Text = "Fitting Completed!!";
            }
            Application.UseWaitCursor = false;
        }

        private void numericUpDown34_ValueChanged(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.sma != null) { 
                _scene.meshAnimation.sma.fittingIterations = (int)numericUpDownFittingIterations.Value;
                textBoxFittingIter.Text = _scene.meshAnimation.sma.fittingIterations.ToString();
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            _scene.meshAnimation.sma.createForm();
            ButtonSmaError.Enabled = false;
        }

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.addMeanPoseToTree = !_scene.meshAnimation.addMeanPoseToTree;
        }

        private void buttonComputeFinalPos_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                _scene.meshAnimation.sma.computeFinalPositions();
                _scene.meshAnimation.sma.computeEigenSkin();
                //_scene.meshAnimation.sma.computeEigenWeights();
                _scene.meshAnimation.sma.computeApproxModels(_scene.meshAnimation);
                _scene.meshAnimation.sma.createChartError();

                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.SKINNING_ERROR;

                _statusLabel.Text = "Skinning Error Completed!!";
            }
            Application.UseWaitCursor = false;

            // GUI
            {
                for (int i = treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes.Count - 1; i >= 0; --i)
                    treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes[i].Remove();

                for (int i = 0; i < _scene.meshAnimation.sma.numPoses; ++i)
                {
                    treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes.Add("Pose_" + i.ToString());
                    treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes[i].Tag = i;
                }

                if (_scene.meshAnimation.addMeanPoseToTree)
                    treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes[_scene.meshAnimation.sma.numPoses - 1].Text += "#";

                treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Expand();
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.meshAnimation.addEditedPose(treeView_models, _progressBar, _statusLabel);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scene.meshAnimation.resetEditedPose(treeView_models);
        }

        private void comboBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(_scene.meshAnimation.sma != null)
                _scene.meshAnimation.sma.approximatingNormalsMode = (Modes.NormalApproximation) comboBox18.SelectedIndex;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            FPS.showLocalForm();
            button27.Enabled = false;
        }

        private void numericUpDownClusteringIterations_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
            {
                foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                    Pose.clusteringMethod.maxIterations = (int)numericUpDownClusteringIterations.Value;
                _scene.meshAnimation.clusteringMethod.maxIterations = (int)numericUpDownClusteringIterations.Value;
            }
        }

        private void checkBoxRandomSeeding_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
            {
                Pose.pCenter.count = (int)numericUpDownClustersCount.Value;
                Pose.clusteringMethod.randomSeeding = checkBoxRandomSeeding.Checked;
            }

            _scene.meshAnimation.pCenter.count                  = (int)numericUpDownClustersCount.Value;
            _scene.meshAnimation.clusteringMethod.randomSeeding = checkBoxRandomSeeding.Checked;
        }

        private void numericUpDownClusteringTolerance_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
            {
                foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                    Pose.clusteringMethod.errorTolerance = (double)numericUpDownClusteringTolerance.Value;
                _scene.meshAnimation.clusteringMethod.errorTolerance = (double)numericUpDownClusteringTolerance.Value;
            }
        }

        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.coplanar = !_scene.multiFragmentRendering.coplanar;
        }

        private void checkBoxDrawRegions_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                Pose.clusteringMethod.drawRegions = checkBoxDrawRegions.Checked;
            _scene.meshAnimation.clusteringMethod.drawRegions = checkBoxDrawRegions.Checked;

            _scene.meshAnimation.setVerticesColor();
        }

        private void comboBox_SelectedIndexChanged_ClusteringDistanceMode(object sender, EventArgs e)
        {
            _scene.meshAnimation.clusteringDistanceMode = (Modes.ClusteringDistance) comboBoxClusteringDistanceMode.SelectedIndex;
        }

        private void checkBoxSkinningScale_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                Pose.clusteringMethod.scalingFactor = checkBoxSkinningScale.Checked;
            _scene.meshAnimation.clusteringMethod.scalingFactor = checkBoxSkinningScale.Checked;
        }

        private void checkBoxDrawNeighbors_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                Pose.clusteringMethod.drawNeighbors = checkBoxDrawNeighbors.Checked;
            _scene.meshAnimation.clusteringMethod.drawNeighbors = checkBoxDrawNeighbors.Checked;

            _scene.meshAnimation.setVerticesColor();
        }

        private void comboBox12_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            _scene.meshAnimation.clusteringSpectralDistanceMode = (Modes.ClusteringSpectralDistance)comboBox12.SelectedIndex;
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                Pose.kSpectral.percentageInit = (int)numericUpDownPercentageSpectral.Value;
            _scene.meshAnimation.kSpectral.percentageInit = (int)numericUpDownPercentageSpectral.Value;
        }

        private void checkBoxDgNormalize_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.dgNormalize = !_scene.meshAnimation.dgNormalize;
        }

        private void comboBoxClusteringSpectralGraphMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.clusteringSpectralGraphMode = (Modes.ClusteringSpectralGraph)comboBoxClusteringSpectralGraphMode.SelectedIndex;
        }

        private void numericUpDownClusteringError_ValueChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
            {
                foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                    Pose.clusteringMethod.errorTotalTolerance = (double)numericUpDownClusteringError.Value;
                _scene.meshAnimation.clusteringMethod.errorTotalTolerance = (double)numericUpDownClusteringError.Value;
            }
        }

        private void comboBox_SelectedIndexChanged_DistanceMode(object sender, EventArgs e)
        {
            _scene.meshAnimation.clusteringVertexDistanceMode = (Modes.ClusteringVertexDistance)comboBoxDistanceMode.SelectedIndex;
        }

        private void checkBoxNNG_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                Pose.kSpectral.nearestNeighborGraph = checkBoxNNG.Checked;
            _scene.meshAnimation.kSpectral.nearestNeighborGraph = checkBoxNNG.Checked;
        }

        private void checkBoxNipals_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                Pose.clusteringMethod.nipals = checkBoxNipals.Checked;
            _scene.meshAnimation.clusteringMethod.nipals = checkBoxNipals.Checked;
        }

        private void numericUpDownSpectralEigenGap_ValueChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                Pose.kSpectral.eigenGap = (double)numericUpDownSpectralEigenGap.Value;
            _scene.meshAnimation.kSpectral.eigenGap = (double)numericUpDownSpectralEigenGap.Value;
        }

        private void numericUpDownBasisVectorsCount_ValueChanged(object sender, EventArgs e)
        {
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                Pose.cPCA.basisVectorsCount = (int)numericUpDownBasisVectorsCount.Value;
            _scene.meshAnimation.cPCA.basisVectorsCount = (int)numericUpDownBasisVectorsCount.Value;
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.dgVariability = !_scene.meshAnimation.dgVariability;
        }

        private void numericUpDown30_ValueChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.dgVariabilityEigenCount = (int)numericUpDown30.Value;
        }

        private void buttonCalculateGeodesicDistances_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                {
                    _progressBar.Value = 0;
                    _progressBar.Maximum = _scene.meshAnimation.poses.Count;
                    foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                    {
                        Pose.calculateVectorsGeodesicDistances();
                        _progressBar.Increment(1);
                    }
                    _statusLabel.Text = "Geodesic Distances Calculation Completed!!";
                }
                timer.Stop();
                Console.WriteLine("Geodesic Distances Time Elapsed: {0}", timer.Elapsed);

                numericUpDownSelectedVertex.Maximum = _scene.meshAnimation.verticesCount;

                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.GEODESIC_DISTANCES;
            }
            Application.UseWaitCursor = false;
        }

        private void numericUpDownSelectedVertex_ValueChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.selectedVertex = (int)numericUpDownSelectedVertex.Value;
            _scene.meshAnimation.setVerticesColor();
        }

        private void numericUpDownMaxFPS_ValueChanged(object sender, EventArgs e)
        {
#if FPS
            FPS.maxValue = (int)numericUpDownMaxFPS.Value;
#endif
        }

        private void checkBoxClusteringPerPoseMerging_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.clusteringPerPoseMerging = !_scene.meshAnimation.clusteringPerPoseMerging;
            if (_scene.meshAnimation.clusteringPerPose)
            {
                foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                {
                    Pose.clusteringMethod.computeNeighborhood(Pose, _scene.meshAnimation.clusteringPerPoseMerging);
                }
                int Count = _scene.meshAnimation.clusteringPerPoseMerging ? _scene.meshAnimation.pose.clusteringMethod.clustersMerged.Count : _scene.meshAnimation.pose.clusteringMethod.clusters.Count;
                textBoxClustersCount.Text = Count.ToString();
                numericUpDownClusterSelection.Maximum = Count - 1;
            }

            _scene.meshAnimation.setVerticesColor();
        }

        private void checkBoxClusteringSetFixedColor_CheckedChanged(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.clusteringPerPose)
                foreach (Mesh3D Pose in _scene.meshAnimation.poses)
                    Pose.clusteringMethod.setColor(checkBoxClusteringSetFixedColor.Checked, _scene.meshAnimation.clusteringPerPoseMerging, _scene.meshAnimation.clustering2RingColoring);
            else
                _scene.meshAnimation.clusteringMethod.setColor(checkBoxClusteringSetFixedColor.Checked, _scene.meshAnimation.clusteringPerPoseMerging, _scene.meshAnimation.clustering2RingColoring);
            _scene.meshAnimation.setVerticesColor();
        }

        private void checkBoxMerged2MergedClustering_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.clusteringIncremental = !_scene.meshAnimation.clusteringIncremental;
        }

        private void numericUpDownMergingPrevTolerance_ValueChanged(object sender, EventArgs e)
        {
#if CPU_PARALLEL
            Parallel.ForEach(_scene.meshAnimation.poses, Pose =>
#else
            foreach (Mesh3D Pose in _scene.meshAnimation.poses)
#endif
            {
                Pose.clusteringMethod.h_cleaning = (double)numericUpDownMergingPrevTolerance.Value;
            }
#if CPU_PARALLEL
);
#endif
            _scene.meshAnimation.clusteringMethod.h_cleaning = (double)numericUpDownMergingPrevTolerance.Value;

            if (_scene.meshAnimation.clusteringClean)
            {
                //if(_scene.meshAnimation.clusteringDistanceMode == Modes.ClusteringDistance.MERGING)
                  //  buttonComputeClustering_Click(sender, e);
                //else
                //if (_scene.meshAnimation.clusteringDistanceMode == Modes.ClusteringDistance.OVER_SEGMENTATION)
                {
                    //if (_scene.meshAnimation.clusteringIncremental)
                      //  buttonComputeClustering_Click(sender, e);
                    buttonPerformCleaning_Click(sender, e);
                }
            }
        }

        private void buttonVariableSegmentation_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                if (_scene.meshAnimation.clusteringPerPose)
                {
                    _scene.meshAnimation.computeVariableSegmentation(_progressBar, _statusLabel, RenderWindow);

                    int Count = _scene.meshAnimation.clusteringPerPoseMerging ? _scene.meshAnimation.pose.clusteringMethod.clustersMerged.Count : _scene.meshAnimation.pose.clusteringMethod.clusters.Count;
                    textBoxClustersCount.Text = Count.ToString();
                    numericUpDownClusterSelection.Maximum = Count - 1;
                    
                    _scene.meshAnimation.setVerticesColor();
                }
            }
            Application.UseWaitCursor = false;
        }

        private void buttonPropagateClusteringColors_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                if (_scene.meshAnimation.clusteringPerPose)
                {
                    _scene.meshAnimation.propagateClusteringColors(_progressBar, _statusLabel);
                    _scene.meshAnimation.setVerticesColor();
                }
            }
            Application.UseWaitCursor = false;
        }

        private void buttonPerformCleaning_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                if (_scene.meshAnimation.clusteringDistanceMode == Modes.ClusteringDistance.OVER_SEGMENTATION)
                {
                    if (_scene.meshAnimation.clusteringIncremental)
                        buttonComputeClustering_Click(sender, e);

                    _scene.meshAnimation.performCleaning(_progressBar, _statusLabel);

                    int Count = _scene.meshAnimation.clusteringMethod.clusters.Count;
                    textBoxClustersCount.Text = Count.ToString();
                    numericUpDownClusterSelection.Maximum = Count - 1;

                    _scene.meshAnimation.setVerticesColor();
                }
            }
            Application.UseWaitCursor = false;
        }

        private void cleaningCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.clusteringClean = !_scene.meshAnimation.clusteringClean;
        }

        private void buttonSmoothBoundaries_Click(object sender, EventArgs e)
        {
            _scene.meshAnimation.smoothBoundaries(_progressBar, _statusLabel);
            _scene.meshAnimation.setVerticesColor();
        }

        private void FileStep_ValueChanged(object sender, EventArgs e)
        {
            WaveFront_OBJ_File._step = (int)FileStep.Value;
        }
        
        private void loadPoseClustering_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CLU .clu|*.clu|TXT .txt|*.txt";
            ofd.Title = "Select Clustering File";
            ofd.Multiselect = false;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();
            Application.DoEvents();

            if (ofd.FileName.Trim() == "")
                return;

            Application.UseWaitCursor = true;
            switch (ofd.FilterIndex)
            {
                case 1:
                case 2:
                    {
                        _progressBar.Value = 0;
                        _progressBar.Maximum = 1;

                        int     Count;
                        Mesh3D  Pose;
                        // LOAD
                        if (_scene.meshAnimation.clusteringPerPose)
                        {
                            Pose = _scene.meshAnimation.pose;
                            if (ofd.FileName.Contains("P_CENTER"))
                            {
                                _scene.meshAnimation.pose.clusteringMode = Modes.Clustering.P_CENTER;
                                _scene.meshAnimation.pose.clusteringMethods[0] = new P_Center_Clustering();
                            }
                            else if (ofd.FileName.Contains("K_MEANS"))
                            {
                                _scene.meshAnimation.pose.clusteringMode = Modes.Clustering.K_MEANS;
                                _scene.meshAnimation.pose.clusteringMethods[1] = new K_Means_Clustering();
                            }
                            else if (ofd.FileName.Contains("K_RG"))
                            {
                                _scene.meshAnimation.pose.clusteringMode = Modes.Clustering.K_RG;
                                _scene.meshAnimation.pose.clusteringMethods[2] = new K_RG_Clustering();
                            }
                            else if (ofd.FileName.Contains("MERGE_RG"))
                            {
                                _scene.meshAnimation.pose.clusteringMode = Modes.Clustering.MERGE_RG;
                                _scene.meshAnimation.pose.clusteringMethods[3] = new Merge_RG_Clustering();
                            }
                            else if (ofd.FileName.Contains("DIVIDE_CONQUER"))
                            {
                                _scene.meshAnimation.pose.clusteringMode = Modes.Clustering.DIVIDE_CONQUER;
                                _scene.meshAnimation.pose.clusteringMethods[4] = new Divide_Conquer_Clustering();
                            }
                            else if (ofd.FileName.Contains("SPECTRAL"))
                            {
                                _scene.meshAnimation.pose.clusteringMode = Modes.Clustering.SPECTRAL;
                                _scene.meshAnimation.pose.clusteringMethods[5] = new K_Spectral_Clustering();
                            }
                            else if (ofd.FileName.Contains("C_PCA"))
                            {
                                _scene.meshAnimation.pose.clusteringMode = Modes.Clustering.C_PCA;
                                _scene.meshAnimation.pose.clusteringMethods[6] = new C_PCA_Clustering();
                            }
                            else // ERROR
                            {
                                break;
                            }

                            Count = _scene.meshAnimation.clusteringPerPoseMerging ? _scene.meshAnimation.pose.clusteringMethod.clustersMerged.Count : _scene.meshAnimation.pose.clusteringMethod.clusters.Count;
                            comboBoxClusteringMode.SelectedIndex = (int)_scene.meshAnimation.pose.clusteringMode;
                        }
                        else
                        {
                            Pose = _scene.meshAnimation.restPose;
                            if      (ofd.FileName.Contains("P_CENTER"))
                            {
                                _scene.meshAnimation.clusteringMode = Modes.Clustering.P_CENTER;
                                _scene.meshAnimation.clusteringMethods[0] = new P_Center_Clustering();
                            }
                            else if (ofd.FileName.Contains("K_MEANS"))
                            {
                                _scene.meshAnimation.clusteringMode = Modes.Clustering.K_MEANS;
                                _scene.meshAnimation.clusteringMethods[1] = new K_Means_Clustering();
                            }
                            else if (ofd.FileName.Contains("K_RG"))
                            {
                                _scene.meshAnimation.clusteringMode = Modes.Clustering.K_RG;
                                _scene.meshAnimation.clusteringMethods[2] = new K_RG_Clustering();
                            }
                            else if (ofd.FileName.Contains("MERGE_RG"))
                            {
                                _scene.meshAnimation.clusteringMode = Modes.Clustering.MERGE_RG;
                                _scene.meshAnimation.clusteringMethods[3] = new Merge_RG_Clustering();
                            }
                            else if (ofd.FileName.Contains("DIVIDE_CONQUER"))
                            {
                                _scene.meshAnimation.clusteringMode = Modes.Clustering.DIVIDE_CONQUER;
                                _scene.meshAnimation.clusteringMethods[4] = new Divide_Conquer_Clustering();
                            }
                            else if (ofd.FileName.Contains("SPECTRAL"))
                            {
                                _scene.meshAnimation.clusteringMode = Modes.Clustering.SPECTRAL;
                                _scene.meshAnimation.clusteringMethods[5] = new K_Spectral_Clustering();
                            }
                            else if (ofd.FileName.Contains("C_PCA"))
                            {
                                _scene.meshAnimation.clusteringMode = Modes.Clustering.C_PCA;
                                _scene.meshAnimation.clusteringMethods[6] = new C_PCA_Clustering();
                            }
                            else // ERROR
                            {
                                break;
                            }

                            Count = _scene.meshAnimation.clusteringPerPoseMerging ? _scene.meshAnimation.clusteringMethod.clustersMerged.Count : _scene.meshAnimation.clusteringMethod.clusters.Count;
                            comboBoxClusteringMode.SelectedIndex = (int)_scene.meshAnimation.clusteringMode;
                        }

                        _scene.meshAnimation.loadClustering(Pose, ofd.FileName.Trim());

                        // GUI
                        textBoxClustersCount.Text = Count.ToString();
                        numericUpDownClusterSelection.Maximum = Count - 1;

                        comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.CLUSTER;

                        _progressBar.Increment(1);
                        _statusLabel.Text = ofd.FileName.Trim() + " clustering loaded!!";

                        break;
                    }
                default:
                    break;
            }

            checkBoxClusteringSetFixedColor_CheckedChanged(sender, e);

            Application.UseWaitCursor = false;
        }

        private void loadAllPosesClustering_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CLU .clu|*.clu|TXT .txt|*.txt";
            ofd.Title = "Select Clustering Files";
            ofd.Multiselect = true;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();
            Application.DoEvents();

            Application.UseWaitCursor = true;
            
            _progressBar.Value = 0;
            _progressBar.Maximum = ofd.FileNames.Length;

            for (int FileID = 0; FileID < ofd.FileNames.Length; FileID++)
            {
                if (ofd.FileNames[FileID].Trim() == "")
                    return;

                switch (ofd.FilterIndex)
                {
                    case 1:
                    case 2:
                        {
                            // LOAD
                            {
                                if (ofd.FileNames[FileID].Contains("P_CENTER"))
                                {
                                    _scene.meshAnimation.poses[FileID].clusteringMode = Modes.Clustering.P_CENTER;
                                    _scene.meshAnimation.poses[FileID].clusteringMethods[0] = new P_Center_Clustering();
                                }
                                else if (ofd.FileNames[FileID].Contains("K_MEANS"))
                                {
                                    _scene.meshAnimation.poses[FileID].clusteringMode = Modes.Clustering.K_MEANS;
                                    _scene.meshAnimation.poses[FileID].clusteringMethods[1] = new K_Means_Clustering();
                                }
                                else if (ofd.FileNames[FileID].Contains("K_RG"))
                                {
                                    _scene.meshAnimation.poses[FileID].clusteringMode = Modes.Clustering.K_RG;
                                    _scene.meshAnimation.poses[FileID].clusteringMethods[2] = new K_RG_Clustering();
                                }
                                else if (ofd.FileNames[FileID].Contains("MERGE_RG"))
                                {
                                    _scene.meshAnimation.poses[FileID].clusteringMode = Modes.Clustering.MERGE_RG;
                                    _scene.meshAnimation.poses[FileID].clusteringMethods[3] = new Merge_RG_Clustering();
                                }
                                else if (ofd.FileNames[FileID].Contains("DIVIDE_CONQUER"))
                                {
                                    _scene.meshAnimation.poses[FileID].clusteringMode = Modes.Clustering.DIVIDE_CONQUER;
                                    _scene.meshAnimation.poses[FileID].clusteringMethods[4] = new Divide_Conquer_Clustering();
                                }
                                else if (ofd.FileNames[FileID].Contains("SPECTRAL"))
                                {
                                    _scene.meshAnimation.poses[FileID].clusteringMode = Modes.Clustering.SPECTRAL;
                                    _scene.meshAnimation.poses[FileID].clusteringMethods[5] = new K_Spectral_Clustering();
                                }
                                else if (ofd.FileNames[FileID].Contains("C_PCA"))
                                {
                                    _scene.meshAnimation.poses[FileID].clusteringMode = Modes.Clustering.C_PCA;
                                    _scene.meshAnimation.poses[FileID].clusteringMethods[6] = new C_PCA_Clustering();
                                }
                                else // ERROR
                                {
                                    break;
                                }
                                _scene.meshAnimation.loadClustering(_scene.meshAnimation.poses[FileID], ofd.FileNames[FileID].Trim());
                            }
                            _progressBar.Increment(1);
                            _statusLabel.Text = ofd.FileNames[FileID].Trim() + " clustering loaded!!";

                            break;
                        }
                    default:
                        break;
                }
            }

            // GUI
            {
                int Count = _scene.meshAnimation.clusteringPerPoseMerging ? _scene.meshAnimation.poses[0].clusteringMethod.clustersMerged.Count : _scene.meshAnimation.poses[0].clusteringMethod.clusters.Count;

                textBoxClustersCount.Text = Count.ToString();
                numericUpDownClusterSelection.Maximum = Count - 1;

                comboBoxClusteringMode.SelectedIndex = (int)_scene.meshAnimation.pose.clusteringMode;
                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.CLUSTER;
            }

            checkBoxClusteringSetFixedColor_CheckedChanged(sender, e);

            Application.UseWaitCursor = false;
        }

        private void savePoseClustering_Click(object sender, EventArgs e)
        {
            string PoseName = _scene.meshAnimation.clusteringPerPose ? _scene.meshAnimation.pose.name : _scene.meshAnimation.restPose.name;

            _progressBar.Value = 0;
            _progressBar.Maximum = 1;
            {
                _scene.meshAnimation.saveClustering(false);
            }
            _progressBar.Increment(1);
            _statusLabel.Text = PoseName + " clustering saved!!";
        }

        private void saveAllPosesClustering_Click(object sender, EventArgs e)
        {
            _progressBar.Value = 0;
            _progressBar.Maximum = 1;
            {
                _scene.meshAnimation.saveClustering(true);
            }
            _progressBar.Increment(1);
            _statusLabel.Text = "All clustering saved!!";
        }

        private void loadPoseDG_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "FEA .fea|*.fea";
            ofd.Title = "Select Vertex Feature File";
            ofd.Multiselect = false;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();
            Application.DoEvents();

            if (ofd.FileName.Trim() == "")
                return;

            Application.UseWaitCursor = true;
            switch (ofd.FilterIndex)
            {
                case 1:
                    {
                        _progressBar.Value = 0;
                        _progressBar.Maximum = 1;

                        _scene.meshAnimation.loadDGs(_scene.meshAnimation.pose, ofd.FileName);

                        _progressBar.Increment(1);
                        _statusLabel.Text = ofd.FileName.Trim() + " deformation gradient loaded!!";

                        break;
                    }
                default:
                    break;
            }

            // GUI
            {
                //comboBoxDGmode.SelectedIndex = (int)_scene.meshAnimation.dgMode;
                //comboBoxDGComponentsMode.SelectedIndex = (int)_scene.meshAnimation.dgComponentsMode;
                //comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.DEFORMATION_GRADIENTS;
                //_scene.meshAnimation.setVerticesColor();
            }

            Application.UseWaitCursor = false;
        }

        private void loadAllPosesDG_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "FEA .fea|*.fea";
            ofd.Title = "Select Vertex Feature Files";
            ofd.Multiselect = true;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();
            Application.DoEvents();

            Application.UseWaitCursor = true;

            _progressBar.Value = 0;
            _progressBar.Maximum = ofd.FileNames.Length;

            for (int FileID = 0; FileID < ofd.FileNames.Length; FileID++)
            {
                if (ofd.FileNames[FileID].Trim() == "")
                    return;

                switch (ofd.FilterIndex)
                {
                    case 1:
                        {
                            _scene.meshAnimation.loadDGs(_scene.meshAnimation.poses[FileID], ofd.FileNames[FileID].Trim());

                            _progressBar.Increment(1);
                            _statusLabel.Text = ofd.FileNames[FileID].Trim() + " deformation gradient loaded!!";

                            break;
                        }
                    default:
                        break;
                }
            }

            // GUI
            {
                //comboBoxDGmode.SelectedIndex = (int)_scene.meshAnimation.dgMode;
                //comboBoxDGComponentsMode.SelectedIndex = (int)_scene.meshAnimation.dgComponentsMode;
                //comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.DEFORMATION_GRADIENTS;
                //_scene.meshAnimation.setVerticesColor();
            }

            Application.UseWaitCursor = false;
        }

        private void savePoseDG_Click(object sender, EventArgs e)
        {
            _progressBar.Value = 0;
            _progressBar.Maximum = 1;
            {
                _scene.meshAnimation.saveDGs(false);
            }
            _progressBar.Increment(1);
            _statusLabel.Text = _scene.meshAnimation.pose.name + " DG saved!!";
        }

        private void saveAllPosesDG_Click(object sender, EventArgs e)
        {
            _progressBar.Value = 0;
            _progressBar.Maximum = 1;
            {
                _scene.meshAnimation.saveDGs(true);
            }
            _progressBar.Increment(1);
            _statusLabel.Text = "All DG saved!!";
        }

        private void checkBoxTwoRingColoring_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.clustering2RingColoring = !_scene.meshAnimation.clustering2RingColoring;            
        }

        private void checkBoxTessEnable_CheckedChanged(object sender, EventArgs e)
        {
            _scene.tessellation = !_scene.tessellation;

            if (_scene.meshAnimation.selectedPose == -1)
                for (int i = 0; i < _scene.meshAnimation.poses.Count; i++)
                    _scene.meshAnimation.poses[i].primitiveMode = (checkBoxTessEnable.Checked) ? PrimitiveType.Patches : PrimitiveType.Triangles;
            else
                _scene.meshAnimation.pose.primitiveMode = (checkBoxTessEnable.Checked) ? PrimitiveType.Patches : PrimitiveType.Triangles;
        }

        private void numericUpDownTessLevelInner_ValueChanged(object sender, EventArgs e)
        {
            Example._scene.tessLevelInner = (float)numericUpDownTessLevelInner.Value;
            Example._scene.tessLevelOuter = (float)Example._scene.tessLevelInner;

            if (_scene.meshAnimation.selectedPose != -1)
            {
                int TotalTriangles = _scene.meshAnimation.pose.facetsCount;
                if (_scene.tessellation)
                    TotalTriangles *= GeometryFunctions.calculateTriangles((int)_scene.tessLevelInner);
                textBox6.Text = TotalTriangles.ToString();
            }
        }

        private void numericUpDownTessLevelOuter_ValueChanged(object sender, EventArgs e)
        {
            Example._scene.tessLevelOuter = (float)numericUpDownTessLevelOuter.Value;

            if (_scene.meshAnimation.selectedPose != -1)
            {
                int TotalTriangles = _scene.meshAnimation.pose.facetsCount;
                if (_scene.tessellation)
                    TotalTriangles *= GeometryFunctions.calculateTriangles((int)_scene.tessLevelInner);
                textBox6.Text = TotalTriangles.ToString();
            }
        }

        private void checkBoxInstancingEnable_CheckedChanged(object sender, EventArgs e)
        {
            _scene.instancing = !_scene.instancing;

            if (!_scene.instancing)
            {
                if (_scene.meshAnimation.selectedPose == -1)
                    for (int i = 0; i < _scene.meshAnimation.poses.Count; i++)
                        _scene.meshAnimation.poses[i].instancesCount = 1;
                else
                    _scene.meshAnimation.pose.instancesCount = 1;
            }
            else
                numericUpDownInstancesCount_ValueChanged(sender, e);
        }

        private void numericUpDown17_ValueChanged_1(object sender, EventArgs e)
        {
            _scene.instance_translation = new Vector3((float)numericUpDown17.Value, (float)numericUpDown15.Value, (float)numericUpDown31.Value);
        }

        private void numericUpDown15_ValueChanged_1(object sender, EventArgs e)
        {
            _scene.instance_translation = new Vector3((float)numericUpDown17.Value, (float)numericUpDown15.Value, (float)numericUpDown31.Value);
        }

        private void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            _scene.instance_translation = new Vector3((float)numericUpDown17.Value, (float)numericUpDown15.Value, (float)numericUpDown31.Value);
        }

        private void numericUpDownInstancesCount_ValueChanged(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.selectedPose == -1)
                for (int i = 0; i < _scene.meshAnimation.poses.Count; i++)
                    _scene.meshAnimation.poses[i].instancesCount = (int)numericUpDownInstancesCount.Value;
            else
                _scene.meshAnimation.pose.instancesCount = (int)numericUpDownInstancesCount.Value;
        }

        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.sma != null)
                _scene.meshAnimation.sma.weightingMode = (Modes.Weighting)comboBox3.SelectedIndex;
        }

        private void numericUpDown32_ValueChanged(object sender, EventArgs e)
        {
            _scene.discardThreshold = (float)numericUpDown32.Value;
        }

        private void checkBox38_CheckedChanged(object sender, EventArgs e)
        {
            _scene.randomBias = !_scene.randomBias;
        }

        private void checkBox40_CheckedChanged(object sender, EventArgs e)
        {
            if (!_modelSelectionChanged)
                _scene.meshAnimation.pose.convexHull.drawable = !_scene.meshAnimation.pose.convexHull.drawable;
        }

        private void checkBox41_CheckedChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.maxKfixed = !_scene.multiFragmentRendering.maxKfixed;
        }

        private void numericUpDown34_ValueChanged_1(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.maxK = (int) numericUpDown34.Value;
        }

        private void numericUpDown35_ValueChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.maxKerror = (int)numericUpDown35.Value;
        }

        private void numericUpDown36_ValueChanged(object sender, EventArgs e)
        {
            _scene.multiFragmentRendering.maxKmemory = (int) numericUpDown36.Value;
        }
        
        private void numericUpDown37_ValueChanged(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.sma != null)
            {
                _scene.meshAnimation.sma.fitMatricesIter = (int)numericUpDownFittingMatricesIter.Value;

                _scene.meshAnimation.sma.fittingIterations = Math.Max(Math.Max(
                    _scene.meshAnimation.sma.fitMatricesIter, _scene.meshAnimation.sma.fitRestPoseIter)
                    , _scene.meshAnimation.sma.fitWeightsIter);

                textBoxFittingIter.Text = _scene.meshAnimation.sma.fittingIterations.ToString();
            }
        }

        private void numericUpDown38_ValueChanged(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.sma != null)
            {
                _scene.meshAnimation.sma.fitRestPoseIter = (int)numericUpDownFittingRestPoseIter.Value;
                _scene.meshAnimation.sma.fittingIterations = Math.Max(Math.Max(
                    _scene.meshAnimation.sma.fitMatricesIter, _scene.meshAnimation.sma.fitRestPoseIter), 
                    _scene.meshAnimation.sma.fitWeightsIter);

                textBoxFittingIter.Text = _scene.meshAnimation.sma.fittingIterations.ToString();
            }
        }

        private void numericUpDown39_ValueChanged(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.sma != null)
            {
                _scene.meshAnimation.sma.fitWeightsIter = (int)numericUpDownFittingWeightsIter.Value;

                _scene.meshAnimation.sma.fittingIterations = Math.Max(Math.Max(
                    _scene.meshAnimation.sma.fitMatricesIter, _scene.meshAnimation.sma.fitRestPoseIter),
                    _scene.meshAnimation.sma.fitWeightsIter);

                textBoxFittingIter.Text = _scene.meshAnimation.sma.fittingIterations.ToString();
            }
        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                _scene.meshAnimation.sma.computeFittingMatrices(_scene.meshAnimation, _scene.meshAnimation.sma.fitMatricesIter, _progressBar);
                numericUpDownFittingMatricesIter.Value += 1;
                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.NONE;
                _statusLabel.Text = "Matrices Fitting Completed!!";
            }
            Application.UseWaitCursor = false;

            buttonComputeFinalPos_Click(sender, e);

            if (ButtonSmaError.Enabled)
            {
                _scene.meshAnimation.sma.createForm();
                ButtonSmaError.Enabled = false;
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                _scene.meshAnimation.sma.computeFittingRestPose(_scene.meshAnimation, _progressBar);
                numericUpDownFittingRestPoseIter.Value += 1;
                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.NONE;
                _statusLabel.Text = "Rest Pose Fitting Completed!!";
            }
            Application.UseWaitCursor = false;

            buttonComputeFinalPos_Click(sender, e);
            if (ButtonSmaError.Enabled)
            {
                _scene.meshAnimation.sma.createForm();
                ButtonSmaError.Enabled = false;
            }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                _scene.meshAnimation.sma.computeFittingWeights(_scene.meshAnimation, _progressBar);
                numericUpDownFittingWeightsIter.Value += 1;
                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.NONE;
                _statusLabel.Text = "Weights Fitting Completed!!";
            }
            Application.UseWaitCursor = false;

            buttonComputeFinalPos_Click(sender, e);
            if (ButtonSmaError.Enabled)
            {
                _scene.meshAnimation.sma.createForm();
                ButtonSmaError.Enabled = false;
            }
        }

        private void rMAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                _scene.meshAnimation.sma.computeFitting(_scene.meshAnimation, _progressBar);
                if (!_scene.meshAnimation.sma.loadRMA(_scene.meshAnimation, _progressBar, _statusLabel))
                {
                    _scene.meshAnimation.sma.errorDataVertex.computeApproxDataError();
                    _scene.meshAnimation.sma.errorDataNormal.computeApproxDataError();

                    _scene.meshAnimation.sma.computeApproxModels(_scene.meshAnimation);
                    _scene.meshAnimation.sma.createChartError();

                    comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.SKINNING_ERROR;

                    _statusLabel.Text = "Skinning Error Completed!!";
                }
            }
            Application.UseWaitCursor = false;

            // GUI
            {
                for (int i = treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes.Count - 1; i >= 0; --i)
                    treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes[i].Remove();

                for (int i = 0; i < _scene.meshAnimation.sma.numPoses; ++i)
                {
                    treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes.Add("Pose_" + i.ToString());
                    treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes[i].Tag = i;
                }

                if (_scene.meshAnimation.addMeanPoseToTree)
                    treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Nodes[_scene.meshAnimation.sma.numPoses - 1].Text += "#";

                treeView_models.Nodes[0].Nodes[_scene.meshAnimation.selectedRestPose].Expand();

                if (ButtonSmaError.Enabled)
                {
                    _scene.meshAnimation.sma.createForm();
                    ButtonSmaError.Enabled = false;
                }
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                _scene.meshAnimation.sma.colorMin = colorDialog1.Color;
                _scene.meshAnimation.setVerticesColor();
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                _scene.meshAnimation.sma.colorMax = colorDialog1.Color;
                _scene.meshAnimation.setVerticesColor();
            }
        }

        private void checkBox42_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.sma.colorHeatMap = checkBox42.Checked;

            if (_scene.meshAnimation.sma.fittingErrorMode == Modes.FittingError.STED)
                _scene.meshAnimation.sma.errorDataVertex.normalizeErrorSTED();
            else
                _scene.meshAnimation.sma.errorDataVertex.normalizeError();
            _scene.meshAnimation.setVerticesColor();
        }

        private void checkBox43_CheckedChanged(object sender, EventArgs e)
        {
            _scene.meshAnimation.sma.colorTemporalCoherence = checkBox43.Checked;
            if (_scene.meshAnimation.sma.fittingErrorMode == Modes.FittingError.STED)
                _scene.meshAnimation.sma.errorDataVertex.normalizeErrorSTED();
            else
                _scene.meshAnimation.sma.errorDataVertex.normalizeError();
            _scene.meshAnimation.setVerticesColor();
        }

        private void button38_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            {
                _scene.meshAnimation.sma.computeInitMatrices(_scene.meshAnimation);
                numericUpDownFittingMatricesIter.Value += 1;
                comboBoxVertexColoringMode.SelectedIndex = (int)Modes.VertexColoring.NONE;
                _statusLabel.Text = "Matrices Init Completed!!";
            }
            Application.UseWaitCursor = false;

            buttonComputeFinalPos_Click(sender, e);

            if (ButtonSmaError.Enabled)
            {
                _scene.meshAnimation.sma.createForm();
                ButtonSmaError.Enabled = false;
            }
        }

        private void numericUpDown33_ValueChanged(object sender, EventArgs e)
        {
            if (_scene.meshAnimation.sma != null)
                _scene.meshAnimation.sma.fittingThreshold = (int)numericUpDown33.Value;
        }
    }
}