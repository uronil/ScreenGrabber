using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections;
using screengrab.Classes;

namespace screengrab
{
    public partial class MainWindow : Window
    {
        // Variables for keyboard
        List<Key> pressedKeys;
        KeyboardListener KListener = new KeyboardListener();

        // Hotkeys
        Hotkey screen, screenFast;

        Settings settings = new Settings();

        private void writeSetting() {
            settings.fields.loadImageToDisk = LoadImagesToDiskCheckBox.IsChecked.Value;
            settings.fields.imageFormat = ImageFormatComboBox.SelectedIndex;
            settings.fields.startup = StartupCheckBox.IsChecked.Value;
            settings.fields.openPictureInBrowser = OpenInBrowserCheckBox.IsChecked.Value;
            settings.WriteXml();
        }
        
        private void readSetting() {
            settings.ReadXml();
            LoadImagesToDiskCheckBox.IsChecked = settings.fields.loadImageToDisk;
            ImageFormatComboBox.SelectedIndex = settings.fields.imageFormat;
            StartupCheckBox.IsChecked = settings.fields.startup;
            OpenInBrowserCheckBox.IsChecked = settings.fields.openPictureInBrowser;
        }

        public MainWindow() {
            
            InitializeComponent();
            
            // Keyboard initialization
            pressedKeys = new List<Key>();
            KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
            KListener.KeyUp += new RawKeyEventHandler(KListener_KeyUp);

            // Hotkeys initialization
            screen = new Hotkey("screen", new List<Key> {Key.C, Key.LeftCtrl, Key.LeftShift });
            screenFast = new Hotkey("screenFast", new List<Key> { Key.X, Key.LeftCtrl, Key.LeftShift });

            readSetting();
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs e) {
            // Control pressed keys
            if (!pressedKeys.Contains(e.Key))
                pressedKeys.Add(e.Key);

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
        
        private void ChangeSettings(object sender, RoutedEventArgs e) {
            writeSetting();
        }
        
        // Open CaptureWindow method
        public void OpenCaptureWindow(int settings) {
            CaptureWindow captureWindow = new CaptureWindow(settings);
            captureWindow.Show();
        }
    }
}
