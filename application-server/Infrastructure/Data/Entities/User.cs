namespace Entity {

    public class User {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string UserType { get; set; }
        public string CreatedAt {get; set; }
    }

}
