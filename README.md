# GildedRose Store

For the **architecture** I chose to build up a simple MVC web application using two views: the main and the login. Made up using HTML and Razor in the front-end, MVC pattern in the back-end and a simple static class to works as data layer. Two controller support all the operation: one for authorization (login, logout) purposes and the other exclusively to handle basic operations such as default webpage and Purchasing items.
I decided to use **ASP.NET Core v5** as it allows application to be multiplatform and it can take advantage of some of the new features.
The **authentication** is based on cookies as it is quite simple to implement and it does not require a data base.
**Unit testing** covering both controllers and a general **Integration Testing** projects were implemented respectively. 
