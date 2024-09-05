namespace Classes
{
    public class Konto //informacje o posiadaczu konta, plus stan konta
    {
        public string Owner { get; }
        public decimal Balance { get; set; }
        public string accountNum { get; set; }
        public Konto(string name, decimal initialBalance, string accNum)
        {
            this.Owner = name;
            this.Balance = initialBalance;
            this.accountNum = accNum;
        }

        public void Withdraw(decimal balance) //metoda do wypłacania pieniędzy z konta
        {
            this.Balance -= balance;
        }

        public void Deposit(decimal balance) //metoda do wpłacania pieniędzy na konto
        {
            this.Balance += balance;
        }
    }

    public class Karta //przechowywanie danych karty, do walidacji numeru PIN
    {
        public string pinNum { get; set; }

        public string cardNum { get; set; }
        public Karta(string PIN, string cardNum)
        {
            this.pinNum = PIN;
            this.cardNum = cardNum;
        }
    }
}

