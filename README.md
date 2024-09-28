# Easy Route Choice API
This is the backend of the easy route choice web application (todo: link).

## Features
TBD

## Installation
TBD

## Resources
This program uses data from the following sources to enrich tour information data:
* Route planning data from the OSRM API (https://project-osrm.org/), which is under the (simplified) 2-clause BSD license (https://opensource.org/license/bsd-2-clause).
* Avalanche information data from EAWS (https://avalanche.report/more/open-data), which is under the CC BY 4.0 Deed license (https://creativecommons.org/licenses/by/4.0/). The associated region data is freely available at https://regions.avalanches.org/.
* Weather forecast data from YR (https://developer.yr.no/), which is under the CC BY 4.0 Deed license (https://creativecommons.org/licenses/by/4.0/).

The data provided by these sources is being filtered but not changed with regards to their meaning.

## Contact: 
You can contact me with questions regarding this project at jf-dev@gmx.de.

## Priority List
1. Establish database access
2. Establish weather, travel data, and avalanche services
3. Establish filtering and memory validation
4. Establish authorization for deleting resources
5. Establish tour data image extraction service
6. Improve logging and error handling