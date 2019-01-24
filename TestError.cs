using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public static class TestError
{
    [FunctionName(nameof(TestError))]
    public static async Task Run(
        [TimerTrigger("0 */30 * * * *", RunOnStartup = true)]
        TimerInfo timerInfo,
        ILogger logger, CancellationToken cancellationToken)
    {
        try
        {
            var builder = new DbContextOptionsBuilder<TestDbContext>();
            builder.UseSqlServer(Environment.GetEnvironmentVariable("DefaultConnection"));
            using (var ctx = new TestDbContext(builder.Options))
            {
                await ctx.Database.EnsureCreatedAsync(cancellationToken);
                await ctx.Database.MigrateAsync(cancellationToken);

                IThingDb thingDb = ctx;

                ctx.Items.Add(new Item {Name = "Item #1"});
                thingDb.Things.Add(new Thing {Name = "Thing #1"});
                await ctx.SaveChangesAsync(cancellationToken);

                var items = await ctx.Items.Take(10).ToListAsync(cancellationToken);
                foreach (Item item in items)
                {
                    logger.LogInformation("Item - {item}", item.Name);
                }

                var things = await thingDb.Things.Take(10).ToListAsync(cancellationToken);
                foreach (Thing thing in things)
                {
                    logger.LogInformation("Thing - {thing}", thing.Name);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error running function: ");
        }
    }
}
