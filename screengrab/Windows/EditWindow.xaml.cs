using Microsoft.Win32;
using screengrab.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace screengrab.Windows
{
    public partial class EditWindow : Window
    {
        PaintType paintType;

        public EditWindow(Image image) { 
            InitializeComponent();

            editCanvas.Children.Add(image);

            ConfigurateWindowSize(image);
            paintType = PaintType.Pencil;
            ButtonPaintPen.Background = Brushes.White;
            ButtonPaintRect.Background = Brushes.LightGray;
        }

        void ConfigurateWindowSize(Image image) {
            // Added pixels for windows border and fields
            int addedHeight = 80, addedWidth = 58;
            if (image.Source.Height < 175) {
                this.Height = 175 + addedHeight;
            } else if (image.Source.Height > 700) {
                this.Height = 700 + addedHeight;
            } else {
                this.Height = image.Source.Height + addedHeight;
            }

            if (image.Source.Width < 270) {
                this.Width = 270 + addedWidth;
            } else if (image.Source.Width > 1000) {
                this.Width = 1000 + addedWidth;
            } else {
                this.Width = image.Source.Width + addedWidth;
            }

            this.MinHeight = 175 + addedHeight;
            this.MinWidth = 270 + addedWidth;

            editCanvas.Height = image.Source.Height;
            editCanvas.Width = image.Source.Width;
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e) {
            ImageConverter.CopyToClipboard(editCanvas, Properties.Settings.Default.ImageFormat);
            this.Close();
        }

        private void SaveImageAs_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "image"; // Default file name
            saveFileDialog.DefaultExt = ".png"; // Default file extension
            saveFileDialog.Filter = "Png (*.png)|*.png|JPG(*.jpg)|*.jpg|BMP (*.bmp)|*.bmp";
            if (saveFileDialog.ShowDialog() == true) {
                ImageConverter.SaveImageTo(
                    saveFileDialog.FilterIndex,
                    saveFileDialog.FileName,
                    editCanvas);
            }
        }

        // For painting by pencil
        Point currentPoint = new Point();
        bool first = false;

        // For painting by rectangle
        Point firstPoint = new Point();
        Rectangle rect;

        public enum PaintType {
            Pencil,
            Rect
        }
        
        private void editCanvas_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ButtonState == MouseButtonState.Pressed) {
                currentPoint = e.GetPosition(editCanvas);
            }
            switch (paintType) {
                case PaintType.Pencil:
                    first = false;
                    break;

                case PaintType.Rect:
                    firstPoint = e.GetPosition(editCanvas);

                    rect = new Rectangle() {
                        StrokeThickness = 2,
                        Stroke = Brushes.Red
                    };

                    Canvas.SetLeft(rect, firstPoint.X);
                    Canvas.SetTop(rect, firstPoint.Y);

                    editCanvas.Children.Add(rect);
                    break;
            }
        }
        
        private void editCanvas_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                switch(paintType) {
                    case PaintType.Pencil:
                        
                        Line line = new Line();
                        
                        line.Stroke = new SolidColorBrush(Colors.Red);
                        line.StrokeThickness = 3;
                        line.X1 = currentPoint.X;
                        line.Y1 = currentPoint.Y;
                        line.X2 = e.GetPosition(editCanvas).X;
                        line.Y2 = e.GetPosition(editCanvas).Y;
                        
                        // Straight line (HOLD SHIFT)
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) {

                            if (!first) {
                                firstPoint = e.GetPosition(editCanvas);
                                first = true;
                            }
                                
                            line.Y1 = firstPoint.Y;
                            line.Y2 = firstPoint.Y;

                        } else {
                            first = false;
                        }

                        currentPoint = e.GetPosition(editCanvas);

                        editCanvas.Children.Add(line);

                        break;

                    case PaintType.Rect:

                        if (e.LeftButton == MouseButtonState.Released || rect == null)
                            return;

                        currentPoint = e.GetPosition(editCanvas);

                        var x = Math.Min(currentPoint.X, firstPoint.X);
                        var y = Math.Min(currentPoint.Y, firstPoint.Y);

                        var w = Math.Max(currentPoint.X, firstPoint.X) - x;
                        var h = Math.Max(currentPoint.Y, firstPoint.Y) - y;
                        
                        rect.Width = w;
                        rect.Height = h;

                        Canvas.SetLeft(rect, x);
                        Canvas.SetTop(rect, y);

                        break;
                }
            }
        }

        private void editCanvas_MouseUp(object sender, MouseButtonEventArgs e) {
            rect = null;
        }

        private void ButtonPaintPen_Click(object sender, RoutedEventArgs e) {
            paintType = PaintType.Pencil;
            ButtonPaintPen.Background = Brushes.White;
            ButtonPaintRect.Background = Brushes.LightGray;
            editCanvas.Cursor = Cursors.Pen;
        }

        private void ButtonPaintRect_Click(object sender, RoutedEventArgs e) {
            paintType = PaintType.Rect;
            ButtonPaintPen.Background = Brushes.LightGray;
            ButtonPaintRect.Background = Brushes.White;
            editCanvas.Cursor = Cursors.Cross;
        }
    }
}
