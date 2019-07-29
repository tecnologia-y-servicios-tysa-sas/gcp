using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCP_CF.Helpers
{
    public class ChartData
    {
        public List<string> labels;
        public List<ChartDataSet> datasets;

        public void Initialize()
        {
            this.labels = new List<string>();
            this.datasets = new List<ChartDataSet>();
        }
    }
    public class ChartDataSet
    {
        public string label;
        public List<double> data;

        public void Initialize()
        {
            this.label = string.Empty;
            this.data = new List<double>();
        }
    }

    public static class GraphicsHelper
    {
    }
}