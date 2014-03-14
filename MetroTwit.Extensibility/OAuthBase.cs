namespace MetroTwit.Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public class OAuthBase
    {
        protected const string HMACSHA1SignatureType = "HMAC-SHA1";
        protected const string OAuthCallbackKey = "oauth_callback";
        protected const string OAuthConsumerKeyKey = "oauth_consumer_key";
        protected const string OAuthNonceKey = "oauth_nonce";
        protected const string OAuthParameterPrefix = "oauth_";
        protected const string OAuthSignatureKey = "oauth_signature";
        protected const string OAuthSignatureMethodKey = "oauth_signature_method";
        protected const string OAuthTimestampKey = "oauth_timestamp";
        protected const string OAuthTokenKey = "oauth_token";
        protected const string OAuthTokenSecretKey = "oauth_token_secret";
        protected const string OAuthVersion = "1.0";
        protected const string OAuthVersionKey = "oauth_version";
        protected const string PlainTextSignatureType = "PLAINTEXT";
        protected Random random = new Random();
        protected const string RSASHA1SignatureType = "RSA-SHA1";
        protected string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        private string ComputeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            return Convert.ToBase64String(hashAlgorithm.ComputeHash(bytes));
        }

        public virtual string GenerateNonce()
        {
            return this.random.Next(0x1e208, 0x98967f).ToString();
        }

        public string GenerateSignature(Uri url, string consumerKey, string consumerSecret, string token, string tokenSecret, string httpMethod, string timeStamp, string nonce, out string normalizedUrl, out string normalizedRequestParameters)
        {
            return this.GenerateSignature(url, consumerKey, consumerSecret, token, tokenSecret, httpMethod, timeStamp, nonce, SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedRequestParameters);
        }

        public string GenerateSignature(Uri url, string consumerKey, string consumerSecret, string token, string tokenSecret, string httpMethod, string timeStamp, string nonce, SignatureTypes signatureType, out string normalizedUrl, out string normalizedRequestParameters)
        {
            normalizedUrl = null;
            normalizedRequestParameters = null;
            switch (signatureType)
            {
                case SignatureTypes.HMACSHA1:
                    {
                        string signatureBase = this.GenerateSignatureBase(url, consumerKey, token, tokenSecret, httpMethod, timeStamp, nonce, "HMAC-SHA1", out normalizedUrl, out normalizedRequestParameters);
                        HMACSHA1 hash = new HMACSHA1
                        {
                            Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", this.UrlEncode(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? "" : this.UrlEncode(tokenSecret)))
                        };
                        return this.GenerateSignatureUsingHash(signatureBase, hash);
                    }
                case SignatureTypes.PLAINTEXT:
                    return HttpUtility.UrlEncode(string.Format("{0}&{1}", consumerSecret, tokenSecret));

                case SignatureTypes.RSASHA1:
                    throw new NotImplementedException();
            }
            throw new ArgumentException("Unknown signature type", "signatureType");
        }

        public string GenerateSignatureBase(Uri url, string consumerKey, string token, string tokenSecret, string httpMethod, string timeStamp, string nonce, string signatureType, out string normalizedUrl, out string normalizedRequestParameters)
        {
            if (token == null)
            {
                token = string.Empty;
            }
            if (tokenSecret == null)
            {
                tokenSecret = string.Empty;
            }
            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException("consumerKey");
            }
            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException("httpMethod");
            }
            if (string.IsNullOrEmpty(signatureType))
            {
                throw new ArgumentNullException("signatureType");
            }
            normalizedUrl = null;
            normalizedRequestParameters = null;
            List<QueryParameter> queryParameters = this.GetQueryParameters(url.Query);
            queryParameters.Add(new QueryParameter("oauth_version", "1.0"));
            queryParameters.Add(new QueryParameter("oauth_nonce", nonce));
            queryParameters.Add(new QueryParameter("oauth_timestamp", timeStamp));
            queryParameters.Add(new QueryParameter("oauth_signature_method", signatureType));
            queryParameters.Add(new QueryParameter("oauth_consumer_key", consumerKey));
            if (!string.IsNullOrEmpty(token))
            {
                queryParameters.Add(new QueryParameter("oauth_token", token));
            }
            queryParameters.Sort(new QueryParameterComparer());
            normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
            if (((url.Scheme != "http") || (url.Port != 80)) && ((url.Scheme != "https") || (url.Port != 0x1bb)))
            {
                normalizedUrl = normalizedUrl + ":" + url.Port;
            }
            normalizedUrl = normalizedUrl + url.AbsolutePath;
            normalizedRequestParameters = this.NormalizeRequestParameters(queryParameters);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}&", httpMethod.ToUpper());
            builder.AppendFormat("{0}&", this.UrlEncode(normalizedUrl));
            builder.AppendFormat("{0}", this.UrlEncode(normalizedRequestParameters));
            return builder.ToString();
        }

        public string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
        {
            return this.ComputeHash(hash, signatureBase);
        }

        public virtual string GenerateTimeStamp()
        {
            TimeSpan span = (TimeSpan)(DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0));
            long totalSeconds = (long)span.TotalSeconds;
            return totalSeconds.ToString();
        }

        private List<QueryParameter> GetQueryParameters(string parameters)
        {
            if (parameters.StartsWith("?"))
            {
                parameters = parameters.Remove(0, 1);
            }
            List<QueryParameter> list = new List<QueryParameter>();
            if (!string.IsNullOrEmpty(parameters))
            {
                string[] strArray = parameters.Split(new char[] { '&' });
                foreach (string str in strArray)
                {
                    if (!string.IsNullOrEmpty(str) && !str.StartsWith("oauth_"))
                    {
                        if (str.IndexOf('=') > -1)
                        {
                            string[] strArray2 = str.Split(new char[] { '=' });
                            list.Add(new QueryParameter(strArray2[0], strArray2[1]));
                        }
                        else
                        {
                            list.Add(new QueryParameter(str, string.Empty));
                        }
                    }
                }
            }
            return list;
        }

        protected string NormalizeRequestParameters(IList<QueryParameter> parameters)
        {
            StringBuilder builder = new StringBuilder();
            QueryParameter parameter = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                parameter = parameters[i];
                builder.AppendFormat("{0}={1}", parameter.Name, parameter.Value);
                if (i < (parameters.Count - 1))
                {
                    builder.Append("&");
                }
            }
            return builder.ToString();
        }

        protected string UrlEncode(string value)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char ch in value)
            {
                if (this.unreservedChars.IndexOf(ch) != -1)
                {
                    builder.Append(ch);
                }
                else
                {
                    builder.Append('%' + string.Format("{0:X2}", (int)ch));
                }
            }
            return builder.ToString();
        }

        protected class QueryParameter
        {
            private string name = null;
            private string value = null;

            public QueryParameter(string name, string value)
            {
                this.name = name;
                this.value = value;
            }

            public string Name
            {
                get
                {
                    return this.name;
                }
            }

            public string Value
            {
                get
                {
                    return this.value;
                }
            }
        }

        protected class QueryParameterComparer : IComparer<OAuthBase.QueryParameter>
        {
            public int Compare(OAuthBase.QueryParameter x, OAuthBase.QueryParameter y)
            {
                if (x.Name == y.Name)
                {
                    return string.Compare(x.Value, y.Value);
                }
                return string.Compare(x.Name, y.Name);
            }
        }

        public enum SignatureTypes
        {
            HMACSHA1,
            PLAINTEXT,
            RSASHA1
        }
    }
}

