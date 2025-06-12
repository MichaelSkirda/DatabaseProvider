# DatabaseProvider

Easy way to inject DB into project and extra mapping methods.

## Installation

NuGet - https://www.nuget.org/packages/DatabaseProvider/

## Usage

How to inject dependency.

```csharp
using DatabaseProvider;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbProvider>(services =>
{
    string connectionString = "Host=localhost;Port=5432;Database=test_db;Username=postgres;Password=password123";
    return new DbProvider(() => new NpgsqlConnection(connectionString));
});
```

```csharp
public class CustomerController : Controller
{
    private IDbProvider Db { get; set; }

    public CustomerController(IDbProvider db)
    {
        Db = db;
    }
    
    ...
}
```

All examples contains these two models:

```csharp
public class Customer
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public List<Product> PurchasedProducts { get; set; } = [];
}
```
```csharp
public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Price { get; set; }
}
```

## Extra mapping methods:

### OneToManyAsync<TOne, TMany>
Return collection of TOne entities. Calls callback for each TOne and TMany pair. For example list of customers with their purchases:


```csharp
public async Task<IEnumerable<Customer>> ReadCustomers()
{
string sql = @"
SELECT
    c.id,
    c.username,
    pr.id AS product_id,
    pr.name,
    pr.price
FROM customer c

JOIN purchase p
ON p.customer_id = c.id

JOIN product pr
ON p.product_id = pr.id
";
    IEnumerable<Customer> customers = await Db.OneToManyAsync<Customer, Product>(
        sql,
        customer => customer.Id,
        (customer, product) => customer.PurchasedProducts.Add(product), // <- Callback
        splitOn: "id, product_id");
    return customers;
}
```

### OneToManyFirstOrDefaultAsync<TOne, TMany>
Return TOne entity or null. Calls callback for each TOne, TMany entity pair. For example find customer by ID with his purchases:

```csharp
public async Task<Customer?> ReadCustomerById(int customerId)
{
    string sql = @"
SELECT
    c.id,
    c.username,
    pr.id AS product_id,
    pr.name,
    pr.price
FROM customer c

JOIN purchase p
ON p.customer_id = c.id

JOIN product pr
ON p.product_id = pr.id

WHERE c.id = @customerId
";
    Customer? customer = await Db.OneToManyFirstOrDefaultAsync<Customer, Product>(
        sql,
        (customer, product) => customer.PurchasedProducts.Add(product), // <- Callback
        splitOn: "id, product_id",
        param: new { customerId });
     return customer;
}
```

### QueryOneToOneAsync<TParent, TChild>
Return collection of TParent entities. Calls callback for each TParent, TChild pair. Unlike ```OneToMany``` does not identify TParent entities by ID, so even if there is several rows with same ID they will be different instances of TParent in returning collection. Example TODO

## License

[MIT](https://choosealicense.com/licenses/mit/)
