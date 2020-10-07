# Créer les projets
Créer le projet `HotelLandon.Models` dans le répértoire `C:\Synopsia`.

> Pour créer un projet, il existe la commande `dotnet new [TYPE_PROJET] --name [REPERTOIRE_PROJET]`. _L'option `name` n'est pas indispensable et permet uniquement de créer le projet dans un autre répértoire._

Créer la classe `Customer` avec deux propriétés de type `string` nommées `FirstName` et `LastName`.

```CSharp
public class Customer 
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

&Eacute;crire un constructeur avec deux paramètres pour renseigner les deux propriétés.

```CSharp
public Customer(string firstName, string lastName)
{
    FirstName = firstName;
    LastName = lastName;
}
```

## Exercices
1. Créer le projet `HotelLandon.Demo-Csv` dans le répertoire de base, `C:\Synopsia`.
1. Enregistrer un `HotelLandon.Models.Customer` au format CSV.
1. Créer le proejt `HotelLandon.Demo-Json` dans le répertoire de base `C:\Synopsia`.
1. Enregistrer un `HotelLandon.Models.Customer` au format JSON, à l'aide de la librairie `Newtonsoft.Json`.

> Pour ajouter une librairie, il suffit d'utiliser la commande `dotnet-cli` suivante : `dotnet add package [NOM_PACKAGE]`.

# Entity Framework Core

## Créer la base de données

On va avoir besoin de plusieurs packages NuGet dans un nouveau projet de type `classlib` qui sera nommé `HotelLandon.Data` :

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Design`

> EF Core n'est pas compatible `netstandard2.0`. Utiliser `netcoreapp3.1`.

Ajouter la classe `HotelLandonContext` qui hérite de `Microsoft.EntityFrameworkCore.DbContext`.

Ajouter une propriété de type `Microsoft.EntityFrameworkCore.DbSet<T>`, où `T` sera de type `HotelLandon.Models.Customer`.

```CSharp
using HotelLandon.Models;
using Microsoft.EntityFrameworkCore;

// ...

public DbSet<Customer> Customers { get; set; }
```

> Par défaut, le nom de la propriété sera le nom de la table.

Surcharger la méthode `protected OnConfiguring(DbContextOptionsBuilder optionsBuilder)` afin de configurer le serveur de données :

```CSharp
 protected override void OnConfiguring(DbContextOptionsBuilder builder)
 {
     builder.UseSqlServer("[CONNECTION_STRING]");
 }
```

Installer ou mettre à jour le tool `dotnet-ef` à votre installation du SDK dotnet. 

> Utiliser la commande `dotnet tool add [TOOL_NAME] --global` pour installer le tool de manière globale.

Ajouter une migration permettant de comparer la version existante de la base de données à celle voulue. 

> Pour cela, il sera nécessaire d'utiliser la commande `dotnet ef migrations add [MIGRATION_NAME]`
> Il est possible d'avoir une ou plusieurs migrations.

Pour appliquer les migrations, il suffit d'utiliser la commande `dotnet ef database update`. 
> Pour voir la requête SQL génrée, il est possible d'utiliser l'option `--verbose`.

La base de données est créée.

## Accéder aux données
Pour lire des données depuis la base de données, avec le contexte :
```CSharp
_context.Customers.ToList();
``` 

À l'aide de l'object `IQueryable<T>` et de `System.Linq`, on va pouvoir préciser notre requête. Elle sera exécutée lors de l'appel de la méthode `ToList()`.
```CSharp
// A ne pas faire 
// la requête SQL est exécutée ici
IEnumerable<Customer> query = _context.Customers.ToList();

// filtrer les résultats déjà obtenus
IEnumerable<Customer> query2 = query.Where(c => c.FirstName.Contains("A"));

// renvoyer
return query2.ToList();


// La bonne réponse : 
IQueryable<Customer> query = _context.Customers;
query = query.Where(c => c.FirstName.Contains("A"));
return query.ToList();
``` 

## Créer, modifier et supprimer des données
```CSharp
_context.Customers.Add(customer);
_context.SaveChanges();
```

```CSharp
_context.Customers.Update(customer);
_context.SaveChanges();
```

```CSharp
_context.Customers.Remove(customer);
_context.SaveChanges();
```

# Créer une solution ...et passer à Visual Studio !
Pour créer une solution avec `dotnet-cli`, il suffit d'utiliser la commande `dotnet new sln` et pour ajouter les projets : `dotnet sln add [RELATIVE_PATH]`

