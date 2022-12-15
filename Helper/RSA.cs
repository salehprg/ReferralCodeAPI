using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

class RSA
{
    public static string Decrypt(string Data)
    {
        using(var rsa = new RSACryptoServiceProvider(1024))
        {
            try
            {
                var privateKey = File.ReadAllText("key.pem");
                
                rsa.ImportFromPem(privateKey);

                var resultBytes = Convert.FromBase64String(Data);
                var decryptedBytes = rsa.Decrypt(resultBytes, true);
                var decryptedData = Encoding.UTF8.GetString(decryptedBytes);

                return decryptedData;
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }

        return null;
    }
}