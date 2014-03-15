
using FlattyTweet.Extensibility;
using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace FlattyTweet.TwitPic
{
  [Export(typeof (IImageUploadService))]
  public class TwitPicImageUploadService : IImageUploadService
  {
    private bool cancelledFlag;

    public string Name
    {
      get
      {
        return "twitpic";
      }
    }

    public string UniqueID
    {
      get
      {
        return ((object) Assembly.GetExecutingAssembly()).GetType().ToString();
      }
    }

    public TwitPicImageUploadService()
    {
      this.cancelledFlag = false;
    }

    public Action UploadImage(ImageUploadRequest uploadRequest)
    {
      ((Action) (() =>
      {
        try
        {
          string local_0 = "c473f235206724a473a6b30d0eb1861f";
          string local_1 = "OAuth realm=\"{0}\", oauth_consumer_key=\"{1}\", oauth_signature_method=\"HMAC-SHA1\", oauth_token=\"{2}\", oauth_timestamp=\"{3}\", oauth_nonce=\"{4}\", oauth_version=\"1.0\", oauth_signature=\"{5}\"";
          string local_2 = "http://api.twitter.com/";
          string local_3 = "http://api.twitpic.com/2/upload.xml";
          string local_4 = "https://api.twitter.com/1.1/account/verify_credentials.json";
          string local_5 = "iso-8859-1";
          OAuthBase local_6 = new OAuthBase();
          string local_9 = local_6.GenerateTimeStamp();
          string local_10 = local_6.GenerateNonce();
          string local_7;
          string local_8;
          string local_11_1 = HttpUtility.UrlEncode(local_6.GenerateSignature(new Uri(local_4), uploadRequest.ConsumerKey, uploadRequest.ConsumerSecret, uploadRequest.AccessToken, uploadRequest.AccessTokenSecret, "GET", local_9, local_10, out local_7, out local_8));
          string local_12 = Guid.NewGuid().ToString();
          HttpWebRequest local_13 = (HttpWebRequest) WebRequest.Create(local_3);
          local_13.PreAuthenticate = true;
          local_13.AllowWriteStreamBuffering = false;
          local_13.ContentType = string.Format("multipart/form-data; boundary={0}", (object) local_12);
          ((NameValueCollection) local_13.Headers).Add("X-Auth-Service-Provider", local_4);
          string local_14 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, local_1, (object) local_2, (object) uploadRequest.ConsumerKey, (object) uploadRequest.AccessToken, (object) local_9, (object) local_10, (object) local_11_1);
          ((NameValueCollection) local_13.Headers).Add("X-Verify-Credentials-Authorization", local_14);
          local_13.Method = "POST";
          string local_15 = string.Format("--{0}", (object) local_12);
          string local_16 = string.Format("--{0}--", (object) local_12);
          StringBuilder local_17 = new StringBuilder();
          local_17.AppendLine(local_15);
          string local_18 = "image/png";
          string local_19 = string.Format("Content-Disposition: file; name=\"{0}\"; filename=\"{1}\"", (object) "media", (object) Path.GetFileName(uploadRequest.FilePath));
          string local_20 = Encoding.GetEncoding(local_5).GetString(System.IO.File.ReadAllBytes(uploadRequest.FilePath));
          local_17.AppendLine(local_19);
          local_17.AppendLine(string.Format("Content-Type: {0}", (object) local_18));
          local_17.AppendLine();
          local_17.AppendLine(local_20);
          local_17.AppendLine(local_15);
          local_17.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", (object) "key"));
          local_17.AppendLine();
          local_17.AppendLine(local_0);
          local_17.AppendLine(local_15);
          local_17.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", (object) "message"));
          local_17.AppendLine();
          local_17.AppendLine(uploadRequest.Caption);
          local_17.AppendLine(local_16);
          byte[] local_21 = Encoding.GetEncoding(local_5).GetBytes(((object) local_17).ToString());
          local_13.ContentLength = (long) local_21.Length;
          using (MemoryStream resource_3 = new MemoryStream(local_21))
          {
            using (Stream resource_2 = ((WebRequest) local_13).GetRequestStream())
            {
              long local_24 = 0L;
              byte[] local_25 = new byte[local_21.Length / 100];
              int local_26_1;
              while ((local_26_1 = resource_3.Read(local_25, 0, local_25.Length)) > 0)
              {
                resource_2.Write(local_25, 0, local_26_1);
                resource_2.Flush();
                local_24 += (long) local_26_1;
                int local_29 = (int) ((double) local_24 / (double) local_21.Length * 100.0);
                if (uploadRequest.ProgressCallback != null)
                  uploadRequest.ProgressCallback(local_29);
              }
              using (HttpWebResponse resource_1 = (HttpWebResponse) local_13.GetResponse())
              {
                using (StreamReader resource_0 = new StreamReader(resource_1.GetResponseStream()))
                {
                  string local_32 = resource_0.ReadToEnd();
                  if (resource_1.StatusCode == HttpStatusCode.OK)
                  {
                    XElement local_34 = XDocument.Parse(local_32).Element((XName) "image");
                    if (uploadRequest.CompletedCallback == null)
                      return;
                    uploadRequest.CompletedCallback(local_34.Element((XName) "url").Value);
                  }
                  else if (uploadRequest.ErrorCallback != null)
                    uploadRequest.ErrorCallback((string) (object) resource_1.StatusCode + (object) ": " + resource_1.StatusDescription);
                }
              }
            }
          }
        }
        catch (Exception exception_0)
        {
          if (uploadRequest.ErrorCallback == null)
            return;
          uploadRequest.ErrorCallback(((object) exception_0).ToString());
        }
      })).BeginInvoke((AsyncCallback) null, (object) null);
      return new Action(this.CancelUpload);
    }

    private void CancelUpload()
    {
      this.cancelledFlag = true;
    }
  }
}
