
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ByteTool 
{
    public static byte[] Object2Bytes(object obj)
    {
        byte[] buffer;
        using (MemoryStream ms = new MemoryStream())
        {
            IFormatter iFormatter = new BinaryFormatter();
            iFormatter.Serialize(ms, obj);
            buffer = ms.GetBuffer();
        }
        return buffer;
    }
}
