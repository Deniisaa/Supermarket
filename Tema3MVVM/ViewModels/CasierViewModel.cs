using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tema3MVVM.Helpers;

namespace Tema3MVVM.ViewModels
{
    public class CasierViewModel : INotifyPropertyChanged
    {
        private SupermarketMVPEntities _context;
        private string _selectedTable;
        private ObservableCollection<object> _tableData;
        private string _searchText;
        private ObservableCollection<ReceiptItem> _currentReceipt;
        private bool _isReceiptFinalized;
        private int _casierId;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public CasierViewModel(int casierId)
        {
            _casierId = casierId;
            _context = new SupermarketMVPEntities();
            SearchByNameCommand = new RelayCommand(param => SearchByName());
            SearchByBarcodeCommand = new RelayCommand(param => SearchByBarcode());
            SearchByExpirationCommand = new RelayCommand(param => SearchByExpiration());
            SearchByProducerCommand = new RelayCommand(param => SearchByProducer());
            SearchByCategoryCommand = new RelayCommand(param => SearchByCategory());
            ReloadTableDataCommand = new RelayCommand(param => LoadTableData());
            AddToReceiptCommand = new RelayCommand4(AddToReceipt, CanAddToReceipt);
            FinalizeReceiptCommand = new RelayCommand4(FinalizeReceipt, CanFinalizeReceipt);
            TableNames = new ObservableCollection<string>
            {
                "Bon",
                "Categorie",
                "Producator",
                "Produs",
                "ProduseVandute",
                "Stoc",
                "Utilizator"
            };
            _currentReceipt = new ObservableCollection<ReceiptItem>();
            SelectedTable = "Produs";
            SearchText = "";
            LoadTableData();
        }

