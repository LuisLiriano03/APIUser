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
        Task<List<UserInformationDTO>> List();
        Task<UserInformationDTO> Create(UserInformationDTO model);
        Task<bool> Edit(UserInformationDTO model);
        Task<bool> Delete(int id);
    }
}
