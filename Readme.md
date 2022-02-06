## Meter Reading API

This repository contains:

- An api that can used to process a meter readings csv file, and return back the number of successful and failed readings. The api includes a Swagger page that can be used to try out the endpoint.
- A console app that calls the api with a sample meter readings csv file and outputs the response
- Unit tests, using XUnit and FakeItEasy.
- For simplicity, the repositories are implemented using in-memory persistence