using Microsoft.AspNetCore.Identity;

namespace APIDevSteam.Models
{
    public class Usuario : IdentityUser
    {
        public string? NomeCompleto { get; set; }
        public DateOnly DataNascimento { get; set; }

        public Usuario() : base()
        {
        }

    }
}

