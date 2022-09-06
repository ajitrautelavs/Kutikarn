using System;
using System.IO;
using System.Security.Cryptography;

namespace Kutikarn
{
    internal class Kutikarn
    {
        private bool _d;

        public Kutikarn(bool jhanda)
        {
            _d = jhanda;   
        }
        
        internal string Kuti(string naam, string kunji)
        {
            if (File.Exists(naam))
            {
                KutiFile(naam, kunji);
            }
            else
            {
                string[] files = Directory.GetFiles(naam);
                foreach (string file in files)
                    {
                    KutiFile(file, kunji);
                    }
                }
            return String.Empty;
        }

        private void KutiFile(string file, string kunji)
        {
            // Create a string to encrypt.
            string bahar = Path.Combine(Path.GetFullPath(file).Replace(Path.GetFileName(file), ""), Path.GetFileName(file) + ".kkf");
            byte[] Key = KeyBytes(kunji);
            // Encrypt text to a file using the file name, key, and IV.
            EncryptBinaryToFile(file, bahar, Key);
            if (_d == true)
            {
                File.Delete(file);
            }
        }

        internal string Duti(string naam, string kunji)
        {
            if (File.Exists(naam))
            {
                DutiFile(naam, kunji);
            }
            else
            {
                string[] files = Directory.GetFiles(naam);
                foreach (string file in files)
                    {
                    DutiFile(file, kunji);
                    }
                }
            return String.Empty;
        }

        private void DutiFile(string file, string kunji)
        {
            string bahar = Path.Combine(Path.GetFullPath(file).Replace(Path.GetFileName(file), ""), Path.GetFileName(file).Replace(".kkf",""));
            byte[] Key = KeyBytes(kunji);
            // Decrypt the text from a file using the file name, key, and IV.
            DecryptBinaryFromFile(file, bahar, Key);
            if (_d == true)
            {
                File.Delete(file);
            }
        }

        private void EncryptBinaryToFile(String fileName, string outputFileName, byte[] Key)
        {
            try
            {
                // Create a new TripleDESCryptoServiceProvider object
                // to generate a key and initialization vector (IV).
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

                // Create or open the specified file.
                using (FileStream fsout = File.Open(outputFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {

                    // Create a CryptoStream using the FileStream 
                    // and the passed key and initialization vector (IV).
                    using (CryptoStream cs = new CryptoStream(fsout, des.CreateEncryptor(Key, IV()), CryptoStreamMode.Write))
                    {
                        // Create a StreamWriter using the CryptoStream.

                        // Write the data to the stream 
                        // to encrypt it.
                        using (FileStream fsin = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                        {
                            int BufferSize = 1024;
                            byte[] ioBuffer = new byte[BufferSize];
                            int BytesRead;

                            do
                            {
                                BytesRead = fsin.Read(ioBuffer, 0, BufferSize);
                                if ((BytesRead == 0))
                                    break;
                                cs.Write(ioBuffer, 0, BytesRead);
                            }
                            while (true);
                            cs.Flush();
                        }
                    }
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file access error occurred: {0}", e.Message);
            }
        }

        private void DecryptBinaryFromFile(String fileName, string outputFileName, byte[] Key)
        {
            try
            {
                // Create a new TripleDESCryptoServiceProvider object
                // to generate a key and initialization vector (IV).
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

                // Create or open the specified file. 
                using (FileStream fsin = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Create a CryptoStream using the FileStream 
                    // and the passed key and initialization vector (IV).
                    using (CryptoStream cs = new CryptoStream(fsin, des.CreateDecryptor(Key, IV()), CryptoStreamMode.Read))
                    {
                        // Create a StreamReader using the CryptoStream.
                        using (FileStream fsout = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                        {
                            int BufferSize = 1024;
                            byte[] ioBuffer = new byte[BufferSize];
                            int BytesRead;

                            do
                            {
                                BytesRead = cs.Read(ioBuffer, 0, BufferSize);
                                if (BytesRead == 0)
                                    break;
                                fsout.Write(ioBuffer, 0, BytesRead);
                            }
                            while (true);
                            fsout.Flush();
                        }
                    }
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file access error occurred: {0}", e.Message);
            }
        }

        [System.Reflection.Obfuscation(Feature = "virtualization", Exclude = false)]
        private byte[] KeyBytes(string strEncrKey)
        {
            // length of 16 or 24 will work for 128 bit or 192 bit respectively
            if (strEncrKey.Length < 16)
            {
                int i = 15 - strEncrKey.Length;
                while (strEncrKey.Length < 16)
                {
                    strEncrKey = strEncrKey + ChArr()[i];
                    i = i - 1;
                }
            }
            else if (strEncrKey.Length > 16 & strEncrKey.Length < 24)
            {
                int i = 23 - strEncrKey.Length;
                while (strEncrKey.Length < 24)
                {
                    strEncrKey = strEncrKey + ChArr()[i];
                    i = i - 1;
                }
            }
            else if (strEncrKey.Length > 24)
                strEncrKey = strEncrKey.Substring(0, 24);

            return System.Text.Encoding.UTF8.GetBytes(strEncrKey);
        }
        [System.Reflection.Obfuscation(Feature = "virtualization", Exclude = false)]
        private char[] ChArr()
        {
            return "Bg82an&%2hvbOA*d".ToCharArray();
        }

        [System.Reflection.Obfuscation(Feature = "virtualization", Exclude = false)]
        private byte[] IV()
        {
            return new byte[] { 0xB1, 0xC5, 0x60, 0x5, 0x59, 0x13, 0x4E, 0x8 };
        }
    }
}
