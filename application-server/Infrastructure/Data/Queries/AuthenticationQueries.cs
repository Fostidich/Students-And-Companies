public class AuthenticationQueries : IAuthenticationQueries {

    private readonly IDataService dataService;

    public AuthenticationQueries(IDataService dataService) {
        this.dataService = dataService;
    }

    public bool RegisterCompany(Entity.Company user) {return false;}
    public bool RegisterStudent(Entity.Student user) {return false;}
    public Entity.Company FindCompanyFromUsername(string username) {return new Entity.Company {} ;}
    public Entity.Student FindStudentFromUsername(string username) {return new Entity.Student {} ;}
    public Entity.Company FindCompanyFromEmail(string email) {return new Entity.Company {} ;}
    public Entity.Student FindStudentFromEmail(string email) {return new Entity.Student {} ;}

}
