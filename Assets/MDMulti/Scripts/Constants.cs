// using https://app.quicktype.io/#l=cs&r=json2csharp

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MDMulti.Constants
{
    public partial class ConstantsObject
    {
        [JsonProperty("this_loc", Required = Required.Always)]
        public Uri ThisLoc { get; set; }

        [JsonProperty("version", Required = Required.Always)]
        public long Version { get; set; }

        [JsonProperty("mdmc_version", Required = Required.Always)]
        public long MdmcVersion { get; set; }

        [JsonProperty("mdms_version", Required = Required.Always)]
        public long MdmsVersion { get; set; }

        [JsonProperty("peerdb_file", Required = Required.Always)]
        public PeerdbFile PeerdbFile { get; set; }

        [JsonProperty("oid", Required = Required.Always)]
        public Oid Oid { get; set; }

        [JsonProperty("lan", Required = Required.Always)]
        public Lan Lan { get; set; }
    }

    public partial class Lan
    {
        [JsonProperty("multicast", Required = Required.Always)]
        public Multicast Multicast { get; set; }

        [JsonProperty("broadcast", Required = Required.Always)]
        public Broadcast Broadcast { get; set; }
    }

    public partial class Broadcast
    {
        [JsonProperty("port", Required = Required.Always)]
        public int Port { get; set; }
    }

    public partial class Multicast
    {
        [JsonProperty("address", Required = Required.Always)]
        public string Address { get; set; }

        [JsonProperty("port", Required = Required.Always)]
        public int Port { get; set; }
    }

    public partial class Oid
    {
        [JsonProperty("base", Required = Required.Always)]
        public string Base { get; set; }
    }

    public partial class PeerdbFile
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("version", Required = Required.Always)]
        public long Version { get; set; }
    }

    public partial class ConstantsObject
    {
        public static ConstantsObject FromJson(string json) => JsonConvert.DeserializeObject<ConstantsObject>(json, MDMulti.Constants.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ConstantsObject self) => JsonConvert.SerializeObject(self, MDMulti.Constants.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
