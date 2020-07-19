# Advanced Multi-Purpose Library (AMPL)

A collection of classes intended to speed-up console, web, and mobile development.


## Updates

> 2020-07-19: Moved from private repo to GitHub.


## About AMPL

AMPL's main goal is to provide a set of utility classes that help to write well-structured, concise an robust code.


## Assemblies


### Ampl.Core


### Ampl.Annotations

Contains utility methods for extracting the the applied `DisplayAttribute` property values.


### Ampl.Configuration

Provides interfaces and classes for reading/writing configuration objects.


### Ampl.Identity

Provides a set of abstractions to determine access with a Claims authorization pattern.


### Ampl.Identity.Claims

Provides an ability to explicitly check user acces to the specified object (e.g. action and resource).


### Ampl.Web.Http

Contains a set of useful `Attribute`s for ASP.NET WebApi model binding.

### Ampl.Web.Http.Authorization

Contains `Attribute`s used in ASP.NET WebApi authorization.

### Ampl.Web.Mvc

Provides a lot of useful classes helping to write more brief and clear code, especially code for Razor pages/views.

One of those are:
    
  * Bootstrap Alert Helpers;
  * `DateTime` validation;
  * Delayed render, primarily used to **include JS in partial views**;
  * The **DropDown** helper with custom data sources;
  * `ShortNameAttribute` utilizes `ShortName` property of the `DisplayAttribute`
  * ... and so on.


### Ampl.Web.Mvc.Authorization

Contains implementation of the `ClaimsAuthorizationManager` for the ASP.NET allowing to explicitly check user's rights.


### Ampl.Web.Mvc.SecondaryLogon

The Secondary Logon implementation allowing one user (usually admin) to log in as another user without specifying her credentials.


### Ampl.Web.Optimization

Provides a set of helper classes for `System.Web.Optimization`.


### Ampl.Web.Mvc.EditorTemplates

A collection of editor templates (`Views/Shared/EditorTemplates`) styled with Bootstrap 3.