# Vitalyst.msstemassilentauth

## Create an Azure App Registration

This repository requires an Azure App Registration configured for the app in question, the following elements need to be present in the App Registration in order for the app to successfully authenticate.

1. In the App Authentication options
- Add a Web-based Redirect URI to point to "https://localhost:44370/auth/msteamsSilentEnd"
- Under Implicit grant and hybrid flows, enable both ID and Access Tokens
- Under Supported Account Types, select "Accounts in any organizational directory (Any Azure AD directory - Multitenant)"

2. In Secrets & Certificates options
- Add a Client secret and make note of the secret value to add to the app's [Web.config](src/TeamsSilentAuthMVC/TeamsSilentAuthMVC/Web.config) later.

3. In API Permissions
- Add a permission: Microsoft APIs | Microsoft Graph | Delegated Permissions | add openid, profile, and User.Read

## Set up the Local Repository

4. In your cloned repository, open [Web.config](src/TeamsSilentAuthMVC/TeamsSilentAuthMVC/Web.config) and supply the needed values for the following keys:
- ida:ClientId
- ida:ClientSecret
- ida:Domain
- ida:TenantId

5. Restore any missing NuGet packages needed for your local repository.
6. Run the app from Visual Studio, which should start IIS running on https://localhost:44370.

## Side-load App Package to Teams

7. A [package](src/TeamsSilentAuthMVC/TeamsSilentAuthMVC/Manifest/manifest.zip) .zip file is located in the Manifest folder of the Vitalyst.msstemassilentauth\src\TeamsSilentAuthMVC\TeamsSilentAuthMVC project.
8. Double-check [manifest.json](src/TeamsSilentAuthMVC/TeamsSilentAuthMVC/Manifest/manifest.json) in the [Manifest](src/TeamsSilentAuthMVC/TeamsSilentAuthMVC/Manifest/) folder to ensure all valid values are appropriately set.
9. If you need to edit the [manifest.json](src/TeamsSilentAuthMVC/TeamsSilentAuthMVC/Manifest/manifest.json) file, be sure to re-add it to the .zip file before side-loading to Teams.
10. In Teams, go to Apps | Manage your apps | Upload an App | Upload a custom app | browse to and select the [manifest.zip](src/TeamsSilentAuthMVC/TeamsSilentAuthMVC/Manifest/manifest.zip) package file.
11. Accept the package info dialog and the app should now be available in Teams as "AuthTest (Teams)."
12. Attempt to view the App and log in via the Home tab.
