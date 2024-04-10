using System;
using System.IO;
namespace Breakout.Utilities {
    public class FileCollector {
        private static FileCollector instance;

        public static FileCollector GetInstance() {
            return FileCollector.instance ?? 
                (FileCollector.instance = new FileCollector());
        }

        //effectively copied from Texture.cs in DIKUArcade and not
        //original work
        public static string FilePath(string filename) {
            // find base path
            var dir = new DirectoryInfo(Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location));

            while (dir.Name != "bin")
            {
                dir = dir.Parent;
            }
            dir = dir.Parent;

            // load image file
            var path = Path.Combine(dir.FullName.ToString(), "..", "Breakout", filename);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Error: The file \"{path}\" does not exist.");
            }
            return path;
        }
    }
}