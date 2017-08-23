using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace screengrab.Classes {
    static class ImageConverter {

        static public void SaveImageTo(int imageFormat, string fileName, Canvas canvas) {
            BitmapEncoder enc = GetImage(canvas, imageFormat);
            using (var stm = System.IO.File.Create(fileName)) {
                enc.Save(stm); // Saving image
            }
        }

        static public string ImageFormat(int imageformat) {
            switch (imageformat) {
                case 1: // PNG
                    return ".png";
                    break;
                case 2: // JPG
                    return ".jpg";
                    break;
                case 3: // BMP
                    return ".bmp";
                    break;
            }
            return "";
        }

        static public void SaveImageTo(int imageFormat, string fileName, Image image) {
            BitmapEncoder enc = GetImage(image, imageFormat);
            using (var stm = System.IO.File.Create(fileName)) {
                enc.Save(stm); // Saving image
            }
        }

        static public void CopyToClipboard(Canvas canvas, int imageFormat) {
            Clipboard.SetImage(GetImage(canvas, imageFormat).Frames[0]);
        }

        static public void CopyToClipboard(Image image, int imageFormat) {
            Clipboard.SetImage(GetImage(image, imageFormat).Frames[0]);
        }

        static public BitmapEncoder GetImage(Canvas surface, int format) {

            // Get the size of canvas
            Size size = new Size(surface.Width, surface.Height);

            var scale = 1;//100/96d;
            surface.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var sz = surface.DesiredSize;
            var rect = new Rect(sz);
            surface.Arrange(rect);
            var bmp = new RenderTargetBitmap((int)(scale * (rect.Width)),
                                             (int)(scale * (rect.Height)),
                                              scale * 96,
                                              scale * 96,
                                              PixelFormats.Default);
            bmp.Render(surface);

            BitmapEncoder enc = null;
            switch (format) {
                case 1: // PNG
                    enc = new PngBitmapEncoder();
                    break;
                case 2: // JPG
                    enc = new JpegBitmapEncoder();
                    break;
                case 3: // BMP
                    enc = new BmpBitmapEncoder();
                    break;
            }

            enc.Frames.Add(BitmapFrame.Create(bmp));
            return enc;
        }

        static public BitmapEncoder GetImage(Image surface, int format) {

            // Get the size of canvas
            Size size = new Size(surface.Width, surface.Height);

            var scale = 1;//100/96d;
            surface.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var sz = surface.DesiredSize;
            var rect = new Rect(sz);
            surface.Arrange(rect);
            var bmp = new RenderTargetBitmap((int)(scale * (rect.Width)),
                                             (int)(scale * (rect.Height)),
                                              scale * 96,
                                              scale * 96,
                                              PixelFormats.Default);
            bmp.Render(surface);

            BitmapEncoder enc = null;
            switch (format) {
                case 1: // PNG
                    enc = new PngBitmapEncoder();
                    break;
                case 2: // JPG
                    enc = new JpegBitmapEncoder();
                    break;
                case 3: // BMP
                    enc = new BmpBitmapEncoder();
                    break;
            }

            enc.Frames.Add(BitmapFrame.Create(bmp));
            return enc;
        }
    }
}
