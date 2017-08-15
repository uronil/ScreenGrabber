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

        public void FirstLaunch() {
            //if (settings.fields.firstLoad) {
            //    Properties.Settings.Default.HotkeyScreen = new Hotkey("screen", new List<Key> { Key.C, Key.LeftCtrl, Key.LeftShift });
            //    Properties.Settings.Default.HotkeyScreenFast = new Hotkey("screenFast", new List<Key> { Key.X, Key.LeftCtrl, Key.LeftShift });
            //}
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            //Properties.Settings.Default.Save();
            writeSetting();
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

            if (!ScreenToClipboard.IsEnabled) {
                ScreenToClipboard.Text = string.Join<Key>("+", pressedKeys);
            }

            Console.Write("Pressed: " + string.Join<Key>("+", pressedKeys));
            Console.Write(" screen: " + screen.ToString() + " screenfast: " + screenFast.ToString());
            Console.WriteLine();
        }

        void KListener_KeyUp(object sender, RawKeyEventArgs e) {
            if (!ScreenToClipboard.IsEnabled) {
                ScreenToClipboard.Text = string.Join<Key>("+", pressedKeys);
                ScreenToClipboard.IsEnabled = true;
                screenFast.ChangeHotkey(pressedKeys);
                Console.WriteLine("!" + screenFast.ToString());
            }
            // Control pressed keys
            pressedKeys.Remove(e.Key);
            
        }

        public void Button_Click(object sender, RoutedEventArgs e) {
            OpenCaptureWindow(0);
        }
        
        private void ChangeSettings(object sender, RoutedEventArgs e) {
            writeSetting();
        }

        private void ScreenToClipboard_MouseDown(object sender, MouseButtonEventArgs e) {
            ScreenToClipboard.IsEnabled = false;
            ScreenToClipboard.Text = "Press keys combinations";
        }

        // Open CaptureWindow method
        public void OpenCaptureWindow(int settings) {
            CaptureWindow captureWindow = new CaptureWindow(settings);
            captureWindow.Show();
        }
    }
}
