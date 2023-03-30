using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using DogGo.Models;

namespace DogGo.Repositories
{
    public class WalkerRepository: IWalkerRepository
    {
        private readonly IConfiguration _config;

        //The constructor accepts an IConfiguration object as a parameter. 
        //This class comes from the ASP.Net Framework and is useful for 
        //retrieving things out of the appsettings.json file like connection strings.

        public WalkerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {

            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            }
        }

        public int TotalWalksTime (int walkerId)
        {
            using (SqlConnection conn = Connection) 
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT
                SUM(Duration) as ALLWalksDuration, 
                WalkerId as walkerId
                FROM Walks
                WHERE walkerId = @walkerId
                GROUP BY walkerId";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        if (reader.Read())
                        {
                            int WalkDuration = reader.GetInt32(reader.GetOrdinal("AllWalksDuration"));
                            return WalkDuration;
                        }
                        else
                        {
                            return 0;
                        }
                
                    }
                }
                }
        }
        public List <Walk> WalksbyWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Walks.Id, 
                Date as walkDate,
                Duration as walkDuration,
                WalkerId as walkerId, 
                Walks.DogId,
                Dog.OwnerId,
                Dog.Id,
                Owner.Id,
                Owner.Name ownerName
                FROM Walks
                Left Join Dog on Walks.DogId = Dog.Id
                Left Join Owner on Dog.OwnerId = Owner.Id
                WHERE walkerId = @walkerId
            ";
                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();
                        while (reader.Read())
                        {
                            Walk walk = new Walk()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("walkDate")),
                                Duration = reader.GetInt32(reader.GetOrdinal("walkDuration")),
                                OwnerName = reader.GetString(reader.GetOrdinal("ownerName"))
                            };


                            walks.Add(walk);
                        }
                        return walks;
                    }
                }
            }
        }
        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Walker.Id, Walker.[Name], Walker.ImageUrl, Walker.NeighborhoodId, 
                           Neighborhood.Id as neighborHoodId, Neighborhood.Name as neighborHoodName
                    FROM Walker
                    LEFT JOIN Neighborhood on Neighborhood.Id = Walker.NeighborhoodId";

                using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walker> walkers = new List<Walker>();
                        while(reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood
                                {
                                   Id=reader.GetInt32(reader.GetOrdinal("neighborHoodId")),
                                   Name = reader.GetString(reader.GetOrdinal("neighborHoodName"))
                                }

                            };
                            walkers.Add(walker);
                        }
                        return walkers;
                    }
                }
            }
        }
        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                     SELECT Walker.Id, Walker.[Name], Walker.ImageUrl, Walker.NeighborhoodId, 
                           Neighborhood.Id as neighborHoodId, Neighborhood.Name as neighborHoodName
                    FROM Walker
                    LEFT JOIN Neighborhood on Neighborhood.Id = Walker.NeighborhoodId
                    WHERE NeighborhoodId = @neighborhoodId";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walker> walkers = new List<Walker>();
                        while (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("neighborHoodId")),
                                    Name = reader.GetString(reader.GetOrdinal("neighborHoodName"))
                                }

                            };
                            walkers.Add(walker);
                        }
                            return walkers;
                    }
                }
            }
        }
        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn =Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Walker.Id, Walker.[Name], ImageUrl, 
                        Neighborhood.Id as neighborHoodId, Neighborhood.Name as neighborHoodName
                        FROM Walker
                        LEFT JOIN Neighborhood on Neighborhood.Id = Walker.NeighborhoodId
                        WHERE Walker.Id=@id";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Walker walker = new Walker()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("neighborHoodId")),
                                    Name = reader.GetString(reader.GetOrdinal("neighborHoodName"))
                                }
                            };
                            return walker;

                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
