using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheNewAmazon.Models;

namespace TheNewAmazon.Operations
{
    static class RecommendationHelper
    {
        /// <summary>
        /// Takes a list of users and products, and then sorts and returns the list based on units sold, from users.
        /// </summary>
        /// <param name="users"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public static Dictionary<string, int> ListOfRecommendedMoviesByUnitsSold(List<User> users, List<Product> products)
        {
            Dictionary<string, int> unitsSold = new Dictionary<string, int>();

            //Fill unitsSold dictionary...
            foreach (Product product in products)
            {
                foreach (User user in users)
                {
                    //As purchased products is loaded in as "x;y;z;k" we have to split it.
                    string[] values = user.purchasedProducts.Split(';');

                    foreach (var item in values)
                    {
                        if (item.Replace(" ", "").Equals(product.id.ToString()))
                        {
                            //Check if products already exists, if yes update, otherwhise add.
                            if (unitsSold.Keys.Contains(product.name))
                            {
                                unitsSold[product.name] = unitsSold[product.name] + 1;
                            }
                            else
                            {
                                unitsSold.Add(product.name, 1);
                            }
                        }
                    }
                }
            }

            //Sort unitsSold by value and convert back to Dict. The most sold item is now the first item.
            var sortedDict = from entry in unitsSold orderby entry.Value descending select entry;
            return unitsSold = sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        /// Takes a list of users and products, and then sorts and returns the list based on units viewed, from users.
        /// </summary>
        /// <param name="users"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public static Dictionary<string, int> ListOfRecommendedMoviesByUnitsViewed(List<User> users, List<Product> products)
        {
            Dictionary<string, int> unitsViewed = new Dictionary<string, int>();

            //Fill unitsSold dictionary...
            foreach (Product product in products)
            {
                foreach (User user in users)
                {
                    //As purchased products is loaded in as "x;y;z;k" we have to split it.
                    string[] values = user.viewedProducts.Split(';');

                    foreach (var item in values)
                    {
                        if (item.Replace(" ", "").Equals(product.id.ToString()))
                        {
                            //Check if products already exists, if yes update, otherwhise add.
                            if (unitsViewed.Keys.Contains(product.name))
                            {
                                unitsViewed[product.name] = unitsViewed[product.name] + 1;
                            }
                            else
                            {
                                unitsViewed.Add(product.name, 1);
                            }
                        }
                    }
                }
            }

            //Sort unitsSold by value and convert back to Dict. The most sold item is now the first item.
            var sortedDict = from entry in unitsViewed orderby entry.Value descending select entry;
            return unitsViewed = sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static Dictionary<string, float> ListOfRecommendedMoviesByUserRating(List<User> users, List<Product> products)
        {
            Dictionary<string, float> moviesByRating = new Dictionary<string, float>();

            foreach (Product product in products)
            {
                if (moviesByRating.Keys.Contains(product.name))
                {
                    moviesByRating[product.name] = product.rating;
                }
                else
                {
                    moviesByRating.Add(product.name, product.rating);
                }
            }

            var sortedDict = from entry in moviesByRating orderby entry.Value descending select entry;
            return moviesByRating = sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        /// Takes a list of users, products and currentUserSessions, and then creates a list of recommendations for the individual users.
        /// </summary>
        /// <param name="users"></param>
        /// <param name="products"></param>
        /// <param name="userSessions"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> ListOfRecommendedMoviesByMovieGenre(List<User> users, List<Product> products, List<UserSession> userSessions)
        {
            //The dictionary to be returned.
            Dictionary<string, List<string>> recommendations = new Dictionary<string, List<string>>();

            foreach (UserSession session in userSessions)
            {
                //Initialize variables
                Dictionary<int, string> currentSessionGenres = new Dictionary<int, string>();
                List<string> tempProducts = new List<string>();
                Product currentProduct = new Product();
                User currentUser = new User();
                List<string> genres = new List<string>();

                //Get current Movie info.
                products.ForEach(product =>
                {
                    if (session.productID == product.id)
                    {
                        currentProduct = product;
                    }
                });

                //get current User info.
                users.ForEach(user =>
                {
                    if (session.userID == user.id)
                    {
                        currentUser = user;
                    }
                });

                //Add currentProduct genres to local List variable genres.
                genres = currentProduct.getGenresAsList();

                //Locate recommendations based on genres..
                foreach (Product product in products)
                {
                    int matches = 0;

                    //Check against all genres and count matches up if found.
                    if(genres.Any(genre => (product.keywordOne.Contains(genre)))) matches++;

                    if (genres.Any(genre => (product.keywordTwo.Contains(genre)))) matches++;
                    
                    if (genres.Any(genre => (product.keywordThree.Contains(genre)))) matches++;
                    
                    if (genres.Any(genre => (product.keywordFour.Contains(genre)))) matches++;
                    
                    if (genres.Any(genre => (product.keywordFive.Contains(genre)))) matches++;

                    //add movie to recommendation if 2 or more genres match.
                    if (matches >= 3)
                    {
                        tempProducts.Add(product.name);
                    }
                }

                //Check if products already exists, if yes update, otherwhise add.
                if (recommendations.ContainsKey(currentUser.name))
                {
                    recommendations[currentUser.name] = tempProducts;
                }
                else
                {
                    recommendations.Add(currentUser.name, tempProducts);
                }
            }

            return recommendations;
        }


        public static List<Product> ListOfRecommendedMoviesByUnitsSoldAndUserReviews(List<User> users, List<Product> products)
        {
            List<Product> moviesOnlyWithHighRating = new List<Product>();
            List<Product> moviesWithUnitsSold = new List<Product>();

            for (int y = 0; y < products.Count; y++)
            {
                if (products.ElementAt(y).rating > 3.5)
                {
                    moviesOnlyWithHighRating.Add(products.ElementAt(y));
                }
            }

            users.ForEach(user =>
            {
                user.purchasedProductsSplit.ForEach(productId =>
                {
                    products.ForEach(product =>
                    {
                        if (productId.Equals(product.id))
                        {
                            moviesWithUnitsSold.Add(product);
                        }
                    });
                });
            });

            List<Product> movieRecommendationList = moviesWithUnitsSold.Union(moviesOnlyWithHighRating).ToList();
            return movieRecommendationList;
        }
    }
}
