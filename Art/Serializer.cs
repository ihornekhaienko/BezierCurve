using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Art
{
    public static class Serializer
    {
        public static void Serialize(List<List<Polyline>> list, string file)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(file, FileMode.Truncate))
            {
                formatter.Serialize(fs, list);
            }
        }

        public static void Serialize(PolyWrapper list, string file)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(file, FileMode.Truncate))
            {
                formatter.Serialize(fs, list);
            }
        }

        public static PolyWrapper Deserialize(string file, PolyWrapper wrapper)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            PolyWrapper list = new PolyWrapper();
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                list = (PolyWrapper)formatter.Deserialize(fs);
            }

            return list;
        }

        public static List<List<Polyline>> Deserialize(string file)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<List<Polyline>> list = new List<List<Polyline>>();
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                list = (List<List<Polyline>>)formatter.Deserialize(fs);
            }

            return list;
        }

        public static void Serialize(List<Dictionary<Ellipse, PointUI>> list, string file)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(file, FileMode.Truncate))
            {
                formatter.Serialize(fs, list);
            }
        }

        public static List<Dictionary<Ellipse, PointUI>> Deserialize(string file, bool b)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<Dictionary<Ellipse, PointUI>> list;
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                list = (List<Dictionary<Ellipse, PointUI>>)formatter.Deserialize(fs);
            }

            return list;
        }
    }
}
