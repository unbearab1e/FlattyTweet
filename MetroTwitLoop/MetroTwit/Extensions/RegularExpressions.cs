namespace MetroTwit.Extensions
{
    using System;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using Twitterizer.Models;

    public static class RegularExpressions
    {
        public static Regex AT_SIGNS;
        private static string AT_SIGNS_CHARS;
        public static Regex AUTO_LINK_HASHTAGS;
        public static int AUTO_LINK_HASHTAGS_GROUP_BEFORE;
        public static int AUTO_LINK_HASHTAGS_GROUP_HASH;
        public static int AUTO_LINK_HASHTAGS_GROUP_TAG;
        public static int AUTO_LINK_USERNAME_OR_LISTS_GROUP_AT;
        public static int AUTO_LINK_USERNAME_OR_LISTS_GROUP_BEFORE;
        public static int AUTO_LINK_USERNAME_OR_LISTS_GROUP_LIST;
        public static int AUTO_LINK_USERNAME_OR_LISTS_GROUP_USERNAME;
        public static Regex AUTO_LINK_USERNAMES_OR_LISTS;
        public static Regex EXTRACT_MENTIONS;
        public static int EXTRACT_MENTIONS_GROUP_BEFORE;
        public static int EXTRACT_MENTIONS_GROUP_USERNAME;
        public static Regex EXTRACT_REPLY;
        public static int EXTRACT_REPLY_GROUP_USERNAME;
        private static string HASHTAG_ALPHA;
        private static string HASHTAG_ALPHA_CHARS;
        private static string HASHTAG_ALPHA_NUMERIC;
        private static string HASHTAG_ALPHA_NUMERIC_CHARS;
        public static Regex HASHTAG_MATCH_END;
        public static readonly Regex HTML_ANCHOR_EXPRESSION;
        internal static readonly Regex HTML_TITLE;
        private static string LATIN_ACCENTS_CHARS;
        public static Regex SCREEN_NAME_MATCH_END;
        private static string UNICODE_SPACE_RANGES;
        private static string URL_BALANCED_PARENS;
        private static string URL_PUNYCODE;
        private static string URL_VALID_CCTLD;
        private static string URL_VALID_CHARS;
        private static string URL_VALID_DOMAIN;
        private static string URL_VALID_DOMAIN_NAME;
        private static string URL_VALID_GENERAL_PATH_CHARS;
        private static string URL_VALID_GTLD;
        private static string URL_VALID_PATH;
        private static string URL_VALID_PATH_ENDING_CHARS;
        private static string URL_VALID_PORT_NUMBER;
        private static string URL_VALID_PRECEEDING_CHARS;
        private static string URL_VALID_SUBDOMAIN;
        private static string URL_VALID_UNICODE_CHARS;
        private static string URL_VALID_URL_QUERY_CHARS;
        private static string URL_VALID_URL_QUERY_ENDING_CHARS;
        public static Regex VALID_TCO_URL;
        public static Regex VALID_URL;
        public static int VALID_URL_GROUP_ALL;
        public static int VALID_URL_GROUP_BEFORE;
        public static int VALID_URL_GROUP_DOMAIN;
        public static int VALID_URL_GROUP_PATH;
        public static int VALID_URL_GROUP_PORT;
        public static int VALID_URL_GROUP_PROTOCOL;
        public static int VALID_URL_GROUP_QUERY_STRING;
        public static int VALID_URL_GROUP_URL;
        private static string VALID_URL_PATTERN_STRING;

        static RegularExpressions()
        {
            AT_SIGNS_CHARS = "@＠";
            AT_SIGNS = new Regex("[" + AT_SIGNS_CHARS + "]");
            EXTRACT_REPLY_GROUP_USERNAME = 1;
            AUTO_LINK_HASHTAGS_GROUP_BEFORE = 1;
            AUTO_LINK_HASHTAGS_GROUP_HASH = 2;
            AUTO_LINK_HASHTAGS_GROUP_TAG = 3;
            AUTO_LINK_USERNAME_OR_LISTS_GROUP_AT = 2;
            AUTO_LINK_USERNAME_OR_LISTS_GROUP_BEFORE = 1;
            AUTO_LINK_USERNAME_OR_LISTS_GROUP_LIST = 4;
            AUTO_LINK_USERNAME_OR_LISTS_GROUP_USERNAME = 3;
            EXTRACT_MENTIONS_GROUP_BEFORE = 1;
            EXTRACT_MENTIONS_GROUP_USERNAME = 2;
            URL_PUNYCODE = "(?:xn--[0-9a-z]+)";
            LATIN_ACCENTS_CHARS = @"\u00c0-\u00d6\u00d8-\u00f6\u00f8-\u00ff\u0100-\u024f\u0253\u0254\u0256\u0257\u0259\u025b\u0263\u0268\u026f\u0272\u0289\u028b\u02bb\u0300-\u036f\u1e00-\u1eff";
            HASHTAG_ALPHA_CHARS = ("a-z" + LATIN_ACCENTS_CHARS + @"\u0400-\u04ff\u0500-\u0527\u2de0-\u2dff\ua640-\ua69f\u0591-\u05bd\u05bf\u05c1-\u05c2\u05c4-\u05c5\u05c7\u05d0-\u05ea\u05f0-\u05f2\ufb1d-\ufb28\ufb2a-\ufb36\ufb38-\ufb3c\ufb3e\ufb40-\ufb41\ufb43-\ufb44\ufb46-\ufb4f\u0610-\u061a\u0620-\u065f\u066e-\u06d3\u06d5-\u06dc\u06de-\u06e8\u06ea-\u06ef\u06fa-\u06fc\u06ff\u0750-\u077f\u08a0\u08a2-\u08ac\u08e4-\u08fe\ufb50-\ufbb1\ufbd3-\ufd3d\ufd50-\ufd8f\ufd92-\ufdc7\ufdf0-\ufdfb\ufe70-\ufe74\ufe76-\ufefc\u200c\u0e01-\u0e3a\u0e40-\u0e4e\u1100-\u11ff\u3130-\u3185\uA960-\uA97F\uAC00-\uD7AF\uD7B0-\uD7FF\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}\u3005\u303b\uff21-\uff3a\uff41-\uff5a\uff66-\uff9f\uffa1-\uffdc");
            HASHTAG_ALPHA_NUMERIC_CHARS = (@"0-9\uff10-\uff19_" + HASHTAG_ALPHA_CHARS);
            HASHTAG_ALPHA = ("[" + HASHTAG_ALPHA_CHARS + "]");
            HASHTAG_ALPHA_NUMERIC = ("[" + HASHTAG_ALPHA_NUMERIC_CHARS + "]");
            AUTO_LINK_HASHTAGS = new Regex("(^|[^&/" + HASHTAG_ALPHA_NUMERIC_CHARS + "])(#|＃)(" + HASHTAG_ALPHA_NUMERIC + "*" + HASHTAG_ALPHA + HASHTAG_ALPHA_NUMERIC + "*)", RegexOptions.IgnoreCase);
            AUTO_LINK_USERNAMES_OR_LISTS = new Regex(string.Concat(new object[] { "([^a-z0-9_!#$%&*", AT_SIGNS_CHARS, "]|^|RT:?)(", AT_SIGNS, @"+)([a-z0-9_]{1,20})(/[a-z][a-z0-9_\-]{0,24})?" }), RegexOptions.IgnoreCase);
            EXTRACT_MENTIONS = new Regex(string.Concat(new object[] { "(^|[^a-z0-9_!#$%&*", AT_SIGNS_CHARS, "])", AT_SIGNS, "([a-z0-9_]{1,20})" }), RegexOptions.IgnoreCase);
            UNICODE_SPACE_RANGES = @"\u0009-\u000d\u0020\u0085\u00a0\u1680\u180E\u2000-\u200a\u2028\u2029\u202F\u205F\u3000";
            EXTRACT_REPLY = new Regex(string.Concat(new object[] { "^(?:[", UNICODE_SPACE_RANGES, "])*", AT_SIGNS, "([a-z0-9_]{1,20})" }), RegexOptions.IgnoreCase);
            HASHTAG_MATCH_END = new Regex("^(?:[#＃]|://)");
            HTML_ANCHOR_EXPRESSION = new Regex("<a.*href=[\"'](?<url>[^\"^']+[.]*)[\"'].*>(?<name>[^<]+[.]*)</a>");
            HTML_TITLE = new Regex("<title.*?>(.*?)</title>");
            URL_VALID_GENERAL_PATH_CHARS = (@"[a-z0-9!\*';:=\+,.\$/%#\[\]\-_~\|&" + LATIN_ACCENTS_CHARS + "]");
            SCREEN_NAME_MATCH_END = new Regex("^(?:[" + AT_SIGNS_CHARS + LATIN_ACCENTS_CHARS + "]|://)");
            URL_BALANCED_PARENS = (@"\(" + URL_VALID_GENERAL_PATH_CHARS + @"+\)");

            URL_VALID_CCTLD = @"(?:(?:ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|ss|st|su|sv|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|za|zm|zw)(?=\P{L}|$))";
            URL_VALID_CHARS = (@"\p{L}\p{N}" + LATIN_ACCENTS_CHARS);
            URL_VALID_GTLD = @"(?:(?:aero|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|xxx)(?=\P{L}|$))";
            URL_VALID_PATH_ENDING_CHARS = (@"[a-z0-9=_#/\-\+" + LATIN_ACCENTS_CHARS + "]|(?:" + URL_BALANCED_PARENS + ")");
            URL_VALID_PORT_NUMBER = "[0-9]+";
            URL_VALID_PATH = ("(?:(?:" + URL_VALID_GENERAL_PATH_CHARS + "*(?:" + URL_BALANCED_PARENS + URL_VALID_GENERAL_PATH_CHARS + "*)*" + URL_VALID_PATH_ENDING_CHARS + ")|(?:@" + URL_VALID_GENERAL_PATH_CHARS + "+/))");
            URL_VALID_SUBDOMAIN = ("(?:(?:[" + URL_VALID_CHARS + "][" + URL_VALID_CHARS + @"\-_]*)?[" + URL_VALID_CHARS + @"]\.)");
            URL_VALID_PRECEEDING_CHARS = "(?:[^\\-/\"'!=A-Z0-9_@＠$#＃.\\u202A-\\u202E]|^)";
            URL_VALID_DOMAIN_NAME = ("(?:(?:[" + URL_VALID_CHARS + "][" + URL_VALID_CHARS + @"\-]*)?[" + URL_VALID_CHARS + @"]\.)");
            URL_VALID_UNICODE_CHARS = @"[.[^\p{P}\p{S}\s\p{Z}\p{IsGeneralPunctuation}]]";
            URL_VALID_DOMAIN = ("(?:" + URL_VALID_SUBDOMAIN + "+" + URL_VALID_DOMAIN_NAME + "(?:" + URL_VALID_GTLD + "|" + URL_VALID_CCTLD + "|" + URL_PUNYCODE + "))|(?:" + URL_VALID_DOMAIN_NAME + "(?:" + URL_VALID_GTLD + "|" + URL_PUNYCODE + "))|(?:(?<=https?://)(?:(?:" + URL_VALID_DOMAIN_NAME + URL_VALID_CCTLD + ")|(?:" + URL_VALID_UNICODE_CHARS + @"+\.(?:" + URL_VALID_GTLD + "|" + URL_VALID_CCTLD + "))))|(?:" + URL_VALID_DOMAIN_NAME + URL_VALID_CCTLD + "(?=/))");

            URL_VALID_URL_QUERY_CHARS = @"[a-z0-9!?\*'\(\);:&=\+\$/%#\[\]\-_\.,~\|]";
            URL_VALID_URL_QUERY_ENDING_CHARS = "[a-z0-9_&=#/]";
            VALID_TCO_URL = new Regex("^https?://t.co/[a-z0-9]+", RegexOptions.IgnoreCase);
            VALID_URL_PATTERN_STRING = ("((" + URL_VALID_PRECEEDING_CHARS + ")((https?://)?(" + URL_VALID_DOMAIN + ")(?::(" + URL_VALID_PORT_NUMBER + "))?(/" + URL_VALID_PATH + @"*)?(\?" + URL_VALID_URL_QUERY_CHARS + "*" + URL_VALID_URL_QUERY_ENDING_CHARS + ")?))");
            VALID_URL = new Regex(VALID_URL_PATTERN_STRING, RegexOptions.IgnoreCase);
            VALID_URL_GROUP_ALL = 1;
            VALID_URL_GROUP_BEFORE = 2;
            VALID_URL_GROUP_DOMAIN = 5;
            VALID_URL_GROUP_PATH = 7;
            VALID_URL_GROUP_PORT = 6;
            VALID_URL_GROUP_PROTOCOL = 4;
            VALID_URL_GROUP_QUERY_STRING = 8;
            VALID_URL_GROUP_URL = 3;
            


        }

        public static EntityCollection ExtractEntities(string Text)
        {
            EntityCollection entitys = new EntityCollection();
            if (Text.Contains("@") || Text.Contains("＠"))
            {
                MatchCollection matchs = EXTRACT_MENTIONS.Matches(Text);
                foreach (Match match in matchs)
                {
                    if (!SCREEN_NAME_MATCH_END.Match(match.Groups[EXTRACT_MENTIONS_GROUP_USERNAME].Value).Success)
                    {
                        MentionEntity uninitializedObject = (MentionEntity)FormatterServices.GetUninitializedObject(typeof(MentionEntity));
                        uninitializedObject.StartIndex = match.Groups[EXTRACT_MENTIONS_GROUP_USERNAME].Index - 1;
                        uninitializedObject.EndIndex = match.Groups[EXTRACT_MENTIONS_GROUP_USERNAME].Index + match.Groups[EXTRACT_MENTIONS_GROUP_USERNAME].Length;
                        uninitializedObject.ScreenName = match.Groups[EXTRACT_MENTIONS_GROUP_USERNAME].Value.Replace("@", "").Trim();
                        entitys.Add(uninitializedObject);
                    }
                }
            }
            if (Text.Contains("#") || Text.Contains("＃"))
            {
                MatchCollection matchs2 = AUTO_LINK_HASHTAGS.Matches(Text);
                foreach (Match match in matchs2)
                {
                    HashTagEntity item = (HashTagEntity)FormatterServices.GetUninitializedObject(typeof(HashTagEntity));
                    item.StartIndex = match.Groups[AUTO_LINK_HASHTAGS_GROUP_HASH].Index;
                    item.EndIndex = (match.Groups[AUTO_LINK_HASHTAGS_GROUP_HASH].Index + match.Groups[AUTO_LINK_HASHTAGS_GROUP_HASH].Length) + match.Groups[AUTO_LINK_HASHTAGS_GROUP_TAG].Length;
                    item.Text = match.Groups[AUTO_LINK_HASHTAGS_GROUP_TAG].Value;
                    entitys.Add(item);
                }
            }
            if (Text.Contains(":") || Text.Contains("."))
            {
                MatchCollection matchs3 = VALID_URL.Matches(Text);
                foreach (Match match in matchs3)
                {
                    if (!string.IsNullOrEmpty(match.Groups[VALID_URL_GROUP_PROTOCOL].Value))
                    {
                        UrlEntity entity3 = (UrlEntity)FormatterServices.GetUninitializedObject(typeof(UrlEntity));
                        entity3.StartIndex = match.Groups[VALID_URL_GROUP_URL].Index;
                        entity3.EndIndex = match.Groups[VALID_URL_GROUP_URL].Index + match.Groups[VALID_URL_GROUP_URL].Length;
                        entity3.Url = match.Groups[VALID_URL_GROUP_URL].Value;
                        entitys.Add(entity3);
                    }
                }
            }
            return entitys;
        }

        internal static string ExtractHTMLTitle(string p)
        {
            Match match = HTML_TITLE.Match(p);
            if (match != null)
            {
                return match.Groups[1].Value;
            }
            return "Unknown Twitter Error.";
        }
    }
}

