namespace LibrariesAdministrator.Application.DTOs.Common
{
    public class ResponseBase
    {
        public bool Successful { get; set; }
        public string UserMessage { get; set; }
        public string InternalErrorMessage { get; set; }
        public int HttpCode { get; set; }
    }
}
