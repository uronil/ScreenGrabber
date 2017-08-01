using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace screengrab
{
    public partial class MainWindow : Window
    {
        // Variables for keyboard
        List<Key> pressedKeys; // Keyboard stack
        KeyboardListener KListener = new KeyboardListener();

        public MainWindow() {
            InitializeComponent();

            // Keyboard initialization
            pressedKeys = new List<Key>();
            KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
            KListener.KeyUp += new RawKeyEventHandler(KListener_KeyUp);
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs e) {
            if (!pressedKeys.Contains(e.Key))
                pressedKeys.Add(e.Key);
            link_website.Content = e.Key.ToString();
            link_profile.Content = string.Join<Key>(" + ", pressedKeys);
        }

        void KListener_KeyUp(object sender, RawKeyEventArgs e) {
            pressedKeys.Remove(e.Key);
            link_profile.Content = string.Join<Key>(" + ", pressedKeys);
        }
        
        private void Button_Click(object sender, RoutedEventArgs e) {
            
        }
        
    }
}
