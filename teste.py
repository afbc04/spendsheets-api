import random

def test_a():

    f = open("entries.csv","w")

    for i in range(0,1_000_000):

        numero = random.randint(-100, 100)
        f.write(f"1,1,true,A,100,{numero},2026-01-20 17:55:00,2026-01-20,,2026-01-20,,,2\n")

def test_b():

    f = open("entries_movements.csv","w")

    for i in range(0,1_000_000):

        jmax = random.randint(0,5)
        j = 0

        while j < jmax:

            numero = random.randint(-100, 100)
            f.write(f"{i+1},{numero},,2026-01-20\n")
            j += 1



"""
                CREATE TABLE IF NOT EXISTS EntryMovements (
                  id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                  entryId BIGINT NOT NULL,
                  money INTEGER NOT NULL,
                  comment VARCHAR({EntryRules.movement_comment_length_max}),
                  date DATE NOT NULL,
                CONSTRAINT fk_entrymovements_entry
                  FOREIGN KEY (entryId)
                  REFERENCES Entries(id)
                  ON DELETE CASCADE

"""


test_b()