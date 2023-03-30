using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkRepository: IWalkRepository
    {
        private readonly IConfiguration _config;
        public WalkRepository(IConfiguration config)
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



        public List<Walk> GetAllWalks()
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
                Dog.Name as dogName,
                Dog.Id,
                Walker.Name as walkerName,
                Owner.Id,
                Owner.Name ownerName
                FROM Walks
                Left Join Dog on Walks.DogId = Dog.Id
                Left Join Owner on Dog.OwnerId = Owner.Id
                Left Join Walker on Walker.Id = walkerId
                ";
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
                                OwnerName = reader.GetString(reader.GetOrdinal("ownerName")),
                                DogName = reader.GetString(reader.GetOrdinal("dogName")),
                                WalkerName = reader.GetString(reader.GetOrdinal("walkerName"))

                            };
                            walks.Add(walk);
                        }
                        return walks;
                    }
                }
            }
        }
            public void AddAWalk(Walk walk)
            { 

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Walks ( Date, Duration, WalkerId, DogId)
                    OUTPUT INSERTED.ID
                    VALUES (@date, @duration, @walkerId, @dogId);
                ";

                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration*60);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);
       

                    int id = (int)cmd.ExecuteScalar();

                    walk.Id = id;
                }
            }
        }
    

    }
}