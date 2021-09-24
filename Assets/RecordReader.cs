using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace BAStudio.Unity.KineticMovementLab
{
    public static class RecordReader<T> where T : Record
    {
        public static int Read (string entries, IList<T> container)
        {
            int count = 0;
            StringReader reader = new StringReader(entries);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                container.Add(MessagePackSerializer.Deserialize<T>(Encoding.UTF8.GetBytes(line)));
                count++;
            }
            return count;
        }
    }
}