namespace ManejoPresupuesto.Models
{
    public class Usuario
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string EmailNormalizado { get; set; }
        public int Id { get; set; }
    }
}
