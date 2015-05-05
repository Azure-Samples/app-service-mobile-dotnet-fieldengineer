using System;

namespace FieldEngineerLite.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string HouseNumberOrName { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string PrimaryContactNumber { get; set; }
        public string AdditionalContactNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string WorkPerformed {get; set;}

        public string Address 
        {
            get { return string.Format ("{0} {1} {2} {3} {4}", HouseNumberOrName, Street, Town, County, Postcode); }
        }
    }
}
