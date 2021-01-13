using BankApplication.BlazorFrontend.Models;
using BankApplication.SharedLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.BlazorFrontend.Services
{
    public interface IUserManagmentService
    {
        StorageUserModel User { get; }
        Task Initialize();
        Task Login(LoginModel model);
        Task Logout();
        Task Register(UserModel model);
        Task Update(UserModel model);

    }
    public class UserManagmentService : IUserManagmentService
    {
        public StorageUserModel User { get; private set; }
        private readonly ILocalStorageService _localStorageService;
        private readonly IApiComunicationService _apiComunicationService;
        private readonly string _baseAdress = "http://localhost:61005/api/Users";

        public UserManagmentService(ILocalStorageService localStorageService, IApiComunicationService apiComunicationService)
        {
            _localStorageService = localStorageService;
            _apiComunicationService = apiComunicationService;
        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<StorageUserModel>("user");
        }

        public async Task Login(LoginModel model)
        {
            User = await _apiComunicationService.Post<StorageUserModel>(_baseAdress+"/login", model);
            await _localStorageService.SetItem("user", User);
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItem("user");
        }

        public async Task Register(UserModel model)
        {
           await _apiComunicationService.Post<UserModel>(_baseAdress + "/register", model);
        }

        public Task Update(UserModel model)
        {
            throw new NotImplementedException();
        }
    }
}
