using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Reporting;
using OxyPlot.Wpf;

namespace FericaReader
{
    public class GraphView
    {
        public PlotModel Model { get;} = new PlotModel();
        public PlotController Controller { get; } = new PlotController();


        public OxyPlot.Axes.DateTimeAxis X {get; private set; } = new OxyPlot.Axes.DateTimeAxis();
        public OxyPlot.Axes.LinearAxis Y {get; private set; }   = new OxyPlot.Axes.LinearAxis();

        public CalcResultGraph CsvCalcResultGraph{get;set; }

 
        public void CreateGraph(List<CsvCalcResults> results)
        {
            int days = DateTime.DaysInMonth(results.Last().FromDate.Year,results.Last().FromDate.Month);
            var XwidthMin = new DateTime(results.First().FromDate.Year,results.First().FromDate.Month,1);
            var XwidthMax = new DateTime(results.Last().FromDate.Year,results.Last().FromDate.Month,days);
            
            //始点終点をリストに追加
            var resultList = new List<CsvCalcResults>()
            {
                new CsvCalcResults {IDm = "", FromDate = XwidthMin ,ToDate = XwidthMin,Deposit = 0,Payment = 0,FromToDate=XwidthMin.ToString("yy/MM/dd")}
            };
            results.Add(new CsvCalcResults { IDm = "", FromDate = XwidthMax, ToDate = XwidthMax, Deposit = 0, Payment = 0 ,FromToDate=XwidthMax.ToString("yy/MM/dd")});
            resultList.AddRange(results);

            this.CsvCalcResultGraph = new CalcResultGraph(resultList);

            this.X = new OxyPlot.Axes.DateTimeAxis()
            {
                Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(XwidthMin),
                Maximum = OxyPlot.Axes.DateTimeAxis.ToDouble(XwidthMax),
                LabelFormatter = (x) =>
                {
                    var dt = OxyPlot.Axes.DateTimeAxis.ToDateTime(x);
                    return resultList.FirstOrDefault(d => d.FromDate == dt)?.FromToDate;
                }
                //StringFormat = "yy/MM/dd",

            };

            //OxyPlot.Axes.LinearAxis X = new OxyPlot.Axes.LinearAxis();

            this.Y = new OxyPlot.Axes.LinearAxis()
            {
                Minimum = 0,
            };
            Model.Title = "CalcResultsGraph";
            Model.Axes.Add(X);
            Model.Axes.Add(Y);
            Model.Series.Add(CsvCalcResultGraph.DepositLineSeries);
            Model.Series.Add(CsvCalcResultGraph.PaymentLineSeries);

            Model.InvalidatePlot(true);
        }

        public class CalcResultGraph
        {
            public OxyPlot.Series.LineSeries DepositLineSeries { get; private set; }

            public OxyPlot.Series.LineSeries PaymentLineSeries { get; private set; }

            public ObservableCollection<CsvCalcResults> Results { get; private set; }

            public CalcResultGraph()
            {

            }
            public CalcResultGraph(List<CsvCalcResults> results)
            {

                Results = new ObservableCollection<CsvCalcResults>(results);

                // 線グラフ
                DepositLineSeries = new OxyPlot.Series.LineSeries();
                DepositLineSeries.Title = "入金";
                DepositLineSeries.ItemsSource = Results;
                //DepositLineSeries.ItemsSource = WindowManager.Instance.ResultItemsSource;
                DepositLineSeries.DataFieldX = nameof(CsvCalcResults.FromDate);
                //DepositLineSeries.DataFieldX.ActualLabels = 
                //DepositLineSeries.DataFieldX = nameof(CsvCalcResults.FromToDate);
                DepositLineSeries.DataFieldY = nameof(CsvCalcResults.Deposit);
                
                 // 線グラフ
                PaymentLineSeries = new OxyPlot.Series.LineSeries();
                PaymentLineSeries.Title = "出金";
                PaymentLineSeries.ItemsSource = Results;
                //PaymentLineSeries.ItemsSource = WindowManager.Instance.ResultItemsSource;
                PaymentLineSeries.DataFieldX = nameof(CsvCalcResults.FromDate);
                //PaymentLineSeries.DataFieldX = nameof(CsvCalcResults.FromToDate);
                PaymentLineSeries.DataFieldY = nameof(CsvCalcResults.Payment);
            }
        }

    }
}
