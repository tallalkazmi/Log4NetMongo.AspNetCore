# Log4NetMongo.Core
Includes all Log4Net methods with `MongoDbAppender` configuration for creating application logs for multiple applications and environments with `user_id` from `ClaimsIdentity`. Supports multi-tenancy by logging `tenant` information from `ClaimsIdentity`.

Updated log4mongo-net classes to work with .Net Core 2.2 (<https://github.com/log4mongo/log4mongo-net>)

## Installation
1. Add Log4NetMongo.Core package.
2. Set __MongoLogConnection__ in `ConnectionStrings` and __MongoLogCollectionName__ in `ApplicationSettings`.
3. Add __ApplicationName__ and __Environment__ values in `ApplicationSettings`, if you choose to you can initialize the class as shown in the first example.

## Configuration

##### appsettings.json

```
{
  "ConnectionStrings": {
    "MongoLogConnection": "mongodb://localhost"
  },
  "ApplicationSettings": {
    "ApplicationName": "Log4netMongo.Core",
    "Environment": "Test",
    "MongoLogCollectionName": "application.log"
  }
}
```

## Usage

1. Initialize class with __ApplicationName__ and __Environment__
```csharp
using Log4NetMongo;

AppLogger logger = new AppLogger("Test Application", "Dev");
logger.LogInfo("Test Info");
```
2. Initialize class with __ApplicationName__ and __Environment__ in `AppSettings`
```csharp
using Log4NetMongo;

//Configuration from appsettings.json
AppLogger logger = new AppLogger();
logger.LogInfo("Test Info");
```

### Log Model
1. Timestamp (System Time)
2. Level
3. Thread
4. Application
5. Environment
6. User (Application's user from `ClaimsPrincipal.Current.Identity.Name`)
7. Tenant (User's tenant from `ClaimsPrincipal.Current.FindFirst("tenant")`)
8. Message
9. Data (JSON string of object passed in Data parameter)
10. Exception
11. Method Name (Calling method's name)
12. File Name (Calling log's file name)
13. Line Number (Calling log's line number)