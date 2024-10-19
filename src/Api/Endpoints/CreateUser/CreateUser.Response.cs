namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.CreateUser;

public static partial class CreateUser
{
    public class Response
    {
        public string FullName { get; set; } = string.Empty;
        public bool IsOver18 { get; set; }
        public int RequestedId { get; set; }
    }
}