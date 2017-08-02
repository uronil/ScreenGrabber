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
            link_profile.Content = string.Join<Key>(" + ", pressedKeys);
        }

        void KListener_KeyUp(object sender, RawKeyEventArgs e) {
            pressedKeys.Remove(e.Key);
        }

        public void Button_Click(object sender, RoutedEventArgs e) {

        }

    }
}
