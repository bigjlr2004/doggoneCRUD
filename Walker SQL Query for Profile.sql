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
                Left Join Owner on Dog.OwnerId = Owner.Id;

                SELECT
                
              (SUM(Duration)/60) as ALLWalksDuration, 
                WalkerId as walkerId
                
                FROM Walks
                WHERE walkerId = 2

                GROUP BY walkerId
