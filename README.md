# Refactor Guidance Tool
A tool designed to aid developers perform code refactorings by giving structural refactoring advice.

## Features
- Probably
- Some
- Cool
- Stuff

## Docker

```sh
cd path\to\Refactor-Guidance-Tool
docker build -t isa-lab/refactor-guidance-tool .
```

This will create the Refactor Guidance Tool image and pull in the necessary dependencies.

After that run the Docker image, and map the port you want to use to access Swagger (in this case 1337).
Also make sure to mount the directory of the project you want to refactor to the /app/project directory.

```sh
docker run -d -p 1337:80 --name isa-lab/refactor-guidance-tool --rm -v path/to/refactor/project:/app/project refactor-guidance-tool
```

> Note: `:80` is required to put after the desired port, as port 80 is exposed by default by the Docker image.

All set! Now you can open up the Swagger panel:

```sh
http://localhost:1337/Swagger
```

## License

Yes
