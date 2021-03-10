using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace EasyNow.Utility.Tools
{
    /// <summary>
    /// RSA密钥转换
    /// </summary>
    public class RsaKeyConverter
    {
        /// <summary>
        /// 根据pem字符串得到私钥
        /// </summary>
        /// <param name="pem"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider PrivateKeyFromPem(string pem)
        {
            using (var privateKeyTextReader = new StringReader(pem))
            {
                var readKeyPair = (AsymmetricCipherKeyPair)new PemReader(privateKeyTextReader).ReadObject();

                var privateKeyParams = ((RsaPrivateCrtKeyParameters)readKeyPair.Private);
                var cryptoServiceProvider = new RSACryptoServiceProvider();
                var parms = new RSAParameters
                {
                    Modulus = privateKeyParams.Modulus.ToByteArrayUnsigned(),
                    P = privateKeyParams.P.ToByteArrayUnsigned(),
                    Q = privateKeyParams.Q.ToByteArrayUnsigned(),
                    DP = privateKeyParams.DP.ToByteArrayUnsigned(),
                    DQ = privateKeyParams.DQ.ToByteArrayUnsigned(),
                    InverseQ = privateKeyParams.QInv.ToByteArrayUnsigned(),
                    D = privateKeyParams.Exponent.ToByteArrayUnsigned(),
                    Exponent = privateKeyParams.PublicExponent.ToByteArrayUnsigned()
                };

                cryptoServiceProvider.ImportParameters(parms);

                return cryptoServiceProvider;
            }
        }

        /// <summary>
        /// 根据pem字符串得到公钥
        /// </summary>
        /// <param name="pem"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider PublicKeyFromPem(string pem)
        {
            using (var publicKeyTextReader = new StringReader(pem))
            {
                var publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();

                var cryptoServiceProvider = new RSACryptoServiceProvider();
                var parms = new RSAParameters
                {
                    Modulus = publicKeyParam.Modulus.ToByteArrayUnsigned(),
                    Exponent = publicKeyParam.Exponent.ToByteArrayUnsigned()
                };
                cryptoServiceProvider.ImportParameters(parms);

                return cryptoServiceProvider;
            }
        }
    }
}
