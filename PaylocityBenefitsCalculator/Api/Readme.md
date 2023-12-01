# Project organization
As an API scales to have dozens of controllers, a more "domain" driven approach to organization might make sense, keeping related things all next to each other.

For instance:
```
Dependents\
  Models\
  Repositories\
  DependentsController
```

This isn't always appropriate for a smaller scale API, where a more technical layer split may make more sense, as is the case here. While I thought about this and wanted to note it, I didn't make this change.

# Models and Proxies
For APIs that are consumed in a C# code base, it often makes sense to provide a "Proxy" or "Client" NuGet package, allowing for ease of sharing models, and a client wrapper specific for the API.

This would normally be done with a separate project that houses this information.

# Repository Pattern
I opted to go with a repository pattern because it's very similar to what you see with a lot of DB providers, some providers, like EF provide this for you with the DataContext. Others encourage you to write this yourself.

The plus side of the repository pattern is that it's dependency injectable, and as such can assist with unit testing. It also helps centralize business logic in a functional Data Access Layer.

The downside is that the repository pattern can require you to write a lot more boilerplace code. Another option could be inlining this behavior into the controller, reducing boilerplace at the cost of losing a clean separation of responsibilities- while unfortunate, this may still make sense for small APIs that are believed to stay small.