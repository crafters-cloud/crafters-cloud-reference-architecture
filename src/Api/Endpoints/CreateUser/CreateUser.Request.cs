using FastEndpoints;
using FluentValidation;
using JetBrains.Annotations;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.CreateUser;

public static partial class CreateUser
{
    [UsedImplicitly]
    public class Request
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public int Id { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.FirstName).NotEmpty();
            RuleFor(r => r.LastName).NotEmpty();
            RuleFor(r => r.Age).GreaterThan(17);
        }
    }
}