using Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exchange.Data
{
    public class ExchangeDbInitializer
    {
        public static void Initialize(ExchangeDbContext context, string filePath = "")
        {
            context.Database.EnsureCreated();
            if (!context.Currencies.Any())
            {
                var currencies = new Currency[]
                {
                    new Currency(){ Name ="Sol peruano",Symbol ="S/",Code = "PER",CreatedBy ="Enrique Inca" },
                    new Currency(){ Name ="Dólar estadounidense",Symbol ="$",Code = "USD",CreatedBy ="Enrique Inca" }
                };

                context.AddRange(currencies);
                context.SaveChanges();
            }


            if (!context.ExchangeRates.Any())
            {

                var typeChanges = new ExchangeRate[]{
                    new ExchangeRate()
                    {
                        Pair ="USD/PEN",Offer = 4.0860M ,Demand = 4.0900M,ExchangeRateAt = DateTime.Now,CreatedBy ="Enrique Inca"
                    },
                    new ExchangeRate()
                    {
                        Pair ="USD/PEN",Offer = 4.0750M ,Demand = 4.086M,ExchangeRateAt = DateTime.Now.AddDays(-1),CreatedBy ="Enrique Inca"
                    },
                };

                context.AddRange(typeChanges);
                context.SaveChanges();
            }

        }
    }
}
