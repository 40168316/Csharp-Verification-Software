using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace SEM2
{
    public partial class Form1 : Form
    {
        // Global variables
        string whatcourse; // Identifies the coursetype - ie postgrad or undergrad
        string firstword; //
        bool universityfound = false;
        bool coursetypefound = false;
        bool subjectfound = false;
        bool messagebad = false;
        bool universtiesadded = false;
        bool subjectsadded = false;
        List<string> messagesplit = new List<string>();
        List<string> textwordsab = new List<string>();
        List<string> universitymessage = new List<string>();
        List<string> validmessage = new List<string>();
        List<string> coursetype = new List<string>();
        List<string> subject = new List<string>();
        List<string> foundsubjects = new List<string>();
        List<string> founduniversities = new List<string>();
        List<message> Message = new List<message>();

        public Form1()
        {
            InitializeComponent();
        }

        // When the submit button is pressed
        private void submitbtn_Click(object sender, EventArgs e)
        {
            // Reset all the booleans
            messagebad = false;
            universityfound = false;
            coursetypefound = false;
            subjectfound = false;
            messagebad = false;
            int uncoursecounter = 0;
            int pocoursecounter = 0;

            // If no message has been entered then produce an error
            if (string.IsNullOrWhiteSpace(messagetxt.Text))
            {
                MessageBox.Show("Please enter a message to be searched!");
            }
            else
            {
                //Empty lists
                foundsubjects.Clear();
                founduniversities.Clear();
                
                // Add the courses into the coursetype list
                coursetype.Add("Undergraduate");
                coursetype.Add("U/G");
                coursetype.Add("UG");
                coursetype.Add("Postgraduate");
                coursetype.Add("P/G");
                coursetype.Add("PG");

                // Counts the number of words enter into the message textbox
                int wordcount = messagetxt.Text.Split(' ').Count();
                // Make the content of the message box equal to a string
                string messagefrommessagebox = messagetxt.Text;
                // Use string split to split the message up into individual words
                string[] words = messagefrommessagebox.Split(' ');
                // Create a loop to then add them individual words to a list
                for (int i = 0; i < wordcount; i++)
                {
                    messagesplit.Add(words[i]);
                }

                // Create a string called textwordsfilename which store the file name of the textwords file
                string textwordsfilename = "..\\..\\textwordslist.csv";
                // Set textwordsline, which will read the lines of the textwords file, to default/nothing
                string textwordsline = "";

                // If the file name exists/is true
                if (System.IO.File.Exists(textwordsfilename) == true)
                {
                    // Create a stream reader
                    System.IO.StreamReader ReaderTextwords;
                    ReaderTextwords = new System.IO.StreamReader(textwordsfilename);
                    // Create a do while loop
                    do
                    {
                        // Read in a line from the text file one by one
                        textwordsline = ReaderTextwords.ReadLine();
                        // Then take the first word of each line/the abreviation by using string split
                        firstword = textwordsline.Split(',').First();
                        // Add these first words to a listbox
                        textwordsab.Add(firstword);
                        // Loop while there is still a line to be red
                    } while (ReaderTextwords.Peek() != -1);
                }
                // If the file does not exist then produce an error
                else
                {
                    // No such file exists so produce an error message
                    MessageBox.Show("The file does not exist - " + textwordsfilename);
                }

                // For loop looping through all the words entered
                for (int i = 0; i < wordcount; i++)
                {
                    // For each input in the textwordsab
                    foreach (var item in textwordsab)
                    {
                        // If item in textwordsab is equal to words then
                        if (item.ToString() == words[i])
                        {
                            // Match found
                            MessageBox.Show("Unacceptable content found in message. The message enetered will be added to the quarantine file.");
                            // Set the messagebad variable to true
                            messagebad = true;
                        }
                    }
                }

                // Create valid file if it does not exist
                string validpath = @"..\\..\\valid.csv";
                // If the path does not exist then
                if (!File.Exists(validpath))
                {
                    // Create the file
                    File.Create(validpath).Dispose();
                }
                // Create quarantine file if it does not exist
                string quarantinepath = @"..\\..\\quarantine.csv";
                // If the path does not exist then
                if (!File.Exists(quarantinepath))
                {
                    // Create the file
                    File.Create(quarantinepath).Dispose();
                }

                // If the bolean messagebad is false then
                if (messagebad == false)
                {
                    // Use textwriter to ammend the file we have create
                    using (TextWriter tw = File.AppendText(validpath))
                    {
                        // Write the message to the file then close it
                        tw.WriteLine(messagefrommessagebox);
                        tw.Close();
                    }
                }
                // If messagebad is not false (true)
                else
                {
                    // Use textwriter to ammend the file we have create
                    using (TextWriter tw = File.AppendText(quarantinepath))
                    {
                        // Write the message to the file then close it
                        tw.WriteLine(messagefrommessagebox);
                        tw.Close();
                    }
                    messagetxt.Text = string.Empty;
                    // Exit the if statement and reset
                    return;
                }
                
                // Set validmessageline, which will read the lines of the validmessage file, to default/nothing
                string validmessageline= "";
                // Create a stream reader
                System.IO.StreamReader ReaderValid;
                ReaderValid = new System.IO.StreamReader(validpath);
                // Read in the last line from the valid file
                validmessageline = File.ReadAllLines(validpath).Last();
                // Close the file
                ReaderValid.Close();

                // If the Universities have not been added from the file
                if (universtiesadded == false)
                {
                    // Create path name for the list of universities
                    string universityname = @"..\\..\\universitylist.csv";
                    // Set universitymessageline, which will read the lines of the universitymessage file, to default/nothing
                    string universitymessageline = "";
                    // If the file name exists/is true
                    if (System.IO.File.Exists(universityname) == true)
                    {
                        // Create a stream reader
                        System.IO.StreamReader ReaderUniversity;
                        ReaderUniversity = new System.IO.StreamReader(universityname);
                        // Create a do while loop
                        do
                        {
                            // Read in a line from the text file one by one 
                            universitymessageline = ReaderUniversity.ReadLine();
                            // Add line into the universitymessage
                            universitymessage.Add(universitymessageline);
                            // Loop while there is still a line to be red
                        } while (ReaderUniversity.Peek() != -1);
                        // Close the university list file
                        ReaderUniversity.Close();
                    }
                    else
                    {
                        // Else then no such file exists so produce an error message
                        MessageBox.Show("The file does not exist - " + universityname);
                    }
                    universtiesadded = true;
                }

                // If the Subjects have not been added from the file
                if (subjectsadded == false)
                {
                    // Create a string with a file path for subjectnames
                    string subjectnames = @"..\\..\\subjectslist.csv";
                    // Set universitymessageline, which will read the lines of the subject file, to default/nothing
                    string subjectline = "";
                    // If the file name exists/is true
                    if (System.IO.File.Exists(subjectnames) == true)
                    {
                        // Create a stream reader
                        System.IO.StreamReader ReaderUniversity;
                        ReaderUniversity = new System.IO.StreamReader(subjectnames);
                        // Create a do while loop
                        do
                        {
                            // Read in a line from the text file one by one then adding return and new line at the end
                            subjectline = ReaderUniversity.ReadLine();
                            // Add the line to the subject list
                            subject.Add(subjectline);
                            // Loop while there is still a line to be red
                        } while (ReaderUniversity.Peek() != -1);
                        // Close the university list file
                        ReaderUniversity.Close();
                    }
                    else
                    {
                        // Else then no such file exists so produce an error message
                        MessageBox.Show("The file does not exist - " + subject);
                    }
                    subjectsadded = true;
                }

                // To get the university we do a foreach to loop through all the universities in the university list
                foreach (string uni in universitymessage)
                {
                    // Being incasesensitive, if the entery of validmessage contains a uni
                    if (validmessageline.IndexOf(uni, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Make the uni known and add it to the list of Universties found
                        founduniversities.Add(uni);
                        // Set boolean to true
                        universityfound = true;
                    }
                }

                //To get the coursetype we do a foreach to loop through all the courses in the coursetype list
                foreach (string course in coursetype)
                {
                    // Being incasesensitive, if the entery of validmessage contains a course
                    if (validmessageline.IndexOf(course, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Make the course known and assign it to a variable
                        whatcourse = course;
                        // Set boolean to true
                        coursetypefound = true;
                        // Group the undergraduate names and make them equal one term
                        if (whatcourse == "Undergraduate" || whatcourse == "U/G" || whatcourse == "UG")
                        {
                            whatcourse = "Undergraduate";
                            // Add one to the counter
                            uncoursecounter += 1;
                        }
                        // Group the posygraduate names and make them equal one term
                        if (whatcourse == "Postgraduate" || whatcourse == "P/G" || whatcourse == "PG")
                        {
                            whatcourse = "Postgraduate";
                            // Add one to the counter
                            pocoursecounter += 1;
                        }
                    }
                }

                // If a Undergraduate and Postgraduate coursetype was foudn within the same message then notify the user and select the last one entered
                if (uncoursecounter > 0 && pocoursecounter > 0)
                {
                    MessageBox.Show("To make you aware, more than 1 coursetype has been found. We have made the assumption that the last coursetype entered (Undergraduate or Postgraduate) has been made the coursetype.");
                }

                //To get the subject we do a foreach to loop through all the sujects in the subject list
                foreach (string sub in subject)
                {
                    // Being incasesensitive, if the entery of validmessage contains a subject
                    if (validmessageline.IndexOf(sub, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // As there can be more than one subject found these are then added to a list
                        foundsubjects.Add(sub);
                        // Set boolean to true
                        subjectfound = true;
                    }
                }

                // Count how many subjects have been found
                int numberofsubjects = foundsubjects.Count;
                // Merge all the subjects together to make one variable called foundsubject
                string subjectsjoin = string.Join(" - ", foundsubjects);
                string universitiesjoin = string.Join(" - ", founduniversities);

                // Add the message and the found information into the message class
                Message.Add(new message { Universities = universitiesjoin, Coursetype = whatcourse, Subjects = subjectsjoin, Originalmessage = validmessageline });

                // Custom messages are displayed showing if universties, coursetype and subjects have been found or not. If they have been found they will be displayed, if not then a part of the message will show that nothing was found.
                // If the number of subjects is greater than one
                if (universityfound == true && coursetypefound == true && subjectfound == true)
                {
                    if (numberofsubjects > 1)
                    {
                        MessageBox.Show("The university(s) found is " + universitiesjoin + " on a " + whatcourse + " applying for the subjects: " + subjectsjoin);
                    }
                    // If number of subjects is 1
                    else
                    {
                        MessageBox.Show("The university(s) found is " + universitiesjoin + " on a " + whatcourse + " applying for the subject(s) " + subjectsjoin);
                    }
                }
                else if (universityfound == true && coursetypefound == true && subjectfound == false)
                {
                    MessageBox.Show("The university(s) found is " + universitiesjoin + " on a " + whatcourse + " however no subject were found");
                }
                else if (universityfound == true && coursetypefound == false && subjectfound == true)
                {
                    MessageBox.Show("The university(s) found is " + universitiesjoin + " however no coursetype was found, applying for the subject(s) " + subjectsjoin);
                }
                else if (universityfound == false && coursetypefound == true && subjectfound == true)
                {
                    MessageBox.Show("No university(s) was found, however the coursetype is " + whatcourse + " applying for the subject(s) " + subjectsjoin);
                }
                else if (universityfound == true && coursetypefound == false && subjectfound == false)
                {
                    MessageBox.Show("The university(s) found is " + universitiesjoin + " however no coursetype or subjects were found");
                }
                else if (universityfound == false && coursetypefound == false && subjectfound == true)
                {
                    MessageBox.Show("No university(s) or coursetype was found however these subject(s) were found - " + subjectsjoin);
                }
                else if (universityfound == false && coursetypefound == true && subjectfound == false)
                {
                    MessageBox.Show("No university(s) or subject(s) were found however " + whatcourse + " course type was found");
                }

                // Clear variables
                subjectsjoin = "";
                universitiesjoin = "";
                // Empting the input box
                messagetxt.Text = string.Empty;

                // Create a new Serializer for Java Script to convert the message into Json
                var json = new JavaScriptSerializer();
                // Take the last message entered into the message class and convert to Json
                var jsonmessage = json.Serialize(Message.Last());
                // Display the Json content in a messagebox
                MessageBox.Show(jsonmessage);

                // Create json file if it does not exist - this will store all the 
                string jsonpath = @"..\\..\\json.csv";
                // If the path does not exist then
                if (!File.Exists(jsonpath))
                {
                    // Create the file
                    File.Create(jsonpath).Dispose();
                }

                // Use textwriter to ammend the file we have create
                using (TextWriter tw = File.AppendText(jsonpath))
                {
                    // Write the message to the file then close it
                    tw.WriteLine(jsonmessage);
                    tw.Close();
                }
            }
        }

        private void messagetxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}