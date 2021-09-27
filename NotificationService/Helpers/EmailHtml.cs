using MimeKit;
using NotificationService.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Helpers
{
    public static class EmailHtml
    {
        public static string InfoSend(InfoSend info)
        {
            return @"
                    <div><p>Hi, " + info.FullName + @" sent you a message</p>
                        <p>" + info.Message + @"</p>
                        <p> Phone number: " + info.PhoneNumber + @" </p>";
        }
        public static string EmailActivation(string FirstName, string activationLink)
        {
            return @"
                    <div><p>Hi, "+ FirstName + @"</p>
                        <p>To complete your registration, please verify your email:</p>
                        <a href='" + activationLink + @"' style='margin-top: 20px''></p>
                          Verify
                          </a></div>";
        }

        public static string ResetPassword(string FirstName, string link)
        {
            return @"
                    <div><p>Hi, " + FirstName + @"</p>
                        <p>To reset your password click the next link:</p>
                        <a href='" + link + @"' style='margin-top: 20px''></p>
                          Reset
                          </a>
                        <p> Thank you, The BusPlus Team </p>";
        }

        public static BodyBuilder InvoiceBody(byte[] attachment)
        {
            var body = new BodyBuilder()
            {
                HtmlBody = @"<p>Your invoice is ready.</p>",
                TextBody = "Your invoice is ready.",
            };

            body.Attachments.Add("Invoicee.pdf", attachment);
            return body;
        }

        public static string InvoiceHtml(InvoiceSend invoice)
        {
            double total = 0.0;
            StringBuilder seatsTable = new StringBuilder();
            foreach (var seat in invoice.SeatNumbers) {
                total = total + double.Parse(invoice.Amount);
                seatsTable.Append(
                @" <tr style='height: 18px'>
                <td style='width: 143.344px; text-align: center; height: 18px'>
                  "+ seat + @"
                </td>
                <td style='width: 535.656px; text-align: center; height: 18px'>
                  <strong><span style='color: #3366ff'>$" + invoice.Amount + @"</span></strong>
                </td>
              </tr>");
            }
            return @"
<div>
<div>
<div>
<div>
<div style='display: flex; justify-content: space-between; align-items: center;'>
<h1>INVOICE</h1>
<div style='display: flex; flex-direction: column;'>
<p style='display: flex; flex-direction: column;'>Ticket Bus Plus Zenica</p>
<p style='display: flex; flex-direction: column;'>Bosnia and Herzegovina</p>
</div>
</div>
<div>
<div>&nbsp;</div>
<div><hr /></div>
<div>
<table style='height: 36px; width: 100%; border-collapse: collapse; border: none; margin-left: auto; margin-right: auto;' border='0'>
<tbody>
<tr style='height: 18px;'>
<td style='width: 33.3333%; height: 18px;'><strong>DATE</strong></td>
<td style='width: 33.3333%; height: 18px;'><strong>INVOICE ID</strong></td>
<td style='width: 33.3333%; height: 18px;'><strong>TO</strong></td>
</tr>
<tr style='height: 18px;'>
<td style='width: 33.3333%; height: 18px;'>" + DateTime.Now + @"</td>
<td style='width: 33.3333%; height: 18px;'>" + Guid.NewGuid().ToString() + @"</td>
<td style='width: 33.3333%; height: 18px;'>" + invoice.Email + @"</td>
</tr>

</tbody>
</table>
</div>
<div>&nbsp;</div>
<div><hr /></div>
</div>
</div>
</div>
<div style='width: 100%;'>
<div style='width: 100%;'>

<table style='height: 18px; width: 100%; border-collapse: collapse; border: none; margin-left: auto; margin-right: auto;' border='0'>
<thead>
<tr style='height: 18px;'>
<th style='height: 18px; width: 52.2727%;'>Seat number</th>
<th style='width: 47.5852%; text-align: center; height: 18px;'>Amount</th>
</tr>
" + seatsTable.ToString() + @"
</thead>
</table>
</div>
</div>
</div>
</div>
</div>
<hr />
<div style='width: 100%; display: flex; justify-content-end: end;'><strong>Total: $ "+  total  + @"</strong></div>
";
        }

    }
}
