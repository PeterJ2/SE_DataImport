using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;
using System.Data.SqlClient;
using HtmlAgilityPack;
using System.IO;
using System.Xml;

namespace SE_DataImport
{
    public partial class MainForm : Form
    {
        private bool isClosing = false;

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
            toolStripStatus.Text = msg;
            string completeMsg = String.Format("{0}: {1}{2}", DateTime.Now, msg, Environment.NewLine);
            messages.AppendText(completeMsg);
        }

        private void beginImportButton_Click(object sender, EventArgs e)
        {
            if (importDirectory.Text.Length == 0)
            {
                logMessage("Please specify input directoty");
                return;
            }
            try {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString.Text);
                string databaseName = builder.InitialCatalog;
                if (databaseName.Length == 0)
                {
                    logMessage("Please specify a database name (initial catalog) in the connection string");
                    return;
                }
                logMessage("Beginning import");
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
                        string tableName = tablePrefix.Text + node.Descendants("span").First().InnerHtml;
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
                                        if (columnNames[i] == "Id")
                                            sqlCreateTable += " NOT NULL";
                                    }
                                }
                                sqlCreateTable += ")";
                                new SqlCommand(sqlCreateTable, conn).ExecuteNonQuery();
                            }
                        }   
                    }
                    foreach (string fileName in Directory.GetFiles(importDirectory.Text, "*.XML"))
                    {
                        string tableName = tablePrefix.Text + Path.GetFileNameWithoutExtension(fileName);
                        logMessage(string.Format("Importing table {0}", tableName));
                        string sqlTruncateTable = string.Format("TRUNCATE TABLE {0}", tableName);
                        new SqlCommand(sqlTruncateTable, conn).ExecuteNonQuery();
                        using (XmlTextReader reader = new XmlTextReader(fileName))
                        {
                            int rowCount = 0;
                            reader.Read();
                            while (reader.Read())
                            {
                                if (reader.HasAttributes)
                                {
                                    rowCount++;
                                    List<string> columnNames = new List<string>();
                                    List<string> dataValues = new List<string>();
                                    SqlCommand cmd = new SqlCommand("", conn);
                                    while (reader.MoveToNextAttribute())
                                    {
                                        columnNames.Add(reader.Name);
                                        dataValues.Add(reader.Value);
                                        cmd.Parameters.AddWithValue("@" + reader.Name, reader.Value);
                                    }
                                    string columnList = columnNames.Aggregate((x, y) => x + "," + y);
                                    string paramList = columnNames.Aggregate((x, y) => x + ",@" + y);
                                    string sqlInsertCmd = String.Format("INSERT INTO {0} ({1}) VALUES (@{2})", tableName, columnList, paramList);
                                    cmd.CommandText = sqlInsertCmd;
                                    cmd.ExecuteNonQuery();
                                    reader.MoveToElement();
                                    if (rowCount % 100 == 0)
                                    {
                                        toolStripStatus.Text = String.Format("Inserted {0} rows", rowCount.ToString());
                                        Application.DoEvents();
                                    }
                                    if (isClosing)
                                        return;
                                }
                            }
                        }
                        logMessage(String.Format ("Adding primary key to {0}", tableName));
                        string sqlAddPrimaryKey = string.Format("IF NOT EXISTS (SELECT *  FROM sys.indexes WHERE name = 'PK_{0}') BEGIN ALTER TABLE {0} ADD CONSTRAINT PK_{0} PRIMARY KEY CLUSTERED (Id ASC) END", tableName);
                        SqlCommand addKeyCmd = new SqlCommand(sqlAddPrimaryKey, conn);
                        addKeyCmd.CommandTimeout = 600;
                        addKeyCmd.ExecuteNonQuery();
                    }
                    const string importFilename = "ImportReferenceTables.SQL";
                    if (File.Exists(importFilename))
                    {
                        logMessage("Importing reference table data");
                        using (StreamReader reader = new StreamReader(importFilename))
                        {
                            string sqlCmd;
                            while ((sqlCmd = reader.ReadLine()) != null)
                            {
                                sqlCmd = sqlCmd.Replace("INSERT [dbo].[", "INSERT [" + tablePrefix.Text);
                                SqlCommand cmd = new SqlCommand(sqlCmd, conn);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                        logMessage(String.Format("File {0} not found, skipping import of reference table data", importFilename));
                    conn.Close();
                    logMessage("Import finished");
                }
            } catch (Exception ex)
            {
                logMessage(ex.ToString());
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;
        }
    }
}
