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

        public Form1()
        {
            InitializeComponent();

            connection = new SqlConnection(@"Data Source=DESKTOP-9LEJM0T\SQLEXPRESS; Initial Catalog=DBName; Integrated Security=true");
            dataSet = new DataSet();

            childAdapter = new SqlDataAdapter("select * from ChildTable", connection);
            parentAdapter = new SqlDataAdapter("select * from ParentTable", connection);
            commandBuilder = new SqlCommandBuilder(childAdapter);

            parentAdapter.Fill(dataSet, "ParentTable");
            childAdapter.Fill(dataSet, "ChildTable");

            DataRelation dr = new DataRelation("FK_Parent_Child", dataSet.Tables["ParentTable"].Columns["id"], dataSet.Tables["ChildTable"].Columns["id"]);
            dataSet.Relations.Add(dr);

            BindingSource bsParent = new BindingSource();
            BindingSource bsChild = new BindingSource();

            bsParent.DataSource = dataSet;
            bsParent.DataMember = "ParentTable";

            bsChild.DataSource = bsParent;
            bsChild.DataMember = "FK_Parent_Child";

            dataGridChild.DataSource = bsChild;
            dataGridParent.DataSource = bsParent;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            childAdapter.Update(dataSet, "ChildTable");
        }
    }
}
