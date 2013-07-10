using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace WpfTools.Controls
{
    /// <summary>
    /// Логика взаимодействия для FlipBoard.xaml
    /// </summary>
    public partial class FlipBoard : UserControl
    {
        [System.ComponentModel.Category("Кисти")]
        /// <summary>
        /// Кисть залики плитки под надписью
        /// </summary>
        public Brush TileBrush
        {
            get { return (Brush)GetValue(TileBrushProperty); }
            set { SetValue(TileBrushProperty, value); }
        }
        public static readonly DependencyProperty TileBrushProperty = DependencyProperty.Register("TileBrush", typeof(Brush), typeof(FlipBoard), new UIPropertyMetadata());

        /// <summary>
        /// Размер полей вокруг
        /// </summary>
        public Thickness TileMargin
        {
            get { return (Thickness)GetValue(TileMarginProperty); }
            set { SetValue(TileMarginProperty, value); }
        }
        public static readonly DependencyProperty TileMarginProperty = DependencyProperty.Register("TileMargin", typeof(Thickness), typeof(FlipBoard), new UIPropertyMetadata(new Thickness(10,0,10,0)));

        /// <summary>
        /// Угол обзора камеры
        /// </summary>
        public double CameraViewAngle
        {
            get { return (double)GetValue(CameraViewAngleProperty); }
            set { SetValue(CameraViewAngleProperty, value); }
        }
        public static readonly DependencyProperty CameraViewAngleProperty = DependencyProperty.Register("CameraViewAngle", typeof(double), typeof(FlipBoard), new UIPropertyMetadata(45.0));

        

        public int MyProperty { get; set; }
        

        public FlipBoard()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ResizeMesh();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeMesh();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((e.NewValue ?? "").ToString() != (e.OldValue ?? "").ToString())
                BeginFlip();
        }
        void FlipOut_Completed(object sender, EventArgs e)
        {
            ContinuneFlip();
        }

        private void ResizeMesh()
        {
            double w = ContentPresenter.ActualWidth;
            double h = ContentPresenter.ActualHeight;
            mesh.Positions = new System.Windows.Media.Media3D.Point3DCollection(
                new Point3D[]
                {
                    new Point3D(-w/2,  h/2, 0),
                    new Point3D( w/2,  h/2, 0),
                    new Point3D( w/2, -h/2, 0),
                    new Point3D(-w/2, -h/2, 0)
                });

            double d = Math.Max(
                ((w + TileMargin.Left + TileMargin.Right) / 2) / Math.Tan(0.5 * CameraViewAngle * Math.PI / 180),
                ((h + TileMargin.Top + TileMargin.Bottom) / 2) / Math.Tan(0.5 * CameraViewAngle * Math.PI / 180));
            camera.Position = new Point3D(0, 0, d);
            camera.FieldOfView = CameraViewAngle;
        }

        PowerEase EaseIn = new PowerEase() { EasingMode = EasingMode.EaseIn };
        PowerEase EaseOut = new PowerEase() { EasingMode = EasingMode.EaseOut };
        private void BeginFlip()
        {
            OriginalContent.Opacity = 0;
            ThreeDContent.Opacity = 1;
            var anim = new DoubleAnimation(0, 90, TimeSpan.FromMilliseconds(500)) { EasingFunction = EaseIn };
            anim.Completed += new EventHandler(FlipOut_Completed);
            Rotator.BeginAnimation(AxisAngleRotation3D.AngleProperty, anim);
        }
        private void ContinuneFlip()
        {
            label.Text = this.DataContext.ToString();
            var anim = new DoubleAnimation(-90, 0, TimeSpan.FromMilliseconds(500)) { EasingFunction = EaseOut };
            anim.Completed += new EventHandler(FlipIn_Completed);
            Rotator.BeginAnimation(AxisAngleRotation3D.AngleProperty, anim);
        }

        void FlipIn_Completed(object sender, EventArgs e)
        {
            OriginalContent.Opacity = 1;
            ThreeDContent.Opacity = 0;
        }
    }
}
