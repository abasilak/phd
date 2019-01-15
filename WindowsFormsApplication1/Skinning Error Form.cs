using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace abasilak
{
    public partial class ResultsForm : Form
    {
        SMA           _sma;
        SMA_ErrorData _errorData;

        public ResultsForm()
        {
            InitializeComponent();

            _sma        = Example._scene.meshAnimation.sma;
            _errorData  = null;

            eigenSkinDisplacementsNumericUpDown.Maximum    = _sma.numPoses;
            eigenWeightsDisplacementsNumericUpDown.Maximum = _sma.numPoses;
        }

        #region Log Functions
        private void clearLogButton_Click(object sender, EventArgs e)
        {
            loggerRichTextBox.Clear();
        }
        private void writeLogText(string text)
        {
            loggerRichTextBox.AppendText(text + "\n");
        }
        private void writeLogHeadingText()
        {
            string fittingModeText;
            fittingModeText = "\t\t --- ";
            fittingModeText += _errorData.fittingModeText();
            fittingModeText += " ---";
            writeLogText(fittingModeText);
        }
        private void writeLogErrorData()
        {
            string fittingErrorModeText = "";
            if (_sma.fittingErrorMode == Modes.FittingError.MSE) fittingErrorModeText = "MSE (Mean Square Error): " + _errorData.totalErrorMSE.ToString();
            else if (_sma.fittingErrorMode == Modes.FittingError.RMSE) fittingErrorModeText = "RMSE (Root Mean Square Error): " + _errorData.totalErrorRMSE.ToString();
            else if (_sma.fittingErrorMode == Modes.FittingError.SME) fittingErrorModeText = "SME: " + _errorData.totalErrorSME.ToString();
            else if (_sma.fittingErrorMode == Modes.FittingError.KG) fittingErrorModeText = "KG: " + _errorData.totalErrorKG.ToString();
            else if (_sma.fittingErrorMode == Modes.FittingError.STED) fittingErrorModeText = "STED: " + _errorData.totalErrorSTED.ToString();

            writeLogText(fittingErrorModeText);
            writeLogText(_errorData.fittingTime);           
        }
        #endregion

        #region Chart Functions
        private void writeChartErrorData()
        {
            string fittingErrorModeText = "Total: ";
            if (_sma.fittingErrorMode == Modes.FittingError.MSE) fittingErrorModeText += _errorData.totalErrorMSE.ToString();
            else if (_sma.fittingErrorMode == Modes.FittingError.RMSE) fittingErrorModeText += _errorData.totalErrorRMSE.ToString();
            else if (_sma.fittingErrorMode == Modes.FittingError.SME) fittingErrorModeText += _errorData.totalErrorSME.ToString();
            else if (_sma.fittingErrorMode == Modes.FittingError.KG) fittingErrorModeText += _errorData.totalErrorKG.ToString();
            else if (_sma.fittingErrorMode == Modes.FittingError.STED) fittingErrorModeText += _errorData.totalErrorSTED.ToString();

            // Data arrays.
            string[] seriesArray = new string[_sma.numPoses];
            double[] pointsArray = new double[_sma.numPoses];
            for (int i = 0; i < seriesArray.Length; i++)
            {
                seriesArray[i] = "Pose_" + i.ToString();
                if (_sma.fittingErrorMode == Modes.FittingError.MSE) pointsArray[i] = _errorData.poseErrorMSE[i];
                else if (_sma.fittingErrorMode == Modes.FittingError.RMSE) pointsArray[i] = _errorData.poseErrorRMSE[i];
                else if (_sma.fittingErrorMode == Modes.FittingError.KG) pointsArray[i] = _errorData.poseErrorKG[i];
                else if (_sma.fittingErrorMode == Modes.FittingError.SME) pointsArray[i] = _errorData.poseErrorSME[i];
                else if (_sma.fittingErrorMode == Modes.FittingError.STED) pointsArray[i] = _errorData.poseErrorSTED[i];
            }

            chart.Titles.Clear(); 
            chart.Titles.Add(fittingErrorModeText);

            // Add series.
            chart.Series.Clear();
            chart.ResetAutoValues();
            Series newSeries = chart.Series.Add("Skinning");
            for (int i = 0; i < seriesArray.Length; i++)
            {
                newSeries.Points.AddXY((double)i, pointsArray[i]);
                newSeries.IsValueShownAsLabel = true;
            }
            chart.Update();
        }
        public  void createChartError()
        {
            setErrorDataChart();

            treeViewCharts.Nodes.Add(_errorData.fittingModeText());
            treeViewCharts.Nodes[treeViewCharts.Nodes.Count - 1].Tag = treeViewCharts.Nodes.Count - 1;            
        }
        public  void setTextChartError()
        {
            string FittingModeText = _errorData.fittingModeText();
            if (_errorData.enableEigenSkin)
                FittingModeText += " + EigenSkin(" + _sma.errorDataVertex.eigenSkinDisplacements.ToString() + ")";

            if (_errorData.enableEigenWeights)
                FittingModeText += " + EigenWeights(" + _sma.errorDataVertex.eigenWeightsDisplacements.ToString() + ")";

            treeViewCharts.Nodes[_sma.selectedErrorData].Text = FittingModeText;
        }

        private void saveChartButton_Click(object sender, EventArgs e)
        {
            chart.SaveImage("C:\\Users\\abasilak\\Desktop\\" + treeViewCharts.Nodes[_sma.selectedErrorData].Text + ".png", ChartImageFormat.Png);
        }
        private void treeChartView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_sma.selectedErrorData != (int)e.Node.Tag)
            {
                _sma.selectedErrorData = (int)e.Node.Tag;
                _sma.computeApproxModels(Example._scene.meshAnimation);
                setData();
            }
        }
        private void setErrorDataChart()
        {
            _errorData = (_sma.fittingErrorVectorMode == Modes.FittingErrorVector.VERTEX) ? _sma.errorDataVertex : _sma.errorDataNormal;
        }
        #endregion

        private void setData()
        {
            setErrorDataChart();

            writeLogHeadingText();
            writeLogErrorData();

            writeChartErrorData();

            writeCompressionTextBox();

            setTextChartError();
        }
        private void writeCompressionTextBox()
        {
            float Compression = 0.0f;
            float EigenSkinStorage      = (3 * _sma.numVertices + _sma.numPoses) * _errorData.eigenSkinDisplacements;
            float EigenWeightsStorage   = (4 * _sma.numVertices + _sma.numPoses) * _errorData.eigenWeightsDisplacements;

            if (_sma.errorDataVertex.enableEigenSkin)
                Compression += EigenSkinStorage; // Vertex
            if (_sma.errorDataNormal.enableEigenSkin)
                Compression += EigenSkinStorage; // Normal

            if (_sma.errorDataVertex.enableEigenWeights)
                Compression += EigenWeightsStorage; // Vertex

            Compression += _sma.errorDataVertex.smaStorage;
            Compression /= 3.0f * _sma.numPoses * _sma.numVertices;

            compressionTextBox.Text = Compression.ToString();
        }
        private void fittingErrorModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _sma.fittingErrorMode = (Modes.FittingError)fittingErrorComboBox.SelectedIndex;
            setData();
        }
        private void fittingErrorVectoreModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _sma.fittingErrorVectorMode = (Modes.FittingErrorVector)fittingErrorVectorComboBox.SelectedIndex;           
            setData();
        }

        #region EigenSkin Functions
        private void eigenSkinResetButton_Click(object sender, EventArgs e)
        {
            _errorData.enableEigenSkin = false;
            _errorData.setEigenSkinBuffer();
            _errorData.computeFinalPositions();
            _sma.computeApproxModels(Example._scene.meshAnimation);
            setData();
        }
        private void eigenSkinSetButton_Click(object sender, EventArgs e)
        {
            _errorData.enableEigenSkin = true;
            _errorData.setEigenSkinApproximation();
            _errorData.setEigenSkinBuffer();
            _errorData.computeFinalPositions();
            _sma.computeApproxModels(Example._scene.meshAnimation);
            setData();
        }
        private void eigenSkinDisplacementsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _errorData.eigenSkinDisplacements = (int)eigenSkinDisplacementsNumericUpDown.Value;
        }
        #endregion

        #region EigenWeights Functions
        private void eigenWeightsDisplacementsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _errorData.eigenWeightsDisplacements = (int)eigenWeightsDisplacementsNumericUpDown.Value;
        }
        private void eigenWeightsSetButton_Click(object sender, EventArgs e)
        {
            _errorData.enableEigenWeights = true;
            _errorData.setEigenWeightsApproximation();
            _errorData.setEigenWeightsBuffer();
            _errorData.computeFinalPositions();
            _sma.computeApproxModels(Example._scene.meshAnimation);
            setData();
        }
        private void eigenWeightsResetButton_Click(object sender, EventArgs e)
        {
            _errorData.enableEigenWeights = false;
            _errorData.setEigenWeightsBuffer();
            _errorData.computeFinalPositions();
            _sma.computeApproxModels(Example._scene.meshAnimation);
            setData();
        }
        #endregion
    }
}
