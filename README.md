# CoronavirusWebScaper

This is a site that collects and shows information about the coronavirus in Bulgaria.

## Description

This site collects every day information about the coronavirus. It collects it from the official site about Covid-19 in Bulgaria (https://coronavirus.bg).
The information is stored in the MongoDB database that is made for this. Every user can easily see if the new cases are more than the heald. If they are more the counter in the right corner will be red. If they are less the counter will be in red.

## Services

### MongoService
This service helps to connect to the database.

### UpdateInformationService
This service collects the information from the site (https://coronavirus.bg) on every 24 hours and adds it in to the database. It works on hosted service.