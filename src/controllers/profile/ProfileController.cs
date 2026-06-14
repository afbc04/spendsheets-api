public static class ProfileController
{
    public static async Task<SendingPacket> ListProfiles(bool isHidden)
    {
        var sessions = SessionManager.ListSessions();
        var list = sessions.Select(s => ProfileView.ToView(s, isHidden));
        return SendingPacket.Success(200, list);
    }

    public static async Task<SendingPacket> GetProfile(string username, bool isHidden)
    {
        var session = SessionManager.GetProfile(username);
        return session is not null
            ? SendingPacket.Success(200,ProfileView.ToView(session, isHidden))
            : SendingPacket.Error(404, "Profile does not exists");
    }
    
    public static async Task<SendingPacket> CreateProfile(Session? session, Dictionary<string, object?> profileData)
    {
        try
        {
            string username = (string)profileData["username"]!;
            if (SessionManager.GetProfile(username) != null)
                return SendingPacket.Error(400, $"Profile already exists");

            var profile = new Profile();

            profile.Username = (string)profileData["username"]!;
            profile.SetPassword((string)profileData["password"]!);

            if (profileData.TryGetValue("active", out var isActive))
                profile.IsActive = (bool)isActive!;

            if (profileData.TryGetValue("name", out var name))
                profile.Name = (string)name!;
            else
                profile.Name = profile.Username;

            if (SessionManager.CountProfiles() == 0)
            {
                profile.IsAdmin = true;
            }
            else
            {
                if (session is null || !session.Profile.IsAdmin)
                    return SendingPacket.Error(403, "Only administrators can create profiles");
            
                profile.IsAdmin = false;
            }

            profile.IsAdmin = SessionManager.CountProfiles() == 0;

            bool wasProfileCreated = await ProfileRepository.Put(profile);

            if (!wasProfileCreated)
                return SendingPacket.Error(422, "Error while creating profile into database");

            var sessionOfProfile = SessionManager.PutProfile(profile);
            return SendingPacket.Success(201, ProfileView.ToView(sessionOfProfile!, false));
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.statusCode, ex.message);
        }
    }

    public static async Task<SendingPacket> UpdateProfile(string username, Dictionary<string, object?> profileData)
    {
        try
        {
            var session = SessionManager.GetProfile(username);
            if (session is null)
                return SendingPacket.Error(404, $"Profile doesn't exists");

            var profile = session.Profile;

            if (profileData.TryGetValue("password", out var password))
                profile.SetPassword((string)password!);

            if (profileData.TryGetValue("active", out var isActive))
                profile.IsActive = (bool)isActive!;

            if (profileData.TryGetValue("name", out var name))
                profile.Name = (string)name!;

            bool wasProfileUpdated = await ProfileRepository.Put(profile);

            if (!wasProfileUpdated)
                return SendingPacket.Error(422, "Error while updating profile of database");

            var newSession = SessionManager.PutProfile(profile);
            return SendingPacket.Success(200, ProfileView.ToView(newSession!, false));
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.statusCode, ex.message);
        }
    }

    public static async Task<SendingPacket> DeleteProfile(string username)
    {
        var session = SessionManager.GetProfile(username);
        if (session is null)
            return SendingPacket.Error(404, "Profile does not exists");

        if (session.Profile.IsAdmin)
            return SendingPacket.Error(403, "Administrator profiles can not be deleted");

        bool wasProfileDeleted = await ProfileRepository.Delete(username);
        if (!wasProfileDeleted)
            return SendingPacket.Error(422, "Error while deleting profile from database");

        var deletedSession = SessionManager.DeleteProfile(username);
        return SendingPacket.Success(200, ProfileView.ToView(deletedSession!, false));
    }
}