using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Xml;

namespace Designer
{
    public class XMLEncryptor
    {
        private byte[] signature;
        private string username, password;
        const int BIN_SIZE = 4096;
        public byte[] md5Key, md5IV;
        private bool validParameters;

        public XMLEncryptor(string username, string password)
        {
            this.username = username;
            this.password = password;
            if (username.Length + password.Length < 6)
            {
                validParameters = false;
                // abort the constructor. Calls to public functions will not work.
                return;
            }
            else
            {
                validParameters = true;
            }
            GenerateSignature();
            GenerateKey();
            GenerateIV();
        }
        // constructor for master file
        public XMLEncryptor()
        {
            this.username = "#9@rnsr~dk";
            this.password = "kd~rsnr@9#";
            if (username.Length + password.Length < 6)
            {
                validParameters = false;
                // abort the constructor. Calls to public functions will not work.
                return;
            }
            else
            {
                validParameters = true;
            }
            GenerateSignature();
            GenerateKey();
            GenerateIV();
        }

        #region Helper functions called from constructor only
        /// <summary>
        /// Generates a standard signature for the file. The signature may be longer than 16 bytes if deemed necessary.
        /// The signature, which is NOT ENCRYPTED, serves two purposes. 
        /// 1. It allows to recognize the file as one that has been encrypted with the XMLEncryptor class.
        /// 2. The first bytes of each XML file are quite similar (<?xml version="1.0" encoding="utf-8" ?>).
        ///	 This can be exploite to "guess" the key the file has been encrypted with. Adding a signature of a reasonably
        ///	 large number of bytes can be used to overcome this limitation.
        /// </summary>
        private void GenerateSignature()
        {
            signature = new byte[16] {
												 123,	151,	099,	106,
												 031,	167,	091,	211,
												 006,	089,	239,	254,
												 045,	188,	009,	255
											 };
        }
        /// <summary>
        /// Generates an MD5 key for encryption/decryption. This method is only called during construction.
        /// </summary>
        private void GenerateKey()
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            StringBuilder hash = new StringBuilder(username + password);

            // Manipulate the hash string
            for (int i = 1; i < hash.Length; i += 2)
            {
                char c = hash[i - 1];
                hash[i - 1] = hash[i];
                hash[i] = c;
            }

            // Convert the string into a byte array.
            Encoding unicode = Encoding.Unicode;
            byte[] unicodeBytes = unicode.GetBytes(hash.ToString());
            // Compute the key from the byte array
            md5Key = md5.ComputeHash(unicodeBytes);
        }
        /// <summary>
        /// Generates an MD5 key for encryption/decryption. This method is only called during construction.
        /// </summary>
        internal string GetKey()
        {
            string mykey=string.Empty;
            for (int i = 0; i < md5Key.Length; i++)
            {
                mykey += md5Key[i].ToString();                
            }
            return mykey;
        }
        /// <summary>
        /// Generates an MD5 Initiakization Vector for encryption/decryption. This method is only called during construction.
        /// </summary>
        private void GenerateIV()
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            string hash = password + username;

            // Convert the string into a byte array.
            Encoding unicode = Encoding.Unicode;
            byte[] unicodeBytes = unicode.GetBytes(hash);

