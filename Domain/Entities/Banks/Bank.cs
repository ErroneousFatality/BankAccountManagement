using Domain.Entities.BankAccounts;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Banks
{
    public class Bank
    {
        // Properties
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public ICollection<BankAccount> BankAccounts { get; private set; }

        // Constructors
        public Bank(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < NameMinLength || name.Length > NameMinLength)
            {
                throw new ArgumentException($"{nameof(Bank)} {nameof(name)} is required and its length must be between [{NameMinLength}, {NameMaxLength}]");
            }
            Name = name;
        }
        private Bank() { }

        // Constants
        public const int NameMinLength = 1;
        public const int NameMaxLength = 80;
    }
}