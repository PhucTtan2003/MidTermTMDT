namespace MyEStore.MyTool
{
    using System.Security.Cryptography;
    using System.Text;

    public static class HashHelper
    {
        public static string ToMd5Hash(this string input, string key)
        {
            // Combine the password with the random key
            string combined = input + key;

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(combined);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

}
