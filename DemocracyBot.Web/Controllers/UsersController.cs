using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository;
using DemocracyBot.Web.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DemocracyBot.Web.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("birthDate")]
        public async Task<IActionResult> SetBirthDate(UpdateUserBirthDateRequest request)
        {
            var user = await _userRepository.GetUser(request.ChatId, request.UserId);

            user.BirthDate = request.BirthDate;

            await _userRepository.UpdateUser(user);

            return Ok();
        }
    }
}