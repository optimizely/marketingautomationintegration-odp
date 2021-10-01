
# Optimizely Labs ODP Marking Automation Integration for Optimizely Forms
## Setup
### ODP
Before we can see the odp fields for the customer table and lists for campaigns, we need to enter the private api key into the configuration manager for handling the connection between the ODP connector and ODP REST API.
To do so, we must first login to ODP and navigate to the account settings page.  Once we have hit the account settings page, we care going to click on "APIs" under the DATA MANAGEMENT section.  This will bring us to the APIs page which will allow us to copy the private key from the tab pane.
Copy the key somewhere safe.
![ODP Account Settings](https://github.com/optimizely/marketingautomationintegration-odp/blob/main/images/ODP-AccountSettings.png)
### Optimizely
Once we have our api key, we are going to head over the the web.config and you should see 2 entries.  You will need to update the appSettings key *"ma-odp-apikey"*.  Once the key is inserted into the config value, you should be done with configuration
```xml
<add key="ma-odp-apikey" value="" />
```
