using System.Collections.Generic;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.ValueObjects
{
    public class PersonalName : ValueObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        #region ToString()
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
        #endregion

        #region GetEqualityComponents()
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
        #endregion
    }
}
