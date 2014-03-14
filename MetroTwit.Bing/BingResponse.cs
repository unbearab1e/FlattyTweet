// Type: MetroTwit.Bing.BingResponse
// Assembly: MetroTwit.Bing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D3D736DA-69B5-45EA-AE15-6F850EDBB636
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Bing.dll

using System.Runtime.Serialization;

namespace MetroTwit.Bing
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
