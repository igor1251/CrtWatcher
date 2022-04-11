﻿namespace X509Observer.MagicStrings.DatabaseQueries
{
    public static class ApiUsersRepositoryQueries
    {
        public static readonly string ADD_API_USER = "INSERT INTO [ApiUsers] (UserName, PasswordHash) VALUES (@UserName, @PasswordHash);",
                                      UPDATE_API_USER = "UPDATE [ApiUsers] SET UserName=@UserName, PasswordHash=@PasswordHash WHERE ID=@ID;",
                                      REMOVE_API_USER = "DELETE FROM [ApiUsers] WHERE ID=@ID;",
                                      GET_API_USER_BY_ID = "SELECT * FROM [ApiUsers] WHERE ID=@ID;",
                                      GET_API_USERS = "SELECT * FROM [ApiUsers];";
    }
}
