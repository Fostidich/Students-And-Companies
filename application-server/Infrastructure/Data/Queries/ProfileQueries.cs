
public class ProfileQueries : IProfileQueries {

    private readonly IDataService dataService;

    public ProfileQueries(IDataService dataService) {
        this.dataService = dataService;
    }

    public Entity.Company FindCompanyFromId(int id) {return new Entity.Company {} ;}
    public Entity.Student FindStudentFromId(int id) {return new Entity.Student {} ;}
    public bool UpdateSaltAndPassword(UserType type, int id, string salt, string hash) {return false;}
    public bool UpdateUsername(UserType type, int id, string username) {return false;}
    public bool UpdateEmail(UserType type, int id, string email) {return false;}

}
