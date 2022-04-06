namespace X509Observer.MagicStrings.DatabaseQueries
{
    public static class ClientDesktopsRepositoryQueries
    {
        public static readonly string GET_CLIENT_DESKTOPS = "SELECT * FROM [ClientDesktops];",
                                      GET_CLIENT_DESKTOP_BY_NAME = "SELECT * FROM [ClientDesktops] WHERE Name=@Name;",
                                      GET_CLIENT_DESKTOP_BY_IP = "SELECT * FROM [ClientDesktops] WHERE IP=@IP;",
                                      GET_CLIENT_DESKTOP_BY_ID = "",
                                      ADD_CLIENT_DESKTOP = "INSERT INTO [ClientDesktops] (IP, Name, Comment, LastResponseTime) VALUES (@IP, @Name, @Comment, @LastResponseTime);",
                                      REMOVE_CLIENT_DESKTOP = "DELETE FROM [ClientDesktops] WHERE IP=@IP AND Name=@Name;",
                                      REMOVE_CLIENT_DESKTOP_BY_NAME = "DELETE FROM [ClientDesktops] WHERE Name=@Name;",
                                      REMOVE_CLIENT_DESKTOP_BY_IP = "DELETE FROM [ClientDesktops] WHERE IP=@IP;";
    }
}
