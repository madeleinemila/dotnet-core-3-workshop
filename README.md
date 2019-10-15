# ASP.NET Core - NDC 2019 App Building Workshop

On this branch, we attempted to publish using Docker.

(Works except for Identity. Also had to switch to Postgres db.)

### Commands to run:

From root level

`docker-compose build`
`docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d`

Then go to the backend: http://localhost:45801
If you hit an endpoint on Swagger, notice how you get a UI returned - that's the package that lets you run migrations.
Instead hit the endpoint directly, e.g. http://localhost:45801/api/Sessions, and click Apply Migrations.

### Other commands:

`docker ps -a` - See processes
`docker-compose down -v` - Kill Docker
`docker-compose logs <name or start of container id>` - Troubleshoot
e.g. `docker-compose logs db` or `docker-compose logs 819`
You only need enough of the container ID to be unique.
`docker attach <container id snippet>`
