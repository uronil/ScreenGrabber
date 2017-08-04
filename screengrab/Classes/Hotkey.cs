using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace screengrab
{
    class Hotkey
    {
        public string name;
        private List<Key> hotkey;

        public Hotkey(string name, List<Key> hotkey) {
            this.name = name;
            this.hotkey = hotkey;
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
