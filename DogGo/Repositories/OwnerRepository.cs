using Microsoft.Extensions.Configuration;
using DogGo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;


namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;


        public OwnerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        { get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            } 
        }

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd= conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, Email, Name, Address, NeighborhoodId, Phone
                    FROM Owner";

                    using SqlDataReader reader = cmd.ExecuteReader();
                    {
                        List<Owner> owners = new List<Owner>();
                        while (reader.Read())
                        {
                            Owner owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone"))
                                };
                           owners.Add(owner);

                            }
                        return owners;
           
                    }
                }

            }
        }
        

    public Owner GetOwnerById(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                  SELECT Owner.Id,  
                                Owner.Email as ownerEmail, 
                                Owner.Name as ownerName, 
                                Owner.Address as ownerAddress , 
                                Owner.NeighborhoodId as ownerNeighborhoodId, 
                                Owner.Phone as ownerPhone,
                                Dog.Id as dogId, 
                                Dog.Name as dogName, 
                                Dog.OwnerId as dogOwnerId
                                FROM Owner
                                left JOIN Dog on dog.OwnerId = Owner.Id
                                WHERE Owner.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    Owner owner = null;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (owner == null)
                        {

                            owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("ownerEmail")),
                                Name = reader.GetString(reader.GetOrdinal("ownerName")),
                                Address = reader.GetString(reader.GetOrdinal("ownerAddress")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("ownerNeighborhoodId")),
                                Phone = reader.GetString(reader.GetOrdinal("ownerPhone")),
                                DogList = new List<Dog>()
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("dogId")))
                        {
                            owner.DogList.Add(new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("dogId")),
                                Name = reader.GetString(reader.GetOrdinal("dogName")),

                            });
                        }
                      
                    }
                        reader.Close();
                        return owner;
                }
            }
        }

        public Owner GetOwnerByEmail(string email)
        {
            using SqlConnection conn = Connection;
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT Id, [Name], Email, Address, Phone, NeighborhoodId
                            FROM Owner
                            WHERE Email = @email";

                    cmd.Parameters.AddWithValue("@email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Owner owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            };
                            return owner;
                        }
                        return null;
                    }
                }
            }
        }
        public void AddOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Owner ([Name], Email, Phone, Address, NeighborhoodId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @email, @phoneNumber, @address, @neighborhoodId);
                ";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@phoneNumber", owner.Phone);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);

                    int id = (int)cmd.ExecuteScalar();

                    owner.Id = id;
                }
            }
        }

        public void UpdateOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Owner
                            SET 
                                [Name] = @name, 
                                Email = @email, 
                                Address = @address, 
                                Phone = @phone, 
                                NeighborhoodId = @neighborhoodId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@phone", owner.Phone);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);
                    cmd.Parameters.AddWithValue("@id", owner.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOwner(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Owner
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", ownerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
    



