using System.Drawing;
namespace API.Helpers.Enums
{
    public class EmailContentContants
    {
        private static string factory = SettingsConfigUtility.GetCurrentSettings("AppSettings:Factory");
        private static string area = SettingsConfigUtility.GetCurrentSettings("AppSettings:Area");
        // private static string note = factory == "CB"
        //     ? "Vui lòng duyệt đơn trước ngày xx/xx/xxxx (dd/mm/yyyy).<br/>"
        //     : @"Vui lòng duyệt đơn trong vòng 3 ngày kể từ ngày người lao động nghỉ phép.<br/>
        //        xxxxxxxxxx";
        private static readonly string phoneNumber = factory == "SHC" ? "6115 & 6121" 
                                          : factory == "CB" ? "3137" 
                                          : factory == "GQT1" ? "02933.953.249 (Nhân Sự)" 
                                          : "02933.900.099";
        public static string title = $"Thông báo từ Leave.App (請假系統通知) - {factory}";
        public static string displayname = "Leave.App";
        public static string WaitingApprovalContent = @$"<div style='width: 800px; font-family: Arial; font-size:14px;'>
                                        <p><b>Xin chào:</b></p>
                                        <div style='line-height:30px; padding-left:50px;'>
                                        <p>Có rất nhiều đơn xin nghỉ phép đang chờ bạn xét duyệt.<br/>
                                        Vui lòng duyệt đơn trước ngày xx/xx/xxxx (mm/dd/yyyy).<br/>
                                        xxxxxxxxxx
                                        Mọi trường hợp duyệt đơn trễ đều không được chấp nhận bởi bộ phận nhân sự.<br/>
                                        {GetBaseUrl()}#/leave/approval
                                        </p></div>
                                        <div>
                                        您好 !<br/>
                                        您部門的假單在等您核准。<br/>
                                        請您在 xx/xx/xxxx (mm/dd/yyyy) 未核准該假單會作廢.<br/>
                                        未核准該假單會作廢。<br/>
                                        以上，謝謝! <br/>

                                        </div>
                                        <p><i>Sincerely, have a nice day !</i></p>
                                        <p style='font-size:12px; color:#ff0000;'>**********************************************************************************************</p>
                                        <div style='font-size:12px; color:#085060; line-height:24px;'>
                                        <table style='font-size:12px; color:#085060; line-height:24px;'><tr><td style='padding-right:22px;'>
                                        <img alt='logo' src='data:image/jpeg;base64,{ImageToBase64()}'/>
                                        </td>
                                        <td style='border-left:2px solid #b49327; padding-left:22px;'>
                                        Email tự động được gởi bởi hệ thống. Không thể nhận thư. Vui lòng không hồi đáp lại. <br/>
                                        Mọi thắc mắc xin vui lòng liên hệ các bộ phận liên quan. <br/>
                                        <b> Nghỉ phép:</b> {phoneNumber}
                                        </td></tr></table>
                                        </div>";
        public static string ApprovedBySuperiorContent = @$"<div style='width: 800px; font-family: Arial; font-size:14px;'>
                                        <p><b>Xin chào:</b></p>
                                        <div style='line-height:30px; padding-left:50px;'>
                                        <p>Đơn xin phép số <b>xxxxxxxx</b>  của bạn đã được cấp trên phê duyệt.<br/>
                                        Vui lòng truy cập lại hệ thống để xem kết quả.
                                        <br/>
                                        {GetBaseUrl()}#/leave/detail/xxxxxxxx
                                        </p></div>

                                        <p><i>Sincerely, have a nice day !</i></p>
                                        <p style='font-size:12px; color:#ff0000;'>**********************************************************************************************</p>
                                        <div style='font-size:12px; color:#085060; line-height:24px;'>
                                        <table style='font-size:12px; color:#085060; line-height:24px;'><tr><td style='padding-right:22px;'>
                                        <img alt='logo' src='data:image/jpeg;base64,{ImageToBase64()}'/>
                                        </td>
                                        <td style='border-left:2px solid #b49327; padding-left:22px;'>
                                        Email tự động được gởi bởi hệ thống. Không thể nhận thư. Vui lòng không hồi đáp lại. <br/>
                                        Mọi thắc mắc xin vui lòng liên hệ các bộ phận liên quan. <br/>
                                        <b> Nghỉ phép:</b> {phoneNumber}
                                        </td></tr></table>
                                        </div>";
        private static string GetBaseUrl()
        {
            return SettingsConfigUtility.GetCurrentSettings($"ServerSettings:{factory}_{area}_Url");
        }

        private static string ImageToBase64()
        {
            var result = GetByteUrl();
            // Set width cho ảnh
            byte[] resizedBytes = ResizeImage(result, 88);
            return Convert.ToBase64String(resizedBytes);
        }

        private static byte[] GetByteUrl()
        {
            string imageUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "syf1.png");
            byte[] result = File.ReadAllBytes(imageUrl);
            return result;
        }

        private static byte[] ResizeImage(byte[] imageBytes, int targetWidth)
        {
            using MemoryStream ms = new(imageBytes);
            using Image image = Image.FromStream(ms);
            // Tính toán tỉ lệ
            int targetHeight = (int)((float)targetWidth / image.Width * image.Height);
            // Tạo ảnh mới với kích thước mong muốn
            using Bitmap resizedImage = new(image, new Size(targetWidth, targetHeight));
            using MemoryStream msResized = new();
            // Lưu ảnh mới vào MemoryStream
            resizedImage.Save(msResized, System.Drawing.Imaging.ImageFormat.Png);
            return msResized.ToArray();
        }
    }
}