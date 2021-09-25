using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebshopBackendApi.Models
{
    public class UserModel
    {
        public UserModel()
        {
          Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isAdministrator { get; set; }
        public Guid CartId { get; set; }
    }
}
