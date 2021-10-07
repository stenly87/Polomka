﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Поломка.db;
using Поломка.mvvm;
using Поломка.Views;

namespace Поломка.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {
        private BitmapImage imageClient;

        public EditClientViewModel(Client client)
        {
            if (client == null)
                EditClient = new Client { RegistrationDate = DateTime.Now, GenderCode = "м" };
            else
            {
                EditClient = new Client
                {
                    ID = client.ID,
                    Birthday = client.Birthday,
                    ClientService = client.ClientService,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    Gender = client.Gender,
                    GenderCode = client.GenderCode,
                    LastName = client.LastName,
                    Patronymic = client.Patronymic,
                    Phone = client.Phone,
                    PhotoPath = client.PhotoPath,
                    RegistrationDate = client.RegistrationDate,
                    Tag = client.Tag
                };
                ImageClient = GetImageFromPath(Environment.CurrentDirectory + EditClient.PhotoPath);
            }

            SelectImage = new CustomCommand(() =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == true)
                {
                    try
                    {
                        var info = new FileInfo(ofd.FileName);
                        if (info.Length > 2 * 1024 * 1024)
                        {
                            MessageBox.Show("Размер фото не должен превышать 2МБ");
                            return;
                        }
                        ImageClient = GetImageFromPath(ofd.FileName);
                        EditClient.PhotoPath = $"/Клиенты/{info.Name}";
                        var newPath = Environment.CurrentDirectory + EditClient.PhotoPath;
                        File.Copy(ofd.FileName, newPath);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            });

            Save = new CustomCommand(() =>
            {
                try
                {
                    if (EditClient.ID == 0)
                        DBInstance.Get().Client.Add(EditClient);
                    else
                        DBInstance.Get().Entry(client).CurrentValues.SetValues(EditClient);
                    DBInstance.Get().SaveChanges();
                    MainWindow.Navigate(new ListClientsView());
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                };
            });
        }

        public Client EditClient { get; set; }

        public BitmapImage ImageClient
        {
            get => imageClient;
            set
            {
                imageClient = value;
                SignalChanged();
            }
        }

        private BitmapImage GetImageFromPath(string url)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(url, UriKind.Absolute);
            img.EndInit();
            return img;
        }

        public CustomCommand SelectImage { get; set; }
        public CustomCommand Save { get; set; }
    
    }
}
