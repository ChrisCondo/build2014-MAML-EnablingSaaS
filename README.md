# Using the Microsoft Azure Management Libraries for Enabling SaaS#
The code in this repository was used to demonstrate usage of MAML to enable SaaS vendors running their web applications in Microsoft Azure. 

The code contains a single Web Project. In this Web Project's AppData folder is a copy of the code for the [MiniBlog](https://github.com/madskristensen/miniblog) blogging tool. The MVC web application represented by the Web Project displays the user a form. The contents of the form are used to create a new Web Site, and to deploy the MiniBlog code from the AppData folder into the new live site. Form data captured from the user are injected into the live site's configuration settings using MAML. The result is a live running instance of MiniBlog configured according to user input. 

To watch this demo in action see the [//build/ 2014 session's recording on Channel 9](http://channel9.msdn.com/Events/Build/2014/3-621). 

## Setup ##
This site's usage of MAML demonstrates the X509 Certificate method of authenticating (also known as Management Certificates). In order to run this sample you'll need a certificate file pair:

1. A .CER file that you'll upload to Microsoft Azure in the Management Certificates panel in the Management Portal
2. A password-protected .PFX file that you'll put into the site's folder tree. 

Once you've created this certificate pair and placed both files in their appropriate locations (portal and web site path), configure the site using the following settings in the project's **Web.config** file:

	<add key="AZURE-SUBSCRIPTION-ID" value="YOUR AZURE SUBSCRIPTION ID" />
    <add key="CERTIFICATE-PATH" value="~/App_Data/SaaSDemo.pfx" />
    <add key="CERTIFICATE-PASSWORD" value="Passw0rd!" />

**Note:** use caution when using CER/PFX pairs. If someone gets your subscription ID and PFX file, they can administer your Azure subscription. This demo is simply a demonstration of this technique, and is meant for demonstration purposes only. 