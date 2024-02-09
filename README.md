## Hi there 👋
Niloo Mont Here [About Me](https://www.linkedin.com/in/niloufar-mont/)

I'm a dotnet enthusiast, I'm looking for chances to boost my know-how and abilities.

# MicroWebFrameworkDI
A httpListenere that listens for requests and runs them through a pipleline with three pipes that routes urls semi-dynamically and handles exceptions and authentication.

I have also tried to implement a dependency injection service with three lifetimes: singleton, scoped, and transient which enabled using DI in controllers.

You can see number of instances created change in ```http://localhost:7776/Users/NotifyUser/{id}``` with each request.

**You can try these URLs like:**
```
http://localhost:7776/Products/GetAllProducts
http://localhost:7776/Orders/GetAllOrders
http://localhost:7776/Orders/GetOrderById/{id}
http://localhost:7776/Users/GetUserById/{id}
http://localhost:7776/Users/NotifyUser/{id}
```
