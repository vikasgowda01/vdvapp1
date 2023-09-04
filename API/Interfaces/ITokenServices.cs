using API.entites;
namespace API.Interfaces
{
    public interface ITokenServices
    {
        Task<string> CreateToken(AppUser user);//contract between interface and implementaion 
    }
}