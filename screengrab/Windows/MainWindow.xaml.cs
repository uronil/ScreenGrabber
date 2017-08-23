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

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public MainWindow() {
            InitializeComponent();
            SetSettings();
            Console.WriteLine(Properties.Settings.Default.LaunchCount);
        }
        
        public void SetSettings() {
            // First launch, set default settings
            if (Properties.Settings.Default.LaunchCount == 0) {
                Properties.Settings.Default.Hotkey = new Hotkey("screen", new List<Key> { Key.X, Key.LeftCtrl, Key.LeftShift });
                Properties.Settings.Default.HotkeyWithEdit = new Hotkey("screenFast", new List<Key> { Key.C, Key.LeftCtrl, Key.LeftShift });
                Properties.Settings.Default.LoadImagePath = System.AppDomain.CurrentDomain.BaseDirectory;
            }

            // Crutch (ебаный костыль - russian jargon)
            Properties.Settings.Default.CaptureWindowOpened = false;

            // Statistic
            Properties.Settings.Default.LaunchCount++;

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

            // Launch application minimized
            WindowState = WindowState.Minimized;
            this.Hide();
            
            // Create a tray icon
            trayIcon = new NotifyIcon();
            trayIcon.Text = "ScreenGrab";
            trayIcon.Icon = Properties.Resources.icon;

            // Add menu to tray icon and show it
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.MouseClick += TrayIcon_MouseClick;

            // Remove  when realize
            //Properties.Settings.Default.Save();
        }

        private void TrayIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                OpenCaptureWindow(0);
            if (e.Button == MouseButtons.Right) {
                this.Show();
                WindowState = WindowState.Normal;
            }   
        }

        // Minimize to system tray when applicaiton is minimized
        protected override void OnStateChanged(EventArgs e) {
            if (WindowState == WindowState.Minimized) this.Hide();
            base.OnStateChanged(e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Properties.Settings.Default.Save();
            System.Windows.Application.Current.Shutdown();
        }
        
        void KListener_KeyDown(object sender, RawKeyEventArgs e) {
            // Control pressed keys
            if (!pressedKeys.Contains(e.Key))
                pressedKeys.Add(e.Key);

            // Hotkey checking
            if (!setHotkeys) {
                if (Properties.Settings.Default.Hotkey.IsPressed(pressedKeys) && !Properties.Settings.Default.CaptureWindowOpened)
                    OpenCaptureWindow(1);
                if (Properties.Settings.Default.HotkeyWithEdit.IsPressed(pressedKeys) && !Properties.Settings.Default.CaptureWindowOpened)
                    OpenCaptureWindow(2);
            }
            
            if (!ScreenToClipboard.IsEnabled) {
                ScreenToClipboard.Text = string.Join<Key>(" + ", pressedKeys);
            }
            if (!ScreenWithEdit.IsEnabled) {
                ScreenWithEdit.Text = string.Join<Key>(" + ", pressedKeys);
            }
        }

        void KListener_KeyUp(object sender, RawKeyEventArgs e) {
            if (!ScreenToClipboard.IsEnabled) {
                ScreenToClipboard.IsEnabled = true;
                setHotkeys = false;
                ScreenToClipboard.BorderBrush = System.Windows.Media.Brushes.Gray;
                Properties.Settings.Default.Hotkey.ChangeHotkey(pressedKeys);
                ScreenToClipboard.Text = Properties.Settings.Default.Hotkey.ToString();
            }
            if (!ScreenWithEdit.IsEnabled) {
                ScreenWithEdit.IsEnabled = true;
                setHotkeys = false;
                ScreenWithEdit.BorderBrush = System.Windows.Media.Brushes.Gray;
                Properties.Settings.Default.HotkeyWithEdit.ChangeHotkey(pressedKeys);
                ScreenWithEdit.Text = Properties.Settings.Default.HotkeyWithEdit.ToString();
            }

            // Control pressed keys
            pressedKeys.Remove(e.Key);
        }

        public void Button_Click(object sender, RoutedEventArgs e) {
            OpenCaptureWindow(0);
        }

        bool setHotkeys = false;

        private void ScreenToTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            System.Windows.Controls.TextBox tb = sender as System.Windows.Controls.TextBox;
            tb.BorderBrush = System.Windows.Media.Brushes.Red;
            setHotkeys = true;
            tb.IsEnabled = false;
            tb.Text = "Press keys combinations";
        }

        // Open CaptureWindow method
        CaptureWindow captureWindow;
        public void OpenCaptureWindow(int settings) {
            captureWindow = new CaptureWindow(settings);
            Properties.Settings.Default.CaptureWindowOpened = true;
            captureWindow.Show();
        }

        private void LoadImagesToDiskCheckBox_Checked(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.LoadToDisk = (bool)LoadImagesToDiskCheckBox.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void ImageFormatComboBox_LostFocus(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.ImageFormat = ImageFormatComboBox.SelectedIndex + 1;
            Properties.Settings.Default.Save();
        }

        private void StartupCheckBox_Checked(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.Startup = (bool)StartupCheckBox.IsChecked;
            Properties.Settings.Default.Save();

            // Startup application setting, need windows registry
            if (Properties.Settings.Default.Startup) {
                Microsoft.Win32.RegistryKey Key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);
                Key.SetValue("screengrab", AppDomain.CurrentDomain.BaseDirectory + "screengrab.exe");
                Key.Close();
            } else {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.DeleteValue("screengrab", false);
                key.Close();
            }
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
