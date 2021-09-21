using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebshopBackendApi.Models;
using WebshopBackendApi.Repositories;
using WebshopBackendApi.DTO;

namespace WebshopBackendApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserRepository UserRepository;

        public UserController(UserRepository repo) => UserRepository = repo;

        [HttpGet]
        public List<UserDTO> GetAll()
        {
            return UserRepository.Users.ConvertAll(user => new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                isAdministrator = user.isAdministrator
            });
        }

        [HttpGet("{uuid}")]
        public UserDTO Get(string uuid)
        {
            UserModel user = UserRepository.Users.Find(item => item.Id == uuid);

            return new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                isAdministrator = user.isAdministrator
            };
        }

    }
}
