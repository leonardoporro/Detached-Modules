using Detached.Mappers;
using Detached.Mappers.Model;
using Detached.Modules.EntityFramework.Tests.Mocks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;

namespace Detached.Modules.EntityFramework.Tests
{
    public class MappingTests
    {
        [Fact]
        public void TestMappingComponent()
        {
            // GIVEN a configured application
            IConfiguration configuration = new ConfigurationBuilder().Build();
            IHostEnvironment hostEnvironment = new HostEnvironmentMock();

            Application app = new Application(configuration, hostEnvironment);

            // GIVEN a db context
            app.AddDbContext<TestMappingDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestMapping?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            // GIVEN a mapper
            app.AddMapping<TestMappingDbContext, TestMapping>();

            // WHEN the application is built
            IServiceCollection services = new ServiceCollection();
            app.ConfigureServices(services);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // THEN model configuration is applied
            TestMappingDbContext dbContext = serviceProvider.GetService<TestMappingDbContext>();

            // get the random annotation to check that ConfigureModel was executed.
            IEntityType entityType = dbContext.Model.FindEntityType(typeof(TestMappingEntity));
            IProperty property = entityType.FindProperty("Id");

            Assert.Contains(property.GetAnnotations(), a => a.Name == "test annotation");

            // THEN mapper configuration is applied
            ITypeOptions typeOptions = dbContext.GetInfrastructure().GetService<Mapper>().GetTypeOptions(typeof(TestMappingEntity));
            Assert.True(typeOptions.GetMember("Name").IsKey);
        }

        public class TestMappingEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class TestMappingDbContext : DbContext
        {
            public TestMappingDbContext(DbContextOptions<TestMappingDbContext> options)
                : base(options)
            {
            }

            public DbSet<TestMappingEntity> Entities { get; set; }
        }

        public class TestMapping
        {
            public void ConfigureModel(ModelBuilder modelBuilder)
            {
                // apply a random annotation to check that this was executed.
                modelBuilder.Entity<TestMappingEntity>().Property(t => t.Id).HasAnnotation("test annotation", true);
            }

            public void ConfigureMapper(MapperOptions mapperOptions)
            {
                // apply a random annotation to check that this was executed.
                mapperOptions.Configure<TestMappingEntity>().Member(t => t.Name).IsKey();
            }
        }
    }
}