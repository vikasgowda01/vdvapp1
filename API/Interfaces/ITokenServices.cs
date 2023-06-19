using API.entites;
namespace API.Interfaces
{
    public interface ITokenServices
    {
        string CreateToken(AppUser user);//contract between interface and implementaion 
    }
}