using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int serial_no;
        double v1, v2, res;
        string buffer, operation, update_value_1, update_value_2, update_result;
        string state = @"Data Source=HP-PROBOOK;Initial Catalog=CalculatorDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=true";
        SqlConnection connect;
        bool alert;

        // Constructor
        public Form1()
        {
            serial_no = 0;
            v1 = v2 = res = -1;
            buffer = "";
            operation = "";
            update_value_1 = update_value_2 = update_result = "";
            alert = false;
            connect = new SqlConnection(state);
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Open the connection
                connect.Open();

                // Fetch table names from the database
                DataTable tables = connect.GetSchema("Tables");

                // Close the connection
                connect.Close();

                // Iterate through the table names and add them to the ComboBox
                foreach (DataRow row in tables.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    comboBox1.Items.Add(tableName);
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("NullReferenceException occurred: " + ex.Message);
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                connect.Close();
            }
        }



        // History of Tables
        private void History(object sender, EventArgs e)
        {
            string selectedTableName = comboBox1.SelectedItem.ToString(); // Get the selected table name from the ComboBox

            if (!String.IsNullOrEmpty(selectedTableName) && (selectedTableName == "Square") || (selectedTableName == "Square_Root"))
            {
                connect.Open();
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {selectedTableName}", connect);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                newValue_2TextBox.Text = "Not Available";
                newValue_2TextBox.ReadOnly = true;
                connect.Close();
            }
            

            else if (!string.IsNullOrEmpty(selectedTableName) && (selectedTableName != "Square") || (selectedTableName != "Square_Root"))
            {
                connect.Open();
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {selectedTableName}", connect);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                newValue_2TextBox.Text = "";
                newValue_2TextBox.ReadOnly = false;
                connect.Close();
            }

            selectedTableName = "";
        }



        // Deletion of Table Entries
        private void Delete(object sender, EventArgs e)
        {
            string selectedTableName = comboBox1.SelectedItem.ToString(); // Get the selected table name from the ComboBox
            connect.Open();
            string serial = primary_Key_Delete.Text;
            string DeleteQuery = $"Delete From {selectedTableName}  Where [Serial_No] = {serial} ";
            SqlCommand qry = new SqlCommand(DeleteQuery, connect);

            int rowsAffected = qry.ExecuteNonQuery();

            // Check if any rows were affected
            if (rowsAffected > 0)
            {
                MessageBox.Show("Record updated successfully.");
            }
            else
            {
                MessageBox.Show("No record found with the specified primary key.");
            }

            primary_Key_Delete.Text = "";
            UpdateSerialNumbers(selectedTableName);
            connect.Close();
            // Update_Serial_No(selectedTableName);
        }








        // Update Tables Entities
        private void Update(object sender, EventArgs e)
        {
            string selectedTableName = comboBox1.SelectedItem.ToString(); // Get the selected table name from the ComboBox
            string primaryKeyValue = primaryKeyTextBox.Text; // Get the primary key value entered by the user
            double v1, v2, result;
            bool flag = false;
            

            switch (selectedTableName)
            {
                case "Addition":
                    update_value_1 = newValue_1TextBox.Text;
                    update_value_2 = newValue_2TextBox.Text;
                    double.TryParse(update_value_1, out double a);
                    double.TryParse(update_value_2, out double b);
                    v1 = a;
                    v2 = b;
                    result = v1 + v2;
                    update_result = result.ToString();
                    break;


                case "Subtraction":
                    update_value_1 = newValue_1TextBox.Text;
                    update_value_2 = newValue_2TextBox.Text;
                    double.TryParse(update_value_1, out double c);
                    double.TryParse(update_value_2, out double d);
                    v1 = c;
                    v2 = d;
                    result = v1 - v2;
                    update_result = result.ToString();
                    break;


                case "Multiplication":
                    update_value_1 = newValue_1TextBox.Text;
                    update_value_2 = newValue_2TextBox.Text;
                    double.TryParse(update_value_1, out double k);
                    double.TryParse(update_value_2, out double f);
                    v1 = k;
                    v2 = f;
                    result = v1 * v2;
                    update_result = result.ToString();
                    break;


                case "Division":
                    update_value_1 = newValue_1TextBox.Text;
                    update_value_2 = newValue_2TextBox.Text;
                    double.TryParse(update_value_1, out double g);
                    double.TryParse(update_value_2, out double h);
                    v1 = g;
                    v2 = h;
                    result = v1 / v2;
                    update_result = result.ToString();
                    break;



                case "Square":
                    update_value_1 = newValue_1TextBox.Text;
                    double.TryParse(update_value_1, out double i);
                    v1 = i;
                    result = v1 * v1;
                    update_result = result.ToString();
                    flag = true;
                    break;


                case "Square_Root":
                    update_value_1 = newValue_1TextBox.Text;
                    double.TryParse(update_value_1, out double j);
                    v1 = j;
                    result = Math.Sqrt(v1);
                    update_result = result.ToString();
                    flag = true;
                    break;
            }


            if (!string.IsNullOrEmpty(selectedTableName) && !string.IsNullOrEmpty(primaryKeyValue) && !string.IsNullOrEmpty(update_value_1) && flag == true)
            {
                string updateQuery = $"UPDATE {selectedTableName} SET [Input Value] = '{update_value_1}', Result = '{update_result}'  WHERE Serial_No = {primaryKeyValue}";

                connect.Open();
                SqlCommand command = new SqlCommand(updateQuery, connect);
                int rowsAffected = command.ExecuteNonQuery();
                connect.Close();

                // Check if any rows were affected
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Record updated successfully.");
                }
                else
                {
                    MessageBox.Show("No record found with the specified primary key.");
                }
            }

            else if (!string.IsNullOrEmpty(selectedTableName) && !string.IsNullOrEmpty(primaryKeyValue) && !string.IsNullOrEmpty(update_value_2) && flag == false)
            {
                // Construct the update query
                string updateQuery = $"UPDATE {selectedTableName} SET [Input Value - 1] = '{update_value_1}', [Input Value - 2] = '{update_value_2}', Result = '{update_result}'  WHERE Serial_No = {primaryKeyValue}";


                // Open connection and execute the update query
                connect.Open();
                SqlCommand command = new SqlCommand(updateQuery, connect);
                int rowsAffected = command.ExecuteNonQuery();
                connect.Close();

                // Check if any rows were affected
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Record updated successfully.");
                }
                else
                {
                    MessageBox.Show("No record found with the specified primary key.");
                }
            }
            else
            {
                MessageBox.Show("Please select a table, enter the primary key, and specify the new value.");
            }

            v1 = v2 = result = 0;
            update_value_1 = update_value_2 = update_result = "";
            selectedTableName = "";

            if(flag == true)
            {
                flag = false;
                primaryKeyTextBox.Text = "";
                newValue_1TextBox.Text = "";
                newValue_2TextBox.ReadOnly = false;
                newValue_2TextBox.Text = "";
            }
            else
            {
                primaryKeyTextBox.Text = "";
                newValue_1TextBox.Text = "";
                newValue_2TextBox.Text = "";
            }
        }







        // Single Button Press - either operand or operation
        private void button_Click(object sender, EventArgs e)
        {
            alert = false;

            if (operation == "")
            {
                // Check Operation
                buffer = (sender as Button).Text;
            }         

            if(buffer == "+" || buffer == "-" || buffer == "X" || buffer == "/")
            {
                double.TryParse(currentCalculation, out double a);
                v1 = a;
                currentCalculation += (sender as Button).Text;
                operation = buffer;
                alert = true;
                buffer = "";
            }
            else if (operation != "")
            {
                buffer += (sender as Button).Text;
                currentCalculation += (sender as Button).Text;    
            }
            else
            {
                buffer = "";
                // This adds the number or operator to the string calculation
                currentCalculation += (sender as Button).Text;
            }

            // Display the current calculation back to the user
            textBoxOutput.Text = currentCalculation;
        }



        // Equality and Results
        private void button_Equals_Click(object sender, EventArgs e)
        {
            if (buffer != "")
            {
                double.TryParse(buffer, out double a);
                v2 = a;
            }
            string formattedCalculation = currentCalculation.Replace("X", "*").Replace("&divide;", "/");
            

            try
            {
                var result = new DataTable().Compute(formattedCalculation, null);

                if (result != null)
                {
                    textBoxOutput.Text = result.ToString();
                    currentCalculation = textBoxOutput.Text;
                    textBoxOutput.Text = currentCalculation;
                    double.TryParse(currentCalculation, out double a);
                    res = a;
                    
                    // Function Call to store it in tables.
                    Store();
                }
                else
                {
                    // Handle unexpected result
                    textBoxOutput.Text = "0";
                    currentCalculation = "";
                    MessageBox.Show("An unexpected error occurred. Please check your input.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SyntaxErrorException)
            {
                // Handle syntax error
                textBoxOutput.Text = "0";
                currentCalculation = "";
                MessageBox.Show("Syntax error. Please check your input.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                textBoxOutput.Text = "0";
                currentCalculation = "";
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Storing Data to tables
        private void Store()
        {
            SqlCommand command;
            connect.Open();
            string query = "";
            int check;
            currentCalculation = "";
            serial_no = 0;

            switch (operation)
            {
                case "+":
                    serial_no = AutoSerial_No("+");
                    query = "INSERT INTO ADDITION Values('" +serial_no+ "','" + v1 + "' , '" + v2 + "', '" + res + "' )";
                    textBoxOutput.Text = res.ToString();
                    break;

                case "-":
                    serial_no = AutoSerial_No("-");
                    query = "INSERT INTO SUBTRACTION Values('" + serial_no + "','" + v1 + "' , '" + v2 + "', '" + res + "' )";
                    textBoxOutput.Text = res.ToString();
                    break;

                case "X":
                    serial_no = AutoSerial_No("X");
                    query = "INSERT INTO MULTIPLICATION Values('" + serial_no + "','" + v1 + "' , '" + v2 + "', '" + res + "' )";
                    textBoxOutput.Text = res.ToString();
                    break;

                case "/":
                    serial_no = AutoSerial_No("/");
                    query = "INSERT INTO DIVISION Values('" + serial_no + "','" + v1 + "' , '" + v2 + "', '" + res + "' )";
                    textBoxOutput.Text = res.ToString();
                    break;

                case "A²":
                    serial_no = AutoSerial_No("A²");
                    query = "INSERT INTO SQUARE Values('" + serial_no + "','" + v1 + "' , '" + res + "' )";
                    textBoxOutput.Text = res.ToString();
                    break;

                case "√":
                    serial_no = AutoSerial_No("√");
                    query = "INSERT INTO SQUARE_ROOT Values('" + serial_no + "','" + v1 + "' , '" + res + "' )";
                    textBoxOutput.Text = res.ToString();
                    break;
            }

            operation = "";
            command = new SqlCommand(query, connect);
            check = command.ExecuteNonQuery();

            if (check == 1)
            {
                MessageBox.Show("Inserted Successful");
            }
            else
            {
                MessageBox.Show("Error");
            }

            connect.Close();
            textBoxOutput.Text = "";
        }














        // AUTO Serial No Generator
        public int AutoSerial_No(string a)
        {
            int i = 0;

            if (a == "+")
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(Serial_No) FROM [ADDITION]", connect);
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    i = Convert.ToInt32(result) + 1;
                }
                UpdateSerialNumbers("ADDITION");
                return i;
            }
            else if (a == "-")
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(Serial_No) FROM [SUBTRACTION]", connect);
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    i = Convert.ToInt32(result) + 1;
                }
                UpdateSerialNumbers("SUBTRACTION");
                return i;
            }
            else if (a == "X")
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(Serial_No) FROM [MULTIPLICATION]", connect);
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    i = Convert.ToInt32(result) + 1;
                }
                UpdateSerialNumbers("MULTIPLICATION");
                return i;
            }
            else if (a == "/")
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(Serial_No) FROM [DIVISION]", connect);
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    i = Convert.ToInt32(result) + 1;
                }
                UpdateSerialNumbers("DIVISION");
                return i;
            }
            else if (a == "A²")
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(Serial_No) FROM [SQUARE]", connect);
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    i = Convert.ToInt32(result) + 1;
                }
                UpdateSerialNumbers("SQUARE");
                return i;
            }
            else if (a == "√")
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(Serial_No) FROM [SQUARE_ROOT]", connect);
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    i = Convert.ToInt32(result) + 1;
                }
                UpdateSerialNumbers("SQUARE_ROOT");
                return i;
            }
            return 0;
        }

        private void UpdateSerialNumbers(string tableName)
        {
            SqlCommand cmd = new SqlCommand($"SELECT Serial_No FROM [{tableName}]", connect);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            int count = dt.Rows.Count;
            for (int j = 0; j < count; j++)
            {
                SqlCommand updateCmd = new SqlCommand($"UPDATE [{tableName}] SET Serial_No = {j + 1} WHERE Serial_No = {dt.Rows[j]["Serial_No"]}", connect);
                updateCmd.ExecuteNonQuery();
            }
        }





        // Clear Screen 
        private void button_Clear_Click(object sender, EventArgs e)
        {
            // Reset the calculation and empty the textbox
            textBoxOutput.Text = "0";
            currentCalculation = "";
        }

        






        // Single Entry Removal
        private void button_ClearEntry_Click(object sender, EventArgs e)
        {
            // If the calculation is not empty, remove the last number/operator entered
            if (currentCalculation.Length > 0)
            {
                currentCalculation = currentCalculation.Remove(currentCalculation.Length - 1, 1);
            }
            if (alert == true)
            {
                operation = "";
            }
            if ((operation != ""))
            {
                buffer = buffer.Remove(buffer.Length - 1, 1);
            }

            // Re-display the calculation onto the screen
            textBoxOutput.Text = currentCalculation;
        }
        


        // Square Function
        private void button_square_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentCalculation))
            {
                try
                {
                    int.TryParse(currentCalculation, out int a);
                    v1 = a;
                    double num = double.Parse(currentCalculation);
                    double result = num * num;
                    textBoxOutput.Text = result.ToString();
                    currentCalculation = textBoxOutput.Text;
                    res = result;
                    operation = "A²";
                    Store();
                }
                catch (FormatException)
                {
                    // Handle format exception (e.g., invalid input)
                    textBoxOutput.Text = "0";
                    currentCalculation = "";
                    MessageBox.Show("Invalid input. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    textBoxOutput.Text = "0";
                    currentCalculation = "";
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Handle case when there's no input
                textBoxOutput.Text = "0";
                currentCalculation = "";
                MessageBox.Show("Please enter a number before squaring.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Function to calculate Square Root
        private void button_SquareRoot_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentCalculation))
            {
                try
                {
                    double num = double.Parse(currentCalculation);

                    if (num >= 0)
                    {
                        v1 = num;
                        double result = Math.Sqrt(num);
                        textBoxOutput.Text = result.ToString();
                        currentCalculation = textBoxOutput.Text;
                        res = result;
                        operation = "√";
                        Store();
                    }
                    else
                    {
                        // Handle case when the input is negative
                        textBoxOutput.Text = "0";
                        currentCalculation = "";
                        MessageBox.Show("Cannot calculate square root of a negative number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (FormatException)
                {
                    // Handle format exception (e.g., invalid input)
                    textBoxOutput.Text = "0";
                    currentCalculation = "";
                    MessageBox.Show("Invalid input. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    textBoxOutput.Text = "0";
                    currentCalculation = "";
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Handle case when there's no input
                textBoxOutput.Text = "0";
                currentCalculation = "";
                MessageBox.Show("Please enter a number before calculating square root.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }



    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create an instance of your main form
            Form1 mainForm = new Form1();

            // Run the application, showing the main form
            Application.Run(mainForm);
        }
    }
}
