using NuGet.Protocol.Plugins;
using ViewModel.Common;
using ViewModel.Users;

namespace Application.System.Users
{
    public interface IUserService
    {
        Task<ResultModel<string>> Authencate(LoginVM request);

        Task<ResultModel<bool>> Register(RegisterVM request);

        Task<ResultModel<bool>> Update(string id, UpdateUserVM request);

        Task<PagedResult<UserVM>> GetUsersPaging(GetUserPagingRequest request);

        Task<UserVM> GetById(string id);

        Task<bool> Delete(string id);

		Task<string> GetUserID();
	}
}
