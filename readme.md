# Procédure

## Créer les projets
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

### Exercices
1. Créer le projet `HotelLandon.Demo-Csv` dans le répertoire de base, `C:\Synopsia`.
1. Enregistrer un `HotelLandon.Models.Customer` au format CSV.
1. Créer le proejt `HotelLandon.Demo-Json` dans le répertoire de base `C:\Synopsia`.
1. Enregistrer un `HotelLandon.Models.Customer` au format JSON, à l'aide de la librairie `Newtonsoft.Json`.

> Pour ajouter une librairie, il suffit d'utiliser la commande `dotnet-cli` suivante : `dotnet add package [NOM_PACKAGE]`.

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