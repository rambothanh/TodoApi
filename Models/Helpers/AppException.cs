using System;
using System.Globalization;

namespace TodoApi.Models.Helpers
{
    //AppException là một lớp ngoại lệ tùy chỉnh được sử dụng để 
    //phân biệt giữa các ngoại lệ được xử lý và không được xử lý.
    //Các ngoại lệ được xử lý là những ngoại lệ do ứng dụng tạo ra
    //và được sử dụng để hiển thị các thông báo lỗi thân thiện với
    //khách hàng, ví dụ: ngoại lệ logic nghiệp vụ hoặc xác thực do
    //người dùng nhập sai. Các ngoại lệ chưa được xử lý được tạo
    //bởi .NET framework và do lỗi trong mã ứng dụng gây ra.
    public class AppException : Exception
    {
        public AppException() : base() {}

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}