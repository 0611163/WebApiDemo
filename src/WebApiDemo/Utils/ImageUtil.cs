namespace Utils
{
    public class ImageUtil
    {
        public static string GetImageBase64(string path)
        {
            byte[] bytes = null;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
            }
            return Convert.ToBase64String(bytes);
        }
    }
}
