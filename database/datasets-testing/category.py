import csv
import random
import string
import argparse

def random_string(max_length):
    length = random.randint(1, max_length)
    return ''.join(random.choices(string.ascii_letters + string.digits, k=length))

def generate_csv(n):
    with open("./datasets-output/category.csv", mode="w", newline="", encoding="utf-8") as csvfile:
        writer = csv.writer(csvfile)
        writer.writerow(["name", "description"])

        for _ in range(n):
            name = random_string(30)

            # 70% chance de description ser null
            if random.random() < 0.7:
                description = None
            else:
                description = random_string(100)

            writer.writerow([name, description])

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("N", type=int, help="Number of entries to generate")

    args = parser.parse_args()
    generate_csv(args.N)
