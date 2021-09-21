using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebshopBackendApi.Models;

namespace WebshopBackendApi.Repositories
{
    public class UserRepository
    {
        public List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { FirstName = "John", LastName = "Jones"},
            new UserModel() { FirstName = "Robin", LastName = "Mager", Email = "robinmager@home.nl", Password = "HelloWorld123"},
            new UserModel() { FirstName = "Rick", LastName = "Ashley"},
        };


    }
}