            // Compute the IV from the byte array
            md5IV = md5.ComputeHash(unicodeBytes);
        }

        #endregion

        #region Methods to write and verify the signature
        private void WriteSignature(FileStream fOut)
        {
            fOut.Position = 0;
            fOut.Write(signature, 0, 16);
        }
        private bool VerifySignature(FileStream fIn)
        {
            byte[] bin = new byte[16];
            fIn.Read(bin, 0, 16);
            for (int i = 0; i < 16; i++)
            {
                if (bin[i] != signature[i])
                {
                    return false;
                }
            }
            // Reset file pointer.
            fIn.Position = 0;
            return true;
        }

        #endregion

        #region Public Functions
        /// <summary>
        /// Reads an encrypted XML file into a DataSet.
        /// </summary>
        /// <param name="fileName">The path to the XML file.</param>
        /// <returns>The DataSet, or null if an error occurs.</returns>
        public DataSet ReadEncryptedXML(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            FileStream inFile;

            #region Check for possible errors (includes verification of the signature)
            if (!validParameters)
            {
                Trace.WriteLine("Invalid parameters");
                return null;
            }
            if (!fi.Exists)
            {
                Trace.WriteLine("Could not perform decryption: File " + fileName + " does not exist");
                return null;
            }
            if (fi.Length > Int32.MaxValue)
            {
                Trace.WriteLine("Only files up to 2GB in size can be handled");
                return null;
            }

            try
            {
                inFile = new FileStream(fi.ToString(), FileMode.Open);
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message + "Could not perform decryption");
                return null;
            }
            if (!VerifySignature(inFile))
            {
                Trace.WriteLine("Invalid signature");
                return null;
            }
            #endregion

            RijndaelManaged rijn = new RijndaelManaged();
            rijn.Padding = PaddingMode.Zeros;
            ICryptoTransform decryptor = rijn.CreateDecryptor(md5Key, md5IV);
            // Allocate byte array buffer to read only the xml part of the file (ie everything following the signature).
            byte[] encryptedXmlData = new byte[(int)fi.Length - signature.Length];
            inFile.Position = signature.Length;
            inFile.Read(encryptedXmlData, 0, encryptedXmlData.Length);

            // Convert the byte array to a MemoryStream object so that it can be passed on to the CryptoStream
            MemoryStream encryptedXmlStream = new MemoryStream(encryptedXmlData);
            // Create a CryptoStream, bound to the MemoryStream containing the encrypted xml data
            CryptoStream csDecrypt = new CryptoStream(encryptedXmlStream, decryptor, CryptoStreamMode.Read);

            // Read in the DataSet from the CryptoStream
            DataSet data = new DataSet();
            data.Locale = new System.Globalization.CultureInfo("en-US");
            try
            {
                data.ReadXml(csDecrypt, XmlReadMode.Auto);
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message, "Error decrypting XML");
                return null;
            }

            // flush & close files.
            encryptedXmlStream.Flush();
            encryptedXmlStream.Close();
            inFile.Close();
            return data;
        }

        /// <summary>
        /// Writes a DataSet to an encrypted XML file.
        /// </summary>
        /// <param name="dataset">The DataSet to encrypt.</param>
        /// <param name="encFileName">The name of the encrypted file. Existing files will be overwritten.</param>
        public void WriteEncryptedXML(DataSet dataset, string encFileName)
        {
            FileStream fOut;

            #region Check for possible errors
            if (!validParameters)
            {
                Trace.WriteLine("Invalid parameters");
                return;
            }
            #endregion
            // Create a MemoryStream and write the DataSet to it.
            MemoryStream xmlStream = new MemoryStream();
            dataset.WriteXml(xmlStream);
            // Reset the pointer of the MemoryStream (which is at the EOF after the WriteXML function).
            xmlStream.Position = 0;

            // Create a write FileStream and write the signature to it (unencrypted).
            fOut = new FileStream(encFileName, FileMode.Create);
            WriteSignature(fOut);

            #region Encryption objects
            RijndaelManaged rijn = new RijndaelManaged();
            rijn.Padding = PaddingMode.Zeros;
            ICryptoTransform encryptor = rijn.CreateEncryptor(md5Key, md5IV);
            CryptoStream csEncrypt = new CryptoStream(fOut, encryptor, CryptoStreamMode.Write);
            #endregion
            //Create variables to help with read and write.
            byte[] bin = new byte[BIN_SIZE];			// Intermediate storage for the encryption.
            int rdlen = 0;									// The total number of bytes written.
            int totlen = (int)xmlStream.Length;	// The total length of the input stream.
            int len;											// The number of bytes to be written at a time.

            //Read from the input file, then encrypt and write to the output file.
            while (rdlen < totlen)
            {
                len = xmlStream.Read(bin, 0, bin.Length);
                if (len == 0 && rdlen == 0)
                {
                    Trace.WriteLine("Could not read");
                    break;
                }
                csEncrypt.Write(bin, 0, len);
                rdlen += len;
            }
            csEncrypt.FlushFinalBlock();
            csEncrypt.Close();
            fOut.Close();
            xmlStream.Close();
        }
        #endregion
    }
}
