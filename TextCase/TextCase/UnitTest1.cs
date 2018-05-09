using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextCase;
using System.IO;
using System.Linq;

namespace TextCase
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ProgrammerTest()
        {
            var address = new Address("56 Main St", "Mesa", "AZ", "38574");
            var customer = new Customer("John", "Doe", address);
            var company = new Company("Google", address);

            Assert.IsNull(customer.Id);
            customer.Save();
            Assert.IsNotNull(customer.Id);

            Assert.IsNull(company.Id);
            company.Save();
            Assert.IsNotNull(company.Id);

            Customer savedCustomer = Customer.Find(customer.Id);
            Assert.IsNotNull(savedCustomer);
            Assert.AreSame(customer.Address, address);
            Assert.AreEqual(savedCustomer.Address, address);
            Assert.AreEqual(customer.Id, savedCustomer.Id);
            Assert.AreEqual(customer.FirstName, savedCustomer.FirstName);
            Assert.AreEqual(customer.LastName, savedCustomer.LastName);
            Assert.AreEqual(customer, savedCustomer);
            Assert.AreNotSame(customer, savedCustomer);

            Company savedCompany = Company.Find(company.Id);
            Assert.IsNotNull(savedCompany);
            Assert.AreSame(company.Address, address);
            Assert.AreEqual(savedCompany.Address, address);
            Assert.AreEqual(company.Id, savedCompany.Id);
            Assert.AreEqual(company.Name, savedCompany.Name);
            Assert.AreEqual(company, savedCompany);
            Assert.AreNotSame(company, savedCompany);

            customer.Delete();
            Assert.IsNull(customer.Id);
            Assert.IsNull(Customer.Find(customer.Id));

            company.Delete();
            Assert.IsNull(company.Id);
            Assert.IsNull(Company.Find(company.Id));
        }

        public class Address
        {
            public string Direction;
            public string City;
            public string State;
            public string Zipcode;

            public Address(string Direction, string City, string State, string Zipcode)
            {
                this.Direction = Direction;
                this.City = City;
                this.State = State;
                this.Zipcode = Zipcode;
            }
        }

        public class Customer
        {
            public Address Address;
            public string FirstName;
            public string LastName;
            private string InitialValue;
            private int c;
            
            public Customer(string FirstName, string LastName, Address address)
            {
                this.FirstName = FirstName;
                this.LastName = LastName;
                this.Address = address;
            }

            public int Id
            {
                set
                {
                    string path = @"c:\temp\Customer.txt";
                    InitialValue = "";
                    c = 0;
                    if (File.Exists(path))
                    {
                        string[] lines = System.IO.File.ReadAllLines(path);
                        string lastLine = lines[lines.Length - 1];
                        while (!(lastLine.Contains(',')))
                        {
                            InitialValue = InitialValue + lastLine[c].ToString();

                            c++;
                        }
                        Id = int.Parse(InitialValue) + 1;
                    }
                    else
                    {
                        Id = 1;
                    }
                }
                get
                {
                    return Id;
                }
            }

            public void Save()
            {               
                    string path = @"c:\temp\Customer.txt";
                    string text = Id.ToString() + ',' + FirstName + ',' + LastName + ',' + Address;
                    if (File.Exists(path))
                    {
                        using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(path, true))
                        {
                            file.WriteLine(text);
                            
                        }
                    }
                    else
                    {
                        System.IO.File.WriteAllText(path, text);
                        
                    }                
            }

            public static Customer Find(int Key)
            {
                string path = @"c:\temp\Customer.txt";
                string InitialValue = "";
                int c = 0;
                int n;
                string[] selectedline = null;
                Address Address = null;
                string FirstName = null;
                string LastName = null;
                int Id;
                Customer Client = null;
                bool exists = false; 
                if (File.Exists(path))
                {
                    string[] lines = System.IO.File.ReadAllLines(path);
                    int size = lines.Length-1;
                    for (n = 0; n < size; n++)
                    {
                        string currentline = lines[n];
                        while (!(currentline.Contains(',')))
                        {
                            InitialValue = InitialValue + currentline[c].ToString();
                            c++;
                        }
                        Id = int.Parse(InitialValue);
                        if (Key == Id)
                        {
                            selectedline = currentline.Split(',');
                            exists = true;
                            break;
                        }
                    }
                    if (exists)
                    {
                        FirstName = selectedline[1].ToString();
                        LastName = selectedline[2].ToString();
                        Address.Direction = selectedline[3].ToString();
                        Address.City = selectedline[4].ToString();
                        Address.State = selectedline[5].ToString();
                        Address.Zipcode = selectedline[6].ToString();
                        Client.FirstName = FirstName;
                        Client.LastName = LastName;
                        Client.Address = Address;                        
                        return Client;
                    }
                    else
                    {
                        return null;
                    }
                        
                    
                }
                else
                {
                    return null;
                }                
            }

            public void Delete()
            {
                string path = @"c:\temp\Customer.txt";
                int n;                                                                            
                if (File.Exists(path))
                {
                    string[] lines = System.IO.File.ReadAllLines(path);
                    int size = lines.Length - 1;
                    for (n = 0; n < size; n++)
                    {
                        string currentline = lines[n];
                        currentline = null;
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                        {
                            file.WriteLine(currentline);
                        }
                    }
                }
            }

        }

        public class Company
        {
            public Address Address;
            public string Name;
            private string InitialValue;
            private int c;

            public Company(string Name, Address address)
            {
                this.Name = Name;
                this.Address = address;
            }

            public int Id
            {
                set
                {
                    string path = @"c:\temp\Company.txt";
                    InitialValue = "";
                    c = 0;
                    if (File.Exists(path))
                    {
                        string[] lines = System.IO.File.ReadAllLines(path);
                        string lastLine = lines[lines.Length - 1];
                        while (!(lastLine.Contains(',')))
                        {
                            InitialValue = lastLine[c].ToString();

                            c++;
                        }
                        Id = int.Parse(InitialValue) + 1;
                    }
                    else
                    {
                        Id = 1;
                    }
                }
                get { return Id; }
            }

            public void Save()
            {               
                    string path = @"c:\temp\Company.txt";
                    string text = Id.ToString() + ',' + Name + ',' + Address;
                    if (File.Exists(path))
                    {
                        using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(path, true))
                        {
                            file.WriteLine(text);
                        }
                    }
                    else
                    {
                        System.IO.File.WriteAllText(path, text);
                    }
                                
            }

            public static Company Find(int Key)
            {
                string path = @"c:\temp\Company.txt";
                string InitialValue = "";
                int c = 0;
                int n;
                string[] selectedline = null;
                Address Address = null;
                string Name = null;               
                int Id;
                Company Corporation = null;
                bool exists = false;
                if (File.Exists(path))
                {
                    string[] lines = System.IO.File.ReadAllLines(path);
                    int size = lines.Length - 1;
                    for (n = 0; n < size; n++)
                    {
                        string currentline = lines[n];
                        while (!(currentline.Contains(',')))
                        {
                            InitialValue = InitialValue + currentline[c].ToString();
                            c++;
                        }
                        Id = int.Parse(InitialValue);
                        if (Key == Id)
                        {
                            selectedline = currentline.Split(',');
                            exists = true;
                            break;
                        }
                    }
                    if (exists)
                    {
                        Name = selectedline[1].ToString();                        
                        Address.Direction = selectedline[2].ToString();
                        Address.City = selectedline[3].ToString();
                        Address.State = selectedline[4].ToString();
                        Address.Zipcode = selectedline[5].ToString();
                        Corporation.Name = Name;                        
                        Corporation.Address = Address;
                        return Corporation;
                    }
                    else
                    {
                        return null;
                    }


                }
                else
                {
                    return null;
                }
            }

            public void Delete()
            {
                string path = @"c:\temp\Company.txt";
                int n;
                if (File.Exists(path))
                {
                    string[] lines = System.IO.File.ReadAllLines(path);
                    int size = lines.Length - 1;
                    for (n = 0; n < size; n++)
                    {
                        string currentline = lines[n];
                        currentline = null;
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                        {
                            file.WriteLine(currentline);
                        }
                    }
                }
            }

        }        

    }
}
