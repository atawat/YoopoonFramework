using System.Net.Mail;

namespace YooPoon.Common.Email
{
    public class SendEmail
    {
        /// <summary>        
        /// 发送邮件功能         
        /// </summary> 
        /// <param name="receviers"></param> 接收者地址(可以为多个，之间用"；"号隔开)         
        /// <param name="sender"></param> 发送者地址         
        /// 
        /// <param name="sname"></param>  发送者姓名        
        /// <param name="subject"></param> 邮件标题
        /// 
        /// <param name="content"></param> 邮件内容 
        /// <param name="username"></param> 发送者用户名 
        /// <param name="pass"></param> 发送者密码
        /// <param name="host"></param> SMTP主机地址 
        /// <param name="port"></param> 端
        /// <returns></returns> 
        public static bool SendMailUseSmtp(string receviers, string sender, string sname, string subject, string content, string username, string pass, string host, int port,bool useSSL)
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            try
            {
                // 拆分接收者地址字符串 
                string[] recevier = receviers.Trim().Split(';');
                for (int i = 0; i < recevier.Length; i++)
                {
                    msg.To.Add(recevier[i]);
                }
            }
            catch
            {
                return false;
            }

            /*  
            * msg.CC.Add("c@c.com");可以抄送给多人 
            * // 这里的处理与发送多个人一样*/

            /*3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.From = new MailAddress(sender, sname, System.Text.Encoding.UTF8);
            msg.Subject = subject; //邮件标题  
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            //邮件标题编码
            //msg.Attachments.Add(new Attachment(@"D:\a.txt")); 可以发送附件
            msg.Body = content; //邮件内容  
            msg.BodyEncoding = System.Text.Encoding.UTF8; //邮件内容编码              
            msg.IsBodyHtml = true; //是否是HTML邮件
            msg.Priority = MailPriority.High; //邮件优先级     
            
            SmtpClient client = new SmtpClient();
            //username-邮箱用户名  pass-密码 
            client.Credentials = new System.Net.NetworkCredential(username, pass);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = host; //邮箱服务器地址 
            client.Port = port; //邮箱服务器使用的端口
            client.EnableSsl = useSSL;//经过ssl加密  
            //client.UseDefaultCredentials = true;

            try
            {
                //                client.SendAsync(msg, userState);//简单一点儿可以 
                //                ViewBag.Message = "成功！";
                client.Send(msg);
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //MessageBox.Show(ex.Message, "发送邮件出错");
                //Console.WriteLine("发送邮件出错:" + ex.Message);   
                //                ViewBag.Message = "帐错误！";
                return false;

            }
        }
    }
}
