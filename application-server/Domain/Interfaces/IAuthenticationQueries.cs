public interface IAuthenticationQueries {

    bool RegisterCompany(Entity.Company user);
    bool RegisterStudent(Entity.Student user);
    Entity.Company FindCompanyFromUsername(string username);
    Entity.Student FindStudentFromUsername(string username);
    Entity.Company FindCompanyFromEmail(string email);
    Entity.Student FindStudentFromEmail(string email);
    User FindFromUsername(string username);
    User FindFromEmail(string email);

}

