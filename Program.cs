using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.Text;

namespace _05._09
{
    class UserProfile
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public UserProfile(string login, string password)
        {
            Login = login;
            PasswordHash = password;
        }
    }
    class UserCollection
    {
        private List<UserProfile> users = new List<UserProfile>();

        public int Count
        {
            get { return users.Count; }
        }

        public string this[int index]
        {
            get
            {
                if (index >= 0 && index < users.Count)
                    return users[index].Login;
                throw new IndexOutOfRangeException("Неверный индекс.");
            }
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
        public void AddUser(string login, string password)
        {
            bool loginExists = false;
            foreach (var user in users)
            {
                if (user.Login == login)
                {
                    loginExists = true;
                    break;
                }
            }

            if (loginExists)
                Console.WriteLine("Пользователь с таким логином уже существует.");
            else
            {
                string passwordHash = HashPassword(password); 
                users.Add(new UserProfile(login, passwordHash));
                Console.WriteLine("Пользователь успешно добавлен.");
            }
        }

        public void RemoveUser(string login)
        {
            UserProfile userToRemove = null;
            foreach (var user in users)
            {
                if (user.Login == login)
                {
                    userToRemove = user;
                    break;
                }
            }

            if (userToRemove != null)
            {
                users.Remove(userToRemove);
                Console.WriteLine("Пользователь успешно удалён.");
            }
            else
                Console.WriteLine("Пользователь не найден.");

        }


        public void EditUser(string oldLogin, string newLogin, string newPassword)
        {
            UserProfile userToEdit = null;
            foreach (var user in users)
            {
                if (user.Login == oldLogin)
                {
                    userToEdit = user;
                    break;
                }
            }

            if (userToEdit != null)
            {
                bool newLoginExists = false;
                foreach (var user in users)
                {
                    if (user.Login == newLogin && user != userToEdit)
                    {
                        newLoginExists = true;
                        break;
                    }
                }

                if (newLoginExists)
                    Console.WriteLine("Логин уже используется другим пользователем.");
                else
                {
                    userToEdit.Login = newLogin;
                    userToEdit.PasswordHash = HashPassword(newPassword);
                    Console.WriteLine("Данные пользователя успешно изменены.");
                }
            }
            else
                Console.WriteLine("Пользователь не найден.");
        }


        public void ViewLogins()
        {
            if (users.Count == 0)
                Console.WriteLine("Список пользователей пуст.");
            else
            {
                Console.WriteLine("Список пользователей:");
                foreach (var user in users)
                    Console.WriteLine(user.Login + " " +user.PasswordHash);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            UserCollection userCollection = new UserCollection();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Посмотреть логины пользователей");
                Console.WriteLine("2. Добавить пользователя");
                Console.WriteLine("3. Удалить пользователя");
                Console.WriteLine("4. Изменить логин и пароль пользователя");
                Console.WriteLine("5. Посмотреть логин по индексу");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите действие: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        userCollection.ViewLogins();
                        break;

                    case "2":
                        Console.Write("Введите логин: ");
                        string newLogin = Console.ReadLine();
                        Console.Write("Введите пароль: ");
                        string newPassword = Console.ReadLine();
                        userCollection.AddUser(newLogin, newPassword);
                        break;

                    case "3":
                        Console.Write("Введите логин пользователя, которого хотите удалить: ");
                        string loginToRemove = Console.ReadLine();
                        userCollection.RemoveUser(loginToRemove);
                        break;

                    case "4":
                        Console.Write("Введите текущий логин пользователя: ");
                        string oldLogin = Console.ReadLine();
                        Console.Write("Введите новый логин: ");
                        string updatedLogin = Console.ReadLine();
                        Console.Write("Введите новый пароль: ");
                        string updatedPassword = Console.ReadLine();
                        userCollection.EditUser(oldLogin, updatedLogin, updatedPassword);
                        break;

                    case "5":
                        Console.Write("Введите индекс пользователя: ");
                        int index;
                        if (int.TryParse(Console.ReadLine(), out index))
                        {
                            try
                            {
                                Console.WriteLine($"Логин по индексу {index}: {userCollection[index]}");
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Индекс должен быть числом.");
                        }
                        break;

                    case "6":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}
