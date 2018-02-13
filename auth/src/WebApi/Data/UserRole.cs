namespace Chiron.Auth.Data {
    //Limitation of EFCore 1.1 - This class should not exist.
    public class UserRole {
        public int UserId { set; get; }
        public User User { set; get; }
        public int RoleId { set; get; }
        public Role Role { set; get; }
    }
}