Dans `C:\Synopsia`, taper les commandes suivantes :
- `dotnet new sln`
- `dotnet sln add HotelLandon.Data`
- `dotnet sln add HotelLandon.Models`

# ASP.NET Core

## Injection des dépendances
Pour accéder aux informations de la base de données `HotelLandon`, il est nécessaire d'injecter `HotelLandonContext`. _Voir l'injection de dépendences_.

Dans la classe `Startup`, ajouter le service d'accès aux données :
```CSharp
public void ConfigureServices(IServiceCollection services)
{
    //...
    services.AddDbContext<HotelLandonContext>();
    //...
}
```

Puis dans les contrôleurs, l'injecter à travers le contructeur des contrôleurs :
```CSharp
public class CustomersController : ControlerBase
{
    private readonly HotelLandonContext _context;

    public CustomersController(HotelLandonContext context)
    {
        _context = context;
    }
}
```

...ou des Pages Razor :
```
@inject HotelLandonContext context;
```

...ou du contructeur du code behind de la page Razor :
```CSharp
private readonly HotelLandonContext _context;
public MyPage(HotelLandonContext context) 
{
    _context = context;
}
```

## Configuration

Pour personnaliser la configuration d'une application ASP.NET Core, on peut utiliser l'interface `IConfiguration` et l'injecter, via le constructeur de de la classe `Startup`.

Dans l'ordre, la configuration est lue de manière suivante :

- `X:\Users\%User%\AppData\Roaming\Microsoft\UserSecrets\Projet\secrets.json` ;
- Variables d'environnement ;
- `%Projet%\appsettings.json` (le nom du fichier peut personnalisé dans la configuration de la classe `Startup`).

Pour la configuration suivante : 
```JSON
{
    "Section_01": {
        "Property_01" : "Value_01",
        "Property_02" : "Value_02",
        "Property_03" : 3
    },
    "Section_02": {
        "Property_01" : "Value_01",
        "Property_02" : "Value_02",
        "Property_03" : 3
    }
}
```
Pour accéder à la configuration : 
```CSharp
// classe Startup, méthode ConfigureServices(IServiceCollection)
services.Configure<Section>(Configuration.GetSection("Section_01"));

// classe Section
public class Section
{
    public string Property_01 { get; set; }
    public string Property_02 { get; set; }
    public int Property_03 { get; set; }
}
```

Dans une page Razor :
```
@inject Microsoft.Extensions.Options.IOption<Section> sectionOptions

<h1>@sectionOptions.Value.Property_03</h1>
```

Dans un contrôleur MVC :

```CSharp
public class MyController : Controller
{
    private readonly Section _section;
    public MyController(IOptions<Section> sectionOptions)
    {
        _section = sectionOptions.Value;
    }
}
```

## Utiliser un Middleware (Swagger)
Swagger utilise le standard OpenAPI pour générer le JSON qui sera lu dans l'interface SwaggerUI.

Installer le package `Swashbuckle.AspNetCore`

Ajouter SwaggerGen dans la collection de services ASP.NET Core
```CSharp
services.AddSwaggerGen(o => o.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" }));
```

Configurer Swagger et SwaggerUI :
```CSharp
app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));
```

## Créer son propre `Middleware`

1. Dans le projet `HotelLandon.Api`, ajouter un dossier nommé `Middlewares`.
1. Aller dans le menu `Ajouter...`, puis `Nouvel élément...`
1. Chercher "Interlogiciel" ou "Middleware"

Deux classes se créent : 
```CSharp
public class MyMiddleware
{
    private readonly RequestDelegate _next;

    public MyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {

        return _next(httpContext);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class MyMiddlewareExtensions
{
    public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyMiddleware>();
    }
}
```
La classe `MyMiddleware` contient un constructeur avec un `RequestDelegate` qui va permettre l'appel au suivant middleware. Il est possible de rajouter d'autres services (par exemple `ILogger`).

`MyMiddleware.Invoke(HttpContext)` défini le travail à faire pendant la configuration de l'application.

La méthode d'extension `UseMyMiddleware()` ajoute à `IApplicationBuilder` la méthode permettant de référencer `MyMiddleware` dans la pipeline ASP.NET Core.

## WebApi
Les API d'ASP.NET Core utilisent le pattern MVC. Chaque contrôleur doit hériter au moins de la classe `ControllerBase`.

