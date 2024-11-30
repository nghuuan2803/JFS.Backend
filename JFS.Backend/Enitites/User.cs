namespace JFS.Backend.Enitites
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User(string userName, string password, string role)
        {
            UserName = userName;
            Password = password;
            Role = role;
        }
    }

    public static class MockUserData
    {
        public static List<User> Users { get; set; } = new List<User>
        {
            new User("trung","123","admin"),
            new User("an","123","staff"),
        };
    }
}
