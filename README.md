# Queo.Commons.Web

[![Build Status](https://dev.azure.com/queo-commons/Commons-OpenSource/_apis/build/status%2FqueoGmbH.csharp-commons.web?branchName=main)](https://dev.azure.com/queo-commons/Commons-OpenSource/_build/latest?definitionId=2&branchName=main) [![Build Status](https://dev.azure.com/queo-commons/Commons-OpenSource/_apis/build/status%2FqueoGmbH.csharp-commons.web?branchName=develop)](https://dev.azure.com/queo-commons/Commons-OpenSource/_build/latest?definitionId=2&branchName=develop)

## Description
Queo.Commons.Web can be used working with an web app or web api.


## Example
-

### Steps:
-

## How to use ModelBinding
- include Nuget-Package (queo.commons.web.modelBinding)

```csharp
<PackageReference Include="Queo.Commons.Web.ModelBinding" Version="1.0.1" />
```

- If you want to use the ModelBinding, you have to call the method "AddJsonModelConverter(this IServiceCollection services)" from the JsonModelBindingConfiguration class 
and AddEntityBinder(this IServiceCollection services, int insertPosition) from the ModelBindingConfiguration class inside your program.cs or startup.cs.

## How to use ExceptionHandling
- include Nuget-Package (queo.commons.web.exceptionHandling)


```csharp
<PackageReference Include="Queo.Commons.Web.ExceptionHandling" Version="1.0.1" />
```

- add the following code to your Programm.cs or Startup.cs

- If you want to use the ExceptionHandling, you have to call the method "UseExtendedProblemDetails (this IServiceCollection services)" from the ProblemDetailsExtensions class inside your program.cs or startup.cs.