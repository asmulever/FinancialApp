﻿using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using DllDalFinancial;
using DllEntityLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NUnitProj.Test
{
    [TestFixture]
    public class UnitSqlsrvTest
    {
        private SqlAbstractGenericRepository<Ticker> _repository;
        private SqlConnection _connection;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();

            var sqlConnectionString = configuration.GetConnectionString("SqlConnection");
            _connection = new SqlConnection(sqlConnectionString);
            _repository = new TickerSqlRepository(_connection);
        }

        [TearDown]
        public void TearDown()
        {
            var cmd = new SqlCommand("DELETE FROM Ticker",_connection); // Limpieza de la tabla Ticker            
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            _connection.Dispose();
        }

        [Test]
        public async Task GetTicker_Returns_Ticker_With_Correct_Id()
        {
            // Arrange
            var ticker = new Ticker
            {
                Fecha = DateTime.Now,
                tickerName = "TestTicker",
                Price = 10.2M,
                Categoria = 9
            };

            await _repository.InsertAsync(ticker);

            // Act
            var retrievedTicker = await _repository.GetByIdAsync(ticker.Id);

            // Assert
            Assert.IsNotNull(retrievedTicker);
            Assert.AreEqual(ticker.Id, retrievedTicker.Id);
        }

        [Test]
        public async Task SetTicker_Sets_Ticker_Correctly()
        {
            // Arrange
            var ticker = new Ticker
            {
                Fecha = DateTime.Now,
                tickerName = "SYP",
                Price = 10.2M,
                Categoria = 9
            };

            // Act
            await _repository.InsertAsync(ticker);
            var retrievedTicker = await _repository.GetByIdAsync(ticker.Id);

            // Assert
            Assert.IsNotNull(retrievedTicker);
            Assert.AreEqual(ticker.Id, retrievedTicker.Id);
            Assert.AreEqual(ticker.tickerName, retrievedTicker.tickerName);
            Assert.AreEqual(ticker.Price, retrievedTicker.Price);
            Assert.AreEqual(ticker.Categoria, retrievedTicker.Categoria);
        }

        [Test]
        public async Task GetAll_Returns_All_Tickers()
        {
            // Arrange
            var ticker1 = new Ticker
            {
                Fecha = DateTime.Now,
                tickerName = "Ticker1",
                Price = 10.2M,
                Categoria = 1
            };

            var ticker2 = new Ticker
            {
                Fecha = DateTime.Now,
                tickerName = "Ticker2",
                Price = 15.5M,
                Categoria = 2
            };

            await _repository.InsertAsync(ticker1);
            await _repository.InsertAsync(ticker2);

            // Act
            var allTickers = await _repository.GetAllAsync();

            // Assert
            Assert.IsNotNull(allTickers);
            Assert.AreEqual(2, allTickers.Count());
        }

        [Test]
        public async Task UpdateTicker_Updates_Ticker_Correctly()
        {
            // Arrange
            var ticker = new Ticker
            {
                Fecha = DateTime.Now,
                tickerName = "OldTicker",
                Price = 10.2M,
                Categoria = 9
            };

            await _repository.InsertAsync(ticker);
            ticker.tickerName = "UpdatedTicker";
            ticker.Price = 20.5M;

            // Act
            var updateResult = await _repository.UpdateAsync(ticker);
            var updatedTicker = await _repository.GetByIdAsync(ticker.Id);

            // Assert
            Assert.IsTrue(updateResult);
            Assert.IsNotNull(updatedTicker);
            Assert.AreEqual("UpdatedTicker", updatedTicker.tickerName);
            Assert.AreEqual(20.5M, updatedTicker.Price);
        }

        [Test]
        public async Task DeleteTicker_Deletes_Ticker_Correctly()
        {
            // Arrange
            var ticker = new Ticker
            {
                Fecha = DateTime.Now,
                tickerName = "TickerToDelete",
                Price = 10.2M,
                Categoria = 9
            };

            await _repository.InsertAsync(ticker);

            // Act
            var deleteResult = await _repository.DeleteAsync(ticker.Id);
            var deletedTicker = await _repository.GetByIdAsync(ticker.Id);

            // Assert
            Assert.IsTrue(deleteResult);
            Assert.IsNull(deletedTicker);
        }
    }
}