        public ObservableCollection<string> TableNames { get; }
        public ObservableCollection<object> TableData
        {
            get => _tableData;
            set
            {
                _tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }

        public ObservableCollection<ReceiptItem> CurrentReceipt
        {
            get => _currentReceipt;
            private set
            {
                _currentReceipt = value;
                OnPropertyChanged(nameof(CurrentReceipt));
                OnPropertyChanged(nameof(ReceiptTotal));
            }
        }

        public string ReceiptTotal => $"Total: {CurrentReceipt.Sum(item => item.TotalPrice)} lei";

        public bool IsReceiptFinalized
        {
            get => _isReceiptFinalized;
            private set
            {
                _isReceiptFinalized = value;
                OnPropertyChanged(nameof(IsReceiptFinalized));
                ((RelayCommand4)AddToReceiptCommand).RaiseCanExecuteChanged();
                ((RelayCommand4)FinalizeReceiptCommand).RaiseCanExecuteChanged();
            }
        }

        private object _selectedTableRow;
        public object SelectedTableRow
        {
            get { return _selectedTableRow; }
            set
            {
                _selectedTableRow = value;
                OnPropertyChanged(nameof(SelectedTableRow));
            }
        }

        public string SelectedTable
        {
            get => _selectedTable;
            set
            {
                _selectedTable = value;
                OnPropertyChanged(nameof(SelectedTable));
            }
        }

        public ICommand LoadTableDataCommand { get; }
        public ICommand SearchByNameCommand { get; }
        public ICommand SearchByBarcodeCommand { get; }
        public ICommand SearchByExpirationCommand { get; }
        public ICommand SearchByProducerCommand { get; }
        public ICommand SearchByCategoryCommand { get; }
        public ICommand ReloadTableDataCommand { get; }
        public ICommand AddToReceiptCommand { get; }
        public ICommand FinalizeReceiptCommand { get; }

        private void LoadTableData()
        {
            if (string.IsNullOrEmpty(SelectedTable))
                return;

            switch (SelectedTable)
            {
                case "Bon":
                    TableData = new ObservableCollection<object>(_context.Bons.ToList());
                    break;
                case "Categorie":
                    TableData = new ObservableCollection<object>(_context.Categories.ToList());
                    break;
                case "Producator":
                    TableData = new ObservableCollection<object>(_context.Producators.ToList());
                    break;
                case "Produs":
                    var produse = _context.Produs.Include(p => p.Categorie).Include(p => p.Producator).ToList();
                    TableData = new ObservableCollection<object>(produse);
                    break;
                case "ProduseVandute":
                    TableData = new ObservableCollection<object>(_context.ProduseVandutes.ToList());
                    break;
                case "Stoc":
                    TableData = new ObservableCollection<object>(_context.Stocs.ToList());
                    break;
                case "Utilizator":
                    TableData = new ObservableCollection<object>(_context.Utilizators.ToList());
                    break;
            }
        }

        private void SearchByName()
        {
            var filteredProdus = _context.Produs.Where(p =>
                p.nume_produs.ToLower().Contains(SearchText.ToLower())
            ).ToList();
            TableData = new ObservableCollection<object>(filteredProdus);
        }

        private void SearchByBarcode()
        {
            var filteredProdus = _context.Produs.Where(p =>
                p.cod_bare.ToString().Contains(SearchText)
            ).ToList();
            TableData = new ObservableCollection<object>(filteredProdus);
        }

        private void SearchByExpiration()
        {
            if (DateTime.TryParse(SearchText, out DateTime searchDate))
            {
                var filteredProdus = _context.Stocs
                    .Where(s => DbFunctions.TruncateTime(s.data_expirare) == DbFunctions.TruncateTime(searchDate))
                    .Select(s => s.Produ)
                    .Distinct()
                    .ToList();

                TableData = new ObservableCollection<object>(filteredProdus);
            }
            else
            {
                MessageBox.Show("Invalid date format. Please enter a valid date.");
            }
        }

        private void SearchByProducer()
        {
            var filteredProdus = _context.Produs.Where(p =>
                p.Producator.nume_producator.ToLower().Contains(SearchText.ToLower())
            ).ToList();
            TableData = new ObservableCollection<object>(filteredProdus);
        }

        private void SearchByCategory()
        {
            var filteredProdus = _context.Produs.Where(p =>
                p.Categorie.nume_categorie.ToLower().Contains(SearchText.ToLower())
            ).ToList();
            TableData = new ObservableCollection<object>(filteredProdus);
        }


        string connectionString = "data source=DESKTOP-S8NVOLJ\\SQLEXPRESS;initial catalog=SupermarketMVP;integrated security=True;trustservercertificate=True;MultipleActiveResultSets=True";
        private void AddToReceipt()
        {
            if (SelectedTableRow is Produ selectedProduct)
            {
                var inputDialog = new InputDialog("Enter quantity:");
                if (inputDialog.ShowDialog() == true)
                {
                    if (int.TryParse(inputDialog.Answer, out int quantity) && quantity > 0)
                    {
                        try
                        {
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();

                                // Check if there is sufficient stock for the selected product
                                string stockQuery = @"SELECT TOP 1 * FROM Stoc WHERE IDprodus = @IDprodus AND exista = 1 AND cantitate >= @quantity ORDER BY data_aprovizionare";
                                SqlCommand stockCommand = new SqlCommand(stockQuery, connection);
                                stockCommand.Parameters.AddWithValue("@IDprodus", selectedProduct.IDprodus);
                                stockCommand.Parameters.AddWithValue("@quantity", quantity);
                                SqlDataReader stockReader = stockCommand.ExecuteReader();

                                if (!stockReader.HasRows)
                                {
                                    MessageBox.Show("Insufficient stock for the selected quantity.");
                                    stockReader.Close();
                                    return;
                                }

                                stockReader.Read();

                                // Check if the selected stock is expired
                                int expDateIndex = stockReader.GetOrdinal("data_expirare");
                                DateTime? dataExpirare = stockReader.IsDBNull(expDateIndex) ? (DateTime?)null : stockReader.GetDateTime(expDateIndex);
                                if (dataExpirare.HasValue && dataExpirare.Value <= DateTime.Today)
                                {
                                    MessageBox.Show("Selected stock is expired. Cannot add it to the receipt.");
                                    stockReader.Close();
                                    return;
                                }

                                // Update the stock
                                int stocID = stockReader.GetInt32(stockReader.GetOrdinal("IDstoc"));
                                double pretVanzare = stockReader.GetDouble(stockReader.GetOrdinal("pret_vanzare"));
                                stockReader.Close();

                                string updateStockQuery = "UPDATE Stoc SET cantitate = cantitate - @quantity, exista = CASE WHEN cantitate - @quantity > 0 THEN 1 ELSE 0 END WHERE IDstoc = @IDstoc";
                                SqlCommand updateStockCommand = new SqlCommand(updateStockQuery, connection);
                                updateStockCommand.Parameters.AddWithValue("@quantity", quantity);
                                updateStockCommand.Parameters.AddWithValue("@IDstoc", stocID);
                                updateStockCommand.ExecuteNonQuery();

                                // Check if the product is already in the ProduseVandute table for this receipt
                                var lastBon = _context.Bons.OrderByDescending(b => b.IDbon).FirstOrDefault();
                                if (lastBon != null)
                                {
                                    string checkProduseVanduteQuery = "SELECT COUNT(*) FROM ProduseVandute WHERE IDbon = @IDbon AND IDprodus = @IDprodus";
                                    SqlCommand checkProduseVanduteCommand = new SqlCommand(checkProduseVanduteQuery, connection);
                                    checkProduseVanduteCommand.Parameters.AddWithValue("@IDbon", lastBon.IDbon);
                                    checkProduseVanduteCommand.Parameters.AddWithValue("@IDprodus", selectedProduct.IDprodus);
                                    int count = (int)checkProduseVanduteCommand.ExecuteScalar();

                                    if (count > 0)
                                    {
                                        // Update existing record
                                        string updateProduseVanduteQuery = "UPDATE ProduseVandute SET cantitate = cantitate + @quantity, subtotal = subtotal + @subtotal WHERE IDbon = @IDbon AND IDprodus = @IDprodus";
                                        SqlCommand updateProduseVanduteCommand = new SqlCommand(updateProduseVanduteQuery, connection);
                                        updateProduseVanduteCommand.Parameters.AddWithValue("@quantity", quantity);
                                        updateProduseVanduteCommand.Parameters.AddWithValue("@subtotal", quantity * pretVanzare);
                                        updateProduseVanduteCommand.Parameters.AddWithValue("@IDbon", lastBon.IDbon);
                                        updateProduseVanduteCommand.Parameters.AddWithValue("@IDprodus", selectedProduct.IDprodus);
                                        updateProduseVanduteCommand.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        // Insert new record
                                        string insertProduseVanduteQuery = "INSERT INTO ProduseVandute (IDbon, IDprodus, cantitate, subtotal, exista) VALUES (@IDbon, @IDprodus, @cantitate, @subtotal, 1)";
                                        SqlCommand insertProduseVanduteCommand = new SqlCommand(insertProduseVanduteQuery, connection);
                                        insertProduseVanduteCommand.Parameters.AddWithValue("@IDbon", lastBon.IDbon);
                                        insertProduseVanduteCommand.Parameters.AddWithValue("@IDprodus", selectedProduct.IDprodus);
                                        insertProduseVanduteCommand.Parameters.AddWithValue("@cantitate", quantity);
                                        insertProduseVanduteCommand.Parameters.AddWithValue("@subtotal", quantity * pretVanzare);
                                        insertProduseVanduteCommand.ExecuteNonQuery();
                                    }
                                }

                                connection.Close();
                            }

                            // Update UI
                            var existingItem = CurrentReceipt.FirstOrDefault(item => item.Product.IDprodus == selectedProduct.IDprodus);
                            if (existingItem != null)
                            {
                                existingItem.Quantity += quantity;
                            }
                            else
                            {
                                CurrentReceipt.Add(new ReceiptItem { Product = selectedProduct, Quantity = quantity });
                            }

                            OnPropertyChanged(nameof(CurrentReceipt));
                            OnPropertyChanged(nameof(ReceiptTotal));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid quantity.");
                    }
                }
            }
        }






        private bool CanAddToReceipt() => !IsReceiptFinalized && SelectedTableRow is Produ;

        private void FinalizeReceipt()
        {
            try
            {
                var sumaIncasata = CurrentReceipt?.Sum(item => item.TotalPrice) ?? 0;

                if (CurrentReceipt == null || !CurrentReceipt.Any())
                {
                    MessageBox.Show("The receipt is empty. Please add items to the receipt before finalizing.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert a new record into the Bon table
                        string insertBonQuery = @"
                    INSERT INTO Bon (data_eliberare, IDcasier, suma_incasa, exista)
                    OUTPUT INSERTED.IDbon
                    VALUES (@data_eliberare, @IDcasier, @suma_incasa, @exista)";
                        SqlCommand insertBonCommand = new SqlCommand(insertBonQuery, connection, transaction);
                        insertBonCommand.Parameters.AddWithValue("@data_eliberare", DateTime.Now);
                        insertBonCommand.Parameters.AddWithValue("@IDcasier", _casierId);
                        insertBonCommand.Parameters.AddWithValue("@suma_incasa", (double)sumaIncasata);
                        insertBonCommand.Parameters.AddWithValue("@exista", true);

                        // Get the inserted IDbon
                        int newBonId = (int)insertBonCommand.ExecuteScalar();

                        // Insert all items in the current receipt into the ProduseVandute table
                        foreach (var item in CurrentReceipt)
                        {
                            if (item.Product == null)
                            {
                                throw new Exception("Product in receipt item is null.");
                            }

                            string insertProduseVanduteQuery = @"
                        INSERT INTO ProduseVandute (IDbon, IDprodus, cantitate, subtotal, exista)
                        VALUES (@IDbon, @IDprodus, @cantitate, @subtotal, @exista)";
                            SqlCommand insertProduseVanduteCommand = new SqlCommand(insertProduseVanduteQuery, connection, transaction);
                            insertProduseVanduteCommand.Parameters.AddWithValue("@IDbon", newBonId);
                            insertProduseVanduteCommand.Parameters.AddWithValue("@IDprodus", item.Product.IDprodus);
                            insertProduseVanduteCommand.Parameters.AddWithValue("@cantitate", item.Quantity);
                            insertProduseVanduteCommand.Parameters.AddWithValue("@subtotal", (double)item.TotalPrice);
                            insertProduseVanduteCommand.Parameters.AddWithValue("@exista", true);
                            insertProduseVanduteCommand.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();

                        // Update the UI
                        IsReceiptFinalized = true;
                        ((RelayCommand4)AddToReceiptCommand).RaiseCanExecuteChanged();
                        ((RelayCommand4)FinalizeReceiptCommand).RaiseCanExecuteChanged();

                        MessageBox.Show("Receipt finalized and saved to database.");
                        ResetReceipt();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();
                        MessageBox.Show($"An error occurred while finalizing the receipt: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while finalizing the receipt: {ex.Message}");
            }
        }



        private void ResetReceipt()
        {
            CurrentReceipt.Clear();
            IsReceiptFinalized = false;
            OnPropertyChanged(nameof(CurrentReceipt));
            OnPropertyChanged(nameof(ReceiptTotal));
        }


        private bool CanFinalizeReceipt() => CurrentReceipt.Any() && !IsReceiptFinalized;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ReceiptItem
    {
        public Produ Product { get; set; }
        public Stoc Stoc { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => (decimal)(Stoc?.pret_vanzare ?? 0) * Quantity;
    }

}
