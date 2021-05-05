using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApi.Core.Email
{
    public static class EmailTemplates
    {
        public static string RegisterTemplate = @"  <div>
    <table style=""border-collapse:collapse;table-layout:fixed;min-width:320px;width:100%;background-color:#f2f4f6;""
      cellpadding=""0"" cellspacing=""0"" role=""presentation"">

      <tbody>
        <tr>
          <td>
            <div>
              <div style=""Margin:0 auto;max-width:560px;min-width:280px;width:280px;"">
                <div style=""border-collapse:collapse;display:table;width:100%;"">
                  <div style=""display:table-cell;
                    Float:left;
                    font-size:12px;
                    line-height:19px;
                    max-width:280px;
                    min-width:140px;               
                    padding:10px 0 5px 0;
                    color:#717a8a;
                    font-family:sans-serif;"">
                    <p style=""Margin-top:0;
                    Margin-bottom:0;"">
                      Confirm your email to activate your account</p>
                  </div>
                  <div style=""display:table-cell;
                    Float:left;
                    font-size:12px;
                    line-height:19px;
                    max-width:280px;
                    min-width:139px;                    
                    padding:10px 0 5px 0;text-align:right;
                    color:#717a8a;
                    font-family:sans-serif;"">
                  </div>
                </div>
              </div>
            </div>

            <div>
              <div style=""background-color:#ffffff;"">
                <div style=""Margin:0 auto;max-width:600px;min-width:320px;width:320px;word-wrap:break-word;"">
                  <div style=""border-collapse:collapse;display:table;width:100%;"">

                    <div style=""max-width:600px;
                      min-width:320px;
                      width:320px;
                      text-align:left;
                      color:#111324;
                      font-size:16px;
                      line-height:24px;
                      font-family:sans-serif;"">

                      <div style=""Margin-left:20px;
                            Margin-right:20px;"">
                        <div style=""line-height:45px;font-size:1px;"">&nbsp;</div>
                      </div>

                      <div style=""Margin-left:20px;Margin-right:20px;"">
                        <div
                          style=""font-size:12px;font-style:normal;font-weight:normal;line-height:19px;Margin-bottom:20px;"">
                            <img style=""height:200px""border:0;display:block;height:auto;width:100%;max-width:200px;height:200px""
                              alt=""Shop Assistant logo""
                              src=""https://i.ibb.co/DKBjmqr/register.jpg""></a>
                        </div>
                      </div>

                      <div style=""Margin-left:20px;
                                  Margin-right:20px;"">
                        <div style=""line-height:20px;
                        font-size:1px;"">&nbsp;</div>
                      </div>

                      <div style=""Margin-left:20px;Margin-right:20px;"">
                        <p style=""Margin-top:0;Margin-bottom:20px;"">Hi {0},</p>
                      </div>

                      <div style=""Margin-left:20px;Margin-right:20px;"">
                        <div style=""line-height:1px;font-size:1px;"">&nbsp;</div>
                      </div>

                    </div>

                  </div>
                </div>
              </div>

              <div style=""background-color:#ffffff;"">
                <div style=""Margin:0 auto;
                  max-width:600px;
                  min-width:320px;
                  width: 320px;
                  word-wrap:break-word;"">
                  <div style=""border-collapse:collapse;
                              display:table;
                              width:100%;"">
                    <div
                      style=""max-width:600px;min-width:320px;width:320px;text-align:left;color:#111324;font-size:16px;line-height:24px;font-family:sans-serif;"">

                      <div style=""Margin-left:20px;Margin-right:20px;"">
                        <div style=""line-height:2px;font-size:1px;"">&nbsp;</div>
                      </div>

                      <div style=""Margin-left:20px;Margin-right:20px;"">

                        <h1
                          style=""Margin-top:0;Margin-bottom:20px;font-style:normal;font-weight:normal;color:#111324;font-size:22px;line-height:31px;text-align:left;"">
                          Thanks for joining Shop Assistant!</h1>

                      </div>

                      <div style=""Margin-left:20px;Margin-right:20px;"">
                        <div style="""">
                          <p style=""Margin-top:0;Margin-bottom:20px;"">Please confirm your email address to activate your
                            account.</p>
                        </div>
                      </div>

                      <div style=""Margin-left:20px;Margin-right:20px;"">
                        <div style=""Margin-bottom:20px;text-align:left;"">
                          <a style=""border-radius:4px;
                                    display:inline-block;
                                    font-size:14px;
                                    font-weight:bold;
                                    line-height:24px;
                                    padding:12px 24px;
                                    text-align:center;
                                    text-decoration:none !important;
                                    transition:opacity 0.1s ease-in;
                                    color:#ffffff !important;
                                    background-color:#7856ff;
                                    font-family:sans-serif;"" href={1}>Confirm
                            my address</a>
                        </div>
                      </div>

                      <div style=""Margin-left:20px;Margin-right:20px;"">

                        <p style=""Margin-top:0;Margin-bottom:20px;font-size:12px;line-height:19px;"">If you didn't sign
                          up to Shop Assistant, please ignore this email. It's likely someone else accidentally
                          entered your address or made a typo.</p>

                      </div>

                      <div style=""Margin-left:20px;Margin-right:20px;"">
                        <div style=""line-height:20px;font-size:1px;"">&nbsp;</div>
                      </div>

                    </div>

                  </div>
                </div>
              </div>


              <div style="""">
                <div style=""Margin:0 auto;
                  max-width:600px;
                  min-width:320px;
                  width:320px;
                  word-wrap:break-word;"">
                  <div style=""border-collapse:collapse;
                              display:table;
                              width:100%;"">

                    <div style=""text-align:left;
                            font-size:12px;
                            line-height:19px;
                            color:#717a8a;
                            font-family:sans-serif;
                            Float:left;
                            max-width:400px;
                            min-width:320px;
                            width:320px;"">

                      <div style=""Margin-left:20px;
                                  Margin-right:20px;
                                  Margin-top:10px;
                                  Margin-bottom:10px;"">

                        <div style=""font-size:12px;line-height:19px;"">
                          <div>	&#169; Shop Assistant<br>
                           Maramures,Lucacesti, Somensului nr 8</div>
                        </div>
                        <div style=""font-size:12px;line-height:19px;Margin-top:18px;"">
                        </div>
                      </div>
                    </div>

                    <div  style=""height:50px;""text-align:left;
                    font-size:12px;
                    line-height:19px;
                    color:#717a8a;
                    font-family:sans-serif;
                    Float:left;
                    max-width:320px;
                    min-width:200px;
                    width:320px;"">
                      <div style=""Margin-left:20px;Margin-right:20px;Margin-top:10px;Margin-bottom:10px;"">

                      </div>
                    </div>

                  </div>
                </div>
                <div style=""Margin:0 auto;max-width:600px;min-width:320px;width:320px;word-wrap:break-word;"">
                  <div style=""border-collapse:collapse;display:table;width:100%;"">

                    <div
                      style=""text-align:left;font-size:12px;line-height:19px;color:#717a8a;font-family:sans-serif;max-width:600px;min-width:320px;width:320px;"">
                      <div style=""Margin-left:20px;Margin-right:20px;Margin-top:10px;Margin-bottom:10px;"">
                        <div style=""font-size:12px;line-height:19px;"">
                        </div>
                      </div>
                    </div>

                  </div>
                </div>
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</div>";
    }
}
