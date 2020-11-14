using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Text;

namespace TechFlurry.SparkLedger.Shared.Helpers
{
    public static class Cryptography
    {
        private const string key = "Sparkfxe45&%dj@abmKloias.Mnei*uh";
        private static readonly Encoding encoding = Encoding.ASCII;
        private static readonly IBlockCipherPadding padding = new Pkcs7Padding();
        public static string Encrypt(string plainValue)
        {
            var encryptedString = GetEngine().Encrypt(plainValue, key);
            return encryptedString;
        }
        public static string Decrypt(string cipherValue)
        {
            var decryptedString = GetEngine().Decrypt(cipherValue, key);
            return decryptedString;
        }
        private static BCEngine GetEngine()
        {
            BCEngine bCEngine = new BCEngine(new AesEngine(), encoding);
            bCEngine.SetPadding(padding);
            return bCEngine;
        }
    }
    class BCEngine
    {
        private readonly Encoding _encoding;
        private readonly IBlockCipher _blockCipher;
        private PaddedBufferedBlockCipher _cipher;
        private IBlockCipherPadding _padding;

        public BCEngine(IBlockCipher blockCipher, Encoding encoding)
        {
            _blockCipher = blockCipher;
            _encoding = encoding;
        }

        public void SetPadding(IBlockCipherPadding padding)
        {
            if (padding != null)
                _padding = padding;
        }

        public string Encrypt(string plain, string key)
        {
            byte[] result = BouncyCastleCrypto(true, _encoding.GetBytes(plain), key);
            return Convert.ToBase64String(result);
        }

        public string Decrypt(string cipher, string key)
        {
            byte[] result = BouncyCastleCrypto(false, Convert.FromBase64String(cipher), key);
            return _encoding.GetString(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="forEncrypt"></param>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="CryptoException"></exception>


        private byte[] BouncyCastleCrypto(bool forEncrypt, byte[] input, string key)
        {
            try
            {
                _cipher = _padding == null ? new PaddedBufferedBlockCipher(_blockCipher) : new PaddedBufferedBlockCipher(_blockCipher, _padding);
                byte[] keyByte = _encoding.GetBytes(key);
                _cipher.Init(forEncrypt, new KeyParameter(keyByte));
                return _cipher.DoFinal(input);
            }
            catch (CryptoException ex)
            {
                throw ex;
            }
        }
    }
}
