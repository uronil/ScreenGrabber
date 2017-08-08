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
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace screengrab
{
    public partial class CaptureWindow : Window
    {
        double screenWidth, screenHeight, screenLeft, screenTop;

        // Close window on Escape click
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape)
                this.Close();
        }

        public CaptureWindow(int inf) {
            InitializeComponent();

            screenLeft = SystemParameters.VirtualScreenLeft;
            screenTop = SystemParameters.VirtualScreenTop;
            screenWidth = SystemParameters.VirtualScreenWidth;
            screenHeight = SystemParameters.VirtualScreenHeight;

            this.Height = screenHeight;
            this.Width = screenWidth;

            this.Top = 0;
            this.Left = 0;

            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Source = CopyScreen();
            canvas.Children.Add(img);
        }

        private BitmapSource CopyScreen() {
            var left = (int)screenLeft;
            var top = (int)screenTop;
            var width = (int)screenWidth;
            var height = (int)screenHeight;

            using (var screenBmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb)) {
                using (var bmpGraphics = Graphics.FromImage(screenBmp)) {
                    bmpGraphics.CopyFromScreen(left, top, 0, 0, new System.Drawing.Size(width, height));
                    return Imaging.CreateBitmapSourceFromHBitmap(
                        screenBmp.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
        }

        System.Windows.Point currentPoint = new System.Windows.Point();
        System.Windows.Point firstPoint = new System.Windows.Point();

        private void MouseUp(object sender, MouseButtonEventArgs e) {
            first = false;
        }

        bool first = false;

        private void MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(this);
            if (first == false && e.ButtonState == MouseButtonState.Pressed) {
                first = true;
                firstPoint = e.GetPosition(this);
            }
        }

        private void MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                Line line = new Line();

                System.Windows.Shapes.Rectangle myRect = new System.Windows.Shapes.Rectangle();
                myRect.Stroke = System.Windows.Media.Brushes.White;
                myRect.Fill = System.Windows.Media.Brushes.SkyBlue;
                myRect.HorizontalAlignment = HorizontalAlignment.Left;
                myRect.VerticalAlignment = VerticalAlignment.Center;
                myRect.Height = firstPoint.X;
                myRect.Width = firstPoint.Y;
                canvas.Children.Add(myRect);

                line.Stroke = System.Windows.SystemColors.WindowFrameBrush;
                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y;

                currentPoint = e.GetPosition(this);

                canvas.Children.Add(line);
            }
        }
    }
}
