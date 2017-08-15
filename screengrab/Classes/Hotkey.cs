using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace screengrab.Classes
{
    public class Hotkey  {
        public string name { get; set; }
        public List<Key> hotkey;

        public Hotkey() { }
        
        public Hotkey(string name, List<Key> hotkey) {
            this.name = name;
            this.hotkey = hotkey;
        }

        public void ChangeHotkey(List<Key> hotkey) {
            this.hotkey.Clear();
            foreach(Key k in hotkey) {
                this.hotkey.Add(k);
            }
        }

        public bool IsPressed(List<Key> pressedKeys) {
            bool temp = true;
            foreach(Key k in this.hotkey) {
                if (!pressedKeys.Contains(k))
                    temp = false;
            }
            return temp;
        }
        
        public override string ToString() {
            return string.Join<Key>(" + ", hotkey);
        }
    }
}
