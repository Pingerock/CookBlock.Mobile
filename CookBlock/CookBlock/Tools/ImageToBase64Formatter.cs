using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

namespace CookBlock.Tools
{
    public class ImageToBase64Formatter
    {
        public byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        public Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }

        public byte[] ByteArrayFromImage(string imageFilePath)
        {
            /*MemoryStream ms = new MemoryStream();
            System.Drawing.Image img = System.Drawing.Image.FromFile(imageFilePath);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, System.Drawing.Imaging.ImageFormat.Jpeg);*/
            //img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            //return ms.ToArray();
            return File.ReadAllBytes(imageFilePath);
        }


    }
}
