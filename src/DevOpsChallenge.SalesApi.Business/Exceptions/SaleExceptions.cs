using System;

namespace DevOpsChallenge.SalesApi.Business.Exceptions
{
    public static class SaleExceptions
    {
        public abstract class SaleException : Exception
        {
        }

        public class NotFound : SaleException
        {
        }

        public class AlreadyExists : SaleException
        {
        }

        public class TransactionIdRequired : SaleException
        {
        }

        public class TransactionIdInvalid : SaleException
        {
        }

        public class DateRequired : SaleException
        {
        }

        public class DateInvalid : SaleException
        {
        }

        public class ToDateRequired : SaleException
        {
        }

        public class FromDateRequired : SaleException
        {
        }

        public class ToDateMustBeAfterFromDate : SaleException
        {
        }

        public class DateRangeMustNoMoreThanAYear : SaleException
        {
        }

        public class AmountRequired : SaleException
        {
        }

        public class AmountInvalid : SaleException
        {
        }

        public class NotesInvalid : SaleException
        {
        }
    }
}
