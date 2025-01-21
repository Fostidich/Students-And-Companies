using Xunit;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class ProfileTest : IClassFixture<TestServerFixture> {

    private readonly HttpClient client;

    public ProfileTest(TestServerFixture fixture) {
        client = fixture.Client;
    }

}
