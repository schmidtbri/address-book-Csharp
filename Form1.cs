using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;

namespace Address_Book
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button2.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string homepath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            label7.Text = "Data and pdf files are stored in: " + homepath;
            
            if (File.Exists(homepath + "\\listboxdata.xml"))
            {
                //Opens file "listboxdata.xml" and deserializes the object from it.
                Stream stream = File.Open(homepath + "\\listboxdata.xml", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                MyList list = (MyList)formatter.Deserialize(stream);
                stream.Close();

                // Now use list object to populate the ListBox before dispalying it:
                foreach (object obj in list.array)
                {
                    Person person= (Person)obj;
                     
                    listBox1.Items.Add(person);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                Person item = (Person)listBox1.SelectedItem;
                label6.Text = item.FirstName + " " + item.LastName + "\nEmail: " + item.Email + "\nHome Phone: " + item.HomePhone + "\nCell Phone: " + item.CellPhone;
                button2.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text.Trim();
            textBox2.Text.Trim();
            textBox3.Text.Trim();
            textBox4.Text.Trim();
            textBox5.Text.Trim();

            if( textBox1.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("First name is empty.");
            }
            else if(textBox2.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Last name is empty.");
            }
            else if (textBox4.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Email is empty.");
            }
            else if(textBox5.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Home phone is empty.");
            }
            else if(textBox3.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Cell phone is empty.");
            }
            else if(!IsPhoneNumber(textBox5.Text))
            {
                System.Windows.Forms.MessageBox.Show("Home phone field does not contain a phone number.\nExample: (269)845-2929");
            }
            else if(!IsPhoneNumber(textBox3.Text))
            {
                System.Windows.Forms.MessageBox.Show("Cell phone field does not contain a phone number.\nExample: (269)845-2929");
            }
            else if (!IsEmail(textBox4.Text))
            {
                System.Windows.Forms.MessageBox.Show("Email field does not contain a valid email.\nExample: asdf@kdk.com");
            }
            else
	        {
                var item = new Person { FirstName = textBox1.Text.Trim(), LastName = textBox2.Text.Trim(), Email = textBox4.Text.Trim(), CellPhone = textBox3.Text.Trim(), HomePhone = textBox5.Text.Trim() };

                listBox1.Items.Add(item);
            }        
        }

        private bool IsPhoneNumber(string text)
        {
            Regex regex = new Regex(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$");
            return regex.IsMatch(text);
        }

        private bool IsEmail(string text)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.IsMatch(text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (listBox1.SelectedItems.Count > 0)
            {
                listBox1.Items.Remove(listBox1.SelectedItems[0]);
            }
            listBox1.SelectedItem = null;
            label6.Text = "";
            button2.Visible = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //save the xml file

            //Create a MyList object:
            MyList list = new MyList();

            //copy the data from the list box 
            foreach (object person in listBox1.Items)
            {
                list.array.Add(person);
            }
            
            string homepath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            //Opens a file and serializes the object into it in binary format.
            Stream stream = File.Open(homepath + "\\listboxdata.xml", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, list);
            stream.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ceTe.DynamicPDF.Document document;

             // Create a document and set it's properties
            try
            {
                document = new ceTe.DynamicPDF.Document();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Problem encountered creating the file.");
                return;
            }

             document.Creator = "Address Book";
             document.Author = "Address Book";
             document.Title = "Adress Book";

             // Create a page to add to the document
             ceTe.DynamicPDF.Page page = new ceTe.DynamicPDF.Page( PageSize.Letter, PageOrientation.Portrait, 54.0f );

             int counter = 0;
             foreach (Person person in listBox1.Items)
             {
                 if (counter > 600)
                 {
                     // Add page to document
                     document.Pages.Add(page);

                     page = new ceTe.DynamicPDF.Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);

                     counter = 0;                     
                 }

                 ceTe.DynamicPDF.PageElements.Label label = new ceTe.DynamicPDF.PageElements.Label(person.FirstName + " " + person.LastName, 0, counter + 0, 1000, 200);
                 page.Elements.Add(label);
                 label = new ceTe.DynamicPDF.PageElements.Label("Email: " + person.Email, 0, counter + 20, 1000, 200);
                 page.Elements.Add(label);
                 label = new ceTe.DynamicPDF.PageElements.Label("Home Phone: " + person.HomePhone, 0, counter + 40, 1000, 200);
                 page.Elements.Add(label);
                 label = new ceTe.DynamicPDF.PageElements.Label("Cell Phone: " + person.CellPhone, 0, counter + 60, 1000, 200);
                 page.Elements.Add(label);

                 counter += 100;
                 
                 
             }
             document.Pages.Add(page);

             page = new ceTe.DynamicPDF.Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);

             try
             {
                 string homepath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                 var writer = new BinaryWriter(File.Open(homepath + "\\contacts.pdf", FileMode.OpenOrCreate));
                 // Outputs the document to the current web page
                 writer.Write(document.Draw());
                 writer.Close();
             }
             catch
             {
                 System.Windows.Forms.MessageBox.Show("Problem encountered writing the file.\nIs the file open?");
             }


        }

    }
}
