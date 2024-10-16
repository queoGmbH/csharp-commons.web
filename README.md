# Queo.Commons.Web

[![Build Status](https://dev.azure.com/queo-commons/Commons-OpenSource/_apis/build/status%2FqueoGmbH.csharp-commons.web?branchName=main)](https://dev.azure.com/queo-commons/Commons-OpenSource/_build/latest?definitionId=2&branchName=main) [![Build Status](https://dev.azure.com/queo-commons/Commons-OpenSource/_apis/build/status%2FqueoGmbH.csharp-commons.web?branchName=develop)](https://dev.azure.com/queo-commons/Commons-OpenSource/_build/latest?definitionId=2&branchName=develop)

## Description
Queo.Commons.Web can be used working with an web app or web api.

## How to use it
### How to use ModelBinding
- include Nuget-Package (queo.commons.web.modelBinding)

```csharp
<PackageReference Include="Queo.Commons.Web.ModelBinding" Version="1.0.1" />
```

- If you want to use the ModelBinding, you have to add the following to your program.cs or startup.cs.

```csharp
builder.Services.AddJsonModelConverter();
builder.Services.AddEntityBinder(0);
```

### How to use ExceptionHandling
- include Nuget-Package (queo.commons.web.exceptionHandling)

```csharp
<PackageReference Include="Queo.Commons.Web.ExceptionHandling" Version="1.0.1" />
```

- Add the following code to your Programm.cs or Startup.cs

```csharp
builder.Services.UseExtendedProblemDetails();
```

### How to use Security

- include Nuget-Package (queo.commons.web.security)

```csharp
    <PackageReference Include="Queo.Commons.Web.Security" Version="1.0.2" />
```

- Implement Classes for the the following interfaces:
	- IPermissionCalculator (here RoleToPermissionCalculator is used as an example)
	- ISecurityContext (here SecurityContext is used as an example)
	- ISecurityContextFactory (here SecurityContextFactory is used as an example)

- Add the following code to your Programm.cs or Startup.cs

```csharp
builder.Services.AddMethodAuthorize<AuthorizeService>();
builder.Services.AddActionDecorator();
builder.Services.AddSecurityContextServices<SecurityContext, SecurityContextFactory>();
builder.Services.AddScoped<IPermissionCalculator, RoleToPermissionCalculator>();
```