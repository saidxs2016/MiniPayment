using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPayment.Domain.Helpers;

public sealed class TransactionTypesHelper
{
    public readonly static string Sale = "Sale";
    public readonly static string Refund = "Refund";
    public readonly static string Cancel = "Cancel";
}
