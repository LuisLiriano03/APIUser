using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.DTOs;

namespace User.BLL.Services.Contracts
{
    public interface IUserService
    {
        Task<List<UserInformationDTO>> AllUsersList();
        Task<UserInformationDTO> CreateUser(UserInformationDTO model);
        Task<bool> ModifyUser(UserInformationDTO model);
        Task<bool> EliminateUser(int id);
    }
}
