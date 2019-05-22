using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBMS_Practical_Exam
{
    public partial class Form1 : Form
    {
        SqlConnection connection = null;
        DataSet dataSet = null;
        SqlDataAdapter childAdapter = null;
        SqlDataAdapter parentAdapter = null;
        SqlCommandBuilder commandBuilder = null;
        BindingSource bsParent = null;
        BindingSource bsChild = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            childAdapter.Update(dataSet, "Engine");
        }

        private void AddButton_Click(object sender, EventArgs e) {
            childAdapter.InsertCommand = new SqlCommand("insert into Child() values (@a, @b, @c, @d)", connection);

            childAdapter.InsertCommand.Parameters.Add("@a", SqlDbType.Date).Value = DateTime.Parse(textBox1.Text);
            childAdapter.InsertCommand.Parameters.Add("@a", SqlDbType.VarChar).Value = textBox2.Text;
            childAdapter.InsertCommand.Parameters.Add("@a", SqlDbType.Int).Value = Int32.Parse(textBox3.Text);

            connection.Open();
            childAdapter.InsertCommand.ExecuteNonQuery();
            connection.Close();

            MessageBox.Show("Added");
        }

        private void UpdateButton_Click(object sender, EventArgs e) {
            int selectedRow = Int32.Parse(dataGridChild.SelectedRows[0].Cells[0].Value.ToString());

            childAdapter.UpdateCommand = new SqlCommand("update Child set field1=@a, field2=@b, field3=@c where pid=@d", connection);

            childAdapter.UpdateCommand.Parameters.Add("@a", SqlDbType.Date).Value = DateTime.Parse(textBox1.Text);
            childAdapter.UpdateCommand.Parameters.Add("@b", SqlDbType.VarChar).Value = textBox2.Text;
            childAdapter.UpdateCommand.Parameters.Add("@c", SqlDbType.Int).Value = Int32.Parse(textBox3.Text);

            childAdapter.UpdateCommand.Parameters.Add("@d", SqlDbType.Int).Value = selectedRow;

            connection.Open();
            childAdapter.UpdateCommand.ExecuteNonQuery();
            connection.Close();

            MessageBox.Show("Updated");
        }

        private void RemoveButton_Click(object sender, EventArgs e) {
            int selectedRow = Int32.Parse(dataGridChild.SelectedRows[0].Cells[0].Value.ToString());
            childAdapter.DeleteCommand = new SqlCommand("delete from Child where pid=@pid", connection);

            childAdapter.DeleteCommand.Parameters.Add("@pid", SqlDbType.Int).Value = selectedRow;

            connection.Open();
            childAdapter.DeleteCommand.ExecuteNonQuery();
            connection.Close();

            MessageBox.Show("Removed");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(@"Data Source=DESKTOP-9LEJM0T\SQLEXPRESS; Initial Catalog=SpaceLaunchCompany; Integrated Security=true");
            dataSet = new DataSet();

            childAdapter = new SqlDataAdapter("select * from Engine", connection);
            parentAdapter = new SqlDataAdapter("select * from Stage", connection);
            commandBuilder = new SqlCommandBuilder(childAdapter);

            parentAdapter.Fill(dataSet, "Stage");
            childAdapter.Fill(dataSet, "Engine");

            DataRelation dr = new DataRelation("FK_Stage_Engine", dataSet.Tables["Stage"].Columns["sid"], dataSet.Tables["Engine"].Columns["sid"]);
            dataSet.Relations.Add(dr);

            bsParent = new BindingSource();
            bsChild = new BindingSource();

            bsParent.DataSource = dataSet;
            bsParent.DataMember = "Stage";

            bsChild.DataSource = bsParent;
            bsChild.DataMember = "FK_Stage_Engine";

            dataGridChild.DataSource = bsChild;
            dataGridParent.DataSource = bsParent;
        }
    }
}
