# Uintra - A social framework
A flexible and lightweight Umbraco based framework, for making an Intranet, Extranet or social platform based on known conventions.

[![ScreenShot](Img/vimeo.png)](https://player.vimeo.com/video/263109862)

## Why Open Source?
We are trying to create a flexible and robust social framework. We think the best way to achieve this is through an online community, that can share and iterate through ideas. In order to create a solution that has valuable functionality validated by the users.

## Getting started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

## 1. Prerequisites
Before installing Uintra you will have to make certain preparations.

1. Install MS NET Framework .NET 4.7.1, can be found [here](https://www.microsoft.com/en-us/download/details.aspx?id=56115).

2. Install IIS with advanced settings (as seen in the screenshot below)
![ScreenShot](Img/IIS_settings.png)

3. MSSQLServer with Management Studio.
   Remember that the version of MSSQL server depends on your Windows version - 32-bit or 64-bit: 
    - for latest version MSSQL server you need Windows 64-bit. 
    - if you have Windows 32-bit we recommend you to install MSSQL server 2014. Here you can find all files to download and     
      install (Express 32BIT WoW64\SQLEXPR32_x86 _ENU and MgmtStudio 32BIT\SQLManagementStudio_x86_ENU).

4. Visual Studio 2017 with update 15.8.8 or later, can be found [here](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=15). 
While installing Visual Studio 2017 you need to choose “ASP.NET and web development” on the Workloads tab: ![ScreenShot](Img/installation/1.png)

   and fill in checkboxes “.NET Framework 4.7 SDK”, “.NET Framework 4.7  targeting pack” on the Individual components tab:        
   ![ScreenShot](Img/installation/2.png)

That is it for the prerequisites now we need to setup the server.

## 2. Setting up the database server

Using MSSQL Server Management Studio(in administrator mode) we will set up the database(DB) server:
1.	Open your DB server                           
   ![ScreenShot](Img/installation/3.png)
   
      And add a new user for the login to the DB: ![ScreenShot](Img/installation/4.png)
   
      Set up the server roles for the user: ![ScreenShot](Img/installation/5.png)

2. Create an empty DB                                       
   ![ScreenShot](Img/installation/6.png)
   
   Give it an appropriate name: ![ScreenShot](Img/installation/7.png)

3.	Add security options for the server authentication, open properties ![ScreenShot](Img/installation/8.png)

      Then go into "Security" and check the box as shown below: ![ScreenShot](Img/installation/9.png)

4.	Restart your server                                      
   ![ScreenShot](Img/installation/10.png)

5.	Connect with the user you created in step 1 ![ScreenShot](Img/installation/11.png)

Now your server is set up and ready for the Uintra installation!

## 3. Installing Uintra

Installing Uintra
Open Visual Studio 2017 that you installed earlier
1.	Create a new project ![ScreenShot](Img/installation/12.png)

      Choose “WEB” -> “ASP.NET Web Application(.NET Framework)”. Then choose “NET Framework 4.7.1” and add “Name”, “Location” 
      (data 
      storage on local machine), “Solution name”: ![ScreenShot](Img/installation/13.png)

      In the next step choose the “Empty” template and press "OK": ![ScreenShot](Img/installation/14.png)

2.	Right click on the new project in the "Solution explorer" block then click on "Manage NuGet packages" ![ScreenShot](Img/installation/15.png)

3.	Press "Browse and search for "Uintra" ![ScreenShot](Img/installation/16.png)

      Install the latest package version: ![ScreenShot](Img/installation/17.png)

4.	Press "OK", "Accept" and “yes to all” in the next popups accept proposals ![ScreenShot](Img/installation/18.png) ![ScreenShot](Img/installation/19.png) ![ScreenShot](Img/installation/20.png)

5.	You should receive a message about 0 errors/warnings ![ScreenShot](Img/installation/21.png)

6.	Right click on the new project in the “Solution explorer” block and press “Build” ![ScreenShot](Img/installation/22.png)

7.	You should receive a message about a successful installation ![ScreenShot](Img/installation/23.png)

With that Uintra is installed, the next and final step is setting up Uintra in IIS and continue the browser configuration.

## 4. Setting up Uintra

Using the Internet Information Services (IIS) Manager, we are going to add the new website.
1.	Adding the website                                                 
   ![ScreenShot](Img/installation/24.png)

      Fill in the required fields and create the website: ![ScreenShot](Img/installation/25.png)
      
      NB! The “Physical path” address should be the folder location for the Uintra project you made in Visual Studio 2017(data 
      storage on local machine).
      In the "Applications Pools" set .NET Framework version 4.0.30319 for your new site.

2.	Add your new site IP-address and name to the “Hosts” file in the windows folder on your computer.

3.	Open your new site in the browser. You’ll be redirected to the “Install Umbraco” page. Fill in all the fields and choose "Customize" ![ScreenShot](Img/installation/26.png)

      In the “Database type” dropdown choose Microsoft SQL Server: ![ScreenShot](Img/installation/27.png)

      Fill in all the fields. Ensure that the "Server name" and "Database name" is identical with the labels in the MSSQL Server 
      database. Use the credentials for the new user that was created earlier and press "Continue": ![ScreenShot] 
      (Img/installation/28.png)

      Press "Continue": ![ScreenShot](Img/installation/29.png)

      Press the "No thanks, I do not want ...": ![ScreenShot](Img/installation/30.png)

      Wait while Umbraco is installed and when it is done you should see the welcoming message: ![ScreenShot](Img/installation
      /31.png)

Now Umbraco is installed.

4.	To administrate your new site, open it in a new tab ![ScreenShot](Img/installation/32.png)

      The first login uses the following data:
      Login - admin, password - qwerty1234
      (P.S. We strongly recommend to change this later)

That's all! Now you can open new site based on Uintra and enjoy!

If you are having trouble getting Uintra installed or experience other problems, please contact us at kne@Compent.net, the response time will be very fast within normal business hours, 09-16 CET/CEST.

## Running tests
We have created some simple examples of how to manually test Uintra [here](Testing.md).

## Contributing to Uintra?
A good way to contribute to Uintra is by providing issue reports. For information on how to submit an issue report refer to our [guide to contributing](CONTRIBUTING.md) for details on our code of conduct, and information regarding the process for submitting pull requests to us.

## Contact
We have several managers and a full team working on Uintra, if you have any questions please send them to our contact person, Kasper at kne@compent.net. 

## License
This project is licensed under the [MIT License](LICENSE.md) - see the file for details.
