namespace FeedBackApp.Data.Api.UserApiRepo.Models
{
    public class UserListParamApiDataModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool IsIncludeRole { get; set; }
        public string ToQueryString()
        {
            QueryString result = new QueryString();
            result = result.Add(nameof(IsIncludeRole), IsIncludeRole.ToString());

            if (!string.IsNullOrWhiteSpace(UserName))
                result = result.Add(nameof(UserName), UserName.ToString());

            if (!string.IsNullOrWhiteSpace(Email))
                result = result.Add(nameof(Email), Email.ToString());

            return result.ToUriComponent();
        }
    }
}
