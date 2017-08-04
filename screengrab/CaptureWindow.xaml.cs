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
using System.Windows.Shapes;

namespace screengrab
{
    public partial class CaptureWindow : Window
    {
        double screenWidth, screenHeight;
        
        public CaptureWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape)
                this.Close();
        }

        public CaptureWindow(int inf) {
            InitializeComponent();
            
            screenHeight = SystemParameters.PrimaryScreenHeight;
            screenWidth = SystemParameters.PrimaryScreenWidth;

            this.Height = screenHeight;
            this.Width = screenWidth;

            this.Top = 0;
            this.Left = 0;
        }
    }
}
