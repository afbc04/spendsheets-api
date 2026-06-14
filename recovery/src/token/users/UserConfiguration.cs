public static class UserConfiguration
{
    private static User? _user = null;

    public static async Task<User?> GetUser()
    {
        return _user;
    }

    public static async void Update()
    {
        _user = await UserRepository.Get();
    }
}