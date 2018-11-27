# communications
AberFitness Communications Microservice

| Branch | Status |
|-|-|
| Development | [![Development Build Status](https://travis-ci.org/sem5640-2018/communications.svg?branch=development)](https://travis-ci.org/sem5640-2018/communications) |
| Release | [![Release Build Status](https://travis-ci.org/sem5640-2018/communications.svg?branch=master)](https://travis-ci.org/sem5640-2018/communications) |

# Environment Variables

## Required Keys (All Environments)

| Environment Variable | Default | Description |
|-|-|-|
| ASPNETCORE_ENVIRONMENT | Production | Runtime environment, should be 'Development', 'Staging', or 'Production'. |
| Comms__SendgridApiKey | N/A | Sendgrid API key. |
| Comms__FromAddress | notifications@aberfitness.biz | The email address from which emails should be sent. |
| Comms__FromName | AberFitness | The name from which emails should be sent. |
| Comms__ClientId | N/A | Gatekeeper client ID. |
| Comms__ClientSecret | N/A | Gatekeeper client secret. |
| Comms__ApiResourceName | comms | The name of the API resource being protected. |
| Comms__GatekeeperUrl | N/A | Gatekeeper OAuth authroity URL.  **Important**: Must include a trailing slash. |


## Required Keys (Production + Staging Environments)
In addition to the above keys, you will also require:

| Environment Variable | Default | Description |
|-|-|-|
| Kestrel__Certificates__Default__Path | N/A | Path to the PFX certificate to use for HTTPS. |
| Kestrel__Certificates__Default__Password | N/A | Password for the HTTPS certificate. |
| Comms__ReverseProxyHostname | http://nginx | The internal docker hostname of the reverse proxy being used. |
| Comms__PathBase | /comms | The pathbase (name of the directory) that the app is being served from. |