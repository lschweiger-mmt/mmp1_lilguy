
/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using Npgsql;


public static class PostgresAccess
{
    public static async Task Insert(string command, string connString)
    {
        await using var conn = new NpgsqlConnection(connString);
        try
        {
            await conn.OpenAsync();

            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {
            if (ex is System.Net.Sockets.SocketException) return; // When there is no internet, there shouldn't be an error
            throw;
        }
    }

    public static async Task<List<LeaderboardEntry>> SelectLeaderboard(string command)
    {

        await using var conn = new NpgsqlConnection(Config.connString_Leaderboard);

        List<LeaderboardEntry> ret = new();
        try
        {
            await conn.OpenAsync();

            await using (var cmd = new NpgsqlCommand(command, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    ret.Add(new LeaderboardEntry(reader.GetString(0), reader.GetInt32(1), reader.GetDateTime(2), reader.GetString(3)));
                }
            }
        }
        catch (Exception ex)
        {
            if (ex is System.Net.Sockets.SocketException) return ret; // When there is no internet, there shouldn't be an error
            throw;
        }

        return ret;
    }
    public static async Task<string> SelectNewestVersion()
    {
        string ret = "";
        await using var conn = new NpgsqlConnection(Config.connString_Leaderboard);
        try
        {
            await conn.OpenAsync();
            string command = "SELECT ver FROM leaderboard GROUP BY ver ORDER BY ver DESC LIMIT 1;";

            await using (var cmd = new NpgsqlCommand(command, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    ret = reader.GetString(0);
                }
            }
        }
        catch (Exception ex)
        {
            if (ex is System.Net.Sockets.SocketException) return ret; // When there is no internet, there shouldn't be an error
            throw;
        }

        return ret;
    }
}