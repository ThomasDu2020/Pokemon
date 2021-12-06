# Getting Started
This project is not a production-ready App, but a complete and robust App which is based on .Net core WebApi.

## Complete Unit Tests
The project include totally detailed test cases including unit tests and integration tests, which can lead to a code coverage rate of 100%.

## Production Consideration
However for production App, there are at least stuff below needed to consider:
- Add Authentication and Autorisaztion;
- Unified Http Response processing, especially for the error and exception handling;
- Considering configuration; Like the PokeApi and translator's Url should be configuration, instead of hard coding;
- Decoupling the controller and Pokemon logic by using Mediator Pattern, like using MediatR;


# Build
The project is compiled with Microsoft Visual Studio Community 2019 (Version 16.11.6) based on NET 5.

```bash
cd src\Pokemon
dotnet build
```

# Run
```bash
cd src\Pokemon\bin\Release\net5.0
dotnet pokemon.dll
```

# Build and Run With Docker

## build
```bash
cd src\Pokemon
docker build -t pokemon -f src/Pokemon/Dockerfile .
```

## Run
```bash
docker run -d -p 5000:80 -p 5001:443 pokemon_v1
```

