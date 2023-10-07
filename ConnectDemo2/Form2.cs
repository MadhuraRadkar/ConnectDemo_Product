using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms;

namespace ConnectDemo2
{
    public partial class Form2 : Form
    {
        SqlConnection con;
        SqlDataAdapter da;
        SqlCommandBuilder builder;
        DataSet ds;
        public Form2()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                string qry = "select * from category";
                da = new SqlDataAdapter(qry, con);
                ds = new DataSet();
                da.Fill(ds, "Catg");
                cmbCategory.DataSource = ds.Tables["Catg"];
                cmbCategory.DisplayMember = "cname";
                cmbCategory.ValueMember = "cid";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private DataSet GetProducts()
        {
            string qry = "select * from product";
            // assign the query
            da = new SqlDataAdapter(qry, con);
            // when app load the in DataSet, we need to manage the PK also
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            // SCB will track the DataSet & update quries to the DataAdapter
            builder = new SqlCommandBuilder(da);
            ds = new DataSet();
            da.Fill(ds, "Product");// this name given to the DataSet table
            return ds;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProducts();
                // create new row to add recrod
                DataRow row = ds.Tables["Product"].NewRow();
                // assign value to the row
                row["pname"] = txtName.Text;
                row["price"] = txtPrice.Text;
                row["cid"] = cmbCategory.SelectedValue;
                // attach this row in DataSet table
                ds.Tables["Product"].Rows.Add(row);
                // update the changes from DataSet to DB
                int result = da.Update(ds.Tables["Product"]);
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProducts();
                // find the row
                DataRow row = ds.Tables["Product"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    row["pname"] = txtName.Text;
                    row["price"] = txtPrice.Text;
                    row["cid"] = cmbCategory.SelectedValue;
                    // update the changes from DataSet to DB
                    int result = da.Update(ds.Tables["Product"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record updated");
                    }
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProducts();
                // find the row
                DataRow row = ds.Tables["Product"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    // delete the current row from DataSet table
                    row.Delete();
                    // update the changes from DataSet to DB
                    int result = da.Update(ds.Tables["Product"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record deleted");
                    }
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowall_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select p.*, c.cname from product p inner join category c on c.cid=p.cid ";
                da = new SqlDataAdapter(qry, con);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ds = new DataSet();
                da.Fill(ds, "prod");
                dataGridView1.DataSource = ds.Tables["prod"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select p.*, c.cname from product p inner join category c on c.cid=p.cid ";
                da = new SqlDataAdapter(qry, con);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ds = new DataSet();
                da.Fill(ds, "prod");
                //find method can only seach the data if PK is applied in the DataSet table
                DataRow row = ds.Tables["prod"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    txtName.Text = row["pname"].ToString();
                    txtPrice.Text = row["price"].ToString();
                    cmbCategory.Text = row["cname"].ToString();
                }
                else
                {
                    MessageBox.Show("Record not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dataGridView1.CurrentRow.Cells["pid"].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["pname"].Value.ToString();
            txtPrice.Text = dataGridView1.CurrentRow.Cells["price"].Value.ToString();
            cmbCategory.Text = dataGridView1.CurrentRow.Cells["cname"].Value.ToString() ;
        }
    }
}
