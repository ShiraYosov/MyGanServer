using MyGanServerBL.Models;
namespace MyGanServer.DTO
{
    public class RegisterUserDto
    {
        public User User { get; set; }
        public Student Student { get; set; }
        public StudentOfUser StudentOfUser { get; set; }
    }
}
