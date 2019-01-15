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
    public partial class PerformanceForm : Form
    {
        public PerformanceForm()
        {
            InitializeComponent();

            string chartText = "Performance";
            chart.Titles.Clear();
            chart.Titles.Add(chartText);
        }

        #region Chart Functions
        public void writeChartErrorData(List<double> _localPerf, List<string> _localNames)
        {

            try
            {
                int Count = _localPerf.Count + 1;

                // Data arrays.
                string[] seriesArray = new string[Count];
                double[] pointsArray = new double[Count];

                double Sum = 0.0f;
                for (int i = 0; i < Count - 1; i++)
                {
                    seriesArray[i] = _localNames[i];
                    pointsArray[i] = _localPerf[i];
                    Sum += pointsArray[i];
                }

                seriesArray[Count - 1] = "*. Global Time";
                pointsArray[Count - 1] = Sum;

                // Add series.
                Series newSeries;
                chart.ResetAutoValues();
                chart.Series.Clear();
                for (int i = 0; i < seriesArray.Length; i++)
                {
                    newSeries = chart.Series.Add(seriesArray[i]);
                    newSeries.IsValueShownAsLabel = true;
                    newSeries.Points.Add(pointsArray[i]);
                }
                chart.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion 
    }
}
