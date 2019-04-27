![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/Slide1.PNG)
________________

### What ###

Binoculars is a data tracker for Dynamo, it reports key stats about Dynamo Use to Google Sheets, which can be visualised using Google Data Studio. The data collected can be used to monitor Dynamo use eg. identify use of outdated Dynamo versions, quantify time saved.


![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/Slide6.PNG)


### How to use it ###

Download the github repository, use visual studio (or similar) to add to a new google sheets file ID. On build it will be placed in the correct location for ongoing Dynamo use. This build works with Dynamo Sandbox, but can be edited to work with Dynamo for Revit.

Once installed Binoculars will activate when you run a Dynamo graph.

Further work is required to facilitate wider implementation to overcome google authentication, and legal requirements.

*Visualising the data* we've created a template in Google Data Studio that can be connected to your Google Sheet to show your results.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/Copy_of_ET_Dashboard-1.png)

### Quickstart ###

1. Clone/download the repository.
2. Make sure all References are set correctly.
3. Follow this [link](https://developers.google.com/sheets/api/quickstart/dotnet) to enable Google API and generate credentials.json file.
4. Download credentials.json into the root of your project repo. (While testing, we failed collecting the file using IE. Try using Chrome if you experience a similar issue).
5. Create a new Google Sheets document; **Share** it (top-right corner) so that **everyone with the link** can **edit** the document.
6. Make note of the Sheet name and the Worksheet name:
<img src="https://github.com/teamtreedyn/Binoculars/blob/master/Images/walkthrough_1.png" alt="wlk_1" height="250"/>

7. In Data.cs replace String spreadsheetId and String spreadsheetTab with the values from your own Google Sheet. (Line 128)
8. Compile, run Dynamo, run a Graph. You should see a new entry appear in your Google Sheet.
9. Tap yourself on the shoulder.

### Disclaimer ###

Creating and distributing credentials.json file will expose the Google account associated with it in some ways. Please, investigate the concept before commiting further. Using a neutral Google account might be the best option if you don't understand the repercussions of using Google API fully.

### Why ###

Collecting data about your dynamo script use can help in so many ways, we've identified three catagories of users but there are probably many more
- *Executive Level* understanding the benefits/return on investment of dynamo use
- *Diagnostics* evaluate where outdated versions of Dynamo or graphs are use
- *Users* see which scripts are used the most, identify champion users to go to for help

### Please contribute ###

Binoculars is a community project arising out of the UKDUG Hackathon April 2019. Feel free to make suggestions through pull requests, track and submit bugs through issues.
_____________

### How we got here ###

Building on the python code presented by Olly Green of AHMM at UKDUG meeting on 19th of February 2012.
Further developed by Wayne Patrick Dalton and Brendan Cassidy.

The initial version of this extension was developed by Wayne Patrick Dalton, Laurence Elsdon, Deyan Nenov, Caoimhe Loftus, over a period of two days during the UKDUG Hackathon April 2019.

Building on the extensions workshop by Radu Gidei of Enstoa, we decided that an extension was the best way to deploy Binoculars.
Rather than relying on the graph user to add a custom/zero touch node to their script - or not to delete it! - the extension triggers when any Dynamo graph is run*. This extension can be installed via the local IT team without any depending on user input.

Binoculars appears as a drop down on the menu bar, with information about the information that is being collected, and access to the report produced from the data collected.

![](https://github.com/teamtreedyn/Binoculars/blob/master/Images/Slide14.PNG)

Obviously any data monitoring has GDPR (or similar!) implications so we built in a popup that makes the user aware that their data is being tracked.

The code can be broken down into 3 main pieces

- UI elements including privacy screen on startup
- Collecting the data, done on 'evaluation complete'
- Publishing the data collected using Post Request to Google Sheets

________________

### Creating the sample data set ###

As a side exercise to help us visualise the data we were expecting to get from Binoculars we wrote a dynamo graph to produce a sample data set.

Collecting a sample set of information from the internet, we used python nodes to generate and randomise a series of outputs.

__________________

*We propose that in future version of Binoculars the GDPR notifications can be edited to appear at designated time intervals instead of at every use
