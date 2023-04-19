using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Models;

namespace WalletService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateAsyncScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Wallets.Any())
            {
                Console.WriteLine("==> Seeding Data..");
                context.Wallets.AddRange(
                    new Wallet()
                    {
                        Username = "user1",
                        Fullname = "Erzal Fahrezy P",
                        Cash = 50000
                    },
                    new Wallet()
                    {
                        Username = "user2",
                        Fullname = "Rahel Angela C",
                        Cash = 80000
                    },
                    new Wallet()
                    {
                        Username = "user3",
                        Fullname = "Farhan Kusuma",
                        Cash = 65000
                    });
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("==> Data telah tersedia <==");
            }
        }
    }
}