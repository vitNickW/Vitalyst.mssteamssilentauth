# Vitalyst.msstemassilentauth

## Create an Azure App Registration

This repository requires an Azure App Registration configured for the app in question, the following elements need to be present in the App Registration in order for the app to successfully authenticate.

1. In the App Authentication options
- Add a Web-based Redirect URI to point to "https://localhost:44370/auth/msteamsSilentEnd"
- Under Implicit grant and hybrid flows, enable both ID and Access Tokens
- Under Supported Account Types, select "Accounts in any organizational directory (Any Azure AD directory - Multitenant)"

2. In Secrets & Certificates options
- Add a Client secret and make note of the secret value to add to the app's web.config later.

3. In API Permissions
- Add a permission | Microsoft APIs | Microsoft Graph | Delegated Permissions | and add openid, profile, and User.Read

## Set up the Local Repository

4. In your cloned repository, open web.config and supply the needed values for the following keys:
- ida:ClientId
- ida:ClientSecret
- ida:Domain
- ida:TenantId

5. Restore any missing NuGet packages needed for your local repository.
6. Run 
