namespace abasilak
{
    partial class ResultsForm
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.label = new System.Windows.Forms.Label();
            this.compressionTextBox = new System.Windows.Forms.TextBox();
            this.loggerRichTextBox = new System.Windows.Forms.RichTextBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.chartsGroupBox = new System.Windows.Forms.GroupBox();
            this.treeViewCharts = new System.Windows.Forms.TreeView();
            this.saveFigureButton = new System.Windows.Forms.Button();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.fittingErrorComboBox = new System.Windows.Forms.ComboBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.fittingErrorVectorComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.eigenSkinDisplacementsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.button27 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.eigenWeightsDisplacementsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.chartsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eigenSkinDisplacementsNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eigenWeightsDisplacementsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(99, 13);
            this.label.TabIndex = 0;
            this.label.Text = "Compression Level:";
            // 
            // compressionTextBox
            // 
            this.compressionTextBox.BackColor = System.Drawing.SystemColors.Info;
            this.compressionTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.compressionTextBox.Location = new System.Drawing.Point(117, 6);
            this.compressionTextBox.Name = "compressionTextBox";
            this.compressionTextBox.ReadOnly = true;
            this.compressionTextBox.Size = new System.Drawing.Size(124, 20);
            this.compressionTextBox.TabIndex = 11;
            // 
            // loggerRichTextBox
            // 
            this.loggerRichTextBox.Location = new System.Drawing.Point(1488, 44);
            this.loggerRichTextBox.Name = "loggerRichTextBox";
            this.loggerRichTextBox.Size = new System.Drawing.Size(280, 655);
            this.loggerRichTextBox.TabIndex = 12;
            this.loggerRichTextBox.Text = "";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(1488, 12);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(280, 23);
            this.clearButton.TabIndex = 13;
            this.clearButton.Text = "Clear Log";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearLogButton_Click);
            // 
            // chartsGroupBox
            // 
            this.chartsGroupBox.Controls.Add(this.treeViewCharts);
            this.chartsGroupBox.Controls.Add(this.saveFigureButton);
            this.chartsGroupBox.Controls.Add(this.chart);
            this.chartsGroupBox.Location = new System.Drawing.Point(13, 77);
            this.chartsGroupBox.Name = "chartsGroupBox";
            this.chartsGroupBox.Size = new System.Drawing.Size(1456, 622);
            this.chartsGroupBox.TabIndex = 14;
            this.chartsGroupBox.TabStop = false;
            this.chartsGroupBox.Text = "Charts";
            // 
            // treeViewCharts
            // 
            this.treeViewCharts.Location = new System.Drawing.Point(1326, 19);
            this.treeViewCharts.Name = "treeViewCharts";
            this.treeViewCharts.Size = new System.Drawing.Size(124, 590);
            this.treeViewCharts.TabIndex = 31;
            this.treeViewCharts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeChartView_AfterSelect);
            // 
            // saveFigureButton
            // 
            this.saveFigureButton.Location = new System.Drawing.Point(1248, 581);
            this.saveFigureButton.Name = "saveFigureButton";
            this.saveFigureButton.Size = new System.Drawing.Size(72, 23);
            this.saveFigureButton.TabIndex = 29;
            this.saveFigureButton.Text = "Save Figure";
            this.saveFigureButton.UseVisualStyleBackColor = true;
            this.saveFigureButton.Click += new System.EventHandler(this.saveChartButton_Click);
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Location = new System.Drawing.Point(7, 20);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(1313, 589);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            // 
            // fittingErrorComboBox
            // 
            this.fittingErrorComboBox.DisplayMember = "0";
            this.fittingErrorComboBox.FormattingEnabled = true;
            this.fittingErrorComboBox.Items.AddRange(new object[] {
            "MSE",
            "RMSE",
            "SME",
            "KG",
            "STED"});
            this.fittingErrorComboBox.Location = new System.Drawing.Point(285, 6);
            this.fittingErrorComboBox.MaxDropDownItems = 4;
            this.fittingErrorComboBox.Name = "fittingErrorComboBox";
            this.fittingErrorComboBox.Size = new System.Drawing.Size(114, 21);
            this.fittingErrorComboBox.TabIndex = 27;
            this.fittingErrorComboBox.SelectedIndexChanged += new System.EventHandler(this.fittingErrorModeComboBox_SelectedIndexChanged);
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(247, 9);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(32, 13);
            this.errorLabel.TabIndex = 28;
            this.errorLabel.Text = "Error:";
            // 
            // fittingErrorVectorComboBox
            // 
            this.fittingErrorVectorComboBox.DisplayMember = "0";
            this.fittingErrorVectorComboBox.FormattingEnabled = true;
            this.fittingErrorVectorComboBox.Items.AddRange(new object[] {
            "VERTEX",
            "NORMAL"});
            this.fittingErrorVectorComboBox.Location = new System.Drawing.Point(405, 5);
            this.fittingErrorVectorComboBox.MaxDropDownItems = 4;
            this.fittingErrorVectorComboBox.Name = "fittingErrorVectorComboBox";
            this.fittingErrorVectorComboBox.Size = new System.Drawing.Size(114, 21);
            this.fittingErrorVectorComboBox.TabIndex = 30;
            this.fittingErrorVectorComboBox.SelectedIndexChanged += new System.EventHandler(this.fittingErrorVectoreModeComboBox_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(137, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 23);
            this.button1.TabIndex = 31;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.eigenSkinResetButton_Click);
            // 
            // eigenSkinDisplacementsNumericUpDown
            // 
            this.eigenSkinDisplacementsNumericUpDown.Location = new System.Drawing.Point(6, 19);
            this.eigenSkinDisplacementsNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.eigenSkinDisplacementsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.eigenSkinDisplacementsNumericUpDown.Name = "eigenSkinDisplacementsNumericUpDown";
            this.eigenSkinDisplacementsNumericUpDown.Size = new System.Drawing.Size(38, 20);
            this.eigenSkinDisplacementsNumericUpDown.TabIndex = 50;
            this.eigenSkinDisplacementsNumericUpDown.ThousandsSeparator = true;
            this.eigenSkinDisplacementsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.eigenSkinDisplacementsNumericUpDown.ValueChanged += new System.EventHandler(this.eigenSkinDisplacementsNumericUpDown_ValueChanged);
            // 
            // button27
            // 
            this.button27.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button27.Location = new System.Drawing.Point(50, 17);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(81, 23);
            this.button27.TabIndex = 52;
            this.button27.Text = "Set";
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Click += new System.EventHandler(this.eigenSkinSetButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button27);
            this.groupBox1.Controls.Add(this.eigenSkinDisplacementsNumericUpDown);
            this.groupBox1.Location = new System.Drawing.Point(14, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 46);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EigenSkin";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.eigenWeightsDisplacementsNumericUpDown);
            this.groupBox2.Location = new System.Drawing.Point(220, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 46);
            this.groupBox2.TabIndex = 54;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "EigenWeights";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(137, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(54, 23);
            this.button2.TabIndex = 31;
            this.button2.Text = "Reset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.eigenWeightsResetButton_Click);
            // 
            // button3
            // 
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button3.Location = new System.Drawing.Point(50, 17);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 23);
            this.button3.TabIndex = 52;
            this.button3.Text = "Set";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.eigenWeightsSetButton_Click);
            // 
            // eigenWeightsDisplacementsNumericUpDown
            // 
            this.eigenWeightsDisplacementsNumericUpDown.Location = new System.Drawing.Point(6, 19);
            this.eigenWeightsDisplacementsNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.eigenWeightsDisplacementsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.eigenWeightsDisplacementsNumericUpDown.Name = "eigenWeightsDisplacementsNumericUpDown";
            this.eigenWeightsDisplacementsNumericUpDown.Size = new System.Drawing.Size(38, 20);
            this.eigenWeightsDisplacementsNumericUpDown.TabIndex = 50;
            this.eigenWeightsDisplacementsNumericUpDown.ThousandsSeparator = true;
            this.eigenWeightsDisplacementsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.eigenWeightsDisplacementsNumericUpDown.ValueChanged += new System.EventHandler(this.eigenWeightsDisplacementsNumericUpDown_ValueChanged);
            // 
            // ResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1780, 711);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.fittingErrorVectorComboBox);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.fittingErrorComboBox);
            this.Controls.Add(this.chartsGroupBox);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.loggerRichTextBox);
            this.Controls.Add(this.compressionTextBox);
            this.Controls.Add(this.label);
            this.Name = "ResultsForm";
            this.Text = "Skinning Results";
            this.chartsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eigenSkinDisplacementsNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eigenWeightsDisplacementsNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public  System.Windows.Forms.TextBox     compressionTextBox;
        public  System.Windows.Forms.RichTextBox loggerRichTextBox;
        public System.Windows.Forms.ComboBox fittingErrorComboBox;
        
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.GroupBox chartsGroupBox;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Button saveFigureButton;
        public System.Windows.Forms.ComboBox fittingErrorVectorComboBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.TreeView treeViewCharts;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown eigenSkinDisplacementsNumericUpDown;
        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.NumericUpDown eigenWeightsDisplacementsNumericUpDown;
    }
}