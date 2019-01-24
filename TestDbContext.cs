using Microsoft.EntityFrameworkCore;

public class TestDbContext : DbContext, IThingDb
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
    DbSet<Thing> IThingDb.Things { get; set; }
}

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Thing
{
    public int Id { get; set; }
    public string Name { get; set; }
}

interface IThingDb
{
    DbSet<Thing> Things { get; set; }
}
