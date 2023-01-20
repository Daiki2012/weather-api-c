1. [C# API on Azure](#c-api-on-azure)
   1. [Local Development](#local-development)
   2. [Deployment](#deployment)
      1. [Endpoints](#endpoints)
   3. [Invocation](#invocation)
   4. [Supported Scenarios](#supported-scenarios)
      1. [Success](#success)
      2. [400 error - listing all cities weather info is not supported](#400-error---listing-all-cities-weather-info-is-not-supported)
      3. [400 Searching non-existent cities in open weather api](#400-searching-non-existent-cities-in-open-weather-api)
      4. [404 Empty string was passed as city name](#404-empty-string-was-passed-as-city-name)
      5. [400 Unexpected input property](#400-unexpected-input-property)
      6. [404 non existing path is called](#404-non-existing-path-is-called)
   5. [Future Implementation](#future-implementation)



# C# API on Azure

REST ful API for OpenWeather

a simple C# API service running on Azure Function with Visual Studio and .NET 6 as Function worker

## Local Development
```
Double click .sln file
F5
```
After running offline, the below urls will be available:
| TYPE | URL                                     |
| ---- | --------------------------------------- |
| GET  | http://localhost:7191/api/cities?name={ENTER CITY NAME}     |
| POST | http://localhost:7191/api/cities?name={Enter CITY NAME}          |

Future endpoints !! (NOT IMPLEMENTED YET)
| GET  | http://localhost:7191/api/cities?lat=xxx&lon=xxx     |
| GET  | http://localhost:7191/api/cities/{cityId}     |

## Deployment
* Follow this Publish [tutorial](https://learn.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio?tabs=in-process#publish-the-project-to-azure)

### Endpoints
After running deploy, the below endpoints will be available:
| TYPE | URL                                                                          |
| ---- | ---------------------------------------------------------------------------- |
| POST | https://xxxxxxxxxxxxx.azurewebsites.net/api/cities?name=Sydney         |


## Invocation
After successful deployment, you can call the created application via HTTP:

```bash
curl https://xxxxxxxxxxxxx.azurewebsites.net/api/cities?name=Sydney
```

Which should result in the following response:

```
{ "message": "Hello Tokyo! Current temperature is 9.62 degree, and weather condition is light intensity shower rain!" }
```


## Supported Scenarios
### Success
```
   When I request "GET /cities?name=Sydney"
   Then I get a "200" response
   And return a message says:
   ""
   Hello Sydney! Current temperature is xx.xx degree, and weather condition is xxxx!
   ""
```

### 400 error - listing all cities weather info is not supported
```
   When I request "Get /cities"
   Then I get "400" with a message "not supported function"
```

### 400 Searching non-existent cities in open weather api
```
   When I request "Get /cities?name=TEST"
   Then I get "400" with a message "City not found"
```

### 404 Empty string was passed as city name
```
   When I request "Get /cities?name="
   Then I get "404" with a message "City name not found"
```

### 400 Unexpected input property
```
   When I request "Get /cities?lat="
   Then I get "400" with a message "City name not found"
```

### 404 non existing path is called
```
   When I request "Get /NONEXISTINGPATH"
   Then I get "404" with a message "Not Found"
```



## Future Implementation
* add more endpoints for different services
* only allow selected users to access the service with JWT Tokens etc? Refer to [`httpApi` event docs](https://www.serverless.com/framework/docs/providers/Azure/events/http-api/).
