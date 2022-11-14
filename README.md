# Cinema-Booking-System

Cinema-Booking-System is an application dedicated to small cinema company, which include among others: user reservation, administrator panel for management of cinema enviroment.
It allows you to make a temporary reservation for a period of 10 minutes or another specified time. All movies are inserting from an external api(omdb and tmdb) via ImdbId. 
Application also includes collaborative recommendation and content-based filtering.

## Tech Stack

**Client:** Vue JS

**Designs Paterns:** SPA

****

**Admin Panel:** Vue JS, PrimeVue

**Designs Paterns:** SPA

****

**Server API:** .NET 5

**Design Paterns:** 
Mediator, CQRS

****

**Server Identity:** .NET 5, IdentityServer4


## Authors

- [@Piotr Saja](https://github.com/PiotrSaja)


## Features

- Informations about movies.
- Search string of movies.
- A temporary seat's reservation system(two users can't reserved the same seats on this same time) - foreground service.
- The user's profile panel.
- The ability to reservatining for seance.
- The administrator's profile panel.
- The CRUD operations for all tables in datbase with validations.
- The functionality that provide for an insertion movie from external API(omdb and tmdb) by imdbId to database.
- The ability to logging and registering user account.
- Movies recomandation system: Collaborative Recommendation and Content-Based Filtering.


## Demo

* Link to the application demo is bellow:

[Web Public - Link](https://saja.website/)

[Web Admin - Link](https://saja.website:44301/)

* Test users data is bellow:

```bash
  Administrator account:
  Login: bob
  Password: Pass123$

  User account:
  Login: user10
  Password: Pass123$
```
