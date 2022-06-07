using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace BezierCurve
{
	/// <summary>
	/// Curve Functions Names
	/// </summary>
	public enum CurveNames
	{
		/// <summary>
		/// Sinus curve, <see cref="MainWindow.Sinus"/>
		/// </summary>
		Sinus,
		/// <summary>
		/// Runge curve, <see cref="MainWindow.Runge"/>
		/// </summary>
		Runge,
		/// <summary>
		/// Arc curve, <see cref="MainWindow.Arc"/>
		/// </summary>
		Arc,
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		#region DependencyProperties
		#region Curve
		/// <summary>
		/// Identifies the Curve dependency property.
		/// </summary>
		public static readonly DependencyProperty CurveProperty
			= DependencyProperty.Register("Curve", typeof(CurveNames), typeof(MainWindow)
				, new FrameworkPropertyMetadata(CurveNames.Sinus
					, FrameworkPropertyMetadataOptions.AffectsMeasure
						| FrameworkPropertyMetadataOptions.AffectsRender)
				, validateCurve);
		/// <summary>
		/// Gets or sets the Curve property.
		/// </summary>
		/// <value>CurveNames value</value>
		public CurveNames Curve
		{
			get { return (CurveNames)GetValue(CurveProperty); }
			set { SetValue(CurveProperty, value); }
		}
		/// <summary>
		/// Validates the proposed Curve property value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private static bool validateCurve(object value)
		{
			CurveNames cName = (CurveNames)value;
			foreach (CurveNames item in Enum.GetValues(typeof(CurveNames)))
			{
				if (item == cName)
					return true;
			}
			return false;
		}
		#endregion Curve

		#region PointCount
		/// <summary>
		/// Identifies the PointCount dependency property.
		/// </summary>
		public static readonly DependencyProperty PointCountProperty
			= DependencyProperty.Register("PointCount", typeof(int), typeof(MainWindow)
				, new FrameworkPropertyMetadata(10
					, FrameworkPropertyMetadataOptions.AffectsMeasure
						| FrameworkPropertyMetadataOptions.AffectsRender)
				, validatePointCount);
		/// <summary>
		/// Gets or sets the PointCount property.
		/// </summary>
		/// <value>integer > 1</value>
		public int PointCount
		{
			get { return (int)GetValue(PointCountProperty); }
			set { SetValue(PointCountProperty, value); }
		}
		/// <summary>
		/// Validates the proposed PointCount property value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private static bool validatePointCount(object value)
		{
			int cnt = (int)value;
			return (cnt > 1 ? true : false);
		}
		#endregion PointCount

		#region ScaleX
		/// <summary>
		/// Identifies the ScaleX dependency property.
		/// </summary>
		public static readonly DependencyProperty ScaleXProperty
			= DependencyProperty.Register("ScaleX", typeof(double), typeof(MainWindow)
				, new FrameworkPropertyMetadata(100.0
					, FrameworkPropertyMetadataOptions.AffectsMeasure
						| FrameworkPropertyMetadataOptions.AffectsRender)
				, validateScaleX);
		/// <summary>
		/// Gets or sets the ScaleX property.
		/// </summary>
		/// <value>double >= 1</value>
		public double ScaleX
		{
			get { return (double)GetValue(ScaleXProperty); }
			set { SetValue(ScaleXProperty, value); }
		}
		/// <summary>
		/// Validates the proposed ScaleX property value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private static bool validateScaleX(object value)
		{
			double scale = (double)value;
			return (scale >= 1.0 ? true : false);
		}
		#endregion ScaleX

		#region ScaleY
		/// <summary>
		/// Identifies the ScaleY dependency property.
		/// </summary>
		public static readonly DependencyProperty ScaleYProperty
			= DependencyProperty.Register("ScaleY", typeof(double), typeof(MainWindow)
				, new FrameworkPropertyMetadata(100.0
					, FrameworkPropertyMetadataOptions.AffectsMeasure
						| FrameworkPropertyMetadataOptions.AffectsRender)
				, validateScaleY);
		/// <summary>
		/// Gets or sets the ScaleY property.
		/// </summary>
		/// <value>double >= 1</value>
		public double ScaleY
		{
			get { return (double)GetValue(ScaleYProperty); }
			set { SetValue(ScaleYProperty, value); }
		}
		/// <summary>
		/// Validates the proposed ScaleY property value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private static bool validateScaleY(object value)
		{
			double scale = (double)value;
			return (scale >= 1.0 ? true : false);
		}
		#endregion ScaleY
		#endregion DependencyProperties

		/// <summary>
		/// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
		/// </summary>
		/// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
		protected override void OnRender(DrawingContext drawingContext)
		{
			switch (Curve)
			{
				case CurveNames.Sinus:
					DrawCurve(Sinus);
					break;
				case CurveNames.Runge:
					DrawCurve(Runge);
					break;
				case CurveNames.Arc:
					DrawCurve(Arc);
					break;
			}
			base.OnRender(drawingContext);
		}

		/// <summary>
		/// Function points provider
		/// </summary>
		delegate Point[] Function();

		/// <summary>
		/// Draw the Curve.
		/// </summary>
		/// <param name="curve">The curve to draw.</param>
		void DrawCurve(Function curve)
		{
			canvas.Children.Clear();

			Point[] points = curve();
			if (points.Length < 2)
				return;

			const double markerSize = 5;
			// Draw Curve points (Black)
			for (int i = 0; i < points.Length; ++i)
			{
				Rectangle rect = new Rectangle()
				{
					Stroke = Brushes.Black,
					Fill = Brushes.Black,
					Height = markerSize,
					Width = markerSize
				};
				Canvas.SetLeft(rect, points[i].X - markerSize / 2);
				Canvas.SetTop(rect, points[i].Y - markerSize / 2);
				canvas.Children.Add(rect);
			}

			// Get Bezier Spline Control Points.
			Point[] cp1, cp2;
			ovp.BezierSpline.GetCurveControlPoints(points, out cp1, out cp2);

			// Draw curve by Bezier.
			PathSegmentCollection lines = new PathSegmentCollection();
			for (int i = 0; i < cp1.Length; ++i)
			{
				lines.Add(new BezierSegment(cp1[i], cp2[i], points[i + 1], true));
			}
			PathFigure f = new PathFigure(points[0], lines, false);
			PathGeometry g = new PathGeometry(new PathFigure[] { f });
			Path path = new Path() { Stroke = Brushes.Red, StrokeThickness = 1, Data = g };
			canvas.Children.Add(path);

			// Draw Bezier control points markers
			for (int i = 0; i < cp1.Length; ++i)
			{
				// First control point (Blue)
				Ellipse marker = new Ellipse()
				{
					Stroke = Brushes.Blue,
					Fill = Brushes.Blue,
					Height = markerSize,
					Width = markerSize
				};
				Canvas.SetLeft(marker, cp1[i].X - markerSize / 2);
				Canvas.SetTop(marker, cp1[i].Y - markerSize / 2);
				canvas.Children.Add(marker);

				// Second control point (Green)
				marker = new Ellipse()
				{
					Stroke = Brushes.Green,
					Fill = Brushes.Green,
					Height = markerSize,
					Width = markerSize
				};
				Canvas.SetLeft(marker, cp2[i].X - markerSize / 2);
				Canvas.SetTop(marker, cp2[i].Y - markerSize / 2);
				canvas.Children.Add(marker);
			}

			// Print points
			//Trace.WriteLine(string.Format("Start=({0})", points[0]));
			//for (int i = 0; i < cp1.Length; ++i)
			//{
			//    Trace.WriteLine(string.Format("CP1=({0}) CP2=({1}) Stop=({2})"
			//        , cp1[i], cp2[i], points[i + 1]));
			//}
		}

		#region Curves
		/// <summary>
		/// Sinus points in [0,2PI].
		/// </summary>
		/// <returns></returns>
		Point[] Sinus()
		{
			// Fill point array with scaled in X,Y Sin values in [0, PI].
			Point[] points = new Point[PointCount];
			double step = 2 * Math.PI / (PointCount - 1);
			for (int i = 0; i < PointCount; ++i)
			{
				points[i] = new Point(ScaleX * i * step, ScaleY * (1 - Math.Sin(i * step)));
			}
			return points;
		}

		/// <summary>
		/// Runge function points in [-1,1].
		/// </summary>
		/// <returns></returns>
		Point[] Runge()
		{
			// Fill point array with scaled in X,Y Runge (1 / (1 + 25 * x * x)) values in [-1, 1].
			Point[] points = new Point[PointCount];
			double step = 2.0 / (PointCount - 1);
			for (int i = 0; i < PointCount; ++i)
			{
				double x = -1 + i * step;
				points[i] = new Point(ScaleX * (x + 1), ScaleY * (1 - 1 / (1 + 25 * x * x)));
			}
			return points;
		}

		/// <summary>
		/// Arc curve points in [0º,270º], radius=1.
		/// </summary>
		/// <returns></returns>
		Point[] Arc()
		{
			// Fill point array with scaled in X,Y Arc values in [0º,270º].
			Point[] points = new Point[PointCount];
			double step = 1.5 * Math.PI / (PointCount - 1);
			for (int i = 0; i < PointCount; ++i)
			{
				double x = i * step;
				points[i] = new Point(ScaleX * (1 + Math.Cos(x)), ScaleY * (1 + Math.Sin(x)));
			}
			return points;
		}
		#endregion Curves
	}

	/// <summary>
	/// ValidationRule class to validate that a value is a number >= 1.
	/// </summary>
	public class ScaleRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string? stringValue = value as string;
			if (!string.IsNullOrEmpty(stringValue))
			{
                if (double.TryParse(stringValue, out double doubleValue))
                {
                    if (doubleValue >= 1)
                        return new ValidationResult(true, null);
                }
            }
			return new ValidationResult(false, "Must be a number greater or equal to 1");
		}
	}
}
