using System;
using Nest;

namespace nest_test
{
    public class Transaction
    {
        [Keyword(Name = "iban_to")]
        public string IbanTo { get; set; }
        [Keyword(Name = "iban_from")]
        public string IbanFrom { get; set; }
        public Decimal Amount { get; set; }
    }
}