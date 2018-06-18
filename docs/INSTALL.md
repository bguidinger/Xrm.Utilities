# Installation
First, download the files from the [Releases](../../../releases).

## Dynamics 365
1. Go to Settings > Solutions, click Import.
 
2. Follow the prompts to import the `VersionChecker_X_X_X_X_managed.zip` file.

3. After the solution is imported, open it up.

4. Go to the Configuration page, set the username/password, and click Submit.

   ![](./VersionChecker_Solution_Config.png "Solution Configuration")

5. Open the `Check Version` process and activate it.

## Azure AD
1. Open Azure Active Directory from the [Azure Portal](https://portal.azure.com).

2. Open App registrations.

3. Click the link to add a new application registration.

   ![](./AzureAD_Create.png "New Registration")

4. After the application registration is created, copy the Application ID.  You will use this as the Client ID.

5. Click Settings.

6. Click Reply URLs, add the wildcard [Reply URL](https://*.consent.azure-apim.net/redirect) used by Flow, then click Save.

   ![](./AzureAD_ReplyURLs.png "Reply URLs")

7. Add a required permission for Dynamics CRM Online.  After choosing the delegated permission, click Select, and then click Done.

   ![](./AzureAD_Permissions.png "Permissions")

8. Add a new key. After saving, copy the generated Value.  You will use this as the Client Secret below.

   ![](./AzureAD_Keys.png "Keys")


## Flow
1. Create the connector by importing the OpenAPI file.

   ![](./Flow_Connector_Import.png "Import Connector")
   ![](./Flow_Connector_Create.png "Create Connector")

2. After the import, replace the Host with your Dynamics 365 host.

   ![](./Flow_Connector_General.png "General")

3. Next, configure Security.  Use the Client ID/Secret from the app registration above.  Also replace the Resource URL.

   ![](./Flow_Connector_Security.png "Security")

4. Click on the Test link, then click Create Connector.

5. Create a new connection.

   ![](./Flow_Connector_NewConnection.png "New Connection")

6. After logging in, you will be asked to consent to the permissions.

   ![](./Flow_Connector_Consent.png "Grant Consent")

7. Finally, test the API call.  
   _You may get an error stating that the operation is being provisioned.  Wait approximately 30 minutes and try again._

   ![](./Flow_Connector_Provisioning.png "Grant Consent")