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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace screengrab
{
    public partial class CaptureWindow : Window
    {
        double screenWidth, screenHeight, screenLeft, screenTop;
        Image img = new Image();

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
            
            img.Source = CopyScreen();
            canvas.Children.Add(img);
        }

        private BitmapSource CopyScreen() {
            var left = (int)screenLeft;
            var top = (int)screenTop;
            var width = (int)screenWidth;
            var height = (int)screenHeight;

            using (var screenBmp = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb)) {
                using (var bmpGraphics = System.Drawing.Graphics.FromImage(screenBmp)) {
                    bmpGraphics.CopyFromScreen(left, top, 0, 0, new System.Drawing.Size(width, height));
                    return Imaging.CreateBitmapSourceFromHBitmap(
                        screenBmp.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
        }

        Point currentPoint = new Point();
        Point firstPoint = new Point();

        private void MouseUp(object sender, MouseButtonEventArgs e) {
            first = false;
            this.Close();
        }

        bool first = false;

        private void MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(this);
            if (first == false && e.ButtonState == MouseButtonState.Pressed) {
                first = true;
                firstPoint = e.GetPosition(this);

                _rect = new Rectangle {
                    Stroke = Brushes.White,
                    StrokeThickness = 1,
                    Fill = new SolidColorBrush(Color.FromArgb(75, 255, 255, 255))
                };
                Canvas.SetLeft(_rect, firstPoint.X);
                Canvas.SetTop(_rect, firstPoint.Y);
                
                canvas.Children.Add(_rect);
            }
        }

        Rectangle _rect;

        private void MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                currentPoint = e.GetPosition(this);

                if (currentPoint.Y < 0 || currentPoint.X < 0 || currentPoint.Y > Height || currentPoint.X > Width) {
                    return;
                }

                var x = Math.Min(currentPoint.X, firstPoint.X);
                var y = Math.Min(currentPoint.Y, firstPoint.Y);

                var w = Math.Max(currentPoint.X, firstPoint.X) - x;
                var h = Math.Max(currentPoint.Y, firstPoint.Y) - y;

                _rect.Width = w;
                _rect.Height = h;

                if (w > 40 && h > 40) {
                    WidthTB.Text = w.ToString();
                    Canvas.SetTop(WidthPanel, y + 5);
                    Canvas.SetLeft(WidthPanel, x + w / 2 - 10);

                    HeightTB.Text = h.ToString();
                    Canvas.SetTop(HeightPanel, y + h / 2 - 10);
                    Canvas.SetLeft(HeightPanel, x + 5);
                } else {
                    WidthTB.Text = "";
                    HeightTB.Text = "";
                }



                Canvas.SetLeft(_rect, x);
                Canvas.SetTop(_rect, y);

            }
        }
    }
}
