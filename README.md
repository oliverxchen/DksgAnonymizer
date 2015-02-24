# DksgAnonymizer
Proof of concept for an app to anonymize personal/confidential fields in a CSV file.

## Description
This is a bare bones PoC for a light, deployable app that DKSG partner organizations can install themselves. The app can take in CSV files and anonymize the user indicated columns, linking columns as necessary. Only the anonymized files need to be sent to Data Ambassadors. The map linking the original values to the anonymized values is kept by the partner organization, so that they can later link the original fields to the analysis (the app needs to be extended to link back). If the organization is not able to run this themselves, a Data Ambassador can perform this under supervision.

This is currently written in C#/Mono, but it probably makes sense to port this to Python.

## What this PoC does
1. Clicking on the "Generate Test Input Files" generates two test input files. Both files have an email column and other columns which are just random characters. 
2. Clicking on the "Anonymize" button will:
  1. Enumerate the emails in common between the two input files, meaning that the same email in either file will have the same number.
  2. The enumeration is randomized. The randomization uses a Knuth Shuffle (http://en.wikipedia.org/wiki/Random_permutation) with the random integers generated using a Linear Congruential Generator (http://en.wikipedia.org/wiki/Linear_congruential_generator). These are implemented in order that the randomization is cross-platform reproducible.
  3. The rows are then sorted on the random enumeration by batches of 1000, to destroy any ordering there might have been in the original file. This is to avoid, for example, the case where the field to be anonymized is an NRIC number which starts with the person's birth year and the file is sorted by NRIC. Knowing the location of the row in the file might narrow down the possible individuals.
  4. The output of the program is the two modified input files with randomized enumerated email addresses, and the map between the original email addresses and the enumeration.

## Extensions
It would be possible to generalize this to multiple linked fields, multiple files, etc. Ages/birth dates could be rounded, postal codes approximated, etc. Anonymizing within a field would also be possible, but more difficult. For example, anonymizing names within free text would require a list of names that needed to be anonymized. But it would be extremely rare that this would be needed.

## Extremely brief guide to the code
The Anonymizer.cs file is the main class.
