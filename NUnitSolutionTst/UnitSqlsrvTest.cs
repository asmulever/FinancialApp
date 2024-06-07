﻿using NUnit.Framework;
using Microsoft.Extensions.Configuration;

using DllDalFinancial;
using DllEntityLayer;

namespace NUnitProj.Test;

public class UnitSqlsrvTest
{
    private ISqlAbstractGenericRepository<Ticker> _repository;

    [SetUp]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        var sqlConnectionString = configuration.GetConnectionString("SqlConnection");
        _repository = new TickerSqlRepository(sqlConnectionString);

    }

    [Test]
    public void GetUser_Returns_Tiker_With_Correct_Id()
    {
        // Arrange
        int tickerId = 2;

        // Act
        var ticker = _repository.GetByIdAsync(tickerId).Result;

        // Assert
        Assert.IsNotNull(ticker);
        Assert.AreEqual(tickerId, ticker.Id);
    }

    [Test]
    public async Task SetUser_Sets_Ticker_Correctly()
    {
        var ticker = new Ticker
        {
            Fecha = DateTime.Now,
            tickerName = "lac",
            Price = 10.2M,
            Categoria = 10
        };

        var ticker1 = new Ticker
        {
            Fecha = DateTime.Now,
            tickerName = "lac",
            Price = 14.2M,
            Categoria = 10
        };

        // Act
        await _repository.InsertAsync(ticker);
        await _repository.InsertAsync(ticker1);
        var retrievedTicker = await _repository.GetByIdAsync(ticker.Id);

        // Assert
        Assert.IsNotNull(retrievedTicker);
        Assert.AreEqual(ticker.Id, retrievedTicker.Id);

    }

}