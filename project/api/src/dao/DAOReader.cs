using Npgsql;

public static class DAOReader {

    public static int getInt(this NpgsqlDataReader r, int index)
        => r.GetInt32(index);

    public static int? tryGetInt(this NpgsqlDataReader r, int index)
        => r.IsDBNull(index) ? null : r.GetInt32(index);

    public static long getLong(this NpgsqlDataReader r, int index)
        => r.GetInt64(index);

    public static long? tryGetLong(this NpgsqlDataReader r, int index)
        => r.IsDBNull(index) ? null : r.GetInt64(index);

    public static string getString(this NpgsqlDataReader r, int index)
        => r.GetString(index);

    public static string? tryGetString(this NpgsqlDataReader r, int index) {
        return r.IsDBNull(index) ? null : r.GetString(index);
    }

    public static bool getBool(this NpgsqlDataReader r, int index)
        => r.GetBoolean(index);

    public static byte[] getBytes(this NpgsqlDataReader r, int index)
        => r.GetFieldValue<byte[]>(index);

    public static DateTime getDateTime(this NpgsqlDataReader r, int index)
        => r.GetDateTime(index);

}