# Binoculars #
{literally watching your every move*}   *not really, just when your graph runs. 🔍

### What ###

A user friendly tracker for Dynamo, that reports key stats about Dynamo Use to Google sheets, which can be visualised using a report template in Google Data Store with an option to embed the report in your personal/company website.

### How it works ###


### Why ###



### Contribute ###

Binoculars is a community project arising out of the UKDUG Hackathon April 2019. Feel free to make suggestions, track and submit bugs. 

### How we got here ### 

Building on the python code presented by Olly Green of AHMM at UKDUG meeting on 19th of February 2012.
Further developed by Wayne Patrick Dalton and Brendan Cassidy.

The initial version of this extension was developed by Wayne Patrick Dalton, Laurence Elsdon, Deyan Nenov, Caoimhe Loftus, over a period of two days during the UKDUG Hackathon April 2019.

Building on the extensions workshop by Radu Gidei of Enstoa, we decided that an extension was the best way to deploy our tracker. 
Rather than relying on the user to add a custom/zero touch node to their script - or not to delete it! - and extension works triggers when any dynamo graph is run. This extension can be installed via the local IT team without any depending on user input.

The tracker appears as a drop down on the menu bar, with information about the information that is being collected, and access to the report produced from the data collected.

Obviously any data monitoring has GDPR (or similar!) implications so we built in a popup that alerts the use. 


Extension vs. Zero touch node 
Interface... GDPR
Collecting the data
location - ip address, covert
exporting to Google sheets - free to use - authorising... Access to Google sheets - give people with a link access
Associating the action with an event
Visualising can be done in Google sheets, slicker in Google data store
connect Google sheets (live linked, can be refreshed at regular intervals)
User groups
Filtering information
Give people with a link access
Creating a template vs embedding in exe or company website

How we made our sample data
Dynamo - python nodes 
____
Dynamo already use google Analytics
