using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REFLEXION_PLAYER
{
    [Serializable]
    internal sealed class Settings
    {
        private static Settings _instance;
        public static Settings Option { get { return _instance; } }

        private string _spacePath;
        [NonSerialized]
        private string _configPath;

        public static Settings MakeNewSetting(string configPath = null)
        {
            if (System.IO.File.Exists(configPath))
                return LoadFromFile(configPath);
            _instance = new Settings() { _configPath = configPath };
            return _instance;
        }
        internal static Settings LoadFromFile(string configPath)
        {
            System.IO.FileStream file = new System.IO.FileStream(configPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            _instance = (Settings)formatter.Deserialize(file);
            _instance._configPath = configPath;
            file.Close();
            return _instance;
        }

        internal void SaveToFile()
        {
            System.IO.FileStream file = new System.IO.FileStream(_configPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(file, this);
            file.Close();           
        }

        public string SpacePath { get { return _spacePath; } set { _spacePath = value; this.SaveToFile(); } }

    };
}
