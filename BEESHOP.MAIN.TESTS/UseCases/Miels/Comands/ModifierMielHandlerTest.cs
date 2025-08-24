using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Handlers;
using BEESHOP.MAIN.DOMAIN.Miels;
using BEESHOP.MAIN.TESTS.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace BEESHOP.MAIN.TESTS.UseCases.Miels.Comands;

public class ModifierMielHandlerTest : IAsyncLifetime
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
    private readonly Mock<IMielRepository> _mielRepositoryMock = new();
    private readonly Mock<ILogger<ModifierMielHandler>> _loggerMock = new();
    private readonly ModifierMielHandler _handler;

    public ModifierMielHandlerTest()
    {
        _handler = new ModifierMielHandler(
            _loggerMock.Object,
            _mielRepositoryMock.Object,
            _httpContextAccessorMock.Object
        );
    }

    [Fact(DisplayName = "ModifierMielHandler - Handle should modify an existing Miel")]
    public async Task Handle_ShouldModifyExistingMiel()
    {
        // Arrange
        var mielId = Guid.NewGuid();

        var command = new ModifierMielCommand
        {
            Id = mielId,
            Nom = "Miel de Lavande Modifié",
            Type = ETypeMiel.EteLiquide,
            Prix = 7,
            Description = "Un miel doux et parfumé, modifié.",
            Poids = 600,
            Image = null // nouvelle image simulée via HttpContext.Items
        };

        var existingMiel = MielsProvider.GetMielDummy(mielId);
        existingMiel.ImagePath = "old_image_path.jpg";

        _mielRepositoryMock
            .Setup(x => x.RecupererParId(mielId))
            .ReturnsAsync(existingMiel);

        var httpContext = new DefaultHttpContext();
        httpContext.Items["ImagePath"] = "new_image_path.jpg";
        _httpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(httpContext);

        var updatedMiel = new Miel
        {
            Id = mielId,
            Nom = command.Nom,
            TypeMiel = command.Type,
            Prix = command.Prix,
            Description = command.Description,
            Poids = command.Poids,
            ImagePath = "new_image_path.jpg"
        };

        _mielRepositoryMock
            .Setup(x => x.Modifier(It.Is<Miel>(m =>
                m.Id == mielId &&
                m.Nom == command.Nom &&
                m.TypeMiel == command.Type &&
                m.Prix == command.Prix &&
                m.Description == command.Description &&
                m.Poids == command.Poids &&
                m.ImagePath == "new_image_path.jpg"
            )))
            .ReturnsAsync(updatedMiel);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Nom, result.Nom);
        Assert.Equal(command.Type, result.TypeMiel);
        Assert.Equal(command.Prix, result.Prix);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(command.Poids, result.Poids);

        // Verifies explicites pour satisfaire VerifyNoOtherCalls()
        _mielRepositoryMock.Verify(x => x.RecupererParId(mielId), Times.Once);
        _mielRepositoryMock.Verify(x => x.Modifier(It.IsAny<Miel>()), Times.Once);
        _httpContextAccessorMock.Verify(x => x.HttpContext, Times.AtLeastOnce);
    }

    [Fact(DisplayName = "ModifierMielHandler - Handle should throw when Miel not found")]
    public async Task Handle_ShouldThrow_WhenMielNotFound()
    {
        // Arrange
        var missingId = Guid.NewGuid();
        var command = new ModifierMielCommand
        {
            Id = missingId,
            Nom = "x",
            Type = ETypeMiel.PrintempsCremeux,
            Prix = 5,
            Description = "x",
            Poids = 250
        };

        _mielRepositoryMock
            .Setup(x => x.RecupererParId(missingId))
            .ReturnsAsync((Miel?)null);

        // Act + Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        _mielRepositoryMock.Verify(x => x.RecupererParId(missingId), Times.Once);
        _mielRepositoryMock.Verify(x => x.Modifier(It.IsAny<Miel>()), Times.Never);
        _httpContextAccessorMock.Verify(x => x.HttpContext, Times.Never);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        // important: aucun appel non vérifié ne doit rester
        _httpContextAccessorMock.VerifyNoOtherCalls();
        _mielRepositoryMock.VerifyNoOtherCalls();
        return Task.CompletedTask;
    }
}