using Logitar.Identity.Core.Models;
using Logitar.Identity.EntityFrameworkCore.SqlServer;

namespace Logitar.Identity.IntegrationTests;

internal record CurrentActorMock(Actor Actor) : ICurrentActor;
