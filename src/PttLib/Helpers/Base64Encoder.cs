using System.Security.Cryptography;
using System.IO;
using System;

namespace PttLib.Helpers
{
    public class Base64Encoder
    {
        public void Encode(string inFileName, string outFileName)
        {
            var transform = new ToBase64Transform();
            using (FileStream inFile = File.OpenRead(inFileName),
                                        outFile = File.Create(outFileName))
            using (var cryptStream = new CryptoStream(outFile, transform, CryptoStreamMode.Write))
            {
                // I'm going to use a 4k buffer, tune this as needed
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = inFile.Read(buffer, 0, buffer.Length)) > 0)
                    cryptStream.Write(buffer, 0, bytesRead);

                cryptStream.FlushFinalBlock();
            }
        }

        public void Decode(string inFileName, string outFileName)
        {
            var transform = new FromBase64Transform();
            using (FileStream inFile = File.OpenRead(inFileName),
                                        outFile = File.Create(outFileName))
            using (var cryptStream = new CryptoStream(inFile, transform, CryptoStreamMode.Read))
            {
                var buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = cryptStream.Read(buffer, 0, buffer.Length)) > 0)
                    outFile.Write(buffer, 0, bytesRead);

                outFile.Flush();
            }
        }

        // this version of Encode pulls everything into memory at once
        // you can compare the output of my Encode method above to the output of this one
        // the output should be identical, but the crytostream version
        // will use way less memory on a large file than this version.
        public void MemoryEncode(string inFileName, string outFileName)
        {
            byte[] bytes = File.ReadAllBytes(inFileName);
            File.WriteAllText(outFileName, Convert.ToBase64String(bytes));
        }
    }
}