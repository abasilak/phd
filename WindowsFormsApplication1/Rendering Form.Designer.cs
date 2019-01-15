using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    public partial class Example
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // this.RenderWindow = new GLControl(new GraphicsMode(), 4, 3, GraphicsContextFlags.ForwardCompatible);

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Example));
            this.groupBox_Camera = new System.Windows.Forms.GroupBox();
            this.numericUpDown24 = new System.Windows.Forms.NumericUpDown();
            this.button19 = new System.Windows.Forms.Button();
            this.numericUpDown23 = new System.Windows.Forms.NumericUpDown();
            this.label36 = new System.Windows.Forms.Label();
            this.numericUpDown22 = new System.Windows.Forms.NumericUpDown();
            this.checkBox24 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.checkBox29 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.label37 = new System.Windows.Forms.Label();
            this.comboBox11 = new System.Windows.Forms.ComboBox();
            this.checkBox22 = new System.Windows.Forms.CheckBox();
            this.label33 = new System.Windows.Forms.Label();
            this.checkBox23 = new System.Windows.Forms.CheckBox();
            this.numericUpDown19 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown20 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown21 = new System.Windows.Forms.NumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.numericUpDown26 = new System.Windows.Forms.NumericUpDown();
            this.label39 = new System.Windows.Forms.Label();
            this.numericUpDown25 = new System.Windows.Forms.NumericUpDown();
            this.label38 = new System.Windows.Forms.Label();
            this.comboBox13 = new System.Windows.Forms.ComboBox();
            this.comboBox10 = new System.Windows.Forms.ComboBox();
            this.label35 = new System.Windows.Forms.Label();
            this.comboBoxRenderingType = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.label61 = new System.Windows.Forms.Label();
            this.numericUpDown35 = new System.Windows.Forms.NumericUpDown();
            this.label60 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.numericUpDown36 = new System.Windows.Forms.NumericUpDown();
            this.textBoxKPercentage = new System.Windows.Forms.TextBox();
            this.numericUpDown34 = new System.Windows.Forms.NumericUpDown();
            this.textBoxKValue = new System.Windows.Forms.TextBox();
            this.checkBox41 = new System.Windows.Forms.CheckBox();
            this.checkBox35 = new System.Windows.Forms.CheckBox();
            this.checkBox26 = new System.Windows.Forms.CheckBox();
            this.checkBox25 = new System.Windows.Forms.CheckBox();
            this.textBoxMemorySize = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.checkBox27 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.textBoxTotalPasses = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button27 = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.treeView_models = new System.Windows.Forms.TreeView();
            this.button21 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this.button16 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.button11 = new System.Windows.Forms.Button();
            this.checkBox34 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groups = new System.Windows.Forms.CheckBox();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.checkBox14 = new System.Windows.Forms.CheckBox();
            this.FileStep = new System.Windows.Forms.NumericUpDown();
            this.button10 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button18 = new System.Windows.Forms.Button();
            this.checkBox20 = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.numericUpDown10 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown9 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown8 = new System.Windows.Forms.NumericUpDown();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.numericUpDown7 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBox_lightSpot = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.checkBox38 = new System.Windows.Forms.CheckBox();
            this.label58 = new System.Windows.Forms.Label();
            this.numericUpDown32 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInstancesCount = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown31 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown15 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown17 = new System.Windows.Forms.NumericUpDown();
            this.checkBox37 = new System.Windows.Forms.CheckBox();
            this.Tesselation = new System.Windows.Forms.GroupBox();
            this.label57 = new System.Windows.Forms.Label();
            this.numericUpDownTessLevelOuter = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTessLevelInner = new System.Windows.Forms.NumericUpDown();
            this.checkBoxTessEnable = new System.Windows.Forms.CheckBox();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.label42 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.checkBox28 = new System.Windows.Forms.CheckBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.label56 = new System.Windows.Forms.Label();
            this.numericUpDownMaxFPS = new System.Windows.Forms.NumericUpDown();
            this.checkBox13 = new System.Windows.Forms.CheckBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.button_backGrColor = new System.Windows.Forms.Button();
            this.button_backGrTex = new System.Windows.Forms.Button();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.numericUpDown_gamma = new System.Windows.Forms.NumericUpDown();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox40 = new System.Windows.Forms.CheckBox();
            this.checkBox21 = new System.Windows.Forms.CheckBox();
            this.numericUpDown18 = new System.Windows.Forms.NumericUpDown();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.comboBox8 = new System.Windows.Forms.ComboBox();
            this.comboBox9 = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.label49 = new System.Windows.Forms.Label();
            this.numericUpDown27 = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.label28 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown16 = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.numericUpDown12 = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.numericUpDown11 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown28 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown29 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDown14 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown13 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.checkBox19 = new System.Windows.Forms.CheckBox();
            this.checkBox18 = new System.Windows.Forms.CheckBox();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.checkBox17 = new System.Windows.Forms.CheckBox();
            this.checkBox16 = new System.Windows.Forms.CheckBox();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.button17 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setToModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setToBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CSGtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.setOperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intersectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.differenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSMA = new System.Windows.Forms.ToolStripMenuItem();
            this.featureToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedPoseToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.sMAToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedPoseToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sMAToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.rMAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.featureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedPoseToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.clusteringToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedPoseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMAToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedPoseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadEditableModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsRestPoseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propagateColoringModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBox18 = new System.Windows.Forms.ComboBox();
            this.groupBox34 = new System.Windows.Forms.GroupBox();
            this.checkBox43 = new System.Windows.Forms.CheckBox();
            this.checkBox42 = new System.Windows.Forms.CheckBox();
            this.button37 = new System.Windows.Forms.Button();
            this.button36 = new System.Windows.Forms.Button();
            this.ButtonSmaError = new System.Windows.Forms.Button();
            this.button30 = new System.Windows.Forms.Button();
            this.groupBox32 = new System.Windows.Forms.GroupBox();
            this.button38 = new System.Windows.Forms.Button();
            this.button35 = new System.Windows.Forms.Button();
            this.button31 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.label64 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.numericUpDown33 = new System.Windows.Forms.NumericUpDown();
            this.textBoxFittingIter = new System.Windows.Forms.TextBox();
            this.label67 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.numericUpDownFittingWeightsIter = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFittingRestPoseIter = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFittingMatricesIter = new System.Windows.Forms.NumericUpDown();
            this.label62 = new System.Windows.Forms.Label();
            this.numericUpDownFittingIterations = new System.Windows.Forms.NumericUpDown();
            this.label48 = new System.Windows.Forms.Label();
            this.comboBox17 = new System.Windows.Forms.ComboBox();
            this.button25 = new System.Windows.Forms.Button();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.DG = new System.Windows.Forms.TabPage();
            this.numericUpDownSelectedVertex = new System.Windows.Forms.NumericUpDown();
            this.button28 = new System.Windows.Forms.Button();
            this.numericUpDown30 = new System.Windows.Forms.NumericUpDown();
            this.checkBox31 = new System.Windows.Forms.CheckBox();
            this.checkBox39 = new System.Windows.Forms.CheckBox();
            this.checkBox30 = new System.Windows.Forms.CheckBox();
            this.comboBoxDGComponentsMode = new System.Windows.Forms.ComboBox();
            this.comboBoxDGmode = new System.Windows.Forms.ComboBox();
            this.button22 = new System.Windows.Forms.Button();
            this.Clustering = new System.Windows.Forms.TabPage();
            this.button34 = new System.Windows.Forms.Button();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBoxMergeClusterings = new System.Windows.Forms.CheckBox();
            this.button33 = new System.Windows.Forms.Button();
            this.button32 = new System.Windows.Forms.Button();
            this.button29 = new System.Windows.Forms.Button();
            this.textBoxClustersCount = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.comboBoxClusteringMode = new System.Windows.Forms.ComboBox();
            this.checkBox32 = new System.Windows.Forms.CheckBox();
            this.groupBox29 = new System.Windows.Forms.GroupBox();
            this.checkBox36 = new System.Windows.Forms.CheckBox();
            this.numericUpDownMergingPrevTolerance = new System.Windows.Forms.NumericUpDown();
            this.checkBoxClusteringSetFixedColor = new System.Windows.Forms.CheckBox();
            this.checkBox33 = new System.Windows.Forms.CheckBox();
            this.checkBoxDrawNeighbors = new System.Windows.Forms.CheckBox();
            this.checkBoxDrawRegions = new System.Windows.Forms.CheckBox();
            this.label45 = new System.Windows.Forms.Label();
            this.numericUpDownClusterSelection = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownBoneSelection = new System.Windows.Forms.NumericUpDown();
            this.checkBoxDrawSpheres = new System.Windows.Forms.CheckBox();
            this.label43 = new System.Windows.Forms.Label();
            this.numericUpDownClustersCount = new System.Windows.Forms.NumericUpDown();
            this.button23 = new System.Windows.Forms.Button();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.label55 = new System.Windows.Forms.Label();
            this.numericUpDownBasisVectorsCount = new System.Windows.Forms.NumericUpDown();
            this.checkBoxNipals = new System.Windows.Forms.CheckBox();
            this.label54 = new System.Windows.Forms.Label();
            this.comboBoxDistanceMode = new System.Windows.Forms.ComboBox();
            this.textBoxClusteringErrorTolerance = new System.Windows.Forms.TextBox();
            this.textBoxClusteringIterationsCount = new System.Windows.Forms.TextBox();
            this.numericUpDownClusteringError = new System.Windows.Forms.NumericUpDown();
            this.checkBoxSkinningScale = new System.Windows.Forms.CheckBox();
            this.groupBox33 = new System.Windows.Forms.GroupBox();
            this.numericUpDownSpectralEigenGap = new System.Windows.Forms.NumericUpDown();
            this.checkBoxNNG = new System.Windows.Forms.CheckBox();
            this.comboBoxClusteringSpectralGraphMode = new System.Windows.Forms.ComboBox();
            this.numericUpDownPercentageSpectral = new System.Windows.Forms.NumericUpDown();
            this.comboBox12 = new System.Windows.Forms.ComboBox();
            this.label52 = new System.Windows.Forms.Label();
            this.checkBoxRandomSeeding = new System.Windows.Forms.CheckBox();
            this.label51 = new System.Windows.Forms.Label();
            this.comboBoxClusteringDistanceMode = new System.Windows.Forms.ComboBox();
            this.label50 = new System.Windows.Forms.Label();
            this.numericUpDownClusteringTolerance = new System.Windows.Forms.NumericUpDown();
            this.textBoxClusteringErrorTotal = new System.Windows.Forms.TextBox();
            this.numericUpDownClusteringIterations = new System.Windows.Forms.NumericUpDown();
            this.label53 = new System.Windows.Forms.Label();
            this.SMA = new System.Windows.Forms.TabPage();
            this.groupBox31 = new System.Windows.Forms.GroupBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label47 = new System.Windows.Forms.Label();
            this.button24 = new System.Windows.Forms.Button();
            this.numericUpDownPFactor = new System.Windows.Forms.NumericUpDown();
            this.label44 = new System.Windows.Forms.Label();
            this.comboBoxVertexColoringMode = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sMAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMAToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.clusteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMAToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.RenderWindow = new OpenTK.GLControl();
            this.groupBox_Camera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown22)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox25.SuspendLayout();
            this.groupBox21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown21)).BeginInit();
            this.groupBox18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown25)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown35)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown36)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown34)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.groupBox11.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileStep)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInstancesCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown31)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown17)).BeginInit();
            this.Tesselation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTessLevelOuter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTessLevelInner)).BeginInit();
            this.groupBox26.SuspendLayout();
            this.groupBox22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxFPS)).BeginInit();
            this.groupBox17.SuspendLayout();
            this.groupBox16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_gamma)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown18)).BeginInit();
            this.groupBox14.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown16)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown28)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown29)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.tabPage7.SuspendLayout();
            this.groupBox23.SuspendLayout();
            this.groupBox24.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.groupBox34.SuspendLayout();
            this.groupBox32.SuspendLayout();
            this.groupBox20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown33)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFittingWeightsIter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFittingRestPoseIter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFittingMatricesIter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFittingIterations)).BeginInit();
            this.tabControl4.SuspendLayout();
            this.DG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSelectedVertex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown30)).BeginInit();
            this.Clustering.SuspendLayout();
            this.groupBox29.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMergingPrevTolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClusterSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBoneSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClustersCount)).BeginInit();
            this.groupBox27.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBasisVectorsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClusteringError)).BeginInit();
            this.groupBox33.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpectralEigenGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPercentageSpectral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClusteringTolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClusteringIterations)).BeginInit();
            this.SMA.SuspendLayout();
            this.groupBox31.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPFactor)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Camera
            // 
            this.groupBox_Camera.Controls.Add(this.numericUpDown24);
            this.groupBox_Camera.Controls.Add(this.button19);
            this.groupBox_Camera.Controls.Add(this.numericUpDown23);
            this.groupBox_Camera.Controls.Add(this.label36);
            this.groupBox_Camera.Controls.Add(this.numericUpDown22);
            this.groupBox_Camera.Controls.Add(this.checkBox24);
            this.groupBox_Camera.Controls.Add(this.groupBox1);
            this.groupBox_Camera.Controls.Add(this.label2);
            this.groupBox_Camera.Controls.Add(this.numericUpDown1);
            this.groupBox_Camera.Controls.Add(this.checkBox1);
            this.groupBox_Camera.Controls.Add(this.label1);
            this.groupBox_Camera.Controls.Add(this.comboBox1);
            this.groupBox_Camera.Location = new System.Drawing.Point(14, 12);
            this.groupBox_Camera.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox_Camera.Name = "groupBox_Camera";
            this.groupBox_Camera.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox_Camera.Size = new System.Drawing.Size(432, 271);
            this.groupBox_Camera.TabIndex = 1;
            this.groupBox_Camera.TabStop = false;
            this.groupBox_Camera.Text = "Camera";
            // 
            // numericUpDown24
            // 
            this.numericUpDown24.DecimalPlaces = 2;
            this.numericUpDown24.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown24.Location = new System.Drawing.Point(262, 106);
            this.numericUpDown24.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown24.Maximum = new decimal(new int[] {
            1800,
            0,
            0,
            65536});
            this.numericUpDown24.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDown24.Name = "numericUpDown24";
            this.numericUpDown24.Size = new System.Drawing.Size(88, 20);
            this.numericUpDown24.TabIndex = 11;
            this.numericUpDown24.ThousandsSeparator = true;
            this.numericUpDown24.ValueChanged += new System.EventHandler(this.numericUpDown24_ValueChanged);
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(12, 135);
            this.button19.Margin = new System.Windows.Forms.Padding(6);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(345, 26);
            this.button19.TabIndex = 9;
            this.button19.Text = "Rotate";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // numericUpDown23
            // 
            this.numericUpDown23.DecimalPlaces = 2;
            this.numericUpDown23.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown23.Location = new System.Drawing.Point(162, 106);
            this.numericUpDown23.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown23.Maximum = new decimal(new int[] {
            1800,
            0,
            0,
            65536});
            this.numericUpDown23.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDown23.Name = "numericUpDown23";
            this.numericUpDown23.Size = new System.Drawing.Size(88, 20);
            this.numericUpDown23.TabIndex = 10;
            this.numericUpDown23.ThousandsSeparator = true;
            this.numericUpDown23.ValueChanged += new System.EventHandler(this.numericUpDown23_ValueChanged);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(12, 108);
            this.label36.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(31, 13);
            this.label36.TabIndex = 8;
            this.label36.Text = "XYZ:";
            // 
            // numericUpDown22
            // 
            this.numericUpDown22.DecimalPlaces = 2;
            this.numericUpDown22.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown22.Location = new System.Drawing.Point(62, 106);
            this.numericUpDown22.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown22.Maximum = new decimal(new int[] {
            1800,
            0,
            0,
            65536});
            this.numericUpDown22.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDown22.Name = "numericUpDown22";
            this.numericUpDown22.Size = new System.Drawing.Size(88, 20);
            this.numericUpDown22.TabIndex = 7;
            this.numericUpDown22.ThousandsSeparator = true;
            this.numericUpDown22.ValueChanged += new System.EventHandler(this.numericUpDown22_ValueChanged);
            // 
            // checkBox24
            // 
            this.checkBox24.AutoSize = true;
            this.checkBox24.Location = new System.Drawing.Point(111, 82);
            this.checkBox24.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox24.Name = "checkBox24";
            this.checkBox24.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox24.Size = new System.Drawing.Size(102, 17);
            this.checkBox24.TabIndex = 6;
            this.checkBox24.Text = "Inverse Z-Buffer";
            this.checkBox24.UseVisualStyleBackColor = true;
            this.checkBox24.CheckedChanged += new System.EventHandler(this.checkBox24_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 173);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(345, 75);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Z";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(41, 45);
            this.textBox2.Margin = new System.Windows.Forms.Padding(6);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(266, 20);
            this.textBox2.TabIndex = 9;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(41, 16);
            this.textBox1.Margin = new System.Windows.Forms.Padding(6);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(266, 20);
            this.textBox1.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 48);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Far:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Near:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "FoV:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 1;
            this.numericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(53, 50);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1800,
            0,
            0,
            65536});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(304, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.ThousandsSeparator = true;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(12, 82);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox1.Size = new System.Drawing.Size(87, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Best Clipping";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "View:";
            // 
            // comboBox1
            // 
            this.comboBox1.DisplayMember = "0";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "User",
            "Front",
            "Top",
            "Right"});
            this.comboBox1.Location = new System.Drawing.Point(53, 16);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox1.MaxDropDownItems = 4;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(302, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.Text = "User";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Location = new System.Drawing.Point(1046, 52);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(502, 893);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox25);
            this.tabPage1.Controls.Add(this.groupBox21);
            this.tabPage1.Controls.Add(this.groupBox18);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox11);
            this.tabPage1.Controls.Add(this.button27);
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage1.Size = new System.Drawing.Size(494, 867);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Scene";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox25
            // 
            this.groupBox25.BackColor = System.Drawing.Color.LightCoral;
            this.groupBox25.Controls.Add(this.checkBox29);
            this.groupBox25.Controls.Add(this.checkBox11);
            this.groupBox25.Location = new System.Drawing.Point(188, 400);
            this.groupBox25.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox25.Size = new System.Drawing.Size(90, 69);
            this.groupBox25.TabIndex = 20;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "Translucency";
            // 
            // checkBox29
            // 
            this.checkBox29.AutoSize = true;
            this.checkBox29.Location = new System.Drawing.Point(12, 45);
            this.checkBox29.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox29.Name = "checkBox29";
            this.checkBox29.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox29.Size = new System.Drawing.Size(60, 17);
            this.checkBox29.TabIndex = 21;
            this.checkBox29.Text = "Closest";
            this.checkBox29.UseVisualStyleBackColor = true;
            this.checkBox29.CheckedChanged += new System.EventHandler(this.checkBox29_CheckedChanged);
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Location = new System.Drawing.Point(11, 25);
            this.checkBox11.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox11.Size = new System.Drawing.Size(59, 17);
            this.checkBox11.TabIndex = 20;
            this.checkBox11.Text = "Enable";
            this.checkBox11.UseVisualStyleBackColor = true;
            this.checkBox11.CheckedChanged += new System.EventHandler(this.checkBox11_CheckedChanged);
            // 
            // groupBox21
            // 
            this.groupBox21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.groupBox21.Controls.Add(this.label37);
            this.groupBox21.Controls.Add(this.comboBox11);
            this.groupBox21.Controls.Add(this.checkBox22);
            this.groupBox21.Controls.Add(this.label33);
            this.groupBox21.Controls.Add(this.checkBox23);
            this.groupBox21.Controls.Add(this.numericUpDown19);
            this.groupBox21.Controls.Add(this.numericUpDown20);
            this.groupBox21.Controls.Add(this.numericUpDown21);
            this.groupBox21.Controls.Add(this.label34);
            this.groupBox21.Controls.Add(this.label32);
            this.groupBox21.Location = new System.Drawing.Point(10, 792);
            this.groupBox21.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox21.Size = new System.Drawing.Size(466, 131);
            this.groupBox21.TabIndex = 1;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "Trimming";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(11, 19);
            this.label37.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(61, 13);
            this.label37.TabIndex = 27;
            this.label37.Text = "Technique:";
            // 
            // comboBox11
            // 
            this.comboBox11.DisplayMember = "0";
            this.comboBox11.FormattingEnabled = true;
            this.comboBox11.Location = new System.Drawing.Point(76, 16);
            this.comboBox11.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox11.MaxDropDownItems = 4;
            this.comboBox11.Name = "comboBox11";
            this.comboBox11.Size = new System.Drawing.Size(300, 21);
            this.comboBox11.TabIndex = 26;
            this.comboBox11.SelectedIndexChanged += new System.EventHandler(this.comboBox11_SelectedIndexChanged);
            // 
            // checkBox22
            // 
            this.checkBox22.AutoSize = true;
            this.checkBox22.Location = new System.Drawing.Point(113, 104);
            this.checkBox22.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox22.Name = "checkBox22";
            this.checkBox22.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox22.Size = new System.Drawing.Size(125, 17);
            this.checkBox22.TabIndex = 17;
            this.checkBox22.Text = "ReTrim Father Frame";
            this.checkBox22.UseVisualStyleBackColor = true;
            this.checkBox22.CheckedChanged += new System.EventHandler(this.checkBox22_CheckedChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(175, 51);
            this.label33.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(37, 13);
            this.label33.TabIndex = 22;
            this.label33.Text = "Angle:";
            // 
            // checkBox23
            // 
            this.checkBox23.AutoSize = true;
            this.checkBox23.Location = new System.Drawing.Point(17, 104);
            this.checkBox23.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox23.Name = "checkBox23";
            this.checkBox23.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox23.Size = new System.Drawing.Size(73, 17);
            this.checkBox23.TabIndex = 25;
            this.checkBox23.Text = "Z-Fighting";
            this.checkBox23.UseVisualStyleBackColor = true;
            this.checkBox23.CheckedChanged += new System.EventHandler(this.checkBox23_CheckedChanged);
            // 
            // numericUpDown19
            // 
            this.numericUpDown19.DecimalPlaces = 2;
            this.numericUpDown19.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown19.Location = new System.Drawing.Point(76, 49);
            this.numericUpDown19.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown19.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown19.Name = "numericUpDown19";
            this.numericUpDown19.Size = new System.Drawing.Size(88, 20);
            this.numericUpDown19.TabIndex = 19;
            this.numericUpDown19.ThousandsSeparator = true;
            this.numericUpDown19.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown19.ValueChanged += new System.EventHandler(this.numericUpDown19_ValueChanged);
            // 
            // numericUpDown20
            // 
            this.numericUpDown20.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown20.Location = new System.Drawing.Point(219, 49);
            this.numericUpDown20.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown20.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown20.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown20.Name = "numericUpDown20";
            this.numericUpDown20.Size = new System.Drawing.Size(157, 20);
            this.numericUpDown20.TabIndex = 21;
            this.numericUpDown20.ThousandsSeparator = true;
            this.numericUpDown20.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown20.ValueChanged += new System.EventHandler(this.numericUpDown20_ValueChanged);
            // 
            // numericUpDown21
            // 
            this.numericUpDown21.Location = new System.Drawing.Point(192, 75);
            this.numericUpDown21.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown21.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown21.Name = "numericUpDown21";
            this.numericUpDown21.Size = new System.Drawing.Size(184, 20);
            this.numericUpDown21.TabIndex = 23;
            this.numericUpDown21.ThousandsSeparator = true;
            this.numericUpDown21.ValueChanged += new System.EventHandler(this.numericUpDown21_ValueChanged);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(14, 75);
            this.label34.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(128, 13);
            this.label34.TabIndex = 24;
            this.label34.Text = "#Poses for a deformation:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(14, 47);
            this.label32.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(37, 13);
            this.label32.TabIndex = 20;
            this.label32.Text = "Plane:";
            // 
            // groupBox18
            // 
            this.groupBox18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox18.Controls.Add(this.numericUpDown26);
            this.groupBox18.Controls.Add(this.label39);
            this.groupBox18.Controls.Add(this.numericUpDown25);
            this.groupBox18.Controls.Add(this.label38);
            this.groupBox18.Controls.Add(this.comboBox13);
            this.groupBox18.Controls.Add(this.comboBox10);
            this.groupBox18.Controls.Add(this.label35);
            this.groupBox18.Controls.Add(this.comboBoxRenderingType);
            this.groupBox18.Controls.Add(this.label14);
            this.groupBox18.Controls.Add(this.comboBox5);
            this.groupBox18.Controls.Add(this.label17);
            this.groupBox18.Location = new System.Drawing.Point(10, 478);
            this.groupBox18.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox18.Size = new System.Drawing.Size(468, 133);
            this.groupBox18.TabIndex = 1;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Modes";
            // 
            // numericUpDown26
            // 
            this.numericUpDown26.DecimalPlaces = 2;
            this.numericUpDown26.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown26.Location = new System.Drawing.Point(83, 96);
            this.numericUpDown26.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown26.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDown26.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown26.Name = "numericUpDown26";
            this.numericUpDown26.Size = new System.Drawing.Size(88, 20);
            this.numericUpDown26.TabIndex = 30;
            this.numericUpDown26.ThousandsSeparator = true;
            this.numericUpDown26.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown26.ValueChanged += new System.EventHandler(this.numericUpDown26_ValueChanged);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(14, 98);
            this.label39.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(34, 13);
            this.label39.TabIndex = 29;
            this.label39.Text = "Point:";
            // 
            // numericUpDown25
            // 
            this.numericUpDown25.DecimalPlaces = 2;
            this.numericUpDown25.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown25.Location = new System.Drawing.Point(248, 96);
            this.numericUpDown25.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown25.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDown25.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown25.Name = "numericUpDown25";
            this.numericUpDown25.Size = new System.Drawing.Size(88, 20);
            this.numericUpDown25.TabIndex = 28;
            this.numericUpDown25.ThousandsSeparator = true;
            this.numericUpDown25.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown25.ValueChanged += new System.EventHandler(this.numericUpDown25_ValueChanged);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(183, 98);
            this.label38.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(30, 13);
            this.label38.TabIndex = 28;
            this.label38.Text = "Line:";
            // 
            // comboBox13
            // 
            this.comboBox13.DisplayMember = "0";
            this.comboBox13.FormattingEnabled = true;
            this.comboBox13.Items.AddRange(new object[] {
            "Back",
            "Front",
            "FrontBack"});
            this.comboBox13.Location = new System.Drawing.Point(83, 69);
            this.comboBox13.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox13.MaxDropDownItems = 4;
            this.comboBox13.Name = "comboBox13";
            this.comboBox13.Size = new System.Drawing.Size(146, 21);
            this.comboBox13.TabIndex = 27;
            this.comboBox13.SelectedIndexChanged += new System.EventHandler(this.comboBox13_SelectedIndexChanged_1);
            // 
            // comboBox10
            // 
            this.comboBox10.DisplayMember = "0";
            this.comboBox10.FormattingEnabled = true;
            this.comboBox10.Items.AddRange(new object[] {
            "Point",
            "Line",
            "Fill"});
            this.comboBox10.Location = new System.Drawing.Point(247, 69);
            this.comboBox10.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox10.MaxDropDownItems = 4;
            this.comboBox10.Name = "comboBox10";
            this.comboBox10.Size = new System.Drawing.Size(122, 21);
            this.comboBox10.TabIndex = 25;
            this.comboBox10.SelectedIndexChanged += new System.EventHandler(this.comboBox10_SelectedIndexChanged);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(12, 72);
            this.label35.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(48, 13);
            this.label35.TabIndex = 26;
            this.label35.Text = "Polygon:";
            // 
            // comboBoxRenderingType
            // 
            this.comboBoxRenderingType.DisplayMember = "0";
            this.comboBoxRenderingType.FormattingEnabled = true;
            this.comboBoxRenderingType.Location = new System.Drawing.Point(83, 16);
            this.comboBoxRenderingType.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxRenderingType.MaxDropDownItems = 4;
            this.comboBoxRenderingType.Name = "comboBoxRenderingType";
            this.comboBoxRenderingType.Size = new System.Drawing.Size(286, 21);
            this.comboBoxRenderingType.TabIndex = 21;
            this.comboBoxRenderingType.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 19);
            this.label14.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "Rendering:";
            // 
            // comboBox5
            // 
            this.comboBox5.DisplayMember = "0";
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            "Phong",
            "Cook"});
            this.comboBox5.Location = new System.Drawing.Point(83, 43);
            this.comboBox5.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox5.MaxDropDownItems = 4;
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(286, 21);
            this.comboBox5.TabIndex = 23;
            this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 46);
            this.label17.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(62, 13);
            this.label17.TabIndex = 24;
            this.label17.Text = "Illumination:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox2.Controls.Add(this.groupBox28);
            this.groupBox2.Controls.Add(this.checkBox35);
            this.groupBox2.Controls.Add(this.checkBox26);
            this.groupBox2.Controls.Add(this.checkBox25);
            this.groupBox2.Controls.Add(this.textBoxMemorySize);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.checkBox27);
            this.groupBox2.Controls.Add(this.checkBox6);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.textBoxTotalPasses);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numericUpDown3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Location = new System.Drawing.Point(10, 623);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox2.Size = new System.Drawing.Size(468, 157);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Peeling";
            // 
            // groupBox28
            // 
            this.groupBox28.Controls.Add(this.label61);
            this.groupBox28.Controls.Add(this.numericUpDown35);
            this.groupBox28.Controls.Add(this.label60);
            this.groupBox28.Controls.Add(this.label59);
            this.groupBox28.Controls.Add(this.numericUpDown36);
            this.groupBox28.Controls.Add(this.textBoxKPercentage);
            this.groupBox28.Controls.Add(this.numericUpDown34);
            this.groupBox28.Controls.Add(this.textBoxKValue);
            this.groupBox28.Controls.Add(this.checkBox41);
            this.groupBox28.Location = new System.Drawing.Point(223, 75);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Size = new System.Drawing.Size(236, 71);
            this.groupBox28.TabIndex = 22;
            this.groupBox28.TabStop = false;
            this.groupBox28.Text = "Dynamic K";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(116, 22);
            this.label61.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(33, 13);
            this.label61.TabIndex = 73;
            this.label61.Text = "maxK";
            // 
            // numericUpDown35
            // 
            this.numericUpDown35.Location = new System.Drawing.Point(71, 41);
            this.numericUpDown35.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown35.Name = "numericUpDown35";
            this.numericUpDown35.Size = new System.Drawing.Size(42, 20);
            this.numericUpDown35.TabIndex = 69;
            this.numericUpDown35.ThousandsSeparator = true;
            this.numericUpDown35.ValueChanged += new System.EventHandler(this.numericUpDown35_ValueChanged);
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(115, 45);
            this.label60.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(40, 13);
            this.label60.TabIndex = 72;
            this.label60.Text = "% Error";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(210, 20);
            this.label59.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(23, 13);
            this.label59.TabIndex = 23;
            this.label59.Text = "MB";
            // 
            // numericUpDown36
            // 
            this.numericUpDown36.Location = new System.Drawing.Point(157, 18);
            this.numericUpDown36.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown36.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown36.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown36.Name = "numericUpDown36";
            this.numericUpDown36.Size = new System.Drawing.Size(46, 20);
            this.numericUpDown36.TabIndex = 71;
            this.numericUpDown36.ThousandsSeparator = true;
            this.numericUpDown36.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown36.ValueChanged += new System.EventHandler(this.numericUpDown36_ValueChanged);
            // 
            // textBoxKPercentage
            // 
            this.textBoxKPercentage.Location = new System.Drawing.Point(9, 43);
            this.textBoxKPercentage.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxKPercentage.Name = "textBoxKPercentage";
            this.textBoxKPercentage.ReadOnly = true;
            this.textBoxKPercentage.Size = new System.Drawing.Size(53, 20);
            this.textBoxKPercentage.TabIndex = 70;
            // 
            // numericUpDown34
            // 
            this.numericUpDown34.Location = new System.Drawing.Point(71, 18);
            this.numericUpDown34.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown34.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDown34.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown34.Name = "numericUpDown34";
            this.numericUpDown34.Size = new System.Drawing.Size(42, 20);
            this.numericUpDown34.TabIndex = 68;
            this.numericUpDown34.ThousandsSeparator = true;
            this.numericUpDown34.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numericUpDown34.ValueChanged += new System.EventHandler(this.numericUpDown34_ValueChanged_1);
            // 
            // textBoxKValue
            // 
            this.textBoxKValue.Location = new System.Drawing.Point(9, 19);
            this.textBoxKValue.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxKValue.Name = "textBoxKValue";
            this.textBoxKValue.ReadOnly = true;
            this.textBoxKValue.Size = new System.Drawing.Size(53, 20);
            this.textBoxKValue.TabIndex = 23;
            // 
            // checkBox41
            // 
            this.checkBox41.AutoSize = true;
            this.checkBox41.Location = new System.Drawing.Point(157, 44);
            this.checkBox41.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox41.Name = "checkBox41";
            this.checkBox41.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox41.Size = new System.Drawing.Size(70, 17);
            this.checkBox41.TabIndex = 22;
            this.checkBox41.Text = "Temporal";
            this.checkBox41.UseVisualStyleBackColor = true;
            this.checkBox41.CheckedChanged += new System.EventHandler(this.checkBox41_CheckedChanged);
            // 
            // checkBox35
            // 
            this.checkBox35.AutoSize = true;
            this.checkBox35.Location = new System.Drawing.Point(126, 128);
            this.checkBox35.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox35.Name = "checkBox35";
            this.checkBox35.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox35.Size = new System.Drawing.Size(78, 17);
            this.checkBox35.TabIndex = 21;
            this.checkBox35.Text = "Coplanarity";
            this.checkBox35.UseVisualStyleBackColor = true;
            this.checkBox35.CheckedChanged += new System.EventHandler(this.checkBox35_CheckedChanged);
            // 
            // checkBox26
            // 
            this.checkBox26.AutoSize = true;
            this.checkBox26.Location = new System.Drawing.Point(158, 99);
            this.checkBox26.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox26.Name = "checkBox26";
            this.checkBox26.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox26.Size = new System.Drawing.Size(46, 17);
            this.checkBox26.TabIndex = 18;
            this.checkBox26.Text = "Max";
            this.checkBox26.UseVisualStyleBackColor = true;
            this.checkBox26.CheckedChanged += new System.EventHandler(this.checkBox26_CheckedChanged);
            // 
            // checkBox25
            // 
            this.checkBox25.AutoSize = true;
            this.checkBox25.Location = new System.Drawing.Point(12, 99);
            this.checkBox25.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox25.Name = "checkBox25";
            this.checkBox25.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox25.Size = new System.Drawing.Size(111, 17);
            this.checkBox25.TabIndex = 17;
            this.checkBox25.Text = "Global Time query";
            this.checkBox25.UseVisualStyleBackColor = true;
            this.checkBox25.CheckedChanged += new System.EventHandler(this.checkBox25_CheckedChanged);
            // 
            // textBoxMemorySize
            // 
            this.textBoxMemorySize.Location = new System.Drawing.Point(347, 44);
            this.textBoxMemorySize.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxMemorySize.Name = "textBoxMemorySize";
            this.textBoxMemorySize.ReadOnly = true;
            this.textBoxMemorySize.Size = new System.Drawing.Size(112, 20);
            this.textBoxMemorySize.TabIndex = 16;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(292, 47);
            this.label29.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(30, 13);
            this.label29.TabIndex = 15;
            this.label29.Text = "Size:";
            // 
            // checkBox27
            // 
            this.checkBox27.AutoSize = true;
            this.checkBox27.Location = new System.Drawing.Point(12, 128);
            this.checkBox27.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox27.Name = "checkBox27";
            this.checkBox27.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox27.Size = new System.Drawing.Size(111, 17);
            this.checkBox27.TabIndex = 20;
            this.checkBox27.Text = "Occlusion Peeling";
            this.checkBox27.UseVisualStyleBackColor = true;
            this.checkBox27.CheckedChanged += new System.EventHandler(this.checkBox27_CheckedChanged);
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(153, 74);
            this.checkBox6.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox6.Size = new System.Drawing.Size(51, 17);
            this.checkBox6.TabIndex = 14;
            this.checkBox6.Text = "Back";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(12, 73);
            this.checkBox4.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox4.Size = new System.Drawing.Size(102, 17);
            this.checkBox4.TabIndex = 13;
            this.checkBox4.Text = "Occlusion query";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // textBoxTotalPasses
            // 
            this.textBoxTotalPasses.Location = new System.Drawing.Point(347, 17);
            this.textBoxTotalPasses.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxTotalPasses.Name = "textBoxTotalPasses";
            this.textBoxTotalPasses.ReadOnly = true;
            this.textBoxTotalPasses.Size = new System.Drawing.Size(112, 20);
            this.textBoxTotalPasses.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(292, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Passes:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 44);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Layers:";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(76, 42);
            this.numericUpDown3.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(209, 20);
            this.numericUpDown3.TabIndex = 6;
            this.numericUpDown3.ThousandsSeparator = true;
            this.numericUpDown3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Technique:";
            // 
            // comboBox2
            // 
            this.comboBox2.DisplayMember = "0";
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(76, 16);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox2.MaxDropDownItems = 4;
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(209, 21);
            this.comboBox2.TabIndex = 6;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // groupBox11
            // 
            this.groupBox11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groupBox11.Controls.Add(this.checkBox9);
            this.groupBox11.Controls.Add(this.label20);
            this.groupBox11.Controls.Add(this.comboBox6);
            this.groupBox11.Controls.Add(this.checkBox2);
            this.groupBox11.Location = new System.Drawing.Point(10, 400);
            this.groupBox11.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox11.Size = new System.Drawing.Size(170, 69);
            this.groupBox11.TabIndex = 19;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Transparency";
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Location = new System.Drawing.Point(70, 25);
            this.checkBox9.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox9.Size = new System.Drawing.Size(60, 17);
            this.checkBox9.TabIndex = 18;
            this.checkBox9.Text = "Correct";
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged_1);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 48);
            this.label20.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(37, 13);
            this.label20.TabIndex = 9;
            this.label20.Text = "Mode:";
            // 
            // comboBox6
            // 
            this.comboBox6.DisplayMember = "0";
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Items.AddRange(new object[] {
            "AVERAGE",
            "WEIGHT_SUM"});
            this.comboBox6.Location = new System.Drawing.Point(55, 45);
            this.comboBox6.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox6.MaxDropDownItems = 2;
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(102, 21);
            this.comboBox6.TabIndex = 8;
            this.comboBox6.SelectedIndexChanged += new System.EventHandler(this.comboBox6_SelectedIndexChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(4, 25);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox2.Size = new System.Drawing.Size(59, 17);
            this.checkBox2.TabIndex = 13;
            this.checkBox2.Text = "Enable";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged_1);
            // 
            // button27
            // 
            this.button27.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button27.Location = new System.Drawing.Point(290, 400);
            this.button27.Margin = new System.Windows.Forms.Padding(6);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(186, 69);
            this.button27.TabIndex = 47;
            this.button27.Text = "Performance Chart";
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Click += new System.EventHandler(this.button27_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(6, 6);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(474, 382);
            this.tabControl2.TabIndex = 9;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.treeView_models);
            this.tabPage6.Controls.Add(this.button21);
            this.tabPage6.Controls.Add(this.button20);
            this.tabPage6.Controls.Add(this._progressBar);
            this.tabPage6.Controls.Add(this.button16);
            this.tabPage6.Controls.Add(this.button12);
            this.tabPage6.Controls.Add(this.button15);
            this.tabPage6.Controls.Add(this.button14);
            this.tabPage6.Controls.Add(this.button13);
            this.tabPage6.Controls.Add(this.groupBox15);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(466, 356);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "Components";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // treeView_models
            // 
            this.treeView_models.LabelEdit = true;
            this.treeView_models.Location = new System.Drawing.Point(17, 35);
            this.treeView_models.Margin = new System.Windows.Forms.Padding(6);
            this.treeView_models.Name = "treeView_models";
            this.treeView_models.Size = new System.Drawing.Size(266, 184);
            this.treeView_models.TabIndex = 19;
            this.treeView_models.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_models_AfterSelect);
            // 
            // button21
            // 
            this.button21.BackgroundImage = global::abasilak.Properties.Resources.Pose;
            this.button21.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button21.Location = new System.Drawing.Point(117, 5);
            this.button21.Margin = new System.Windows.Forms.Padding(6);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(36, 28);
            this.button21.TabIndex = 30;
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // button20
            // 
            this.button20.BackgroundImage = global::abasilak.Properties.Resources.successGreenDot;
            this.button20.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button20.Location = new System.Drawing.Point(165, 6);
            this.button20.Margin = new System.Windows.Forms.Padding(6);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(28, 27);
            this.button20.TabIndex = 29;
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button20_Click);
            // 
            // _progressBar
            // 
            this._progressBar.Location = new System.Drawing.Point(15, 224);
            this._progressBar.Margin = new System.Windows.Forms.Padding(6);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(268, 25);
            this._progressBar.TabIndex = 1;
            // 
            // button16
            // 
            this.button16.BackgroundImage = global::abasilak.Properties.Resources.minus;
            this.button16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button16.Location = new System.Drawing.Point(248, 5);
            this.button16.Margin = new System.Windows.Forms.Padding(6);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(32, 27);
            this.button16.TabIndex = 28;
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button12
            // 
            this.button12.BackgroundImage = global::abasilak.Properties.Resources.plus;
            this.button12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button12.Location = new System.Drawing.Point(205, 5);
            this.button12.Margin = new System.Windows.Forms.Padding(6);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(33, 27);
            this.button12.TabIndex = 27;
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button15
            // 
            this.button15.BackgroundImage = global::abasilak.Properties.Resources.StopHS;
            this.button15.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button15.Location = new System.Drawing.Point(82, 6);
            this.button15.Margin = new System.Windows.Forms.Padding(6);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(26, 26);
            this.button15.TabIndex = 26;
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button14
            // 
            this.button14.BackgroundImage = global::abasilak.Properties.Resources.PauseHS;
            this.button14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button14.Location = new System.Drawing.Point(51, 6);
            this.button14.Margin = new System.Windows.Forms.Padding(6);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(25, 26);
            this.button14.TabIndex = 25;
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button13
            // 
            this.button13.BackgroundImage = global::abasilak.Properties.Resources.FormRunHS;
            this.button13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button13.Location = new System.Drawing.Point(20, 6);
            this.button13.Margin = new System.Windows.Forms.Padding(6);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(25, 26);
            this.button13.TabIndex = 24;
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.button11);
            this.groupBox15.Controls.Add(this.checkBox34);
            this.groupBox15.Controls.Add(this.button3);
            this.groupBox15.Controls.Add(this.groups);
            this.groupBox15.Controls.Add(this.checkBox15);
            this.groupBox15.Controls.Add(this.checkBox14);
            this.groupBox15.Controls.Add(this.FileStep);
            this.groupBox15.Controls.Add(this.button10);
            this.groupBox15.Controls.Add(this.button2);
            this.groupBox15.Location = new System.Drawing.Point(15, 251);
            this.groupBox15.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox15.Size = new System.Drawing.Size(270, 92);
            this.groupBox15.TabIndex = 22;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Model";
            // 
            // button11
            // 
            this.button11.Image = global::abasilak.Properties.Resources.delete;
            this.button11.Location = new System.Drawing.Point(208, 64);
            this.button11.Margin = new System.Windows.Forms.Padding(6);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(21, 21);
            this.button11.TabIndex = 21;
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // checkBox34
            // 
            this.checkBox34.AutoSize = true;
            this.checkBox34.Location = new System.Drawing.Point(182, 20);
            this.checkBox34.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox34.Name = "checkBox34";
            this.checkBox34.Size = new System.Drawing.Size(78, 17);
            this.checkBox34.TabIndex = 26;
            this.checkBox34.Text = "mean pose";
            this.checkBox34.UseVisualStyleBackColor = true;
            this.checkBox34.CheckedChanged += new System.EventHandler(this.checkBox34_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Image = global::abasilak.Properties.Resources.AddFShader;
            this.button3.Location = new System.Drawing.Point(182, 64);
            this.button3.Margin = new System.Windows.Forms.Padding(6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(20, 21);
            this.button3.TabIndex = 21;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.buttonAddTexture_Click);
            // 
            // groups
            // 
            this.groups.AutoSize = true;
            this.groups.Location = new System.Drawing.Point(182, 45);
            this.groups.Margin = new System.Windows.Forms.Padding(6);
            this.groups.Name = "groups";
            this.groups.Size = new System.Drawing.Size(69, 17);
            this.groups.TabIndex = 25;
            this.groups.Text = "in groups";
            this.groups.UseVisualStyleBackColor = true;
            this.groups.CheckedChanged += new System.EventHandler(this.checkBox30_CheckedChanged);
            // 
            // checkBox15
            // 
            this.checkBox15.AutoSize = true;
            this.checkBox15.Location = new System.Drawing.Point(103, 45);
            this.checkBox15.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Size = new System.Drawing.Size(74, 17);
            this.checkBox15.TabIndex = 24;
            this.checkBox15.Text = "re-normals";
            this.checkBox15.UseVisualStyleBackColor = true;
            this.checkBox15.CheckedChanged += new System.EventHandler(this.checkBox15_CheckedChanged);
            // 
            // checkBox14
            // 
            this.checkBox14.AutoSize = true;
            this.checkBox14.Location = new System.Drawing.Point(103, 20);
            this.checkBox14.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox14.Name = "checkBox14";
            this.checkBox14.Size = new System.Drawing.Size(48, 17);
            this.checkBox14.TabIndex = 23;
            this.checkBox14.Text = "-X90";
            this.checkBox14.UseVisualStyleBackColor = true;
            this.checkBox14.CheckedChanged += new System.EventHandler(this.checkBox14_CheckedChanged);
            // 
            // FileStep
            // 
            this.FileStep.Location = new System.Drawing.Point(11, 64);
            this.FileStep.Margin = new System.Windows.Forms.Padding(6);
            this.FileStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FileStep.Name = "FileStep";
            this.FileStep.Size = new System.Drawing.Size(80, 20);
            this.FileStep.TabIndex = 22;
            this.FileStep.ThousandsSeparator = true;
            this.FileStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FileStep.ValueChanged += new System.EventHandler(this.FileStep_ValueChanged);
            // 
            // button10
            // 
            this.button10.Image = global::abasilak.Properties.Resources.delete;
            this.button10.Location = new System.Drawing.Point(53, 20);
            this.button10.Margin = new System.Windows.Forms.Padding(6);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(38, 42);
            this.button10.TabIndex = 20;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button2
            // 
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button2.Image = global::abasilak.Properties.Resources.AddGLSLProg;
            this.button2.Location = new System.Drawing.Point(9, 20);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(37, 42);
            this.button2.TabIndex = 20;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonAddModel_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox_Camera);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage3.Size = new System.Drawing.Size(466, 356);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Camera";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button18);
            this.tabPage4.Controls.Add(this.checkBox20);
            this.tabPage4.Controls.Add(this.groupBox6);
            this.tabPage4.Controls.Add(this.groupBox5);
            this.tabPage4.Controls.Add(this.checkBox_lightSpot);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage4.Size = new System.Drawing.Size(466, 356);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Light";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(157, 8);
            this.button18.Margin = new System.Windows.Forms.Padding(6);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(124, 23);
            this.button18.TabIndex = 12;
            this.button18.Text = "Center";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // checkBox20
            // 
            this.checkBox20.AutoSize = true;
            this.checkBox20.Location = new System.Drawing.Point(72, 12);
            this.checkBox20.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox20.Name = "checkBox20";
            this.checkBox20.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox20.Size = new System.Drawing.Size(73, 17);
            this.checkBox20.TabIndex = 12;
            this.checkBox20.Text = "Transform";
            this.checkBox20.UseVisualStyleBackColor = true;
            this.checkBox20.CheckedChanged += new System.EventHandler(this.checkBox20_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.numericUpDown10);
            this.groupBox6.Controls.Add(this.numericUpDown9);
            this.groupBox6.Controls.Add(this.numericUpDown8);
            this.groupBox6.Controls.Add(this.button6);
            this.groupBox6.Controls.Add(this.button4);
            this.groupBox6.Controls.Add(this.button5);
            this.groupBox6.Location = new System.Drawing.Point(10, 41);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox6.Size = new System.Drawing.Size(408, 113);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Color";
            // 
            // numericUpDown10
            // 
            this.numericUpDown10.DecimalPlaces = 1;
            this.numericUpDown10.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown10.Location = new System.Drawing.Point(234, 81);
            this.numericUpDown10.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown10.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericUpDown10.Name = "numericUpDown10";
            this.numericUpDown10.Size = new System.Drawing.Size(162, 20);
            this.numericUpDown10.TabIndex = 11;
            this.numericUpDown10.ThousandsSeparator = true;
            this.numericUpDown10.ValueChanged += new System.EventHandler(this.numericUpDown10_ValueChanged);
            // 
            // numericUpDown9
            // 
            this.numericUpDown9.DecimalPlaces = 1;
            this.numericUpDown9.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown9.Location = new System.Drawing.Point(234, 52);
            this.numericUpDown9.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown9.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericUpDown9.Name = "numericUpDown9";
            this.numericUpDown9.Size = new System.Drawing.Size(162, 20);
            this.numericUpDown9.TabIndex = 10;
            this.numericUpDown9.ThousandsSeparator = true;
            this.numericUpDown9.ValueChanged += new System.EventHandler(this.numericUpDown9_ValueChanged);
            // 
            // numericUpDown8
            // 
            this.numericUpDown8.DecimalPlaces = 1;
            this.numericUpDown8.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown8.Location = new System.Drawing.Point(234, 21);
            this.numericUpDown8.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown8.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericUpDown8.Name = "numericUpDown8";
            this.numericUpDown8.Size = new System.Drawing.Size(162, 20);
            this.numericUpDown8.TabIndex = 9;
            this.numericUpDown8.ThousandsSeparator = true;
            this.numericUpDown8.ValueChanged += new System.EventHandler(this.numericUpDown8_ValueChanged);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(7, 52);
            this.button6.Margin = new System.Windows.Forms.Padding(6);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(210, 20);
            this.button6.TabIndex = 5;
            this.button6.Text = "Diffuse";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(7, 21);
            this.button4.Margin = new System.Windows.Forms.Padding(6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(210, 20);
            this.button4.TabIndex = 7;
            this.button4.Text = "Ambient";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(7, 81);
            this.button5.Margin = new System.Windows.Forms.Padding(6);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(210, 23);
            this.button5.TabIndex = 6;
            this.button5.Text = "Specular";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.numericUpDown7);
            this.groupBox5.Controls.Add(this.numericUpDown6);
            this.groupBox5.Controls.Add(this.numericUpDown5);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Location = new System.Drawing.Point(12, 166);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox5.Size = new System.Drawing.Size(406, 53);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Attenuation";
            // 
            // numericUpDown7
            // 
            this.numericUpDown7.DecimalPlaces = 3;
            this.numericUpDown7.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDown7.Location = new System.Drawing.Point(318, 17);
            this.numericUpDown7.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown7.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericUpDown7.Name = "numericUpDown7";
            this.numericUpDown7.Size = new System.Drawing.Size(76, 20);
            this.numericUpDown7.TabIndex = 8;
            this.numericUpDown7.ThousandsSeparator = true;
            this.numericUpDown7.ValueChanged += new System.EventHandler(this.numericUpDown7_ValueChanged);
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.DecimalPlaces = 2;
            this.numericUpDown6.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown6.Location = new System.Drawing.Point(174, 17);
            this.numericUpDown6.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(73, 20);
            this.numericUpDown6.TabIndex = 7;
            this.numericUpDown6.ThousandsSeparator = true;
            this.numericUpDown6.ValueChanged += new System.EventHandler(this.numericUpDown6_ValueChanged);
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.DecimalPlaces = 1;
            this.numericUpDown5.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown5.Location = new System.Drawing.Point(64, 17);
            this.numericUpDown5.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(56, 20);
            this.numericUpDown5.TabIndex = 6;
            this.numericUpDown5.ThousandsSeparator = true;
            this.numericUpDown5.ValueChanged += new System.EventHandler(this.numericUpDown5_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(255, 19);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Quadratic:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(123, 19);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Linear:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 19);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Constant:";
            // 
            // checkBox_lightSpot
            // 
            this.checkBox_lightSpot.AutoSize = true;
            this.checkBox_lightSpot.Location = new System.Drawing.Point(12, 12);
            this.checkBox_lightSpot.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox_lightSpot.Name = "checkBox_lightSpot";
            this.checkBox_lightSpot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox_lightSpot.Size = new System.Drawing.Size(48, 17);
            this.checkBox_lightSpot.TabIndex = 9;
            this.checkBox_lightSpot.Text = "Spot";
            this.checkBox_lightSpot.UseVisualStyleBackColor = true;
            this.checkBox_lightSpot.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox13);
            this.tabPage5.Controls.Add(this.Tesselation);
            this.tabPage5.Controls.Add(this.groupBox26);
            this.tabPage5.Controls.Add(this.groupBox22);
            this.tabPage5.Controls.Add(this.groupBox17);
            this.tabPage5.Controls.Add(this.groupBox16);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(466, 356);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Render";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.BackColor = System.Drawing.Color.PeachPuff;
            this.groupBox13.Controls.Add(this.checkBox38);
            this.groupBox13.Controls.Add(this.label58);
            this.groupBox13.Controls.Add(this.numericUpDown32);
            this.groupBox13.Controls.Add(this.numericUpDownInstancesCount);
            this.groupBox13.Controls.Add(this.numericUpDown31);
            this.groupBox13.Controls.Add(this.numericUpDown15);
            this.groupBox13.Controls.Add(this.numericUpDown17);
            this.groupBox13.Controls.Add(this.checkBox37);
            this.groupBox13.Location = new System.Drawing.Point(6, 121);
            this.groupBox13.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox13.Size = new System.Drawing.Size(446, 121);
            this.groupBox13.TabIndex = 26;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Instancing";
            // 
            // checkBox38
            // 
            this.checkBox38.AutoSize = true;
            this.checkBox38.Location = new System.Drawing.Point(76, 25);
            this.checkBox38.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox38.Name = "checkBox38";
            this.checkBox38.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox38.Size = new System.Drawing.Size(66, 17);
            this.checkBox38.TabIndex = 32;
            this.checkBox38.Text = "Random";
            this.checkBox38.UseVisualStyleBackColor = true;
            this.checkBox38.CheckedChanged += new System.EventHandler(this.checkBox38_CheckedChanged);
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(3, 86);
            this.label58.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(57, 13);
            this.label58.TabIndex = 31;
            this.label58.Text = "Discard %:";
            // 
            // numericUpDown32
            // 
            this.numericUpDown32.DecimalPlaces = 2;
            this.numericUpDown32.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown32.Location = new System.Drawing.Point(76, 84);
            this.numericUpDown32.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown32.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown32.Name = "numericUpDown32";
            this.numericUpDown32.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown32.TabIndex = 15;
            this.numericUpDown32.ThousandsSeparator = true;
            this.numericUpDown32.ValueChanged += new System.EventHandler(this.numericUpDown32_ValueChanged);
            // 
            // numericUpDownInstancesCount
            // 
            this.numericUpDownInstancesCount.Location = new System.Drawing.Point(6, 54);
            this.numericUpDownInstancesCount.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownInstancesCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownInstancesCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownInstancesCount.Name = "numericUpDownInstancesCount";
            this.numericUpDownInstancesCount.Size = new System.Drawing.Size(132, 20);
            this.numericUpDownInstancesCount.TabIndex = 21;
            this.numericUpDownInstancesCount.ThousandsSeparator = true;
            this.numericUpDownInstancesCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownInstancesCount.ValueChanged += new System.EventHandler(this.numericUpDownInstancesCount_ValueChanged);
            // 
            // numericUpDown31
            // 
            this.numericUpDown31.DecimalPlaces = 5;
            this.numericUpDown31.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown31.Location = new System.Drawing.Point(154, 86);
            this.numericUpDown31.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown31.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown31.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown31.Name = "numericUpDown31";
            this.numericUpDown31.Size = new System.Drawing.Size(188, 20);
            this.numericUpDown31.TabIndex = 21;
            this.numericUpDown31.ThousandsSeparator = true;
            this.numericUpDown31.ValueChanged += new System.EventHandler(this.numericUpDown31_ValueChanged);
            // 
            // numericUpDown15
            // 
            this.numericUpDown15.DecimalPlaces = 5;
            this.numericUpDown15.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown15.Location = new System.Drawing.Point(154, 54);
            this.numericUpDown15.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown15.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown15.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown15.Name = "numericUpDown15";
            this.numericUpDown15.Size = new System.Drawing.Size(188, 20);
            this.numericUpDown15.TabIndex = 20;
            this.numericUpDown15.ThousandsSeparator = true;
            this.numericUpDown15.ValueChanged += new System.EventHandler(this.numericUpDown15_ValueChanged_1);
            // 
            // numericUpDown17
            // 
            this.numericUpDown17.DecimalPlaces = 5;
            this.numericUpDown17.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown17.Location = new System.Drawing.Point(154, 22);
            this.numericUpDown17.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown17.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown17.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown17.Name = "numericUpDown17";
            this.numericUpDown17.Size = new System.Drawing.Size(190, 20);
            this.numericUpDown17.TabIndex = 20;
            this.numericUpDown17.ThousandsSeparator = true;
            this.numericUpDown17.ValueChanged += new System.EventHandler(this.numericUpDown17_ValueChanged_1);
            // 
            // checkBox37
            // 
            this.checkBox37.AutoSize = true;
            this.checkBox37.Checked = true;
            this.checkBox37.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox37.Location = new System.Drawing.Point(5, 25);
            this.checkBox37.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox37.Name = "checkBox37";
            this.checkBox37.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox37.Size = new System.Drawing.Size(59, 17);
            this.checkBox37.TabIndex = 20;
            this.checkBox37.Text = "Enable";
            this.checkBox37.UseVisualStyleBackColor = true;
            this.checkBox37.CheckedChanged += new System.EventHandler(this.checkBoxInstancingEnable_CheckedChanged);
            // 
            // Tesselation
            // 
            this.Tesselation.BackColor = System.Drawing.Color.PeachPuff;
            this.Tesselation.Controls.Add(this.label57);
            this.Tesselation.Controls.Add(this.numericUpDownTessLevelOuter);
            this.Tesselation.Controls.Add(this.numericUpDownTessLevelInner);
            this.Tesselation.Controls.Add(this.checkBoxTessEnable);
            this.Tesselation.Location = new System.Drawing.Point(6, 65);
            this.Tesselation.Margin = new System.Windows.Forms.Padding(6);
            this.Tesselation.Name = "Tesselation";
            this.Tesselation.Padding = new System.Windows.Forms.Padding(6);
            this.Tesselation.Size = new System.Drawing.Size(446, 52);
            this.Tesselation.TabIndex = 25;
            this.Tesselation.TabStop = false;
            this.Tesselation.Text = "Tesselation";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(77, 26);
            this.label57.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(65, 13);
            this.label57.TabIndex = 8;
            this.label57.Text = "Inner/Outer:";
            // 
            // numericUpDownTessLevelOuter
            // 
            this.numericUpDownTessLevelOuter.Location = new System.Drawing.Point(240, 24);
            this.numericUpDownTessLevelOuter.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownTessLevelOuter.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numericUpDownTessLevelOuter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTessLevelOuter.Name = "numericUpDownTessLevelOuter";
            this.numericUpDownTessLevelOuter.Size = new System.Drawing.Size(86, 20);
            this.numericUpDownTessLevelOuter.TabIndex = 20;
            this.numericUpDownTessLevelOuter.ThousandsSeparator = true;
            this.numericUpDownTessLevelOuter.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTessLevelOuter.ValueChanged += new System.EventHandler(this.numericUpDownTessLevelOuter_ValueChanged);
            // 
            // numericUpDownTessLevelInner
            // 
            this.numericUpDownTessLevelInner.Location = new System.Drawing.Point(154, 24);
            this.numericUpDownTessLevelInner.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownTessLevelInner.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numericUpDownTessLevelInner.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTessLevelInner.Name = "numericUpDownTessLevelInner";
            this.numericUpDownTessLevelInner.Size = new System.Drawing.Size(84, 20);
            this.numericUpDownTessLevelInner.TabIndex = 20;
            this.numericUpDownTessLevelInner.ThousandsSeparator = true;
            this.numericUpDownTessLevelInner.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTessLevelInner.ValueChanged += new System.EventHandler(this.numericUpDownTessLevelInner_ValueChanged);
            // 
            // checkBoxTessEnable
            // 
            this.checkBoxTessEnable.AutoSize = true;
            this.checkBoxTessEnable.Checked = true;
            this.checkBoxTessEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTessEnable.Location = new System.Drawing.Point(6, 25);
            this.checkBoxTessEnable.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxTessEnable.Name = "checkBoxTessEnable";
            this.checkBoxTessEnable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxTessEnable.Size = new System.Drawing.Size(59, 17);
            this.checkBoxTessEnable.TabIndex = 20;
            this.checkBoxTessEnable.Text = "Enable";
            this.checkBoxTessEnable.UseVisualStyleBackColor = true;
            this.checkBoxTessEnable.CheckedChanged += new System.EventHandler(this.checkBoxTessEnable_CheckedChanged);
            // 
            // groupBox26
            // 
            this.groupBox26.Controls.Add(this.label42);
            this.groupBox26.Controls.Add(this.textBox11);
            this.groupBox26.Controls.Add(this.textBox10);
            this.groupBox26.Controls.Add(this.label41);
            this.groupBox26.Controls.Add(this.checkBox28);
            this.groupBox26.Controls.Add(this.textBox9);
            this.groupBox26.Controls.Add(this.label40);
            this.groupBox26.Location = new System.Drawing.Point(10, 575);
            this.groupBox26.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox26.Size = new System.Drawing.Size(438, 275);
            this.groupBox26.TabIndex = 20;
            this.groupBox26.TabStop = false;
            this.groupBox26.Text = "Peeling Error";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(12, 213);
            this.label42.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(32, 13);
            this.label42.TabIndex = 26;
            this.label42.Text = "Error:";
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(130, 208);
            this.textBox11.Margin = new System.Windows.Forms.Padding(6);
            this.textBox11.Name = "textBox11";
            this.textBox11.ReadOnly = true;
            this.textBox11.Size = new System.Drawing.Size(286, 20);
            this.textBox11.TabIndex = 25;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(128, 144);
            this.textBox10.Margin = new System.Windows.Forms.Padding(6);
            this.textBox10.Name = "textBox10";
            this.textBox10.ReadOnly = true;
            this.textBox10.Size = new System.Drawing.Size(286, 20);
            this.textBox10.TabIndex = 24;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(16, 150);
            this.label41.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(34, 13);
            this.label41.TabIndex = 23;
            this.label41.Text = "Total:";
            // 
            // checkBox28
            // 
            this.checkBox28.AutoSize = true;
            this.checkBox28.Location = new System.Drawing.Point(12, 37);
            this.checkBox28.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox28.Name = "checkBox28";
            this.checkBox28.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox28.Size = new System.Drawing.Size(59, 17);
            this.checkBox28.TabIndex = 19;
            this.checkBox28.Text = "Enable";
            this.checkBox28.UseVisualStyleBackColor = true;
            this.checkBox28.CheckedChanged += new System.EventHandler(this.checkBox28_CheckedChanged);
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(124, 81);
            this.textBox9.Margin = new System.Windows.Forms.Padding(6);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(286, 20);
            this.textBox9.TabIndex = 22;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(12, 87);
            this.label40.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(50, 13);
            this.label40.TabIndex = 21;
            this.label40.Text = "Samples:";
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.label56);
            this.groupBox22.Controls.Add(this.numericUpDownMaxFPS);
            this.groupBox22.Controls.Add(this.checkBox13);
            this.groupBox22.Location = new System.Drawing.Point(6, 6);
            this.groupBox22.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox22.Size = new System.Drawing.Size(165, 48);
            this.groupBox22.TabIndex = 19;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "FPS";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(7, 19);
            this.label56.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(30, 13);
            this.label56.TabIndex = 68;
            this.label56.Text = "Max:";
            // 
            // numericUpDownMaxFPS
            // 
            this.numericUpDownMaxFPS.Location = new System.Drawing.Point(38, 17);
            this.numericUpDownMaxFPS.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownMaxFPS.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownMaxFPS.Name = "numericUpDownMaxFPS";
            this.numericUpDownMaxFPS.Size = new System.Drawing.Size(53, 20);
            this.numericUpDownMaxFPS.TabIndex = 67;
            this.numericUpDownMaxFPS.ThousandsSeparator = true;
            this.numericUpDownMaxFPS.ValueChanged += new System.EventHandler(this.numericUpDownMaxFPS_ValueChanged);
            // 
            // checkBox13
            // 
            this.checkBox13.AutoSize = true;
            this.checkBox13.Location = new System.Drawing.Point(98, 19);
            this.checkBox13.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox13.Name = "checkBox13";
            this.checkBox13.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox13.Size = new System.Drawing.Size(60, 17);
            this.checkBox13.TabIndex = 18;
            this.checkBox13.Text = "V-Sync";
            this.checkBox13.UseVisualStyleBackColor = true;
            this.checkBox13.CheckedChanged += new System.EventHandler(this.checkBox13_CheckedChanged);
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.button_backGrColor);
            this.groupBox17.Controls.Add(this.button_backGrTex);
            this.groupBox17.Location = new System.Drawing.Point(303, 6);
            this.groupBox17.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox17.Size = new System.Drawing.Size(111, 48);
            this.groupBox17.TabIndex = 17;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Set Background";
            // 
            // button_backGrColor
            // 
            this.button_backGrColor.Image = global::abasilak.Properties.Resources.MeshPrimary;
            this.button_backGrColor.Location = new System.Drawing.Point(12, 16);
            this.button_backGrColor.Margin = new System.Windows.Forms.Padding(6);
            this.button_backGrColor.Name = "button_backGrColor";
            this.button_backGrColor.Size = new System.Drawing.Size(30, 31);
            this.button_backGrColor.TabIndex = 8;
            this.button_backGrColor.UseVisualStyleBackColor = true;
            this.button_backGrColor.Click += new System.EventHandler(this.button_backGrColor_Click);
            // 
            // button_backGrTex
            // 
            this.button_backGrTex.Image = global::abasilak.Properties.Resources.FShader;
            this.button_backGrTex.Location = new System.Drawing.Point(53, 16);
            this.button_backGrTex.Margin = new System.Windows.Forms.Padding(6);
            this.button_backGrTex.Name = "button_backGrTex";
            this.button_backGrTex.Size = new System.Drawing.Size(30, 31);
            this.button_backGrTex.TabIndex = 9;
            this.button_backGrTex.UseVisualStyleBackColor = true;
            this.button_backGrTex.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.numericUpDown_gamma);
            this.groupBox16.Location = new System.Drawing.Point(178, 6);
            this.groupBox16.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox16.Size = new System.Drawing.Size(113, 48);
            this.groupBox16.TabIndex = 16;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Gamma Correction";
            // 
            // numericUpDown_gamma
            // 
            this.numericUpDown_gamma.DecimalPlaces = 2;
            this.numericUpDown_gamma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_gamma.Location = new System.Drawing.Point(5, 18);
            this.numericUpDown_gamma.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown_gamma.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_gamma.Name = "numericUpDown_gamma";
            this.numericUpDown_gamma.Size = new System.Drawing.Size(97, 20);
            this.numericUpDown_gamma.TabIndex = 14;
            this.numericUpDown_gamma.ThousandsSeparator = true;
            this.numericUpDown_gamma.ValueChanged += new System.EventHandler(this.numericUpDown_gamma_ValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox40);
            this.tabPage2.Controls.Add(this.checkBox21);
            this.tabPage2.Controls.Add(this.numericUpDown18);
            this.tabPage2.Controls.Add(this.groupBox14);
            this.tabPage2.Controls.Add(this.label27);
            this.tabPage2.Controls.Add(this.label26);
            this.tabPage2.Controls.Add(this.checkBox7);
            this.tabPage2.Controls.Add(this.comboBox7);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.checkBox3);
            this.tabPage2.Controls.Add(this.groupBox9);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(494, 867);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Model";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox40
            // 
            this.checkBox40.AutoSize = true;
            this.checkBox40.Location = new System.Drawing.Point(136, 8);
            this.checkBox40.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox40.Name = "checkBox40";
            this.checkBox40.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox40.Size = new System.Drawing.Size(62, 17);
            this.checkBox40.TabIndex = 24;
            this.checkBox40.Text = "Convex";
            this.checkBox40.UseVisualStyleBackColor = true;
            this.checkBox40.CheckedChanged += new System.EventHandler(this.checkBox40_CheckedChanged);
            // 
            // checkBox21
            // 
            this.checkBox21.AutoSize = true;
            this.checkBox21.Location = new System.Drawing.Point(210, 8);
            this.checkBox21.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox21.Name = "checkBox21";
            this.checkBox21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox21.Size = new System.Drawing.Size(48, 17);
            this.checkBox21.TabIndex = 23;
            this.checkBox21.Text = "Wire";
            this.checkBox21.UseVisualStyleBackColor = true;
            this.checkBox21.CheckedChanged += new System.EventHandler(this.checkBox21_CheckedChanged);
            // 
            // numericUpDown18
            // 
            this.numericUpDown18.DecimalPlaces = 2;
            this.numericUpDown18.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown18.Location = new System.Drawing.Point(87, 64);
            this.numericUpDown18.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown18.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown18.Name = "numericUpDown18";
            this.numericUpDown18.Size = new System.Drawing.Size(310, 20);
            this.numericUpDown18.TabIndex = 22;
            this.numericUpDown18.ThousandsSeparator = true;
            this.numericUpDown18.ValueChanged += new System.EventHandler(this.numericUpDown18_ValueChanged);
            // 
            // groupBox14
            // 
            this.groupBox14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.groupBox14.Controls.Add(this.comboBox8);
            this.groupBox14.Controls.Add(this.comboBox9);
            this.groupBox14.Controls.Add(this.label30);
            this.groupBox14.Controls.Add(this.label31);
            this.groupBox14.Location = new System.Drawing.Point(10, 1146);
            this.groupBox14.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox14.Size = new System.Drawing.Size(462, 123);
            this.groupBox14.TabIndex = 21;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Texturing:";
            // 
            // comboBox8
            // 
            this.comboBox8.DisplayMember = "0";
            this.comboBox8.FormattingEnabled = true;
            this.comboBox8.Items.AddRange(new object[] {
            "REPLACE",
            "MODULATE",
            "DECAL",
            "BLEND",
            "ADD"});
            this.comboBox8.Location = new System.Drawing.Point(140, 69);
            this.comboBox8.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox8.MaxDropDownItems = 4;
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Size = new System.Drawing.Size(308, 21);
            this.comboBox8.TabIndex = 6;
            this.comboBox8.SelectedIndexChanged += new System.EventHandler(this.comboBox8_SelectedIndexChanged);
            // 
            // comboBox9
            // 
            this.comboBox9.DisplayMember = "0";
            this.comboBox9.FormattingEnabled = true;
            this.comboBox9.Items.AddRange(new object[] {
            "OBJECT_PLANE",
            "EYE_PLANE",
            "SPHERICAL",
            "REFLECTIVE",
            "NORMAL"});
            this.comboBox9.Location = new System.Drawing.Point(140, 27);
            this.comboBox9.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox9.MaxDropDownItems = 4;
            this.comboBox9.Name = "comboBox9";
            this.comboBox9.Size = new System.Drawing.Size(308, 21);
            this.comboBox9.TabIndex = 3;
            this.comboBox9.SelectedIndexChanged += new System.EventHandler(this.comboBox9_SelectedIndexChanged);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(4, 75);
            this.label30.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(62, 13);
            this.label30.TabIndex = 7;
            this.label30.Text = "Application:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(6, 33);
            this.label31.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(58, 13);
            this.label31.TabIndex = 4;
            this.label31.Text = "Parameter:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(16, 58);
            this.label27.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(53, 13);
            this.label27.TabIndex = 19;
            this.label27.Text = "Tr Factor:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(17, 31);
            this.label26.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(37, 13);
            this.label26.TabIndex = 19;
            this.label26.Text = "Mode:";
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(70, 8);
            this.checkBox7.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox7.Size = new System.Drawing.Size(54, 17);
            this.checkBox7.TabIndex = 9;
            this.checkBox7.Text = "AABB";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // comboBox7
            // 
            this.comboBox7.DisplayMember = "0";
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Items.AddRange(new object[] {
            "TRIANGLES",
            "POINTS"});
            this.comboBox7.Location = new System.Drawing.Point(87, 31);
            this.comboBox7.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox7.MaxDropDownItems = 2;
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(310, 21);
            this.comboBox7.TabIndex = 18;
            this.comboBox7.SelectedIndexChanged += new System.EventHandler(this.comboBox7_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.groupBox4.Controls.Add(this.comboBox4);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Location = new System.Drawing.Point(6, 283);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox4.Size = new System.Drawing.Size(407, 60);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Modes:";
            // 
            // comboBox4
            // 
            this.comboBox4.DisplayMember = "0";
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "None",
            "Vertex",
            "Facet",
            "Material",
            "Strips",
            "Texture",
            "Normal",
            "Depth",
            "Luminance",
            "Toon",
            "Xray",
            "Gooch",
            "Tex Coords"});
            this.comboBox4.Location = new System.Drawing.Point(81, 25);
            this.comboBox4.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox4.MaxDropDownItems = 4;
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(308, 21);
            this.comboBox4.TabIndex = 3;
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 19);
            this.label12.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Coloring:";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(8, 8);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox3.Size = new System.Drawing.Size(51, 17);
            this.checkBox3.TabIndex = 5;
            this.checkBox3.Text = "Draw";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged_1);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.textBox7);
            this.groupBox9.Controls.Add(this.label25);
            this.groupBox9.Controls.Add(this.textBox6);
            this.groupBox9.Controls.Add(this.label24);
            this.groupBox9.Controls.Add(this.textBox5);
            this.groupBox9.Controls.Add(this.label23);
            this.groupBox9.Controls.Add(this.textBox4);
            this.groupBox9.Controls.Add(this.textBox3);
            this.groupBox9.Controls.Add(this.label22);
            this.groupBox9.Controls.Add(this.label21);
            this.groupBox9.Location = new System.Drawing.Point(8, 96);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox9.Size = new System.Drawing.Size(405, 175);
            this.groupBox9.TabIndex = 5;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Properties";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(79, 144);
            this.textBox7.Margin = new System.Windows.Forms.Padding(6);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(310, 20);
            this.textBox7.TabIndex = 18;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(12, 144);
            this.label25.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(45, 13);
            this.label25.TabIndex = 17;
            this.label25.Text = "Volume:";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(79, 112);
            this.textBox6.Margin = new System.Windows.Forms.Padding(6);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(310, 20);
            this.textBox6.TabIndex = 16;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(9, 111);
            this.label24.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(42, 13);
            this.label24.TabIndex = 15;
            this.label24.Text = "Facets:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(79, 80);
            this.textBox5.Margin = new System.Windows.Forms.Padding(6);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(310, 20);
            this.textBox5.TabIndex = 14;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(9, 77);
            this.label23.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(48, 13);
            this.label23.TabIndex = 13;
            this.label23.Text = "Vertices:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(79, 48);
            this.textBox4.Margin = new System.Windows.Forms.Padding(6);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(310, 20);
            this.textBox4.TabIndex = 12;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(79, 16);
            this.textBox3.Margin = new System.Windows.Forms.Padding(6);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(310, 20);
            this.textBox3.TabIndex = 10;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(9, 48);
            this.label22.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(21, 13);
            this.label22.TabIndex = 11;
            this.label22.Text = "ID:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(9, 19);
            this.label21.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(38, 13);
            this.label21.TabIndex = 9;
            this.label21.Text = "Name:";
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groupBox7.Controls.Add(this.groupBox19);
            this.groupBox7.Controls.Add(this.groupBox12);
            this.groupBox7.Controls.Add(this.groupBox3);
            this.groupBox7.Controls.Add(this.numericUpDown28);
            this.groupBox7.Controls.Add(this.numericUpDown29);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.groupBox8);
            this.groupBox7.Controls.Add(this.groupBox10);
            this.groupBox7.Controls.Add(this.numericUpDown2);
            this.groupBox7.Location = new System.Drawing.Point(6, 355);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox7.Size = new System.Drawing.Size(462, 332);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Material";
            // 
            // groupBox19
            // 
            this.groupBox19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox19.Controls.Add(this.label49);
            this.groupBox19.Controls.Add(this.numericUpDown27);
            this.groupBox19.Controls.Add(this.label19);
            this.groupBox19.Controls.Add(this.numericUpDown4);
            this.groupBox19.Controls.Add(this.checkBox12);
            this.groupBox19.Location = new System.Drawing.Point(8, 265);
            this.groupBox19.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox19.Size = new System.Drawing.Size(446, 51);
            this.groupBox19.TabIndex = 20;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Transparency";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(300, 26);
            this.label49.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(22, 13);
            this.label49.TabIndex = 19;
            this.label49.Text = "eF:";
            // 
            // numericUpDown27
            // 
            this.numericUpDown27.DecimalPlaces = 2;
            this.numericUpDown27.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown27.Location = new System.Drawing.Point(326, 19);
            this.numericUpDown27.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown27.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown27.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown27.Name = "numericUpDown27";
            this.numericUpDown27.Size = new System.Drawing.Size(94, 20);
            this.numericUpDown27.TabIndex = 18;
            this.numericUpDown27.ThousandsSeparator = true;
            this.numericUpDown27.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown27.ValueChanged += new System.EventHandler(this.numericUpDown27_ValueChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(116, 26);
            this.label19.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(36, 13);
            this.label19.TabIndex = 17;
            this.label19.Text = "alpha:";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.DecimalPlaces = 2;
            this.numericUpDown4.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown4.Location = new System.Drawing.Point(175, 19);
            this.numericUpDown4.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(69, 20);
            this.numericUpDown4.TabIndex = 10;
            this.numericUpDown4.ThousandsSeparator = true;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Location = new System.Drawing.Point(45, 25);
            this.checkBox12.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox12.Size = new System.Drawing.Size(59, 17);
            this.checkBox12.TabIndex = 13;
            this.checkBox12.Text = "Enable";
            this.checkBox12.UseVisualStyleBackColor = true;
            this.checkBox12.CheckedChanged += new System.EventHandler(this.checkBox12_CheckedChanged);
            // 
            // groupBox12
            // 
            this.groupBox12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.groupBox12.Controls.Add(this.checkBox8);
            this.groupBox12.Controls.Add(this.label28);
            this.groupBox12.Controls.Add(this.button1);
            this.groupBox12.Controls.Add(this.numericUpDown16);
            this.groupBox12.Location = new System.Drawing.Point(8, 134);
            this.groupBox12.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox12.Size = new System.Drawing.Size(352, 52);
            this.groupBox12.TabIndex = 5;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Strips";
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(258, 22);
            this.checkBox8.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox8.Size = new System.Drawing.Size(40, 17);
            this.checkBox8.TabIndex = 18;
            this.checkBox8.Text = "XY";
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(116, 22);
            this.label28.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(30, 13);
            this.label28.TabIndex = 23;
            this.label28.Text = "Size:";
            // 
            // button1
            // 
            this.button1.Image = global::abasilak.Properties.Resources.MeshPrimary;
            this.button1.Location = new System.Drawing.Point(71, 15);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 26);
            this.button1.TabIndex = 8;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numericUpDown16
            // 
            this.numericUpDown16.DecimalPlaces = 2;
            this.numericUpDown16.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown16.Location = new System.Drawing.Point(158, 20);
            this.numericUpDown16.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown16.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown16.Name = "numericUpDown16";
            this.numericUpDown16.Size = new System.Drawing.Size(86, 20);
            this.numericUpDown16.TabIndex = 22;
            this.numericUpDown16.ThousandsSeparator = true;
            this.numericUpDown16.ValueChanged += new System.EventHandler(this.numericUpDown16_ValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groupBox3.Controls.Add(this.checkBox5);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.numericUpDown12);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.numericUpDown11);
            this.groupBox3.Location = new System.Drawing.Point(8, 198);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox3.Size = new System.Drawing.Size(444, 55);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Translucency";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(45, 25);
            this.checkBox5.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox5.Size = new System.Drawing.Size(59, 17);
            this.checkBox5.TabIndex = 19;
            this.checkBox5.Text = "Enable";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(263, 25);
            this.label16.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 13);
            this.label16.TabIndex = 18;
            this.label16.Text = "absorption:";
            // 
            // numericUpDown12
            // 
            this.numericUpDown12.DecimalPlaces = 2;
            this.numericUpDown12.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown12.Location = new System.Drawing.Point(326, 24);
            this.numericUpDown12.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown12.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown12.Name = "numericUpDown12";
            this.numericUpDown12.Size = new System.Drawing.Size(94, 20);
            this.numericUpDown12.TabIndex = 17;
            this.numericUpDown12.ThousandsSeparator = true;
            this.numericUpDown12.ValueChanged += new System.EventHandler(this.numericUpDown12_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(116, 24);
            this.label15.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 13);
            this.label15.TabIndex = 16;
            this.label15.Text = "frensel:";
            // 
            // numericUpDown11
            // 
            this.numericUpDown11.DecimalPlaces = 2;
            this.numericUpDown11.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown11.Location = new System.Drawing.Point(175, 22);
            this.numericUpDown11.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown11.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown11.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.numericUpDown11.Name = "numericUpDown11";
            this.numericUpDown11.Size = new System.Drawing.Size(69, 20);
            this.numericUpDown11.TabIndex = 15;
            this.numericUpDown11.ThousandsSeparator = true;
            this.numericUpDown11.ValueChanged += new System.EventHandler(this.numericUpDown11_ValueChanged);
            // 
            // numericUpDown28
            // 
            this.numericUpDown28.DecimalPlaces = 1;
            this.numericUpDown28.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown28.Location = new System.Drawing.Point(366, 134);
            this.numericUpDown28.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown28.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown28.Name = "numericUpDown28";
            this.numericUpDown28.Size = new System.Drawing.Size(62, 20);
            this.numericUpDown28.TabIndex = 21;
            this.numericUpDown28.ThousandsSeparator = true;
            this.numericUpDown28.ValueChanged += new System.EventHandler(this.numericUpDown28_ValueChanged);
            // 
            // numericUpDown29
            // 
            this.numericUpDown29.DecimalPlaces = 1;
            this.numericUpDown29.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown29.Location = new System.Drawing.Point(366, 166);
            this.numericUpDown29.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown29.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown29.Name = "numericUpDown29";
            this.numericUpDown29.Size = new System.Drawing.Size(62, 20);
            this.numericUpDown29.TabIndex = 22;
            this.numericUpDown29.ThousandsSeparator = true;
            this.numericUpDown29.ValueChanged += new System.EventHandler(this.numericUpDown29_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(214, 104);
            this.label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "shininess:";
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.groupBox8.Controls.Add(this.button7);
            this.groupBox8.Controls.Add(this.button8);
            this.groupBox8.Controls.Add(this.button9);
            this.groupBox8.Location = new System.Drawing.Point(8, 16);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox8.Size = new System.Drawing.Size(194, 114);
            this.groupBox8.TabIndex = 9;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Color";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(6, 39);
            this.button7.Margin = new System.Windows.Forms.Padding(6);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(176, 29);
            this.button7.TabIndex = 5;
            this.button7.Text = "Diffuse";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.button8.Location = new System.Drawing.Point(6, 13);
            this.button8.Margin = new System.Windows.Forms.Padding(6);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(178, 24);
            this.button8.TabIndex = 7;
            this.button8.Text = "Ambient";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(6, 74);
            this.button9.Margin = new System.Windows.Forms.Padding(6);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(176, 27);
            this.button9.TabIndex = 6;
            this.button9.Text = "Specular";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox10.Controls.Add(this.label18);
            this.groupBox10.Controls.Add(this.label13);
            this.groupBox10.Controls.Add(this.numericUpDown14);
            this.groupBox10.Controls.Add(this.numericUpDown13);
            this.groupBox10.Location = new System.Drawing.Point(214, 16);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox10.Size = new System.Drawing.Size(226, 76);
            this.groupBox10.TabIndex = 16;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "gaussian";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(10, 19);
            this.label18.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(16, 13);
            this.label18.TabIndex = 18;
            this.label18.Text = "c:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 42);
            this.label13.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(18, 13);
            this.label13.TabIndex = 15;
            this.label13.Text = "m:";
            // 
            // numericUpDown14
            // 
            this.numericUpDown14.DecimalPlaces = 2;
            this.numericUpDown14.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown14.Location = new System.Drawing.Point(38, 17);
            this.numericUpDown14.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown14.Name = "numericUpDown14";
            this.numericUpDown14.Size = new System.Drawing.Size(176, 20);
            this.numericUpDown14.TabIndex = 17;
            this.numericUpDown14.ThousandsSeparator = true;
            this.numericUpDown14.ValueChanged += new System.EventHandler(this.numericUpDown14_ValueChanged);
            // 
            // numericUpDown13
            // 
            this.numericUpDown13.DecimalPlaces = 2;
            this.numericUpDown13.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown13.Location = new System.Drawing.Point(38, 42);
            this.numericUpDown13.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown13.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown13.Name = "numericUpDown13";
            this.numericUpDown13.Size = new System.Drawing.Size(176, 20);
            this.numericUpDown13.TabIndex = 14;
            this.numericUpDown13.ThousandsSeparator = true;
            this.numericUpDown13.ValueChanged += new System.EventHandler(this.numericUpDown13_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 1;
            this.numericUpDown2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown2.Location = new System.Drawing.Point(274, 102);
            this.numericUpDown2.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(154, 20);
            this.numericUpDown2.TabIndex = 6;
            this.numericUpDown2.ThousandsSeparator = true;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.groupBox23);
            this.tabPage7.Controls.Add(this.progressBar2);
            this.tabPage7.Controls.Add(this.button17);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(494, 867);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "MeshLab";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.checkBox19);
            this.groupBox23.Controls.Add(this.checkBox18);
            this.groupBox23.Controls.Add(this.groupBox24);
            this.groupBox23.Location = new System.Drawing.Point(8, 6);
            this.groupBox23.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox23.Size = new System.Drawing.Size(480, 152);
            this.groupBox23.TabIndex = 3;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "Filters";
            // 
            // checkBox19
            // 
            this.checkBox19.AutoSize = true;
            this.checkBox19.Location = new System.Drawing.Point(12, 54);
            this.checkBox19.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox19.Name = "checkBox19";
            this.checkBox19.Size = new System.Drawing.Size(188, 17);
            this.checkBox19.TabIndex = 3;
            this.checkBox19.Text = "Compute Angle-Weighted Normals";
            this.checkBox19.UseVisualStyleBackColor = true;
            // 
            // checkBox18
            // 
            this.checkBox18.AutoSize = true;
            this.checkBox18.Location = new System.Drawing.Point(12, 25);
            this.checkBox18.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox18.Name = "checkBox18";
            this.checkBox18.Size = new System.Drawing.Size(161, 17);
            this.checkBox18.TabIndex = 2;
            this.checkBox18.Text = "Remove Duplicated Vertices";
            this.checkBox18.UseVisualStyleBackColor = true;
            // 
            // groupBox24
            // 
            this.groupBox24.Controls.Add(this.checkBox17);
            this.groupBox24.Controls.Add(this.checkBox16);
            this.groupBox24.Location = new System.Drawing.Point(12, 83);
            this.groupBox24.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox24.Size = new System.Drawing.Size(139, 53);
            this.groupBox24.TabIndex = 1;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "Save";
            // 
            // checkBox17
            // 
            this.checkBox17.AutoSize = true;
            this.checkBox17.Location = new System.Drawing.Point(76, 25);
            this.checkBox17.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox17.Name = "checkBox17";
            this.checkBox17.Size = new System.Drawing.Size(55, 17);
            this.checkBox17.TabIndex = 1;
            this.checkBox17.Text = "Colors";
            this.checkBox17.UseVisualStyleBackColor = true;
            // 
            // checkBox16
            // 
            this.checkBox16.AutoSize = true;
            this.checkBox16.Location = new System.Drawing.Point(12, 25);
            this.checkBox16.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox16.Name = "checkBox16";
            this.checkBox16.Size = new System.Drawing.Size(64, 17);
            this.checkBox16.TabIndex = 0;
            this.checkBox16.Text = "Normals";
            this.checkBox16.UseVisualStyleBackColor = true;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(8, 199);
            this.progressBar2.Margin = new System.Windows.Forms.Padding(6);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(480, 25);
            this.progressBar2.TabIndex = 2;
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(8, 170);
            this.button17.Margin = new System.Windows.Forms.Padding(6);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(480, 27);
            this.button17.TabIndex = 0;
            this.button17.Text = "Run Meshlab Server";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.meshLabButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1276);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 28, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1663, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // _statusLabel
            // 
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(118, 17);
            this._statusLabel.Text = "toolStripStatusLabel1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setToModelToolStripMenuItem,
            this.setToBackgroundToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(172, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // setToModelToolStripMenuItem
            // 
            this.setToModelToolStripMenuItem.Enabled = false;
            this.setToModelToolStripMenuItem.Name = "setToModelToolStripMenuItem";
            this.setToModelToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.setToModelToolStripMenuItem.Text = "Set to Model";
            this.setToModelToolStripMenuItem.Click += new System.EventHandler(this.setToModelToolStripMenuItem_Click);
            // 
            // setToBackgroundToolStripMenuItem
            // 
            this.setToBackgroundToolStripMenuItem.Name = "setToBackgroundToolStripMenuItem";
            this.setToBackgroundToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.setToBackgroundToolStripMenuItem.Text = "Set to Background";
            this.setToBackgroundToolStripMenuItem.Click += new System.EventHandler(this.setToBackgroundToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CSGtoolStripMenuItem,
            this.loadSMA,
            this.saveToolStripMenuItem,
            this.selectedPoseToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(147, 92);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // CSGtoolStripMenuItem
            // 
            this.CSGtoolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setModelToolStripMenuItem,
            this.setOperationToolStripMenuItem,
            this.enableToolStripMenuItem,
            this.disableToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.CSGtoolStripMenuItem.Enabled = false;
            this.CSGtoolStripMenuItem.Name = "CSGtoolStripMenuItem";
            this.CSGtoolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.CSGtoolStripMenuItem.Text = "CSG";
            // 
            // setModelToolStripMenuItem
            // 
            this.setModelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.setModelToolStripMenuItem.Name = "setModelToolStripMenuItem";
            this.setModelToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.setModelToolStripMenuItem.Text = "Set Model";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(80, 22);
            this.toolStripMenuItem3.Text = "1";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.csgModel1toolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(80, 22);
            this.toolStripMenuItem4.Text = "2";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.csgModel2toolStripMenuItem_Click);
            // 
            // setOperationToolStripMenuItem
            // 
            this.setOperationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unionToolStripMenuItem,
            this.intersectionToolStripMenuItem,
            this.differenceToolStripMenuItem,
            this.noneToolStripMenuItem});
            this.setOperationToolStripMenuItem.Name = "setOperationToolStripMenuItem";
            this.setOperationToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.setOperationToolStripMenuItem.Text = "Set Operation";
            // 
            // unionToolStripMenuItem
            // 
            this.unionToolStripMenuItem.Name = "unionToolStripMenuItem";
            this.unionToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.unionToolStripMenuItem.Text = "Union";
            this.unionToolStripMenuItem.Click += new System.EventHandler(this.unionToolStripMenuItem_Click);
            // 
            // intersectionToolStripMenuItem
            // 
            this.intersectionToolStripMenuItem.Name = "intersectionToolStripMenuItem";
            this.intersectionToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.intersectionToolStripMenuItem.Text = "Intersection";
            this.intersectionToolStripMenuItem.Click += new System.EventHandler(this.intersectionToolStripMenuItem_Click);
            // 
            // differenceToolStripMenuItem
            // 
            this.differenceToolStripMenuItem.Name = "differenceToolStripMenuItem";
            this.differenceToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.differenceToolStripMenuItem.Text = "Difference";
            this.differenceToolStripMenuItem.Click += new System.EventHandler(this.differenceToolStripMenuItem_Click);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.noneToolStripMenuItem.Text = "None";
            this.noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.enableToolStripMenuItem.Text = "Enable";
            this.enableToolStripMenuItem.Click += new System.EventHandler(this.enableToolStripMenuItem_Click);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Enabled = false;
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.disableToolStripMenuItem.Text = "Disable";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // loadSMA
            // 
            this.loadSMA.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.featureToolStripMenuItem1,
            this.sMAToolStripMenuItem3,
            this.sMAToolStripMenuItem4,
            this.rMAToolStripMenuItem});
            this.loadSMA.Name = "loadSMA";
            this.loadSMA.Size = new System.Drawing.Size(146, 22);
            this.loadSMA.Text = "Load";
            // 
            // featureToolStripMenuItem1
            // 
            this.featureToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedPoseToolStripMenuItem4,
            this.allToolStripMenuItem3});
            this.featureToolStripMenuItem1.Name = "featureToolStripMenuItem1";
            this.featureToolStripMenuItem1.Size = new System.Drawing.Size(128, 22);
            this.featureToolStripMenuItem1.Text = "Feature";
            // 
            // selectedPoseToolStripMenuItem4
            // 
            this.selectedPoseToolStripMenuItem4.Name = "selectedPoseToolStripMenuItem4";
            this.selectedPoseToolStripMenuItem4.Size = new System.Drawing.Size(146, 22);
            this.selectedPoseToolStripMenuItem4.Text = "Selected Pose";
            this.selectedPoseToolStripMenuItem4.Click += new System.EventHandler(this.loadPoseDG_Click);
            // 
            // allToolStripMenuItem3
            // 
            this.allToolStripMenuItem3.Name = "allToolStripMenuItem3";
            this.allToolStripMenuItem3.Size = new System.Drawing.Size(146, 22);
            this.allToolStripMenuItem3.Text = "All";
            this.allToolStripMenuItem3.Click += new System.EventHandler(this.loadAllPosesDG_Click);
            // 
            // sMAToolStripMenuItem3
            // 
            this.sMAToolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedPoseToolStripMenuItem2,
            this.allToolStripMenuItem1});
            this.sMAToolStripMenuItem3.Name = "sMAToolStripMenuItem3";
            this.sMAToolStripMenuItem3.Size = new System.Drawing.Size(128, 22);
            this.sMAToolStripMenuItem3.Text = "Clustering";
            // 
            // selectedPoseToolStripMenuItem2
            // 
            this.selectedPoseToolStripMenuItem2.Name = "selectedPoseToolStripMenuItem2";
            this.selectedPoseToolStripMenuItem2.Size = new System.Drawing.Size(143, 22);
            this.selectedPoseToolStripMenuItem2.Text = "SelectedPose";
            this.selectedPoseToolStripMenuItem2.Click += new System.EventHandler(this.loadPoseClustering_Click);
            // 
            // allToolStripMenuItem1
            // 
            this.allToolStripMenuItem1.Name = "allToolStripMenuItem1";
            this.allToolStripMenuItem1.Size = new System.Drawing.Size(143, 22);
            this.allToolStripMenuItem1.Text = "All";
            this.allToolStripMenuItem1.Click += new System.EventHandler(this.loadAllPosesClustering_Click);
            // 
            // sMAToolStripMenuItem4
            // 
            this.sMAToolStripMenuItem4.Name = "sMAToolStripMenuItem4";
            this.sMAToolStripMenuItem4.Size = new System.Drawing.Size(128, 22);
            this.sMAToolStripMenuItem4.Text = "SMA";
            this.sMAToolStripMenuItem4.Click += new System.EventHandler(this.loadSMA_Click);
            // 
            // rMAToolStripMenuItem
            // 
            this.rMAToolStripMenuItem.Name = "rMAToolStripMenuItem";
            this.rMAToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.rMAToolStripMenuItem.Text = "RMA";
            this.rMAToolStripMenuItem.Click += new System.EventHandler(this.rMAToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.featureToolStripMenuItem,
            this.clusteringToolStripMenuItem3,
            this.sMAToolStripMenuItem5});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // featureToolStripMenuItem
            // 
            this.featureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedPoseToolStripMenuItem3,
            this.allToolStripMenuItem2});
            this.featureToolStripMenuItem.Name = "featureToolStripMenuItem";
            this.featureToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.featureToolStripMenuItem.Text = "Feature";
            // 
            // selectedPoseToolStripMenuItem3
            // 
            this.selectedPoseToolStripMenuItem3.Name = "selectedPoseToolStripMenuItem3";
            this.selectedPoseToolStripMenuItem3.Size = new System.Drawing.Size(146, 22);
            this.selectedPoseToolStripMenuItem3.Text = "Selected Pose";
            this.selectedPoseToolStripMenuItem3.Click += new System.EventHandler(this.savePoseDG_Click);
            // 
            // allToolStripMenuItem2
            // 
            this.allToolStripMenuItem2.Name = "allToolStripMenuItem2";
            this.allToolStripMenuItem2.Size = new System.Drawing.Size(146, 22);
            this.allToolStripMenuItem2.Text = "All";
            this.allToolStripMenuItem2.Click += new System.EventHandler(this.saveAllPosesDG_Click);
            // 
            // clusteringToolStripMenuItem3
            // 
            this.clusteringToolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedPoseToolStripMenuItem1,
            this.allToolStripMenuItem});
            this.clusteringToolStripMenuItem3.Name = "clusteringToolStripMenuItem3";
            this.clusteringToolStripMenuItem3.Size = new System.Drawing.Size(128, 22);
            this.clusteringToolStripMenuItem3.Text = "Clustering";
            // 
            // selectedPoseToolStripMenuItem1
            // 
            this.selectedPoseToolStripMenuItem1.Name = "selectedPoseToolStripMenuItem1";
            this.selectedPoseToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.selectedPoseToolStripMenuItem1.Text = "Selected Pose";
            this.selectedPoseToolStripMenuItem1.Click += new System.EventHandler(this.savePoseClustering_Click);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.saveAllPosesClustering_Click);
            // 
            // sMAToolStripMenuItem5
            // 
            this.sMAToolStripMenuItem5.Name = "sMAToolStripMenuItem5";
            this.sMAToolStripMenuItem5.Size = new System.Drawing.Size(128, 22);
            this.sMAToolStripMenuItem5.Text = "SMA";
            // 
            // selectedPoseToolStripMenuItem
            // 
            this.selectedPoseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadEditableModelToolStripMenuItem,
            this.drawToolStripMenuItem,
            this.setAsRestPoseToolStripMenuItem,
            this.propagateColoringModeToolStripMenuItem});
            this.selectedPoseToolStripMenuItem.Name = "selectedPoseToolStripMenuItem";
            this.selectedPoseToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.selectedPoseToolStripMenuItem.Text = "Selected Pose";
            // 
            // loadEditableModelToolStripMenuItem
            // 
            this.loadEditableModelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.loadEditableModelToolStripMenuItem.Name = "loadEditableModelToolStripMenuItem";
            this.loadEditableModelToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.loadEditableModelToolStripMenuItem.Text = "Editable Model";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // drawToolStripMenuItem
            // 
            this.drawToolStripMenuItem.Name = "drawToolStripMenuItem";
            this.drawToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.drawToolStripMenuItem.Text = "Draw";
            this.drawToolStripMenuItem.Click += new System.EventHandler(this.drawToolStripMenuItem_Click);
            // 
            // setAsRestPoseToolStripMenuItem
            // 
            this.setAsRestPoseToolStripMenuItem.Name = "setAsRestPoseToolStripMenuItem";
            this.setAsRestPoseToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.setAsRestPoseToolStripMenuItem.Text = "Set as Rest Pose";
            this.setAsRestPoseToolStripMenuItem.Click += new System.EventHandler(this.setAsRestPoseToolStripMenuItem_Click);
            // 
            // propagateColoringModeToolStripMenuItem
            // 
            this.propagateColoringModeToolStripMenuItem.Name = "propagateColoringModeToolStripMenuItem";
            this.propagateColoringModeToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.propagateColoringModeToolStripMenuItem.Text = "Propagate Coloring Mode";
            this.propagateColoringModeToolStripMenuItem.Click += new System.EventHandler(this.propagateColoringModeToolStripMenuItem_Click);
            // 
            // comboBox18
            // 
            this.comboBox18.DisplayMember = "0";
            this.comboBox18.FormattingEnabled = true;
            this.comboBox18.Items.AddRange(new object[] {
            "WeiLinComInv",
            "MatInv",
            "Recompute"});
            this.comboBox18.Location = new System.Drawing.Point(549, 62);
            this.comboBox18.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox18.MaxDropDownItems = 2;
            this.comboBox18.Name = "comboBox18";
            this.comboBox18.Size = new System.Drawing.Size(118, 21);
            this.comboBox18.TabIndex = 34;
            this.comboBox18.SelectedIndexChanged += new System.EventHandler(this.comboBox18_SelectedIndexChanged);
            // 
            // groupBox34
            // 
            this.groupBox34.Controls.Add(this.checkBox43);
            this.groupBox34.Controls.Add(this.checkBox42);
            this.groupBox34.Controls.Add(this.button37);
            this.groupBox34.Controls.Add(this.button36);
            this.groupBox34.Controls.Add(this.ButtonSmaError);
            this.groupBox34.Controls.Add(this.button30);
            this.groupBox34.Location = new System.Drawing.Point(317, 6);
            this.groupBox34.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox34.Name = "groupBox34";
            this.groupBox34.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox34.Size = new System.Drawing.Size(532, 94);
            this.groupBox34.TabIndex = 6;
            this.groupBox34.TabStop = false;
            this.groupBox34.Text = "Final Positions - Error";
            // 
            // checkBox43
            // 
            this.checkBox43.AutoSize = true;
            this.checkBox43.Checked = true;
            this.checkBox43.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox43.Location = new System.Drawing.Point(337, 63);
            this.checkBox43.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox43.Name = "checkBox43";
            this.checkBox43.Size = new System.Drawing.Size(70, 17);
            this.checkBox43.TabIndex = 53;
            this.checkBox43.Text = "Temporal";
            this.checkBox43.UseVisualStyleBackColor = true;
            this.checkBox43.CheckedChanged += new System.EventHandler(this.checkBox43_CheckedChanged);
            // 
            // checkBox42
            // 
            this.checkBox42.AutoSize = true;
            this.checkBox42.Checked = true;
            this.checkBox42.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox42.Location = new System.Drawing.Point(337, 26);
            this.checkBox42.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox42.Name = "checkBox42";
            this.checkBox42.Size = new System.Drawing.Size(70, 17);
            this.checkBox42.TabIndex = 52;
            this.checkBox42.Text = "HeatMap";
            this.checkBox42.UseVisualStyleBackColor = true;
            this.checkBox42.CheckedChanged += new System.EventHandler(this.checkBox42_CheckedChanged);
            // 
            // button37
            // 
            this.button37.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.button37.Location = new System.Drawing.Point(419, 58);
            this.button37.Margin = new System.Windows.Forms.Padding(6);
            this.button37.Name = "button37";
            this.button37.Size = new System.Drawing.Size(101, 24);
            this.button37.TabIndex = 48;
            this.button37.Text = "Max Color";
            this.button37.UseVisualStyleBackColor = true;
            this.button37.Click += new System.EventHandler(this.button37_Click);
            // 
            // button36
            // 
            this.button36.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.button36.Location = new System.Drawing.Point(419, 25);
            this.button36.Margin = new System.Windows.Forms.Padding(6);
            this.button36.Name = "button36";
            this.button36.Size = new System.Drawing.Size(101, 24);
            this.button36.TabIndex = 47;
            this.button36.Text = "Min Color";
            this.button36.UseVisualStyleBackColor = true;
            this.button36.Click += new System.EventHandler(this.button36_Click);
            // 
            // ButtonSmaError
            // 
            this.ButtonSmaError.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ButtonSmaError.Location = new System.Drawing.Point(146, 31);
            this.ButtonSmaError.Margin = new System.Windows.Forms.Padding(6);
            this.ButtonSmaError.Name = "ButtonSmaError";
            this.ButtonSmaError.Size = new System.Drawing.Size(122, 44);
            this.ButtonSmaError.TabIndex = 43;
            this.ButtonSmaError.Text = "Show";
            this.ButtonSmaError.UseVisualStyleBackColor = true;
            this.ButtonSmaError.Click += new System.EventHandler(this.button26_Click);
            // 
            // button30
            // 
            this.button30.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button30.Location = new System.Drawing.Point(12, 31);
            this.button30.Margin = new System.Windows.Forms.Padding(6);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(122, 44);
            this.button30.TabIndex = 46;
            this.button30.Text = "Calculate";
            this.button30.UseVisualStyleBackColor = true;
            this.button30.Click += new System.EventHandler(this.buttonComputeFinalPos_Click);
            // 
            // groupBox32
            // 
            this.groupBox32.Controls.Add(this.button38);
            this.groupBox32.Controls.Add(this.button35);
            this.groupBox32.Controls.Add(this.button31);
            this.groupBox32.Controls.Add(this.button26);
            this.groupBox32.Controls.Add(this.label64);
            this.groupBox32.Controls.Add(this.label63);
            this.groupBox32.Controls.Add(this.groupBox20);
            this.groupBox32.Controls.Add(this.comboBox18);
            this.groupBox32.Controls.Add(this.comboBox17);
            this.groupBox32.Controls.Add(this.button25);
            this.groupBox32.Location = new System.Drawing.Point(8, 112);
            this.groupBox32.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox32.Name = "groupBox32";
            this.groupBox32.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox32.Size = new System.Drawing.Size(685, 110);
            this.groupBox32.TabIndex = 25;
            this.groupBox32.TabStop = false;
            this.groupBox32.Text = "Fitting";
            // 
            // button38
            // 
            this.button38.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button38.Location = new System.Drawing.Point(7, 74);
            this.button38.Margin = new System.Windows.Forms.Padding(6);
            this.button38.Name = "button38";
            this.button38.Size = new System.Drawing.Size(122, 25);
            this.button38.TabIndex = 75;
            this.button38.Text = "Init Matrices";
            this.button38.UseVisualStyleBackColor = true;
            this.button38.Click += new System.EventHandler(this.button38_Click);
            // 
            // button35
            // 
            this.button35.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button35.Location = new System.Drawing.Point(138, 74);
            this.button35.Margin = new System.Windows.Forms.Padding(6);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(147, 25);
            this.button35.TabIndex = 74;
            this.button35.Text = "Fitting Weights";
            this.button35.UseVisualStyleBackColor = true;
            this.button35.Click += new System.EventHandler(this.button35_Click);
            // 
            // button31
            // 
            this.button31.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button31.Location = new System.Drawing.Point(138, 47);
            this.button31.Margin = new System.Windows.Forms.Padding(6);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(147, 25);
            this.button31.TabIndex = 73;
            this.button31.Text = "Fitting Rest Pose";
            this.button31.UseVisualStyleBackColor = true;
            this.button31.Click += new System.EventHandler(this.button31_Click);
            // 
            // button26
            // 
            this.button26.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button26.Location = new System.Drawing.Point(138, 20);
            this.button26.Margin = new System.Windows.Forms.Padding(6);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(147, 25);
            this.button26.TabIndex = 72;
            this.button26.Text = "Fitting Matrices";
            this.button26.UseVisualStyleBackColor = true;
            this.button26.Click += new System.EventHandler(this.button26_Click_1);
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(500, 67);
            this.label64.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(40, 13);
            this.label64.TabIndex = 71;
            this.label64.Text = "Normal";
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(500, 26);
            this.label63.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(37, 13);
            this.label63.TabIndex = 70;
            this.label63.Text = "Vertex";
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.numericUpDown33);
            this.groupBox20.Controls.Add(this.textBoxFittingIter);
            this.groupBox20.Controls.Add(this.label67);
            this.groupBox20.Controls.Add(this.label66);
            this.groupBox20.Controls.Add(this.label65);
            this.groupBox20.Controls.Add(this.numericUpDownFittingWeightsIter);
            this.groupBox20.Controls.Add(this.numericUpDownFittingRestPoseIter);
            this.groupBox20.Controls.Add(this.numericUpDownFittingMatricesIter);
            this.groupBox20.Controls.Add(this.label62);
            this.groupBox20.Controls.Add(this.numericUpDownFittingIterations);
            this.groupBox20.Controls.Add(this.label48);
            this.groupBox20.Location = new System.Drawing.Point(294, 13);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(197, 91);
            this.groupBox20.TabIndex = 68;
            this.groupBox20.TabStop = false;
            // 
            // numericUpDown33
            // 
            this.numericUpDown33.DecimalPlaces = 7;
            this.numericUpDown33.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDown33.Location = new System.Drawing.Point(105, 65);
            this.numericUpDown33.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown33.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown33.Name = "numericUpDown33";
            this.numericUpDown33.Size = new System.Drawing.Size(83, 20);
            this.numericUpDown33.TabIndex = 44;
            this.numericUpDown33.ThousandsSeparator = true;
            this.numericUpDown33.Value = new decimal(new int[] {
            10,
            0,
            0,
            458752});
            this.numericUpDown33.ValueChanged += new System.EventHandler(this.numericUpDown33_ValueChanged);
            // 
            // textBoxFittingIter
            // 
            this.textBoxFittingIter.Location = new System.Drawing.Point(150, 13);
            this.textBoxFittingIter.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxFittingIter.Name = "textBoxFittingIter";
            this.textBoxFittingIter.ReadOnly = true;
            this.textBoxFittingIter.Size = new System.Drawing.Size(38, 20);
            this.textBoxFittingIter.TabIndex = 63;
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(3, 68);
            this.label67.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(46, 13);
            this.label67.TabIndex = 74;
            this.label67.Text = "Weights";
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(5, 41);
            this.label66.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(37, 13);
            this.label66.TabIndex = 74;
            this.label66.Text = "Vertex";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(5, 15);
            this.label65.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(47, 13);
            this.label65.TabIndex = 73;
            this.label65.Text = "Matrices";
            // 
            // numericUpDownFittingWeightsIter
            // 
            this.numericUpDownFittingWeightsIter.Location = new System.Drawing.Point(52, 66);
            this.numericUpDownFittingWeightsIter.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownFittingWeightsIter.Name = "numericUpDownFittingWeightsIter";
            this.numericUpDownFittingWeightsIter.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownFittingWeightsIter.TabIndex = 72;
            this.numericUpDownFittingWeightsIter.ThousandsSeparator = true;
            this.numericUpDownFittingWeightsIter.ValueChanged += new System.EventHandler(this.numericUpDown39_ValueChanged);
            // 
            // numericUpDownFittingRestPoseIter
            // 
            this.numericUpDownFittingRestPoseIter.Location = new System.Drawing.Point(52, 39);
            this.numericUpDownFittingRestPoseIter.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownFittingRestPoseIter.Name = "numericUpDownFittingRestPoseIter";
            this.numericUpDownFittingRestPoseIter.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownFittingRestPoseIter.TabIndex = 71;
            this.numericUpDownFittingRestPoseIter.ThousandsSeparator = true;
            this.numericUpDownFittingRestPoseIter.ValueChanged += new System.EventHandler(this.numericUpDown38_ValueChanged);
            // 
            // numericUpDownFittingMatricesIter
            // 
            this.numericUpDownFittingMatricesIter.Location = new System.Drawing.Point(52, 12);
            this.numericUpDownFittingMatricesIter.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownFittingMatricesIter.Name = "numericUpDownFittingMatricesIter";
            this.numericUpDownFittingMatricesIter.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownFittingMatricesIter.TabIndex = 70;
            this.numericUpDownFittingMatricesIter.ThousandsSeparator = true;
            this.numericUpDownFittingMatricesIter.ValueChanged += new System.EventHandler(this.numericUpDown37_ValueChanged);
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(102, 16);
            this.label62.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(47, 13);
            this.label62.TabIndex = 69;
            this.label62.Text = "Updates";
            // 
            // numericUpDownFittingIterations
            // 
            this.numericUpDownFittingIterations.Location = new System.Drawing.Point(150, 39);
            this.numericUpDownFittingIterations.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownFittingIterations.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownFittingIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFittingIterations.Name = "numericUpDownFittingIterations";
            this.numericUpDownFittingIterations.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownFittingIterations.TabIndex = 42;
            this.numericUpDownFittingIterations.ThousandsSeparator = true;
            this.numericUpDownFittingIterations.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFittingIterations.ValueChanged += new System.EventHandler(this.numericUpDown34_ValueChanged);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(102, 41);
            this.label48.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(31, 13);
            this.label48.TabIndex = 41;
            this.label48.Text = "Total";
            // 
            // comboBox17
            // 
            this.comboBox17.DisplayMember = "0";
            this.comboBox17.FormattingEnabled = true;
            this.comboBox17.Items.AddRange(new object[] {
            "Rest-Pose",
            "Mean-Pose",
            "P2P-Cor-Cor",
            "P2P-Cor-App",
            "P2P-App-App",
            "P2P-App-App-RPF"});
            this.comboBox17.Location = new System.Drawing.Point(549, 23);
            this.comboBox17.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox17.MaxDropDownItems = 4;
            this.comboBox17.Name = "comboBox17";
            this.comboBox17.Size = new System.Drawing.Size(118, 21);
            this.comboBox17.TabIndex = 26;
            this.comboBox17.SelectedIndexChanged += new System.EventHandler(this.comboBox17_SelectedIndexChanged);
            // 
            // button25
            // 
            this.button25.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button25.Location = new System.Drawing.Point(7, 20);
            this.button25.Margin = new System.Windows.Forms.Padding(6);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(122, 52);
            this.button25.TabIndex = 35;
            this.button25.Text = "Init / Fitting All";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button25_Click);
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.DG);
            this.tabControl4.Controls.Add(this.Clustering);
            this.tabControl4.Controls.Add(this.SMA);
            this.tabControl4.Location = new System.Drawing.Point(8, 847);
            this.tabControl4.Margin = new System.Windows.Forms.Padding(6);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(1500, 429);
            this.tabControl4.TabIndex = 50;
            // 
            // DG
            // 
            this.DG.Controls.Add(this.numericUpDownSelectedVertex);
            this.DG.Controls.Add(this.button28);
            this.DG.Controls.Add(this.numericUpDown30);
            this.DG.Controls.Add(this.checkBox31);
            this.DG.Controls.Add(this.checkBox39);
            this.DG.Controls.Add(this.checkBox30);
            this.DG.Controls.Add(this.comboBoxDGComponentsMode);
            this.DG.Controls.Add(this.comboBoxDGmode);
            this.DG.Controls.Add(this.button22);
            this.DG.Location = new System.Drawing.Point(4, 22);
            this.DG.Margin = new System.Windows.Forms.Padding(6);
            this.DG.Name = "DG";
            this.DG.Padding = new System.Windows.Forms.Padding(6);
            this.DG.Size = new System.Drawing.Size(1492, 403);
            this.DG.TabIndex = 1;
            this.DG.Text = "Deformation";
            this.DG.UseVisualStyleBackColor = true;
            // 
            // numericUpDownSelectedVertex
            // 
            this.numericUpDownSelectedVertex.Location = new System.Drawing.Point(652, 127);
            this.numericUpDownSelectedVertex.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownSelectedVertex.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownSelectedVertex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownSelectedVertex.Name = "numericUpDownSelectedVertex";
            this.numericUpDownSelectedVertex.Size = new System.Drawing.Size(134, 20);
            this.numericUpDownSelectedVertex.TabIndex = 66;
            this.numericUpDownSelectedVertex.ThousandsSeparator = true;
            this.numericUpDownSelectedVertex.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownSelectedVertex.ValueChanged += new System.EventHandler(this.numericUpDownSelectedVertex_ValueChanged);
            // 
            // button28
            // 
            this.button28.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button28.Location = new System.Drawing.Point(12, 127);
            this.button28.Margin = new System.Windows.Forms.Padding(6);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(626, 44);
            this.button28.TabIndex = 65;
            this.button28.Text = "Geodesic Distances";
            this.button28.UseVisualStyleBackColor = true;
            this.button28.Click += new System.EventHandler(this.buttonCalculateGeodesicDistances_Click);
            // 
            // numericUpDown30
            // 
            this.numericUpDown30.Location = new System.Drawing.Point(786, 69);
            this.numericUpDown30.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown30.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown30.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown30.Name = "numericUpDown30";
            this.numericUpDown30.Size = new System.Drawing.Size(134, 20);
            this.numericUpDown30.TabIndex = 64;
            this.numericUpDown30.ThousandsSeparator = true;
            this.numericUpDown30.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown30.ValueChanged += new System.EventHandler(this.numericUpDown30_ValueChanged);
            // 
            // checkBox31
            // 
            this.checkBox31.AutoSize = true;
            this.checkBox31.Location = new System.Drawing.Point(650, 73);
            this.checkBox31.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox31.Name = "checkBox31";
            this.checkBox31.Size = new System.Drawing.Size(69, 17);
            this.checkBox31.TabIndex = 39;
            this.checkBox31.Text = "variability";
            this.checkBox31.UseVisualStyleBackColor = true;
            this.checkBox31.CheckedChanged += new System.EventHandler(this.checkBox31_CheckedChanged);
            // 
            // checkBox39
            // 
            this.checkBox39.AutoSize = true;
            this.checkBox39.Checked = true;
            this.checkBox39.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox39.Location = new System.Drawing.Point(786, 23);
            this.checkBox39.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox39.Name = "checkBox39";
            this.checkBox39.Size = new System.Drawing.Size(70, 17);
            this.checkBox39.TabIndex = 38;
            this.checkBox39.Text = "normalize";
            this.checkBox39.UseVisualStyleBackColor = true;
            this.checkBox39.CheckedChanged += new System.EventHandler(this.checkBoxDgNormalize_CheckedChanged);
            // 
            // checkBox30
            // 
            this.checkBox30.AutoSize = true;
            this.checkBox30.Location = new System.Drawing.Point(650, 23);
            this.checkBox30.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox30.Name = "checkBox30";
            this.checkBox30.Size = new System.Drawing.Size(68, 17);
            this.checkBox30.TabIndex = 37;
            this.checkBox30.Text = "per Pose";
            this.checkBox30.UseVisualStyleBackColor = true;
            this.checkBox30.CheckedChanged += new System.EventHandler(this.checkBoxDGperPose_CheckedChanged);
            // 
            // comboBoxDGComponentsMode
            // 
            this.comboBoxDGComponentsMode.DisplayMember = "0";
            this.comboBoxDGComponentsMode.FormattingEnabled = true;
            this.comboBoxDGComponentsMode.Items.AddRange(new object[] {
            "VELOCITY",
            "ACCELERATION",
            "ROT ANGLE",
            "ROT AXIS",
            "SHEAR",
            "SCALE",
            "DG  FROBENIUS",
            "FACET AREA",
            "ADJ FROBENIUS"});
            this.comboBoxDGComponentsMode.Location = new System.Drawing.Point(296, 19);
            this.comboBoxDGComponentsMode.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxDGComponentsMode.MaxDropDownItems = 2;
            this.comboBoxDGComponentsMode.Name = "comboBoxDGComponentsMode";
            this.comboBoxDGComponentsMode.Size = new System.Drawing.Size(338, 21);
            this.comboBoxDGComponentsMode.TabIndex = 36;
            this.comboBoxDGComponentsMode.SelectedIndexChanged += new System.EventHandler(this.comboBox15_SelectedIndexChanged);
            // 
            // comboBoxDGmode
            // 
            this.comboBoxDGmode.DisplayMember = "0";
            this.comboBoxDGmode.FormattingEnabled = true;
            this.comboBoxDGmode.Items.AddRange(new object[] {
            "REST POSE",
            "MEAN POSE",
            "POSE TO POSE"});
            this.comboBoxDGmode.Location = new System.Drawing.Point(12, 19);
            this.comboBoxDGmode.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxDGmode.MaxDropDownItems = 2;
            this.comboBoxDGmode.Name = "comboBoxDGmode";
            this.comboBoxDGmode.Size = new System.Drawing.Size(268, 21);
            this.comboBoxDGmode.TabIndex = 34;
            this.comboBoxDGmode.SelectedIndexChanged += new System.EventHandler(this.comboBox14_SelectedIndexChanged);
            // 
            // button22
            // 
            this.button22.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button22.Location = new System.Drawing.Point(12, 71);
            this.button22.Margin = new System.Windows.Forms.Padding(6);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(626, 44);
            this.button22.TabIndex = 35;
            this.button22.Text = "Deformation Gradients";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.buttonCalculateDefGradients_Click);
            // 
            // Clustering
            // 
            this.Clustering.Controls.Add(this.button34);
            this.Clustering.Controls.Add(this.checkBox10);
            this.Clustering.Controls.Add(this.checkBoxMergeClusterings);
            this.Clustering.Controls.Add(this.button33);
            this.Clustering.Controls.Add(this.button32);
            this.Clustering.Controls.Add(this.button29);
            this.Clustering.Controls.Add(this.textBoxClustersCount);
            this.Clustering.Controls.Add(this.label46);
            this.Clustering.Controls.Add(this.comboBoxClusteringMode);
            this.Clustering.Controls.Add(this.checkBox32);
            this.Clustering.Controls.Add(this.groupBox29);
            this.Clustering.Controls.Add(this.numericUpDownClustersCount);
            this.Clustering.Controls.Add(this.button23);
            this.Clustering.Controls.Add(this.groupBox27);
            this.Clustering.Location = new System.Drawing.Point(4, 22);
            this.Clustering.Margin = new System.Windows.Forms.Padding(6);
            this.Clustering.Name = "Clustering";
            this.Clustering.Padding = new System.Windows.Forms.Padding(6);
            this.Clustering.Size = new System.Drawing.Size(1492, 403);
            this.Clustering.TabIndex = 0;
            this.Clustering.Text = "Clustering";
            this.Clustering.UseVisualStyleBackColor = true;
            // 
            // button34
            // 
            this.button34.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button34.Location = new System.Drawing.Point(220, 323);
            this.button34.Margin = new System.Windows.Forms.Padding(6);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(188, 44);
            this.button34.TabIndex = 70;
            this.button34.Text = "Propagate";
            this.button34.UseVisualStyleBackColor = true;
            this.button34.Click += new System.EventHandler(this.buttonPropagateClusteringColors_Click);
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(420, 327);
            this.checkBox10.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(52, 17);
            this.checkBox10.TabIndex = 69;
            this.checkBox10.Text = "2-ring";
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBoxTwoRingColoring_CheckedChanged);
            // 
            // checkBoxMergeClusterings
            // 
            this.checkBoxMergeClusterings.AutoSize = true;
            this.checkBoxMergeClusterings.Location = new System.Drawing.Point(728, 327);
            this.checkBoxMergeClusterings.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxMergeClusterings.Name = "checkBoxMergeClusterings";
            this.checkBoxMergeClusterings.Size = new System.Drawing.Size(80, 17);
            this.checkBoxMergeClusterings.TabIndex = 66;
            this.checkBoxMergeClusterings.Text = "incremental";
            this.checkBoxMergeClusterings.UseVisualStyleBackColor = true;
            this.checkBoxMergeClusterings.CheckedChanged += new System.EventHandler(this.checkBoxMerged2MergedClustering_CheckedChanged);
            // 
            // button33
            // 
            this.button33.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button33.Location = new System.Drawing.Point(1136, 323);
            this.button33.Margin = new System.Windows.Forms.Padding(6);
            this.button33.Name = "button33";
            this.button33.Size = new System.Drawing.Size(156, 44);
            this.button33.TabIndex = 66;
            this.button33.Text = "Smooth";
            this.button33.UseVisualStyleBackColor = true;
            this.button33.Click += new System.EventHandler(this.buttonSmoothBoundaries_Click);
            // 
            // button32
            // 
            this.button32.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button32.Location = new System.Drawing.Point(900, 323);
            this.button32.Margin = new System.Windows.Forms.Padding(6);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(158, 44);
            this.button32.TabIndex = 65;
            this.button32.Text = "Clean";
            this.button32.UseVisualStyleBackColor = true;
            this.button32.Click += new System.EventHandler(this.buttonPerformCleaning_Click);
            // 
            // button29
            // 
            this.button29.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button29.Location = new System.Drawing.Point(536, 323);
            this.button29.Margin = new System.Windows.Forms.Padding(6);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(180, 44);
            this.button29.TabIndex = 63;
            this.button29.Text = "Variable";
            this.button29.UseVisualStyleBackColor = true;
            this.button29.Click += new System.EventHandler(this.buttonVariableSegmentation_Click);
            // 
            // textBoxClustersCount
            // 
            this.textBoxClustersCount.Location = new System.Drawing.Point(420, 10);
            this.textBoxClustersCount.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxClustersCount.Name = "textBoxClustersCount";
            this.textBoxClustersCount.ReadOnly = true;
            this.textBoxClustersCount.Size = new System.Drawing.Size(102, 20);
            this.textBoxClustersCount.TabIndex = 62;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(214, 15);
            this.label46.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(32, 13);
            this.label46.TabIndex = 56;
            this.label46.Text = "Num:";
            // 
            // comboBoxClusteringMode
            // 
            this.comboBoxClusteringMode.DisplayMember = "0";
            this.comboBoxClusteringMode.FormattingEnabled = true;
            this.comboBoxClusteringMode.Items.AddRange(new object[] {
            "P-CENTER",
            "K-MEANS",
            "K_RG",
            "MERGE_RG",
            "DIVIDE_CONQUER",
            "K_SPECTRAL",
            "C_PCA"});
            this.comboBoxClusteringMode.Location = new System.Drawing.Point(6, 8);
            this.comboBoxClusteringMode.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxClusteringMode.MaxDropDownItems = 2;
            this.comboBoxClusteringMode.Name = "comboBoxClusteringMode";
            this.comboBoxClusteringMode.Size = new System.Drawing.Size(198, 21);
            this.comboBoxClusteringMode.TabIndex = 54;
            this.comboBoxClusteringMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxClusteringMode_SelectedIndexChanged);
            // 
            // checkBox32
            // 
            this.checkBox32.AutoSize = true;
            this.checkBox32.Location = new System.Drawing.Point(548, 15);
            this.checkBox32.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox32.Name = "checkBox32";
            this.checkBox32.Size = new System.Drawing.Size(68, 17);
            this.checkBox32.TabIndex = 53;
            this.checkBox32.Text = "per Pose";
            this.checkBox32.UseVisualStyleBackColor = true;
            this.checkBox32.CheckedChanged += new System.EventHandler(this.checkBoxClusteringPerPose_CheckedChanged);
            // 
            // groupBox29
            // 
            this.groupBox29.Controls.Add(this.checkBox36);
            this.groupBox29.Controls.Add(this.numericUpDownMergingPrevTolerance);
            this.groupBox29.Controls.Add(this.checkBoxClusteringSetFixedColor);
            this.groupBox29.Controls.Add(this.checkBox33);
            this.groupBox29.Controls.Add(this.checkBoxDrawNeighbors);
            this.groupBox29.Controls.Add(this.checkBoxDrawRegions);
            this.groupBox29.Controls.Add(this.label45);
            this.groupBox29.Controls.Add(this.numericUpDownClusterSelection);
            this.groupBox29.Controls.Add(this.numericUpDownBoneSelection);
            this.groupBox29.Controls.Add(this.checkBoxDrawSpheres);
            this.groupBox29.Controls.Add(this.label43);
            this.groupBox29.Location = new System.Drawing.Point(6, 229);
            this.groupBox29.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox29.Name = "groupBox29";
            this.groupBox29.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox29.Size = new System.Drawing.Size(1286, 87);
            this.groupBox29.TabIndex = 55;
            this.groupBox29.TabStop = false;
            this.groupBox29.Text = "Draw";
            // 
            // checkBox36
            // 
            this.checkBox36.AutoSize = true;
            this.checkBox36.Location = new System.Drawing.Point(712, 33);
            this.checkBox36.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox36.Name = "checkBox36";
            this.checkBox36.Size = new System.Drawing.Size(52, 17);
            this.checkBox36.TabIndex = 68;
            this.checkBox36.Text = "clean";
            this.checkBox36.UseVisualStyleBackColor = true;
            this.checkBox36.CheckedChanged += new System.EventHandler(this.cleaningCheckBox_CheckedChanged);
            // 
            // numericUpDownMergingPrevTolerance
            // 
            this.numericUpDownMergingPrevTolerance.DecimalPlaces = 1;
            this.numericUpDownMergingPrevTolerance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownMergingPrevTolerance.Location = new System.Drawing.Point(820, 31);
            this.numericUpDownMergingPrevTolerance.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownMergingPrevTolerance.Name = "numericUpDownMergingPrevTolerance";
            this.numericUpDownMergingPrevTolerance.Size = new System.Drawing.Size(90, 20);
            this.numericUpDownMergingPrevTolerance.TabIndex = 67;
            this.numericUpDownMergingPrevTolerance.ThousandsSeparator = true;
            this.numericUpDownMergingPrevTolerance.ValueChanged += new System.EventHandler(this.numericUpDownMergingPrevTolerance_ValueChanged);
            // 
            // checkBoxClusteringSetFixedColor
            // 
            this.checkBoxClusteringSetFixedColor.AutoSize = true;
            this.checkBoxClusteringSetFixedColor.Checked = true;
            this.checkBoxClusteringSetFixedColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxClusteringSetFixedColor.Location = new System.Drawing.Point(414, 35);
            this.checkBoxClusteringSetFixedColor.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxClusteringSetFixedColor.Name = "checkBoxClusteringSetFixedColor";
            this.checkBoxClusteringSetFixedColor.Size = new System.Drawing.Size(48, 17);
            this.checkBoxClusteringSetFixedColor.TabIndex = 65;
            this.checkBoxClusteringSetFixedColor.Text = "fixed";
            this.checkBoxClusteringSetFixedColor.UseVisualStyleBackColor = true;
            this.checkBoxClusteringSetFixedColor.CheckedChanged += new System.EventHandler(this.checkBoxClusteringSetFixedColor_CheckedChanged);
            // 
            // checkBox33
            // 
            this.checkBox33.AutoSize = true;
            this.checkBox33.Location = new System.Drawing.Point(530, 33);
            this.checkBox33.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox33.Name = "checkBox33";
            this.checkBox33.Size = new System.Drawing.Size(61, 17);
            this.checkBox33.TabIndex = 64;
            this.checkBox33.Text = "merged";
            this.checkBox33.UseVisualStyleBackColor = true;
            this.checkBox33.CheckedChanged += new System.EventHandler(this.checkBoxClusteringPerPoseMerging_CheckedChanged);
            // 
            // checkBoxDrawNeighbors
            // 
            this.checkBoxDrawNeighbors.AutoSize = true;
            this.checkBoxDrawNeighbors.Location = new System.Drawing.Point(258, 33);
            this.checkBoxDrawNeighbors.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxDrawNeighbors.Name = "checkBoxDrawNeighbors";
            this.checkBoxDrawNeighbors.Size = new System.Drawing.Size(72, 17);
            this.checkBoxDrawNeighbors.TabIndex = 42;
            this.checkBoxDrawNeighbors.Text = "neighbors";
            this.checkBoxDrawNeighbors.UseVisualStyleBackColor = true;
            this.checkBoxDrawNeighbors.CheckedChanged += new System.EventHandler(this.checkBoxDrawNeighbors_CheckedChanged);
            // 
            // checkBoxDrawRegions
            // 
            this.checkBoxDrawRegions.AutoSize = true;
            this.checkBoxDrawRegions.Checked = true;
            this.checkBoxDrawRegions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDrawRegions.Location = new System.Drawing.Point(138, 33);
            this.checkBoxDrawRegions.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxDrawRegions.Name = "checkBoxDrawRegions";
            this.checkBoxDrawRegions.Size = new System.Drawing.Size(60, 17);
            this.checkBoxDrawRegions.TabIndex = 41;
            this.checkBoxDrawRegions.Text = "regions";
            this.checkBoxDrawRegions.UseVisualStyleBackColor = true;
            this.checkBoxDrawRegions.CheckedChanged += new System.EventHandler(this.checkBoxDrawRegions_CheckedChanged);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(918, 35);
            this.label45.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(42, 13);
            this.label45.TabIndex = 40;
            this.label45.Text = "Cluster:";
            // 
            // numericUpDownClusterSelection
            // 
            this.numericUpDownClusterSelection.Location = new System.Drawing.Point(1010, 31);
            this.numericUpDownClusterSelection.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownClusterSelection.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownClusterSelection.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownClusterSelection.Name = "numericUpDownClusterSelection";
            this.numericUpDownClusterSelection.Size = new System.Drawing.Size(92, 20);
            this.numericUpDownClusterSelection.TabIndex = 36;
            this.numericUpDownClusterSelection.ThousandsSeparator = true;
            this.numericUpDownClusterSelection.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownClusterSelection.ValueChanged += new System.EventHandler(this.numericUpDownClusterSelection_ValueChanged);
            // 
            // numericUpDownBoneSelection
            // 
            this.numericUpDownBoneSelection.Location = new System.Drawing.Point(1184, 31);
            this.numericUpDownBoneSelection.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownBoneSelection.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDownBoneSelection.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownBoneSelection.Name = "numericUpDownBoneSelection";
            this.numericUpDownBoneSelection.Size = new System.Drawing.Size(94, 20);
            this.numericUpDownBoneSelection.TabIndex = 23;
            this.numericUpDownBoneSelection.ThousandsSeparator = true;
            this.numericUpDownBoneSelection.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownBoneSelection.ValueChanged += new System.EventHandler(this.numericUpDownBoneSelection_ValueChanged);
            // 
            // checkBoxDrawSpheres
            // 
            this.checkBoxDrawSpheres.AutoSize = true;
            this.checkBoxDrawSpheres.Checked = true;
            this.checkBoxDrawSpheres.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDrawSpheres.Location = new System.Drawing.Point(12, 33);
            this.checkBoxDrawSpheres.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxDrawSpheres.Name = "checkBoxDrawSpheres";
            this.checkBoxDrawSpheres.Size = new System.Drawing.Size(63, 17);
            this.checkBoxDrawSpheres.TabIndex = 34;
            this.checkBoxDrawSpheres.Text = "spheres";
            this.checkBoxDrawSpheres.UseVisualStyleBackColor = true;
            this.checkBoxDrawSpheres.CheckedChanged += new System.EventHandler(this.checkBoxDrawSpheres_CheckedChanged);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(1108, 37);
            this.label43.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(35, 13);
            this.label43.TabIndex = 8;
            this.label43.Text = "Bone:";
            // 
            // numericUpDownClustersCount
            // 
            this.numericUpDownClustersCount.Location = new System.Drawing.Point(288, 10);
            this.numericUpDownClustersCount.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownClustersCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownClustersCount.Name = "numericUpDownClustersCount";
            this.numericUpDownClustersCount.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownClustersCount.TabIndex = 51;
            this.numericUpDownClustersCount.ThousandsSeparator = true;
            this.numericUpDownClustersCount.ValueChanged += new System.EventHandler(this.numericUpDownClusterCount_ValueChanged);
            // 
            // button23
            // 
            this.button23.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button23.Location = new System.Drawing.Point(6, 323);
            this.button23.Margin = new System.Windows.Forms.Padding(6);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(202, 44);
            this.button23.TabIndex = 52;
            this.button23.Text = "Calculate";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.buttonComputeClustering_Click);
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.label55);
            this.groupBox27.Controls.Add(this.numericUpDownBasisVectorsCount);
            this.groupBox27.Controls.Add(this.checkBoxNipals);
            this.groupBox27.Controls.Add(this.label54);
            this.groupBox27.Controls.Add(this.comboBoxDistanceMode);
            this.groupBox27.Controls.Add(this.textBoxClusteringErrorTolerance);
            this.groupBox27.Controls.Add(this.textBoxClusteringIterationsCount);
            this.groupBox27.Controls.Add(this.numericUpDownClusteringError);
            this.groupBox27.Controls.Add(this.checkBoxSkinningScale);
            this.groupBox27.Controls.Add(this.groupBox33);
            this.groupBox27.Controls.Add(this.label52);
            this.groupBox27.Controls.Add(this.checkBoxRandomSeeding);
            this.groupBox27.Controls.Add(this.label51);
            this.groupBox27.Controls.Add(this.comboBoxClusteringDistanceMode);
            this.groupBox27.Controls.Add(this.label50);
            this.groupBox27.Controls.Add(this.numericUpDownClusteringTolerance);
            this.groupBox27.Controls.Add(this.textBoxClusteringErrorTotal);
            this.groupBox27.Controls.Add(this.numericUpDownClusteringIterations);
            this.groupBox27.Controls.Add(this.label53);
            this.groupBox27.Location = new System.Drawing.Point(6, 52);
            this.groupBox27.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox27.Size = new System.Drawing.Size(1286, 175);
            this.groupBox27.TabIndex = 10;
            this.groupBox27.TabStop = false;
            this.groupBox27.Text = "Properties";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(930, 125);
            this.label55.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(70, 13);
            this.label55.TabIndex = 70;
            this.label55.Text = "CPCA Bases:";
            // 
            // numericUpDownBasisVectorsCount
            // 
            this.numericUpDownBasisVectorsCount.Location = new System.Drawing.Point(1074, 119);
            this.numericUpDownBasisVectorsCount.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownBasisVectorsCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownBasisVectorsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownBasisVectorsCount.Name = "numericUpDownBasisVectorsCount";
            this.numericUpDownBasisVectorsCount.Size = new System.Drawing.Size(74, 20);
            this.numericUpDownBasisVectorsCount.TabIndex = 63;
            this.numericUpDownBasisVectorsCount.ThousandsSeparator = true;
            this.numericUpDownBasisVectorsCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownBasisVectorsCount.ValueChanged += new System.EventHandler(this.numericUpDownBasisVectorsCount_ValueChanged);
            // 
            // checkBoxNipals
            // 
            this.checkBoxNipals.AutoSize = true;
            this.checkBoxNipals.Location = new System.Drawing.Point(440, 117);
            this.checkBoxNipals.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxNipals.Name = "checkBoxNipals";
            this.checkBoxNipals.Size = new System.Drawing.Size(55, 17);
            this.checkBoxNipals.TabIndex = 69;
            this.checkBoxNipals.Text = "Nipals";
            this.checkBoxNipals.UseVisualStyleBackColor = true;
            this.checkBoxNipals.CheckedChanged += new System.EventHandler(this.checkBoxNipals_CheckedChanged);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(10, 129);
            this.label54.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(34, 13);
            this.label54.TabIndex = 66;
            this.label54.Text = "DistF:";
            // 
            // comboBoxDistanceMode
            // 
            this.comboBoxDistanceMode.DisplayMember = "0";
            this.comboBoxDistanceMode.FormattingEnabled = true;
            this.comboBoxDistanceMode.Items.AddRange(new object[] {
            "EUCLIDEAN",
            "GEODESIC-ANGLE"});
            this.comboBoxDistanceMode.Location = new System.Drawing.Point(90, 117);
            this.comboBoxDistanceMode.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxDistanceMode.MaxDropDownItems = 2;
            this.comboBoxDistanceMode.Name = "comboBoxDistanceMode";
            this.comboBoxDistanceMode.Size = new System.Drawing.Size(334, 21);
            this.comboBoxDistanceMode.TabIndex = 65;
            this.comboBoxDistanceMode.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged_DistanceMode);
            // 
            // textBoxClusteringErrorTolerance
            // 
            this.textBoxClusteringErrorTolerance.Location = new System.Drawing.Point(1144, 27);
            this.textBoxClusteringErrorTolerance.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxClusteringErrorTolerance.Name = "textBoxClusteringErrorTolerance";
            this.textBoxClusteringErrorTolerance.ReadOnly = true;
            this.textBoxClusteringErrorTolerance.Size = new System.Drawing.Size(128, 20);
            this.textBoxClusteringErrorTolerance.TabIndex = 64;
            // 
            // textBoxClusteringIterationsCount
            // 
            this.textBoxClusteringIterationsCount.Location = new System.Drawing.Point(266, 29);
            this.textBoxClusteringIterationsCount.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxClusteringIterationsCount.Name = "textBoxClusteringIterationsCount";
            this.textBoxClusteringIterationsCount.ReadOnly = true;
            this.textBoxClusteringIterationsCount.Size = new System.Drawing.Size(158, 20);
            this.textBoxClusteringIterationsCount.TabIndex = 63;
            // 
            // numericUpDownClusteringError
            // 
            this.numericUpDownClusteringError.DecimalPlaces = 5;
            this.numericUpDownClusteringError.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownClusteringError.Location = new System.Drawing.Point(1002, 73);
            this.numericUpDownClusteringError.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownClusteringError.Maximum = new decimal(new int[] {
            1316134912,
            2328,
            0,
            0});
            this.numericUpDownClusteringError.Name = "numericUpDownClusteringError";
            this.numericUpDownClusteringError.Size = new System.Drawing.Size(134, 20);
            this.numericUpDownClusteringError.TabIndex = 61;
            this.numericUpDownClusteringError.ThousandsSeparator = true;
            this.numericUpDownClusteringError.ValueChanged += new System.EventHandler(this.numericUpDownClusteringError_ValueChanged);
            // 
            // checkBoxSkinningScale
            // 
            this.checkBoxSkinningScale.AutoSize = true;
            this.checkBoxSkinningScale.Location = new System.Drawing.Point(440, 71);
            this.checkBoxSkinningScale.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxSkinningScale.Name = "checkBoxSkinningScale";
            this.checkBoxSkinningScale.Size = new System.Drawing.Size(53, 17);
            this.checkBoxSkinningScale.TabIndex = 60;
            this.checkBoxSkinningScale.Text = "Scale";
            this.checkBoxSkinningScale.UseVisualStyleBackColor = true;
            this.checkBoxSkinningScale.CheckedChanged += new System.EventHandler(this.checkBoxSkinningScale_CheckedChanged);
            // 
            // groupBox33
            // 
            this.groupBox33.Controls.Add(this.numericUpDownSpectralEigenGap);
            this.groupBox33.Controls.Add(this.checkBoxNNG);
            this.groupBox33.Controls.Add(this.comboBoxClusteringSpectralGraphMode);
            this.groupBox33.Controls.Add(this.numericUpDownPercentageSpectral);
            this.groupBox33.Controls.Add(this.comboBox12);
            this.groupBox33.Location = new System.Drawing.Point(558, 8);
            this.groupBox33.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox33.Name = "groupBox33";
            this.groupBox33.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox33.Size = new System.Drawing.Size(248, 158);
            this.groupBox33.TabIndex = 51;
            this.groupBox33.TabStop = false;
            this.groupBox33.Text = "Spectral";
            // 
            // numericUpDownSpectralEigenGap
            // 
            this.numericUpDownSpectralEigenGap.DecimalPlaces = 4;
            this.numericUpDownSpectralEigenGap.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDownSpectralEigenGap.Location = new System.Drawing.Point(124, 27);
            this.numericUpDownSpectralEigenGap.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownSpectralEigenGap.Name = "numericUpDownSpectralEigenGap";
            this.numericUpDownSpectralEigenGap.Size = new System.Drawing.Size(108, 20);
            this.numericUpDownSpectralEigenGap.TabIndex = 68;
            this.numericUpDownSpectralEigenGap.ThousandsSeparator = true;
            this.numericUpDownSpectralEigenGap.ValueChanged += new System.EventHandler(this.numericUpDownSpectralEigenGap_ValueChanged);
            // 
            // checkBoxNNG
            // 
            this.checkBoxNNG.AutoSize = true;
            this.checkBoxNNG.Location = new System.Drawing.Point(12, 113);
            this.checkBoxNNG.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxNNG.Name = "checkBoxNNG";
            this.checkBoxNNG.Size = new System.Drawing.Size(50, 17);
            this.checkBoxNNG.TabIndex = 66;
            this.checkBoxNNG.Text = "NNG";
            this.checkBoxNNG.UseVisualStyleBackColor = true;
            this.checkBoxNNG.CheckedChanged += new System.EventHandler(this.checkBoxNNG_CheckedChanged);
            // 
            // comboBoxClusteringSpectralGraphMode
            // 
            this.comboBoxClusteringSpectralGraphMode.DisplayMember = "0";
            this.comboBoxClusteringSpectralGraphMode.FormattingEnabled = true;
            this.comboBoxClusteringSpectralGraphMode.Items.AddRange(new object[] {
            "RANDOM_WALK",
            "SYMMETRIC"});
            this.comboBoxClusteringSpectralGraphMode.Location = new System.Drawing.Point(124, 67);
            this.comboBoxClusteringSpectralGraphMode.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxClusteringSpectralGraphMode.MaxDropDownItems = 2;
            this.comboBoxClusteringSpectralGraphMode.Name = "comboBoxClusteringSpectralGraphMode";
            this.comboBoxClusteringSpectralGraphMode.Size = new System.Drawing.Size(104, 21);
            this.comboBoxClusteringSpectralGraphMode.TabIndex = 65;
            this.comboBoxClusteringSpectralGraphMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxClusteringSpectralGraphMode_SelectedIndexChanged);
            // 
            // numericUpDownPercentageSpectral
            // 
            this.numericUpDownPercentageSpectral.Location = new System.Drawing.Point(12, 27);
            this.numericUpDownPercentageSpectral.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownPercentageSpectral.Name = "numericUpDownPercentageSpectral";
            this.numericUpDownPercentageSpectral.Size = new System.Drawing.Size(102, 20);
            this.numericUpDownPercentageSpectral.TabIndex = 63;
            this.numericUpDownPercentageSpectral.ThousandsSeparator = true;
            this.numericUpDownPercentageSpectral.ValueChanged += new System.EventHandler(this.numericUpDown15_ValueChanged);
            // 
            // comboBox12
            // 
            this.comboBox12.DisplayMember = "0";
            this.comboBox12.FormattingEnabled = true;
            this.comboBox12.Items.AddRange(new object[] {
            "NG02",
            "NG02-TH07",
            "NG02-DA08"});
            this.comboBox12.Location = new System.Drawing.Point(12, 69);
            this.comboBox12.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox12.MaxDropDownItems = 2;
            this.comboBox12.Name = "comboBox12";
            this.comboBox12.Size = new System.Drawing.Size(98, 21);
            this.comboBox12.TabIndex = 57;
            this.comboBox12.SelectedIndexChanged += new System.EventHandler(this.comboBox12_SelectedIndexChanged_1);
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(12, 83);
            this.label52.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(28, 13);
            this.label52.TabIndex = 59;
            this.label52.Text = "Dist:";
            // 
            // checkBoxRandomSeeding
            // 
            this.checkBoxRandomSeeding.AutoSize = true;
            this.checkBoxRandomSeeding.Location = new System.Drawing.Point(440, 27);
            this.checkBoxRandomSeeding.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxRandomSeeding.Name = "checkBoxRandomSeeding";
            this.checkBoxRandomSeeding.Size = new System.Drawing.Size(59, 17);
            this.checkBoxRandomSeeding.TabIndex = 51;
            this.checkBoxRandomSeeding.Text = "RandS";
            this.checkBoxRandomSeeding.UseVisualStyleBackColor = true;
            this.checkBoxRandomSeeding.CheckedChanged += new System.EventHandler(this.checkBoxRandomSeeding_CheckedChanged);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(924, 31);
            this.label51.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(34, 13);
            this.label51.TabIndex = 43;
            this.label51.Text = "Toler:";
            // 
            // comboBoxClusteringDistanceMode
            // 
            this.comboBoxClusteringDistanceMode.DisplayMember = "0";
            this.comboBoxClusteringDistanceMode.FormattingEnabled = true;
            this.comboBoxClusteringDistanceMode.Items.AddRange(new object[] {
            "CENTER",
            "NORMAL",
            "DEF. GRADIENT",
            "SKINNING",
            "CLUSTERING",
            "MERGING",
            "OVER-SEGMENTATION"});
            this.comboBoxClusteringDistanceMode.Location = new System.Drawing.Point(90, 73);
            this.comboBoxClusteringDistanceMode.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxClusteringDistanceMode.MaxDropDownItems = 2;
            this.comboBoxClusteringDistanceMode.Name = "comboBoxClusteringDistanceMode";
            this.comboBoxClusteringDistanceMode.Size = new System.Drawing.Size(334, 21);
            this.comboBoxClusteringDistanceMode.TabIndex = 58;
            this.comboBoxClusteringDistanceMode.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged_ClusteringDistanceMode);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(14, 35);
            this.label50.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(25, 13);
            this.label50.TabIndex = 40;
            this.label50.Text = "Iter:";
            // 
            // numericUpDownClusteringTolerance
            // 
            this.numericUpDownClusteringTolerance.DecimalPlaces = 5;
            this.numericUpDownClusteringTolerance.Increment = new decimal(new int[] {
            5,
            0,
            0,
            262144});
            this.numericUpDownClusteringTolerance.Location = new System.Drawing.Point(1002, 27);
            this.numericUpDownClusteringTolerance.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownClusteringTolerance.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownClusteringTolerance.Name = "numericUpDownClusteringTolerance";
            this.numericUpDownClusteringTolerance.Size = new System.Drawing.Size(134, 20);
            this.numericUpDownClusteringTolerance.TabIndex = 42;
            this.numericUpDownClusteringTolerance.ThousandsSeparator = true;
            this.numericUpDownClusteringTolerance.ValueChanged += new System.EventHandler(this.numericUpDownClusteringTolerance_ValueChanged);
            // 
            // textBoxClusteringErrorTotal
            // 
            this.textBoxClusteringErrorTotal.Location = new System.Drawing.Point(1144, 71);
            this.textBoxClusteringErrorTotal.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxClusteringErrorTotal.Name = "textBoxClusteringErrorTotal";
            this.textBoxClusteringErrorTotal.ReadOnly = true;
            this.textBoxClusteringErrorTotal.Size = new System.Drawing.Size(128, 20);
            this.textBoxClusteringErrorTotal.TabIndex = 52;
            // 
            // numericUpDownClusteringIterations
            // 
            this.numericUpDownClusteringIterations.Location = new System.Drawing.Point(90, 29);
            this.numericUpDownClusteringIterations.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownClusteringIterations.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownClusteringIterations.Name = "numericUpDownClusteringIterations";
            this.numericUpDownClusteringIterations.Size = new System.Drawing.Size(164, 20);
            this.numericUpDownClusteringIterations.TabIndex = 39;
            this.numericUpDownClusteringIterations.ThousandsSeparator = true;
            this.numericUpDownClusteringIterations.ValueChanged += new System.EventHandler(this.numericUpDownClusteringIterations_ValueChanged);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(928, 79);
            this.label53.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(32, 13);
            this.label53.TabIndex = 51;
            this.label53.Text = "Error:";
            // 
            // SMA
            // 
            this.SMA.Controls.Add(this.groupBox34);
            this.SMA.Controls.Add(this.groupBox31);
            this.SMA.Controls.Add(this.groupBox32);
            this.SMA.Location = new System.Drawing.Point(4, 22);
            this.SMA.Margin = new System.Windows.Forms.Padding(6);
            this.SMA.Name = "SMA";
            this.SMA.Size = new System.Drawing.Size(1492, 403);
            this.SMA.TabIndex = 2;
            this.SMA.Text = "SMA";
            this.SMA.UseVisualStyleBackColor = true;
            // 
            // groupBox31
            // 
            this.groupBox31.Controls.Add(this.comboBox3);
            this.groupBox31.Controls.Add(this.label47);
            this.groupBox31.Controls.Add(this.button24);
            this.groupBox31.Controls.Add(this.numericUpDownPFactor);
            this.groupBox31.Location = new System.Drawing.Point(6, 6);
            this.groupBox31.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox31.Name = "groupBox31";
            this.groupBox31.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox31.Size = new System.Drawing.Size(299, 94);
            this.groupBox31.TabIndex = 25;
            this.groupBox31.TabStop = false;
            this.groupBox31.Text = "Weights";
            // 
            // comboBox3
            // 
            this.comboBox3.DisplayMember = "0";
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "RIGID",
            "LBS"});
            this.comboBox3.Location = new System.Drawing.Point(140, 55);
            this.comboBox3.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox3.MaxDropDownItems = 4;
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(138, 21);
            this.comboBox3.TabIndex = 43;
            this.comboBox3.Text = "RIGID";
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged_1);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(137, 25);
            this.label47.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(46, 13);
            this.label47.TabIndex = 28;
            this.label47.Text = "pFactor:";
            // 
            // button24
            // 
            this.button24.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button24.Location = new System.Drawing.Point(12, 25);
            this.button24.Margin = new System.Windows.Forms.Padding(6);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(122, 57);
            this.button24.TabIndex = 34;
            this.button24.Text = "Calculate";
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Click += new System.EventHandler(this.buttonComputeWeights_Click);
            // 
            // numericUpDownPFactor
            // 
            this.numericUpDownPFactor.DecimalPlaces = 2;
            this.numericUpDownPFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownPFactor.Location = new System.Drawing.Point(192, 23);
            this.numericUpDownPFactor.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDownPFactor.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownPFactor.Name = "numericUpDownPFactor";
            this.numericUpDownPFactor.Size = new System.Drawing.Size(86, 20);
            this.numericUpDownPFactor.TabIndex = 28;
            this.numericUpDownPFactor.ThousandsSeparator = true;
            this.numericUpDownPFactor.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericUpDownPFactor.ValueChanged += new System.EventHandler(this.numericUpDownPFactor_ValueChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(813, 828);
            this.label44.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(48, 13);
            this.label44.TabIndex = 68;
            this.label44.Text = "Coloring:";
            // 
            // comboBoxVertexColoringMode
            // 
            this.comboBoxVertexColoringMode.DisplayMember = "0";
            this.comboBoxVertexColoringMode.FormattingEnabled = true;
            this.comboBoxVertexColoringMode.Items.AddRange(new object[] {
            "NONE",
            "RANDOM",
            "GEODESIC_DIST",
            "DEF_GRADIENT",
            "CLUSTER",
            "BONE",
            "ERROR"});
            this.comboBoxVertexColoringMode.Location = new System.Drawing.Point(864, 825);
            this.comboBoxVertexColoringMode.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxVertexColoringMode.MaxDropDownItems = 4;
            this.comboBoxVertexColoringMode.Name = "comboBoxVertexColoringMode";
            this.comboBoxVertexColoringMode.Size = new System.Drawing.Size(140, 21);
            this.comboBoxVertexColoringMode.TabIndex = 67;
            this.comboBoxVertexColoringMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxVertexColoringMode_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(1663, 27);
            this.menuStrip1.TabIndex = 51;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadToolStripMenuItem1
            // 
            this.loadToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sMAToolStripMenuItem,
            this.clusteringToolStripMenuItem});
            this.loadToolStripMenuItem1.Name = "loadToolStripMenuItem1";
            this.loadToolStripMenuItem1.Size = new System.Drawing.Size(37, 19);
            this.loadToolStripMenuItem1.Text = "File";
            // 
            // sMAToolStripMenuItem
            // 
            this.sMAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sMAToolStripMenuItem1});
            this.sMAToolStripMenuItem.Name = "sMAToolStripMenuItem";
            this.sMAToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.sMAToolStripMenuItem.Text = "Load";
            // 
            // sMAToolStripMenuItem1
            // 
            this.sMAToolStripMenuItem1.Name = "sMAToolStripMenuItem1";
            this.sMAToolStripMenuItem1.Size = new System.Drawing.Size(99, 22);
            this.sMAToolStripMenuItem1.Text = "SMA";
            this.sMAToolStripMenuItem1.Click += new System.EventHandler(this.loadSMA_Click);
            // 
            // clusteringToolStripMenuItem
            // 
            this.clusteringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sMAToolStripMenuItem2});
            this.clusteringToolStripMenuItem.Name = "clusteringToolStripMenuItem";
            this.clusteringToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.clusteringToolStripMenuItem.Text = "Save";
            // 
            // sMAToolStripMenuItem2
            // 
            this.sMAToolStripMenuItem2.Name = "sMAToolStripMenuItem2";
            this.sMAToolStripMenuItem2.Size = new System.Drawing.Size(99, 22);
            this.sMAToolStripMenuItem2.Text = "SMA";
            // 
            // RenderWindow
            // 
            this.RenderWindow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RenderWindow.BackColor = System.Drawing.Color.Black;
            this.RenderWindow.Location = new System.Drawing.Point(8, 52);
            this.RenderWindow.Margin = new System.Windows.Forms.Padding(8);
            this.RenderWindow.MaximumSize = new System.Drawing.Size(3048, 2048);
            this.RenderWindow.Name = "RenderWindow";
            this.RenderWindow.Size = new System.Drawing.Size(1024, 768);
            this.RenderWindow.TabIndex = 0;
            this.RenderWindow.VSync = false;
            this.RenderWindow.Load += new System.EventHandler(this.RenderWindow_Load);
            this.RenderWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.RenderWindow_Draw);
            this.RenderWindow.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RenderWindow_KeyDown);
            this.RenderWindow.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RenderWindow_KeyUp);
            this.RenderWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderWindow_MouseMove);
            this.RenderWindow.Resize += new System.EventHandler(this.RenderWindow_Resize);
            // 
            // Example
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1680, 1045);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.comboBoxVertexColoringMode);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.RenderWindow);
            this.Controls.Add(this.tabControl4);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Example";
            this.Text = "Final Project";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox_Camera.ResumeLayout(false);
            this.groupBox_Camera.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown22)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown21)).EndInit();
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown25)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox28.ResumeLayout(false);
            this.groupBox28.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown35)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown36)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown34)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileStep)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInstancesCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown31)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown17)).EndInit();
            this.Tesselation.ResumeLayout(false);
            this.Tesselation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTessLevelOuter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTessLevelInner)).EndInit();
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxFPS)).EndInit();
            this.groupBox17.ResumeLayout(false);
            this.groupBox16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_gamma)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown18)).EndInit();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown16)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown28)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown29)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.tabPage7.ResumeLayout(false);
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.groupBox34.ResumeLayout(false);
            this.groupBox34.PerformLayout();
            this.groupBox32.ResumeLayout(false);
            this.groupBox32.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown33)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFittingWeightsIter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFittingRestPoseIter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFittingMatricesIter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFittingIterations)).EndInit();
            this.tabControl4.ResumeLayout(false);
            this.DG.ResumeLayout(false);
            this.DG.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSelectedVertex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown30)).EndInit();
            this.Clustering.ResumeLayout(false);
            this.Clustering.PerformLayout();
            this.groupBox29.ResumeLayout(false);
            this.groupBox29.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMergingPrevTolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClusterSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBoneSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClustersCount)).EndInit();
            this.groupBox27.ResumeLayout(false);
            this.groupBox27.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBasisVectorsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClusteringError)).EndInit();
            this.groupBox33.ResumeLayout(false);
            this.groupBox33.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpectralEigenGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPercentageSpectral)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClusteringTolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClusteringIterations)).EndInit();
            this.SMA.ResumeLayout(false);
            this.groupBox31.ResumeLayout(false);
            this.groupBox31.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPFactor)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_Camera;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button_backGrColor;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button_backGrTex;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TreeView treeView_models;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown numericUpDown10;
        private System.Windows.Forms.NumericUpDown numericUpDown9;
        private System.Windows.Forms.NumericUpDown numericUpDown8;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown numericUpDown7;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox_lightSpot;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.NumericUpDown numericUpDown_gamma;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxMemorySize;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.TextBox textBoxTotalPasses;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.ComboBox comboBoxRenderingType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.ComboBox comboBox8;
        private System.Windows.Forms.ComboBox comboBox9;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.NumericUpDown numericUpDown16;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown numericUpDown12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numericUpDown11;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUpDown14;
        private System.Windows.Forms.NumericUpDown numericUpDown13;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.NumericUpDown numericUpDown18;
        private System.Windows.Forms.NumericUpDown FileStep;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.NumericUpDown numericUpDown19;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.NumericUpDown numericUpDown20;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.CheckBox checkBox13;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.NumericUpDown numericUpDown21;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.CheckBox checkBox14;
        private System.Windows.Forms.CheckBox checkBox15;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.GroupBox groupBox24;
        private System.Windows.Forms.CheckBox checkBox17;
        private System.Windows.Forms.CheckBox checkBox16;
        private System.Windows.Forms.CheckBox checkBox19;
        private System.Windows.Forms.CheckBox checkBox18;
        private System.Windows.Forms.CheckBox checkBox20;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel _statusLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setToModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setToBackgroundToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox10;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.CheckBox checkBox21;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem CSGtoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem setOperationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intersectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem differenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox22;
        private System.Windows.Forms.CheckBox checkBox23;
        private System.Windows.Forms.CheckBox checkBox24;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.NumericUpDown numericUpDown22;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.NumericUpDown numericUpDown24;
        private System.Windows.Forms.NumericUpDown numericUpDown23;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ComboBox comboBox11;
        private System.Windows.Forms.CheckBox checkBox25;
        private System.Windows.Forms.CheckBox checkBox26;
        private System.Windows.Forms.ComboBox comboBox13;
        private System.Windows.Forms.CheckBox checkBox27;
        private System.Windows.Forms.NumericUpDown numericUpDown25;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.NumericUpDown numericUpDown26;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.GroupBox groupBox26;
        private System.Windows.Forms.CheckBox checkBox28;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.CheckBox checkBox29;
        private System.Windows.Forms.NumericUpDown numericUpDown27;
        private System.Windows.Forms.NumericUpDown numericUpDown28;
        private System.Windows.Forms.NumericUpDown numericUpDown29;
        private System.Windows.Forms.CheckBox groups;
        private System.Windows.Forms.ToolStripMenuItem loadSMA;
        private System.Windows.Forms.ToolStripMenuItem selectedPoseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAsRestPoseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propagateColoringModeToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox32;
        private System.Windows.Forms.ComboBox comboBox17;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.NumericUpDown numericUpDownFittingIterations;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Button ButtonSmaError;
        private System.Windows.Forms.CheckBox checkBox34;
        private System.Windows.Forms.GroupBox groupBox34;
        private System.Windows.Forms.Button button30;
        private System.Windows.Forms.ToolStripMenuItem loadEditableModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox18;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.TabPage Clustering;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.ComboBox comboBoxClusteringMode;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.NumericUpDown numericUpDownClusteringTolerance;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.NumericUpDown numericUpDownClusteringIterations;
        private System.Windows.Forms.CheckBox checkBox32;
        private System.Windows.Forms.GroupBox groupBox29;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.NumericUpDown numericUpDownClusterSelection;
        private System.Windows.Forms.NumericUpDown numericUpDownBoneSelection;
        private System.Windows.Forms.CheckBox checkBoxDrawSpheres;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.NumericUpDown numericUpDownClustersCount;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.TabPage DG;
        private System.Windows.Forms.TabPage SMA;
        private System.Windows.Forms.CheckBox checkBox30;
        private System.Windows.Forms.ComboBox comboBoxDGComponentsMode;
        private System.Windows.Forms.ComboBox comboBoxDGmode;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.CheckBox checkBox35;
        private System.Windows.Forms.CheckBox checkBoxDrawRegions;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.ComboBox comboBoxClusteringDistanceMode;
        private System.Windows.Forms.TextBox textBoxClusteringErrorTotal;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.GroupBox groupBox27;
        private System.Windows.Forms.CheckBox checkBoxRandomSeeding;
        private System.Windows.Forms.CheckBox checkBoxSkinningScale;
        private System.Windows.Forms.CheckBox checkBoxDrawNeighbors;
        private System.Windows.Forms.ComboBox comboBox12;
        private System.Windows.Forms.NumericUpDown numericUpDownPercentageSpectral;
        private System.Windows.Forms.CheckBox checkBox39;
        private System.Windows.Forms.GroupBox groupBox33;
        private System.Windows.Forms.ComboBox comboBoxClusteringSpectralGraphMode;
        private System.Windows.Forms.NumericUpDown numericUpDownClusteringError;
        private System.Windows.Forms.TextBox textBoxClustersCount;
        private System.Windows.Forms.TextBox textBoxClusteringIterationsCount;
        private System.Windows.Forms.TextBox textBoxClusteringErrorTolerance;
        private System.Windows.Forms.ComboBox comboBoxDistanceMode;
        private System.Windows.Forms.CheckBox checkBoxNNG;
        private System.Windows.Forms.NumericUpDown numericUpDownSpectralEigenGap;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.CheckBox checkBoxNipals;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.NumericUpDown numericUpDownBasisVectorsCount;
        private System.Windows.Forms.CheckBox checkBox31;
        private System.Windows.Forms.NumericUpDown numericUpDown30;
        private System.Windows.Forms.Button button28;
        private System.Windows.Forms.NumericUpDown numericUpDownSelectedVertex;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxFPS;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.CheckBox checkBox33;
        private System.Windows.Forms.CheckBox checkBoxClusteringSetFixedColor;
        private System.Windows.Forms.CheckBox checkBoxMergeClusterings;
        private System.Windows.Forms.NumericUpDown numericUpDownMergingPrevTolerance;
        private System.Windows.Forms.Button button29;
        private System.Windows.Forms.CheckBox checkBox36;
        private System.Windows.Forms.Button button32;
        private System.Windows.Forms.Button button33;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sMAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clusteringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sMAToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sMAToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem sMAToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem sMAToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clusteringToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem sMAToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem selectedPoseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedPoseToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem featureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedPoseToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem featureToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectedPoseToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem3;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.ComboBox comboBoxVertexColoringMode;
        private System.Windows.Forms.Button button34;
        private System.Windows.Forms.GroupBox Tesselation;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.NumericUpDown numericUpDownTessLevelOuter;
        private System.Windows.Forms.NumericUpDown numericUpDownTessLevelInner;
        private System.Windows.Forms.CheckBox checkBoxTessEnable;
        private GLControl RenderWindow;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.NumericUpDown numericUpDown15;
        private System.Windows.Forms.NumericUpDown numericUpDown17;
        private System.Windows.Forms.CheckBox checkBox37;
        private System.Windows.Forms.NumericUpDown numericUpDown31;
        private System.Windows.Forms.NumericUpDown numericUpDownInstancesCount;
        private System.Windows.Forms.GroupBox groupBox31;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.NumericUpDown numericUpDownPFactor;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.NumericUpDown numericUpDown32;
        private System.Windows.Forms.CheckBox checkBox38;
        private System.Windows.Forms.CheckBox checkBox40;
        private System.Windows.Forms.GroupBox groupBox28;
        private System.Windows.Forms.CheckBox checkBox41;
        private System.Windows.Forms.TextBox textBoxKValue;
        private System.Windows.Forms.NumericUpDown numericUpDown34;
        private System.Windows.Forms.NumericUpDown numericUpDown35;
        private System.Windows.Forms.TextBox textBoxKPercentage;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.NumericUpDown numericUpDown36;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.NumericUpDown numericUpDownFittingWeightsIter;
        private System.Windows.Forms.NumericUpDown numericUpDownFittingRestPoseIter;
        private System.Windows.Forms.NumericUpDown numericUpDownFittingMatricesIter;
        private System.Windows.Forms.TextBox textBoxFittingIter;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.Button button31;
        private System.Windows.Forms.Button button35;
        private System.Windows.Forms.ToolStripMenuItem rMAToolStripMenuItem;
        private System.Windows.Forms.Button button36;
        private System.Windows.Forms.Button button37;
        private System.Windows.Forms.CheckBox checkBox42;
        private System.Windows.Forms.CheckBox checkBox43;
        private System.Windows.Forms.Button button38;
        private System.Windows.Forms.NumericUpDown numericUpDown33;
    }
}

