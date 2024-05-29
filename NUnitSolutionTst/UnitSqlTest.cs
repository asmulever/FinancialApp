using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DllEntityLayer;
using DllDalFinancial;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NUnitProj.Test;

[TestFixture]
public class TickerSqlRepositoryTests
{
    private MyDbContext _context;
    private TickerSqlRepository _repository;

    [SetUp]
    public void Setup()
    {
        // Cargar la configuración de appsettings.Test.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build();

        var connectionString = configuration.GetConnectionString("SqlConnection");

        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        _context = new MyDbContext(options);
        _repository = new TickerSqlRepository(_context);

        // Limpiar la base de datos de prueba
        //_context.Database.EnsureDeleted();
        //_context.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task CreateAsync_AddsTicker()
    {
        // Arrange
        var ticker = new Ticker { Fecha = DateTime.Now, tickerName = "MMM", Categoria = 1 };

        // Act
        var newId = await _repository.CreateAsync(ticker);
        var result = await _repository.GetAsync(newId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(newId, result.Id);
        Assert.AreEqual("MMM", result.tickerName);
    }

    [Test]
    public async Task GetAsync_ReturnsTicker_WithCorrectId()
    {
        // Arrange
        var ticker = new Ticker { Fecha = DateTime.Now, tickerName = "MMM", Categoria = 1 };
        _context.Tickers.Add(ticker);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync(ticker.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(ticker.Id, result.Id);
        Assert.AreEqual("MMM", result.tickerName);
    }
}