Les méthodes doivent être décorées avec un attribut permettant d'indiquer la méthode HTTP : `HttpGetAttribute`, `HttpPostAttribute`, `HttpPutAttribute`, `HttpDeleteAttribute`, etc.
```CSharp
[HttpGet]
public IActionResult Get() 
{ 
    return Ok(); 
}
```

Il est possible d'écrire ses méthodes de 2 manières : 

Laisser ASP.NET Core la gestion des erreurs, des modèles, etc.
En utilisant les type des retour de chaque élément 
```CSharp 
[HttpGet]
public IEnumerable<Customer> GetAll() 
{ 
    /*...*/ 
}
```

Personnaliser le comportement, la gestion des erreurs, etc. 
```CSharp
[HttpGet]
public ActionResult<Customer> Get(int id)
{
    if (id < 0)
    {
        return BadRequest("L'Id doit être entier")
    }
    Customer customer = _context.Customers.Find(id);
    if (customer == null)
    {
        return NotFound("Le client n'existe pas");
    }
    return Ok(customer);
}

[HttpPost]
public ActionResult<Customer> Post(Customer c) 
{
    if (ModelState.IsValid) // vérification par rapport au modèle
    {
        if (c.HasErrors)
        {
            return BadRequest("Le client contient des erreurs.");
        }
        /*...*/
        return Created("Get/" + c.Id, c.Id);
    }
    else 
    {
        return BadRequest("Le modèle n'est pas bon.");
    }
}
```

### Exercices

1. Créer un projet ASP.NET Core WebAPI nommé `HotelLandon.Api`.
1. Référencer les projets `HotelLandon.Data` et `HotelLandon.Models`
1. Créer un contrôleur nommé `CustomersController`
1. Créer 4 méthodes: 
    1. `Get` pour obtenir la liste des clients depuis la base de données
    1. `Post` pour ajouter un client dans la base de données 
    1. `Put` pour modifier un client existant
    1. `Delete` pour supprimer un client à partir de son id.

## UI

### RazorPages

Créer un projet `HotelLandon.WebPages`. La classe Startup est légèrement différente. Elle enregistre dans le `IServiceCollection` le module `RazorPages` avec la méthode d'extension `AddRazorPages()`.

Cet UI va nous permettre d'afficher et de créer des clients. Il existe deux manières de procéder : 

1. Chercher les informations via une API.
1. Se connecter en direct.

Pour la première version, il est nécessaire d'enregistrer un `System.Net.HttpClient` dans le `IServiceCollection` d'ASP.NET Core.
```CSharp
services.AddHttpClient("ApiClient", h => h.BaseAddress = new Uri(Configuration["ApiUrl"]));
```

Il est possible d'injecter le client http dans chaque page razor, via le constructeur du code behind de la page Razor :

`Index.cshtml`
```
@page // toutes les pages doivent avoir cette ligne
@model IndexModel // pas obligatoire
@{
    // bloc de code C# (razor)
}

<table>
    <tr>
        <th>Prénom</th>
        <th>Nom de famille<th>
    </tr>

    @foreach (Customer customer in Model.Customers)
    {
        <tr>
            <td>
                @customer.FirstName
            </td>
            <td>
                @customer.LastName
            </td>
        </tr>
    }
</table>
```

`Index.cshtml.cs`
```
public class IndexModel 
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IEnumerable<Customer> Customers { get; set; }

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        HttpClient httpClient = _httpClientFactory.CreateClient("ApiClient");
        // HTTP Get BaseAddress + Customers
        string json = await httpClient.GetStringAsync("Customers");
        // Parser (déserialiser)
        Customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(json);
    }
}
```

### MVC

Pour gérer les vues, les contrôleurs doivent hériter de la classe `Controller`.

Les vues sont dans le répértoire `Views`. Chaque contrôleur doit avoir un dossier du même nom.
- Project
    - Controllers
        - **Home**Controller.cs
            - `IActionResult Index()`
        - **Customers**Controller.cs
            - `IActionResult Index()`
            - `IActionResult Details(int id)`
    - Views
        - **Home**
            - *Index*.cshtml
        - **Customers**
            - *Index*.cshtml
            - *Details*.cshtml
    - Models
        - Customers
        



Les actions sont par défaut des méthodes HTTP GET et doivent retourner une `ViewResult` (ou tout autre objet héritant de `IActionResult`).

La méthode `View` peut contenir le nom de la vue et un modèle.

```CSharp
public IActionResult Index()
{
    // do something here to get a model
    return View(model);
}
```

> Andrés Talavera 
> andres.talavera@ideastud.io
> +33 6 25 00 19 31