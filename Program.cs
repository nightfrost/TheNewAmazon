using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheNewAmazon.Models;
using TheNewAmazon.Operations;

namespace TheNewAmazon
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize variables...
            List<User> users = new List<User>();
            List<Product> products = new List<Product>();
            List<UserSession> userSessions = new List<UserSession>();
            //Key: Movie Name, Value: Units Sold.
            Dictionary<string, int> movieRecommendationsByUnitsSold = new Dictionary<string, int>();
            //Key: Movie Name, Value: Unit viewed amount.
            Dictionary<string, int> movieRecommendationsByUnitsViewed = new Dictionary<string, int>();
            //Key: User Name, Value: Movie recommendations.
            Dictionary<string, List<string>> movieRecommendationsByGenre = new Dictionary<string, List<string>>();

            //Parse data from CSV files to lists containing objects...
            try
            {
                users = CsvOperations.ParseCsvToUsers();
                products = CsvOperations.ParseCsvToProducts();
                userSessions = CsvOperations.ParseCsvToUserSession();
            }
            catch (Exception e)
            {
                //If this fails, exit no matter what...
                Console.WriteLine("Something failed.... \n" + e.Message);
                Console.WriteLine("Exiting after any button pressed....");
                Console.ReadKey();
                Environment.Exit(0);
            }

            /*
             * Task1:
             * List movie recommendations based on movies sold and viewed.
             * Logic can be found in recommendationHelper.
            */
            //Get movie recommendations based on units sold, and print to user. 
            movieRecommendationsByUnitsSold = RecommendationHelper.ListOfRecommendedMoviesByUnitsSold(users, products);
            Console.WriteLine("Movie recommendations based on units sold:");
            foreach (var item in movieRecommendationsByUnitsSold.Take(4))
            {
                Console.WriteLine($"Movie: {item.Key}, Sold amount: {item.Value}");
            }

            //Get movie recommendations based on units viewed, and print to user.
            movieRecommendationsByUnitsViewed = RecommendationHelper.ListOfRecommendedMoviesByUnitsViewed(users, products);
            Console.WriteLine("\n\nMovie recommendations based on units viewed:");
            foreach (var item in movieRecommendationsByUnitsViewed.Take(4))
            {
                Console.WriteLine($"Movie: {item.Key}, Viewed amount: {item.Value}");
            }
            Console.ReadKey();
            Console.WriteLine("\n");

            /*
             * Task2:
             * List movie recommendations based on currentUserSession and movie genres.
             * Logic can be found in recommendationHelper.
             */
            movieRecommendationsByGenre = RecommendationHelper.ListOfRecommendedMoviesByMovieGenre(users, products, userSessions);
            foreach (var recommendation in movieRecommendationsByGenre.Keys)
            {
                Console.WriteLine($"Movie recommendations for: {recommendation}");
                foreach (var product in movieRecommendationsByGenre[recommendation])
                {
                    Console.WriteLine($"{product}");
                }
                Console.WriteLine("\n");
            }
            Console.ReadKey();
        }
    }
}
