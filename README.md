#  Telerik UI for ASP.NET Core E-Shop application

The E-Shop app is built with <a href="https://www.telerik.com/aspnet-core-ui" target="_blank">Telerik UI for ASP.NET Core</a> components within ASP.NET Core application. The sample app demonstrates the most common use cases related to online stores – product categories, product lists and details, shopping cart, similar and recently viewed products, user profile, sophisticated product filters, and so on. Besides that, the demo includes integration with <a href="https://www.telerik.com/products/reporting.aspx" target="_blank">Telerik Reporting</a> to showcase how to generate order invoice, print product catalogue, or export favorite products in PDF. The styling is powered by the new built-in <a href="https://docs.telerik.com/aspnet-core/styles-and-layout/sass-themes/overview" target="_blank">Fluent theme</a>, which is available for all Telerik and Kendo UI components. The products data is based on AdventureWorks sample database.

----------

## Featured Telerik UI for ASP.NET Core components

The sample application showcases some of the most popular Telerik UI for ASP.NET Core components, such as:

 - [Data Grid][1]
 - [Scrollview][2]
 - [ListView][3]
 - [AutoComplete][4]
 - [TabStrip][5]
 - [Form][6]
 - [Slider][7]
 - [Rating][8]
 - [CheckBoxGroup][9]
 - [RadioGroup][10]
 - [ButtonGroup][11]
 - [Map][12]
 - [Menu][13]
 - [Captcha][14]

  [1]: https://demos.telerik.com/aspnet-core/grid
  [2]: https://demos.telerik.com/aspnet-core/scrollview
  [3]: https://demos.telerik.com/aspnet-core/listview
  [4]: https://demos.telerik.com/aspnet-core/autocomplete
  [5]: https://demos.telerik.com/aspnet-core/tabstrip
  [6]: https://demos.telerik.com/aspnet-core/form
  [7]: https://demos.telerik.com/aspnet-core/slider
  [8]: https://demos.telerik.com/aspnet-core/rating
  [9]: https://demos.telerik.com/aspnet-core/checkboxgroup
  [10]: https://demos.telerik.com/aspnet-core/radiogroup
  [11]: https://demos.telerik.com/aspnet-core/buttongroup
  [12]: https://demos.telerik.com/aspnet-core/map
  [13]: https://demos.telerik.com/aspnet-core/menu
  [14]: https://demos.telerik.com/aspnet-core/captcha
  
## Prerequisits

 - [.NET 6.0][15]
 - [Visual Studio 2022][16]
 - [Microsoft SQL Server 2019][17]

[15]: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
[16]: https://visualstudio.microsoft.com/downloads/
[17]: https://www.microsoft.com/en-us/sql-server/sql-server-downloads

## Running this app

1. [Add the Telerik Nuget feed as a Package Source](https://docs.telerik.com/aspnet-core/installation/nuget-install).
1. Copy the .bak file from the `DatabaseFiles` folder to your SQL Server backup location.
1. Restore the sample database either through the [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/samples/adventureworks-install-configure?view=sql-server-ver16&tabs=ssms#restore-to-sql-server) or by using the [RESTORE command](https://learn.microsoft.com/en-us/sql/samples/adventureworks-install-configure?view=sql-server-ver16&tabs=tsql#restore-to-sql-server).
1. Open `EShop.sln` with Visual Studio.
1. Open the terminal and enter the `Web` directory (`...\fluent-eshop-core\EShop\Web`).
1. Run `npm install` to install the dependencies from the `package.json` file. This step is required to activate the `gulp tasks` defined in the `gulpfile.js` when running the app. 
1. Run the application (Hit `Ctrl` + `F5`).