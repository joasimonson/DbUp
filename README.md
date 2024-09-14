# DbUp

[DbUp](https://dbup.readthedocs.io/en/latest/) is a .NET library used for managing and executing database migrations. It provides an easy way to deploy schema changes to your database by running SQL scripts and ensuring that they are only run once. DbUp is ideal for managing incremental changes to your database schema over time.

Key features:
- Executes SQL scripts in a specified order.
- Keeps track of which scripts have been executed.
- Supports multiple databases, including PostgreSQL, SQL Server, MySQL, and others.

## PostgreSQL Container

### Create
```bash
docker run --name dbuptests -e POSTGRES_PASSWORD=1234@Test -p 5432:5432 -d postgres
```

### Access
```bash
docker run -it --rm postgres psql -h localhost -U postgres
docker exec -it dbuptests bash
psql -h localhost -U postgres -d dbuptests
```

### Checking all tables created
```sql
select * from information_schema.tables where table_schema = 'public';
```