connection between the ODP connector and ODP REST API.
To do so, we must first login to ODP and navigate to the account settings page.  Once we have hit the account settings page, we care going to click on "APIs" under the DATA MANAGEMENT section.  This will bring us to the APIs page which will allow us to copy the private key from the tab pane.
Copy the key somewhere safe.
![ODP Account Settings](https://github.com/optimizely/marketingautomationintegration-odp/blob/main/images/ODP-AccountSettings.png)
### Optimizely
Once we have our api key, we are going to login to Optimizely and click the Admin tab.  This will bring us to the admin screen where we will need to navigate to the "Config" tab and navigate to "Configuration Manager" located under the TOOLS SETTINGS section.  You will be presented with a configuration manager screen whcih will list all of your saved options.  
You can either edit the current option for ODP or if this is the first time, select the Option Type "Optimizely.Labs.MarketingAutomationIntegration.ODP.SettingsOptions" from the dropdown.  
Take the key you saved in the ODP account settings screen and paste it as the value for APIKey.  
```sh
"APIKey":"your key here"
```
and then click save.

![Configuration Manager](https://github.com/optimizely/marketingautomationintegration-odp/blob/main/images/optimizely-connector-configuration.png)

## For more information on setting up your form
https://webhelp.optimizely.com/18-2/addons/marketing-automation/forms-connector.htm