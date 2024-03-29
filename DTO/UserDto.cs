namespace apiAspNet.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string ?Email { get; set; }
        public string ?Address { get; set; }
        public string ?Telephone { get; set; }
        public string ?SecondTelephone { get; set; }
    }
}
