using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections;

namespace screengrab
{
    public partial class MainWindow : Window
    {
        // Variables for keyboard
        List<Key> pressedKeys;
        KeyboardListener KListener = new KeyboardListener();

        // Hotkeys
        Hotkey screen, screenFast;
        

        public MainWindow() {
            InitializeComponent();

            // Keyboard initialization
            pressedKeys = new List<Key>();
            KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
            KListener.KeyUp += new RawKeyEventHandler(KListener_KeyUp);

            // Hotkeys initialization
            screen = new Hotkey("screen", new List<Key> {Key.C, Key.LeftCtrl, Key.LeftShift });
            screenFast = new Hotkey("screenFast", new List<Key> { Key.X, Key.LeftCtrl, Key.LeftShift });
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs e) {
            // Control pressed keys
            if (!pressedKeys.Contains(e.Key))
                pressedKeys.Add(e.Key);
            //link_profile.Content = string.Join<Key>(" + ", pressedKeys);

            // Hotkey checking
            if (screen.IsPressed(pressedKeys))
                OpenCaptureWindow(1);
            if (screenFast.IsPressed(pressedKeys))
                OpenCaptureWindow(2);
        }

        void KListener_KeyUp(object sender, RawKeyEventArgs e) {
            // Control pressed keys
            pressedKeys.Remove(e.Key);
        }

        public void Button_Click(object sender, RoutedEventArgs e) {
            OpenCaptureWindow(0);
        }

        // Open CaptureWindow method
        public void OpenCaptureWindow(int settings) {
            CaptureWindow captureWindow = new CaptureWindow(settings);
            captureWindow.Show();
        }
    }
}
