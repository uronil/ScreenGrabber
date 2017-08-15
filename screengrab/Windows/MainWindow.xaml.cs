using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections;
using screengrab.Classes;
using System.Windows.Forms;
using System.IO;

namespace screengrab
{
    public partial class MainWindow : Window
    {
        // Variables for keyboard
        List<Key> pressedKeys;
        KeyboardListener KListener = new KeyboardListener();

        public void SetSettings() {
            // First launch, set default settings
            if (Properties.Settings.Default.LaunchCount == 0) {
                Properties.Settings.Default.Hotkey = new Hotkey("screen", new List<Key> { Key.X, Key.LeftCtrl, Key.LeftShift });
                Properties.Settings.Default.HotkeyWithEdit = new Hotkey("screenFast", new List<Key> { Key.C, Key.LeftCtrl, Key.LeftShift });
                Properties.Settings.Default.LoadImagePath = System.AppDomain.CurrentDomain.BaseDirectory;
            }

            // Set settings to elements
            ScreenToClipboard.Text = Properties.Settings.Default.Hotkey.ToString();
            ScreenWithEdit.Text = Properties.Settings.Default.HotkeyWithEdit.ToString();

            LoadImagesToDiskCheckBox.IsChecked = Properties.Settings.Default.LoadToDisk;
            LoadImagePathTextBox.Text = Properties.Settings.Default.LoadImagePath;
            ImageFormatComboBox.SelectedIndex = Properties.Settings.Default.ImageFormat;
            StartupCheckBox.IsChecked = Properties.Settings.Default.Startup;

            // Keyboard initialization
            pressedKeys = new List<Key>();
            KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
            KListener.KeyUp += new RawKeyEventHandler(KListener_KeyUp);
        }

        public MainWindow() {
            InitializeComponent();
            SetSettings();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Properties.Settings.Default.Save();
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs e) {
            // Control pressed keys
            if (!pressedKeys.Contains(e.Key))
                pressedKeys.Add(e.Key);

            // Hotkey checking
            if (Properties.Settings.Default.Hotkey.IsPressed(pressedKeys))
                OpenCaptureWindow(1);
            if (Properties.Settings.Default.HotkeyWithEdit.IsPressed(pressedKeys))
                OpenCaptureWindow(2);

            if (!ScreenToClipboard.IsEnabled) {
                ScreenToClipboard.Text = string.Join<Key>("+", pressedKeys);
            }
            if (!ScreenWithEdit.IsEnabled) {
                ScreenWithEdit.Text = string.Join<Key>("+", pressedKeys);
            }
        }

        void KListener_KeyUp(object sender, RawKeyEventArgs e) {
            if (!ScreenToClipboard.IsEnabled) {
                ScreenToClipboard.Text = string.Join<Key>("+", pressedKeys);
                ScreenToClipboard.IsEnabled = true;
                ScreenToClipboard.BorderBrush = System.Windows.Media.Brushes.Blue;
                Properties.Settings.Default.Hotkey.ChangeHotkey(pressedKeys);
            }
            if (!ScreenWithEdit.IsEnabled) {
                ScreenWithEdit.Text = string.Join<Key>("+", pressedKeys);
                ScreenWithEdit.IsEnabled = true;
                ScreenWithEdit.BorderBrush = System.Windows.Media.Brushes.Blue;
                Properties.Settings.Default.HotkeyWithEdit.ChangeHotkey(pressedKeys);
            }

            // Control pressed keys
            pressedKeys.Remove(e.Key);
        }

        public void Button_Click(object sender, RoutedEventArgs e) {
            OpenCaptureWindow(0);
        }
        
        private void ScreenToClipboard_MouseDown(object sender, MouseButtonEventArgs e) {
            ScreenToClipboard.BorderBrush = System.Windows.Media.Brushes.Red;
            ScreenToClipboard.IsEnabled = false;
            ScreenToClipboard.Text = "Press keys combinations";
        }

        private void ScreenWithEdit_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            ScreenWithEdit.BorderBrush = System.Windows.Media.Brushes.Red;
            ScreenWithEdit.IsEnabled = false;
            ScreenWithEdit.Text = "Press keys combinations";
        }

        // Open CaptureWindow method
        public void OpenCaptureWindow(int settings) {
            CaptureWindow captureWindow = new CaptureWindow(settings);
            captureWindow.Show();
        }

        private void LoadImagesToDiskCheckBox_Checked(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.LoadToDisk = (bool)LoadImagesToDiskCheckBox.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void ImageFormatComboBox_LostFocus(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.ImageFormat = ImageFormatComboBox.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void StartupCheckBox_Checked(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.Startup = (bool)StartupCheckBox.IsChecked;
            Properties.Settings.Default.Save();
        }
        
        private void ButtonImagePath_Click(object sender, RoutedEventArgs e) {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath)) {
                string path = folderBrowser.SelectedPath + "\\";
                LoadImagePathTextBox.Text = path;
                Properties.Settings.Default.LoadImagePath = path;
            }
        }
    }
}
