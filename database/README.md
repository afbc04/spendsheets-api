# Init postgreSQL in docker

    docker exec -it ss-database psql -U <username> -d <database>
    docker exec -it ss-database psql -U admin -d spendSheetDev

    docker cp <src file path> ss-database:/tmp/<file>
    docker cp database/datasets-testing/datasets-output/category.csv ss-database:/tmp/category.csv

    COPY Categories(name,description) FROM '/tmp/category.csv' DELIMITER ',' CSV HEADER;


# Datasets

## Category