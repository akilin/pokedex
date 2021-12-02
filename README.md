# pokedex
over-engineered pokedex implementation

## How to run
* Install [docker](https://docs.docker.com/get-docker/) & [docker-compose](https://docs.docker.com/compose/install/)
* Download the repo
* From repo folder run `docker-compose up -d`

## Urls
| Component   | Url                           |
| ----------- | ----------------------------- |
| Swagger     | http://localhost:5000/swagger |
| Seq         | http://localhost:5341         |
| Grafana     | http://localhost:3000         |
| Prometheus  | http://localhost:9090         |

Grafana credentials are `admin`/`admin`
## Things I'd do different for prod
* Change root namespaces for each project to include `Pokedex` and not just be `Core`/`Infra`/`Web` etc
* Use proper graphql client and not a poor-mans graphql client (HttpClient post)
* Don't use graphql api of `pokeapi` as it's currently in Beta
* Currently `en` language id is hardcoded as a constant. I don't expect it to change, but in theory it's better to retrieve it on app start via their api from `https://pokeapi.co/api/v2/language`
* Maybe would've changed some things that are currently `string` to be `enum` (not pokemons. but maybe translation options?)
* I think the place where I handle the rate-limit error is not entirely correct. It should be in service layer, not where we integrate with transaltion api. I noticed this after i was done and decided to keep as-is. 
* Global error handler (both middleware & unobserved exception event)
* CI/CD pipeline
* Replace some `!` with proper null-checks
* Volumes for `seq`/`prometheus` containers
* Tests for rate-limit err handling
* Clarify requirements in regards to `\n` in description strings. I've observed some strange behaviours from translation apis
* Xml docs?


## Not prod, but something I would've done differently
I've decided to try out new minimal apis that came out with dotnet 6, so the code is written in that style.
Now that i've tried it - i prefer the old way of having `Startup.cs`. It just seems more explicit.

## Potential areas of improvement
Currently our app has a hard dependency on `pokeapi` and soft dependency on `translation` apis.
It's also awfully slow as my ping to both those apis is averaging 150-300ms.
Considering a static nature of the data (it rarely changes, if ever) - it makes sense to introduce some local cache for the data. This will in theory solve both latency & hard dependency issues.

I don't currently handle the case where translation api is dead. Only the one where it is alive and returns some proper errors.