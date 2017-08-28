using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace screengrab.Classes {
    public class Tray {
        public static System.Windows.Forms.NotifyIcon trayIcon;
        public static string projectName = "ScreenGrabber";

        public static void ShotNotification(string str) {
            if (Properties.Settings.Default.Notifications) {
                trayIcon.BalloonTipTitle = projectName;
                trayIcon.BalloonTipText = str;
                trayIcon.ShowBalloonTip(1500);
            }
        }
    }
}
