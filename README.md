# CodingChallegeTKB
Code challenge TKB
The code challenge is as follows. Given the example table, the xml schema and the example files, create a tool that synchronizes the Debtors table contents to match the contents of a file that validates against the schema. A unique debtor can be recognized by the Number field.
Must have
•	Visual Studio 2017 must be able to open, build and run the project
•	Accept a file as an input parameter and update the Debtors table
o	New debtors should be added to the table
o	Changed debtors should be updated in the table
o	Debtors not in the file should be marked as Closed in the table
•	Provide feedback on the total number of debtors inserted, changed and closed after synchronization
Should have
•	Make sure unchanged debtors are not updated at all
•	Provide feedback on the total number of debtors inserted, changed and closed before synchronization
Could have
•	Create a graphical user interface that provides all functionality and also displays the current table contents
•	Provide feedback on the details of inserted, changed and closed debtors before synchronization
•	Be able to exclude values from synchronization
•	Be able to generate a new example file based on the current table data
