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
        List<Key> _pressedKeys;
        //private LowLevelKeyboardListener _listener;

        KeyboardListener KListener = new KeyboardListener();

        public MainWindow() {
            InitializeComponent();
            //HotKey _hotKey = new HotKey(Key.C, KeyModifier.Ctrl | KeyModifier.Shift , OnHotKeyHandler);

            _pressedKeys = new List<Key>();

            //KBDHook.KeyDown += new KBDHook.HookKeyPress(KBDHook_KeyDown);
            //KBDHook.KeyUp += new KBDHook.HookKeyPress(KBDHook_KeyUp);
            //KBDHook.LocalHook = false;
            //KBDHook.InstallHook();
            //this.Closed += (s, e) => {
            //    KBDHook.UnInstallHook();
            //};

            KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
            KListener.KeyUp += new RawKeyEventHandler(KListener_KeyUp);

            //_listener = new LowLevelKeyboardListener();
            //_listener.OnKeyPressed += _listener_OnKeyPressed;
            //_listener.OnKeyPressed += _listener_OnKeyUp;

            //_listener.HookKeyboard();
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs e) {
            if (!_pressedKeys.Contains(e.Key))
                _pressedKeys.Add(e.Key);
            link_website.Content = e.Key.ToString();
            link_profile.Content = string.Join<Key>(" + ", _pressedKeys);
        }

        void KListener_KeyUp(object sender, RawKeyEventArgs e) {
            _pressedKeys.Remove(e.Key);
            link_profile.Content = string.Join<Key>(" + ", _pressedKeys);
        }

        //void _listener_OnKeyUp(object sender, KeyPressedArgs e) {
        //    link_profile.Content = e.KeyPressed.ToString();

        //}

        //void _listener_OnKeyPressed(object sender, KeyPressedArgs e) {
        //    if (!_pressedKeys.Contains(e.Keys))
        //                _pressedKeys.Add(e.Keys);
        //           link_profile.Content = string.Join<Key>(" + ", _pressedKeys);
        //        _listener.UnHookKeyboard();
        //    _listener.HookKeyboard();
        //}

        void KBDHook_KeyUp(LLKHEventArgs e) {
            _pressedKeys.Remove(e.Keys);
            link_profile.Content = string.Join<Key>(" + ", _pressedKeys);
        }

        void KBDHook_KeyDown(LLKHEventArgs e) {
            if (!_pressedKeys.Contains(e.Keys))
                _pressedKeys.Add(e.Keys);
            link_website.Content = e.Keys.ToString();
            link_profile.Content = string.Join<Key>(" + ", _pressedKeys);
        }


        private void Button_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("lloll");
        }

        //private void OnHotKeyHandler(HotKey hotKey) {
        //    //link_profile.Content = hotKey.Key.ToString() + " " + hotKey.KeyModifiers.ToString();
        //}
        

    }
}
