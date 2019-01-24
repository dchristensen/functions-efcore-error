using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<TestDbContext>
{
    public TestDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot root = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json", true)
            .Build();

        var builder = new DbContextOptionsBuilder<TestDbContext>();
        builder.UseSqlServer(root["Values:DefaultConnection"]);
        return new TestDbContext(builder.Options);
    }
}