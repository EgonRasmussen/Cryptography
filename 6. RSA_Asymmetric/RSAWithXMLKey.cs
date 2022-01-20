﻿using System.Security.Cryptography;

namespace CryptographyInDotNet;
public class RsaWithXmlKey
{
    public void AssignNewKey(string publicKeyPath, string privateKeyPath)
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.PersistKeyInCsp = false;

            if (File.Exists(privateKeyPath))
            {
                File.Delete(privateKeyPath);
            }

            if (File.Exists(publicKeyPath))
            {
                File.Delete(publicKeyPath);
            }

            var publicKeyfolder = Path.GetDirectoryName(publicKeyPath);
            var privateKeyfolder = Path.GetDirectoryName(privateKeyPath);

            Directory.CreateDirectory(publicKeyfolder); // The folder is only created if it dosn't already exist (check summary)
            Directory.CreateDirectory(privateKeyfolder);// The folder is only created if it dosn't already exist (check summary)

            File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
            File.WriteAllText(privateKeyPath, rsa.ToXmlString(true));
        }
    }

    public byte[] EncryptData(string publicKeyPath, byte[] dataToEncrypt)
    {
        byte[] cipherbytes;

        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.PersistKeyInCsp = false;
            rsa.FromXmlString(File.ReadAllText(publicKeyPath));

            cipherbytes = rsa.Encrypt(dataToEncrypt, false);
        }

        return cipherbytes;
    }

    public byte[] DecryptData(string privateKeyPath, byte[] dataToEncrypt)
    {
        byte[] plain;

        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.PersistKeyInCsp = false;
            rsa.FromXmlString(File.ReadAllText(privateKeyPath));
            plain = rsa.Decrypt(dataToEncrypt, false);
        }

        return plain;
    }
}
