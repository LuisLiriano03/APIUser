﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.BLL.Services.Contracts;
using User.DAL.Repository.Contracts;
using User.DTOs;
using User.Model;

namespace User.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IGenericRepository<UserInformation> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<UserInformation> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<UserInformationDTO>> List()
        {
            try
            {
                var UserQuery = await _userRepository.Consult();
                return _mapper.Map<List<UserInformationDTO>>(UserQuery.ToList());
            }
            catch
            {
                throw;
            }

        }

        public async Task<UserInformationDTO> Create(UserInformationDTO model)
        {
            try
            {
                var userCreated = await _userRepository.Create(_mapper.Map<UserInformation>(model));

                if (userCreated.UserId == 0)
                    throw new TaskCanceledException("Could not be created");

                var query = await _userRepository.Consult(u => u.UserId == userCreated.UserId);

                return _mapper.Map<UserInformationDTO>(userCreated);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(UserInformationDTO model)
        {
            try
            {
                var UserModel = _mapper.Map<UserInformation>(model);
                var userFound = await _userRepository.Get(u => u.UserId == UserModel.UserId);

                if (userFound == null)
                    throw new TaskCanceledException("The user does not exist.");

                userFound.FirstName = UserModel.FirstName;
                userFound.LastName = UserModel.LastName;
                userFound.LastName = UserModel.LastName;
                userFound.Age = UserModel.Age;
                userFound.Email = UserModel.Email;
                userFound.UserPassword = UserModel.UserPassword;
                userFound.IsActive = UserModel.IsActive;

                bool response = await _userRepository.Edit(userFound);

                if (!response)
                    throw new TaskCanceledException("Could not edit");

                return response;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var userFound = await _userRepository.Get(u => u.UserId == id);

                if (userFound == null)
                    throw new TaskCanceledException("The user does not exist.");

                bool response = await _userRepository.Delete(userFound);

                if (!response)
                    throw new TaskCanceledException("Could not delete");

                return response;
            }
            catch
            {
                throw;
            }
        }

        

        
    }
}
