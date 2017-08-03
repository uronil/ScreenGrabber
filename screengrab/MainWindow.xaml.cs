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
            screenFast = new Hotkey("screenFast", new List<Key> { Key.V, Key.LeftCtrl, Key.LeftShift });
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs e) {
            if (!pressedKeys.Contains(e.Key))
                pressedKeys.Add(e.Key);
            //link_profile.Content = string.Join<Key>(" + ", pressedKeys);
            if (screen.IsPressed(pressedKeys))
                link_profile.Content = screen.name;
            if (screenFast.IsPressed(pressedKeys))
                link_profile.Content = screenFast.name;
        }

        void KListener_KeyUp(object sender, RawKeyEventArgs e) {
            pressedKeys.Remove(e.Key);
        }

        public void Button_Click(object sender, RoutedEventArgs e) {

        }

    }
}
