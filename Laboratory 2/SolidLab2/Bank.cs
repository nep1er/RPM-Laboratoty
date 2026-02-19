namespace SOLID_Fundamentals
{
    public abstract class Account
    {
        public decimal Balance { get; protected set; }

        public virtual void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public abstract void Withdraw(decimal amount);

        public virtual bool CanWithdraw(decimal amount)
        {
            return true;
        }

        public virtual decimal CalculateInterest()
        {
            return Balance * 0.01m;
        }
    }

    public class SavingsAccount : Account
    {
        public decimal MinimumBalance { get; } = 100m;

        public override bool CanWithdraw(decimal amount)
        {
            return Balance - amount >= MinimumBalance;
        }

        public override void Withdraw(decimal amount)
        {
            if (!CanWithdraw(amount))
            {
                throw new InvalidOperationException("Cannot go below minimum balance");
            }
            Balance -= amount;
        }
    }

    public class CheckingAccount : Account
    {
        public decimal OverdraftLimit { get; } = 500m;

        public override bool CanWithdraw(decimal amount)
        {
            return Balance - amount >= -OverdraftLimit;
        }

        public override void Withdraw(decimal amount)
        {
            if (!CanWithdraw(amount))
            {
                throw new InvalidOperationException("Overdraft limit exceeded");
            }
            Balance -= amount;
        }
    }

    public class FixedDepositAccount : Account
    {
        public DateTime MaturityDate { get; }

        public FixedDepositAccount(DateTime maturityDate)
        {
            MaturityDate = maturityDate;
        }

        public override bool CanWithdraw(decimal amount)
        {
            if (DateTime.Now < MaturityDate)
                return false;

            return amount <= Balance;
        }

        public override void Withdraw(decimal amount)
        {
            if (!CanWithdraw(amount))
            {
                throw new InvalidOperationException("Cannot withdraw at this time");
            }
            Balance -= amount;
        }

        public override decimal CalculateInterest()
        {
            return Balance * 0.05m;
        }
    }

    public class Bank
    {
        public void ProcessWithdrawal(Account account, decimal amount)
        {
            if (!account.CanWithdraw(amount))
            {
                Console.WriteLine($"Cannot withdraw {amount} from this account");
                return;
            }

            try
            {
                account.Withdraw(amount);
                Console.WriteLine($"Successfully withdrew {amount}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Withdrawal failed: {ex.Message}");
            }
        }

        public bool Transfer(Account from, Account to, decimal amount)
        {
            if (!from.CanWithdraw(amount))
            {
                Console.WriteLine("Transfer failed: cannot withdraw from source account");
                return false;
            }

            from.Withdraw(amount);
            to.Deposit(amount);
            return true;
        }
    }
}