using System.Collections.Generic;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        #region GetEqualityComponents()
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
        }
        #endregion

        #region ToString()
        public override string ToString()
        {
            return $"{Street}, {ZipCode} {City},\n{State}, {Country}";
        }
        #endregion
    }
}
