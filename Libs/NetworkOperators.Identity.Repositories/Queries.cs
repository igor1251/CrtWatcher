namespace NetworkOperators.Identity.Repositories
{
    public static class Queries
    {
        public static readonly string ADD_USER = "INSERT INTO [Users] (UserName, PasswordHash, Permissions) VALUES (@UserName, @PasswordHash, @Permissions);",
                                      UPDATE_USER = "UPDATE [Users] SET PasswordHash=@PasswordHash, Permissions=@Permissions;",
                                      REMOVE_USER = "DELETE FROM [Users] WHERE ID=@ID;",
                                      GET_USER_BY_ID = "SELECT * FROM [Users] WHERE ID=@ID;",
                                      GET_USER_BY_AUTHENTICATION_DATA = "SELECT * FROM [Users] WHERE UserName=@UserName AND PasswordHash=@PasswordHash;",
                                      GET_USER_BY_USERNAME = "SELECT* FROM[Users] WHERE UserName = @UserName;",
                                      GET_USER_ID = "SELECT [Users].ID FROM [Users] WHERE UserName=@UserName;",
                                      GET_USERS = "SELECT * FROM [Users];",
                                      IS_USER_EXISTS = "SELECT EXISTS(SELECT * FROM [Users] WHERE UserName=@UserName);",
                                      
            
                                      CREATE_TABLES = "CREATE TABLE Users(" +
                                                      "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                                      "UserName VARCHAR(120) NOT NULL, " +
                                                      "PasswordHash VARCHAR(256) NOT NULL," +
                                                      "Permissions INTEGER(2) NOT NULL)";
    }
}
