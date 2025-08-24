using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Handlers;
using BEESHOP.MAIN.DOMAIN.Miels;
using BEESHOP.MAIN.TESTS.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace BEESHOP.MAIN.TESTS.UseCases.Miels.Comands;

public class CreerMielHandlerTest : IAsyncLifetime
{

    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
    private readonly Mock<IMielRepository> _mielRepositoryMock = new();
    private readonly Mock<ILogger<CreerMielHandler>> _loggerMock = new();
    private readonly CreerMielHandler _handler;

    private static CreerMielCommand Command => new(
        Nom: "Miel de Lavande",
        Type: ETypeMiel.EteLiquide,
        Prix: 6,
        Description: "Un miel doux et parfumé.",
        Poids: 500,
        Image: GetFakeFormFile("miel_lavande.jpg", "image/jpeg")
    );

    public CreerMielHandlerTest()
    {
        _handler = new CreerMielHandler(
            _loggerMock.Object,
            _mielRepositoryMock.Object,
            _httpContextAccessorMock.Object
        );
    }

    [Fact(DisplayName = "CreerMielHandler - Handle should create a new Miel")]
    public async Task Handle_ShouldCreateNewMiel()
    {
        Miel mielDummy = MielsProvider.GetMielDummy(Guid.NewGuid());

        var httpContext = new DefaultHttpContext();

        httpContext.Items["ImagePath"] = mielDummy.ImagePath;

        _mielRepositoryMock.Setup(x => x.Creer(It.IsAny<Miel>()))
            .ReturnsAsync(mielDummy);

        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(httpContext);

        var handler = new CreerMielHandler(
            _loggerMock.Object,
            _mielRepositoryMock.Object,
            _httpContextAccessorMock.Object);

        var result = await handler.Handle(Command, CancellationToken.None);

        Assert.NotNull(result);
    }

    [Fact(DisplayName = "CreerMielHandler - Handle should throw exeption when no image provided")]
    public async Task Handle_ShouldThrowException_WhenNoImageProvided()
    {
        Miel mielDummy = MielsProvider.GetMielDummy(Guid.NewGuid());

        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext());

        var commandWithoutImage = new CreerMielCommand(
            Nom: mielDummy.Nom,
            Type: mielDummy.TypeMiel,
            Prix: ((int)mielDummy.Prix),
            Description: mielDummy.Description,
            Poids: mielDummy.Poids,
            Image: null
        );

        var handler = new CreerMielHandler(
            _loggerMock.Object,
            _mielRepositoryMock.Object,
            _httpContextAccessorMock.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(commandWithoutImage, CancellationToken.None));
    }

    private static IFormFile GetFakeFormFile(string fileName, string contentType)
    {
        var content = "Fake image content";
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);

        return new FormFile(stream, 0, bytes.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }
    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        _httpContextAccessorMock.VerifyAll();
        _mielRepositoryMock.VerifyAll();
        _httpContextAccessorMock.VerifyNoOtherCalls();
        _mielRepositoryMock.VerifyNoOtherCalls();
        return Task.CompletedTask;
    }
}