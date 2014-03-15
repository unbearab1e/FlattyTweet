
using System.Runtime.Serialization;

namespace FlattyTweet.Bing
{
  [DataContract]
  public class BingResponse
  {
    [DataMember]
    public string authenticationResultCode { get; set; }

    [DataMember]
    public string brandLogoUri { get; set; }

    [DataMember]
    public string copyright { get; set; }

    [DataMember]
    public BingResponse.ResourceSet[] resourceSets { get; set; }

    [DataMember]
    public string statusCode { get; set; }

    [DataMember]
    public string statusDescription { get; set; }

    [DataMember]
    public string traceId { get; set; }

    [DataContract]
    public class ResourceSet
    {
      [DataMember]
      public int estimatedTotal { get; set; }

      [DataMember]
      public BingResponse.ResourceSet.Resource[] resources { get; set; }

      [DataContract(Name = "Location", Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
      public class Resource
      {
        [DataMember]
        public string __type { get; set; }

        [DataMember]
        public double[] bbox { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public BingResponse.ResourceSet.Resource.Point point { get; set; }

        [DataMember]
        public BingResponse.ResourceSet.Resource.Address address { get; set; }

        [DataMember]
        public string confidence { get; set; }

        [DataMember]
        public string entityType { get; set; }

        [DataContract]
        public class Point
        {
          [DataMember]
          public string type { get; set; }

          [DataMember]
          public string[] coordinates { get; set; }
        }

        [DataContract]
        public class Address
        {
          [DataMember]
          public string addressLine { get; set; }

          [DataMember]
          public string adminDistrict { get; set; }

          [DataMember]
          public string adminDistrict2 { get; set; }

          [DataMember]
          public string countryRegion { get; set; }

          [DataMember]
          public string formattedAddress { get; set; }

          [DataMember]
          public string locality { get; set; }

          [DataMember]
          public string postalCode { get; set; }
        }
      }
    }
  }
}
