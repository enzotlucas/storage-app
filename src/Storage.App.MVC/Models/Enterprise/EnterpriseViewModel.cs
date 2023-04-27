namespace Storage.App.MVC.Models.Enterprise
{
    public class EnterpriseViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool Exists()
        {
            return Id != Guid.Empty;
        }
    }
}
