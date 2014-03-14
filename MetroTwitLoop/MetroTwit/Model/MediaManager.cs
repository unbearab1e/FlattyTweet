namespace MetroTwit.Model
{
    using MetroTwit.Extensions;
    using System;
    using System.Web;
    using Twitterizer.Models;

    internal static class MediaManager
    {
        internal static MediaEntity CheckEntityandCreateMedia(UrlEntity u)
        {
            string absolutePath;
            if (((((!u.ExpandedUrl.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase) && !u.ExpandedUrl.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase)) && (!u.ExpandedUrl.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase) && !u.ExpandedUrl.Contains("instagr.am", StringComparison.InvariantCultureIgnoreCase))) && ((!u.ExpandedUrl.Contains("instagram.com", StringComparison.InvariantCultureIgnoreCase) && !u.ExpandedUrl.Contains("yfrog.com", StringComparison.InvariantCultureIgnoreCase)) && (!u.ExpandedUrl.Contains("twitpic.com", StringComparison.InvariantCultureIgnoreCase) && !u.ExpandedUrl.Contains("youtube.com", StringComparison.InvariantCultureIgnoreCase)))) && ((!u.ExpandedUrl.Contains("youtu.be", StringComparison.InvariantCultureIgnoreCase) && !u.ExpandedUrl.Contains("sdrv.ms", StringComparison.InvariantCultureIgnoreCase)) && !u.ExpandedUrl.Contains("skydrive.live.com", StringComparison.InvariantCultureIgnoreCase))) && !u.ExpandedUrl.Contains("lockerz", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            MediaEntity entity = new MediaEntity
            {
                DisplayUrl = u.DisplayUrl,
                ExpandedUrl = u.ExpandedUrl,
                Url = u.Url,
                EndIndex = u.EndIndex,
                StartIndex = u.StartIndex,
                MediaType = MediaEntity.MediaTypes.Photo
            };
            if (u.ExpandedUrl.Contains("instagr.am", StringComparison.InvariantCultureIgnoreCase) || u.ExpandedUrl.Contains("instagram.com", StringComparison.InvariantCultureIgnoreCase))
            {
                absolutePath = u.ExpandedUrl.EndsWith("/") ? u.ExpandedUrl.Remove(u.ExpandedUrl.Length - 1) : u.ExpandedUrl;
                entity.MediaUrl = absolutePath + "/media/";
                entity.MediaUrlSecure = absolutePath + "/media/";
                return entity;
            }
            if (u.ExpandedUrl.Contains("yfrog.com", StringComparison.InvariantCultureIgnoreCase))
            {
                entity.MediaUrl = u.ExpandedUrl + ":medium";
                entity.MediaUrlSecure = u.ExpandedUrl + ":medium";
                return entity;
            }
            if (u.ExpandedUrl.Contains("twitpic.com", StringComparison.InvariantCultureIgnoreCase))
            {
                absolutePath = new Uri(u.ExpandedUrl).AbsolutePath;
                entity.MediaUrl = "https://twitpic.com/show/full" + absolutePath;
                entity.MediaUrlSecure = u.ExpandedUrl;
                return entity;
            }
            if (u.ExpandedUrl.Contains("youtube.com", StringComparison.InvariantCultureIgnoreCase) || u.ExpandedUrl.Contains("youtu.be", StringComparison.InvariantCultureIgnoreCase))
            {
                entity.MediaType = MediaEntity.MediaTypes.Unknown;
                Uri uri = new Uri(u.ExpandedUrl);
                if (u.ExpandedUrl.Contains("v=", StringComparison.InvariantCultureIgnoreCase))
                {
                    absolutePath = HttpUtility.ParseQueryString(uri.Query).Get("v");
                }
                else
                {
                    absolutePath = uri.AbsolutePath.Replace("/", "");
                }
                if (!string.IsNullOrEmpty(absolutePath))
                {
                    entity.MediaUrl = "http://i.ytimg.com/vi/" + absolutePath + "/0.jpg";
                    entity.MediaUrlSecure = u.ExpandedUrl;
                    return entity;
                }
                return null;
            }
            if (u.ExpandedUrl.Contains("sdrv.ms", StringComparison.InvariantCultureIgnoreCase) || u.ExpandedUrl.Contains("skydrive.live.com", StringComparison.InvariantCultureIgnoreCase))
            {
                entity.MediaUrl = "https://apis.live.net/v5.0/skydrive/get_item_preview?type=normal&url=" + HttpUtility.UrlEncode(u.ExpandedUrl);
                entity.MediaUrlSecure = "https://apis.live.net/v5.0/skydrive/get_item_preview?type=normal&url=" + HttpUtility.UrlEncode(u.ExpandedUrl);
                return entity;
            }
            if (u.ExpandedUrl.Contains("lockerz", StringComparison.OrdinalIgnoreCase))
            {
                entity.MediaUrl = "http://api.plixi.com/api/tpapi.svc/imagefromurl?url=" + u.ExpandedUrl + "&size=medium";
                entity.MediaUrlSecure = "http://api.plixi.com/api/tpapi.svc/imagefromurl?url=" + u.ExpandedUrl + "&size=medium";
                return entity;
            }
            entity.MediaUrl = u.ExpandedUrl;
            entity.MediaUrlSecure = u.ExpandedUrl;
            return entity;
        }
    }
}

