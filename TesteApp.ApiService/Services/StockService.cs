
using System.Collections.Concurrent;
using Grpc.Core;
using Teste.Shared;

namespace TesteApp.ApiService.Services
{
    public class StockService: StockTradings.StockTradingsBase
    {
        private static readonly ConcurrentDictionary<string, double> StockPrices = new();
        private static readonly System.Timers.Timer priceUpdateTimer = new(1000);
        public StockService()
        {
            StockPrices["GOLD"] = 3000.00;
            priceUpdateTimer.Elapsed += (sender, e) => UpdateSickPrices();
            priceUpdateTimer.Start();
        }
        private void UpdateSickPrices()
        {
            var random = new Random();
            foreach (var key in StockPrices.Keys)
            {
                StockPrices[key] += random.NextDouble() * 2 - 1;
            }
        }


        public override Task<OrdeResponse> Placeorder(OrderRequest request, ServerCallContext context)
        {
            return Task.FromResult(new OrdeResponse
            {
                Message = $"Placed {request.OrderType} order for {request.Symbol} stocks"
            });
        }
        
        public override async Task SubscribeStockprices(StockRequest request, 
            IServerStreamWriter<StockResponse> responseStream, 
            ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested) {
                await responseStream.WriteAsync(new StockResponse
                {
                    Symbol = request.Symbol,
                    Price = StockPrices.GetValueOrDefault(request.Symbol, 0)
                });
                await Task.Delay(1000);
            }
        }
    }
}
