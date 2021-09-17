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
        public static string EmailActivation(string FirstName, string activationLink)
        {
            return @"
                    <div><p>Hi, "+ FirstName + @"</p>
                        <p>To complete your registration, please verify your email:</p>
                        <a href='" + activationLink + @"' style='margin-top: 20px''></p>
                          Verify
                          </a>
                        <p> Thank you, The DiNero Team </p>";
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

            body.Attachments.Add("CsvErrors.xlsx", attachment);
            return body;
        }

        public static string InvoiceHtml(InvoiceSend invoice)
        {
            StringBuilder seatsTable = new StringBuilder();
            foreach (var seat in invoice.SeatNumbers) {
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
        <div
          style='
            display: flex;
            justify-content: space-between;
            align-items: center;
          '
        >
          <h1>INVOICE</h1>
          <div style='display: flex; flex-direction: column'>
            <span style='display: flex; flex-direction: column'>
              Ticket Bus Plus Zenica</span
            ><span style='display: flex; flex-direction: column'>
              Bosnia and Herzegovina
            </span>
          </div>
        </div>
        <div>
          <div>&nbsp;</div>
          <div><hr /></div>
          <div>
            <table
              style='
                height: 36px;
                width: 100%;
                border-collapse: collapse;
                border: none;
                margin-left: auto;
                margin-right: auto;
              '
              border='0'
            >
              <tbody>
                <tr style='height: 18px'>
                  <td style='width: 33.3333%; height: 18px'>
                    <strong>DATE</strong>
                  </td>
                  <td style='width: 33.3333%; height: 18px'>
                    <strong>INVOICE ID</strong>
                  </td>
                  <td style='width: 33.3333%; height: 18px'>
                    <strong>TO</strong>
                  </td>
                </tr>
                <tr style='height: 18px'>
                  <td style='width: 33.3333%; height: 18px'>" + DateTime.Now + @"</td>
                  <td style='width: 33.3333%; height: 18px'>" + new Guid() + @"</td>
                  <td style='width: 33.3333%; height: 18px'> " + invoice.Email + @"</td>
                </tr>
              </tbody>
            </table>
          </div>
          <div>&nbsp;</div>
          <div>&nbsp;</div>
        </div>
      </div>
    </div>
    <!-- end: Invoice header-->
    <!-- begin: Invoice body-->
    <div>
      <div>
        <div>
          <table>
            <thead>
              <tr style='height: 18px'>
                <th>Description</th>
                <th style='width: 535.656px; text-align: center; height: 18px'>
                  Amount
                </th>
              </tr>
            </thead>
            <tbody>
              " + seatsTable.ToString() + @"
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</div>
<p>
</p>
";
        }

    }
}
