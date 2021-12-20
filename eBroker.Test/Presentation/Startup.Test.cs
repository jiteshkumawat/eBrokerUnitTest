using eBroker.Business;
using eBroker.Core;
using eBroker.DAL;
using eBroker.Model;
using eBroker.Presentation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace eBroker.Test.Controller
{
    /// <summary>
    /// Startup Test Class
    /// </summary>
    public class StartupTest
    {
        /// <summary>
        /// Verify Creation of Web Host
        /// </summary>
        [Fact]
        public void Startup_VerifyCreation()
        {
            // ACT
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();

            // ASSERT
            Assert.NotNull(webHost);
        }

        /// <summary>
        /// Verify Services Available
        /// </summary>
        [Fact]
        public void Startup_VerifyServicesNotNull()
        {
            // ACT
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            
            // ASSERT
            Assert.NotNull(webHost.Services.GetRequiredService<IOperationsUtilityProxy>());
            Assert.NotNull(webHost.Services.GetRequiredService<IBrokerContext>());
            Assert.NotNull(webHost.Services.GetRequiredService<IRepository<Equity>>());
            Assert.NotNull(webHost.Services.GetRequiredService<IManager<Equity>>());
            Assert.NotNull(webHost.Services.GetRequiredService<IRepository<Trader>>());
            Assert.NotNull(webHost.Services.GetRequiredService<ITraderManager>());
        }

        /// <summary>
        /// Verify Services Instance
        /// </summary>
        [Fact]
        public void Startup_VerifyServicesInstance()
        {
            // ACT
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            
            // ASSERT
            Assert.IsType<OperationsUtilityProxy>(webHost.Services.GetRequiredService<IOperationsUtilityProxy>());
            Assert.IsType<BrokerContext>(webHost.Services.GetRequiredService<IBrokerContext>());
            Assert.IsType<EquityRepository>(webHost.Services.GetRequiredService<IRepository<Equity>>());
            Assert.IsType<EquityManager>(webHost.Services.GetRequiredService<IManager<Equity>>());
            Assert.IsType<TraderRepository>(webHost.Services.GetRequiredService<IRepository<Trader>>());
            Assert.IsType<TraderManager>(webHost.Services.GetRequiredService<ITraderManager>());
        }
    }
}
