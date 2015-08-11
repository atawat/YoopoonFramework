namespace YooPoon.Core.Data
{
    public interface IUser
    {
        int Id { get; set; }
        string UserName { get; set; }

        string Password { get; set; }
    }
}