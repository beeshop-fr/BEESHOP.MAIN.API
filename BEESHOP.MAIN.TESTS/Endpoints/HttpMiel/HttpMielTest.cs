using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Text;


namespace BEESHOP.MAIN.TESTS.Endpoints.HttpMiel;

[Trait("Category", "Integration")]
public class HttpMielTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public HttpMielTest(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    //[Fact(DisplayName = "Créer un miel doit retourner 201")]
    //public async Task CreerMiel_ShouldReturn201()
    //{
    //    var form = new MultipartFormDataContent();

    //    form.Add(new StringContent("Miel de Lavande", Encoding.UTF8), nameof(CreerMielCommand.Nom));
    //    form.Add(new StringContent("EteLiquide", Encoding.UTF8), nameof(CreerMielCommand.Type));
    //    form.Add(new StringContent("15", Encoding.UTF8), nameof(CreerMielCommand.Prix));
    //    form.Add(new StringContent("Un miel", Encoding.UTF8), nameof(CreerMielCommand.Description));
    //    form.Add(new StringContent("500", Encoding.UTF8), nameof(CreerMielCommand.Poids));

    //    var bytes = new byte[] { 1, 2, 3, 4, 5 };
    //    var image = new ByteArrayContent(bytes);
    //    image.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

    //    form.Add(image, nameof(CreerMielCommand.Image), "photo.jpg");

    //    // Act
    //    var resp = await _httpClient.PostAsync("/api/main/Miel", form);

    //    // Assert
    //    Assert.Equal(HttpStatusCode.Created, resp.StatusCode);


    //    Assert.NotNull(resp);
    //    Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
    //}
}