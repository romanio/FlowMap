using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlowMapApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;

        public enum DWM_WINDOW_CORNER_PREFERENCE
        {
            DWMWCP_DEFAULT = 0,
            DWMWCP_DONOTROUND = 1,
            DWMWCP_ROUND = 2,
            DWMWCP_ROUNDSMALL = 3
        }

        private static void UseWindowAttributes(IntPtr handle)
        {
            if (IsWindows10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985))
                {
                    attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                }

                int corner = (int)DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_DONOTROUND;
                int darkmode = 1;

                DwmSetWindowAttribute(handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref corner, sizeof(int));
                DwmSetWindowAttribute(handle, (int)attribute, ref darkmode, sizeof(int));
            }
        }

        private static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }

        public PlotModel plotModel
        {
            get => _plotmodel;
        }
        
        OxyPlot.PlotModel _plotmodel = new OxyPlot.PlotModel();


        public double VISL
        {
            get => fm.VISL;
            set => fm.VISL = value;
        }

        public double VISG
        {
            get => fm.VISG;
            set => fm.VISG = value;
        }

        public double DENG
        {
            get => fm.DENG;
            set => fm.DENG = value;
        }

        public double DENL
        {
            get => fm.DENL;
            set => fm.DENL = value;
        }

        public double DIA
        {
            get => fm.DIA;
            set => fm.DIA = value;
        }

        public double ANGLE
        {
            get => fm.ANGLE;
            set => fm.ANGLE = value;
        }


        public MainWindow()
        {
            InitializeComponent();

            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
            UseWindowAttributes(hWnd);
            DataContext = this;

            _plotmodel = new PlotModel
            {
                Title = "Horizontal Flow Map",
                TitleFontSize = 12,
                DefaultFontSize = 11,
                
            };

            _plotmodel.Axes.Add(new OxyPlot.Axes.LogarithmicAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Dash,
                Minimum = 0.03,
                Maximum = 100,
                Title = "UGS, m/s",
                MajorGridlineThickness = 1

            });

            _plotmodel.Axes.Add(new OxyPlot.Axes.LogarithmicAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Dash,
                Title = "ULS, m/s",
                Minimum = 0.003,
                Maximum = 10,
                MajorGridlineThickness = 1,

            });

            _plotmodel.Series.Add(new LineSeries { });
            _plotmodel.Series.Add(new LineSeries { Color = OxyColors.Black, StrokeThickness = 2 });
            _plotmodel.Series.Add(new LineSeries { Color = OxyColors.Black, StrokeThickness = 2 });
            _plotmodel.Series.Add(new LineSeries { Color = OxyColors.Black, StrokeThickness = 2 });
            _plotmodel.Series.Add(new LineSeries { Color = OxyColors.Black, StrokeThickness = 2 });
            _plotmodel.Series.Add(new LineSeries { Color = OxyColors.Black, StrokeThickness = 2 });
            _plotmodel.Series.Add(new LineSeries { Color = OxyColors.Black, StrokeThickness = 2 });

            _plotmodel.Series.Add(new LineSeries {});
            _plotmodel.Series.Add(new LineSeries {});
            _plotmodel.Series.Add(new LineSeries{ });

            _plotmodel.Legends.Add(new OxyPlot.Legends.Legend { LegendPosition = OxyPlot.Legends.LegendPosition.LeftTop });


            ((LineSeries)_plotmodel.Series[1]).Points.Add(new DataPoint(10.668, 0.003048));
            ((LineSeries)_plotmodel.Series[1]).Points.Add(new DataPoint(4.2672, 0.03048));
            ((LineSeries)_plotmodel.Series[1]).Points.Add(new DataPoint(3.2004, 0.06096));
            ((LineSeries)_plotmodel.Series[1]).Points.Add(new DataPoint(0.762, 0.35052));
            ((LineSeries)_plotmodel.Series[1]).Points.Add(new DataPoint(0.762, 1.46304));
            ((LineSeries)_plotmodel.Series[1]).Points.Add(new DataPoint(0.9906, 4.2672));

            ((LineSeries)_plotmodel.Series[2]).Points.Add(new DataPoint(0.03048, 0.1524));
            ((LineSeries)_plotmodel.Series[2]).Points.Add(new DataPoint(1.50208, 0.1524));

            ((LineSeries)_plotmodel.Series[3]).Points.Add(new DataPoint(21.336, 0.003048));
            ((LineSeries)_plotmodel.Series[3]).Points.Add(new DataPoint(18.288, 0.03048));
            ((LineSeries)_plotmodel.Series[3]).Points.Add(new DataPoint(11.5824, 0.09144));
            ((LineSeries)_plotmodel.Series[3]).Points.Add(new DataPoint(12.192, 0.170688));
            ((LineSeries)_plotmodel.Series[3]).Points.Add(new DataPoint(15.24, 0.3048));
            ((LineSeries)_plotmodel.Series[3]).Points.Add(new DataPoint(30.48, 0.762));
            ((LineSeries)_plotmodel.Series[3]).Points.Add(new DataPoint(70.136, 4.2672));

            ((LineSeries)_plotmodel.Series[4]).Points.Add(new DataPoint(2.286, 0.09144));
            ((LineSeries)_plotmodel.Series[4]).Points.Add(new DataPoint(11.5824, 0.09144));

            ((LineSeries)_plotmodel.Series[5]).Points.Add(new DataPoint(0.03048, 4.2672));
            ((LineSeries)_plotmodel.Series[5]).Points.Add(new DataPoint(70.136, 4.2672));

            ((LineSeries)_plotmodel.Series[6]).Points.Add(new DataPoint(70.136, 4.2672));
            ((LineSeries)_plotmodel.Series[6]).Points.Add(new DataPoint(81.9912, 9.144));

 

            _plotmodel.Annotations.Add(new TextAnnotation{ TextPosition = new DataPoint(0.1, 0.02), Text = "STRATIFIED\nFLOW (SS)", StrokeThickness = 0});
            _plotmodel.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(8.5, 0.02), Text = "WAVE\nFLOW (SW)", StrokeThickness = 0 });
            _plotmodel.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(0.1, 0.6), Text = "ELONGATED\nBUBBLE\nFLOW (I)", StrokeThickness = 0 });
            _plotmodel.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(5, 0.75), Text = "SLUG\nFLOW (I)", StrokeThickness = 0 });
            _plotmodel.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(0.1, 6), Text = "DISPERSED FLOW (DB)", StrokeThickness = 0 });
            _plotmodel.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(50,0.1), Text = "ANNULAR-\nANNULAR\nMIST FLOW (AD)", StrokeThickness = 0 });
            _plotmodel.InvalidatePlot(true);

        }

        FlowMap fm = new FlowMap();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((LineSeries)_plotmodel.Series[0]).Points.Clear();
            ((LineSeries)_plotmodel.Series[7]).Points.Clear();
            ((LineSeries)_plotmodel.Series[8]).Points.Clear();
            ((LineSeries)_plotmodel.Series[9]).Points.Clear();


            fm.Generate();

            for (int iw = 0; iw < fm.SS.Count; ++iw)
            {
                ((LineSeries)_plotmodel.Series[0]).Points.Add(new DataPoint(fm.SS[iw].x, fm.SS[iw].y));
      
            }

            for (int iw = 0; iw < fm.DB.Count; ++iw)
            {
                ((LineSeries)_plotmodel.Series[7]).Points.Add(new DataPoint(fm.DB[iw].x, fm.DB[iw].y));
            }

                for (int iw = 0; iw < fm.SW.Count; ++iw)
            {
                ((LineSeries)_plotmodel.Series[8]).Points.Add(new DataPoint(fm.SW[iw].x, fm.SW[iw].y));
            }

            for (int iw = 0; iw < fm.AD.Count; ++iw)
            {
                ((LineSeries)_plotmodel.Series[9]).Points.Add(new DataPoint(fm.AD[iw].x, fm.AD[iw].y));
            }

            _plotmodel.InvalidatePlot(true);
        }
    }
}