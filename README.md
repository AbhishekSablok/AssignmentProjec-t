# AssignmentProject
In this project we have 2 controllers

Auth controller-- which generate the JWT token and Register new user In AuthController as response to successful login, a JWT token will return in response. The JWT token should have following fields encoded: First Name Last Name Email IsActive Role
ToDO Controller -- Which perform all the CURD operation on Users Data TodoController will be protected via [Authorize] header and only authenticated users will have access to the controller. There will be two roles in the database for user with different set of access. User : These users will be able to get list of todo items, when accessing other APIs, they should get a HTTP 401: Unauthorized error Admin : Admin users will be able to access all the APIs
