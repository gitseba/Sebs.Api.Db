# Sebs.Api.Db

This project has the purpose of connecting API's with different databases (MongoDb, Postgres...) by using docker-compose.
- It will be very useful in the future in some projects where I will want to connect by using containers and if errors will appear.

In order to use MongoCompass GUI, I will have to connect with: mongodb://localhost:27017
- because Compass GUI is running outside of docker environment.

USAGE:
- This project can be used by executing in cmd the command: docker-compose up -d
- Or directly in Visual Studio by using docker-compose file.


DEBUG: One error I faced in implementation was the overriding of the connection string inside the docker compose file.
In appSettings I had "ConnectionString": "mongodb://localhost:27017" and docker-compose file didn't override the connection string.
SOLUTION: https://stackoverflow.com/questions/56722291/docker-compose-override-not-overriding-connection-string

Using double underscores worked for me
ConnectionStrings__DefaultConnection=sql.data;database=WebRecipes-LocalDev;User Id=sa;Password=Pass@word

The ASP.NET Core official docs on environment variable configuration has the following explanation:
    When working with hierarchical keys in environment variables, a colon separator (:) 
	may not work on all platforms (for example, Bash). A double underscore (__) is supported by all platforms and is automatically replaced by a colon.

