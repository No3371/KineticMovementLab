using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace BAStudio.Unity.KineticMovementLab
{
    public static class RecordWriter<T> where T : Record
    {
        public static async Task<string> Write (IEnumerable<T> records)
        {
            StringWriter stringWriter = new StringWriter();
            foreach (var r in records)
            {
                await stringWriter.WriteLineAsync(Encoding.UTF8.GetString(MessagePackSerializer.Serialize<T>(r)));
            }
            return stringWriter.ToString();
        }
    }
}