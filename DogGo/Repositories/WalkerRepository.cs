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
                        SELECT Id, [Name], ImageUrl, NeighborhoodId
                        FROM Walker
                        Where Id = @id";

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
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
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
