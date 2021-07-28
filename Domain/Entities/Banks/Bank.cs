using Domain.Entities.BankAccounts;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Banks
{
    public class Bank
    {
        // Properties
        public BankType Type { get; private set; }
        public string Name { get; private set; }
        public bool IsEnabled { get; private set; }
        public ICollection<BankAccount> BankAccounts { get; private set; }

        // Constructors

        /// <exception cref="ArgumentException">
        ///     Invalid name length.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid type.
        /// </exception>
        public Bank(
            BankType type, 
            string name,
            bool isEnabled = true
        )
            : this()
        {
            if (!Enum.IsDefined(type))
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }
            Type = type;
            Update(name, isEnabled);
        }

        private Bank() {
            BankAccounts = new List<BankAccount>();
        }

        // Methods

        /// <exception cref="ArgumentException">
        ///     Invalid name length.
        /// </exception>
        public void Update(string name, bool isEnabled)
        {
            Update(name);
            if (isEnabled)
                Enable();
            else
                Disable();
        }

        /// <exception cref="ArgumentException">
        ///     Invalid name length.
        /// </exception>
        public void Update(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < NameMinLength || name.Length > NameMinLength)
            {
                throw new ArgumentException($"{nameof(Bank)} {nameof(name)} is required and its length must be between [{NameMinLength}, {NameMaxLength}]");
            }
            Name = name;
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }

        // Constants
        public const int NameMinLength = 1;
        public const int NameMaxLength = 80;
    }
}