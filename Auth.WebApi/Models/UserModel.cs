using BuildBlocks.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Auth.WebApi.Models
{
    public class UserModel : IdentityUser
    {
        public IEnumerable<Address> Addresses { get; set; } = [];
    }
}
