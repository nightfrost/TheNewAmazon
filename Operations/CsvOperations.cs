using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using TheNewAmazon.Models;

namespace TheNewAmazon.Operations
{
    static class CsvOperations
    {
        private static CsvConfiguration Configure()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            return config;
        }

        public static List<User> ParseCsvToUsers()
        {
            CsvConfiguration config = Configure();
            using (var reader = new StreamReader(@"C:\Users\Lucas\Documents\Visual Studio 2017\Projects\TheNewAmazon\TheNewAmazon\Ressources\Users.txt"))

            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<User>();
                return records.ToList();
            }
        }

        public static List<Product> ParseCsvToProducts()
        {
            CsvConfiguration config = Configure();
            using (var reader = new StreamReader(@"C:\Users\Lucas\Documents\Visual Studio 2017\Projects\TheNewAmazon\TheNewAmazon\Ressources\Products.txt"))

            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Product>();
                return records.ToList();
            }
        }

        public static List<UserSession> ParseCsvToUserSession()
        {
            CsvConfiguration config = Configure();
            using (var reader = new StreamReader(@"C:\Users\Lucas\Documents\Visual Studio 2017\Projects\TheNewAmazon\TheNewAmazon\Ressources\CurrentUserSession.txt"))

            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<UserSession>();
                return records.ToList();
            }
        }
    }
}
