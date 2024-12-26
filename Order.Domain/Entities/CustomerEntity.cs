using BuildBlocks.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities
{
    public class CustomerEntity : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public string Email { get; private set; }

        public static CustomerEntity Create(Guid id, string name, string email)
        {
            return new CustomerEntity
            {
                Id = id,
                Name = name,
                Email = email
            };
        }
    }
}
