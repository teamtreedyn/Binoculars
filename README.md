![](tracker.png)

# Binoculars #
if you press run, you can't hide... 🔍
________________

### What ###

Binoculars is a data tracker for Dynamo, it reports key stats about Dynamo Use to Google sheets, and visualised using Google Data Store which can be embeded in your personal/company website and used to observe Dynamo use, diagnose outdated Dynamo versions .

### How it works ###

Download the github repository, use visual studio (or similar) to add to a new google sheets file ID. On build it will be placed in the correct location for ongoing Dynamo use. This build works with Dynamo Sandbox, but can be edited to work with Dynamo for Revit. 

Once installed Binoculars will run when you run a Dynamo graph.

Further work is required to facilitate wider implementation to overcome google authentication, and legal requirements.

Visualising the data - we've created a template in Google Data Studio that can be connected to your Google Sheet to show your results. 

### Why ###

Collecting data about your dynamo script use can help in so many ways, we've identified three catagories of users but there are probably many more
- executive level - understanding the benefits/return on investment of dynamo use
- diagnostics - evaluate where outdated versions of Dynamo or graphs are use
- users - see which scripts are used the most, identify champion users to go to for help

### Please contribute ###

Binoculars is a community project arising out of the UKDUG Hackathon April 2019. Feel free to make suggestions, track and submit bugs. 
_____________

### How we got here ### 

Building on the python code presented by Olly Green of AHMM at UKDUG meeting on 19th of February 2012.
Further developed by Wayne Patrick Dalton and Brendan Cassidy.

The initial version of this extension was developed by Wayne Patrick Dalton, Laurence Elsdon, Deyan Nenov, Caoimhe Loftus, over a period of two days during the UKDUG Hackathon April 2019.

Building on the extensions workshop by Radu Gidei of Enstoa, we decided that an extension was the best way to deploy our tracker. 
Rather than relying on the graph user to add a custom/zero touch node to their script - or not to delete it! - the extension triggers when any Dynamo graph is run. This extension can be installed via the local IT team without any depending on user input.

The tracker appears as a drop down on the menu bar, with information about the information that is being collected, and access to the report produced from the data collected.

Obviously any data monitoring has GDPR (or similar!) implications so we built in a popup that alerts the use. 

The code can be broken down into 3 main pieces 

- We have event triggers UI elements privacy screen - what event?
- Collecting the data, done on evaluation complete 
- Publish data collected using Post request to google sheets



location - ip address, covert
exporting to Google sheets - free to use - authorising... Access to Google sheets - give people with a link access
Associating the action with an event
Visualising can be done in Google sheets, slicker in Google data store
connect Google sheets (live linked, can be refreshed at regular intervals)
User groups
Filtering information
Give people with a link access
Creating a template vs embedding in exe or company website
________________

### Creating the sample data set ###

As a side exercise to help us visualise the data we were expecting to get from the tracker we wrote a dynamo graph to produce a sample data set.

Collecting a sample set of information from the internet, we used python nodes to generate and randomise a series of outputs.
____
Dynamo already use google Analytics
