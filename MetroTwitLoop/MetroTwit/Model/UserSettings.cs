namespace MetroTwit.Model
{
    using MetroTwit.Extensibility;
    using MetroTwit.Extensions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class UserSettings
    {
        public bool AllReplies = false;
        public bool AutomaticallyShortenLinks = false;
        [XmlArray("Columns"), XmlArrayItem("Column")]
        public MTColumnCollection Columns = new MTColumnCollection();
        public string CurrentURLShorteningService = "MetroTwit.Bitly.BitlyShorteningService";
        public List<string> Filter = new List<string>();
        public int Index;
        private bool IsSaving = false;
        [XmlIgnore]
        public bool LoginCheckCompleted;
        [XmlIgnore]
        public bool SettingsRecreated = false;
        public SerializableDictionary<string, string> StateBag = new SerializableDictionary<string, string>();
        public bool SyncApplicationSettings = false;
        public bool SyncColumns = false;
        public bool SyncColumnsMerge = false;
        public DateTime? SyncLastUpdate = null;
        public decimal TwitterAccountID;
        public string TwitterAccountLang = "en";
        public string TwitterAccountName;
        public bool TwitterAccountProtected = false;
        public string TwitterAuthSecret = string.Empty;
        public string TwitterAuthToken = string.Empty;
        public string TwitterRealName;
        public string TwitterUserImage;
        public string TwitterUserTweetCount;

        public object Clone()
        {
            return base.MemberwiseClone();
        }

        public static UserSettings Instance(decimal TwitterAccountID)
        {
            if (!File.Exists(ApplicationPaths.UserSettings(TwitterAccountID, FileType.UserSettings)))
            {
                return new UserSettings();
            }
            return Load(TwitterAccountID, FileType.UserSettings);
        }

        private static UserSettings Load(decimal TwitterAccountID, FileType UserFileType)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserSettings));
            using (TextReader reader = new StreamReader(ApplicationPaths.UserSettings(TwitterAccountID, UserFileType)))
            {
                try
                {
                    UserSettings settings = (UserSettings)serializer.Deserialize(reader);
                    reader.Close();
                    if (UserFileType == FileType.UserSettingsBackup)
                    {
                        settings.Save(TwitterAccountID);
                    }
                    return settings;
                }
                catch
                {
                }
                reader.Close();
                if (File.Exists(ApplicationPaths.UserSettings(TwitterAccountID, FileType.UserSettingsBackup)) && (UserFileType != FileType.UserSettingsBackup))
                {
                    return Load(TwitterAccountID, FileType.UserSettingsBackup);
                }
                return new UserSettings { SettingsRecreated = true };
            }
        }

        public T LoadObject<T>(T type)
        {
            string key = type.GetType().ToString();
            if (this.StateBag.ContainsKey(key))
            {
                return JsonConvert.DeserializeObject<T>(this.StateBag[key]);
            }
            return default(T);
        }

        public void Save(decimal TwitterAccountID)
        {
            if (!this.IsSaving)
            {
                this.IsSaving = true;
                try
                {
                    try
                    {
                        File.Copy(ApplicationPaths.UserSettings(TwitterAccountID, FileType.UserSettings), ApplicationPaths.UserSettings(TwitterAccountID, FileType.UserSettingsBackup), true);
                    }
                    catch
                    {
                    }
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(UserSettings));
                        using (TextWriter writer = new StreamWriter(ApplicationPaths.UserSettings(TwitterAccountID, FileType.UserSettings)))
                        {
                            serializer.Serialize(writer, this);
                            writer.Close();
                        }
                    }
                    catch
                    {
                    }
                }
                finally
                {
                    this.IsSaving = false;
                }
            }
        }

        public void SaveObject(object targetObjectToSave)
        {
            string key = targetObjectToSave.GetType().ToString();
            string str2 = JsonConvert.SerializeObject(targetObjectToSave);
            if (!this.StateBag.ContainsKey(key))
            {
                this.StateBag.Add(key, str2);
            }
            else
            {
                this.StateBag[key] = str2;
            }
        }

        [XmlIgnore]
        public IURLShorteningService URLShorteningService
        {
            get
            {
                return (from ius in CoreServices.Instance.URLShorteningServices
                        where ius.GetType().ToString() == this.CurrentURLShorteningService
                        select ius).FirstOrDefault<IURLShorteningService>();
            }
            set
            {
                this.CurrentURLShorteningService = value.GetType().ToString();
            }
        }

        [XmlIgnore]
        public string UserAuthSecret
        {
            get
            {
                try
                {
                    return ((this.TwitterAuthSecret != string.Empty) ? this.TwitterAuthSecret.DecryptStringEx() : string.Empty);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                this.TwitterAuthSecret = value.EncryptString();
            }
        }

        [XmlIgnore]
        public string UserAuthToken
        {
            get
            {
                try
                {
                    return ((this.TwitterAuthToken != string.Empty) ? this.TwitterAuthToken.DecryptStringEx() : string.Empty);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                this.TwitterAuthToken = value.EncryptString();
            }
        }
    }
}

