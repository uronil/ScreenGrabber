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
using System.Drawing;

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
            
            //using (Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight)) {
            //    using (Graphics g = Graphics.FromImage(bmp)) {
            //        String filename = "ScreenCapture-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
            //        Opacity = .0;
            //        g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
            //        bmp.Save(@"C:/Screenshots/" + filename);
            //        Opacity = 1;
                    
            //    }
            //}
        }

    }
}
