using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Task4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeCombobox();
        }

        private void InitializeCombobox()
        {
            var years = new List<int>();
            using (var reader = new StreamReader("SalesData.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var sales = csv.GetRecords<SalesData>();
                foreach (SalesData data in sales)
                {
                    if (!years.Contains(data.Year))
                        years.Add(data.Year);
                }
            }
            comboBox.DataSource = years;
        }

        private void CalculateData(int year)
        {
            var countrySales = new Dictionary<int, int>();
            var countryNames = new Dictionary<int, string>();
            using (var reader = new StreamReader("SalesData.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var sales = csv.GetRecords<SalesData>();
                foreach (SalesData data in sales)
                {
                    if (data.Year == year)
                        if (!countrySales.ContainsKey(data.CountryID))
                        {
                            countrySales.Add(data.CountryID, data.OrderCount);
                            countryNames.Add(data.CountryID, data.CountryName);
                        }
                        else
                            countrySales[data.CountryID] += data.OrderCount;
                }
            }
            var firstTen = countrySales.OrderByDescending(c => c.Value).Take(10).Select(c => new {Country = countryNames[c.Key], OrderCount = c.Value}).ToList();
            dataGridView.DataSource = firstTen;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateData((int)comboBox.SelectedItem);
        }
    }
}
