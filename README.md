# TodoApi
## Feature
#### API authentication and authorization, authentication registration and user management, role based authorization.
#### Authenticate calls to the API to registered users only
#### Admin user can calls to the API Users and Todos Ex: /api/Users
#### User user can only calls to the API Todos Ex: /api/TodoItems
#### POST /api/Users/authenticate, /api/Users/register allow Anonymous user calls to authenticate and register
## Technology
#### ASP .Net 5
#### Entity Framework Core, Migratons
#### SQLite Database
#### JWT 
#### Swashbuckle to serve the Swagger UI

## Demo:
#### Link https://todoapi.sofsog.com/swagger/index.html
##### Admin for test Feature: admin pass: admin.
##### User for test: user pass: string
##### How to test:
POST https://todoapi.sofsog.com/api/Users/authenticate. 
Use the above user to authenticate, API app will send Token code. Use the above user to authenticate, API app will send Token code. Use Token codes to call other functions. Note: Use Token by clicking the Authorize button with the lock icon. Or you can use PostMan to test
