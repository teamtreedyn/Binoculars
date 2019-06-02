# Getting Started with Binoculars

This Getting Started Guide will walk you through the steps to download, install, and configure Binoculars for your version of Dynamo.

To keep it simple we're assuming you want to use Google Sheets to store the log data recorded from Binoculars and then visualise it within Google Data Studio.

1. [Download Binoculars](#download-binoculars)
2. [Create a Google Cloud Platform Service Account](#create-a-google-cloud-platform-service-account)
3. [Create a Google Sheet](#create-a-google-sheet)
4. [Enable the Google Sheets API](#enable-the-google-sheets-api)
5. [Update Binoculars settings.json](#update-binoculars-settings-json)
6. [Test Binoculars](#test-binoculars)
7. [Setup Google Data Studio](#setup-google-data-studio)
8. [Conclusion](#conclusion)

## Download Binoculars

Download the latest release of Binoculars from [releases](https://github.com/teamtreedyn/Binoculars/releases).

Extract the `Binoculars` folder and copy it to the packages folder for the version of Dynamo or Dynamo Revit you use.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/14-package-settings.png)

If you just use Dynamo Sandbox or Dynamo for FormIt this could be either:
```
%appdata%\Dynamo\Dynamo Core\2.0\packages\
%appdata%\Dynamo\Dynamo Core\2.1\packages\
```

And if you run Dynamo from Revit try:
```
%appdata%\Dynamo\Dynamo Revit\2.0\packages\
%appdata%\Dynamo\Dynamo Revit\2.1\packages\
```

If you use Dynamo in multiple contexts then copy the package to each directory.

## Create a Google Cloud Platform Service Account

First we need to set up a Google Cloud Platform Service Account to authenticate access to Google Sheets.

Open the following link:
[http://console.cloud.google.com/iam-admin/serviceaccounts/create](http://console.cloud.google.com/iam-admin/serviceaccounts/create)

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/01-cloud-terms.png)

Agree to the terms and conditions.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/02-cloud-create-project.png)

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/03-cloud-new-project.png)

Create a project. You can call it anything you like.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/04-cloud-create-service-account.png)

Now, create a Service Account.

If you've ended up on a different page then just follow the link again:
[http://console.cloud.google.com/iam-admin/serviceaccounts/create](http://console.cloud.google.com/iam-admin/serviceaccounts/create)

You can call it anything you like but keep it simple.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/05-cloud-service-account-permissions.png)

Now, you'll be asked to assign permissions to the Service Account. We don't want to add any so just press continue.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/06-cloud-service-account-grant.png)

Now, we don't want to Grant any users access to this Service Account but we do want to create a key.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/07-cloud-service-account-key.png)

Select JSON for the key type.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/08-cloud-service-account-key-dl.png)

The key will now be downloaded. We don't need it just yet so save it somewhere memorable.

Click done to return to the Service Accounts index on Google Cloud Platform.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/09-cloud-service-account-email.png)

You should now see the Service Account we've just created. Copy the email address to your clipboard.

## Create a Google Sheet

Next we will create the Google Sheet where your data will be stored.

Open Google Drive with the following link.
[https://drive.google.com/drive/](https://drive.google.com/drive/)

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/10-drive-new.png)

Create a new Google Sheet.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/11-sheets-share.png)

Press Share.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/12-sheets-name.png)

Give the document a name.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/13-sheets-email.png)

Now paste the email address for the Google Cloud Platform Service Account and press send.

That's great! Our Service Account can now edit the Google Sheet.

## Enable the Google Sheets API

Now we need to enable the Google Sheets API on your Google Cloud Console Account.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/13b-google-sheets-api.png)

Just open the following link and press enable:
[https://console.developers.google.com/apis/api/sheets.googleapis.com/overview](https://console.developers.google.com/apis/api/sheets.googleapis.com/overview)

## Update Binoculars settings.json

It's now time to edit `settings.json`.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/14-package-settings.png)

Return to the directory you saved the Binoculars package to and open the `settings.json` file within it.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/15-settings-default.png)

I've opened it in Visual Studio Code here but you can just as easily edit it in Notepad or another text editor.

Scroll down to the `export` section.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/16-settings-updated.png)

Copy the Google Sheet `id` from the url of your Google Sheet file as seen in the screenshot above. 

Now add the `sheet` name as seen in the screenshot above. Assuming you haven't changed it, this is probably just `Sheet1`.

Finally, copy and paste the values from the Service Account credentials `.json` file we downloaded earlier into the `googleSheetsServiceAccount` section.

The file should now look similar to this image above. I've blacked out some details which should be kept secret.

Save the `settings.json` file.

Be sure to copy your `settings.json` changes to all the other contexts you use Dynamo as shown in the first step.

## Test Binoculars

Binoculars should now be configured so open up Dynamo. You should now see an icon for Binoculars on the menu bar.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/17-binoculars-sheets.png)

You can open the Google Sheet from Dynamo by pressing the Binoculars icon in the menu bar and choosing Google Sheets. Do that now and then run a script or graph in Dynamo and check that the data is correctly logged to the Google Sheet.

## Setup Google Data Studio

Open the Google Sheet one more time.

Now we need to add a header row to the data.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/18-sheets-header.png)

Insert a new row with the following values

```
Username	Computer	IP	Geolocation	City	Country	Dynamo Version	Revit Version	Graph Name	Date	GUID					
```

Open the example Google Data Studio Data Source with the following link:
https://datastudio.google.com/open/1f31CffnWADHxDqb_ofQmYl8ZDO6hpkSb

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/20-data-source-edit.png)

Click the Make a Copy icon in the top right hand corner. Then click Edit Connection on the left.

Authorise Data Studio to Access your Google Sheets.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/21-data-source-connection.png)

Select the Spreadsheet we created earlier and then press Reconnect in the top right hand corner.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/22-data-source-connection-changed.png)

*Please note: The template hasn't yet been configured to perfectly match the current data Binoculars is reporting. My test has shown that linking the two works but you may see a similar warning as shown above. For now just click Apply, cross your fingers and hope for the best. That worked for me.*

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/23-data-source-complete.png)

Great, the data should now be connected. You can press Explore to check Data Studio is correctly reading from the Google Sheet but lets skip that for the moment and create the Report.

Open the example Google Data Studio Report with the following link:
https://datastudio.google.com/u/0/reporting/14WNMimtt4muq1A_b4Pk8Iy7HiXPkGeKY

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/25-data-report-source.png)

Click Make a Copy in the top right hand corner and make sure to select the Data Source we just setup - not the existing "Binoculars Dashboard - Public".

That's it! We now have Binoculars linked to Google Data Studio.

The charts and visuals might look a bit bare at the moment because we haven't got much data to read. 

Either run a few different scripts in Dynamo to generate some data or copy and paste some sample data into your Google Sheet.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/getting-started/0.5/26-data-report-date.png)

Be sure to change the Date Range on the Google Data Studio Report to ensure you're correctly filtering the data.

One final step. Copy the id of the Google Data Studio Report in to `settings.json` just as we did for the Google Sheet. This will enable the ability to open Data Studio from inside Dynamo!

# Conclusion

We hope you found this Getting Started Guide easy to follow. If it didn't work for you then please get in touch and we'll try to help you out and improve our documentation for the future!

If you continue to use Google Data Studio then please share any interesting stats and visuals you're able to extract. There's a wealth of data to be explored through Binoculars and our template barely touches the surface.
