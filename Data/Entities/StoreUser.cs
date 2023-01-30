using Microsoft.AspNetCore.Identity;

namespace DutchTreat.Data.Entities
{
    // class for the identity
    public class StoreUser : IdentityUser
    {
        // identityuser has a whole lot of properties

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
