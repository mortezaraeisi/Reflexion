using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using REFLEXION_LIB;
using System.IO;

namespace REFLEXION_DESIGNER
{
    [Serializable]
    internal sealed class Project
    {
        [NonSerialized]
        private static Project _instance;
        public static Project Option { get { return _instance; } }

        private Project()
        {

        }

        [NonSerialized]
        private string _path;
        private Game _game;

        public Game Game { get { return _game; } }
        public string Path { get { return _path; } }

        public static Project CreateNewProject()
        {
            Project prg = new Project();
            prg._game = new Game("Untitled Game");
            prg._game.Add(new Page(Policy.DEFAULT_PAGE_NAME, Policy.DEFAULT_SCREEN_SIZE, Policy.DEFAULT_CELL_SIZE,System.Drawing.Color.White.ToArgb()));
            prg._path = string.Empty;
            return _instance = prg;
        }

        public static void Publish(string path) { Publish(path, _instance._game); }
        public static void Publish(string path, Game game)
        {
            System.IO.FileStream stream = System.IO.File.Open(path, System.IO.FileMode.OpenOrCreate, FileAccess.Write);
            Publish(stream, game);
            stream.Flush();
            stream.Close();
        }
        public static void Publish(Stream dest, Game game)
        {
            game.Publish(dest);
        }
        public void SaveToFile() { this.SaveToFile(_path); }
        public void SaveToFile(string path)
        {
            System.IO.FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            uploadTo(stream, _instance);
            stream.Flush();
            stream.Close();
            _path = path;
        }

        public static Project LoadFromFile(string path)
        {
            System.IO.FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            _instance = downloadFrom(stream);
            stream.Close();
           // Project prg = new Project();
           // prg._game = g;
            _instance._path = path;
            return _instance;
        }

        private static Stream uploadTo(Stream stream, Project prg)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, prg);
            return stream;
        }

        private static  Project downloadFrom(Stream stream)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Project prg = (Project)formatter.Deserialize(stream);
            return prg;
        }
    };
}
