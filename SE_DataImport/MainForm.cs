using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;
using System.Data.SqlClient;
using HtmlAgilityPack;

namespace SE_DataImport
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buildConnectionStringButton_Click(object sender, EventArgs e)
        {
            using (DataConnectionDialog connectionDialog = new DataConnectionDialog())
            {
                connectionDialog.DataSources.Add(DataSource.SqlDataSource);
                connectionDialog.ConnectionString = connectionString.Text;
                if (DataConnectionDialog.Show(connectionDialog) == DialogResult.OK)
                    connectionString.Text = connectionDialog.ConnectionString;
            }
        }

        private void findImportDirectoryButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDlg.ShowDialog() == DialogResult.OK)
                importDirectory.Text = folderBrowserDlg.SelectedPath;
        }

        private void logMessage(string msg)
        {
            string completeMsg = String.Format("{0}: {1}{2}", DateTime.Now, msg, Environment.NewLine);
            messages.AppendText(completeMsg);
        }

        private void beginImportButton_Click(object sender, EventArgs e)
        {
            messages.Text = "";
            try {
                logMessage("Beginning import");
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString.Text);
                string databaseName = builder.InitialCatalog;
                if (databaseName.Length == 0)
                {
                    logMessage("Please specify a database name (initial catalog) in the connection string");
                    return;
                }
                builder.InitialCatalog = "";
                using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
                {
                    conn.Open();
                    string sqlCheckDBExists = string.Format("USE master; SELECT COUNT(*) FROM sys.databases WHERE Name = '{0}'", databaseName);
                    using (SqlCommand cmd = new SqlCommand(sqlCheckDBExists, conn))
                    {
                        if (cmd.ExecuteScalar().ToString() == "0")
                        {
                            logMessage(String.Format("Database {0} does not exist - creating", databaseName));
                            new SqlCommand(string.Format("CREATE DATABASE {0}", databaseName), conn).ExecuteNonQuery();
                        }
                    }
                    conn.Close();
                }
                builder.InitialCatalog = databaseName;
                using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
                {
                    conn.Open();
                    logMessage("Getting table definitions from Stack Exchange Data Explorer");
                    HtmlWeb web = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = web.Load("http://data.stackexchange.com/stackoverflow/query/new");
                    foreach (HtmlNode node in doc.DocumentNode.Descendants("li").Where(d => d.Attributes["data-order"] != null))
                    {
                        string tableName = node.Descendants("span").First().InnerHtml;
                        string sqlCheckTableExists = string.Format("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'", tableName);
                        using (SqlCommand cmd = new SqlCommand(sqlCheckTableExists, conn))
                        {
                            if (cmd.ExecuteScalar().ToString() == "0")
                            {
                                logMessage(String.Format("Table {0} does not exist - creating", tableName));
                                string sqlCreateTable = string.Format("CREATE TABLE {0} (", tableName);
                                foreach (HtmlNode dataNode in node.Descendants("dl"))
                                {
                                    List<string> columnNames = new List<string>();
                                    foreach (HtmlNode columnNameNode in dataNode.Descendants("dt"))
                                        columnNames.Add(columnNameNode.InnerHtml);
                                    List<string> dataTypes = new List<string>();
                                    foreach (HtmlNode dataTypeNode in dataNode.Descendants("dd"))
                                        dataTypes.Add(dataTypeNode.InnerHtml);
                                    for (int i = 0; i < columnNames.Count; i++)
                                    {
                                        if (i > 0)
                                            sqlCreateTable += ",";
                                        sqlCreateTable += columnNames[i] + " " + dataTypes[i];
                                    }
                                }
                                sqlCreateTable += ")";
                                new SqlCommand(sqlCreateTable, conn).ExecuteNonQuery();
                            }
                        }   
                    }
                    conn.Close();
                    logMessage("Import finished");
                }
            } catch (Exception ex)
            {
                logMessage(ex.ToString());
            }
        }
    }
}
