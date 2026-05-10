### Overview

This repository is part of the AscentLister Project and contains the mobile app. The goal for this project was to create an mobile app where I and you can log the climbing route ascents. The project is designed and set up as a local system, where everyone runs a database, api and app on its own. Therefor, there is no user integration, clientid and secret are used for connection and authentication.

The project contains the following repos:
- https://github.com/OliverFrey/AscentLister
- https://github.com/OliverFrey/AscentListerAPI

### How to use it
1. Create a Keycloak instance or a simular authentication service. Be aware that the authentication workflow may differ with other authentication services.
2. Create a PostgreSQL database and import the database schema.
3. Configure the app to use the Keycloak instance and the PostgreSQL database. For this rename the appsettings.json_template to appsettings.json and configure the settings.
4. Run the app.