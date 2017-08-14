using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace screengrab.Classes {

    public class Fields  {
        public String XMLFileName = Environment.CurrentDirectory + "\\settings.xml";
        public Hotkey screenToClipboard;
        public Hotkey screenWithEdit;

        public bool loadImageToDisk;
        public string imagesFolder;
        public int imageFormat;

        public bool startup;
        public bool openPictureInBrowser;
        public bool firstLoad;

        public Fields() {

        }
    }

    public class Settings {

        public Fields fields;

        public Settings() {
            fields = new Fields();
        }

        public void WriteXml() {
            if (fields.firstLoad)
                fields.firstLoad = false;
            XmlSerializer ser = new XmlSerializer(typeof(Fields));
            TextWriter writer = new StreamWriter(fields.XMLFileName);
            ser.Serialize(writer, fields);
            writer.Close();
        }
        public void ReadXml() {
            if (File.Exists(fields.XMLFileName)) {
                XmlSerializer ser = new XmlSerializer(typeof(Fields));
                TextReader reader = new StreamReader(fields.XMLFileName);
                fields = ser.Deserialize(reader) as Fields;
                reader.Close();
            } else {
                fields.firstLoad = true;
            }
        }
    }
}
