using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace Designer
{
	public class PwdTdsCryptoStream : CryptoStream
	{
		private ICryptoTransform m_CryptoTransform;
		private bool m_bDisposed;

		public PwdTdsCryptoStream(System.IO.Stream Stream, CryptoStreamMode Mode, string Password)
			: this(Stream, CreateTripleDESTransform(Mode, Password), Mode)
		{
		}

		public PwdTdsCryptoStream(System.IO.Stream Stream, ICryptoTransform Transform, CryptoStreamMode Mode)
			: base(Stream, Transform, Mode)
		{
			m_CryptoTransform = Transform;
			m_bDisposed = false;
		}


		#region Static functions

		/// <summary>
		/// Creates a transform based on CryptoStreamMode.
		/// </summary>
		/// <param name="Mode"></param>
		/// <param name="Password"></param>
		/// <returns></returns>
		private static ICryptoTransform CreateTripleDESTransform(CryptoStreamMode Mode, string Password)
		{
			byte[] key = null;
			byte[] pdbsalt = null;
			byte[] iv = null;

			try
			{
				// Salt byte array.
				pdbsalt = GetPdbSalt();

				// Create PasswordDeriveBytes object that will generate
				// a Key for TripleDES algorithm.
				PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, pdbsalt);

				iv = new byte[8] { 1, 0, 0, 1, 1, 0, 0, 1 };
				// Create a private key for TripleDES algorithm.
				// The iv parameter is not currently used.
				// * http://blogs.msdn.com/shawnfa/archive/2004/04/14/113514.aspx
				key = pdb.CryptDeriveKey("TripleDES", "SHA1", 192, iv);

				switch (Mode)
				{
					case CryptoStreamMode.Read:
						return TripleDES.Create().CreateDecryptor(key, iv);
					case CryptoStreamMode.Write:
						return TripleDES.Create().CreateEncryptor(key, iv);
					default:
						return null;
				}
			}
			catch (CryptographicException)
			{
				return null;
			}
			finally
			{
				if (key != null)
					Array.Clear(key, 0, key.Length);
				if (pdbsalt != null)
					Array.Clear(pdbsalt, 0, pdbsalt.Length);
				if (iv != null)
					Array.Clear(iv, 0, iv.Length);
			}
		}


		/// <summary>
		/// Creates a random salt vector.
		/// </summary>
		/// <returns></returns>
		private static byte[] GetPdbSalt()
		{
			RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

			// Byte array of the same size as SHA1 hash, which is 160 bits.
			// Would PasswordDeriveBytes benefit from a larger size salt array?
			byte[] arrRandom = new byte[20];
			// Fill the array with random values.
			Gen.GetBytes(arrRandom);
			return arrRandom;
		}

		#endregion Static functions




		#region Public overrides

		public override int ReadByte()
		{
			try
			{
				return base.ReadByte();
			}
			catch (CryptographicException)
			{
				KillStream();
			}
			return -1;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			try
			{
				return base.Read(buffer, offset, count);
			}
			catch (CryptographicException)
			{
				KillStream();
			}
			return -1;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			try
			{
				base.Write(buffer, offset, count);
			}
			catch (CryptographicException)
			{
				KillStream();
			}
		}

		public override void WriteByte(byte value)
		{
			try
			{
				base.WriteByte(value);
			}
			catch (CryptographicException)
			{
				KillStream();
			}
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			try
			{
				return base.EndRead(asyncResult);
			}
			catch (CryptographicException)
			{
				KillStream();
			}
			return -1;
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			try
			{
				base.EndWrite(asyncResult);
			}
			catch (CryptographicException)
			{
				KillStream();
			}
		}


		public override void Flush()
		{
			try
			{
				base.Flush();
			}
			catch (CryptographicException)
			{
				KillStream();
			}
		}

		public override void Close()
		{
			if (!m_bDisposed)
				m_CryptoTransform.Dispose();
		}

		#endregion Public overrides




		private void KillStream()
		{
			m_CryptoTransform.Dispose();
			m_CryptoTransform = null;
			m_bDisposed = true;
			throw new Exception();
		}
	}
}
