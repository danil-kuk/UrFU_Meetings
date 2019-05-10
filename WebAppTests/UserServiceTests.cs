using System;
using WebApp.Controllers;
using WebApp.Services;
using WebApp.Helpers;
using Xunit;
using WebApp;
using WebApp.Models.DataModels.Entities;
using Microsoft.Extensions.Options;
using WebApp.Services.Interfaces;
using WebApp.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace WebAppTests
{
    public class UserServiceTests
    {
        private readonly DataBaseService<User> _userDatabaseService;
        private readonly UserService _userService;
        private readonly EFDBContext _context;
        private readonly User testUser;
        private readonly Random random;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<EFDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new EFDBContext(options);
            random = new Random();
            _userDatabaseService = new DataBaseService<User>(_context);
            _userService = new UserService(_userDatabaseService);
            testUser = new User
            {
                Name = "Имя",
                Surname = "Фамилия",
                Email = "test@test.com",
                Password = "ABC"
            };
            CreateUsersDatabase();
        }

        private void CreateUsersDatabase()
        {
            if (_context.Users.Count() >= 8)
                return;
            for (int i = 1; i < 10; i++)
            {
                var newTestUser = new User
                {
                    Name = "Имя" + i,
                    Surname = "Фамилия" + i,
                    Email = "test@test.com" + i,
                    Password = "ABC" + i
                };
                _userDatabaseService.Insert(newTestUser);
            }
        }

        private void CheckUserData(User expectedUser, User actualUser)
        {
            Assert.Equal(expectedUser.Name, actualUser.Name);
            Assert.Equal(expectedUser.Surname, actualUser.Surname);
            Assert.Equal(expectedUser.Email, actualUser.Email);
            Assert.Equal(expectedUser.Password, actualUser.Password);
        }

        private User GetRandomTestUser()
        {
            var id = random.Next(10);
            return new User
            {
                Name = "Имя" + id,
                Surname = "Фамилия" + id,
                Email = "test@test.com" + id,
                Password = "ABC" + id
            };
        }

        [Fact]
        public void InsertNewTestUser()
        {
            //Arrange
            var userToInsert = testUser;
            var startDbRecordsCount = _context.Users.Count();

            //Act
            _userService.InsertUser(userToInsert);

            //Assert
            var actualRes = _context.Users.Count();
            var expectedRes = startDbRecordsCount + 1;
            Assert.Equal(expectedRes, actualRes);
            Assert.Contains(userToInsert, _userDatabaseService.GetAll());
        }

        [Fact]
        public void DeleteLastUser()
        {
            var startDbRecordsCount = _context.Users.Count();
            var userToDelete = _userDatabaseService.GetAll().LastOrDefault();

            _userService.DeleteUser(userToDelete);

            var actualRes = _context.Users.Count();
            var expectedRes = startDbRecordsCount - 1;
            Assert.Equal(expectedRes, actualRes);
            Assert.DoesNotContain(userToDelete, _userDatabaseService.GetAll());
        }

        [Fact]
        public void DeleteTestUser()
        {
            _userService.InsertUser(testUser);
            var startDbRecordsCount = _context.Users.Count();
            var userToDelete = testUser;

            _userService.DeleteUser(userToDelete);

            var actualRes = _context.Users.Count();
            var expectedRes = startDbRecordsCount - 1;
            Assert.Equal(expectedRes, actualRes);
            Assert.DoesNotContain(userToDelete, _userDatabaseService.GetAll());
        }

        [Fact]
        public void FindByIdTestUser()
        {
            var id = random.Next(10);
            var actualUser = _userService.GetById(id);
            var expectedUser = new User
            {
                Name = "Имя" + id,
                Surname = "Фамилия" + id,
                Email = "test@test.com" + id,
                Password = "ABC" + id
            };

            CheckUserData(expectedUser, actualUser);
        }

        [Fact]
        public void FindByNameTestUser()
        {
            var expectedUser = GetRandomTestUser();
            var actualUser = _userService.GetByFilter(u => u.Name == expectedUser.Name);

            CheckUserData(expectedUser, actualUser);
        }

        [Fact]
        public void FindBySurnameTestUser()
        {
            var expectedUser = GetRandomTestUser();
            var actualUser = _userService.GetByFilter(u => u.Surname == expectedUser.Surname);

            CheckUserData(expectedUser, actualUser);
        }

        [Fact]
        public void FindByEmailTestUser()
        {
            var expectedUser = GetRandomTestUser();
            var actualUser = _userService.GetByFilter(u => u.Email == expectedUser.Email);

            CheckUserData(expectedUser, actualUser);
        }

        [Fact]
        public void UpdateTestUser()
        {
            var rnd = random.Next(10);
            var newUser = new User
            {
                Name = rnd + "Имя",
                Surname = rnd + "Фамилия",
                Email = rnd + "test@test.com",
                Password = rnd + "ABC"
            };
            _userDatabaseService.Insert(newUser);

            newUser.Name += rnd;
            newUser.Surname += rnd;
            newUser.Email += rnd;
            newUser.Password += rnd;
            _userService.UpdateUser(newUser);

            var actualRes = _context.Users.Where(u => u.Email == (rnd + testUser.Email + rnd)).FirstOrDefault();
            var expectedRes = newUser;
            CheckUserData(expectedRes, actualRes);
        }
    }
}
