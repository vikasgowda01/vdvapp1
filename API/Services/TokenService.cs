using API.Interfaces;
using API.entites;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenServices
    {
        private readonly SymmetricSecurityKey _key;//feild and private field so we are using _key
        //two type of keys
        //SymmetricSecurityKey same key is used encrypt and decrypt the data
        //ASymmetricSecurityKey when server needs encrypt something and client needs to decrypt something that time we need both private and public key, private key with server and public key in client
        public TokenService(IConfiguration config)//we are going to store super secret key, we need to configure inject Iconfiguration config
        {
            _key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            //symmetricsecuritykey only takes values in bytes so we are using encoding.utf.getbytes
            //config[TokenKey] contains key value pair this we specified in the Appsettings.development.json file
            //in appsettings.Development.json - "tokenKey":"super secret unguessable key" its length should be 64characters for recent security packages
        }

        //creating a token
        public string CreateToken(AppUser user)
        {   
            //claim that u are u 
            // bit of information user claims 
            // adding list of claims - it might be more claims done by user
            var claims =new List<Claim>
             {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                
                //here we are using username to claim
             };
            // credentials here we speicy key, algorithm, and signature
             var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            // describing the token we are going to return
             var tokenDescriptor = new SecurityTokenDescriptor
             {
                Subject = new ClaimsIdentity(claims), //claim that want to return contains id name
                Expires = DateTime.Now.AddDays(7),// expire date here we gave 7
                SigningCredentials = creds // signingcredentials that we specifed above

             } ;
             //now token handler
             var tokenHandler =new JwtSecurityTokenHandler();
             var token=tokenHandler.CreateToken(tokenDescriptor);

             return tokenHandler.WriteToken(token);
    }   }
}