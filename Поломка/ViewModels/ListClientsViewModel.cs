using System;
using System.Collections.Generic;
using Поломка.db;
using Поломка.mvvm;
using System.Linq;
using Поломка.Views;

namespace Поломка.ViewModels
{
    public class ListClientsViewModel : BaseViewModel
    {
        private string searchText = "";
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }

        public List<string> SearchType { get; set; }
        public string SelectedSearchType
        {
            get => selectedSearchType;
            set
            {
                selectedSearchType = value;
                Search();
            }
        }

        public List<Gender> GenderFilter { get; set; }
        public Gender SelectedGenderFilter
        {
            get => selectedGenderFilter;
            set
            {
                selectedGenderFilter = value;
                Search();
            }
        }

        internal void Sort(string p)
        {
            if (p == "LastName")
                searchResult.Sort((x, y) => x.LastName.CompareTo(y.LastName));
            else if (p == "LastVisit")
            {
                var forSort = searchResult.Where(s => s.LastVisit != null).ToList();
                var notSort = searchResult.Where(s => s.LastVisit == null);
                forSort.Sort((x, y) => y.LastVisit.Value.CompareTo(x.LastVisit));
                searchResult = forSort;
                searchResult.AddRange(notSort);
            }
            else if (p == "CountVisit")
            {
                searchResult.Sort((x, y) => y.CountVisit.CompareTo(x.CountVisit));
            }
            paginationPageIndex = 0;
            Pagination();
        }

        public List<string> ViewCountRows { get; set; }
        public string SelectedViewCountRows
        {
            get => selectedViewCountRows;
            set
            {
                selectedViewCountRows = value;
                paginationPageIndex = 0;
                Pagination();
            }
        }

        public List<Client> Clients
        {
            get => clients;
            set
            {
                clients = value;
                SignalChanged();
            }
        }
        public Client SelectedClient { get; set; }

        public string SearchCountRows
        {
            get => searchCountRows;
            set
            {
                searchCountRows = value;
                SignalChanged();
            }
        }

        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }
        public CustomCommand ViewClientWithBirthdayThisMonth { get; set; }
        public CustomCommand AddClient { get; set; }
        public CustomCommand EditClient { get; set; }
        public CustomCommand RemoveClient { get; set; }

        List<Client> searchResult;
        int paginationPageIndex = 0;
        private List<Client> clients;
        private string searchCountRows;
        private string selectedSearchType;
        private Gender selectedGenderFilter;
        private string selectedViewCountRows;

        public ListClientsViewModel()
        {
            GenderFilter = DBInstance.Get().Gender.ToList();
            GenderFilter.Add(new Gender { Name = "все", Code = "" });
            selectedGenderFilter = GenderFilter.Last();

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] {"10", "50", "200", "все" } );
            selectedViewCountRows = ViewCountRows.First();

            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "ФИО", "Email", "Телефон"});
            selectedSearchType = SearchType.First();

            BackPage = new CustomCommand(()=> {
                if (searchResult == null)
                    return;
                if (paginationPageIndex > 0)
                    paginationPageIndex--;
                Pagination();
            });

            ForwardPage = new CustomCommand(() =>
            {
                if (searchResult == null)
                    return;
                int.TryParse(SelectedViewCountRows, out int rowsOnPage);
                if (rowsOnPage == 0)
                    return;
                int countPage = searchResult.Count() / rowsOnPage;
                if (searchResult.Count() % rowsOnPage != 0)
                    countPage++;
                if (countPage > paginationPageIndex + 1)
                    paginationPageIndex++;
                Pagination();
            });

            ViewClientWithBirthdayThisMonth = new CustomCommand(()=>
            {
                searchResult = DBInstance.Get().Client
                    .Where(c => c.Birthday.HasValue &&
                        c.Birthday.Value.Month == DateTime.Now.Month).ToList();
                InitPagination();
                Pagination();
            });

            AddClient = new CustomCommand(()=> 
            {
                MainWindow.Navigate(new EditClientView());
            });
            EditClient = new CustomCommand(() =>
            {
                if (SelectedClient == null)
                    return;
                MainWindow.Navigate(new EditClientView(SelectedClient));
            });
            RemoveClient = new CustomCommand(() =>
            {
                if (SelectedClient == null)
                {
                    System.Windows.MessageBox.Show("Для удаления клиента нужно его выбрать в списке");
                    return;
                }
                if (SelectedClient.CountVisit > 0)
                {
                    System.Windows.MessageBox.Show("Невозможно удалить клиента, пользовавшегося нашими услугами");
                    return;
                }
                SelectedClient.Tag.Clear();
                DBInstance.Get().Client.Remove(SelectedClient);
                DBInstance.Get().SaveChanges();
                Search();
            });

            Search();
        }

        private void Search()
        {
            var search = SearchText.ToLower();
            if (SelectedSearchType == "ФИО")
                searchResult = DBInstance.Get().Client
                    .Where(c => c.GenderCode.Contains(SelectedGenderFilter.Code) &&
                    (c.FirstName.ToLower().Contains(search) ||
                    c.LastName.ToLower().Contains(search) ||
                    c.Patronymic.ToLower().Contains(search)
                    )).ToList();
            else if (SelectedSearchType == "Email")
                searchResult = DBInstance.Get().Client
                    .Where(c => c.GenderCode.Contains(SelectedGenderFilter.Code) &&
                    c.Email.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Телефон")
                searchResult = DBInstance.Get().Client
                    .Where(c => c.GenderCode.Contains(SelectedGenderFilter.Code) &&
                    c.Phone.ToLower().Contains(search)).ToList();
            InitPagination();
            Pagination();
        }

        private void InitPagination()
        {
            SearchCountRows = $"Найдено записей: {searchResult.Count} из {DBInstance.Get().Client.Count()}";
            paginationPageIndex = 0;
        }

        private void Pagination()
        {
            int rowsOnPage = 0;
            if (!int.TryParse(SelectedViewCountRows, out rowsOnPage))
            {
                Clients = searchResult;
            }
            else
            {
                Clients = searchResult.Skip(rowsOnPage * paginationPageIndex)
                    .Take(rowsOnPage).ToList();
            }
        }
    }
}